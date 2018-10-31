using Newtonsoft.Json;
using PiOne.Api.Business.DTO;
using PiOne.Api.Common;
using PiOne.Api.DataModel.Context;
using PiOne.Api.DataModel.PiOneEntities;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace PiOne.Api.Business.Support
{
    public class CommonFunction
    {
        public static bool CheckValidateAccount(string text)
        {
            return Regex.Match(text, "^[a-zA-Z0-9_.]*$").Success;
        }

        public static async void CheckAccountNotiSetting(string poinsID)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    var listNotiSetting = _db.NotificationSettings.Where(o => o.MemberID == poinsID).ToList();
                    if (listNotiSetting.Count > 0)
                    {
                        var listMerchant = listNotiSetting.GroupBy(o => o.MerchantID).Select(o => new { MerID = o.Key, Count = o.Count() }).ToList();
                        var listMerID = listMerchant.Where(o => o.Count > 1).Select(o => o.MerID).ToList();
                        foreach (var item in listMerID)
                        {
                            var listNoti = listNotiSetting.Where(o => o.MerchantID == item).ToList();
                            for (int i = 0; i < listNoti.Count; i++)
                            {
                                if (i != 0)
                                {
                                    listNoti[i].Status = (byte)Constants.EStatus.Deleted;
                                    listNoti[i].ModifiedBy = poinsID;
                                    listNoti[i].ModifiedDate = DateTime.UtcNow;
                                }
                            }
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("CheckAccountNotiSetting", ex);
            }
        }

        public static string GetHideString(string text)
        {
            if (text.Length > 8)
                text = text.Substring(text.Length - 8, 8);
            var arr = text.ToArray();
            int fullLength = text.Length;
            int length = 0;
            if (fullLength / 2 >= 4) length = fullLength - 4;
            else length = fullLength - (int)(fullLength / 2);
            for (int i = 0; i < length; i++) arr[i] = '*';
            return new string(arr);
        }

        public static string GetSHA512(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] message = UE.GetBytes(text);

            SHA512 hashString = new SHA512Managed();
            string hex = string.Empty;

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += x.ToString("x2");
            }
            return hex;
        }

        public static string GeneratePasswordCode(List<string> listCode, int length = -1)
        {
            Random random = new Random();
            if (length == -1)
            {
                bool isSuccess = int.TryParse(ConfigurationManager.AppSettings["PasswordCodeLength"], out length);
                if (!isSuccess)
                    length = 6;
            }

            string passCode = "";
            int count = 0;
            do
            {
                count += 1;
                if (count != 1)
                    Thread.Sleep(20);
                passCode = new string(Enumerable.Repeat(Constants.PasswordChar, length).Select(s => s[random.Next(s.Length)]).ToArray());
            } while (listCode.Contains(passCode));
            return passCode;
        }

        public static string GeneratePasswordCode(int length)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(Constants.PasswordChar, length).Select(s => s[random.Next(s.Length)]).ToArray()); ;
        }

        public static bool UploadImage(string imageString, ref string outPath)
        {
            try
            {
                byte[] data = Convert.FromBase64String(imageString);
                string fileExextension = string.Empty;
                string ftpLink = ConfigurationManager.AppSettings["FTPWebImage"];
                string ftpUser = ConfigurationManager.AppSettings["FTPUser"];
                string ftpPassword = ConfigurationManager.AppSettings["FTPPassword"];

                Image image;
                //if (string.IsNullOrEmpty(filePath))
                var filePath = Guid.NewGuid().ToString();

                string imagePath = filePath.Split('.')[0];
                //check image  Exextension
                using (var stream = new MemoryStream(data, 0, data.Length))
                {
                    image = Image.FromStream(stream);
                    if (System.Drawing.Imaging.ImageFormat.Jpeg.Equals(image.RawFormat))
                    {
                        fileExextension = System.Drawing.Imaging.ImageFormat.Jpeg.ToString().ToLower();
                    }
                    else if (System.Drawing.Imaging.ImageFormat.Png.Equals(image.RawFormat))
                    {
                        fileExextension = System.Drawing.Imaging.ImageFormat.Png.ToString().ToLower();
                    }
                    else if (System.Drawing.Imaging.ImageFormat.Gif.Equals(image.RawFormat))
                    {
                        fileExextension = System.Drawing.Imaging.ImageFormat.Gif.ToString().ToLower();
                    }
                }
                // Get the object used to communicate with the server.

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpLink + "/" + imagePath + "." + fileExextension.ToLower());

                request.Method = WebRequestMethods.Ftp.UploadFile;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                byte[] fileContents = Convert.FromBase64String(imageString);
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                filePath = imagePath + "." + fileExextension.ToLower();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();

                outPath = filePath;
                NSLog.Logger.Info("UploadImage", outPath);
                return true;
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("UploadImage", ex);
                return false;
            }
        }

        private static string CreateStringLengthDigit(int number, int length)
        {
            string result = number.ToString();
            while (result.Length < length)
            {
                result = "0" + result;
            }
            return result;
        }

        public static void SendContentMail(string EmailTo, string Content, string Name, string Subject, string attachment = "")
        {
            try
            {
                PiOneDb db = new PiOneDb();
                var setting = db.GeneralSettings.Where(o => o.Code == (byte)Constants.ESettingCode.Email || o.Code == (byte)Constants.ESettingCode.Password).ToList();
                string email = setting.Where(o => o.Code == (byte)Constants.ESettingCode.Email).FirstOrDefault().Value;
                string passWord = setting.Where(o => o.Code == (byte)Constants.ESettingCode.Password).FirstOrDefault().Value;
                string smtpServer = ConfigurationManager.AppSettings["SMTP"];
                if (email != "" && passWord != "")
                {
                    MailMessage mail = new MailMessage(email, EmailTo);
                    mail.Subject = Subject;
                    mail.Body = Content;
                    mail.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(email, passWord);
                    client.Host = smtpServer;
                    client.Timeout = 10000;
                    client.EnableSsl = true;
                    if (!string.IsNullOrEmpty(attachment))
                    {
                        mail.Attachments.Add(new System.Net.Mail.Attachment(attachment));
                    }

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Send Mail Error", ex);
            }
        }

        public static List<string> GetStoreOfMerchant(PiOneDb _db, string merchantID)
        {
            var lstStore = new List<string>();
            lstStore = _db.Stores.Where(o => o.MerchantID == merchantID && o.Status != (byte)Constants.EStatus.Deleted).Select(o => o.ID).ToList();
            return lstStore;
        }

        public static string FormatNumberCurrency(double val, bool isCurrency)
        {
            var ret = "";
            try
            {
                if (isCurrency == true) /* currency */
                    ret = val.ToString("0.00");
                else /* value */
                    ret = val.ToString();
            }
            catch (Exception ex) { }
            return ret;
        }

        public static void DeleteLogs(string mapPath)
        {
            try
            {
                bool isDelete = bool.Parse(ConfigurationManager.AppSettings["DeleteLog"] ?? "False");
                if (isDelete && !string.IsNullOrEmpty(mapPath))
                {
                    string pathConfig = Path.Combine(mapPath, "Web.config");
                    int holdDay = int.Parse(ConfigurationManager.AppSettings["HoldDay"] ?? "0");
                    DateTime holdDate = DateTime.Now.AddDays(-holdDay).Date;

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(pathConfig);
                    string pathLog = xdoc.SelectSingleNode("//log4net/appender/file").Attributes["value"].Value;

                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(mapPath, pathLog));
                    var files = dir.GetFiles().Where(o => o.CreationTime < holdDate).ToArray();
                    foreach (var file in files) file.Delete();
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorDeleteLogs", ex); }
        }

        public static void CheckEnum(string mapPath)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    string path = mapPath + "InitializationDb.config";
                    if (File.Exists(path))
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(path);

                        //Enum Setting
                        List<GeneralSetting> listSettingInsert = new List<GeneralSetting>();
                        var listSetting = _db.GeneralSettings.ToList();
                        var settingNodes = xdoc.SelectNodes("//setting");
                        if (listSetting.Count != settingNodes.Count)
                        {
                            foreach (var e in Enum.GetValues(typeof(Constants.ESettingCode)))
                            {
                                var setting = listSetting.Where(o => o.Code == (byte)e.GetHashCode()).FirstOrDefault();
                                if (setting == null)
                                {
                                    var node = settingNodes.Cast<XmlNode>().Where(o => o.Attributes["code"].Value == e.GetHashCode().ToString()).FirstOrDefault();
                                    if (node != null)
                                    {
                                        setting = new GeneralSetting()
                                        {
                                            ID = Guid.NewGuid().ToString(),
                                            Name = node.Attributes["name"].Value,
                                            DisplayName = node.Attributes["display"].Value,
                                            Value = node.Attributes["value"].Value,
                                            ObjectType = node.Attributes["objecttype"].Value,
                                            Code = byte.Parse(node.Attributes["code"].Value),
                                            Status = byte.Parse(node.Attributes["status"].Value),
                                            CreatedDate = DateTime.Now,
                                            CreatedUser = "Admin",
                                            ModifiedUser = "Admin",
                                            LastModified = DateTime.Now,
                                        };
                                        listSettingInsert.Add(setting);
                                    }
                                }
                            }

                            _db.GeneralSettings.AddRange(listSettingInsert);
                        }
                    }

                    _db.SaveChanges();
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorCheckEnum", ex); }
        }

        public static ResponseGetTier GetTierConfig(PiOneDb _db)
        {
            var data = new ResponseGetTier();
            try
            {
                var query = _db.GeneralSettings.Where(o => o.Code == (byte)Constants.ESettingCode.Tier).Select(o => o.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                {
                    data = JsonConvert.DeserializeObject<ResponseGetTier>(query);
                    foreach (var item in data.ListTier)
                    {
                        if (!string.IsNullOrEmpty(item.Img))
                            item.Img = ConfigurationManager.AppSettings["PublicImages"] + item.Img;
                    }

                }
            }
            catch (Exception ex) { NSLog.Logger.Error("Get tier config", ex); }
            return data;
        }

        public static String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }

        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }

        #region socket

        public class SocketParameter
        {
            public QueueForSocketIO.SocketEnumAction Action { get; set; }
            public string PoinsID { get; set; }
            public string Data { get; set; }
            public string DeviceName { get; set; }

            public SocketParameter()
            {
                Data = "";
                DeviceName = "";
                PoinsID = "";
            }

            public string ToSocketString()
            {
                string content = "";
                //content = string.Format(@"""Action"":""{0}"",""Data"":""{1}"",""DeviceName"":""{2}"",""PoinsID"":""{3}""",
                //    Action, Data, DeviceName, PoinsID);
                //content = "{" + content + "}";
                content = string.Format("\"Action\":\"{0}\",\"Data\":\"{1}\",\"DeviceName\":\"{2}\",\"PoinsID\":\"{3}\"", Action, Data, DeviceName, PoinsID);
                content = "{" + content + "}";
                return content;
            }
        }

        public class QueueForSocketIO
        {
            public enum SocketEnumAction
            {
                Wallet_BecomeMembership,
                Wallet_MembershipInfo,
                Wallet_Notification,
                Chat_SendMessage,
            }

            private static ConcurrentQueue<SocketParameter> m_SocketIOQueue = new ConcurrentQueue<SocketParameter>();
            private static ConcurrentQueue<SocketParameter> m_SocketIOQueueSecondDisplay = new ConcurrentQueue<SocketParameter>();
            private static ConcurrentQueue<SocketParameter> m_SocketIOQueueOrderPending = new ConcurrentQueue<SocketParameter>();
            private static ConcurrentQueue<SocketParameter> m_SocketIOQueueOrderServing = new ConcurrentQueue<SocketParameter>();
            private static QueueForSocketIO instance = null;

            private static int m_SocketServerQueueDelay;
            private static string m_SocketServerURL;
            private static Socket m_Socket;

            public static QueueForSocketIO Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new QueueForSocketIO();
                        m_SocketServerQueueDelay = int.Parse(ConfigurationManager.AppSettings["SocketQueueDelay"].ToString());
                        m_SocketServerURL = ConfigurationManager.AppSettings["SocketServerURL"].ToString();
                        m_Socket = IO.Socket(m_SocketServerURL);
                    }
                    return instance;
                }
            }

            private QueueForSocketIO()
            {
                NSLog.Logger.Info("QueueForSocketIO inits");
                System.Threading.Thread queueThread = new System.Threading.Thread(RunQueueSocketIO);
                queueThread.Start();

                NSLog.Logger.Info("QueueForSocketIO ends");
            }

            private void RunQueueSocketIO()
            {
                NSLog.Logger.Info("RunQueueSocketIO ....");
                List<SocketParameter> listParams = new List<SocketParameter>();
                while (true)
                {
                    try
                    {
                        while (m_SocketIOQueue.Count > 0)
                        {
                            SocketParameter param = null;
                            if (m_SocketIOQueue.TryDequeue(out param))
                            {
                                if (!CheckContains(listParams, param))
                                {
                                    listParams.Add(param);
                                }
                            }
                        }

                        if (listParams.Count > 0)
                        {
                            SendMessageToSocketServer(listParams);
                        }

                        System.Threading.Thread.Sleep(m_SocketServerQueueDelay);
                        listParams = new List<SocketParameter>();
                    }
                    catch (Exception ex)
                    {
                        NSLog.Logger.Error("RunQueueSocketIO", ex);
                    }
                }
                NSLog.Logger.Info("RunQueueSocketIO .... end");
            }

            public void AddEvent(SocketParameter param)
            {
                if (param != null)
                {
                    param.Data = param.Data.Replace("\"", "\\\"");
                    NSLog.Logger.Info("AddEvent .... " + param.Action.ToString());
                    switch (param.Action)
                    {
                        default:
                            m_SocketIOQueue.Enqueue(param);
                            break;
                    }
                }
            }

            public void SendMessageToSocketServer(List<SocketParameter> parameters)
            {
                try
                {
                    if (m_Socket.Io().ReadyState == Manager.ReadyStateEnum.OPEN)
                    {
                        foreach (SocketParameter message in parameters)
                        {
                            m_Socket.Emit("api_event", message.ToSocketString()); //the server.js have to have this event.
                            System.Threading.Thread.Sleep(50);
                        }
                    }
                }
                catch (Exception e)
                {
                    NSLog.Logger.Error("Socket has unexpected exception", e);
                }
            }

            private static bool CheckContains(List<SocketParameter> list, SocketParameter input)
            {
                bool result = false;
                if (list != null && input != null)
                {
                    foreach (SocketParameter item in list)
                    {
                        if (item.Action == input.Action && item.Data == input.Data)
                        {
                            result = true;
                            break;
                        }
                    }
                }

                return result;
            }
        }

        public static void RefreshBecomeMembership(string PoinsID, string info, string DeviceName = "")
        {
            QueueForSocketIO.Instance.AddEvent(new SocketParameter()
            {
                Action = QueueForSocketIO.SocketEnumAction.Wallet_BecomeMembership,
                PoinsID = PoinsID,
                Data = info,
                DeviceName = DeviceName
            });
        }

        public static void RefreshMembershipInfo(string PoinsID, string info, string DeviceName = "")
        {
            QueueForSocketIO.Instance.AddEvent(new SocketParameter()
            {
                Action = QueueForSocketIO.SocketEnumAction.Wallet_MembershipInfo,
                PoinsID = PoinsID,
                Data = info,
                DeviceName = DeviceName
            });
        }

        public static void RefreshNotification(string PoinsID, string info, string DeviceName = "")
        {
            QueueForSocketIO.Instance.AddEvent(new SocketParameter()
            {
                Action = QueueForSocketIO.SocketEnumAction.Wallet_Notification,
                PoinsID = PoinsID,
                Data = info,
                DeviceName = DeviceName
            });
        }

        public static void ChatSendMessage(MessageDTO data, string DeviceName = "")
        {
            QueueForSocketIO.Instance.AddEvent(new SocketParameter()
            {
                Action = QueueForSocketIO.SocketEnumAction.Chat_SendMessage,
                PoinsID = "",
                Data = JsonConvert.SerializeObject(data),
                DeviceName = DeviceName
            });
        }

        #endregion socket
    }
}
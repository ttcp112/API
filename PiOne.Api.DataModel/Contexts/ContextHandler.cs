using PiOne.Api.DataModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.DataModel.Contexts
{
    public class ContextHandler : CreateDatabaseIfNotExists<PiOneDb>
    {

        protected override void Seed(PiOneDb context)
        {
            base.Seed(context);
            CreateDefaultData(context);
        }

        public override void InitializeDatabase(PiOneDb context)
        {
            try
            {
                if (context.Database.Exists()) return;
                base.InitializeDatabase(context);
            }
            catch (SqlException ex) { throw new ArgumentException(ex.Message, ex); }
        }

        private void CreateDefaultData(PiOneDb context)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static bool FindError(SqlErrorCollection errors, string error)
        {
            return errors.Cast<SqlError>().ToList().Exists(x => x.ToString().ToLower().Contains(error));
        }
    }
}

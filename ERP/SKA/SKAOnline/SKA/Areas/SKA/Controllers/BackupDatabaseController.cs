using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using System.Data.SqlClient;

namespace SKA.Areas.SKA.Controllers
{
    public class BackupDatabaseController : BaseController
    {
        //
        // GET: /SKA/BackupDatabase/

        public ActionResult Index()
        {
            Backup();
            return View();
        }

        public void Backup()
        {

            string conString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            string databaseName = System.Configuration.ConfigurationManager.AppSettings["Database"].ToString();

            string urlDatabase = System.Configuration.ConfigurationManager.AppSettings["DatabaseBackupURL"].ToString();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                
                string sqlStmt = String.Format("BACKUP DATABASE " + databaseName + " TO DISK='" + urlDatabase  + "{0}'", 
                    DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second +  ".bak");

                using (SqlCommand bu2 = new SqlCommand(sqlStmt, conn))
                {
                    conn.Open();
                    bu2.ExecuteNonQuery();
                    conn.Close();

                    ViewData["message"] = "Database was successfull backup.";
                }
            }
        }
    }
}

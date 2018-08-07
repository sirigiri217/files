using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace PartAnalysis
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            string action = context.Request.QueryString["action"].ToString();
            string pathFromDownloadFile = ConfigurationManager.AppSettings["fileExportPath"].ToString();
            string userfileName = getUserName();
            if (action == "Summarydownload")
            {
                FileInfo file = new FileInfo(pathFromDownloadFile+ "\\Receipts_Summary_"+userfileName+".xlsx");
                string attachment = "attachment; filename=" + file.Name;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.AppendHeader("content-disposition", attachment);
                context.Response.AppendHeader("Content-Length", file.Length.ToString());
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                context.Response.WriteFile(file.FullName);
                context.Response.Flush();
                context.Response.Close();
                context.Response.End();
            }
            if (action == "Detaileddownload")
            {
                FileInfo file = new FileInfo(pathFromDownloadFile + "\\Receipts_Detailed_" + userfileName + ".xlsx");
                string attachment = "attachment; filename=" + file.Name;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.AppendHeader("content-disposition", attachment);
                context.Response.AppendHeader("Content-Length", file.Length.ToString());
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                context.Response.WriteFile(file.FullName);
                context.Response.Flush();
                context.Response.Close();
                context.Response.End();
            }
        }
        public static string getUserName()
        {

            string windowsLogin = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.ToString(); //HttpContext.Current.User.Identity.Name.ToString();
                                                                                                            //windowsLogin = "DSTILLSON80191";
            int hasDomain = windowsLogin.IndexOf(@"\"); // Splitting the Domain name from the NT ID
            if (hasDomain > 0)
            {
                windowsLogin = windowsLogin.Remove(0, hasDomain + 1);
            }

            return windowsLogin;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
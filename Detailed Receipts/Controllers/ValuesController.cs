using Detailed_Receipts.Models;
using Newtonsoft.Json;
using PartAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Detailed_Receipts.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
       

        // GET api/values/5
        public string Get(int id)
        {
            ReceiptsInputModel model = new ReceiptsInputModel();
            var str = "";

            var query = "";


            if (id == 1)
            {
                str = HttpContext.Current.Request.LogonUserIdentity.Name.ToString();


                query = "dbo.usp_gscobi_app_assitant  '" + str + "','login'";
            }

            if (id == 2)
            {
                str = HttpContext.Current.Request.LogonUserIdentity.Name.ToString();
                str = str.Replace("AMAT\\", "");
                query = "insert into  [GSCA_APPS].[dbo].[gscobi_apps_users_access_log] values ('" + str + "','DetailedReceipts',GETDATE())";
            }
            if (id == 3)
            {
                str = HttpContext.Current.Request.LogonUserIdentity.Name.ToString();
                str = str.Replace("AMAT\\", "");
                query = "select EMPLOYEE_NTID, Favouriteslist from [dbo].[gscobi_apps_users_favourites_list] where [EMPLOYEE_NTID]='" + str + "' and [APPLICATION_NAME] = 'DetailedReceipts'";
            }
            if (id == 4)
            {

                query = "select max(receipt_date) refreshdate  from gsca_apps.helper.detailed_receipts_app_receipt";
            }



            return model.getDataFromDB(query, " "); ;
        }

        public ReceiptsInputModel Get()
        {


           
            ReceiptsInputModel input = new ReceiptsInputModel();
            DataTable dt = new DataTable();
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(dt);
            input.plantcd = input.getDataFromDB("exec helper.usp_Detailed_Receipts_inputs 'plantcd'", " ");
            input.suppliersitename = input.getDataFromDB("exec helper.usp_Detailed_Receipts_inputs 'suppliersitename'", " ");
            input.ParentSupplierName = input.getDataFromDB("exec helper.usp_Detailed_Receipts_inputs 'ParentSupplierName'", " ");

            return input;

        }


        public ReceiptsOutputModel Post(ReceiptsInputModel model)
        {
            ReceiptsOutputModel result = new ReceiptsOutputModel();
            ReceiptsInputModel action = new ReceiptsInputModel();
            DataTable dt = new DataTable();
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(dt);

            if ( model.dataflag == "Summary"){

                result.Summary = action.getDataFromDB("EXEC [helper].[usp_Detailed_Receipts_report_data]   '" + model.dataflag + "','" + model.materialnum +  "','" + model.plantcd + "','" + model.fromdate + "','" + model.todate +   "','" + model.suppliersitename + "','" + model.ParentSupplierName + "'", " ");
               

            }
            else if (model.dataflag == "Detailed")
            {
                result.Detailed = action.getDataFromDB("EXEC [helper].[usp_Detailed_Receipts_report_data]   '" + model.dataflag + "','" + model.materialnum + "','" + model.plantcd + "','" + model.fromdate + "','" + model.todate + "','" + model.suppliersitename + "','" + model.ParentSupplierName + "'", " ");

            }

            result.exportFilename = writeExcelFile(result,model.dataflag);
            return result;


        }
        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //}
        private string writeExcelFile(ReceiptsOutputModel result ,string dataflag)
        {
            DataSet ds = new DataSet();
            exportToEPPExcel obj = new exportToEPPExcel();

            string filename = string.Empty;
            if( dataflag == "Summary") {
                if (result != null && result.Summary.Length > 0)
                {
                    DataTable dtSummary = (DataTable)JsonConvert.DeserializeObject(result.Summary, (typeof(DataTable)));
                    dtSummary.TableName = "dtSummary";
                    ds.Tables.Add(dtSummary);
                    filename = obj.ExportToEPPExcel(ds, "Receipts_Summary_" + getUserName());
                }
            }
            if (dataflag == "Detailed")
            {
                if (result != null && result.Detailed.Length > 0)
                {
                    DataTable dtDetailed = (DataTable)JsonConvert.DeserializeObject(result.Detailed, (typeof(DataTable)));
                    dtDetailed.TableName = "dtDetailed";
                    ds.Tables.Add(dtDetailed);
                    filename = obj.ExportToEPPExcel(ds, "Receipts_Detailed_" + getUserName());
                }

            }
            

            return filename;
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
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

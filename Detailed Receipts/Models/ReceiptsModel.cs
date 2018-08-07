using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Detailed_Receipts.Models
{
    public class ReceiptsInputModel
    {
        public string dataflag { get; set; }
        public string materialnum { get; set; }
        public string plantcd { get; set; }
        public string suppliersitename { get; set; }
        public string ParentSupplierName { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

        public string getDataFromDB(string query, string where)
        {
            string strcon = ConfigurationManager.ConnectionStrings["gsca_appsEntities"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query + where, con);
            cmd.CommandTimeout = 300;
            SqlDataAdapter customerDA = new SqlDataAdapter();
            customerDA.SelectCommand = cmd;
            con.Open();
            DataTable dt = new DataTable();

            customerDA.Fill(dt);
            con.Close();
            var rowcounts = dt.Rows.Count;
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(dt);
            return JsonString;
        }
    }

    public class ReceiptsOutputModel
    {
        public string Summary { get; set; }
        public string Detailed { get; set; }
        
        public string exportFilename { get; set; }
    }
}
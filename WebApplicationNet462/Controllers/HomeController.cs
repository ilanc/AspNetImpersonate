using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Data.SqlClient;

namespace WebApplicationNet462.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            /* .NET Core
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () => {
                ViewData["Name"] = ($"{WindowsIdentity.GetCurrent().Name}!");
                ViewData["List"] = sql_vData_BloombergRequest("Data Source=IAMGBLSQLUAT2;Initial Catalog=InfoPortal;Integrated Security=SSPI;");
            });
            /* */

            /* .NET 4.6 */
            var callerIdentity = User.Identity as WindowsIdentity;
            using (callerIdentity.Impersonate())
            {
                ViewData["Name"] = ($"{WindowsIdentity.GetCurrent().Name}!");
                ViewData["List"] = sql_vData_BloombergRequest("Data Source=IAMGBLSQLUAT2;Initial Catalog=InfoPortal;Integrated Security=SSPI;");
            }

            /* */
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public class BBGDataLicenseJson
        {
            public string Ticker { get; set; }
            public string Mnemonic { get; set; }
            public BBGDataLicenseJson(SqlDataReader reader)
            {
                Ticker = Convert.IsDBNull(reader[0]) ? null : reader.GetString(0);
                Mnemonic = Convert.IsDBNull(reader[1]) ? null : reader.GetString(1);
            }
        }

        public static List<BBGDataLicenseJson> sql_vData_BloombergRequest(string connectionString)
        {
            var sql =
@"select external_value as Ticker, internal_value as Mnemonic 
from infoportal_staging.dbo.tbl_CrossReference
where originator_id = 'BloombergRequest' and internal_value2 is null";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    var results = new List<BBGDataLicenseJson>(100);
                    while (reader.Read())
                    {
                        results.Add(new BBGDataLicenseJson(reader));
                    }
                    return results;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApplicationNet462.Controllers
{
    public class HomeController : Controller
    {
        protected string _Conn;

        public HomeController(IConfigurationRoot configuration)
        {
            _Conn = configuration.GetConnectionString("Conn");
        }

        public IActionResult Index()
        {
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
                ViewData["Name"] = ($"{WindowsIdentity.GetCurrent().Name}");
                ViewData["List"] = sql_test(_Conn);
            }

            /* */
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public class SqlTestResult
        {
            public string Function { get; set; }
            public string Username { get; set; }
            public SqlTestResult(SqlDataReader reader)
            {
                Function = Convert.IsDBNull(reader[0]) ? null : reader.GetString(0);
                Username = Convert.IsDBNull(reader[1]) ? null : reader.GetString(1);
            }
        }

        public static List<SqlTestResult> sql_test(string connectionString)
        {
            var sql =
@"select 'SYSTEM_USER', SYSTEM_USER
union
select 'ORIGINAL_LOGIN()', ORIGINAL_LOGIN()
union
select 'SUSER_SNAME()', SUSER_SNAME()";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    var results = new List<SqlTestResult>(100);
                    while (reader.Read())
                    {
                        results.Add(new SqlTestResult(reader));
                    }
                    return results;
                }
            }
        }
    }
}

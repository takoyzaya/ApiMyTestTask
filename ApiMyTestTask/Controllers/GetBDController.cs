using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiMyTestTask.Models;
using System.Data.SqlClient;

namespace ApiMyTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetBDController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ElementBDModel> Get()
        {
            string connectionString = @"Server=tcp:spaceplanserver.database.windows.net,1433;Initial Catalog=TestTaskDB;Persist Security Info=False;User ID=LoginTestWorker;Password=owRajapp!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlExpression = "SELECT * FROM dim_goods";

            List<ElementBDModel> ListOfElements = new List<ElementBDModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        ListOfElements.Add(new ElementBDModel() { good_id = reader.GetValue(0), good_name = reader.GetValue(1), group_id = reader.GetValue(2), group_name = reader.GetValue(3) });
                    }

                    reader.Close();
                }
            }

            return Enumerable.Range(0, ListOfElements.Count).Select(index => new ElementBDModel
            {
                good_id = ListOfElements[index].good_id,
                good_name = ListOfElements[index].good_name,
                group_id = ListOfElements[index].group_id,
                group_name = ListOfElements[index].group_name
            }).ToList();
        }

    }
}

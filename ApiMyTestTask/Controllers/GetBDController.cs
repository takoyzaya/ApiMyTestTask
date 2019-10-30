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
            const string connectionString = @"Server=tcp:spaceplanserver.database.windows.net,1433;Initial Catalog=TestTaskDB;Persist Security Info=False;User ID=LoginTestWorker;Password=owRajapp!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            const string sqlExpression = "SELECT * FROM dim_goods"; // sql команда получения бд

            List<ElementBDModel> ListOfElements = new List<ElementBDModel>(); // список элементов бд

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // подключаемся к бд
                SqlCommand command = new SqlCommand(sqlExpression, connection); // запрос к бд
                SqlDataReader reader = command.ExecuteReader(); // считываем данные

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        ListOfElements.Add(new ElementBDModel() { good_id = reader.GetValue(0).ToString(), good_name = reader.GetValue(1).ToString(), group_id = reader.GetValue(2).ToString(), group_name = reader.GetValue(3).ToString() }); // переносим данные бд в список
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
            }).ToList(); // передаем данные
        }

    }
}

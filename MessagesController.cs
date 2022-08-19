using System.Data;
using System.Runtime.Intrinsics.Arm;
using AISMessages.models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AISMessages.Controllers;

[ApiController]
[Route("ais-messages")]
public class MessagesController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public MessagesController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [Route("last-daily-positions")]
    public JsonResult GetLastMessages(int imoNumber, string startDate)
    {
        string query = @"select * from aismessages where imoNumber=@imoNumber and timestamp in (select max(timestamp) from aismessages where timestamp>@startDate and imoNumber=@imoNumber group by TO_DATE(timestamp, 'YYYY-MM-DD'))";
        DataTable table = new DataTable();
        string sqlDatasource = _configuration.GetConnectionString("AISDatabase");
        NpgsqlDataReader myReader;
        using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDatasource))
        {
            myCon.Open();
            using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@imoNumber", imoNumber);
                myCommand.Parameters.AddWithValue("@startDate", startDate);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                myCon.Close();
            }
        }

        return new JsonResult(table);
    }

    [HttpPost]
    public JsonResult AddAisMessages(AISMessage message)
    {
        string query = @"insert into aismessages(imoNumber, timestamp, latitude, longitude) values (@imoNumber, @timestamp, @latitude, @longitude)";
        DataTable table = new DataTable();
        string sqlDatasource = _configuration.GetConnectionString("AISDatabase");
        NpgsqlDataReader myReader;
        using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDatasource))
        {
            myCon.Open();
            using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@imoNumber", message.imoNumber);
                myCommand.Parameters.AddWithValue("@timestamp", message.timestamp ?? DateTime.Now.ToUniversalTime().ToString("u").Replace(" ", "T"));
                myCommand.Parameters.AddWithValue("@latitude", message.latitude);
                myCommand.Parameters.AddWithValue("@longitude", message.longitude);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                myCon.Close();
            }
        }

        return new JsonResult(table);
    }
}

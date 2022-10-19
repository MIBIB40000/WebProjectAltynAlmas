using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;

namespace WebIP.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly FileLogger _fileLogger = new FileLogger();
		
		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public string SideNumber { get; private set; }
		public string Progress { get; private set; }
		public string LocalIp { get; private set; }
		public string Plan { get; private set; }
		public string Workplace { get; private set; }
		public string Unit { get; private set; }

		public void OnGet()
		{
			LocalIp = GetLocalIpAddress();
			SideNumber = GetDataFromSql(out var progress);
			
			Progress = progress;
			
			List<string> truckData = Requester.CreateRequest(SideNumber);
			
			if(truckData == null)
			{
				return;
			}
			
			Plan = truckData[1];
			Workplace = truckData[2];
			Unit = truckData[3];

			Console.WriteLine("poop");
		}

		public string GetLocalIpAddress()
		{		
			return Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
		}

		public string GetDataFromSql(out string progress)
		{
			string sideNumber = null;
			progress = null;

			try
			{
				// TODO: Подумать о переносе в appsettings.json
				
				const string CONNECTION_STRING = "Data Source=pst-wenco-2.altynalmas.kz;database=WENCO;uid=wenco_reports;Password=Wen7Rep";
				const string DATASET_NAME = "Altyn";
				
				var connection = new SqlConnection(CONNECTION_STRING);
				var dataset = new DataSet(DATASET_NAME);
				
				var query = GetQuery();

				Console.WriteLine("New request created");

				var adapter = new SqlDataAdapter(query, connection);
				adapter.Fill(dataset);

				// Берется только последняя задача из всего набора данных
				
				foreach (DataTable dt in dataset.Tables)
				{
					foreach (DataRow row in dt.Rows)
					{
						sideNumber = row.ItemArray[0].ToString();
						progress = row.ItemArray[1].ToString();
					}
				}

				adapter.Dispose();
				dataset.Dispose();
				connection.Close();
				connection.Dispose();
			}
			catch (Exception e)
			{
				_fileLogger.DisplayLog((e.Message + '\n' + e.StackTrace));
			}

			return sideNumber;
		}

		private string GetQuery()
		{
			return $"select * from (select hauling_unit_ident, sum(payload_reporting) current_tones,(select IP_ADDRESS from equip where equip_ident = hct.hauling_unit_ident) IP_ADDR from haul_cycle_trans hct where dump_end_shift_date = (select shift_date from dbo.current_shiftdate()) and dump_end_shift_ident = (select shift_ident from dbo.current_shiftdate()) group by hauling_unit_ident) afx where IP_ADDR = '{GetLocalIpAddress()}'";
		}
	}
}


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

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public string IPMessage { get; set; }
		public string SideNumber;
		public string Progress;
		public string Plan;
		public string Workplace;
		public string Unit;

		public bool BP { get; set; }
		
		
		//public List<XMLObject> List { get; set; }
		Requester req = new Requester();

		Logger logger1 = new Logger();

		public void OnGet()
		{
			IPMessage = GetLocalIPAddress();
			BP = TryConnectToBase(out SideNumber,out Progress);
			List<string> a = req.CreateRequest(SideNumber);
			if(a == null)
			{
				return;
			}
			Plan = a[1];
			Workplace = a[2];
			Unit = a[3];

			Console.WriteLine("poop");
		}

		public string GetLocalIPAddress()
		{		
			return Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
		}

		public bool TryConnectToBase(out string unitIdent, out string currentTones)//out string progress
		{

			string connstig = "Data Source=" + "pst-wenco-2.altynalmas.kz" + ";database=" + "WENCO" + ";uid=" + "wenco_reports" + ";Password=" + "Wen7Rep"; ;
			DataSet dbset;
			SqlConnection dbcon;
			SqlDataAdapter sda;
			string IP = GetLocalIPAddress();
			unitIdent = null;
			currentTones = null;

			try
			{
				string query;
				dbcon = new SqlConnection(connstig);
				dbset = new DataSet("Altyn");

				query = "select * from (select hauling_unit_ident, sum(payload_reporting) current_tones,(select IP_ADDRESS from equip where equip_ident = hct.hauling_unit_ident) IP_ADDR from haul_cycle_trans hct where dump_end_shift_date = (select shift_date from dbo.current_shiftdate()) and dump_end_shift_ident = (select shift_ident from dbo.current_shiftdate()) group by hauling_unit_ident) afx where IP_ADDR = '" + IP + "'";

				Console.WriteLine("Creating new request!");

				sda = new SqlDataAdapter(query, dbcon);
				sda.Fill(dbset);

				foreach (DataTable dt in dbset.Tables)
				{
					foreach (DataRow row in dt.Rows)
					{
						unitIdent = row.ItemArray[0].ToString(); //Если задач будет ненсколькоЮ все пойдет по пизде
						currentTones = row.ItemArray[1].ToString();
					}
				}

				sda.Dispose();
				dbset.Dispose();
				dbcon.Close();
				dbcon.Dispose();
				return true;
			}
			catch (Exception e)
			{
				logger1.DisplayLog((e.Message + '\n' + e.StackTrace));
				return false;
			}
		}
	}
}


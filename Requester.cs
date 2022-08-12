using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace WebIP
{
	public class Requester
	{
		public List<string> CreateRequest(string A)
		{
			string localTime = DateTime.Now.ToString("dd.MM.yyyy");
			String responseString;

			List<string> showMeData = new List<string>();
			string shift = DefineShift();

			try
			{
				WebRequest request = WebRequest.Create(
					"http://172.17.32.14:88/gate/getOrders/?shift=" + shift + "&date_in=" + localTime + "&district=3&tip=1");
				WebResponse response = request.GetResponse();

				using (Stream stream = response.GetResponseStream())
				{
					StreamReader reader = new StreamReader(stream);
					responseString = reader.ReadToEnd();
				}

				List<ApiData> apis = JsonConvert.DeserializeObject<List<ApiData>>(responseString);

				response.Close();
				GetTruckFromAPIData(apis);

				if (apis != null)
				{
					foreach (var n in apis)
					{
						if ("CAT"+n.SideNumber == A)
						{
							showMeData.Add("CAT" + n.SideNumber);
							showMeData.Add(n.Plan);
							showMeData.Add(n.Workplace);
							showMeData.Add(n.Unit);
							return showMeData;
						}
					}
				}
				else
				{
					return null;
				}
			}
			catch (WebException e)
			{
				Console.WriteLine(e);
				return null;
			}
			return null;
		}
	//}

		public List<ApiData> GetTruckFromAPIData(List<ApiData> x)
		{
			for (int i = x.Count - 1; i >= 0; i--)
			{

				if (x[i].Equip.Contains("Самосвал") is false)
				{
					x.RemoveAt(i);
				}
			}
			return x;
		}

		public string DefineShift()
		{
			TimeSpan start = new TimeSpan(19, 30, 0); //здесь задаются временные границы смены
			TimeSpan end = new TimeSpan(7, 30, 0);
			TimeSpan now = DateTime.Now.TimeOfDay;

			if ((now > start) && (now < end))
			{
				return "1";
			}
			else
			{
				return "2";
			}
		}		
	}
}

using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WebIP
{
	/// <summary>
	/// Класс для получения данных со стороны API
	/// </summary>
	public static class Requester
	{
		public static List<string> CreateRequest(string sideNumber)
		{
			try
			{
				var responseString = GetDataFromApi();

				var trucksDataset = JsonConvert.DeserializeObject<List<TruckData>>(responseString);
				FilterTruckData(trucksDataset, sideNumber);

				if (trucksDataset == null)
				{
					throw new WebException("Data not found");
				}

				foreach (var truckData in trucksDataset)
				{
					var receivedData = new List<string>
					{
						"CAT" + truckData.SideNumber,
						truckData.Plan,
						truckData.Workplace,
						truckData.Unit
					};

					return receivedData;
				}
			}
			catch (WebException e)
			{
				Console.WriteLine(e);
			}
			
			return null;
		}

		private static string GetDataFromApi()
		{
			var shift = GetShift();
			var currentTime = DateTime.Now.ToString("dd.MM.yyyy");
			var requestString = CreateRequestString(shift, currentTime);

			var request = WebRequest.Create(requestString);
			var response = request.GetResponse();

			string responseString;

			using (var stream = response.GetResponseStream())
			{
				var reader = new StreamReader(stream);
				responseString = reader.ReadToEnd();
			}

			response.Close();
			
			return responseString;
		}

		private static string GetShift()
		{
			var start = new TimeSpan(19, 30, 0); //здесь задаются временные границы смены
			var end = new TimeSpan(7, 30, 0);
			var now = DateTime.Now.TimeOfDay;

			if (now > start && now < end)
			{
				return "1";
			}

			return "2";
		}

		private static void FilterTruckData(List<TruckData> data, string sideNumber)
		{
			data.RemoveAll(truckData => !IsTruck(truckData) || IsRequiredNumber(truckData));

			bool IsTruck(TruckData truckData)
			{
				return truckData.Equip.Contains("Самосвал");
			}

			bool IsRequiredNumber(TruckData truckData)
			{
				return "CAT" + truckData.SideNumber != sideNumber;
			}
		}

		private static string CreateRequestString(string shift, string currentTime)
		{
			return $"http://172.17.32.14:88/gate/getOrders/?shift={shift}&date_in={currentTime}&district=3&tip=1";
		}
	}
}

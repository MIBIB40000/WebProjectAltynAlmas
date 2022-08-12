using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;  

namespace WebIP
{
	public class Logger
	{
		private String CurrentDirectory {get; set;}
		private String FileName	{get; set;}
		private String FilePath {get; set;}

		public Logger()
		{
			this.CurrentDirectory = Directory.GetCurrentDirectory()+ "\\Logs";
			this.FileName = DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
			this.FilePath = this.CurrentDirectory + "/" + this.FileName;

		}

		public void DisplayLog(string Messsage)
		{
			System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");

			using (System.IO.StreamWriter w = System.IO.File.AppendText(this.FilePath))
			{
				w.Write("\r\nLog Entry : ");
				w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
				w.WriteLine("  :{0}", Messsage);
				w.WriteLine("-----------------------------------------------");
			}

			CheckOldLogs();
		}

		public string FormatData() //Если в будущем не пригодится - удалить
		{
			string data = DateTime.Now.ToLongDateString();
			return data;
		}

		public void CheckOldLogs()
		{
			var listOfFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Logs");			
			DateTime date;

			for (int i = 0; i < listOfFiles.Length; i++)
			{
				var a = Path.GetFileNameWithoutExtension(listOfFiles[i]);
				date = DateTime.ParseExact(a, "dd-MM-yyyy", CultureInfo.InvariantCulture);

				int t = date.CompareTo(DateTime.Now.AddDays(-7));

				//сравнивает 2 даты на разницу в 7 дней, если минус 1, то удалит файл
				if (t == -1)
				{
					File.Delete(Path.GetFullPath(listOfFiles[i]));
					Console.WriteLine(t);
				}
			}
		}
	}
}

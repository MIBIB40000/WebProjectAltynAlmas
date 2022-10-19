using System;
using System.Globalization;
using System.IO;

namespace WebIP
{
	public class FileLogger
	{
		private readonly string _filePath;

		public FileLogger()
		{
			var currentDirectory = $"{Directory.GetCurrentDirectory()}\\Logs";
			var fileName = $"{DateTime.Now:dd-MM-yyyy}.txt";
			
			_filePath = $"{currentDirectory}/{fileName}";
		}

		public void DisplayLog(string message)
		{
			Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\Logs");

			using (StreamWriter w = File.AppendText(_filePath))
			{
				w.Write("\r\nLog Entry : ");
				w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
				w.WriteLine("  :{0}", message);
				w.WriteLine("-----------------------------------------------");
			}

			DeleteOldLogs();
		}

		private static void DeleteOldLogs()
		{
			var listOfFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Logs");

			foreach (string file in listOfFiles)
			{
				var filename = Path.GetFileNameWithoutExtension(file);

				if (!TryParseDate(filename, out var date))
				{
					File.Delete(Path.GetFullPath(file));
					continue;
				}

				bool isOld = date.CompareTo(DateTime.Now.AddDays(-7)) < 0;

				if (!isOld)
				{
					continue;
				}
				
				File.Delete(Path.GetFullPath(file));
			}
		}

		private static bool TryParseDate(string filename, out DateTime date)
		{
			return DateTime.TryParseExact(filename, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
				out date);
		}
	}
}

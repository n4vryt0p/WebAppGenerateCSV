using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace AF.DAL
{
	public class JsonFileDAL
	{
		public static DataTable JsonStringToTable(string jsonFIle)
		{
			string filePath = @"" + jsonFIle;
			var json = File.ReadAllText(filePath);
			DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
			return dt;
		}

		public static string jsonToCSV(string jsonContent, string delimiter)
		{
			StringWriter csvString = new StringWriter();
			
			return csvString.ToString();
		}

		public static bool AddDataRow(string tableName, string jsonFIle, object objData)
		{
			bool vRet = false;
			try
			{
				string filePath = @"" + jsonFIle;
				var json = File.ReadAllText(filePath);
				var jsonObj = JObject.Parse(json);
				var objTostr = JsonConvert.SerializeObject(objData);
				var dataRowArrary = jsonObj.GetValue(tableName) as JArray;
				var newRowData = JObject.Parse(objTostr);
				dataRowArrary.Add(newRowData);

				jsonObj[tableName] = dataRowArrary;
				string newJsonResult = JsonConvert.SerializeObject(jsonObj);
				File.AppendAllText(filePath, newJsonResult);
				vRet = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message.ToString());
				vRet = false;
			}
			return vRet;
		}

		public static bool WriteDataToFile(string filePath, object jsonObj)
		{
			bool vRet = false;
			string newJsonResult = JsonConvert.SerializeObject(jsonObj);
			try
			{

				StreamWriter sw = new StreamWriter(filePath, true);
				sw.Write(newJsonResult);
				newJsonResult = "";
				sw.Close();

			}
			catch (Exception ex)
			{
				vRet = false;
			}

			return vRet;
		}

		public static bool UpdateRecNoRow(string tableName, string jsonFIle, string totalRecs)
		{
			bool vRet = false;
			string filePath = @"" + jsonFIle;
			string json = File.ReadAllText(filePath);

			try
			{
				var jObject = JObject.Parse(json);
				var dataID = totalRecs;
				if (dataID != "")
				{
					jObject[tableName] = totalRecs;
					Console.Write("Data Value Updated !");

					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject);
					File.WriteAllText(filePath, output);
					vRet = true;
				}
				else
				{
					Console.Write("Invalid ID, Try Again!");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Update Error : " + ex.Message.ToString());
			}
			return vRet;
		}

		public static bool UpdateDataRow(string tableName, string jsonFIle, string idnameToUpdate, string idValueToUpdate)
		{
			bool vRet = false;
			string filePath = @"" + jsonFIle;
			string json = File.ReadAllText(filePath);

			try
			{
				var jObject = JObject.Parse(json);
				JArray dtArrary = (JArray)jObject[tableName];
				Console.Write("Enter ID to Update : ");
				var dataID = idValueToUpdate;

				if (dataID != "")
				{
					Console.Write("Enter new company name : ");
					var companyName = Convert.ToString(Console.ReadLine());
                    
					jObject[tableName] = dtArrary;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(filePath, output);
				}
				else
				{
					Console.Write("Invalid ID, Try Again!");
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine("Update Error : " + ex.Message.ToString());
			}
			return vRet;
		}

		public static bool DeleteDataRow(string tableName, string jsonFIle, string idnameToDelete, string valueTodelete)
		{
			bool vRet = false;
			string filePath = @"" + jsonFIle;
			var json = File.ReadAllText(filePath);
			try
			{
				var jObject = JObject.Parse(json);
				JArray dataArray = (JArray)jObject[tableName];
				var idTodelete = valueTodelete;

				if (idTodelete != "")
				{
					//var companyName = string.Empty;
					var dataToDelete = dataArray.FirstOrDefault(obj => obj[idnameToDelete].Value<string>() == idTodelete);

					dataArray.Remove(dataToDelete);

					string output = JsonConvert.SerializeObject(jObject);
					File.WriteAllText(filePath, output);
					vRet = true;
				}
				else
				{
					Console.Write("Invalid ID, Try Again!");
				}
			}
			catch (Exception)
			{
				throw;
			}

			return vRet;

		}
	}



}

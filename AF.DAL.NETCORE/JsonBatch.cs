
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AF.DAL
{
    class JsonBatch
    {
        
        public Dictionary<string, dynamic> Canonical { get; set; }
        public static Dictionary<string, dynamic> GetColumns(string Field)
        {
            var resp = new Dictionary<string, dynamic>();
            try
            {
                using (SqlDataReader results = SharedUtils
                    .ExecuteSqlCommand("SELECT * FROM [WebApp].[JsonBatchField] WHERE [JsonName] = '" + Field + "' AND [isUsed] = 1 AND [LengthData] <> '*' ORDER BY [SeqOrder]", 
                    SharedUtils.GetDSN()))
                {
                    while (results.Read())
                    {
                        resp.Add(results["FieldName"].ToString(), "character");
                    }

                }

                return resp;
            }
            catch (Exception e)
            {
                resp.Add("Error", e.Message);
                return resp;
            }
        }

        public static List<JsonField> GetFields(string Field)
        {
            var resp = new List<JsonField>();
            try
            {
                using (SqlDataReader results = SharedUtils
                    .ExecuteSqlCommand("SELECT * FROM [WebApp].[JsonBatchField] WHERE ([JsonName] = '" + Field + "' AND [isUsed] = 1 AND [FieldParent] IS NULL) ORDER BY [SeqOrder]",
                    SharedUtils.GetDSN()))
                {
                    while (results.Read())
                    {
                        var JF = new JsonField();
                        JF.FieldName = results["FieldName"].ToString();
                        JF.TypeData = results["TypeData"].ToString();
                        JF.LengthData = results["LengthData"].ToString();
                        JF.Mandatory = results["Mandatory"].ToString();
                        JF.FieldExists = Convert.ToBoolean(results["FieldExists"]);
                        JF.FieldParent = results["FieldParent"].ToString(); 
                        JF.FieldRules = results["FieldRules"].ToString(); 
                        JF.DefaultValue = results["DefaultValue"].ToString();
                        JF.isUsed = Convert.ToBoolean(results["isUsed"]); 
                        JF.SeqOrder = results["SeqOrder"].ToString();

                        if(JF.TypeData == "ARRAY" || JF.TypeData == "JSON OBJECT OF STRING")
                        {
                            JF.JObject = GetObject(JF.FieldName, JF.TypeData);
                        }


                        resp.Add(JF);
                    }

                }

                return resp;
            }
            catch (Exception e)
            {
                var JF = new JsonField();
                JF.DefaultValue = e.Message;
                resp.Add(JF);

                return resp;
            }
        }
        private static Dictionary<string, dynamic> GetObject(string Parent, string TypeData)
        {
            var resp = new Dictionary<string, dynamic>();
            try
            {
                using (SqlDataReader results = SharedUtils
                    .ExecuteSqlCommand("SELECT FieldName, TypeData, LengthData, DefaultValue, FieldExists FROM [WebApp].[JsonBatchField] WHERE ([FieldParent] = '" + Parent + "' AND [isUsed] = 1) AND [FieldExists] = 1 ORDER BY [SeqOrder]",
                    SharedUtils.GetDSN()))
                {
                    while (results.Read())
                    {
                        var fName = results["FieldName"].ToString();
                        var tData = results["TypeData"].ToString();
                        var lData = results["LengthData"].ToString();
                        var dValue = (results["DefaultValue"].ToString() != null ? results["DefaultValue"].ToString() : "");
                        var eField = Convert.ToBoolean(results["FieldExists"]);

                        if (TypeData == "JSON OBJECT OF STRING")
                        {
                            fName = Parent;
                        }

                        resp.Add(fName, new {
                            TypeData = tData,
                            LengthData = lData,
                            DefaultValue = dValue,
                            FieldExists = eField
                        });
                    }

                }

                return resp;
            }
            catch (Exception e)
            {
                resp.Add("Error", e.Message);

                return resp;
            }
        }

        public static string EncryptGPG(string strPath, string fileName, string dstPath)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ServiceGPG_Enabled"]))
            {

                var fullPath = strPath + fileName;
                IEncryptionService encryptionService = new EncryptionService(@ConfigurationManager.AppSettings["ServiceGPG"]); //C:\Program Files\GNU\GnuPG\pub\gpg2.exe

                //  Change the parameters to your private key's keyuserid and input/output files.
                var encryptedFile = encryptionService.EncryptFile(@ConfigurationManager.AppSettings["KeyID_GPG"], fullPath, fullPath + ".gpg");
                //var encryptedFile = encryptionService.DecryptFile(@"F:\LicAndPassport.PDF.pgp", @"F:\LicAndPassport.pdf");

                Console.WriteLine(encryptedFile.Name);

                try
                {
                    var localFileName = @strPath + encryptedFile.Name;
                    var networkFileName = @dstPath + encryptedFile.Name;

                    //AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);
                    //WindowsIdentity idnt = new WindowsIdentity("it_sod app_support", "AX@T0wer12FL!!");
                    //System.Security.Principal.WindowsIdentity idnt = new System.Security.Principal.WindowsIdentity(@ConfigurationManager.AppSettings["WinUser"], @ConfigurationManager.AppSettings["WinPass"]);
                    //WindowsImpersonationContext context = idnt.Impersonate();
                    //System.IO.File.Copy(localFileName, networkFileName, true);
                    //context.Undo();
                    return "SUCCESS"; // Console.WriteLine("Success!");
                }
                catch (Exception e)
                {
                    System.IO.File.WriteAllText(@ConfigurationManager.AppSettings["WebLogs"], e.ToString());

                    return e.ToString();// Console.WriteLine(e.ToString());
                }
            }
            else
            {
                return "";
            }
        }

        public static string EncryptCMD(string strPath, string fileName, string dstPath, string key = "DEF3689ABF690AD6")
        {
            try
            {
                
                //uat
                //gpg--batch--yes--recipient BF690AD6  --encrypt - files *.json
                //gpg --batch --yes --recipient BF690AD6 --encrypt-files *.json
                string command = "gpg --output \"" + dstPath + fileName + ".gpg\" --yes --recipient " + key + " --encrypt \"" + strPath + fileName + "\"";
                //gpg --batch --yes --recipient 69F1D7BE --encrypt-files *.json

                //gpg --batch --yes --trust-model always --hidden-recipient EE1A77D480866320FFA81C431865BABE69F1D7BE --encrypt-files *.json
                //versi2
                //string command = "gpg --output \"" + dstPath + fileName + ".gpg\" --yes --trust-model always --hidden-recipient " + key + " --encrypt \"" + strPath + fileName + "\"";
                //lama
                //string command = "gpg --output \"" + dstPath + fileName + ".gpg\" --recipient " + key + " --encrypt \"" + strPath + fileName + "\"";
                Process.Start("cmd.exe", "/C " + command);
                //22468BB57BE30740 | DEF3689ABF690AD6

 

                //System.Diagnostics.ProcessStartInfo procStartInfo =
                //    new System.Diagnostics.ProcessStartInfo("cmd", "/c " +
                //    "gpg --output " + strPath + fileName + ".gpg --recipient inandjo --encrypt " + strPath + fileName);

                //var localFileName = @strPath + fileName + ".gpg";
                //var networkFileName = @dstPath + fileName + ".gpg";
                //System.IO.File.Copy(localFileName, networkFileName, true);

                return fileName;
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText(@strPath + "logsfile.log", e.ToString());

                return e.Message;
            }
        }
    }

    //CMD for Encrypt GPG Files
    //  gpg --output *.gpg --recipient 22468BB57BE30740 --encrypt "pathFiles.json"
    //  gpg --batch --recipient 22468BB57BE30740 --encrypt-files *.json

    //CMD for Decrypt GPG Files
    //  echo "P@ssw0rd.1"|gpg.exe --passphrase-fd 0 --batch --verbose --yes  --decrypt-files *.gpg


    class JsonField
    {
        public string ID { get; set; }
        public string JsonName { get; set; }
        public string FieldName { get; set; }
        public string TypeData { get; set; }
        public string LengthData { get; set; }
        public string Mandatory { get; set; }
        public bool FieldExists { get; set; }
        public string FieldParent { get; set; }
        public string FieldRules { get; set; }
        public string DefaultValue { get; set; }
        public bool isUsed { get; set; }
        public string SeqOrder { get; set; }
        public Dictionary<string, dynamic> JObject { get; set; }
        public Dictionary<string, string> TArray { get; set; }
    }
}

//var id = new Dictionary<string, dynamic>();
//id.Add("IDENTIFICATION_NUMBER", "3506146006750001");
//id.Add("ISSUING_AUTHORITY", "");
//id.Add("COUNTRY_CODE", "ID");

//var zi = new List<dynamic>();
//zi.Add(id);

//var rt = new Dictionary<string, dynamic>();
//rt.Add("RUN_TIMESTAMP", SharedUtils.GetConfigs("Flow_1"));
//rt.Add("CUSTOMER_SOURCE_UNIQUE_ID", "character");
//rt.Add("ORGUNIT_CODE", "character");
//rt.Add("Customer_Identification", zi);
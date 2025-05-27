using AF.DAL;
//using Microsoft.AspNetCore;
//using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace BatchingFileWatcher
{
    
    public  static class Program
    {
        //public static void Main(string[] args)
        //{

        //    if (args.Length > 0)
        //    {
        //        switch (args[0])
        //        {
        //            case "--EncryptFiles":
        //                if (!string.IsNullOrEmpty(args[1]))
        //                {
        //                    generateEncryptFiles(args[1]);
        //                }
        //                break;
        //            case "--DecryptFiles":
        //                if (!string.IsNullOrEmpty(args[1]))
        //                {
        //                    generateDecryptFiles(args[1]);
        //                }
        //                break;
        //            case "--":

        //                break;
        //            default:
        //                generateEncryptFiles(args[0]);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        generateEncryptFiles();
        //    }
        //    ConfigLogsDAO cl = new ConfigLogsDAO();
        //    //cl.UpdateProses();

        //    Console.WriteLine("Program Exits.");
        //    Console.ReadLine();
        //    //By Amri
        //    //Buat Update Db
        //}

        public static bool generateEncryptFiles(string customRunTimestamp = "")
        {
            try
            {

                Console.WriteLine("From Path: " + ConfigurationManager.AppSettings["JsonDir"]);
                Console.WriteLine("Start Creating Json File.");

                var customerRunTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (!string.IsNullOrEmpty(customRunTimestamp))
                {
                    customerRunTimestamp = Convert.ToDateTime(customRunTimestamp).ToString("yyyy-MM-dd HH:mm:ss");
                }

                var x = createJson(customerRunTimestamp);
                if (x != null)
                {
                    Console.WriteLine("Extracted Status : " + x.extractStat);
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to create JSON file.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }

        public static void generateDecryptFiles(string runTimestamp)
        {
            var jsonPath = Path.Combine(ConfigurationManager.AppSettings["JsonDir"], runTimestamp);
            var udmPath = Path.Combine(ConfigurationManager.AppSettings["UdmDir"], "in");

            StringCollection allFiles = new StringCollection();
            listAllFiles(allFiles, udmPath, "*"+ runTimestamp + "*.gpg", true);

            if(allFiles.Count > 0)
            {
                var destFiles = Path.Combine(ConfigurationManager.AppSettings["JsonDir"] + runTimestamp, "in");
                if (!Directory.Exists(destFiles))
                {
                    DirectoryInfo folder = Directory.CreateDirectory(destFiles);
                }

                foreach (var file in allFiles)
                {
                    var fileEncrypted = new FileInfo(file.ToString());
                    
                    System.IO.File.Copy(Path.Combine(udmPath, fileEncrypted.Name),Path.Combine(destFiles, fileEncrypted.Name), true);
                }

                runScript(destFiles);
            }
        }
        private static dynamic createJson(string customRunTimestamp = "", int tid = 1, string tname = "CUSTOMERS")
        {

            dynamic u = new System.Dynamic.ExpandoObject();

            try
            {
                //ManualProcessCanonical.ManualCustomerExtraction();

                bool n = ManualProcessCanonical.ManualExtractionByTaskID(customRunTimestamp);
                if (n)
                {
                    u.hasil = 1;
                    u.taskName = tname;
                }
                //ManualProcessCanonical.ManualCustomerExtraction();
            }
            catch (Exception e)
            {
                u.hasil = 0;
                u.taskName = tname;

                Console.WriteLine(e.ToString());
            }

            u.extractStat = ManualProcessCanonical.extractedStatus();

            return u;
        }

        private static StringCollection listAllFiles(StringCollection allFiles, string path, string ext, bool scanDirOk)
        {
            // listFilesCurrDir: Table containing the list of files in the 'path' folder
            string[] listFilesCurrDir = Directory.GetFileSystemEntries(path, ext);

            // read the array 'listFilesCurrDir'
            foreach (string rowFile in listFilesCurrDir)
            {
                // If the file is not already in the 'allFiles' list
                if (allFiles.Contains(rowFile) == false)
                {
                    // Add the file (at least its address) to 'allFiles'
                    var file = new FileInfo(rowFile).Name;
                    allFiles.Add(rowFile);
                }
            }
            // Clear the 'listFilesCurrDir' table for the next list of subfolders
            listFilesCurrDir = null;

            // If you allow subfolders to be read
            if (scanDirOk)
            {
                // List all the subfolders present in the 'path'
                string[] listDirCurrDir = Directory.GetDirectories(path);

                // if there are subfolders (if the list is not empty)
                if (listDirCurrDir.Length != 0)
                {
                    // read the array 'listDirCurrDir'
                    foreach (string rowDir in listDirCurrDir)
                    {
                        // Restart the procedure to scan each subfolder
                        listAllFiles(allFiles, rowDir, ext, scanDirOk);
                    }
                }
                // Clear the 'listDirCurrDir' table for the next list of subfolders
                listDirCurrDir = null;

            }
            // return 'allFiles'
            return allFiles;
        }

        private static void runScript(string dirPath)
        {
            try
            {

				var str = "gpg.exe --pinentry-mode=loopback --batch --passphrase \"P@ssw0rd.1\" --decrypt-files *.gpg";
                File.AppendAllText(Path.Combine(dirPath, "process.bat"), str);

                if (File.Exists(Path.Combine(dirPath, "process.bat")))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;

                    psi.WorkingDirectory = dirPath;

                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
                    // actually NEED to set this as a local string variable
                    // and pass it - bombs otherwise!
                    //gpg --pinentry -mode=loopback --passphrase  "PASSWORD" -d -o "PATH\TO\OUTPUT" "PATH\TO\FILE.gpg"
                    string sCommandLine = "process.bat";

                    process.StandardInput.WriteLine(sCommandLine);
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    process.WaitForExit();
                    process.Close();

                    Console.WriteLine("File process.bat Execution Success!");
                }
                else
                {
                    Console.WriteLine("File process.bat Not Found!");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

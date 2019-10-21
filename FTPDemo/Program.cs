using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FTPDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Demoing the file upload...");
            //var files = ListFiles();
            //FileUpload();
            Send();
            Console.WriteLine("Completed.");
            Console.WriteLine("Press any key to continue...");
        }
                
        // Enter your host name or IP here
        //private static string host = "test.rebex.net";

        private static string host = "172.27.120.125";// "172.18.65.209";

        private static int port = 22;

        // Enter your sftp username here
        //private static string username = "demo";
        private static string username = "tester";
        // Enter your sftp password here
        private static string password = "password";
        public static void Send()
        {
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(host, port, username, password);
            try
            {
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Console.WriteLine("I'm connected to the client");
                        Stopwatch stopwatch = new Stopwatch();

                        var uploadfile = @"D:\D.zip";  //Replace it with local file
                        Console.WriteLine("Uploading of file started.");

                        using (var fileStream = File.OpenRead(uploadfile))
                        {
                            //client.BufferSize = (uint)fileStream.Length; // bypass Payload error large files
                            stopwatch.Start();
                            client.UploadFile(fileStream, Path.GetFileName(uploadfile));
                            stopwatch.Stop();

                            Console.WriteLine("Uploading of file is completed.");
                            Console.WriteLine("Total time taken + ", stopwatch.Elapsed);
                        }
                    }
                    else
                    {
                        Console.WriteLine("FTP server is not available.");
                    }
                    client.Disconnect();
                }
            }
            catch (Renci.SshNet.Common.SshConnectionException)
            {
                Console.WriteLine("Cannot connect to the server.");                
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Unable to establish the socket.");                
            }
            catch (Renci.SshNet.Common.SshAuthenticationException)
            {
                Console.WriteLine("Authentication of SSH session failed.");                
            }            
        }

        public static void FileUpload()
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(username, password);

            // Copy the contents of the file to the request stream.
            byte[] fileContents = Encoding.ASCII.GetBytes("nikhil");

            request.ContentLength = fileContents.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(fileContents, 0, fileContents.Length);
            stream.Close();
            FtpWebResponse res = (FtpWebResponse)request.GetResponse();
            Console.WriteLine($"Upload File Complete, status {res.StatusDescription}");
        }

        private static List<string> ListFiles()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://172.18.65.209");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.EnableSsl = true;
                request.UseBinary = true;
                request.UsePassive = true;
                request.Credentials = new NetworkCredential(username, password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static List<string> ListFiles123()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://test.rebex.net/");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential("demo", "password");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }      
    }
}

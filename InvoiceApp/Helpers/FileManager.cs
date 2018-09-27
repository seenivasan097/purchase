using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.SharePoint.Client;
using File = System.IO.File;

namespace InvoiceApp.Helpers
{
    public class FileManager
    {
        private string HRMSFilesRootPath;

        public FileManager()
        {
            HRMSFilesRootPath = ConfigurationManager.AppSettings["HRMSFileStorage"];
            CreateRootPath();
        }

        private void CreateRootPath()
        {
            //if (!Directory.Exists(HRMSFilesRootPath))
            //{
            //    Directory.CreateDirectory(HRMSFilesRootPath);
            //}
            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(HRMSFilesRootPath));

            if (!exists)
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(HRMSFilesRootPath));
        }

        public FileInfo SaveFile(HttpPostedFileBase file, string fileName, string folderName)
        {
            var folderInfo = CreateUniqueDir(HRMSFilesRootPath, folderName);
            var filePath = Path.Combine(folderInfo.FullName, fileName);
            var fileStream = File.Create(filePath);
            fileStream.Dispose();

            string fname = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/"), filePath);
            file.SaveAs(fname);

            //var fileToWrite = File.OpenWrite(filePath);
            //file.InputStream.Position = 0;
            //file.InputStream.CopyTo(fileToWrite);
            //fileToWrite.Dispose();

            return new FileInfo(filePath);
        }


        private DirectoryInfo CreateUniqueDir(string rootPath, string folderName)
        {
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            var userFolderName = guid1 + "-" + guid2;
            var folderPath = Path.Combine(rootPath, folderName);
           
            if (!Directory.Exists(folderPath))
            {
                var dirInfo = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));
                return dirInfo;
            }
            else
            {
                DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath(folderPath));
                return directory;
            }
          
            //throw new Exception("Directory Already Exists");
        }

        /// <summary>
        /// Uploading files to share point
        /// </summary>
        /// <param name="filePath"></param>
        public string UploadFileToSharePoint(string filePath, string folderName)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            try
                {
                    string sharepointLink = ConfigurationManager.AppSettings["SharepointLink"];
                    string sharepointlistName = ConfigurationManager.AppSettings["SPListName"];
                    string sharepointUsername = ConfigurationManager.AppSettings["SPUsername"];
                    string sharepointPassword = ConfigurationManager.AppSettings["SPPassword"];

                    // file path url
                    string sUrl = sharepointLink + sharepointlistName + "/" + folderName + "/" + fileInfo.Name.TrimStart();// "test.txt";

                    //Uri hosturi = new Uri(sharepointLink);
                    using (var ctx = new ClientContext(sharepointLink))
                    {
                        //Authorized credentials 
                        ctx.Credentials = new NetworkCredential(sharepointUsername, sharepointPassword);
                        //Choose parent folder name
                        var list = ctx.Web.Lists.GetByTitle(sharepointlistName);

                        //Check folder already exists
                        int folderCount = GetFolderExists(ctx, sharepointlistName, folderName);
                        if (folderCount == 0)
                        {
                            // Create new folder in path
                            list.CreateFolder(folderName);
                            
                        }
                        if (FileExists(list, new Uri(sUrl).LocalPath))
                        {
                            throw new Exception("File Name Already Exists");
                        }
                        ctx.ExecuteQuery();
                    }

                   

                    WebRequest request = WebRequest.Create(sUrl);
                    request.Credentials = new NetworkCredential(sharepointUsername, sharepointPassword);

                    request.Method = "PUT";
                    byte[] buffer = new byte[1024];

                    //Write file to folder path
                    using (Stream stream = request.GetRequestStream())
                    {
                        using (FileStream fsWorkbook = File.Open(filePath, FileMode.Open, FileAccess.Read))
                        {
                            int i = fsWorkbook.Read(buffer, 0, buffer.Length);

                            while (i > 0)
                            {
                                stream.Write(buffer, 0, i);
                                i = fsWorkbook.Read(buffer, 0, buffer.Length);
                            }
                        }
                    }

                    var responce = request.GetResponse();

                    return responce.ResponseUri.OriginalString;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
        }


        private static int GetFolderExists(ClientContext clientContext, String listTitle, String folderName)
        {
            int existingFolder = 0;

            Web web = clientContext.Web;
            ListCollection lists = web.Lists;

            List list = web.Lists.GetByTitle(listTitle);

            if (list != null)
            {
                FolderCollection folders = list.RootFolder.Folders;
                clientContext.Load(folders, fl => fl.Include(ct => ct.Name).Where(ct => ct.Name == folderName));
                clientContext.ExecuteQuery();
                existingFolder = folders.Count();
            }

            return existingFolder;
        }

        private static bool FileExists(List list, string fileUrl)
        {
            var ctx = list.Context;
            var qry = new CamlQuery();
            qry.ViewXml = string.Format("<View Scope=\"RecursiveAll\"><Query><Where><Eq><FieldRef Name=\"FileRef\"/><Value Type=\"Url\">{0}</Value></Eq></Where></Query></View>", fileUrl);
            var items = list.GetItems(qry);
            ctx.Load(items);
            ctx.ExecuteQuery();
            return items.Count > 0;
        }

    }

    public static class ListExtensions
    {
        public static void CreateFolder(this List list, string name)
        {
            var info = new ListItemCreationInformation
            {
                UnderlyingObjectType = FileSystemObjectType.Folder,
                LeafName = name
            };
            var newItem = list.AddItem(info);
            newItem["Title"] = name;
            newItem.Update();
        }
    }
}
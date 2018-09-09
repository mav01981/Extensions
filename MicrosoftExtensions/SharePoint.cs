namespace MicrosoftExtensions
{
    using Microsoft.SharePoint.Client;
    using System;
    using System.Linq;

    public static class SharePointExtensions
    {
        public static bool FolderExists(this ClientContext context, string filePath)
        {
            try
            {
                var folder = context.Web.GetFolderByServerRelativeUrl(filePath);
                context.Load(folder);
                context.ExecuteQuery();
                return true;
            }
            catch (ServerException ex)
            {
                if (ex.ServerErrorTypeName == "System.IO.DirectoryNotFoundException")
                {
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool FileExists(this ClientContext context, string fileUrl)
        {
            //"/sites/Documents/MyFile.docx"
            File checkFile = context.Web.GetFileByServerRelativeUrl(fileUrl);
            context.Load(checkFile, fe => fe.Exists);
            context.ExecuteQuery();

            return checkFile.Exists;
        }

        public static void CreateFolder(this ClientContext context, string root, string folder)
        {
            List docs = context.Web.Lists.GetByTitle(root);
            docs.EnableFolderCreation = true;
            docs.RootFolder.Folders.Add(folder);
            context.ExecuteQuery();
        }

        public static void DeletFolder(this ClientContext context, string folder)
        {
            context.Web.GetFileByServerRelativeUrl(folder).DeleteObject();
            context.ExecuteQuery();
        }

        public static void DeleteAllFiles(this ClientContext context, string folderRelativeUrl)
        {
            var folder = context.Web.GetFolderByServerRelativeUrl(folderRelativeUrl);
            context.Load(folder.Files);
            context.ExecuteQuery();
            folder.Files.ToList().ForEach(file => file.DeleteObject());
            context.ExecuteQuery();
        }

        public static void DeleteAllFiles(this ClientContext context, string folderRelativeUrl, string search)
        {
            var folder = context.Web.GetFolderByServerRelativeUrl(folderRelativeUrl);
            context.Load(folder.Files);
            context.ExecuteQuery();
            folder.Files.Where(x => x.Name.Contains(search))
                .ToList().ForEach(file => file.DeleteObject());
            context.ExecuteQuery();
        }
    }
}

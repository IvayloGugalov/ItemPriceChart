using System.IO;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        public string GetDirectoryName(string filePath) => Path.GetDirectoryName(filePath);

        public string PathCombine(string leftPath, string rightPath) => Path.Combine(leftPath, rightPath);

        public string GetFileName(string filePath) => Path.GetFileName(filePath);

        public string GetExtension(string filePath) => Path.GetExtension(filePath);

        public string ChangeExtension(string filePath, string extension) => Path.ChangeExtension(filePath, extension);

        public bool FileExists(string filePath) => File.Exists(filePath);

        public void FileCopy(string sourceFileName, string destFileName) => File.Copy(sourceFileName, destFileName);
    }

    /// <summary>
    /// Wrapper on the System.IO Namespace
    /// </summary>
    public interface IFileSystemWrapper
    {
        string GetDirectoryName(string filePath);
        string PathCombine(string leftPath, string rightPath);
        string GetFileName(string filePath);
        string GetExtension(string filePath);
        string ChangeExtension(string filePath, string extension);

        bool FileExists(string filePath);
        void FileCopy(string sourceFileName, string destFileName);
    }
}

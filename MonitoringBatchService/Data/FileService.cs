namespace MonitoringBatchService.Data
{

    public interface IFileService
    {
        string[] GetFiles(string path, string searchPattern);
        FileInfo[] GetFilesa(string path, string searchPattern);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        FileInfo[] GetFiles(string path, string searchPattern, SearchOption searchOption);
        DirectoryInfo[] GetDirectories(string path, string searchPattern, SearchOption searchOption);
        void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite);
        DateTime GetLastWriteTime(string path);
        bool FileExists(string path);
        long GetFileSize(string path);
        Task<string> ReadAllTextAsync(string path);
    }
    public class FileService : IFileService
    {
        public string[] GetFiles(string path , string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }
        public FileInfo[] GetFilesa(string path, string searchPattern)
        {
            return new DirectoryInfo(path).GetFiles(searchPattern);
        }
        public bool DirectoryExists(string path) => Directory.Exists(path);
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);
        public FileInfo[] GetFiles(string path, string searchPattern, SearchOption searchOption) =>
            new DirectoryInfo(path).GetFiles(searchPattern, searchOption);
        public DirectoryInfo[] GetDirectories(string path, string searchPattern, SearchOption searchOption) =>
            new DirectoryInfo(path).GetDirectories(searchPattern, searchOption);
        public void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite) =>
            File.Copy(sourceFilePath, destinationFilePath, overwrite);
        public DateTime GetLastWriteTime(string path) => File.GetLastWriteTime(path);
        public bool FileExists(string path) => File.Exists(path);
        public async Task<string> ReadAllTextAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }
        public long GetFileSize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("File name cannot be null or empty", nameof(path));

            if (!FileExists(path))
                return 0;

            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Length; // Mengembalikan ukuran file dalam byte
        }
    }
}

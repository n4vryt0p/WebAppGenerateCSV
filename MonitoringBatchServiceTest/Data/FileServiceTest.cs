using System;
using System.IO;
using Xunit;
using MonitoringBatchService.Data;

namespace MonitoringBatchServiceTest.Data
{
    public class FileServiceTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly FileService _fileService;

        public FileServiceTests()
        {
            // Buat direktori sementara untuk pengujian
            _testDirectory = Path.Combine(Path.GetTempPath(), "FileServiceTest");
            Directory.CreateDirectory(_testDirectory);

            // Tambahkan file uji
            File.WriteAllText(Path.Combine(_testDirectory, "test1.json.gpg"), "dummy data");
            File.WriteAllText(Path.Combine(_testDirectory, "test2.json.gpg"), "dummy data");
            File.WriteAllText(Path.Combine(_testDirectory, "test3.txt"), "dummy data"); // Harus diabaikan

            _fileService = new FileService();
        }

        [Fact]
        public void GetFiles_Returns_OnlyMatchingFiles()
        {
            // Act
            var result = _fileService.GetFiles(_testDirectory, "*.json.gpg");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length); // Harusnya hanya 2 file yang cocok
            Assert.Contains(result, f => f.EndsWith("test1.json.gpg"));
            Assert.Contains(result, f => f.EndsWith("test2.json.gpg"));
        }

        [Fact]
        public void GetFilesa_Returns_OnlyMatchingFileInfos()
        {
            // Act
            var result = _fileService.GetFilesa(_testDirectory, "*.json.gpg");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Contains(result, f => f.Name == "test1.json.gpg");
            Assert.Contains(result, f => f.Name == "test2.json.gpg");
        }

        [Fact]
        public void DirectoryExists_ReturnsTrue_WhenDirectoryExists()
        {
            // Act
            var result = _fileService.DirectoryExists(_testDirectory);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DirectoryExists_ReturnsFalse_WhenDirectoryDoesNotExist()
        {
            // Arrange
            var nonExistentDirectory = Path.Combine(_testDirectory, "NonExistent");

            // Act
            var result = _fileService.DirectoryExists(nonExistentDirectory);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateDirectory_CreatesDirectory_WhenDirectoryDoesNotExist()
        {
            // Arrange
            var newDirectory = Path.Combine(_testDirectory, "NewDirectory");

            // Act
            _fileService.CreateDirectory(newDirectory);

            // Assert
            Assert.True(Directory.Exists(newDirectory));
        }

        [Fact]
        public void GetFiles_WithSearchOption_ReturnsFilesInSubdirectories()
        {
            // Arrange
            var subDirectory = Path.Combine(_testDirectory, "SubDirectory");
            Directory.CreateDirectory(subDirectory);
            File.WriteAllText(Path.Combine(subDirectory, "test4.json.gpg"), "dummy data");

            // Act
            var result = _fileService.GetFiles(_testDirectory, "*.json.gpg", SearchOption.AllDirectories);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Length); // Harusnya 3 file yang cocok (2 di root, 1 di subdirectory)
        }

        [Fact]
        public void GetDirectories_Returns_OnlyMatchingDirectories()
        {
            // Arrange
            var subDirectory1 = Path.Combine(_testDirectory, "SubDirectory1");
            var subDirectory2 = Path.Combine(_testDirectory, "SubDirectory2");
            Directory.CreateDirectory(subDirectory1);
            Directory.CreateDirectory(subDirectory2);

            // Act
            var result = _fileService.GetDirectories(_testDirectory, "*", SearchOption.TopDirectoryOnly);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Contains(result, d => d.Name == "SubDirectory1");
            Assert.Contains(result, d => d.Name == "SubDirectory2");
        }

        [Fact]
        public void CopyFile_CopiesFile_WhenDestinationDoesNotExist()
        {
            // Arrange
            var sourceFile = Path.Combine(_testDirectory, "test1.json.gpg");
            var destinationFile = Path.Combine(_testDirectory, "test1_copy.json.gpg");

            // Act
            _fileService.CopyFile(sourceFile, destinationFile, overwrite: false);

            // Assert
            Assert.True(File.Exists(destinationFile));
        }

        [Fact]
        public void CopyFile_OverwritesFile_WhenDestinationExistsAndOverwriteIsTrue()
        {
            // Arrange
            var sourceFile = Path.Combine(_testDirectory, "test1.json.gpg");
            var destinationFile = Path.Combine(_testDirectory, "test1_copy.json.gpg");
            File.WriteAllText(destinationFile, "old data");

            // Act
            _fileService.CopyFile(sourceFile, destinationFile, overwrite: true);

            // Assert
            Assert.Equal("dummy data", File.ReadAllText(destinationFile));
        }

        [Fact]
        public void GetLastWriteTime_ReturnsCorrectLastWriteTime()
        {
            // Arrange
            var filePath = Path.Combine(_testDirectory, "test1.json.gpg");
            var expectedLastWriteTime = File.GetLastWriteTime(filePath);

            // Act
            var result = _fileService.GetLastWriteTime(filePath);

            // Assert
            Assert.Equal(expectedLastWriteTime, result);
        }

        [Fact]
        public void FileExists_ReturnsTrue_WhenFileExists()
        {
            // Arrange
            var filePath = Path.Combine(_testDirectory, "test1.json.gpg");

            // Act
            var result = _fileService.FileExists(filePath);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FileExists_ReturnsFalse_WhenFileDoesNotExist()
        {
            // Arrange
            var nonExistentFile = Path.Combine(_testDirectory, "NonExistent.json.gpg");

            // Act
            var result = _fileService.FileExists(nonExistentFile);

            // Assert
            Assert.False(result);
        }

        public void Dispose()
        {
            // Hapus direktori uji setelah pengujian selesai
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }
    }
}
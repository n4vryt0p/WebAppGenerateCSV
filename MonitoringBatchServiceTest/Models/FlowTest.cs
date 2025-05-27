using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class Flowtest

    {
        [Fact]
        public void flowtest()
        {
            // Arrange
            var  keymodel = new Flow();

            // Act
            // Tidak ada logika khusus yang perlu diuji, hanya memeriksa nilai default.

            // Assert
            Assert.Null(keymodel.Key);
            Assert.Null(keymodel.Value);
            Assert.Null(keymodel.Desc);
            Assert.Null(keymodel.Group);
;
        }
    }
}

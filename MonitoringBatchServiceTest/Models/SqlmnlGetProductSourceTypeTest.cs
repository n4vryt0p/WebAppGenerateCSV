using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetProductSourceTypeTest
    {
        [Fact]
        public void SqlmnlGetProductSourceType()
        {
            // Arrange
            var keymodel = new SqlmnlGetProductSourceType();

            // Properti nullable (string? dan DateTime?)
            Assert.Null(keymodel.PRODUCT_ID);
            Assert.Null(keymodel.PRODUCT_SOURCE_TYPE_CODE);
            Assert.Null(keymodel.PRODUCT_SOURCE_TYPE_DESC);
            Assert.Null(keymodel.X_ORGUNIT_CODE);
            Assert.Null(keymodel.MODIFIED_DATE);

            // Properti non-nullable (DateTime)
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); // atau gunakan nilai yang diharapkan
        }
        }
}

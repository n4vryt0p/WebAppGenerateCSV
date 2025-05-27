using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetProductsTest
    {

        [Fact]
        public void SqlmnlGetProducts()
        {
            // Arrange
            var keymodel = new SqlmnlGetProducts();
            // Properti nullable (string? dan DateTime?)
            Assert.Null(keymodel.PRODUCT_ID);
            Assert.Null(keymodel.PRODUCT_NAME);
            Assert.Null(keymodel.PRODUCT_GROUP);
            Assert.Null(keymodel.PRODUCT_CLASS);
            Assert.Null(keymodel.PRODUCT_LINE);
            Assert.Null(keymodel.X_ORGUNIT_CODE);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.SystemSRC_CD);
            Assert.Null(keymodel.Diff_HK);

            // Properti non-nullable (DateTime)
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); // atau gunakan nilai yang diharapkan
        }
        }
}

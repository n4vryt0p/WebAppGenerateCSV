using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetCustomerCustomerLinkTest
    {
        [Fact]
        public void SqlmnlGetCustomerCustomerLinnk()
        {
            // Arrange
            var keymodel = new SqlmnlGetCustomerCustomerLink();

            Assert.Null(keymodel.CUSTOMER1_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.CUSTOMER2_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.LINK_TYPE);
            Assert.Null(keymodel.PERCENTAGE_OWNERSHIP);
            Assert.Null(keymodel.ULTIMATE_BENEFICIAL_OWNER_FLAG);
            Assert.Null(keymodel.X_ORGUNIT_CODE);
       
            Assert.Equal(default(DateTime), keymodel.FROM_TIMESTAMP); 
            Assert.Equal(default(DateTime), keymodel.TO_TIMESTAMP);
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); 
            Assert.Null(keymodel.MODIFIED_DATE);
        }

        }
}

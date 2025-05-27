using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.DAL.Model.ClaimModel;
using static AF.DAL.Model.PolicyModela;

namespace MonitoringBatchServiceTest.Models
{
    public class PolicyModelTest
    {
        [Fact]
        public void Productcovertest()
        {
            // Arrange
            var keymodel = new Policya();

            // Act

            // Assert
            Assert.Null(keymodel.policyNumber);
            Assert.Null(keymodel.policyStatus_CD);
            Assert.Null(keymodel.productCode);
            Assert.Null(keymodel.start_Eff_DT);
            Assert.Null(keymodel.end_Eff_DT);
            Assert.Null(keymodel.start_Issue_DT);
            Assert.Null(keymodel.end_Issue_DT);

        }
        [Fact]
        public void Searchqcover()
        {
            // Arrange
            var keymodel = new Searchq();

            // Act

            // Assert
            Assert.Null(keymodel.policy);


        }
        [Fact]
        public void SearchParameteraqcover()
        {
            // Arrange
            var keymodel = new SearchParameteraq();

            // Act

            // Assert
            Assert.Null(keymodel.search);


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.DAL.Model;
using MonitoringBatchService.Models;
using static AF.DAL.Model.ClaimModel;

namespace MonitoringBatchServiceTest.Models
{
    public class ClaimModelTest
    {

        [Fact]
        public void claimtestcover()
        {
            // Arrange
            var keymodel = new Claima();

            // Act
       
           string policies = keymodel.policyNumber;

            // Assert
            Assert.Null(keymodel.policyNumber);
            Assert.Null(keymodel.claimNumber);
            Assert.Null(keymodel.claimType_CD);
            Assert.Null(keymodel.claimStatus_CD);
            Assert.Null(keymodel.start_Report_DT);
            Assert.Null(keymodel.end_Report_DT);

        }
        
         [Fact]
        public void SearchaCover()
        {
            // Arrange
            var keymodel = new Searcha();

            // Act

            // Assert
            Assert.Null(keymodel.claim);
          
        }
        [Fact]
        public void SearchParameteCover()
        {
            // Arrange
            var keymodel = new SearchParametera();

            // Act

            // Assert
            Assert.Null(keymodel.search);

        }
        
    }
}

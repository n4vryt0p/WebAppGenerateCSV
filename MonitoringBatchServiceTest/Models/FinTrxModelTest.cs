using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.DAL.Model.ClaimModel;
using static AF.DAL.Model.FinTrxModel;
using static AF.DAL.Model.PartyModel;

namespace MonitoringBatchServiceTest.Models
{
    public class FinTrxModelTest
    {

        [Fact]
        public void PartyModelCover()
        {
            // Arrange
            var keymodel = new RequestParam();

            // Act


            // Assert
            Assert.Null(keymodel.PartyType_CD);
            Assert.Null(keymodel.FirstName);
            Assert.Null(keymodel.MiddleName);
            Assert.Null(keymodel.LastName);
            Assert.Null(keymodel.Gender);
            Assert.Null(keymodel.DOB);

        }

        [Fact]
        public void SearchacCover()
        {
            // Arrange
            var keymodel = new Searchac();

            // Act

            // Assert
            Assert.Null(keymodel.finTrx);

        }
        [Fact]
        public void SearchParameteraccover()
        {
            // Arrange
            var keymodel = new SearchParameterac();

            // Act

            // Assert
            Assert.Null(keymodel.search);

        }
    }
}

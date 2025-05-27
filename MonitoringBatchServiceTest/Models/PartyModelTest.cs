using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.DAL.Model;
using static AF.DAL.Model.FinTrxModel;
using static AF.DAL.Model.PartyModel;

namespace MonitoringBatchServiceTest.Models
{
    public class PartyModelTest
    {
        [Fact]
        public void FinTrxaccover()
        {
            // Arrange
            var keymodel = new FinTrxac();

            // Act


            // Assert
            Assert.Null(keymodel.TrxSRC_ID);
            Assert.Null(keymodel.PolicyNumber);
            Assert.Null(keymodel.DebitCredit_Ind);
            Assert.Null(keymodel.trxType_CD);
            Assert.Null(keymodel.trxStatus_CD);
            Assert.Null(keymodel.end_DT);

        }
        [Fact]
        public void PartyViewModelview()
        {
            // Arrange
            var keymodel = new PartyViewModel();

            // Act


            // Assert
            Assert.Null(keymodel.Party);
         

        }

        
        [Fact]
        public void Partycover()
        {
            // Arrange
            var keymodel = new Party();

            // Act


            // Assert
            Assert.Null(keymodel.Party_HK);
            Assert.Null(keymodel.SystemSRC_CD); 
            Assert.Null(keymodel.PartySRC_ID);
            Assert.Null(keymodel.PartyType_CD);
            Assert.Null(keymodel.Person);
            Assert.Null(keymodel.Addresses);
            Assert.Null(keymodel.Emails);
            Assert.Null(keymodel.Identities);
            Assert.Null(keymodel.Phones);



        }

        
        [Fact]
        public void Personcover()
        {
            // Arrange
            var keymodel = new Person();

            // Act

            // Assert
            Assert.Null(keymodel.FirstName);
            Assert.Null(keymodel.LastName);
            Assert.Null(keymodel.Gender);
            Assert.Null(keymodel.Birth_DT);
            Assert.Null(keymodel.BirthPlace);
            Assert.Null(keymodel.Citizenship);

        }
        
        [Fact]
        public void Organizationcover()
        {
            // Arrange
            var keymodel = new Organization();

            // Act

            // Assert
            Assert.Null(keymodel.FullName);
            Assert.Null(keymodel.OrgForm);
            Assert.Null(keymodel.Establish_DT);
            Assert.Null(keymodel.NatureCategory);

        }



        // Test for root PartyDetail class
        [Fact]
        public void PartyDetail_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var partyDetail = new PartyDetail();

            // Assert
            // Uncomment if Data property is enabled
            // Assert.Null(partyDetail.Data);
        }

        // Tests for Data nested class
        [Fact]
        public void Data_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var data = new PartyDetail.Data();

            // Assert
            Assert.Null(data.Party);
        }

        // Tests for Party nested class
        [Fact]
        public void Party_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var party = new PartyDetail.Party();

            // Assert
            Assert.Null(party.PartyHK);
            Assert.Null(party.SystemSRC_CD);
            Assert.Null(party.PartySRC_ID);
            Assert.Null(party.PartyType_CD);
            Assert.Null(party.Person);
            Assert.Null(party.Addresses);
            Assert.Null(party.Emails);
            Assert.Null(party.Identities);
            Assert.Null(party.Phones);
        }

        [Fact]
        public void Party_ShouldHandleArrayProperties()
        {
            // Arrange
            var party = new PartyDetail.Party();

            // Act
            party.Addresses = new PartyDetail.Address[1];
            party.Emails = new PartyDetail.Email[2];
            party.Identities = new PartyDetail.Identity[3];
            party.Phones = new PartyDetail.Phone[4];

            // Assert
            Assert.Single(party.Addresses);
            Assert.Equal(2, party.Emails.Length);
            Assert.Equal(3, party.Identities.Length);
            Assert.Equal(4, party.Phones.Length);
        }

        // Tests for Person nested class
        [Fact]
        public void Person_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var person = new PartyDetail.Person();

            // Assert
            Assert.Null(person.FirstName);
            Assert.Null(person.LastName);
            Assert.Null(person.Gender);
            Assert.Null(person.Birth_DT);
            Assert.Null(person.BirthPlace);
            Assert.Null(person.Citizenship);
            Assert.Null(person.Death_DT);
            Assert.Null(person.MaritalStatus);
        }

        // Tests for Address nested class
        [Fact]
        public void Address_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var address = new PartyDetail.Address();

            // Assert
            Assert.Null(address.AddressType_CD);
            Assert.Null(address.StreetNM);
            Assert.Null(address.City);
            Assert.Null(address.Province);
            Assert.Null(address.Country_CD);
            Assert.Null(address.zip);
        }

        // Tests for Email nested class
        [Fact]
        public void Email_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var email = new PartyDetail.Email();

            // Assert
            Assert.Null(email.Emailtype_CD);
            Assert.Null(email.Address);
        }

        // Tests for Identity nested class
        [Fact]
        public void Identity_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var identity = new PartyDetail.Identity();

            // Assert
            Assert.Null(identity.IdType_CD);
            Assert.Null(identity.IdNumber);
        }

        // Tests for Phone nested class
        [Fact]
        public void Phone_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var phone = new PartyDetail.Phone();

            // Assert
            Assert.Null(phone.PhoneType_CD);
            Assert.Null(phone.CountryCallingCode);
            Assert.Null(phone.AreaCode);
            Assert.Null(phone.DialNumber);
        }

        // Integration test for complete model structure
        [Fact]
        public void CompleteModelStructure_ShouldWorkCorrectly()
        {
            // Arrange
            var partyDetail = new PartyDetail();
            var data = new PartyDetail.Data();
            var party = new PartyDetail.Party();
            var person = new PartyDetail.Person();
            var address = new PartyDetail.Address();
            var email = new PartyDetail.Email();
            var identity = new PartyDetail.Identity();
            var phone = new PartyDetail.Phone();

            // Act - Set sample values
            person.FirstName = "John";
            person.LastName = "Doe";

            address.AddressType_CD = "HOME";
            address.StreetNM = "123 Main St";

            email.Emailtype_CD = "WORK";
            email.Address = "john.doe@example.com";

            identity.IdType_CD = "KTP";
            identity.IdNumber = "1234567890";

            phone.PhoneType_CD = "MOBILE";
            phone.CountryCallingCode = "62";
            phone.AreaCode = "21";
            phone.DialNumber = "12345678";

            party.PartyHK = "PARTY123";
            party.Person = person;
            party.Addresses = new[] { address };
            party.Emails = new[] { email };
            party.Identities = new[] { identity };
            party.Phones = new[] { phone };

            data.Party = party;
            // Uncomment if Data property is enabled
            // partyDetail.Data = data;

            // Assert
            // Verify person data
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Doe", person.LastName);

            // Verify address data
            Assert.Equal("HOME", address.AddressType_CD);
            Assert.Equal("123 Main St", address.StreetNM);

            // Verify party data
            Assert.Equal("PARTY123", party.PartyHK);
            Assert.NotNull(party.Person);
            Assert.Single(party.Addresses);
            Assert.Single(party.Emails);
            Assert.Single(party.Identities);
            Assert.Single(party.Phones);

            // Verify complete structure
            // Uncomment if Data property is enabled
            // Assert.NotNull(partyDetail.Data);
            // Assert.NotNull(partyDetail.Data.Party);
        }
    
}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL.Model
{
	public class PartyModel
	{
		//public Data Data { get; set; }


		public class RequestParam
		{
			public string PartyType_CD { get; set; }
			public string FirstName { get; set; }
			public string MiddleName { get; set; }
			public string LastName { get; set; }
			public string Gender { get; set; }
			public string DOB { get; set; }
		}
		public class Data

		{
			public Party[] Party { get; set; }
		}

		public class PartyViewModel
		{
			public object Party { get; set; }
		}
		public class Party
		{
			public string Party_HK { get; set; }
			public string SystemSRC_CD { get; set; }
			public string PartySRC_ID { get; set; }
			public string PartyType_CD { get; set; }
			public object Person { get; set; }
			public object Addresses { get; set; }
			public object Emails { get; set; }
			public object Identities { get; set; }
			public object Phones { get; set; }
		}
		public class Person
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Gender { get; set; }
			public string Birth_DT { get; set; }
			public string BirthPlace { get; set; }
			public string Citizenship { get; set; }
		}
		public class Organization
		{
			public string FullName { get; set; }
			public string OrgForm { get; set; }
			public string Establish_DT { get; set; }
			public string NatureCategory { get; set; }
		}


	}

	public class PartyDetail
	{
		//public Data Data { get; set; }

		public class Data

		{
			public Party Party { get; set; }
		}
		public class Party
		{
			public string PartyHK { get; set; }
			public string SystemSRC_CD { get; set; }
			public string PartySRC_ID { get; set; }
			public string PartyType_CD { get; set; }
			public Person Person { get; set; }


			public Address[] Addresses { get; set; }
			public Email[] Emails { get; set; }
			public Identity[] Identities { get; set; }
			public Phone[] Phones { get; set; }

		}
		public class Person
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Gender { get; set; }
			public string Birth_DT { get; set; }
			public string BirthPlace { get; set; }
			public string Citizenship { get; set; }

			public string Death_DT { get; set; }
			public string MaritalStatus { get; set; }
		}


		public class Address
		{
			public string AddressType_CD { get; set; }
			public string StreetNM { get; set; }
			public string City { get; set; }
			public string Province { get; set; }
			public string Country_CD { get; set; }
			public string zip { get; set; }
		}

		public class Email
		{
			public string Emailtype_CD { get; set; }
			public string Address { get; set; }
		}

		public class Identity
		{
			public string IdType_CD { get; set; }
			public string IdNumber { get; set; }
		}

		public class Phone
		{
			public string PhoneType_CD { get; set; }
			public string CountryCallingCode { get; set; }
			public string AreaCode { get; set; }
			public string DialNumber { get; set; }
		}
	}



}

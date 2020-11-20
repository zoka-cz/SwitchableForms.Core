using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Zoka.SwitchableForms;

namespace SwitchableForms.Core.Tester.Models
{
	public class ContactModel
	{
		public class ContactTelephone : ISwitchableFormModel
		{
			[Required]
			[DataType(DataType.PhoneNumber)]
			[DisplayName("Please enter your phone number")]
			public string PhoneNumber { get; set; }
		}

		public class ContactEmail : ISwitchableFormModel
		{
			[Required]
			[DataType(DataType.EmailAddress)]
			[DisplayName("Please enter your email")]
			public string Email { get; set; }
		}


		public ContactModel()
		{
			Contact = new SwitchableForms<ContactTelephone, ContactEmail>(
				1, "Phone", new ContactTelephone(),
				2, "Email", new ContactEmail()
				);
		}

		[Required]
		public string Name { get; set; }
		
		public SwitchableForms<ContactTelephone, ContactEmail> Contact { get; set; }
	}
}

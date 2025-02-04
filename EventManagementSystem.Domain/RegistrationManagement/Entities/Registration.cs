using EventManagementSystem.Core.Common;
using EventManagementSystem.Core.EventManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Core.RegistrationManagement.Entities
{
	public class Registration : BaseEntity
	{
		public int EventId { get; private set; }  
		public string Name { get; private set; }
		public string Email { get; private set; }
		public string Phone { get; private set; }

	
		public Event Event { get; private set; }

		private Registration() { } // Required for EF Core

		public Registration(int eventId, string name, string email, string phone)
		{
			EventId = eventId;
			Name = name;
			Email = email;
			Phone = phone;
		}
	}

}

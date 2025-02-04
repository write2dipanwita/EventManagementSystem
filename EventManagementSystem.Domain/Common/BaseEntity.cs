using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Core.Common
{
	public abstract class BaseEntity
	{
		public int Id { get; protected set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; private set; }

		
		public void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;
	}
}

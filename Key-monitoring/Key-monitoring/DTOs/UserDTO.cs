using System;
using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs
{
	public class UserDTO
	{
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Faculty { get; set; }
        public Guid FacultyId { get; set; }
        public RoleEnum Role { get; set; }
    }
}


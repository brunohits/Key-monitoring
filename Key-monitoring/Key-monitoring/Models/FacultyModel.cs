using System;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models
{
	public class FacultyModel
	{
        [Key]
        public Guid FacultyId { get; set; }
        public string Name { get; set; }
    }
}


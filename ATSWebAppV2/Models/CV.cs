using System;
namespace ATSWebAppV2.Models
{
	public class CV
	{
		public int id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }
        public string WorkExperience { get; set; }
        public string Education { get; set; }
        public string HobbiesAndInterests { get; set; }
    }
    
}


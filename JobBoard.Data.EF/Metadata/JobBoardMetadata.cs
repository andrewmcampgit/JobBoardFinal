using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.EF
{
    [MetadataType(typeof(ApplicationMetadata))]
    public partial class Application { }

    public class ApplicationMetadata
    {
        [StringLength(128, ErrorMessage = "Must be less than 128 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "User")]
        public string UserId { get; set; }

        
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Open Position")]
        public int OpenPositionId { get; set; }

        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Application Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public System.DateTime ApplicationDate { get; set; }

        [Display(Name = "Manager Notes")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [UIHint("MultilineText")]
        public string ManagerNotes { get; set; }

        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Application Status")]
        public int ApplicationStatusId { get; set; }

        [Display(Name = "Resume")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string ResumeFilename { get; set; }

        
    }

    [MetadataType(typeof(ApplicationStatusMetadata))]
    public partial class ApplicationStatu { }
    public class ApplicationStatusMetadata
    {
        [StringLength(50, ErrorMessage = "Must be less than 50 characters")]

        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; }

        [StringLength(250, ErrorMessage = "Must be less than 250 characters")]
        [Display(Name = "Status Description")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [UIHint("MultilineText")]
        public string StatusDescription { get; set; }
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }
    public class LocationMetadata
    {
        [StringLength(10, ErrorMessage = "Must be less than 10 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Store Number")]
        public string StoreNumber { get; set; }

        [StringLength(50, ErrorMessage = "Must be less than 50 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        public string City { get; set; }

        [StringLength(2, ErrorMessage = "Must be less than 2 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        public string State { get; set; }

        [StringLength(128, ErrorMessage = "Must be less than 128 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Manager Id")]
        public string ManagerId { get; set; }


    }

    [MetadataType(typeof(OpenPositionMetadata))]
    public partial class OpenPosition { }
    public class OpenPositionMetadata
    {
        [Required(ErrorMessage = "Please insert a valid entry")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Please insert a valid entry")]
        public int PositionId { get; set; }

    }

    [MetadataType(typeof(PositionMetadata))]
    public partial class Position { }
    public class PositionMetadata
    {
        [StringLength(50, ErrorMessage = "Must be less than 50 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Position Title")]
        public string Title { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Job Description")]
        [UIHint("MultilineText")]
        public string JobDescription { get; set; }

    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {

        [Display(Name = "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
    public class UserDetailMetadata
    {
        [StringLength(50, ErrorMessage = "Must be less than 50 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Must be less than 50 characters")]
        [Required(ErrorMessage = "Please insert a valid entry")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(75, ErrorMessage = "Must be less than 75 characters")]
        [DisplayFormat(NullDisplayText ="[N/A]")]
        [Display(Name = "Resume")]
        public string ResumeFileName { get; set; }

    }



}

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

        public string UserId { get; set; }
        public int OpenPositionId { get; set; }
        public System.DateTime ApplicationDate { get; set; }
        public string ManagerNotes { get; set; }
        public int ApplicationStatusId { get; set; }
        public string ResumeFilename { get; set; }

        
    }

    [MetadataType(typeof(ApplicationStatusMetadata))]
    public partial class ApplicationStatu { }
    public class ApplicationStatusMetadata
    {
        public string StudentName { get; set; }
        public string StatusDescription { get; set; }
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }
    public class LocationMetadata
    {
        public string StoreNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ManagerId { get; set; }


    }

    [MetadataType(typeof(OpenPositionMetadata))]
    public partial class OpenPosition { }
    public class OpenPositionMetadata
    {
        public int OpenPositionId { get; set; }
        public int LocationId { get; set; }
        public int PositionId { get; set; }

    }

    [MetadataType(typeof(PositionMetadata))]
    public partial class Position { }
    public class PositionMetadata
    {
        public int PositionId { get; set; }
        public string Title { get; set; }
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
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResumeFileName { get; set; }

    }



}

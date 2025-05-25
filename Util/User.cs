using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    using DBAccessLib.Core.Attributes;
    using System;

    public class UserMaster
    {
        public int User_Id { get; set; }         
        public string User_Code { get; set; }
        [ColumnName("User_Name")]
        public string UserName { get; set; }     
        public string User_Password { get; set; }    
        public int UserRoleId { get; set; }      
        public bool IsActive { get; set; }       
        public int IsLocked { get; set; }       
        public string LastLoginIP { get; set; }  
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Remarks { get; set; }     
    }

}

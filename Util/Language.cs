using DBAccessLib.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class Language
    {
        [ColumnName("id")]
        public int Id { get; set; }
        [ColumnName("Language")]
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}

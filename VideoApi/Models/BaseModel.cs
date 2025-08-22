using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoApi.Models
{
    public class BaseModel
    {
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("xmin", TypeName = "xid")]
        public uint Version { get; set; }
        public string Description { get; set; } = string.Empty;
        public string RecordStatus { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; }        
        public DateTime ModifiedTime { get; set; }
        public DateTime DeletedTime { get; set; }
        public string CreatedBy { get; set; } = string.Empty;        
        public string ModifiedBy { get; set; } = string.Empty;        
        public string? DeletedBy { get; set; }
    }
}
namespace jcDC.Library.EFModel {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("Cache")]
    public partial class Cache {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Key { get; set; }

        [Required]
        public string KeyValue { get; set; }
    }
}
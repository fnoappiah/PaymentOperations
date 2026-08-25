using System.ComponentModel.DataAnnotations;

namespace PaymentOperations.Models
{
    public class PaymentFile
    {
        [Key]
        public int PaymentFileID { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public string Path { get; set; } = string.Empty;

        [Required]
        public string UploaderName { get; set; } = string.Empty;

        [Required]
        public string AppName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? BankName { get; set; }  

        [Required]
        public DateTime PatchDate { get; set; }

        [Required]
        public bool NCRTested { get; set; }

        [Required]
        public bool BankTested { get; set; }

        [Required]
        public bool BankProduce { get; set; }

        [Required]
        public string Creator { get; set; } = string.Empty;
              
    }
}

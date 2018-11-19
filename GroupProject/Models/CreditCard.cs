using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroupProject.Models
{
    public class CreditCard
    {

        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Plain Text Credit Card
        /// </summary>
        [NotMapped]
        [Required]
        [DataType(DataType.CreditCard)]
        //[CreditCard] -- normally you'd have this in here for validation
        [Display(Name ="Plain Text Credit Card")]
        public string PTCC { get; set; }
        /// <summary>
        /// Base 64 Encoded AES Encrypted Credit Card
        /// </summary>
        [Display(Name ="Encrypted Credit Card")]
        public string ECC { get; set; }
        /// <summary>
        /// Base64 Encoded HMAC signature 
        /// </summary>
        [Display(Name = "Signed And Encrypted Credit Card")]
        public string SECC { get; set; }
        [Required]
        public string CvcCode { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

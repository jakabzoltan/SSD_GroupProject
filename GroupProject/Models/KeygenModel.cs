using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupProject.Models
{
    public class KeygenModel
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="KeygenModel"/> class.
		/// </summary>
		public KeygenModel()
        {

        }

        /// <summary>
        /// Gets or sets the HMAC key.
        /// </summary>
        /// <value>The HMAC key.</value>
        [Display(Name = "HMAC Key")]
        public string HmacKey { get; set; }

        /// <summary>
        /// Gets or sets the AES key.
        /// </summary>
        /// <value>The AES key.</value>
        [Display(Name = "AES Key")]
        public string AesKey { get; set; }

        /// <summary>
        /// Gets or sets the AES IV.
        /// </summary>
        /// <value>The AES IV.</value>
        [Display(Name = "AES IV")]
        public string AesIv { get; set; }

        /// <summary>
        /// Gets or sets the asymmetric private key.
        /// </summary>
        /// <value>The asymmetric private key.</value>
        [Display(Name = "Asymmetric Private Key")]
        public string AsymmetricPrivateKey { get; set; }
    }
}

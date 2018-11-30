using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupProject.Models
{
    public class SendEmailViewModel
    {
        public string Email { get; set; }

        public string Subject { get; set; }
        public string Text { get; set; }
    }
}

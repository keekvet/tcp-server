using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace tcp_server
{
    class Message
    {
        public int Id { get; set; }

        [Required]
        public User Sender { get; set; }

        [Required]
        public User Receiver { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}

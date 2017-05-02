using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebRole1.Models
{
    
    [DataContract]
    public class Order
    {
        [DataMember]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

    }
}
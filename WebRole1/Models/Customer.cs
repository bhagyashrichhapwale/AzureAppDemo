using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebRole1.Models
{
    
    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [DataMember]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [DataMember]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataMember]
        public ICollection<Order> Orders { get; set; }

    }
}
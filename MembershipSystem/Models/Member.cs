using System;
using System.ComponentModel.DataAnnotations;

namespace MembershipSystem.Models
{
    public class Member
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Phone]
        public string MobilePhoneNumber { get; set; }

        [RegularExpression(@"^(\d{4})$")]
        public string PinCode { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }

    }
}

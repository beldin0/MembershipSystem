using MembershipSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Authentication
{
    public static class Security
    {
        public static bool Login (string username, string password)
        {
            using (MembersContext mc = MembersController.GetContext())
            {
                return mc.Members.Any(user => user.Name == username && user.PinCode == password);
            }

        }
    }
}

using MembershipSystem.Controllers;
using MembershipSystem.Models;
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
                Member m = mc.Members.Find(username);
                return m == null ? false: m.PinCode == password ;
            }

        }
    }
}

using Microsoft.EntityFrameworkCore;
using OnbordingPlatform.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnbordingPlatform.Domain
{
    internal class Auth
    {
        private readonly VlasovaAaКурсовая1Context _context;
        public Auth() 
        { 
            _context = new VlasovaAaКурсовая1Context();
        }
        public async Task<Account> AuthAsync(string username,  string password)
        {
            var account = await _context.Accounts
                .Include(a => a.RoleIdFkNavigation)
                .Include(a => a.AccountStatusFkNavigation)
                .FirstOrDefaultAsync(a => a.Username == username);

            if (account == null) return null;

            if (account.Password == password && account.AccountStatusFkNavigation.StatusName == "Active")
            {

                return account;
            }


            return null;
        }
    }
}

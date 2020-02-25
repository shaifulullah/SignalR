using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IUser
    {
        void Add(User _user);
        void Remove();

        User GetUserFromId(int UserId);
        User GetUserFromEmail(string UserEmail);
    }
}

using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IUserRole
    {
        IEnumerable<UserRole> GetUserRoles { get; }
        UserRole GetUserRole(int? userRoleId);
        string Add(UserRole _userRole);

        void Update(int userId, int userRole, int typeId);

        ICollection<UserRole> FilterUserRoles(int? requestType, int? userId, int? userRole);

        IEnumerable<User> GetUsers { get; }
        User GetUserByUserRoleId(int UserId);

        IQueryable<UserRole> GetApproversWithType(int RequestTypeId, int CreatorId);
        IQueryable<UserRole> GetValidatorsWithType(int RequestTypeId, int CreatorId);
        IQueryable<UserRole> GetAllValidators();
        IQueryable<UserRole> GetAllApprovers();


    }
}

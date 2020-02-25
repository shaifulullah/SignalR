using Chnage.Models;
using Chnage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class UserRoleRepository : IUserRole
    {
        private readonly MyECODBContext db;

        public UserRoleRepository(MyECODBContext _db)
        {
            db = _db;
        }

        public UserRoleRepository()
        {
        }

        public IEnumerable<UserRole> GetUserRoles => db.UserRoles;
        public IEnumerable<User> GetUsers => db.Users;
        public IEnumerable<RequestType> GetRequests => db.RequestTypes;

        public UserRole GetUserRole(int? Id)
        {
            return db.UserRoles.Where(x => x.Id == Id).FirstOrDefault();
        }
        public string Add(UserRole _UserRole)
        {
            //bool userExists = db.Users.Select(u => u.Id == _UserRole.UserId).Distinct().Single();
            //bool typeExists = db.RequestTypes.Select(r => r.Id == _UserRole.RequestTypeId).Single();
            //if(userExists && typeExists)
            //{
            UserRole userRoleExists = GetUserRoles.Where(ur => ur.UserId == _UserRole.UserId && ur.RequestTypeId == _UserRole.RequestTypeId && ur.RoleInt == _UserRole.RoleInt).SingleOrDefault();
            if (userRoleExists != null)
            {
                return "User Role already exists";
            }
            else
            {
                db.UserRoles.Add(_UserRole);
                //db.SaveChanges();
                return "User Role created.";
            }
            //}
            //return "User or Request Type does not exist.";
        }

        public ICollection<UserRole> FilterUserRoles(int? requestType, int? userId, int? userRole)
        {
            throw new NotImplementedException();
            //work here
        }

        public void Update(int userId, int userRole, int typeId)
        {
            //UserRole userR = (from p in db.UserRoles where p.User.Id == userId select p).SingleOrDefault();
            //userR.RoleInt = typeId;
            //userR.RequestType.Id = typeId;
        }

        public IQueryable<UserRole> GetValidatorsWithType(int RequestTypeId, int UserId)
        {
            return db.UserRoles.Where(u => u.RoleInt == Role.Validator && u.RequestTypeId == RequestTypeId && u.UserId != UserId);
        }

        public IQueryable<UserRole> GetApproversWithType(int RequestTypeId, int UserId)
        {
            return db.UserRoles.Where(u => u.RoleInt == Role.Approver && u.RequestTypeId == RequestTypeId && u.User.isActive && u.UserId != UserId);
        }

        public IQueryable<UserRole> GetAllValidators()
        {
            return db.UserRoles.Where(u => u.RoleInt == Role.Validator && u.User.isActive == true).Distinct();
        }

        public IQueryable<UserRole> GetAllApprovers()
        {
            return db.UserRoles.Where(u => u.RoleInt == Role.Approver && u.User.isActive == true).Distinct();
        }

        public User GetUserByUserRoleId(int ValidatorId)
        {
            var role = db.UserRoles.Where(u => u.Id == ValidatorId).Single();
            var user = db.Users.Where(u => u.Id == role.UserId).Single();
            return user;
        }
    }
}

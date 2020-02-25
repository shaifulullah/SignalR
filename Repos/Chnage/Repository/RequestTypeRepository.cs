using Chnage.Models;
using Chnage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class RequestTypeRepository : IRequestType
    {
        private readonly MyECODBContext db;

        public RequestTypeRepository(MyECODBContext _db)
        {
            db = _db;
        }
        public IEnumerable<RequestType> GetRequestType => throw new NotImplementedException();

        public void Add(RequestType _RequestType)
        {
            throw new NotImplementedException();
        }

        public bool HasApprovers(int id)
        {
            var roles = db.UserRoles.Select(ur => ur.RequestTypeId == id && ur.RoleInt == Role.Approver);
            return roles.Contains(true) ? true : false;
        }

        public bool HasValidators(int RequestId, int UserId)
        {
            var roles = db.UserRoles.Select(ur => ur.RequestTypeId == RequestId && ur.RoleInt == Role.Validator && ur.UserId != UserId && ur.User.isActive == true);
            return roles.Contains(true) ? true : false;
        }
    }
}

using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IRequestType
    {
        IEnumerable<RequestType> GetRequestType { get; }
        void Add(RequestType _RequestType);

        bool HasApprovers(int id);
        bool HasValidators(int Requestid, int UserId);
    }
}

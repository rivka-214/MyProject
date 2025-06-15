using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IVolunteersCallLogic
    {
        Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY);
    }

}

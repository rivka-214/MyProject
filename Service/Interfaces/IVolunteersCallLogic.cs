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
        List<VolunteerCallsDto> GetActiveCallsForVolunteer(int volunteerId);
        List<VolunteerCallsDto> GetHistoryCallsForVolunteer(int volunteerId);
        void RespondToCall(int callId, int volunteerId, string response);
        void UpdateVolunteerStatus(int callId, int volunteerId, string status, string summary = null);
        bool ShouldSendToMoreVolunteers(int callId);
        string GetCallVolunteersInfo(int callId);
    }

}

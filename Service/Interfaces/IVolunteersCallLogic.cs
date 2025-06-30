using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IVolunteersCallLogic
    {
        Task<List<VolunteerCallsDto>> GetActiveCallsForVolunteer(int volunteerId);
        Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId);
        Task RespondToCall(int callId, int volunteerId, string response);
        Task UpdateVolunteerStatus(int callId, int volunteerId, string status, string summary = null);
        Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY);
        Task<bool> ShouldSendToMoreVolunteers(int callId);
        Task<string> GetCallVolunteersInfo(int callId);
    }
}
using Common.Dto;
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
        Task<List<VolunteerCallsDto>> GetnotifiedCallsForVolunteer(int volunteerId);
        Task<List<VolunteerCallsDto>> GetHistoryCallsForVolunteer(int volunteerId);
        Task CompleteCallAsync(int callId, int volunteerId, int currentVolunteerId, CompleteCallDto dto);

        Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY);

        // תיקון החתימה - הסרת הטעויות הכתיב והוספת הפרמטר summary
        Task UpdateVolunteerStatus(int callId, int volunteerId, string status, int currentVolunteerIdy);
        Task<bool> ShouldSendToMoreVolunteers(int callId);
        Task<CallVolunteersInfoDto> GetCallVolunteersInfo(int callId);

        Task<string> GetVolunteerStatus(int callId, int volunteerId);
        Task<int> GetGoingVolunteersCount(int callId);
        Task<bool> HasArrivedVolunteer(int callId);
        Task<VolunteerCallsDto> GetVolunteerCall(int callId, int volunteerId);

        public Task CheckAndReassignVolunteers();

        /// <summary>
        /// מחזיר עד 20 מתנדבים שהוקצו לקריאה מסוימת
        /// </summary>
        Task<List<VolunteersDto>> GetTop20VolunteersForCall(int callId);

        /// <summary>
        /// מחזיר את כל הקריאות שהוקצו למתנדב
        /// </summary>
        Task<List<CallsDto>> GetAllCallsForVolunteer(int volunteerId);

        /// <summary>
        /// מחזיר את כל הקריאות שהוקצו למתנדב לפי סטטוס
        /// </summary>
        Task<List<CallsDto>> GetCallsForVolunteerByStatus(int volunteerId, string status);
    }
}
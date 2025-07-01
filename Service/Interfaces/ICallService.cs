using Common.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICallService:IService<CallsDto>
    public interface ICallService
    {
        Task<string> GetStatus(int callId);
       
     
        Task UpdateStatus(int id, string status);

        Task CompleteCall(int id, CompleteCallDto dto, int volunteerId);
        Task<string> GetCallStatusWithVolunteersInfo(int id);
        Task<CallsDto> AddCallAsync(CallsDto call, IFormFile file); // לטיפול בהוספת קריאה עם תמונה
        Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY); // הוספה

        Task<CallsDto> CreateCallAsync(CallsDto call);
        Task<List<CallsDto>> GetCallsByUserId(int userId);
    }
}
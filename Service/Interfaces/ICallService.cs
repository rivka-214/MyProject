using Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICallService
    {
        string GetStatus(int callId);
        void CompleteCall(int id, CompleteCallDto dto);
        void UpdateStatus(int id, string status);
    }

}

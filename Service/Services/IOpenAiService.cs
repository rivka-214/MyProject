using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    internal interface IOpenAiService
    {
        Task<string> GetFirstAidInstructionsAsync(string description);
    }
}

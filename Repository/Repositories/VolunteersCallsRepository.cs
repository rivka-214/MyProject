using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class VolunteersCallsRepository : IRepository<VolunteerCalls>
    {
        private readonly IContext context;

        public VolunteersCallsRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<VolunteerCalls> AddItem(VolunteerCalls item)
        {
            await this.context.VolunteerCallsDb.AddAsync(item);
            await this.context.SaveAsync();
            return item;
        }
        

        public async Task DeleteItem(int id)
        {
            var volunteerCall = await GetById(id);
            this.context.VolunteerCallsDb.Remove(volunteerCall);
            await this.context.SaveAsync();
        }

       
        public async Task<List<VolunteerCalls>> GetAll()
        {
            return await this.context.VolunteerCallsDb.ToListAsync();
        }

        public async Task<VolunteerCalls> GetById(int id)
        {
            return await context.VolunteerCallsDb.FirstOrDefaultAsync(x => x.Id == id);
        }
     


        public async Task UpdateItem(int id, VolunteerCalls item)
        {
            var volunteerCall = await GetById(id);
            if (volunteerCall != null)
            {
                volunteerCall.VolunteerStatus = item.VolunteerStatus;
                volunteerCall.ResponseTime = item.ResponseTime;
                await context.SaveAsync();
            }
        }

      

      

        /// <summary>
        /// מחזיר קריאות פעילות למתנדב ספציפי (going, arrived)
        /// </summary>

        public async Task<List<VolunteerCalls>> GetActiveCallsForVolunteer(int volunteerId)
        {
            return await context.VolunteerCallsDb
                .Where(vc => vc.VolunteerId == volunteerId && vc.VolunteerStatus != "cant" && vc.VolunteerStatus != "finished")
                .Include(vc => vc.Calls)
                .ToListAsync();
        }

        /// <summary>
        /// מחזיר היסטוריית קריאות למתנדב (finished, cant)
        /// </summary>
        public async Task<List<VolunteerCalls>> GetHistoryCallsForVolunteer(int volunteerId)
        {
            return await context.VolunteerCallsDb
                .Where(vc => vc.VolunteerId == volunteerId && vc.VolunteerStatus == "finished")
                .Include(vc => vc.Calls)
                .ToListAsync();
        }
        /// <summary>
        /// מחזיר קריאה ספציפית של מתנדב
        /// </summary>


        /// <summary>
        /// מעדכן סטטוס מתנדב לקריאה ספציפית
        /// </summary>
     
        public async Task UpdateVolunteerStatus(int callId, int volunteerId, string status)
        {
            var call = await GetVolunteerCall(callId, volunteerId);
            if (call != null)
            {
                call.VolunteerStatus = status;
                call.ResponseTime = DateTime.UtcNow;
                await context.SaveAsync();
            }
        }

        /// <summary>
        /// מחזיר כמה מתנדבים יצאו לקריאה ספציפית
        /// </summary>
        public async Task<int> GetGoingVolunteersCount(int callId)
        {
            return await context.VolunteerCallsDb
                .CountAsync(vc => vc.CallsId == callId && vc.VolunteerStatus == "going");
        }


        /// <summary>
        /// בדיקה אם יש מתנדב שהגיע לקריאה
        /// </summary>
        public async Task<bool> HasArrivedVolunteer(int callId)
        {
            return await context.VolunteerCallsDb
                .AnyAsync(vc => vc.CallsId == callId && vc.VolunteerStatus == "arrived");
        }

        public async Task<VolunteerCalls> GetVolunteerCall(int callId, int volunteerId)
        {
            return await context.VolunteerCallsDb
                .FirstOrDefaultAsync(vc => vc.CallsId == callId && vc.VolunteerId == volunteerId);
        }

       
    }
}
    


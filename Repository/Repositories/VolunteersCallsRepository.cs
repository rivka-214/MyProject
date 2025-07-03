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
        /// מעדכן סטטוס מתנדב לקריאה ספציפית
        /// </summary>

        //public async Task UpdateVolunteerStatus(int callId, int volunteerId, string status, string summary )
        //{
        //    Console.WriteLine($"[Repo] callId={callId}, volunteerId={volunteerId}, status={status}, summary={summary}");

        //    if (string.IsNullOrEmpty(status))
        //        throw new ArgumentException("status cannot be null or empty", nameof(status));

        //    var call = await GetVolunteerCall(callId, volunteerId);
        //    if (call != null)
        //    {
        //        call.VolunteerStatus = status;
        //        call.ResponseTime = DateTime.UtcNow;

        //        // 💡 הוספת עדכון summary אם רלוונטי
        //        //if (!string.IsNullOrEmpty(summary))
        //        //{
        //        //    call.Summary = summary;
        //        //}

        //        await context.SaveAsync();
        //    }
        //}


        public async Task UpdateVolunteerStatus(int callId, int volunteerId, string status )
        {
            if (string.IsNullOrEmpty(status))
                throw new ArgumentException("status cannot be null or empty", nameof(status));

            var call = await GetVolunteerCall(callId, volunteerId);
            if (call != null)
            {
                call.VolunteerStatus = status;
                call.ResponseTime = DateTime.UtcNow;

               
             
                

                await context.SaveAsync();
            }
        }


        public async Task UpdateVolunteerFinish(int callId, int volunteerId, string summary)
        {
            Console.WriteLine($"[Repo] UpdateVolunteerFinish - callId={callId}, volunteerId={volunteerId}, summary={summary}");

            if (string.IsNullOrEmpty(summary))
                throw new ArgumentException("summary is required to finish the call", nameof(summary));

            var call = await GetVolunteerCall(callId, volunteerId);
            if (call == null)
                throw new Exception("המתנדב לא משויך לקריאה זו");

            call.VolunteerStatus = "finished";
            call.ResponseTime = DateTime.UtcNow;

            var mainCall = await context.CallsDb.FirstOrDefaultAsync(c => c.Id == callId);
            if (mainCall == null)
                throw new Exception("קריאה לא נמצאה");

            mainCall.Status = "Closed";
            mainCall.Summary = summary;

            await context.SaveAsync();
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
        /// <summary>
        /// מחזיר קריאה ספציפית של מתנדב
        /// </summary>


        public async Task<VolunteerCalls> GetVolunteerCall(int callId, int volunteerId)
        {
            return await context.VolunteerCallsDb
                .FirstOrDefaultAsync(vc => vc.CallsId == callId && vc.VolunteerId == volunteerId);
        }


    }
}



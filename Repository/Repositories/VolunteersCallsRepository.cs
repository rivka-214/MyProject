using Microsoft.EntityFrameworkCore;
using Reposetory.Entities;
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

        public VolunteerCalls AddItem(VolunteerCalls item)
        {
            this.context.VolunteerCallsDb.Add(item);
            this.context.Save();
            return item;
        }

        public void DeleteItem(int id)
        {
            this.context.VolunteerCallsDb.Remove(GetById(id));
            this.context.Save();
        }

        public List<VolunteerCalls> GetAll()
        {
            return this.context.VolunteerCallsDb.ToList();
        }

        public VolunteerCalls GetById(int id)
        {
            return context.VolunteerCallsDb.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateItem(int id, VolunteerCalls item)
        {
            var volunteerCall = GetById(id);
            if (volunteerCall != null)
            {
                volunteerCall.VolunteerStatus = item.VolunteerStatus;
                volunteerCall.ResponseTime = item.ResponseTime;
                context.Save();
            }
        }

       
        /// <summary>
        /// מחזיר קריאות פעילות למתנדב ספציפי (going, arrived)
        /// </summary>
        public List<VolunteerCalls> GetActiveCallsForVolunteer(int volunteerId)
        {
            return context.VolunteerCallsDb
                .Include(vc => vc.Calls) // ✅ תוקן מ-Call ל-Calls
                .Where(vc => vc.VolunteerId == volunteerId &&
                           (vc.VolunteerStatus == "going" || vc.VolunteerStatus == "arrived"))
                .ToList();
        }

        /// <summary>
        /// מחזיר היסטוריית קריאות למתנדב (finished, cant)
        /// </summary>
        public List<VolunteerCalls> GetHistoryCallsForVolunteer(int volunteerId)
        {
            return context.VolunteerCallsDb
                .Include(vc => vc.Calls) // ✅ תוקן מ-Call ל-Calls
                .Where(vc => vc.VolunteerId == volunteerId &&
                           (vc.VolunteerStatus == "finished" || vc.VolunteerStatus == "cant"))
                .ToList();
        }

        /// <summary>
        /// מחזיר קריאה ספציפית של מתנדב
        /// </summary>
        public VolunteerCalls GetByCallAndVolunteer(int callId, int volunteerId)
        {
            return context.VolunteerCallsDb
                .FirstOrDefault(vc => vc.CallsId == callId && vc.VolunteerId == volunteerId);
        }

        /// <summary>
        /// מעדכן סטטוס מתנדב לקריאה ספציפית
        /// </summary>
        public void UpdateVolunteerStatus(int callId, int volunteerId, string status)
        {
            var volunteerCall = GetByCallAndVolunteer(callId, volunteerId);
            if (volunteerCall != null)
            {
                volunteerCall.VolunteerStatus = status;
                volunteerCall.ResponseTime = DateTime.Now;
                context.Save();
            }
        }

        /// <summary>
        /// מחזיר כמה מתנדבים יצאו לקריאה ספציפית
        /// </summary>
        public int GetGoingVolunteersCount(int callId)
        {
            return context.VolunteerCallsDb
                .Count(vc => vc.CallsId == callId && vc.VolunteerStatus == "going");
        }

        /// <summary>
        /// בדיקה אם יש מתנדב שהגיע לקריאה
        /// </summary>
        public bool HasArrivedVolunteer(int callId)
        {
            return context.VolunteerCallsDb
                .Any(vc => vc.CallsId == callId && vc.VolunteerStatus == "arrived");
        }
    }
}

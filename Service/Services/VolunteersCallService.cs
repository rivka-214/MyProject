using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Interfacese;
using Repository.Repositories;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Service.Services
    {
        public class VolunteersCallService : IService<VolunteerCallsDto>, IVolunteersCallLogic
        {
            private readonly IRepository<VolunteerCalls> repository;
            private readonly IMapper mapper;
            private readonly IVolunteerLogic volunteerLogic;
            private readonly ICallService callService;

            public VolunteersCallService(
                IRepository<VolunteerCalls> repository,
                IMapper mapper,
                IVolunteerLogic volunteerLogic,
                ICallService callService)
            {
                this.repository = repository;
                this.mapper = mapper;
                this.volunteerLogic = volunteerLogic;
                this.callService = callService;
            }

            public VolunteerCallsDto AddItem(VolunteerCallsDto item)
            {
                var entity = mapper.Map<VolunteerCalls>(item);
                var added = repository.AddItem(entity);
                return mapper.Map<VolunteerCallsDto>(added);
            }

            public void DeleteItem(int id)
            {
                repository.DeleteItem(id);
            }

            public List<VolunteerCallsDto> GetAll()
            {
                var list = repository.GetAll();
                return mapper.Map<List<VolunteerCallsDto>>(list);
            }

            public VolunteerCallsDto GetById(int id)
            {
                var item = repository.GetById(id);
                return mapper.Map<VolunteerCallsDto>(item);
            }

            public void UpdateItem(int id, VolunteerCallsDto item)
            {
                var entity = mapper.Map<VolunteerCalls>(item);
                repository.UpdateItem(id, entity);
            }

            public async Task AssignNearbyVolunteersToCall(int callId, double locationX, double locationY)
            {
                var nearbyVolunteers = volunteerLogic.GetNearbyVolunteers(locationX, locationY);
                foreach (var volunteer in nearbyVolunteers)
                {
                    var newItem = new VolunteerCallsDto
                    {
                        CallsId = callId,
                        VolunteerId = volunteer.Id,
                        VolunteerStatus = "notified" // סטטוס ברירת מחדל
                    };
                    var entity = mapper.Map<VolunteerCalls>(newItem);
                    repository.AddItem(entity);
                }
            }

            // 🆕 פונקציות חדשות לתמיכה במערכת הסטטוסים

            /// <summary>
            /// מחזיר קריאות פעילות למתנדב
            /// </summary>
            public List<VolunteerCallsDto> GetActiveCallsForVolunteer(int volunteerId)
            {
                var repo = repository as VolunteersCallsRepository;
                var activeCalls = repo?.GetActiveCallsForVolunteer(volunteerId) ?? new List<VolunteerCalls>();
                return mapper.Map<List<VolunteerCallsDto>>(activeCalls);
            }

            /// <summary>
            /// מחזיר היסטוריית קריאות למתנדב
            /// </summary>
            public List<VolunteerCallsDto> GetHistoryCallsForVolunteer(int volunteerId)
            {
                var repo = repository as VolunteersCallsRepository;
                var historyCalls = repo?.GetHistoryCallsForVolunteer(volunteerId) ?? new List<VolunteerCalls>();
                return mapper.Map<List<VolunteerCallsDto>>(historyCalls);
            }

            /// <summary>
            /// מתנדב מגיב לקריאה (going/cant)
            /// </summary>
            public void RespondToCall(int callId, int volunteerId, string response)
            {
                var repo = repository as VolunteersCallsRepository;
                repo?.UpdateVolunteerStatus(callId, volunteerId, response);

                // אם מתנדב יצא - בדוק אם צריך לעדכן סטטוס קריאה
                if (response == "going")
                {
                    // הקריאה נשארת "open" עד שמישהו מגיע
                }
                else if (response == "arrived")
                {
                    // עדכן סטטוס קריאה ל"in_progress"
                    callService.UpdateStatus(callId, "in_progress");
                }
            }

            /// <summary>
            /// עדכון סטטוס מתנדב לקריאה
            /// </summary>
            public void UpdateVolunteerStatus(int callId, int volunteerId, string status, string summary = null)
            {
                var repo = repository as VolunteersCallsRepository;
                repo?.UpdateVolunteerStatus(callId, volunteerId, status);

                // לוגיקה לעדכון סטטוס קריאה
                if (status == "arrived")
                {
                    callService.UpdateStatus(callId, "in_progress");
                }
                else if (status == "finished")
                {
                    // אם יש סיכום - עדכן גם את הקריאה
                    if (!string.IsNullOrEmpty(summary))
                    {
                        var completeDto = new CompleteCallDto
                        {
                            Summary = summary,
                            SentToHospital = false // ברירת מחדל
                        };
                        callService.CompleteCall(callId, completeDto);
                    }
                    callService.UpdateStatus(callId, "closed");
                }
            }

            /// <summary>
            /// בדיקה אם צריך לשלוח לעוד מתנדבים
            /// </summary>
            public bool ShouldSendToMoreVolunteers(int callId)
            {
                var repo = repository as VolunteersCallsRepository;
                var goingCount = repo?.GetGoingVolunteersCount(callId) ?? 0;
                return goingCount < 3; // אם פחות מ-3 מתנדבים יצאו
            }

            /// <summary>
            /// קבלת מידע על מתנדבים שיצאו לקריאה
            /// </summary>
            public string GetCallVolunteersInfo(int callId)
            {
                var repo = repository as VolunteersCallsRepository;
                var goingCount = repo?.GetGoingVolunteersCount(callId) ?? 0;
                var hasArrived = repo?.HasArrivedVolunteer(callId) ?? false;

                if (hasArrived)
                    return "מתנדב הגיע למקום - בטיפול";
                else if (goingCount > 0)
                    return $"{goingCount} מתנדבים יצאו לקריאה";
                else
                    return "ממתין למתנדבים";
            }
        }
    }


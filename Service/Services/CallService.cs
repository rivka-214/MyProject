using AutoMapper;
using Common.Dto;
using Microsoft.AspNetCore.Http;
using Reposetory.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CallService : IService<CallsDto>, ICallService
    {
        private readonly ICallsRepository repository;  // שינוי כאן
        private readonly IMapper mapper;
        private readonly Func<IVolunteersCallLogic> logicFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private IVolunteersCallLogic Logic => logicFactory();

        public CallService(ICallsRepository repository, IMapper mapper, Func<IVolunteersCallLogic> logicFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logicFactory = logicFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CallsDto> AddItemAsync(CallsDto item)
        {
            var entity = mapper.Map<Calls>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<CallsDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<CallsDto>> GetAllAsync()
        {
            var list = await repository.GetAll();
            return mapper.Map<List<CallsDto>>(list);
        }

        public async Task<CallsDto> GetByIdAsync(int id)
        {
            var entity = await repository.GetById(id);
            return mapper.Map<CallsDto>(entity);
        }

        public async Task UpdateItemAsync(int id, CallsDto item)
        {
            var entity = mapper.Map<Calls>(item);
            await repository.UpdateItem(id, entity);
        }

        public async Task<string> GetStatus(int id)
        {
            var call = await repository.GetById(id);
            return call?.Status;
        }

        public async Task UpdateStatus(int id, string status)
        {
            var call = await repository.GetById(id);
            if (call != null)
            {
                call.Status = status;
                await repository.UpdateItem(id, call);
            }
        }

        public async Task CompleteCall(int id, CompleteCallDto dto)
        {
            var call = await repository.GetById(id);
            if (call == null)
                throw new Exception("קריאה לא קיימת");

            call.Summary = dto.Summary;
            call.SentToHospital = dto.SentToHospital;
            call.HospitalName = dto.SentToHospital ? dto.HospitalName : null;
            await repository.UpdateItem(id, call);
        }
        public async Task<List<CallsDto>> GetCallsByUserId(int userId)
        {
            var userCalls = await repository.GetCallsByUserId(userId);
            Console.WriteLine($"User calls count: {userCalls.Count}");
            return mapper.Map<List<CallsDto>>(userCalls);
        }
        public async Task<CallsDto> CreateCallAsync(CallsDto call)
        {
            // העלאת תמונה אם קיימת
            if (call.FileImage != null)
            {
                var filePath = await UploadImage(call.FileImage);
                Console.WriteLine($"📷 Image uploaded to: {filePath}");
            }

            // שליפת מזהה משתמש מתוך JWT
            var httpContext = _httpContextAccessor.HttpContext;
            var userIdStr = httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdStr, out int userId))
            {
                Console.WriteLine("❌ לא הצלחנו לשלוף מזהה משתמש מה־JWT");
                throw new UnauthorizedAccessException("מזהה משתמש לא תקין ב־JWT");
            }

            Console.WriteLine($"✅ User ID from token: {userId}");
            call.UserId = userId;

            // מצב ברירת מחדל
            call.Status = "נפתחה";

            // שמירת הקריאה בבסיס הנתונים
            var savedCall = await AddItemAsync(call);
            Console.WriteLine($"📦 Call saved to DB with ID: {savedCall.Id}");

            // הקצאת מתנדבים אם יש מיקום
            if (call.LocationX != 0 && call.LocationY != 0)
            {
                Console.WriteLine($"📍 Assigning volunteers near location: {call.LocationX}, {call.LocationY}");
                await Logic.AssignNearbyVolunteersToCall(savedCall.Id, call.LocationX, call.LocationY);
            }

            return savedCall;
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            var folderPath = Path.Combine(Environment.CurrentDirectory, "Images");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}

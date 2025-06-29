using AutoMapper;
using Common.Dto;
using Reposetory.Entities;
using Repository.Entities;
using Repository.Interfacese;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IService<UserDto>
    {
        private readonly IRepository<User> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserDto> AddItemAsync(UserDto item)
        {
            var entity = mapper.Map<User>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<UserDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var list = await repository.GetAll();
            return mapper.Map<List<UserDto>>(list);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var entity = await repository.GetById(id);
            return mapper.Map<UserDto>(entity);
        }

        public async Task UpdateItemAsync(int id, UserDto item)
        {
            var entity = mapper.Map<User>(item);
            await repository.UpdateItem(id, entity);
        }
    }
}

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
        public UserDto AddItem(UserDto item)
        {
            return mapper.Map<User, UserDto>(repository.AddItem(mapper.Map<UserDto, User>(item)));
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<UserDto> GetAll()
        {
            return mapper.Map<List<User>, List<UserDto>>(repository.GetAll());
        }

        public UserDto GetById(int id)
        {
            return mapper.Map<User, UserDto>(repository.GetById(id));
        }

        public void UpdateItem(int id, UserDto item)
        {
            repository.UpdateItem(id, mapper.Map<UserDto, User>(item));
        }


    }
}

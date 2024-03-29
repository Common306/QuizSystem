﻿using Microsoft.AspNetCore.Authorization;
using QuizSystemApi.Dao;
using QuizSystemApi.Dto.Request;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class UserRepository : IUserRepository
    {
        public User? Login(LoginDtoRequest request)
        {
            User userlogin = new User
            {
                Username = request.Username,
                Password = request.Password
            };
            return UserDao.Login(userlogin);
        }
        public User? Register(RegisterDtoRequest request)
        {
            User userRegister = new User
            {
                Username = request.Username,
                Password = request.Password,
                RoleId = request.RoleId,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                CreateAt = DateTime.Now,
                IsEnable = true 
            };
            Role role = new Role();
            if(request.RoleId == 2)
            {
                role = new Role
                {
                    RoleId = request.RoleId,
                    RoleName = "Teacher"
                };
            } else
            {
                role = new Role
                {
                    RoleId = request.RoleId,
                    RoleName = "Student"
                };
            }
            User? user = UserDao.Register(userRegister);
            if(user == null)
            {
                return null;
            }
            userRegister.Role = role;
            return userRegister;
        }
        public List<User> GetAll(string? search, int? page)
        {
            return UserDao.GetAll(search, page);
        }
        public User Create(User user)
        {
            return UserDao.Create(user);
        }
        public User Update(int id, User user)
        {
            return UserDao.Update(id, user);
        }
        public bool Delete(int id)
        {
            return UserDao.Delete(id);
        }
        public User Get(int id)
        {
            return UserDao.Get(id);
        }

        public int Total(string? search)
        {
            return UserDao.Total(search);
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.ApplicationServices.Interfaces;
using TM.Data.Entities;
using TM.Data.Entities.Enums;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using TM.Repositories.Interfaces;

namespace TM.ApplicationServices.Implementations
{
    public class UsersManagementService : BaseManagementService, IUsersManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersManagementService(ILogger<UsersManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            ////Oste validacii

            //if (request.User.FirstName.Length > 20)
            //{
            //    _logger.LogWarning("Firstname '{title}' length must be less than 20!", request.User.FirstName);
            //}

            _unitOfWork.Users.Insert(new()
            {
                Id = request.User.Id,
                Username = request.User.Username,
                Password = request.User.Password,
                FirstName = request.User.FirstName,
                LastName = request.User.LastName,
                Gender = request.User.Gender,
                Email = request.User.Email,
                DateOfBirth = request.User.DateOfBirth,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = 1,
                IsActivated = true
            });

            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

            if (user == null)
            {
                _logger.LogError("User with identifier {user.Id} not found", request.Id);
                throw new Exception("");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            GetUserResponse response = new() { Users = new() };

            var users = await _unitOfWork.Users.GetAllAsync(request.IsActive);

            foreach (var user in users)
            {
                response.Users.Add(new()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    IsAdmin = user.IsAdmin
                });
            }

            return response;
        }

        public async Task<UserViewModel> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("");
            }
            UserViewModel result = new UserViewModel()
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Username = user.Username,
                Password = user.Password,
                Gender = user.Gender,
                IsAdmin = user.IsAdmin
            };
            return result;
        }


        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            User user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError("User with identifier {request.UserId} not found", request.UserId);
                throw new Exception("");
            }

            user.Id = request.UserId;
            user.Username = request.User.Username;
            user.Password = request.User.Password;
            user.FirstName = request.User.FirstName;
            user.LastName = request.User.LastName;
            user.Gender = request.User.Gender;
            user.Email = request.User.Email;
            user.DateOfBirth = request.User.DateOfBirth;

            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync();
            return new();
        }


        public async Task<GetUserResponse> SearchUserByUsername(string name)
        {
            GetUserResponse response = new() { Users = new() };

            var us = _unitOfWork.Users.GetAllAsync().Result.Where(x => x.Username == name).First();
            var user = await _unitOfWork.Users.GetByIdAsync(us.Id);

            if (user == null)
            {
                throw new Exception("");
            }
            UserViewModel result = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Username = user.Username,
                Password = user.Password,
                Gender = user.Gender,
                IsAdmin = user.IsAdmin
            };

            response.Users.Add(result);

            return response;
        }
    }
}

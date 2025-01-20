using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.AppUser;
using acadgest.Models.User;

namespace acadgest.Mappers
{
    public static class AppUserMaps
    {
        public static AppUserDto ToUserDto(this AppUser user)
        {
            return new AppUserDto
            {
                Id = user.Id,
                Name = user.Name,
                IdNumber = user.IdNumber,
                Username = user?.UserName ?? "",
                Email = user?.Email ?? ""
            };
        }
    }
}
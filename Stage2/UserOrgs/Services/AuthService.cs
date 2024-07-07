using Microsoft.EntityFrameworkCore;
using UserOrgs.Data;
using UserOrgs.Dto;

namespace UserOrgs.Services
{
    public class AuthService(AppDbContext dbContext,PasswordService passwordService)
    {
        private readonly AppDbContext _dc = dbContext;
        private readonly PasswordService _passwordService = passwordService;

        public async Task<User?> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var existingUser = _dc.Users.AsNoTracking().FirstOrDefault(x => x.email == userRegisterDto.email);
            if (existingUser is not null)
                return null;

            User user = userRegisterDto.ToModel();
            user.userId = Guid.NewGuid().ToString();
            (user.passwordSalt, user.password) = _passwordService.GenerateSaltAndHash(userRegisterDto.password);
            await _dc.Users.AddAsync(user);
            await _dc.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginUser(UserLoginDto userLoginDto)
        {
            var user= _dc.Users.FirstOrDefault(u=>u.email == userLoginDto.email);
            if (user is null)
                return null;

            if (_passwordService.IsPasswordEqual(userLoginDto.password, user.passwordSalt, user.password))
                return user;
            return null;
        }
    }
}

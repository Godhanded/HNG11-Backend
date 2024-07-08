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
            var userOrganisation = new Organisation()
            {
                orgId = Guid.NewGuid().ToString(),
                name = user.firstName + "'s Organisation",
                description = ""
            };
            var userorg = new UserOrganisation()
            {
                user = user,
                organisation = userOrganisation
            };
            user.organisations.Add(userOrganisation);
            await _dc.Users.AddAsync(user);
            await _dc.SaveChangesAsync();
            
            _dc.Update(user);
            return user;
        }

        public async Task<User?> LoginUser(UserLoginDto userLoginDto)
        {
            var user= await _dc.Users
                .FirstOrDefaultAsync(u=>u.email == userLoginDto.email);
            if (user is null)
                return null;

            if (_passwordService.IsPasswordEqual(userLoginDto.password, user.passwordSalt, user.password))
                return user;
            return null;
        }
    }
}

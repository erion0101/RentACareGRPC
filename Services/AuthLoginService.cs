using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentACareGRPC.SQL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentACareGRPC.Services
{
    public class AuthLoginService : AuthLoginServices.AuthLoginServicesBase
    {
        private readonly IRepository<Customers> _repository;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthLoginService(IRepository<Customers> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        public override Task<AuthLoginRespons> Login(AuthLoginRequest request, ServerCallContext context)
        {
            if (request.Email == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Please input one value for email"));
            if(request.Password == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Please input one value for passsword"));

            // var email = _authService.FindCustomersByEmail(request.Email, context.CancellationToken);
            var email = _repository.GetAll().FirstOrDefaultAsync(x => x.Email == request.Email, context.CancellationToken);

            if (email != null && BCrypt.Net.BCrypt.Verify(request.Password, email.Result.Password))
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Expiration"]));

                var customers = _repository.GetAll().FirstOrDefaultAsync(x => x.Id == email.Result.Id, context.CancellationToken);
                if(customers != null)
                {
                    var roleName = _repository.GetRoleName().FirstOrDefaultAsync(x => x.Id == customers.Result.RoleId, context.CancellationToken);
                    if(roleName != null)
                    {
                        if(roleName.Result.Permission.PermissionName != null)
                        {
                            var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.NameIdentifier, customers.Id.ToString()),
                            new Claim(ClaimTypes.Role, roleName.Result.Name),
                            new Claim("Permission", roleName.Result.Permission.PermissionName)
                            };
                            var token = new JwtSecurityToken(
                                           _configuration["Jwt:Issuer"],
                                           _configuration["Jwt:Issuer"],
                                           expires: expiry,
                                           signingCredentials: signIn,
                                           claims: claims
                                       );
                            AuthLoginRespons responsee = new AuthLoginRespons
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(token)
                            };

                            return Task.FromResult(responsee);
                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                            new Claim(ClaimTypes.NameIdentifier, customers.Id.ToString()),
                            new Claim(ClaimTypes.Role, roleName.Result.Name),
                            };
                            var token = new JwtSecurityToken(
                                           _configuration["Jwt:Issuer"],
                                           _configuration["Jwt:Issuer"],
                                           expires: expiry,
                                           signingCredentials: signIn,
                                           claims: claims
                                       );
                            AuthLoginRespons responsee = new AuthLoginRespons
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(token)
                            };

                            return Task.FromResult(responsee);
                        }
                    }
                }
                else
                {
                    var tokeen = new JwtSecurityToken(
                                          _configuration["Jwt:Issuer"],
                                          _configuration["Jwt:Issuer"],
                                          expires: expiry,
                                          signingCredentials: signIn
                                      );
                    AuthLoginRespons response = new AuthLoginRespons
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokeen)
                    };

                    return Task.FromResult(response);
                }
            }

            throw new RpcException(new Status(StatusCode.InvalidArgument, "email or password is not valid"));
        }
    }
}

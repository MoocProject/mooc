using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mooc.Models.Context;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Mooc.Controllers
{
    public class OAuthController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _iConfiguration;
        private readonly IMapper _modelMapper;
        public OAuthController(DataContext context, IMapper modelMapper, IConfiguration configuration)
        {
            _context = context;
            _modelMapper = modelMapper;
            _iConfiguration = configuration;
        }
        public IActionResult Index()
        {
            return Content("Index");
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> GenToken(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    throw new ArgumentNullException("username", "用户名不能为空！");
                if (string.IsNullOrEmpty(password))
                    throw new ArgumentNullException("password", "密码不能为空！");

                //验证用户名和密码
                var user = await _context.Users.Where(P => P.UserState == 0 && P.UserName == username && P.PassWord == password).FirstOrDefaultAsync(); //await _UserService.CheckUserAndPassword(mobile: user, password: password);
                if (user == null)
                    return Unauthorized();
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_iConfiguration["SecurityKey"]);//_iConfiguration["SecurityKey"]

                var authTime = DateTime.UtcNow;
                var expiresAt = authTime.AddDays(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(JwtClaimTypes.Audience,"api"),
                        new Claim(JwtClaimTypes.Issuer,_iConfiguration["DomainUrl"]),
                        new Claim(JwtClaimTypes.Id,user.Id.ToString()),
                        new Claim(JwtClaimTypes.Name,user.UserName),
                    }),
                    Expires = expiresAt,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    access_token = tokenString,
                    token_type = "Bearer",
                    profile = new
                    {
                        sid = user.Id,
                        name = user.UserName,
                        email = user.Email,
                        nickName = user.NickName,
                        roleType = user.RoleType,
                        auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                        expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                    }
                });
            }
            catch (Exception e)
            {
                return Unauthorized();
            }

        }

    }
}
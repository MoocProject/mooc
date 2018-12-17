using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mooc.Models;
using Mooc.Models.Context;
using Mooc.ViewModels;
using Serilog;


namespace Mooc.Controllers
{
    // [Route("[controller]/[action]")]//the route can be changed directly here
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IMapper _modelMapper;
        public UsersController(DataContext context, IMapper modelMapper)
        {
            _context = context;
            _modelMapper = modelMapper;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<UserViewModel> GetUsers()
        {
            Log.Information(" 这是项目Get User list ");
           // Log.Debug(" 这是项目Get User list Debug ");
            IEnumerable<User> list = _context.Users;
            var viewList = _modelMapper.Map<IEnumerable<UserViewModel>>(list);//延时加载
            return viewList;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            var viewModel = _modelMapper.Map<UserViewModel>(user);
            if (viewModel == null)
            {
                return NotFound();
            }
            viewModel.ShowUserState = System.Enum.GetName(typeof(Models.EnumModels.UserStateEnum),user.UserState);//获取枚举里的值
            return Ok(viewModel);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] long id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }
            if (user.UserState == (int)Models.EnumModels.UserStateEnum.正常)
            {

            }
            else {

            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.UserState = (int)Models.EnumModels.UserStateEnum.正常;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
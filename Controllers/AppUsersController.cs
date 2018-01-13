using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using drevolution.Models.Entities;
using drevolution.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static drevolution.Helpers.Constants;

namespace backendhotel.Controllers
{
  [Produces("application/json")]
  [Route("api/AppUsers")]
  [Authorize(Policy = POLITICAS.ATIVO)]
  public class AppUsersController : Controller
  {
    private readonly ApplicationDbContext _context;

    public AppUsersController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/AppUsers
    [HttpGet]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public IEnumerable<AppUser> GetAppUser()
    {
      return _context.AppUser;
    }

    // GET: api/AppUsers/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppUser([FromRoute] string id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = await _context.AppUser.SingleOrDefaultAsync(m => m.Id == id);

      if (appUser == null)
      {
        return NotFound();
      }
      appUser.UserPhoto = null;
      return Ok(appUser);
    }

    // GET: api/AppUsers/5/foto
    [HttpGet("{id}/foto")]
    [Authorize(Policy = POLITICAS.ATIVO)]
    public async Task<IActionResult> GetAppUserWithFoto([FromRoute] string id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = await _context.AppUser.SingleOrDefaultAsync(m => m.Id == id);

      if (appUser == null)
      {
        return NotFound();
      }
      string ft = Convert.ToBase64String(appUser.UserPhoto);
      appUser.UserPhoto = null;
      return new OkObjectResult(new
      {
        user = appUser,
        foto = ft
      });
    }

    // PUT: api/AppUsers/5
    [HttpPut("{id}")]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public async Task<IActionResult> PutAppUser([FromRoute] string id, [FromBody] AppUser appUser)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != appUser.Id)
      {
        return BadRequest();
      }

      _context.Entry(appUser).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!AppUserExists(id))
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

    // POST: api/AppUsers
    [HttpPost]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public async Task<IActionResult> PostAppUser([FromBody] AppUser appUser)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _context.AppUser.Add(appUser);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetAppUser", new { id = appUser.Id }, appUser);
    }

    // DELETE: api/AppUsers/5
    [HttpDelete("{id}")]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public async Task<IActionResult> DeleteAppUser([FromRoute] string id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = await _context.AppUser.SingleOrDefaultAsync(m => m.Id == id);
      if (appUser == null)
      {
        return NotFound();
      }

      _context.AppUser.Remove(appUser);
      await _context.SaveChangesAsync();

      return Ok(appUser);
    }

    private bool AppUserExists(string id)
    {
      return _context.AppUser.Any(e => e.Id == id);
    }
  }
}

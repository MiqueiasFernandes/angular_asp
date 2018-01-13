using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backendhotel.Auth;
using drevolution.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static drevolution.Helpers.Constants;

namespace backendhotel.Controllers
{
  [Route("api/privilegios")]
  [Authorize(Policy = POLITICAS.ATIVO)]
  public class PrivilegioController : Controller
  {

    private readonly UserManager<AppUser> _userManager;
    private readonly Privilegios _privilegios;

    public PrivilegioController(UserManager<AppUser> userManager, Privilegios privilegios)
    {
      _userManager = userManager;
      _privilegios = privilegios;
    }

    // GET: api/AppUsers/5
    [HttpGet("{id}")]
    public IActionResult GetPrivilegiosFromUser([FromRoute] string id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      try
      {

        var appUser = _userManager.Users.Single(m => m.Id == id);

        if (appUser == null)
        {
          return new OkObjectResult(new { politicas = new string[] { } });
        }

        return new OkObjectResult(new { politicas = getPrivilegiosAsync(appUser) });
      }
      catch
      {
        return new OkObjectResult(new { politicas = new string[] { } });
      }
    }


    // PUT: api/AppUsers/5
    [HttpPut("{id}")]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public async Task<IActionResult> PostPrivilegios([FromRoute] string id, [FromBody] string[] politicas)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = _userManager.Users.Single(m => m.Id == id);

      if (appUser == null)
      {
        return NotFound();
      }

      await _userManager.AddClaimsAsync(appUser, _privilegios.getClaimsByPoliticas(politicas));

      return new OkObjectResult(new { politicas = getPrivilegiosAsync(appUser) });
    }



    // DELETE: api/AppUsers/5/POLITICA
    [HttpDelete("{id}/{politica}")]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public async Task<IActionResult> DeletePrivilegios([FromRoute] string id, [FromRoute] string politica)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = _userManager.Users.Single(m => m.Id == id);

      if (appUser == null)
      {
        return NotFound();
      }

      await _userManager.RemoveClaimAsync(appUser, _privilegios.getClaimByPolitica(politica));

      return new OkObjectResult(new { politicas = getPrivilegiosAsync(appUser) });
    }



    async Task<string[]> getPrivilegiosAsync(AppUser appUser)
    {
      if (appUser == null)
      {
        return null;
      }

      string[] res = _privilegios.ClaimToPolitica((await _userManager.GetClaimsAsync(appUser)).ToArray());

      return res;
    }


  }
}

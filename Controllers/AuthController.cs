using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using drevolution.Models.Entities;
using System.Threading.Tasks;
using drevolution.Helpers;
using System.Security.Claims;
using System.Linq;
using drevolution.ViewModels;
using System;
using backendhotel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using backendhotel.Auth;
using System.Collections.Generic;
using static drevolution.Helpers.Constants;

namespace drevolution.Controllers
{

  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly Privilegios _privilegios;

    public AuthController(UserManager<AppUser> userManager, Privilegios privilegios)
    {
      _userManager = userManager;
      _privilegios = privilegios;
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Post(
      [FromBody]CredentialsViewModel credentials,
      [FromServices]TokenConfigurations tokenConfigurations,
      [FromServices]SigningConfigurations signingConfigurations
      )
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      
      var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
      if (identity == null)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Usuario ou senha inválida.", ModelState));
      }

      if (!_privilegios.temPrivilegio(identity, POLITICAS.ATIVO))
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Usuario não ativado.", ModelState));
      }

      DateTime dataCriacao = DateTime.Now;
      DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

      var handler = new JwtSecurityTokenHandler();
      var securityToken = handler.CreateToken(new SecurityTokenDescriptor
      {
        Issuer = tokenConfigurations.Issuer,
        Audience = tokenConfigurations.Audience,
        SigningCredentials = signingConfigurations.SigningCredentials,
        Subject = identity,
        NotBefore = dataCriacao,
        Expires = dataExpiracao
      });

      return new OkObjectResult(new
      {
        authenticated = true,
        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
        accessToken = handler.WriteToken(securityToken),
        id = identity.Claims.Single(c => c.Type == "id").Value,
        expires_in = tokenConfigurations.Seconds,
        message = "OK"
      });

    }




    private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
    {
      if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
      {
        // get the user to verifty
        var userToVerify = await _userManager.FindByNameAsync(userName);

        if (userToVerify != null)
        {
          // check the credentials  
          if (await _userManager.CheckPasswordAsync(userToVerify, password))
          {

            List<Claim> claims = new List<Claim>(await _userManager.GetClaimsAsync(userToVerify));

            var extendPrivilegios = _privilegios.ExtendForUser(userToVerify, claims);

            if (extendPrivilegios != null)
              claims.AddRange(extendPrivilegios);

            return new ClaimsIdentity(claims);

          }
        }
      }

      // Credentials are invalid, or account doesn't exist
      return await Task.FromResult<ClaimsIdentity>(null);
    }
  }
}

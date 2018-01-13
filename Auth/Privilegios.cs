using drevolution.Helpers;
using drevolution.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace backendhotel.Auth
{
  public class Privilegios
  {

    List<Privilegio> privs = new List<Privilegio>();

    public Privilegios Add(Privilegio privilegio)
    {
      privs.Add(privilegio);
      return this;
    }

    public string[] getPoliticas()
    {
      return privs.ConvertAll(p => p._politica).ToArray();
    }

    public Privilegio getByPolitica(string politica)
    {
      return privs.Find(p => p._politica == politica);
    }


    public Claim getClaimByPolitica(string politica)
    {
      Privilegio p = getByPolitica(politica);
      if (p == null)
        return null;
      return new Claim(Constants.IDENTIFICADORES.Rol, p._token);
    }

    public Claim[] getClaimsByPoliticas(string[] politicas)
    {

      List<Claim> claims = new List<Claim>();

      foreach (var politica in politicas)
      {
        claims.Add(getClaimByPolitica(politica));
      }

      return claims.ToArray();
    }

    public void Build(IServiceCollection services)
    {

      privs.ForEach(privilegio =>
      {

        System.Console.WriteLine("Ativando politica: " + privilegio._politica);

        services.AddAuthorization(options =>
            {
              options.AddPolicy(privilegio._politica, policy => policy
              .RequireClaim(Constants.IDENTIFICADORES.Rol, privilegio._token)
              .RequireAuthenticatedUser()
              .AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme));
            });
      });

      services.AddSingleton(this);

    }

    public Claim[] ExtendForUser(AppUser user, List<Claim> claims)
    {
      return
       /// new[] { new Claim("Type", "Value") }
       null
        ;
    }

    public bool temPrivilegio(ClaimsIdentity identity, string politica)
    {
      return temPrivilegio(identity, getByPolitica(politica));
    }


    public bool temPrivilegio(ClaimsIdentity identity, Privilegio privilegio)
    {
      return (identity.HasClaim(c => privilegio._politica == getPoliticaByClaim(c)));
    }


    public string getPoliticaByClaim(Claim claim)
    {
      return claim.Type == Constants.IDENTIFICADORES.Rol ? privs.Find(p => p._token == claim.Value)?._politica : null;
    }

    public string[] ClaimToPolitica(Claim[] claims)
    {
      var res = new List<string>();

      foreach (var c in claims)
      {
        var p = getPoliticaByClaim(c);
        if (p != null)
        {
          res.Add(p);
        }
      }

      return res.ToArray();
    }


  }


  public class Privilegio
  {
    public Privilegio(string politica)
    {
      _politica = politica;
      _token = Convert.ToBase64String(Encoding.ASCII.GetBytes(politica));
    }
    public Privilegio(string politica, string token)
    {
      _politica = politica;
      _token = token;
    }

    public string _politica { get; }
    public string _token { get; }
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backendhotel.Auth;
using Microsoft.AspNetCore.Identity;

namespace drevolution.Models.Entities
{
  public class AppUser : IdentityUser
  {

    // Extended Properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] UserPhoto { get; set; }
    public string UserPhotoExtensao { get; set; }
    ////user ativado
  }

}

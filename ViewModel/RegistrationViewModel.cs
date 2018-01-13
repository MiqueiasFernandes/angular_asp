using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Validators;
using FluentValidation.Attributes;
using drevolution.ViewModels.Validations;


using Microsoft.AspNetCore.Identity;

namespace drevolution.ViewModels {

 [Validator(typeof(RegistrationViewModelValidator))]
  public class RegistrationViewModel : IdentityUser
  {
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] UserPhoto { get; set; }
    public string UserPhotoExtensao { get; set; }

  }
}


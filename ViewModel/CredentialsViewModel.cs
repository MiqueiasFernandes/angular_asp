using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation.Validators;
using FluentValidation.Attributes;

using drevolution.ViewModels.Validations;


namespace drevolution.ViewModels

{
  [Validator(typeof(CredentialsViewModelValidator))]
  public class CredentialsViewModel
  {

    public string UserName { get; set; }
    public string Password { get; set; }

  }

}

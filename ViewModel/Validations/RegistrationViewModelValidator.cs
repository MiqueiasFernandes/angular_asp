using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;



namespace drevolution.ViewModels.Validations
{

  public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>

  {

    public RegistrationViewModelValidator()
    {

      RuleFor(vm => vm.Email).NotEmpty().WithMessage("Insira um Email válido.");
      RuleFor(vm => vm.Password).NotEmpty().WithMessage("Insira uma senha válida.");
      RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("Insira seu nome.");
    }

  }

}



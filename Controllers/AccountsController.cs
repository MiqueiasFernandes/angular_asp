
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using drevolution.Models.Entities;
using Microsoft.AspNetCore.Identity;
using drevolution.data;
using drevolution.ViewModels;
using drevolution.Helpers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using backendhotel.Auth;
using static drevolution.Helpers.Constants;


namespace drevolution
{

  [Route("api/[controller]")]
  public class AccountsController : Controller
  {

    private readonly ApplicationDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly Privilegios _privilegios;

    public AccountsController(
      UserManager<AppUser> userManager,
      IMapper mapper,
      ApplicationDbContext appDbContext, Privilegios privilegios)
    {
      _userManager = userManager;
      _mapper = mapper;
      _appDbContext = appDbContext;
      _privilegios = privilegios;

    }
    
    [HttpGet]
    [Authorize(Policy = POLITICAS.ATIVO)]
    public async Task<IActionResult> GetCurrentUser()
    {
      return new OkObjectResult(await GetCurrentUserAsync());
    }
    
    // POST api/accounts
    [HttpPost("{role}/{active}")]
    [Authorize(Policy = POLITICAS.ADMIN)]
    public async Task<IActionResult> PostAdmin([FromBody]RegistrationViewModel model, [FromRoute] string role, [FromRoute] bool active)
    {
      return await createAsync(model, active, role);
    }

    // POST api/accounts
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
    {
      return await createAsync(model, false, null);
    }

    // POST api/accounts
    [HttpPost("implantar")]
    public async Task<IActionResult> PostImplantar([FromBody]RegistrationViewModel model)
    {

      foreach (var u in _userManager.Users.ToArray())
      {
        if ("sys" == u.UserName)
          return new OkObjectResult(new { sys = "systema implantado com sucesso!" });
      }
      model.UserName = "sys";
      model.Email = "sys@mail.com";
      model.FirstName = "sys";
      model.LastName = "sys";
      System.Console.WriteLine(".........A IMPLANTAR SYSTEMA............");
      return await createAsync(model, true, POLITICAS.ADMIN);
    }


    async Task<IActionResult> createAsync(RegistrationViewModel model, bool active, string politica)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var userIdentity = _mapper.Map<AppUser>(model);
      var result = await _userManager.CreateAsync(userIdentity, model.Password);

      await _userManager.AddClaimAsync(userIdentity, new Claim(Constants.IDENTIFICADORES.Id, userIdentity.Id));

      if (active)
        await _userManager.AddClaimAsync(userIdentity, _privilegios.getClaimByPolitica(POLITICAS.ATIVO));

      if (politica != null)
        await _userManager.AddClaimAsync(userIdentity, _privilegios.getClaimByPolitica(politica));


      if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
      await _appDbContext.SaveChangesAsync();

      return new OkObjectResult(new { result = new[] { "CREATED", active ? "ACTIVE" : "INACTIVE" } });
    }
    

    private async Task<AppUser> GetCurrentUserAsync()
      => await _userManager.FindByIdAsync(User.FindFirstValue(Constants.IDENTIFICADORES.Id));

  }

}

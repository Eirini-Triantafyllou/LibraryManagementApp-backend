using LibraryManagementApp.Models;
using LibraryManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("AngularApp")]
    public class BaseController : ControllerBase
    {
        public readonly IApplicationService applicationService;

        public BaseController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        private ApplicationUser? appUser;

        protected ApplicationUser? AppUser
        {
            get 
            { 
                if (appUser != null && User.Claims != null && User.Claims.Any())
                {
                    var claimsType = User.Claims.Select(c => c.Type).ToList();
                    if (!claimsType.Contains(ClaimTypes.NameIdentifier))
                    { 
                     return null;
                    }
                    var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                    appUser = new ApplicationUser
                    {
                        Id = userId,
                        Username = User.FindFirst(ClaimTypes.Name)?.Value,
                        Email = User.FindFirst(ClaimTypes.Email)?.Value
                    };
                    return appUser;
                }
                return null;
            }
        }
    }
}

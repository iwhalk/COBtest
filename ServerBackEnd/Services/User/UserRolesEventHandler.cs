using ApiGateway.Data;
using ApiGateway.Interfaces;
using ApiGateway.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ApiGateway.Services
{


    public class UserAddRolesEventHandler : IRequestHandler<UserAddRolesCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IRolService _logRolInsertion;
        //IRolService logRolInsertion
        public UserAddRolesEventHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //_logRolInsertion = logRolInsertion;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Handle(UserAddRolesCommand addRolesCommand, CancellationToken cancellationToken)
        {
        var res = IdentityResult.Failed();
        var user = await _userManager.FindByIdAsync(addRolesCommand.UserId);
        if (user == null)
        {
            res = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidUserName(addRolesCommand.UserId));
            return res;
        }

        if (addRolesCommand.Role != null)
        {
            var roleExists = await _roleManager.RoleExistsAsync(addRolesCommand.Role);
            if (!roleExists)
            {
                res = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidRoleName(addRolesCommand.Role));
                return res;
            }

            res = await _userManager.AddToRoleAsync(user, addRolesCommand.Role);
        }

        if (addRolesCommand.Roles != null)
        {
            foreach (var role in addRolesCommand.Roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    res = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidRoleName(role));
                    return res;
                }
            }
            res = await _userManager.AddToRolesAsync(user, addRolesCommand.Roles);
        }

        //await _logRolInsertion.InsertLogAddOrRemoveRole(addRolesCommand);


        return res;
    }
}

    public class UserRemoveRolesEventHandler : IRequestHandler<UserRemoveRolesCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRemoveRolesEventHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Handle(UserRemoveRolesCommand removeRolesCommand, CancellationToken cancellationToken)
        {
            var res = IdentityResult.Failed();
            var user = await _userManager.FindByIdAsync(removeRolesCommand.UserId);
            if (user == null)
            {
                res = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidUserName(removeRolesCommand.UserId));
                return res;
            }

            if (removeRolesCommand.Role != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(removeRolesCommand.Role);
                if (!roleExists)
                {
                    res = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidRoleName(removeRolesCommand.Role));
                    return res;
                }

                res = await _userManager.RemoveFromRoleAsync(user, removeRolesCommand.Role);
            }

            if (removeRolesCommand.Roles != null)
            {
                foreach (var role in removeRolesCommand.Roles)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (!roleExists)
                    {
                        res = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidRoleName(role));
                        return res;
                    }
                }
                res = await _userManager.RemoveFromRolesAsync(user, removeRolesCommand.Roles);
            }
            return res;
        }
    }

    public class AddRolesEventHandler : IRequestHandler<AddRolesCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IRolService _logRolInsertion;
        //IRolService logRolInsertion
        public AddRolesEventHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //_logRolInsertion = logRolInsertion;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Handle(AddRolesCommand removeRolesCommand, CancellationToken cancellationToken)
        {
            var res = IdentityResult.Failed();

            if (removeRolesCommand.RoleName != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(removeRolesCommand.RoleName);
                if (roleExists)
                {
                    res = IdentityResult.Failed(_userManager.ErrorDescriber.DuplicateRoleName(removeRolesCommand.RoleName));
                    return res;
                }
                res = await _roleManager.CreateAsync(new IdentityRole(removeRolesCommand.RoleName));
            }

            if (removeRolesCommand.RoleNames != null)
            {
                foreach (var role in removeRolesCommand.RoleNames)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (roleExists)
                    {
                        res = IdentityResult.Failed(_userManager.ErrorDescriber.DuplicateRoleName(role));
                        return res;
                    }
                    res = await _roleManager.CreateAsync(new IdentityRole(role));
                }
                foreach (var role in removeRolesCommand.RoleNames)
                {
                    res = await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //await _logRolInsertion.InsertLogAddRole(removeRolesCommand);

            return res;
        }
    }

    public class GetRolesEventHandler : IRequestHandler<GetRolesCommand, List<IdentityRole>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRolesEventHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> Handle(GetRolesCommand getRolesCommand, CancellationToken cancellationToken)
        {
            return _roleManager.Roles.ToList();
        }
    }

    public class UserAddRolesCommand : IRequest<IdentityResult>
    {
        public string UserId { get; set; }
        public string? Role { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
    public class UserRemoveRolesCommand : IRequest<IdentityResult>
    {
        public string UserId { get; set; }
        public string? Role { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
    public class AddRolesCommand : IRequest<IdentityResult>
    {
        public string? RoleName { get; set; }
        public IEnumerable<string>? RoleNames { get; set; }
    }
    public class GetRolesCommand : IRequest<List<IdentityRole>>
    {
        public string? RoleNames { get; set; }
    }

    //public class LogRolInsertion : ILogRolInsertion
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;
    //    private readonly BackOfficeFerromexContext _dbContext;
    //    private readonly RoleManager<IdentityRole> _roleManager;

    //    public LogRolInsertion(IHttpContextAccessor httpContextAccessor, BackOfficeFerromexContext dbContext, RoleManager<IdentityRole> roleManager)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //        _dbContext = dbContext;
    //        _roleManager = roleManager;
    //    }

    //public async Task InsertLogAddOrRemoveRole(UserAddRolesCommand addRolesCommand)
    //{
    //    try
    //    {
    //        var idHttpContext = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Usuario no logueado";
    //        var roles = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? "No tiene rol";


    //        var roleIdOld = await _roleManager.FindByNameAsync(roles);
    //        var roleIdNew = await _roleManager.FindByNameAsync(addRolesCommand.Role);

    //        LogUserActivity logRole = new LogUserActivity()
    //        {
    //            IdModifiedUser = idHttpContext,
    //            UpdatedDate = DateTime.Now,
    //            IdUpdatedUser = addRolesCommand.UserId,
    //            TypeAction = "2",
    //            AspNetRolesIdOld = Convert.ToString(roleIdOld.Id),
    //            AspNetRolesIdNew = Convert.ToString(roleIdNew.Id)
    //        };

    //        await _dbContext.LogUserActivities.AddAsync(logRole);
    //        await _dbContext.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    //    public async Task InsertLogAddRole(AddRolesCommand addRolesCommand)
    //    {
    //        try
    //        {
    //            var idHttpContext = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Usuario no logueado";

    //            var roleIdNew = await _roleManager.FindByNameAsync(addRolesCommand.RoleName);

    //            LogRole logRole = new LogRole()
    //            {
    //                UpdatedDate = DateTime.Now,
    //                IdUser = idHttpContext,
    //                AspNetRolesId = roleIdNew.Id,
    //                TypeAction = "1",
    //                NewNameRol = roleIdNew.Name,
    //                Active = true
    //            };

    //            await _dbContext.LogRoles.AddAsync(logRole);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //        catch (Exception)
    //        {

    //        }
    //    }

    //    public async Task InsertLogEditRole(Rol rol, IdentityRole rolOld)
    //    {
    //        try
    //        {
    //            var idHttpContext = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Usuario no logueado";

    //            LogRole logRole = new LogRole()
    //            {
    //                UpdatedDate = DateTime.Now,
    //                IdUser = idHttpContext,
    //                AspNetRolesId = rolOld.Id,
    //                TypeAction = "5",
    //                OldNameRol = rolOld.Name,
    //                NewNameRol = rol.NombreRol,
    //                Active = rol.Estatus
    //            };

    //            await _dbContext.LogRoles.AddAsync(logRole);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //        catch (Exception)
    //        {

    //        }
    //    }
    //}
}

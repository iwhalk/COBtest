using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ReportesInmobiliaria.Interfaces;

namespace ReportesInmobiliaria.Services
{
    public class ModulesService : IModulesService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ModulesService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtiene el primer modulo de la base de datos
        /// </summary>
        /// <param name="id">Ej. 1</param>
        /// <returns>Devuelve el primer modelo de modules, que encuentre en la base de datos</returns>
        public async Task<Module?> GetModuleAsync(int id)
        {
            return await _dbContext.Modules.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Obtiene todos los modulos que tenga ese rol
        /// </summary>
        /// <param name="role">Ej. ADMINISTRAR</param>
        /// <returns>Devuelve la lista de modulos que tenga ese rol</returns>
        /// <exception cref="ValidationException">El rol dado es invalido</exception>
        public async Task<List<Module>?> GetModulesAsync(string? role = null)
        {
            if (!string.IsNullOrEmpty(role))
            {
                var res = await _dbContext.AspNetRoles.FirstOrDefaultAsync(x => x.NormalizedName == role.ToUpper());
                if (res == null) throw new ValidationException($"El rol {role} es invalido");
                var dbRole = await _dbContext.AspNetRoles.Include(x => x.Modules).FirstOrDefaultAsync(x => x.Name == role);
                return dbRole?.Modules.ToList();
            }
            return await _dbContext.Modules.ToListAsync();
        }

        /// <summary>
        /// Ingresar un nuevo modulo en la base de datos
        /// </summary>
        /// <param name="module"></param>
        /// <returns>Devuelve el modulo anteriormente ingresado</returns>
        public async Task<Module?> PostModuleAsync(Module module)
        {
            await _dbContext.Modules.AddAsync(module);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return module;
        }

        public async Task<bool> PutModuleAsync(int id, Module module)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteModuleAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ingresa un nuevo RoleModul a la base de datos
        /// </summary>
        /// <param name="roleModules"></param>
        /// <returns>Devuelve el roleModul anteriormente ingresado</returns>
        /// <exception cref="ValidationException">El rolModul dado es invalido</exception>
        public async Task<RoleModules?> PostRoleModulesAsync(RoleModules roleModules)
        {
            var role = await _dbContext.AspNetRoles.Include(x => x.Modules).FirstOrDefaultAsync(x => x.Name.ToUpper() == roleModules.RoleName.ToUpper());
            if (role == null) throw new ValidationException($"El rol {roleModules.RoleName} es invalido");

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Usuario no logueado";
            var modulesList = new List<Module?>();
            foreach (var module in roleModules.Modules)
            {
                var dbModule = _dbContext.Modules.FirstOrDefault(x => x.Id == module);
                if (dbModule != null)
                {
                    modulesList.Add(dbModule);
                    _ = _dbContext.LogRoleModules.Add(new() { IdUser = userId, AspNetRolesId = role.Id, ModulesId = dbModule.Id, TypeAction = "3", UpdatedDate = DateTime.Now });
                }
            }
            foreach (var item in role.Modules.Where(x => !modulesList.Contains(x)))
            {
                _ = _dbContext.LogRoleModules.Add(new() { IdUser = userId, AspNetRolesId = role.Id, ModulesId = item.Id, TypeAction = "4", UpdatedDate = DateTime.Now });
            }

            role.Modules = modulesList;

            var re = _dbContext.Update(role);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return roleModules;
        }
    }
}

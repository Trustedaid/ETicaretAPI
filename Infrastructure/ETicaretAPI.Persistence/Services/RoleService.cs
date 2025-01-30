using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Persistence.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> CreateRole(string name)
    {
        var result = await _roleManager.CreateAsync(new AppRole { Id = Guid.NewGuid().ToString(), Name = name });
        return result.Succeeded;
    }

    public async Task<bool> DeleteRole(string id)
    {
        var appRole = await _roleManager.FindByIdAsync(id);
        var result = await _roleManager.DeleteAsync(appRole);
        return result.Succeeded;
    }

    public async Task<bool> UpdateRole(string id, string name)
    {
        var role = await _roleManager.FindByIdAsync(id);
        var result = await _roleManager.UpdateAsync(role);
        return result.Succeeded;
    }

    public (object, int) GetAllRoles(int page, int size)
    {
        var query = _roleManager.Roles;
        var data = query.Skip(page * size).Take(size).ToList();
        var count = query.Count();
        return (data, count);
    }

    public async Task<(string id, string name)> GetRoleById(string id)
    {
        var role = await _roleManager.GetRoleIdAsync(new AppRole { Id = id });
        return (id, role);
    }
}
using System;
using Data.Contexts;
using Data.Models;

namespace Repo;

public class RoleRepo : BaseRepo<Role>, IRoleRepo
{
    public RoleRepo(OutfitDBContext context) : base(context)
    {
    }
}

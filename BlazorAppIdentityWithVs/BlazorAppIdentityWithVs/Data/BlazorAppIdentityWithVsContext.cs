using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorAppIdentityWithVs.Data;

namespace BlazorAppIdentityWithVs.Data
{
    public class BlazorAppIdentityWithVsContext(DbContextOptions<BlazorAppIdentityWithVsContext> options) : IdentityDbContext<BlazorAppIdentityWithVsUser>(options)
    {
    }
}

using Microsoft.AspNetCore.Identity;

namespace Reservei.Core.Entities;

public class User : IdentityUser
{
  public string FullName { get; set; } = string.Empty;
}

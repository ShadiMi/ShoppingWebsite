using Microsoft.AspNetCore.Identity;

namespace ShoppingWebsite.Areas.Identity.Data;

public class ShoppingWebsiteUser : IdentityUser
{
    public string ContactName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    public string Country { get; set; }
    // Use the built-in PhoneNumber property of IdentityUser for storing phone numbers
    public string Phone { get; set; }
}

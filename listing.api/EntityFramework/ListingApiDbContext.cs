using Microsoft.EntityFrameworkCore;

namespace listing.api.EntityFramework {
    public class ListingApiDbContext : DbContext {
        public ListingApiDbContext (DbContextOptions<ListingApiDbContext> options) : base (options) {

        }
    }
}
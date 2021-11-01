using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IepProjekat.Models.Database {
    public class IepProjekatContext : IdentityDbContext<User> {

        public DbSet<Auction> Auctions {get; set;} 
        public DbSet<BagToken> BagTokens {get; set;} 
        public DbSet<TokenTransaction> TokenTransactions {get; set;} 

        public IepProjekatContext ( DbContextOptions options ) : base ( options ) { }

        protected override void OnModelCreating ( ModelBuilder builder ) {
            base.OnModelCreating ( builder );

            builder.Entity<User>()
                .Property(u => u.state)
                .HasDefaultValue("Active");

            builder.Entity<User>()
                .Property(u => u.tokens)
                .HasDefaultValue(0.00);

            builder.ApplyConfiguration ( new IdentityRoleConfiguration ( ) );
            builder.ApplyConfiguration(new AuctionConfiguration());
            builder.ApplyConfiguration(new BagTokenConfiguration());
            builder.ApplyConfiguration(new TokenTransactionConfiguration());
        }
    }
}
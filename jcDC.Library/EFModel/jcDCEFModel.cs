namespace jcDC.Library.EFModel {
    using System.Data.Entity;

    public partial class jcDCEFModel : DbContext {
        public jcDCEFModel()
            : base("name=jcDCEFModel") {
        }

        public virtual DbSet<Cache> Caches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Cache>()
                .Property(e => e.Key)
                .IsUnicode(false);

            modelBuilder.Entity<Cache>()
                .Property(e => e.KeyValue)
                .IsUnicode(false);
        }
    }
}
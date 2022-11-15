using Microsoft.EntityFrameworkCore;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ChatApplicationWithSQLServer.DataModels
{
    public class ChatApplicationDBContext: DbContext
    {
        public readonly string _connstring = string.Empty;

        public ChatApplicationDBContext()
        {
            _connstring = string.Empty;
        }

        public ChatApplicationDBContext(string connection)
        {
            _connstring = connection;
        }

        public ChatApplicationDBContext(DbContextOptions<ChatApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if(!dbContextOptionsBuilder.IsConfigured)
            {
                if(!string.IsNullOrEmpty(_connstring))
                {
                    dbContextOptionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings[_connstring].ConnectionString, options => options.EnableRetryOnFailure());
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.UserName).HasColumnName("UserName");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Password).HasColumnName("Password");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.Property(e => e.ActivationToken).HasColumnName("ActivationToken");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("ROOM");
                entity.HasKey(e => e.RoomId);
                entity.Property(e => e.RoomId).HasColumnName("RoomId");
                entity.Property(e => e.RoomName).HasColumnName("RoomName");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("CHAT");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.RoomId).HasColumnName("RoomId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Message).HasColumnName("Message");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
            });
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
    }
}

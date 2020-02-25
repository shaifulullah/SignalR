using Chnage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class MyECODBContext : DbContext

    {
        public MyECODBContext()
        {

        }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyECODBContext(DbContextOptions<MyECODBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            //DbContext.Configuration.ProxyCreationEnabled = true;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductECR> ProductECRs { get; set; }
        public DbSet<ProductECO> ProductECOs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleECR> UserRoleECRs { get; set; }
        public DbSet<UserRoleECO> UserRoleECOs { get; set; }
        public DbSet<UserRoleECN> UserRoleECNs { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestTypeECR> RequestTypeECRs { get; set; }
        public DbSet<RequestTypeECO> RequestTypeECOs { get; set; }
        public DbSet<ECR> ECRs { get; set; }
        public DbSet<ECO> ECOs { get; set; }
        public DbSet<ECN> ECNs { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<ECRHasECO> ECRHasECOs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<UpdateWebAppLogs> UpdateWebAppLogs { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<ECNHasECO> ECNHasECOs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //users to be notified. did not work.
            //modelBuilder.Entity<ECR>()
            //    .HasOne(e => e.Originator)
            //    .WithMany(u => u.ECRs)
            //    .HasForeignKey(e => e.OriginatorId);
            //modelBuilder.Entity<ECR>()
            //    .HasOne(e => e.UsersToBeNotified)
            //    .WithMany()
            //    .HasForeignKey(e => e.UsersToBeNotified);
            //modelBuilder.Entity<ECO>()
            //    .HasOne(e => e.Originator)
            //    .WithMany(u => u.ECOs)
            //    .HasForeignKey(e => e.OriginatorId);
            //modelBuilder.Entity<ECO>()
            //    .HasOne(e => e.UsersToBeNotified)
            //    .WithMany()
            //    .HasForeignKey(e => e.UsersToBeNotified);
            //modelBuilder.Entity<ECN>()
            //    .HasOne(e => e.Originator)
            //    .WithMany(u => u.ECNs)
            //    .HasForeignKey(e => e.OriginatorId);
            //modelBuilder.Entity<ECN>()
            //    .HasOne(e => e.UsersToBeNotified)
            //    .WithMany()
            //    .HasForeignKey(e => e.UsersToBeNotified);
            modelBuilder.Entity<Audit>(e =>
            {
                e.ToTable("Audits");

                e.HasKey(x => x.Id);
                e.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                e.Property(x => x.TableName)
                    .IsRequired()
                    .HasMaxLength(100);
                e.Property(x => x.Action)
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(x => x.EntityId)
                    .HasColumnType("varchar(100)");

                e.Property(x => x.ChangedColumns)
                    .HasColumnType("varchar(max)");

                e.Property(x => x.OldData)
                    .HasColumnType("varchar(max)");

                e.Property(x => x.NewData)
                    .HasColumnType("varchar(max)");
            });
            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<ProductECR>()
                .HasKey(pe => new { pe.ProductId, pe.ECRId });
            modelBuilder.Entity<ProductECR>()
                .HasOne(pe => pe.ECR)
                .WithMany(e => e.AffectedProducts)
                .HasForeignKey(pe => pe.ECRId);

            modelBuilder.Entity<ProductECO>()
                .HasKey(pe => new { pe.ProductId, pe.ECOId });
            modelBuilder.Entity<ProductECO>()
                .HasOne(pc => pc.ECO)
                .WithMany(e => e.AffectedProducts)
                .HasForeignKey(pc => pc.ECOId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRoleECR>()
                .HasKey(ur => new { ur.UserRoleId, ur.ECRId });
            modelBuilder.Entity<UserRoleECR>()
                .HasOne(ur => ur.ECR)
                .WithMany(e => e.Approvers)
                .HasForeignKey(ur => ur.ECRId);

            modelBuilder.Entity<RequestTypeECR>()
                .HasKey(rc => new { rc.RequestTypeId, rc.ECRId });

            modelBuilder.Entity<RequestTypeECR>()
                .HasOne(rc => rc.ECR)
                .WithMany(rt => rt.AreasAffected)
                .HasForeignKey(rc => rc.ECRId);

            modelBuilder.Entity<UserRoleECO>()
                .HasKey(ur => new { ur.UserRoleId, ur.ECOId });
            modelBuilder.Entity<UserRoleECO>()
                .HasOne(ur => ur.ECO)
                .WithMany(e => e.Approvers)
                .HasForeignKey(ur => ur.ECOId);

            modelBuilder.Entity<RequestTypeECO>()
                .HasKey(rc => new { rc.RequestTypeId, rc.ECOId });

            modelBuilder.Entity<RequestTypeECO>()
                .HasOne(rc => rc.ECO)
                .WithMany(rt => rt.AreasAffected)
                .HasForeignKey(rc => rc.ECOId);

            modelBuilder.Entity<UserRoleECN>()
                .HasKey(ur => new { ur.UserRoleId, ur.ECNId });
            modelBuilder.Entity<UserRoleECN>()
                .HasOne(ur => ur.ECN)
                .WithMany(e => e.Approvers)
                .HasForeignKey(ur => ur.ECNId);

            modelBuilder.Entity<ECRHasECO>()
                .HasKey(ee => new { ee.ECRId, ee.ECOId });
            modelBuilder.Entity<ECRHasECO>()
                .HasOne(ee => ee.ECR)
                .WithMany(ecr => ecr.RelatedECOs)
                .HasForeignKey(ee => ee.ECRId);
            modelBuilder.Entity<ECRHasECO>()
                .HasOne(ee => ee.ECO)
                .WithMany(eco => eco.RelatedECRs)
                .HasForeignKey(ee => ee.ECOId);

            modelBuilder.Entity<ECNHasECO>()
                .HasKey(ee => new { ee.ECOId, ee.ECNId });
            modelBuilder.Entity<ECNHasECO>()
                .HasOne(ee => ee.ECO)
                .WithMany(eco => eco.RelatedECNs)
                .HasForeignKey(ee => ee.ECOId);
            modelBuilder.Entity<ECNHasECO>()
                .HasOne(ee => ee.ECN)
                .WithMany(ecn => ecn.RelatedECOs)
                .HasForeignKey(ee => ee.ECNId);


            //code for having a Dictionary inside the Model
            modelBuilder.Entity<ECO>(e =>
            {
                e.Property(u => u.NotesForApprover)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s)
                )
                .HasMaxLength(4000);
                e.Property(u => u.NotesForValidator)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s)
                )
                .HasMaxLength(4000);
                e.Property(u => u.LinkUrls)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s)
                )
                .HasMaxLength(4000);
            });
            modelBuilder.Entity<ECR>(e =>
            {
                e.Property(u => u.LinkUrls)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s))
                .HasMaxLength(4000);
            });

            modelBuilder.Entity<AuditLog>(e =>
            {
                e.Property(u => u.LinkUrls)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s))
                    .HasMaxLength(4000);
                e.Property(u => u.NotesForApprovers)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s)
                );
                e.Property(u => u.NotesForValidators)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, string>>(s)
                );
            });
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var auditEntries = GetAuditEntries();
            var count = base.SaveChanges(acceptAllChangesOnSuccess);

            if (auditEntries == null || auditEntries.Count == 0) return count;
            //var audits = new List<Audit>();

            foreach (var auditEntry in auditEntries)
            {
                Add(auditEntry.PerformAfterSaveTasks());
                //var audit = auditEntry.PerformAfterSaveTasks();
                //Add(audit);
                //audits.Add(audit);
            }

            base.SaveChanges();

            //NotificationSenderRepository notificationSenderRepository = new NotificationSenderRepository(this, _httpContextAccessor);
            //notificationSenderRepository.SendNotification(audits);
            return count;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var auditEntries = GetAuditEntries();
            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (auditEntries == null || auditEntries.Count == 0) return count;

            //var audits = new List<Audit>();

            foreach (var auditEntry in auditEntries)
            {
                Add(auditEntry.PerformAfterSaveTasks());
                //var audit = auditEntry.PerformAfterSaveTasks();
                //Add(audit);
                //audits.Add(audit);
            }

            await base.SaveChangesAsync();

            //NotificationSenderRepository notificationSenderRepository = new NotificationSenderRepository(this, _httpContextAccessor);
            //notificationSenderRepository.SendNotification(audits);

            return count;
        }

        private List<AuditEntryRepository> GetAuditEntries()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var auditEntries = new List<AuditEntryRepository>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntryRepository(entry, userName);
                auditEntry.PerformBeforeSaveTasks();
                auditEntries.Add(auditEntry);
            }
            return auditEntries;
        }
    }
}

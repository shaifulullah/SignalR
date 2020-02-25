namespace MDA.Security.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
            : base("Application_Db")
        {
            var sqlServerInstance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<ApplicationDBContext>(null);
        }

        public DbSet<ApplicationComponent> ApplicationComponentSet { get; set; }
        public DbSet<Application> ApplicationSet { get; set; }
        public DbSet<ApplicationSettings> ApplicationSettingsSet { get; set; }
        public DbSet<AppUsageLog> AppUsageLogSet { get; set; }
        public DbSet<Audit> AuditSet { get; set; }
        public DbSet<BusinessEntityAccessByAD> BusinessEntityAccessByADSet { get; set; }
        public DbSet<BusinessEntityAccessByRole> BusinessEntityAccessByRoleSet { get; set; }
        public DbSet<BusinessEntityAccessByUserAccounts> BusinessEntityAccessByUserAccountsSet { get; set; }
        public DbSet<BusinessEntityAccessByUser> BusinessEntityAccessByUserSet { get; set; }
        public DbSet<BusinessEntityRestrictionByAD> BusinessEntityRestrictionByADSet { get; set; }
        public DbSet<BusinessEntityRestrictionByRole> BusinessEntityRestrictionByRoleSet { get; set; }
        public DbSet<BusinessEntityRestrictionByUserAccounts> BusinessEntityRestrictionByUserAccountsSet { get; set; }
        public DbSet<BusinessEntityRestrictionByUser> BusinessEntityRestrictionByUserSet { get; set; }
        public DbSet<CompanyInApplication> CompanyInApplicationSet { get; set; }
        public DbSet<Company> CompanySet { get; set; }
        public DbSet<ExternalCompany> ExternalCompanySet { get; set; }
        public DbSet<ExternalPerson> ExternalPersonSet { get; set; }
        public DbSet<LogAction> LogActionSet { get; set; }
        public DbSet<MasterUser> MasterUserSet { get; set; }
        public DbSet<Projects> ProjectsSet { get; set; }
        public DbSet<RO_Department> RO_DepartmentSet { get; set; }
        public DbSet<RO_EmployeeAddInfo> RO_EmployeeAddInfoSet { get; set; }
        public DbSet<RO_EmployeeDetails> RO_EmployeeDetailsSet { get; set; }
        public DbSet<RO_Employee> RO_EmployeeSet { get; set; }
        public DbSet<RO_Skills> RO_SkillsSet { get; set; }
        public DbSet<SecurityBusinessEntities> SecurityBusinessEntitiesSet { get; set; }
        public DbSet<SecurityItem> SecurityItemSet { get; set; }
        public DbSet<SecurityLevel> SecurityLevelSet { get; set; }
        public DbSet<SecurityRightsForSecurityRole> SecurityRightsForSecurityRoleSet { get; set; }
        public DbSet<SecurityRightsForUserAccount> SecurityRightsForUserAccountSet { get; set; }
        public DbSet<SecurityRoleRights> SecurityRoleRightsSet { get; set; }
        public DbSet<SecurityRolesDetails> SecurityRolesDetailsSet { get; set; }
        public DbSet<SecurityRoles> SecurityRolesSet { get; set; }
        public DbSet<SecurityType> SecurityTypeSet { get; set; }
        public DbSet<SecurityUserInRoles> SecurityUserInRolesSet { get; set; }
        public DbSet<SecurityUserRights> SecurityUserRightsSet { get; set; }
        public DbSet<TreeMenu> TreeMenuSet { get; set; }
        public DbSet<UserAccountDetails> UserAccountDetailsSet { get; set; }
        public DbSet<UserAccount> UserAccountSet { get; set; }
        public DbSet<UserApplicationFavourites> UserApplicationFavouritesSet { get; set; }
        public DbSet<UserApplicationSettings> UserApplicationSettingsSet { get; set; }
        public DbSet<UserDelegate> UserDelegateSet { get; set; }
        public DbSet<UserInCompanyInApplicationDetails> UserInCompanyInApplicationDetailsSet { get; set; }
        public DbSet<UserInCompanyInApplication> UserInCompanyInApplicationSet { get; set; }
        public DbSet<UserLog> UserLogSet { get; set; }
        public DbSet<UserPreferences> UserPreferencesSet { get; set; }
        public DbSet<UserSettings> UserSettingsSet { get; set; }
        public DbSet<UserAttributes> UserAttributeSet { get; set; }

        /// <summary>
        /// Gets Application Name
        /// </summary>
        protected string Application
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationName"];
            }
        }

        /// <summary>
        /// SaveChanges Throw Exception if called without user or application Information
        /// </summary>
        /// <returns>Exception</returns>
        public override int SaveChanges()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>True on Success, False on Failure</returns>
        public int SaveChanges(string userName)
        {
            try
            {
                foreach (var dbEntityEntry in ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added || x.State == EntityState.Deleted || x.State == EntityState.Modified))
                {
                    if (dbEntityEntry.State != EntityState.Deleted)
                    {
                        foreach (var propertyName in dbEntityEntry.CurrentValues.PropertyNames)
                        {
                            if (dbEntityEntry.Entity.GetType().GetProperty(propertyName).ToString().Contains("System.Int32"))
                            {
                                dbEntityEntry.Property(propertyName).CurrentValue = dbEntityEntry.Property(propertyName).CurrentValue ?? 0;
                            }
                        }
                    }

                    foreach (var audit in GetAuditRecord(dbEntityEntry, userName))
                    {
                        AuditSet.Add(audit);
                    }
                }

                return base.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
        }

        /// <summary>
        /// Get Audit Record
        /// </summary>
        /// <param name="dbEntityEntry">DB Entry</param>
        /// <param name="userName">User Name</param>
        /// <returns>Audit Record</returns>
        private IEnumerable<Audit> GetAuditRecord(DbEntityEntry dbEntityEntry, string userName)
        {
            IList<Audit> result = new List<Audit>();

            var tableName = dbEntityEntry.Entity.GetType().Name;
            var keyName = dbEntityEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any()).Name;

            if (dbEntityEntry.State == EntityState.Added)
            {
                result.Add(new Audit
                {
                    Type = "I",
                    TableName = tableName,
                    PrimaryKeyField = dbEntityEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                    FieldName = "*ALL",
                    NewValue = GetEntityData(dbEntityEntry.CurrentValues),
                    UserName = userName,
                    Application = Application,
                    UpdateDate = DateTime.UtcNow
                });
            }
            else if (dbEntityEntry.State == EntityState.Deleted)
            {
                result.Add(new Audit
                {
                    Type = "D",
                    TableName = tableName,
                    PrimaryKeyField = dbEntityEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    FieldName = "*ALL",
                    OldValue = GetEntityData(dbEntityEntry.OriginalValues),
                    UserName = userName,
                    Application = Application,
                    UpdateDate = DateTime.UtcNow
                });
            }
            else if (dbEntityEntry.State == EntityState.Modified)
            {
                foreach (var propertyName in dbEntityEntry.CurrentValues.PropertyNames)
                {
                    if (!Equals(dbEntityEntry.GetDatabaseValues().GetValue<object>(propertyName),
                        dbEntityEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        var previousValue = dbEntityEntry.GetDatabaseValues().GetValue<object>(propertyName);
                        var currentValue = dbEntityEntry.CurrentValues.GetValue<object>(propertyName);

                        result.Add(new Audit
                        {
                            Type = "U",
                            TableName = tableName,
                            PrimaryKeyField = dbEntityEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                            FieldName = propertyName,
                            FieldType = previousValue == null ? currentValue.GetType().Name : previousValue.GetType().Name,
                            OldValue = previousValue == null ? null : previousValue.ToString(),
                            NewValue = currentValue == null ? null : currentValue.ToString(),
                            UserName = userName,
                            Application = Application,
                            UpdateDate = DateTime.UtcNow
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get Entity Data
        /// </summary>
        /// <param name="dbPropertyValues">DB Property Values</param>
        /// <returns>Entity Data as name:value strings for all properties in the Entity</returns>
        private string GetEntityData(DbPropertyValues dbPropertyValues)
        {
            var data = string.Empty;
            foreach (var propertyName in dbPropertyValues.PropertyNames)
            {
                data = string.Format("{0}{1}:{2},", data, propertyName, dbPropertyValues.GetValue<object>(propertyName));
            }

            return data.TrimEnd(',');
        }
    }
}
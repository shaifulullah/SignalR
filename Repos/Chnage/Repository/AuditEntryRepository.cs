using Chnage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class AuditEntryRepository
    {
        private readonly string[] TablesToAudit = { "ECNs", "ECOs", "ECRs", "ECRHasECOs", "ECRs", "Notifications", "ProductECOs", "ProductECRs",
            "RequestTypeECOs","RequestTypeECRs","RequestTypes","UserRoleECNs","UserRoleECOs","UserRoleECRs" };

        private bool CanAudit => TablesToAudit.Any(x => x.ToLower() == TableName.ToLower());

        private string UserName { get; set; }

        private EntityEntry EntityEntry { get; set; }

        private string TableName { get; set; }

        private string EntityId { get; set; }

        private EntityState Action { get; set; }

        private List<string> ChangedColumns { get; set; }

        private Dictionary<string, object> OldValues { get; set; }

        private Dictionary<string, object> NewValues { get; set; }

        private Dictionary<string, object> TempProperties { get; set; }


        public AuditEntryRepository(EntityEntry entityEntry, string username)
        {
            UserName = username;
            EntityEntry = entityEntry;
            Action = entityEntry.State;
            TableName = entityEntry.Metadata.Relational().TableName;
            OldValues = new Dictionary<string, object>();
            NewValues = new Dictionary<string, object>();
            TempProperties = new Dictionary<string, object>();
            ChangedColumns = new List<string>();
        }

        public void PerformBeforeSaveTasks()
        {
            switch (Action)
            {
                case EntityState.Deleted:

                    foreach (var property in EntityEntry.Properties)
                    {
                        if (property.Metadata.IsPrimaryKey())
                        {
                            EntityId = property.OriginalValue.ToString();
                        }

                        OldValues.Add(property.Metadata.Name, property.OriginalValue);
                    }

                    break;
                case EntityState.Modified:

                    var originalValues = EntityEntry.GetDatabaseValues();
                    foreach (var property in EntityEntry.Properties)
                    {
                        var originalValue = originalValues[property.Metadata.Name];
                        OldValues.Add(property.Metadata.Name, originalValue);

                        if (property.Metadata.IsPrimaryKey())
                        {
                            EntityId = property.OriginalValue.ToString();
                        }

                        if (property.IsTemporary)
                        {
                            TempProperties.Add(property.Metadata.Name, originalValue);
                            continue;
                        }

                        NewValues.Add(property.Metadata.Name, property.CurrentValue);

                        if (property.IsModified &&
                            (property.CurrentValue != null || originalValue != null) &&
                            ((property.CurrentValue != null) && !property.CurrentValue.Equals(originalValue)))
                        {
                            ChangedColumns.Add(property.Metadata.Name);
                        }
                    }
                    break;
            }
        }

        public Audit PerformAfterSaveTasks()
        {
            switch (Action)
            {
                case EntityState.Added:
                    foreach (var property in EntityEntry.Properties)
                    {
                        if (property.Metadata.IsPrimaryKey())
                        {
                            EntityId = property.CurrentValue.ToString();
                        }

                        NewValues.Add(property.Metadata.Name, property.CurrentValue);
                    }
                    break;
                case EntityState.Modified:
                    foreach (var property in EntityEntry.Properties)
                    {
                        if (!property.IsTemporary || !TempProperties.ContainsKey(property.Metadata.Name)) continue;

                        NewValues.Add(property.Metadata.Name, property.CurrentValue);

                        var oldValue = TempProperties[property.Metadata.Name];

                        if ((property.CurrentValue != null || oldValue != null) &&
                            ((property.CurrentValue != null) && !property.CurrentValue.Equals(oldValue)))
                        {
                            ChangedColumns.Add(property.Metadata.Name);
                        }
                    }
                    break;
            }

            return GetAudit();
        }

        private Audit GetAudit()
        {
            return new Audit
            {
                TableName = TableName,
                Action = Action.ToString(),
                EntityId = EntityId,
                UserName = UserName,
                CreatedOn = DateTime.Now,
                ChangedColumns = ChangedColumns.Count > 0 ? string.Join(',', ChangedColumns) : null,
                NewData = NewValues.Count > 0 ? JsonConvert.SerializeObject(NewValues) : null,
                OldData = OldValues.Count > 0 ? JsonConvert.SerializeObject(OldValues) : null
            };
        }
    }
}

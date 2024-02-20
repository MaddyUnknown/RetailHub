using Inventory.Core.Entities.Common;
using Inventory.Infrastructure.Utility;

namespace Inventory.Infrastructure.Extensions
{
    internal static class EntityBaseExtension
    {
        internal static void SetId(this EntityBase entity, long id)
        {
            ReflectionUtil.SetPropertyValue<EntityBase, long>(entity, nameof(entity.Id), id);
        }

        internal static void SetCreationDate(this EntityBase entity, DateTime? creationDate)
        {
            ReflectionUtil.SetPropertyValue<EntityBase, DateTime?>(entity, nameof(entity.CreationDate), creationDate);
        }

        internal static void SetUpdateDate(this EntityBase entity, DateTime? updateDate)
        {
            ReflectionUtil.SetPropertyValue<EntityBase, DateTime?>(entity, nameof(entity.UpdateDate), updateDate);
        }

        internal static void SetCreatedBy(this EntityBase entity, string? createdBy)
        {
            ReflectionUtil.SetPropertyValue<EntityBase, string?>(entity, nameof(entity.CreatedBy), createdBy);
        }

        internal static void SetUpdatedBy(this EntityBase entity, string? updatedBy)
        {
            ReflectionUtil.SetPropertyValue<EntityBase, string?>(entity, nameof(entity.UpdatedBy), updatedBy);
        }
    }
}

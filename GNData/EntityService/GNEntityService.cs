using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.Security;
using GenomeNext.Data.Metadata.Audit;

namespace GenomeNext.Data
{
    public class GNEntityService<T> : BaseEntityService<T>
    {
        public new GNEntityModelContainer db
        {
            get
            {
                return (GNEntityModelContainer)base.db;
            }
            set
            {
                base.db = value;
            }
        }

        public GNEntityService()
            : base()
        {
        }

        public GNEntityService(GNEntityModelContainer db) : base(db)
        {
            base.db = db;
        }

        public override async Task<T> Find(GNContact userContact, params object[] keys)
        {
            T entity = await base.Find(userContact, keys);

            if (entity != null)
            {
                var props = entity.GetType().GetProperties().Select(p => p.Name);

                if (props.Contains("CreatedBy") && props.Contains("CreatedByContact"))
                {
                    object createdByGuidObj = entity.GetType().GetProperties()
                        .Where(p => p.Name == "CreatedBy")
                        .FirstOrDefault()
                        .GetValue(entity);

                    if(createdByGuidObj != null)
                    {
                        GNContact createdByContact = this.db.GNContacts.Find((Guid)createdByGuidObj);

                        if (createdByContact != null)
                        {
                            entity.GetType().GetProperties()
                                .Where(p => p.Name == "CreatedByContact")
                                .FirstOrDefault()
                                .SetValue(entity, createdByContact);
                        }
                    }
                }
                /*
                if (props.Contains("CreateDateTime"))
                {
                    entity.GetType().GetProperties()
                        .Where(p => p.Name == "CreateDateTime")
                        .FirstOrDefault()
                        .SetValue(entity, 
                            entity.GetType().GetProperties()
                                .Where(p => p.Name == "CreateDateTime")
                                .FirstOrDefault()
                                .GetValue(entity));
                }*/
            }

            return entity;
        }
    }
}

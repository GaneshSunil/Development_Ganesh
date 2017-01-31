using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.Security;
using GenomeNext.Utility;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace GenomeNext.Data
{
    public abstract class BaseEntityService<T>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /*
        public static readonly string EVENT_VIEW = "VIEW";
        public static readonly string EVENT_INSERT = "INSERT";
        public static readonly string EVENT_UPDATE = "UPDATE";
        public static readonly string EVENT_DELETE = "DELETE";
        public static readonly string EVENT_ARCHIVE = "ARCHIVE";
        public static readonly string EVENT_ADD = "ADD";
        public static readonly string EVENT_REMOVE = "REMOVE";
        */

        public DbContext db { get; set; }

        public BaseEntityService()
        {
        }

        public BaseEntityService(DbContext db)
        {
            this.db = db;
        }

        public virtual async Task<List<T>> FindAll(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, "Finding entitities ["+start+","+end+","+filters+"]");

            List<T> entities = await FindAll(start, end, filters);

            if (entities == null) LogUtil.Warn(logger, "entities is NULL");
            if (entities != null) LogUtil.Info(logger, "entities.Count = " + entities.Count);

            entities = EvalEntityListSecurity(userContact, entities);

            return entities;
        }
        
        public virtual async Task<List<T>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await default(Task<List<T>>);
        }

        public virtual async Task<T> Find(GNContact userContact, params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, "Finding entity");

            T entity = await Find(keys);

            if (userContact != null)
            {
                LogUtil.Info(logger, "Found entity");

                entity = EvalEntitySecurity(userContact, (T)entity);
            }
            else
            {
                LogUtil.Info(logger, "Entity not found");
            }

            return entity;
        }

        public virtual async Task<T> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await default(Task<T>);
        }

        //public bool ExistsInDatabase<T>(T obj) where T : class
        //{
        //    bool exists = false;
        //    var ctx = ((IObjectContextAdapter)db).ObjectContext;
        //    var entry = db.Entry(obj);
        //    if (entry.State != EntityState.Detached)
        //    {
        //        var set = ctx.CreateObjectSet<T>().EntitySet;
        //        var keyprop = set.ElementType.KeyMembers.First();
        //        //if the key is integer we can check based on if id > 0 or not
        //        if (keyprop.TypeUsage.EdmType.Name == "Int32")
        //        {
        //            int keyval = entry.CurrentValues.GetValue<int>(keyprop.Name);
        //            if (keyval > 0)
        //            {
        //                exists = true;
        //            }
        //        }
        //        else
        //        {
        //            var databasevalues = entry.GetDatabaseValues();
        //            if (databasevalues != null)
        //            {
        //                exists = true;
        //            }
        //        }
        //    }
        //    return exists;
        //}

        public virtual async Task<T> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            T result = default(T);

            try
            {
                var props = entity.GetType().GetProperties().Select(p => p.Name);

                if (props.Contains("Id"))
                {
                    var idProp = entity.GetType().GetProperty("Id");

                    if (idProp.PropertyType.Equals(typeof(Guid)) && (Guid)idProp.GetValue(entity) == Guid.Empty)
                    {
                        idProp.SetValue(entity, Guid.NewGuid());
                    }
                }
                
                if (props.Contains("CreateDateTime"))
                {
                    entity.GetType().GetProperties().Where(p => p.Name == "CreateDateTime").FirstOrDefault().SetValue(entity, DateTime.Now);
                }

                //var test = ExistsInDatabase(entity);
                

                db.Entry(entity).State = EntityState.Added;

                LogUtil.Info(logger, "Inserting entity");

                int resultVal = await db.SaveChangesAsync();

                if (resultVal != 0)
                {
                    result = (T)entity;
                }
                else
                {
                    return default(T);
                }

            }
            catch (Exception e1)
            {
                string friendlyErrorMsg = string.Empty;
                string errorMsg = "Unable to create item.";
                friendlyErrorMsg = errorMsg;
                errorMsg += GetSqlExceptionErrorMessage(e1);

                if (errorMsg.Contains("Violation of PRIMARY KEY constraint"))
                    errorMsg = friendlyErrorMsg + "Cannot insert duplicate item";

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger,e2.Message, e2);
                throw e2;
            }

            LogUtil.Info(logger, "Inserted entity");

            return result;        
        }

        public virtual async Task<T> Insert(GNContact userContact, object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            T result = default(T);

            entity = EvalEntitySecurity(userContact, (T)entity);

            var props = entity.GetType().GetProperties().Select(p => p.Name);

            if (!props.Contains("CanCreate") || (bool)entity.GetType().GetProperty("CanCreate").GetValue(entity))
            {
                LogUtil.Info(logger, "Inserting entity");

                if(props.Contains("CreatedBy"))
                {
                    entity.GetType().GetProperties().Where(p=>p.Name=="CreatedBy").FirstOrDefault().SetValue(entity,userContact.Id);
                }

                result = await Insert((T)entity);
            }
            else
            {
                Exception e = new Exception("Not allowed to create item.");
                LogUtil.Error(logger,e.Message, e);
                throw e;
            }

            LogUtil.Info(logger, "Inserted entity");

            return result;
        }

        public virtual async Task<T> Update(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            T result = default(T);

            try
            {
                db.Entry(entity).State = EntityState.Modified;

                LogUtil.Info(logger, "Updating entity");

                int resultVal = await db.SaveChangesAsync();

                if (resultVal != 0)
                {
                    result = (T)entity;
                }
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to update item.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            LogUtil.Info(logger, "Updated entity");

            return result;
        }

        public virtual async Task<T> Update(GNContact userContact, object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            T result = default(T);

            db.Entry(entity).State = EntityState.Modified;

            entity = EvalEntitySecurity(userContact, (T)entity);

            var props = entity.GetType().GetProperties().Select(p => p.Name);

            if (!props.Contains("CanEdit") || (bool)entity.GetType().GetProperty("CanEdit").GetValue(entity))
            {
                LogUtil.Info(logger, "Updating entity");

                result = await Update((T)entity);
            }
            else
            {
                Exception e = new Exception("Not allowed to update item.");
                LogUtil.Error(logger,e.Message, e);
                throw e;
            }

            LogUtil.Info(logger, "Updated entity");

            return result;
        }

        public virtual async Task<int> Delete(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            int result = 0;

            try
            {
                object entity = await Find(keys);

                db.Entry(entity).State = EntityState.Deleted;

                LogUtil.Info(logger, "Deleting entity");

                result = await db.SaveChangesAsync();
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to delete item.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger,e2.Message, e2);
                throw e2;
            }

            LogUtil.Info(logger, "Deleted entity");

            return result;
        }

        public virtual async Task<int> Delete(GNContact userContact, params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            int result = 0;

            object entity = await Find(keys);

            entity = EvalEntitySecurity(userContact, (T)entity);

            var props = entity.GetType().GetProperties().Select(p => p.Name);

            if (!props.Contains("CanDelete") || (bool)entity.GetType().GetProperty("CanDelete").GetValue(entity))
            {
                LogUtil.Info(logger, "Deleting entity");

                result = await Delete(keys);
            }
            else
            {
                Exception e = new Exception("Not allowed to delete item.");
                LogUtil.Error(logger,e.Message,e);
                throw e;
            }

            LogUtil.Info(logger, "Deleted entity");

            return result;
        }

        public virtual T EvalEntitySecurity(GNContact userContact, T entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (entity != null)
            {
                LogUtil.Info(logger, "Adding entity security info to entity");

                var props = entity.GetType().GetProperties().Select(p => p.Name);

                if (props.Contains("CanCreate"))
                {
                    entity.GetType().GetProperty("CanCreate").SetValue(entity, true);
                }
                if (props.Contains("CanView"))
                {
                    entity.GetType().GetProperty("CanView").SetValue(entity, true);
                }
                if (props.Contains("CanEdit"))
                {
                    entity.GetType().GetProperty("CanEdit").SetValue(entity, true);
                }
                if (props.Contains("CanDelete"))
                {
                    entity.GetType().GetProperty("CanDelete").SetValue(entity, true);
                }
            }

            LogUtil.Info(logger, "Added entity security info to entity");

            return entity;
        }

        public virtual List<T> EvalEntityListSecurity(GNContact userContact, List<T> entities)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (userContact != null && entities != null)
            {
                LogUtil.Info(logger,"Adding entity security info to entity list items");

                List<T> entitiesSecure = new List<T>();
                foreach (var entity in entities)
                {
                    entitiesSecure.Add(EvalEntitySecurity(userContact, entity));
                };

                LogUtil.Debug(logger,"entities.Count = " + entities.Count);
                LogUtil.Debug(logger,"entitiesSecure.Count = " + entitiesSecure.Count);

                entities = entitiesSecure;
            }

            if (userContact == null) LogUtil.Warn(logger, "userContact is NULL");

            LogUtil.Info(logger, "Added entity security info to entity list items");

            return entities;
        }


        protected static string GetSqlExceptionErrorMessage(Exception ex)
        {
            string errorMsg = "";

            if (typeof(SqlException) != ex.GetType())
            {
                while (ex != null && ex.GetType() != typeof(SqlException))
                {
                    ex = ex.InnerException;
                }
            }

            if (typeof(SqlException) == ex.GetType())
            {
                for (int i = 0; i < ((SqlException)ex).Errors.Count; i++)
                {
                    errorMsg += " " + ((SqlException)ex).Errors[i].Message + ".";
                }
            }

            return errorMsg;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CuplexApiCommon.BulkCopyModels;


namespace CuplexApiCommon.GeoIP.BoModels
{
    public class BulkCopyItemBo : IBulkCopyItem
    {
        public BulkCopyItemBo(object bulkCopyObjSource)
        {
            var properties = bulkCopyObjSource.GetType().GetProperties();
            List<object> objList = new List<object>();

            foreach (PropertyInfo property in properties)
            {
                Type pType = property.PropertyType;
                if (pType == typeof(String) || (pType.Namespace != null && (pType.Namespace.StartsWith("System") && pType.IsPublic && !pType.HasElementType && !((IList)pType.GetInterfaces()).Contains(typeof(IEnumerable)))))
                    objList.Add(property.GetValue(bulkCopyObjSource));
            }
            RowObjects = objList;
        }
        public IEnumerable<object> RowObjects { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace MyStore.DataAccess
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Extension method for adding in a bunch of parameters
        /// </summary>
        public static void AddParams(this DbCommand cmd, params object[] args)
        {
            IDictionary<string, object> paramDict;
            if (args.Length == 1 && (paramDict = args[0] as IDictionary<string, object>) != null)
            {
                foreach (var key in paramDict.Keys)
                {
                    AddParam(cmd, key, paramDict[key]);
                }
            }
            else
            {
                for (var i = 0; i < args.Length; i++)
                {
                    AddParam(cmd, i.ToString(), args[i]);
                }
            }


        }
        /// <summary>
        /// Extension for adding single parameter
        /// </summary>
        public static void AddParam(this DbCommand cmd, string name, object item)
        {
            DbParameter p;
            var usrParam = item as DbParameter;
            if (usrParam != null)
            {
                p = usrParam;
            }
            else
            {
                p = cmd.CreateParameter();
                p.ParameterName = string.Format("@{0}", name);
                if (item == null)
                {
                    p.Value = DBNull.Value;
                    cmd.Parameters.Add(p);
                }
                else if (item.GetType() == typeof(Guid))
                {
                    p.Value = item.ToString();
                    p.DbType = DbType.String;
                    p.Size = 4000;
                }
                else if (item.GetType() == typeof(ExpandoObject))
                {
                    var d = (IDictionary<string, object>)item;
                    p.Value = d.Values.FirstOrDefault();
                }
                else if (item.GetType() == typeof(string))
                {
                    p.Size = ((string)item).Length > 4000 ? -1 : 4000;
                    p.Value = item;
                }
                else
                {
                    p.Value = item;
                }
            }
            cmd.Parameters.Add(p);
        }
        /// <summary>
        /// Report to table
        /// </summary>
        /// <param name="rdr">Data reader</param>
        /// <returns>Datatable</returns>
        public static DataTable ToDataTable(this IDataReader rdr)
        {
            var result = new DataTable();
            result.Load(rdr);
            return result;
        }

        /// <summary>
        /// Turns an IDataReader to a Dynamic list of things
        /// </summary>
        public static List<ExpandoObject> ToExpandoList(this IDataReader rdr)
        {
            var result = new List<ExpandoObject>();
            while (rdr.Read())
            {
                result.Add(rdr.RecordToExpando());
            }
            return result;
        }
        /// <summary>
        /// Records to expando.
        /// </summary>
        /// <param name="rdr">The RDR.</param>
        /// <returns></returns>
        public static dynamic RecordToExpando(this IDataReader rdr)
        {
            var e = new ExpandoObject();
            var d = e as IDictionary<string, object>;
            for (int i = 0; i < rdr.FieldCount; i++)
                d.Add(rdr.GetName(i), DBNull.Value.Equals(rdr[i]) ? null : rdr[i]);
            return e;
        }
        /// <summary>
        /// Turns the object into an ExpandoObject
        /// </summary>
        public static dynamic ToExpando(this object o)
        {
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary
            if (o.GetType() == typeof(ExpandoObject)) return o; //shouldn't have to... but just in case
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }
        /// <summary>
        /// Turns the object into a Dictionary
        /// </summary>
        public static IDictionary<string, object> ToDictionary(this object thingy)
        {
            return (IDictionary<string, object>)thingy.ToExpando();
        }
    }
}

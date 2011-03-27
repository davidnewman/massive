using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Massive;

namespace Massive.Ext
{
    public static class DynamicModelExtensions
    {
        /// <summary>
        /// Adds a simple query to from.Queries that fetches all objects related to an instance of from
        /// </summary>
        /// <param name="from">model on the 'one-side' of the relationship</param>
        /// <param name="to">model on the 'many-side' of the relationship</param>
        /// <param name="foreignKey">name of the foreign key column in 'to', defaults to string.format("{0}{1}", from.TableName, from.PrimaryKeyField)</param>
        /// <param name="propertyName">name of desired property on result object, defaults to a plural form of to.TableName</param>
        public static void OneToMany(this DynamicModel from, DynamicModel to, string foreignKey = "", string propertyName = "")
        {
            var fk = string.IsNullOrEmpty(foreignKey) ? string.Format("{0}{1}", from.TableName, from.PrimaryKeyField) : foreignKey;
            var p = string.IsNullOrEmpty(propertyName) ? Pluralize(to.TableName) : propertyName;
            var w = string.Format("{0} = @0", fk);

            from.Prototype[p] = (Func<dynamic, IEnumerable<dynamic>>)(m => to.All(where: w, args: new object[] { ((IDictionary<string, object>)m)[from.PrimaryKeyField] }));
        }

        /// <summary>
        /// Simple implementation of a pluralizer. If the string ends in:
        /// y - y is replaced with ies
        /// o|s|z|ch|sh|x - es is appended
        /// anything else - s is appended
        /// 
        /// This doesn't attempts to detect cases like man -> men, and obviously doesn't handle all instances of 'o' properly.
        /// Also, this method is really only intended to work on a single word, however whitespace is trimmed on both ends.
        /// </summary>
        /// <param name="s">string to pluralize</param>
        /// <returns>pluralize string</returns>
        private static string Pluralize(string s)
        {
            var ps = string.Empty;
            s = s.Trim();

            if (!string.IsNullOrEmpty(s))
            {
                Match ending = null;
                if (s.EndsWith("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    ps = string.Format("{0}ies", s.Substring(0, s.Length - 1));
                }
                else if ((ending = Regex.Match(s, "(?<ending>o|s|z|ch|sh|x)$")).Success)
                {
                    ps = string.Format("{0}es", s);
                }
                else
                {
                    ps = string.Format("{0}s", s);
                }
            }

            return ps;
        }
    }
}

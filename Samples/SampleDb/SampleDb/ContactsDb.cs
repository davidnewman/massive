using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Massive;
using Massive.Ext;

namespace SampleDb
{
    public class ContactsDb
    {
        protected string ConnectionStringName { get; set; }
        protected DynamicModel Stub { get; set; }
        public DynamicModel Contact { get; set; }
        public DynamicModel Address { get; set; }

        public ContactsDb(string connectionStringName)
        {
            ConnectionStringName = connectionStringName;

            Stub = new DynamicModel(ConnectionStringName);
            Contact = new DynamicModel(ConnectionStringName, "Contact");
            Address = new DynamicModel(ConnectionStringName, "Address");

            //creates a one-to-many replationship called Addresses on all "Contact" records
            Contact.OneToMany(Address);
        }

        public IEnumerable<dynamic> Query(string sql, params object[] args)
        {
            return Stub.Query(sql, args);
        }
    }
}

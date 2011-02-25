using System;

namespace Logistikcenter.Domain
{
    public class Destination
    {
        private Int64 _id;
        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Destination()
        {
        }

        public Destination(Int64 id)
        {
            this.Id = id;
        }


        public Destination(string name)
        {
            this.Name = name;
        }

        public Destination(Int64 id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

     
    }
}

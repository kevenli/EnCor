using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.Security
{
    public class UserIdentity
    {
        private string _Id;
        private string _Name;

        public UserIdentity()
        {
        }

        public UserIdentity(string id, string name)
        {
            _Id = id;
            _Name = name;
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public string Id
        {
            get
            {
                return _Id;
            }
        }
    }
}

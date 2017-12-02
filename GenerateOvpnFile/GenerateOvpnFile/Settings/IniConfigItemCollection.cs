using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GenerateOvpnFile.Settings
{
    public class IniConfigItemCollection 
    {
        private readonly Dictionary<string, string> _dictionary;
        public IniConfigItemCollection()
        {
            _dictionary= new Dictionary<string, string>();
        }

        public string this[string key]
        {
            get
            {
                if (_dictionary.ContainsKey(key))
                {
                    return _dictionary[key];
                }

                return null;
            }
            set
            {
                if (!_dictionary.ContainsKey(key))
                {
                    _dictionary.Add(key, value);
                }
            }
        }

        public void Add(string key, string value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key,value);
            }
        }

    //public  string []
    //    {
    //        get
    //        {
    //            if (ContainsKey(key))
    //            {
    //                return base.Values[key];
    //            }

        //        }

        //        set => this.Add(key,value);
        //    }

        public IEnumerable<string> AllKeys
        {
            get
            {
                return _dictionary.Keys;
            }
        }


        public IEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Clear()
        {
            _dictionary.Clear();
        }
    }
}

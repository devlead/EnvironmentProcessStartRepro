using System;
using System.Collections;
using System.Collections.Specialized;

namespace EnvironmentProcessStartRepro
{
    public class CaseSensitiveStringDictionary : StringDictionary
    {
        private Hashtable Internalcontents => this.GetFieldValue<Hashtable>("contents");

        public override string this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                return (string)Internalcontents[key];
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                Internalcontents[key] = value;
            }
        }

        public override void Add(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Internalcontents.Add(key, value);
        }

        public override bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Internalcontents.ContainsKey(key);
        }

        public override void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Internalcontents.Remove(key);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Util
{
    internal class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        private Dictionary<string, object> _dictionary;
        private string _key;
        private string _category;
        private bool _isReadOnly;
        public override string Category
        {
            get
            {
                return this._category;
            }
        }
        public override Type PropertyType
        {
            get
            {
                return this._dictionary[this._key].GetType();
            }
        }
        public override bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
        }
        public override Type ComponentType
        {
            get
            {
                return null;
            }
        }
        internal DictionaryPropertyDescriptor(Dictionary<string, object> d, string key, string category, bool isReadOnly)
            : base(key.ToString(), null)
        {
            this._dictionary = d;
            this._key = key;
            this._category = category;
            this._isReadOnly = isReadOnly;
        }
        public override void SetValue(object component, object value)
        {
            this._dictionary[this._key] = value;
        }
        public override object GetValue(object component)
        {
            return this._dictionary[this._key];
        }
        public override bool CanResetValue(object component)
        {
            return false;
        }
        public override void ResetValue(object component)
        {
        }
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}

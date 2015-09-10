using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Util
{
    public class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        private Dictionary<string, Dictionary<string, object>> properties = new Dictionary<string, Dictionary<string, object>>();
        private Dictionary<string, bool> readOnly = new Dictionary<string, bool>();
        public void Add(string category, Dictionary<string, object> property)
        {
            this.properties.Add(category, property);
            this.readOnly.Add(category, false);
        }
        public void Add(string category, Dictionary<string, object> property, bool isReadOnly)
        {
            this.properties.Add(category, property);
            this.readOnly.Add(category, isReadOnly);
        }
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<DictionaryPropertyDescriptor> list = new List<DictionaryPropertyDescriptor>();
            foreach (string current in this.properties.Keys)
            {
                Dictionary<string, object> dictionary = this.properties[current];
                foreach (string current2 in dictionary.Keys)
                {
                    list.Add(new DictionaryPropertyDescriptor(dictionary, current2, current, this.readOnly[current]));
                }
            }
            PropertyDescriptor[] array = list.ToArray();
            return new PropertyDescriptorCollection(array);
        }
    }
}

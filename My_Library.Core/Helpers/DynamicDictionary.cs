using System.Collections.Generic;
using System.Dynamic;

namespace My_Library.Core.Helpers
{
    public class DynamicDictionary<TValue> : DynamicObject
    {
        readonly IDictionary<string, TValue> _dictionary;

        public DynamicDictionary(IDictionary<string, TValue> dict)
        {
            _dictionary = dict;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = (TValue)value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            TValue value;
            var r = _dictionary.TryGetValue(binder.Name, out value);
            result = value;
            return r;
        }
    }
}

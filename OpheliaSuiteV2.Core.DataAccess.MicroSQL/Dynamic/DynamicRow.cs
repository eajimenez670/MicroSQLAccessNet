using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL.Dynamic {
    internal sealed class DynamicRow : IDynamicMetaObjectProvider, IDictionary<string, object> {

        #region Members

        private readonly FieldNames table;
        private object[] values;

        #endregion

        #region Builders


        public DynamicRow() : this(new FieldNames(new string[] { }), new object[] { }) { }

        public DynamicRow(FieldNames table, object[] values) {
            this.table = table ?? throw new ArgumentNullException(nameof(table));
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        #endregion

        #region Methods

        public DynamicMetaObject GetMetaObject(Expression parameter) {
            return new DynamicRowMetaObject(parameter, System.Dynamic.BindingRestrictions.Empty, this);
        }

        public bool Remove(string key) {
            int index = table.IndexOfName(key);
            if (index < 0 || index >= values.Length || values[index] is DeadValue)
                return false;
            values[index] = DeadValue.Default;
            return true;
        }

        public bool Remove(KeyValuePair<string, object> item) {
            IDictionary<string, object> dic = this;
            return dic.Remove(item.Key);
        }

        public bool TryGetValue(string key, out object value) {
            var index = table.IndexOfName(key);
            if (index < 0) { // No existe
                value = null;
                return false;
            }
            // Existe
            value = index < values.Length ? values[index] : null;
            if (value is DeadValue) {
                value = null;
                return false;
            }
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public object this[string key] {
            get {
                TryGetValue(key, out object val);
                return val;
            }
            set {
                SetValue(key, value, false);
            }
        }

        public ICollection<string> Keys {
            get {
                return this.Select(kv => kv.Key).ToArray();
            }
        }

        public ICollection<object> Values {
            get {
                return this.Select(kv => kv.Value).ToArray();
            }
        }

        public int Count {
            get {
                int count = 0;
                for (int i = 0; i < values.Length; i++) {
                    if (!(values[i] is DeadValue))
                        count++;
                }
                return count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(string key, object value) {
            SetValue(key, value, true);
        }

        public void Add(KeyValuePair<string, object> item) {
            IDictionary<string, object> dic = this;
            dic.Add(item.Key, item.Value);
        }

        public void Clear() {
            for (int i = 0; i < values.Length; i++)
                values[i] = DeadValue.Default;
        }

        public bool Contains(KeyValuePair<string, object> item) {
            return TryGetValue(item.Key, out object value) && Equals(value, item.Value);
        }

        public bool ContainsKey(string key) {
            int index = table.IndexOfName(key);
            if (index < 0 || index >= values.Length || values[index] is DeadValue)
                return false;
            return true;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
            foreach (var kv in this) {
                array[arrayIndex++] = kv;
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            var names = table.Fields;
            for (var i = 0; i < names.Length; i++) {
                object value = i < values.Length ? values[i] : null;
                if (!(value is DeadValue)) {
                    yield return new KeyValuePair<string, object>(names[i], value);
                }
            }
        }

        public object SetValue(string key, object value) {
            return SetValue(key, value, false);
        }

        private object SetValue(string key, object value, bool isAdd) {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            int index = table.IndexOfName(key);
            if (index < 0) {
                index = table.AddField(key);
            } else if (isAdd && index < values.Length && !(values[index] is DeadValue)) {
                // Semanticamente el valor ya existe
                throw new ArgumentException("An item with the same key has already been added", nameof(key));
            }
            int oldLength = values.Length;
            if (oldLength <= index) {
                Array.Resize(ref values, table.FieldCount);
                for (int i = oldLength; i < values.Length; i++) {
                    values[i] = DeadValue.Default;
                }
            }
            return values[index] = value;
        }

        #endregion

        #region Classes

        private sealed class DeadValue {
            public static readonly DeadValue Default = new DeadValue();
            private DeadValue() { /* Oculta el constructor */
            }
        }

        #endregion
    }
}

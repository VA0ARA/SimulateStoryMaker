using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Timeline.Helper;
using Timeline.Model.Interfaces;

namespace Timeline.Model
{
    public class KeyValues : IValueAtTime , IKeyFinder
    {
        private SortedList<int,int> _indexes { get; }

        private ObservableCollection<Tuple<int, KeyValue>> _observable { get; }

        public ReadOnlyObservableCollection<Tuple<int, KeyValue>> SortedDictionary { get; }

        private int _lastKey;
        public int LastKey => _lastKey;

        public KeyValues()
        {
            _indexes = new SortedList<int,int>();
            _observable = new ObservableCollection<Tuple<int, KeyValue>>();
            SortedDictionary = new ReadOnlyObservableCollection<Tuple<int, KeyValue>>(_observable);
        }

        public void Add(int frame , object value)
        {
            Add(new KeyValue(frame, value));
        }

        public void Add(KeyValue keyValue)
        {
            if (!_indexes.ContainsKey(keyValue.Frame))
            {
                _indexes.Add(keyValue.Frame, keyValue.Frame);
                _observable.Insert(_indexes.IndexOfKey(keyValue.Frame), new Tuple<int, KeyValue>(keyValue.Frame, keyValue));
                if (keyValue.Frame > _lastKey)
                    _lastKey = keyValue.Frame;
            }
            else
            {
                _observable[_indexes.IndexOfKey(keyValue.Frame)] = new Tuple<int, KeyValue>(keyValue.Frame, keyValue);
            }
        }

        public void Remove(int frame)
        {
            if (_indexes.ContainsKey(frame))
            {
                _observable.RemoveAt(_indexes.IndexOfKey(frame));

                _indexes.Remove(frame);

                if (frame == _lastKey)
                    _lastKey = KeyFrameHelper.GetPreviousKey(_indexes.Keys, frame, false);
            }
        }

        public void Remove(KeyValue keyValue)
        {
            Remove(keyValue.Frame);
        }

        public bool ContainsKeyAtFrame(int frame)
        {
            return _indexes.ContainsValue(frame);
        }

        public KeyValue GetKeyValue(int frame)
        {
            if (!_indexes.ContainsKey(frame)) return null;
            var valueIndex = _indexes.IndexOfKey(frame);
            return _observable[valueIndex].Item2;
        }

        public object GetValue(float t)
        {
            if (SortedDictionary.Count == 0) return null;

            if (Math.Abs(t - Math.Round(t)) <= 0 && _indexes.ContainsValue((int) t))
                return _observable[_indexes.IndexOfKey((int) t)].Item2.Val;

            KeyFrameHelper.GetKeys(_indexes.Keys, t, out var beforeKey, out var afterKey , true);

            if (beforeKey == -1 && afterKey == -1) return null;

            if (!_indexes.ContainsKey(beforeKey) || Math.Abs(t - afterKey) <= 0) return _observable[_indexes.IndexOfKey(afterKey)].Item2.Val;
            if (!_indexes.ContainsKey(afterKey) || Math.Abs(t - beforeKey) <= 0) return _observable[_indexes.IndexOfKey(beforeKey)].Item2.Val;

            var timeBetweenToKey = (t - beforeKey) / (afterKey - beforeKey);

            var per = _observable[_indexes.IndexOfKey(beforeKey)].Item2.Val;
            var next = _observable[_indexes.IndexOfKey(afterKey)].Item2.Val;

            return Lerp(per, next, timeBetweenToKey);
        }

        private static object Lerp(object a, object b, float t)
        {
            if (a.GetType() != b.GetType()) return null;

            var type = a.GetType();

            if (type == typeof(bool))
                return a;

            if (IsNumericType(a))
            {
                double.TryParse(a.ToString(), out var a1);
                double.TryParse(b.ToString(), out var b1);
                return Convert.ChangeType(Lerp(a1, b1, t), type);
            }

            if (!type.IsValueType) return a;

            var obj = Activator.CreateInstance(type);

            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.CanWrite) continue;
                var lerpedValue = Lerp(propertyInfo.GetValue(a), propertyInfo.GetValue(b), t);
                propertyInfo.SetValue(obj, lerpedValue);
            }

            return obj;
        }

        private static double Lerp(double a, double b, float t)
        {
            if (t < 0)
                return a;

            if (t > 1)
                return b;

            var result = (1 - t) * a + t * b;

            return result;
        }

        private static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public int PreviousKey(int key, bool containsSelf)
        {
            return KeyFrameHelper.GetPreviousKey(_indexes.Keys, key, containsSelf);
        }

        public int NextKey(int key, bool containsSelf)
        {
            return KeyFrameHelper.GetNextKey(_indexes.Keys, key, containsSelf);
        }
    }
}
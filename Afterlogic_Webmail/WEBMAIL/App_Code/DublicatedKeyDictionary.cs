using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;

namespace WebMail
{
	public class DublicatedKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		protected object _syncRoot = new object();
		protected List<KeyValuePair<TKey, TValue>> _list = new List<KeyValuePair<TKey, TValue>>();
		protected IComparer<TKey> _comparer = null;

		public DublicatedKeyDictionary() { }

		public DublicatedKeyDictionary(IComparer<TKey> comparer)
		{
			_comparer = comparer;
		}

		#region IDictionary<TKey,TValue> Members

		public void Add(TKey key, TValue value)
		{
			_list.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		public bool ContainsKey(TKey key)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in _list)
			{
				if ((_comparer != null) ? (_comparer.Compare(kvp.Key, key) == 0) : kvp.Key.Equals(key))
				{
					return true;
				}
				if (kvp.Key.Equals(key)) return true;
			}
			return false;
		}

		public ICollection<TKey> Keys
		{
			get
			{
				List<TKey> result = new List<TKey>();
				foreach (KeyValuePair<TKey, TValue> pair in _list)
				{
					result.Add(pair.Key);
				}
				return result;
			}
		}

		public bool Remove(TKey key)
		{
			bool result = false;
			for (int i = 0; i < _list.Count; i++)
			{
				KeyValuePair<TKey, TValue> pair = _list[i];
				if ((_comparer != null) ? (_comparer.Compare(pair.Key, key) == 0) : pair.Key.Equals(key))
				{
					_list.RemoveAt(i);
					i--;
					result = true;
				}
			}
			return result;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			value = default(TValue);
			bool result = false;
			foreach (KeyValuePair<TKey, TValue> pair in _list)
			{
				if ((_comparer != null) ? (_comparer.Compare(pair.Key, key) == 0) : pair.Key.Equals(key))
				{
					value = pair.Value;
					result = true;
					break;
				}
			}
			return result;
		}

		public ICollection<TValue> Values
		{
			get
			{
				List<TValue> result = new List<TValue>();
				foreach (KeyValuePair<TKey, TValue> pair in _list)
				{
					result.Add(pair.Value);
				}
				return result;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				KeyValuePair<TKey, TValue> result = _list.Find(
delegate(KeyValuePair<TKey, TValue> pair) { return (_comparer != null) ? (_comparer.Compare(pair.Key, key) == 0) : pair.Key.Equals(key); }
					);
				return result.Value;
			}
			set
			{
				int index = _list.FindIndex(
delegate(KeyValuePair<TKey, TValue> pair) { return (_comparer != null) ? (_comparer.Compare(pair.Key, key) == 0) : pair.Key.Equals(key); }
					);
				_list[index] = new KeyValuePair<TKey, TValue>(key, value);
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		public virtual void Add(KeyValuePair<TKey, TValue> item)
		{
			_list.Add(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item);
		}

		public virtual void Clear()
		{
			_list.Clear();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			Clear();
		}

		public virtual bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return _list.Contains(item);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return Contains(item);
		}

		public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			CopyTo(array, arrayIndex);
		}

		public virtual int Count
		{
			get { return _list.Count; }
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count
		{
			get { return Count; }
		}

		public virtual bool IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return IsReadOnly; }
		}

		public virtual bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return _list.Remove(item);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return Remove(item);
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		public List<TValue> GetValuesByKey(TKey key)
		{
			List<TValue> result = new List<TValue>();
			int startIndex = 0;
			while (true)
			{
				startIndex = _list.FindIndex(startIndex,
	delegate(KeyValuePair<TKey, TValue> pair) { return (_comparer != null) ? (_comparer.Compare(pair.Key, key) == 0) : pair.Key.Equals(key); }
					);
				if (startIndex < 0) break;

				result.Add(_list[startIndex].Value);
				startIndex++;
			}
			return result;
		}

	}

}

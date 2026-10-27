using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200140D RID: 5133
public static class DictionaryExtension
{
	// Token: 0x0600A0A1 RID: 41121 RVA: 0x003C8794 File Offset: 0x003C6994
	public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TValue, bool> predicate)
	{
		foreach (TKey tkey in dic.Keys.Where<TKey>((TKey k) => predicate(dic[k])).ToList<TKey>())
		{
			dic.Remove(tkey);
		}
	}

	// Token: 0x0600A0A2 RID: 41122 RVA: 0x003C881C File Offset: 0x003C6A1C
	public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey, bool> predicate)
	{
		foreach (TKey tkey in dic.Keys.Where<TKey>((TKey k) => predicate(k)).ToList<TKey>())
		{
			dic.Remove(tkey);
		}
	}

	// Token: 0x0600A0A3 RID: 41123 RVA: 0x003C8894 File Offset: 0x003C6A94
	public static void CopyTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, IDictionary<TKey, TValue> _dest, bool _overwriteExisting = false)
	{
		if (_overwriteExisting)
		{
			using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = _src.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
					_dest[keyValuePair.Key] = keyValuePair.Value;
				}
				return;
			}
		}
		foreach (KeyValuePair<TKey, TValue> keyValuePair2 in _src)
		{
			_dest.Add(keyValuePair2.Key, keyValuePair2.Value);
		}
	}

	// Token: 0x0600A0A4 RID: 41124 RVA: 0x003C8930 File Offset: 0x003C6B30
	public static void CopyKeysTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, ICollection<TKey> _dest)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest.Add(keyValuePair.Key);
		}
	}

	// Token: 0x0600A0A5 RID: 41125 RVA: 0x003C8980 File Offset: 0x003C6B80
	public static void CopyKeysTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, TKey[] _dest)
	{
		if (_dest.Length != _src.Count)
		{
			throw new ArgumentOutOfRangeException("_dest", "Target array does not have the same size as the dictionary");
		}
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest[num++] = keyValuePair.Key;
		}
	}

	// Token: 0x0600A0A6 RID: 41126 RVA: 0x003C89F0 File Offset: 0x003C6BF0
	public static void CopyValuesTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, IList<TValue> _dest)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest.Add(keyValuePair.Value);
		}
	}

	// Token: 0x0600A0A7 RID: 41127 RVA: 0x003C8A40 File Offset: 0x003C6C40
	public static void CopyValuesTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, TValue[] _dest)
	{
		if (_dest.Length != _src.Count)
		{
			throw new ArgumentOutOfRangeException("_dest", "Target array does not have the same size as the dictionary");
		}
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest[num++] = keyValuePair.Value;
		}
	}

	// Token: 0x0600A0A8 RID: 41128 RVA: 0x003C8AB0 File Offset: 0x003C6CB0
	public static bool ValuesEquals<TKey, TValue>(this IDictionary<TKey, TValue> _src, IDictionary<TKey, TValue> _other)
	{
		if (_src.Count != _other.Count)
		{
			return false;
		}
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			TValue tvalue;
			if (!_other.TryGetValue(keyValuePair.Key, out tvalue))
			{
				return false;
			}
			if (!tvalue.Equals(keyValuePair.Value))
			{
				return false;
			}
		}
		return true;
	}
}

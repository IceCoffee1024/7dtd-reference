using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020013DB RID: 5083
public static class DictionaryExtension
{
	// Token: 0x06009F9E RID: 40862 RVA: 0x003C17F4 File Offset: 0x003BF9F4
	public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TValue, bool> predicate)
	{
		foreach (TKey tkey in dic.Keys.Where<TKey>((TKey k) => predicate(dic[k])).ToList<TKey>())
		{
			dic.Remove(tkey);
		}
	}

	// Token: 0x06009F9F RID: 40863 RVA: 0x003C187C File Offset: 0x003BFA7C
	public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey, bool> predicate)
	{
		foreach (TKey tkey in dic.Keys.Where<TKey>((TKey k) => predicate(k)).ToList<TKey>())
		{
			dic.Remove(tkey);
		}
	}

	// Token: 0x06009FA0 RID: 40864 RVA: 0x003C18F4 File Offset: 0x003BFAF4
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

	// Token: 0x06009FA1 RID: 40865 RVA: 0x003C1990 File Offset: 0x003BFB90
	public static void CopyKeysTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, ICollection<TKey> _dest)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest.Add(keyValuePair.Key);
		}
	}

	// Token: 0x06009FA2 RID: 40866 RVA: 0x003C19E0 File Offset: 0x003BFBE0
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

	// Token: 0x06009FA3 RID: 40867 RVA: 0x003C1A50 File Offset: 0x003BFC50
	public static void CopyValuesTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, IList<TValue> _dest)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest.Add(keyValuePair.Value);
		}
	}

	// Token: 0x06009FA4 RID: 40868 RVA: 0x003C1AA0 File Offset: 0x003BFCA0
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

	// Token: 0x06009FA5 RID: 40869 RVA: 0x003C1B10 File Offset: 0x003BFD10
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

using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020013FB RID: 5115
public static class DictionaryExtension
{
	// Token: 0x0600A049 RID: 41033 RVA: 0x003C6390 File Offset: 0x003C4590
	public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TValue, bool> predicate)
	{
		foreach (TKey tkey in dic.Keys.Where<TKey>((TKey k) => predicate(dic[k])).ToList<TKey>())
		{
			dic.Remove(tkey);
		}
	}

	// Token: 0x0600A04A RID: 41034 RVA: 0x003C6418 File Offset: 0x003C4618
	public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey, bool> predicate)
	{
		foreach (TKey tkey in dic.Keys.Where<TKey>((TKey k) => predicate(k)).ToList<TKey>())
		{
			dic.Remove(tkey);
		}
	}

	// Token: 0x0600A04B RID: 41035 RVA: 0x003C6490 File Offset: 0x003C4690
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

	// Token: 0x0600A04C RID: 41036 RVA: 0x003C652C File Offset: 0x003C472C
	public static void CopyKeysTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, ICollection<TKey> _dest)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest.Add(keyValuePair.Key);
		}
	}

	// Token: 0x0600A04D RID: 41037 RVA: 0x003C657C File Offset: 0x003C477C
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

	// Token: 0x0600A04E RID: 41038 RVA: 0x003C65EC File Offset: 0x003C47EC
	public static void CopyValuesTo<TKey, TValue>(this IDictionary<TKey, TValue> _src, IList<TValue> _dest)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in _src)
		{
			_dest.Add(keyValuePair.Value);
		}
	}

	// Token: 0x0600A04F RID: 41039 RVA: 0x003C663C File Offset: 0x003C483C
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

	// Token: 0x0600A050 RID: 41040 RVA: 0x003C66AC File Offset: 0x003C48AC
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

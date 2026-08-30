using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014A5 RID: 5285
public class StringSpanDictionary<T> : IDictionary<string, T>, ICollection<KeyValuePair<string, T>>, IEnumerable<KeyValuePair<string, T>>, IEnumerable, IReadOnlyDictionary<string, T>, IReadOnlyCollection<KeyValuePair<string, T>>
{
	// Token: 0x0600A5D8 RID: 42456 RVA: 0x003DBB5C File Offset: 0x003D9D5C
	public StringSpanDictionary()
		: this(new Dictionary<string, T>())
	{
	}

	// Token: 0x0600A5D9 RID: 42457 RVA: 0x003DBB69 File Offset: 0x003D9D69
	public StringSpanDictionary(IDictionary<string, T> dict)
	{
		this.m_dict = dict;
		this.m_hashToKeys = new Dictionary<int, List<string>>();
	}

	// Token: 0x0600A5DA RID: 42458 RVA: 0x003DBB83 File Offset: 0x003D9D83
	[PublicizedFrom(EAccessModifier.Private)]
	public static int GenerateHash(StringSpan key)
	{
		return key.GetHashCode();
	}

	// Token: 0x0600A5DB RID: 42459 RVA: 0x003DBB94 File Offset: 0x003D9D94
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddHash(string key)
	{
		int num = StringSpanDictionary<T>.GenerateHash(key);
		List<string> list;
		if (!this.m_hashToKeys.TryGetValue(num, out list))
		{
			list = new List<string>();
			this.m_hashToKeys.Add(num, list);
		}
		using (List<string>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == key)
				{
					return;
				}
			}
		}
		list.Add(key);
	}

	// Token: 0x0600A5DC RID: 42460 RVA: 0x003DBC1C File Offset: 0x003D9E1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveHash(string key)
	{
		int num = StringSpanDictionary<T>.GenerateHash(key);
		List<string> list;
		if (!this.m_hashToKeys.TryGetValue(num, out list))
		{
			return;
		}
		list.Remove(key);
		if (list.Count > 0)
		{
			return;
		}
		this.m_hashToKeys.Remove(num);
	}

	// Token: 0x0600A5DD RID: 42461 RVA: 0x003DBC68 File Offset: 0x003D9E68
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetStringFromHashedKeys(StringSpan spanKey, out string stringKey)
	{
		int num = StringSpanDictionary<T>.GenerateHash(spanKey);
		List<string> list;
		if (!this.m_hashToKeys.TryGetValue(num, out list))
		{
			stringKey = null;
			return false;
		}
		foreach (string text in list)
		{
			if (!(text != spanKey))
			{
				stringKey = text;
				return true;
			}
		}
		stringKey = null;
		return false;
	}

	// Token: 0x0600A5DE RID: 42462 RVA: 0x003DBCE8 File Offset: 0x003D9EE8
	public void Add(StringSpan key, T value)
	{
		string text;
		if (!this.TryGetStringFromHashedKeys(key, out text))
		{
			this.Add(key.ToString(), value);
			return;
		}
		this.Add(text, value);
	}

	// Token: 0x0600A5DF RID: 42463 RVA: 0x003DBD20 File Offset: 0x003D9F20
	public bool ContainsKey(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.ContainsKey(text);
	}

	// Token: 0x0600A5E0 RID: 42464 RVA: 0x003DBD44 File Offset: 0x003D9F44
	public bool Remove(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.Remove(text);
	}

	// Token: 0x0600A5E1 RID: 42465 RVA: 0x003DBD68 File Offset: 0x003D9F68
	public bool TryGetValue(StringSpan key, out T value)
	{
		string text;
		if (!this.TryGetStringFromHashedKeys(key, out text))
		{
			value = default(T);
			return false;
		}
		return this.TryGetValue(text, out value);
	}

	// Token: 0x17001356 RID: 4950
	public T this[StringSpan key]
	{
		get
		{
			string text;
			if (!this.TryGetStringFromHashedKeys(key, out text))
			{
				throw new KeyNotFoundException();
			}
			return this[text];
		}
		set
		{
			string text;
			if (!this.TryGetStringFromHashedKeys(key, out text))
			{
				this[key.ToString()] = value;
				return;
			}
			this[text] = value;
		}
	}

	// Token: 0x0600A5E4 RID: 42468 RVA: 0x003DBDF1 File Offset: 0x003D9FF1
	public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A5E5 RID: 42469 RVA: 0x003DBDF1 File Offset: 0x003D9FF1
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A5E6 RID: 42470 RVA: 0x003DBDFE File Offset: 0x003D9FFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.Add(KeyValuePair<string, T> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A5E7 RID: 42471 RVA: 0x003DBE14 File Offset: 0x003DA014
	public void Clear()
	{
		this.m_dict.Clear();
		this.m_hashToKeys.Clear();
	}

	// Token: 0x0600A5E8 RID: 42472 RVA: 0x003DBE2C File Offset: 0x003DA02C
	public bool Contains(KeyValuePair<string, T> item)
	{
		return this.m_dict.Contains(item);
	}

	// Token: 0x0600A5E9 RID: 42473 RVA: 0x003DBE3A File Offset: 0x003DA03A
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
	{
		this.m_dict.CopyTo(array, arrayIndex);
	}

	// Token: 0x0600A5EA RID: 42474 RVA: 0x003DBE49 File Offset: 0x003DA049
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.Remove(KeyValuePair<string, T> item)
	{
		bool flag = this.m_dict.Remove(item);
		if (!this.m_dict.ContainsKey(item.Key))
		{
			this.RemoveHash(item.Key);
		}
		return flag;
	}

	// Token: 0x17001357 RID: 4951
	// (get) Token: 0x0600A5EB RID: 42475 RVA: 0x003DBE78 File Offset: 0x003DA078
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x17001358 RID: 4952
	// (get) Token: 0x0600A5EC RID: 42476 RVA: 0x003DBE85 File Offset: 0x003DA085
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.IsReadOnly;
		}
	}

	// Token: 0x0600A5ED RID: 42477 RVA: 0x003DBE92 File Offset: 0x003DA092
	public void Add(string key, T value)
	{
		this.m_dict.Add(key, value);
		this.AddHash(key);
	}

	// Token: 0x0600A5EE RID: 42478 RVA: 0x003DBEA8 File Offset: 0x003DA0A8
	public bool ContainsKey(string key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A5EF RID: 42479 RVA: 0x003DBEB6 File Offset: 0x003DA0B6
	public bool Remove(string key)
	{
		bool flag = this.m_dict.Remove(key);
		if (!this.m_dict.ContainsKey(key))
		{
			this.RemoveHash(key);
		}
		return flag;
	}

	// Token: 0x0600A5F0 RID: 42480 RVA: 0x003DBED9 File Offset: 0x003DA0D9
	public bool TryGetValue(string key, out T value)
	{
		return this.m_dict.TryGetValue(key, out value);
	}

	// Token: 0x17001359 RID: 4953
	public T this[string key]
	{
		get
		{
			return this.m_dict[key];
		}
		set
		{
			this.m_dict[key] = value;
			this.AddHash(key);
		}
	}

	// Token: 0x1700135A RID: 4954
	// (get) Token: 0x0600A5F3 RID: 42483 RVA: 0x003DBF0C File Offset: 0x003DA10C
	public ICollection<string> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x1700135B RID: 4955
	// (get) Token: 0x0600A5F4 RID: 42484 RVA: 0x003DBF19 File Offset: 0x003DA119
	public ICollection<T> Values
	{
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x1700135C RID: 4956
	// (get) Token: 0x0600A5F5 RID: 42485 RVA: 0x003DBF0C File Offset: 0x003DA10C
	public IEnumerable<string> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x1700135D RID: 4957
	// (get) Token: 0x0600A5F6 RID: 42486 RVA: 0x003DBF19 File Offset: 0x003DA119
	public IEnumerable<T> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x04007B01 RID: 31489
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<string, T> m_dict;

	// Token: 0x04007B02 RID: 31490
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, List<string>> m_hashToKeys;
}

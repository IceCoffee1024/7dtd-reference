using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014F4 RID: 5364
public class StringSpanDictionary<T> : IDictionary<string, T>, ICollection<KeyValuePair<string, T>>, IEnumerable<KeyValuePair<string, T>>, IEnumerable, IReadOnlyDictionary<string, T>, IReadOnlyCollection<KeyValuePair<string, T>>
{
	// Token: 0x0600A7E0 RID: 42976 RVA: 0x003E80F4 File Offset: 0x003E62F4
	public StringSpanDictionary()
		: this(new Dictionary<string, T>())
	{
	}

	// Token: 0x0600A7E1 RID: 42977 RVA: 0x003E8101 File Offset: 0x003E6301
	public StringSpanDictionary(IDictionary<string, T> dict)
	{
		this.m_dict = dict;
		this.m_hashToKeys = new Dictionary<int, List<string>>();
	}

	// Token: 0x0600A7E2 RID: 42978 RVA: 0x003E811B File Offset: 0x003E631B
	[PublicizedFrom(EAccessModifier.Private)]
	public static int GenerateHash(StringSpan key)
	{
		return key.GetHashCode();
	}

	// Token: 0x0600A7E3 RID: 42979 RVA: 0x003E812C File Offset: 0x003E632C
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

	// Token: 0x0600A7E4 RID: 42980 RVA: 0x003E81B4 File Offset: 0x003E63B4
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

	// Token: 0x0600A7E5 RID: 42981 RVA: 0x003E8200 File Offset: 0x003E6400
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

	// Token: 0x0600A7E6 RID: 42982 RVA: 0x003E8280 File Offset: 0x003E6480
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

	// Token: 0x0600A7E7 RID: 42983 RVA: 0x003E82B8 File Offset: 0x003E64B8
	public bool ContainsKey(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.ContainsKey(text);
	}

	// Token: 0x0600A7E8 RID: 42984 RVA: 0x003E82DC File Offset: 0x003E64DC
	public bool Remove(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.Remove(text);
	}

	// Token: 0x0600A7E9 RID: 42985 RVA: 0x003E8300 File Offset: 0x003E6500
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

	// Token: 0x170013E0 RID: 5088
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

	// Token: 0x0600A7EC RID: 42988 RVA: 0x003E8389 File Offset: 0x003E6589
	public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A7ED RID: 42989 RVA: 0x003E8389 File Offset: 0x003E6589
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A7EE RID: 42990 RVA: 0x003E8396 File Offset: 0x003E6596
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.Add(KeyValuePair<string, T> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A7EF RID: 42991 RVA: 0x003E83AC File Offset: 0x003E65AC
	public void Clear()
	{
		this.m_dict.Clear();
		this.m_hashToKeys.Clear();
	}

	// Token: 0x0600A7F0 RID: 42992 RVA: 0x003E83C4 File Offset: 0x003E65C4
	public bool Contains(KeyValuePair<string, T> item)
	{
		return this.m_dict.Contains(item);
	}

	// Token: 0x0600A7F1 RID: 42993 RVA: 0x003E83D2 File Offset: 0x003E65D2
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
	{
		this.m_dict.CopyTo(array, arrayIndex);
	}

	// Token: 0x0600A7F2 RID: 42994 RVA: 0x003E83E1 File Offset: 0x003E65E1
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

	// Token: 0x170013E1 RID: 5089
	// (get) Token: 0x0600A7F3 RID: 42995 RVA: 0x003E8410 File Offset: 0x003E6610
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x170013E2 RID: 5090
	// (get) Token: 0x0600A7F4 RID: 42996 RVA: 0x003E841D File Offset: 0x003E661D
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.IsReadOnly;
		}
	}

	// Token: 0x0600A7F5 RID: 42997 RVA: 0x003E842A File Offset: 0x003E662A
	public void Add(string key, T value)
	{
		this.m_dict.Add(key, value);
		this.AddHash(key);
	}

	// Token: 0x0600A7F6 RID: 42998 RVA: 0x003E8440 File Offset: 0x003E6640
	public bool ContainsKey(string key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A7F7 RID: 42999 RVA: 0x003E844E File Offset: 0x003E664E
	public bool Remove(string key)
	{
		bool flag = this.m_dict.Remove(key);
		if (!this.m_dict.ContainsKey(key))
		{
			this.RemoveHash(key);
		}
		return flag;
	}

	// Token: 0x0600A7F8 RID: 43000 RVA: 0x003E8471 File Offset: 0x003E6671
	public bool TryGetValue(string key, out T value)
	{
		return this.m_dict.TryGetValue(key, out value);
	}

	// Token: 0x170013E3 RID: 5091
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

	// Token: 0x170013E4 RID: 5092
	// (get) Token: 0x0600A7FB RID: 43003 RVA: 0x003E84A4 File Offset: 0x003E66A4
	public ICollection<string> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x170013E5 RID: 5093
	// (get) Token: 0x0600A7FC RID: 43004 RVA: 0x003E84B1 File Offset: 0x003E66B1
	public ICollection<T> Values
	{
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x170013E6 RID: 5094
	// (get) Token: 0x0600A7FD RID: 43005 RVA: 0x003E84A4 File Offset: 0x003E66A4
	public IEnumerable<string> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x170013E7 RID: 5095
	// (get) Token: 0x0600A7FE RID: 43006 RVA: 0x003E84B1 File Offset: 0x003E66B1
	public IEnumerable<T> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x04007C9E RID: 31902
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<string, T> m_dict;

	// Token: 0x04007C9F RID: 31903
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, List<string>> m_hashToKeys;
}

using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014E2 RID: 5346
public class StringSpanDictionary<T> : IDictionary<string, T>, ICollection<KeyValuePair<string, T>>, IEnumerable<KeyValuePair<string, T>>, IEnumerable, IReadOnlyDictionary<string, T>, IReadOnlyCollection<KeyValuePair<string, T>>
{
	// Token: 0x0600A788 RID: 42888 RVA: 0x003E5CF0 File Offset: 0x003E3EF0
	public StringSpanDictionary()
		: this(new Dictionary<string, T>())
	{
	}

	// Token: 0x0600A789 RID: 42889 RVA: 0x003E5CFD File Offset: 0x003E3EFD
	public StringSpanDictionary(IDictionary<string, T> dict)
	{
		this.m_dict = dict;
		this.m_hashToKeys = new Dictionary<int, List<string>>();
	}

	// Token: 0x0600A78A RID: 42890 RVA: 0x003E5D17 File Offset: 0x003E3F17
	[PublicizedFrom(EAccessModifier.Private)]
	public static int GenerateHash(StringSpan key)
	{
		return key.GetHashCode();
	}

	// Token: 0x0600A78B RID: 42891 RVA: 0x003E5D28 File Offset: 0x003E3F28
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

	// Token: 0x0600A78C RID: 42892 RVA: 0x003E5DB0 File Offset: 0x003E3FB0
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

	// Token: 0x0600A78D RID: 42893 RVA: 0x003E5DFC File Offset: 0x003E3FFC
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

	// Token: 0x0600A78E RID: 42894 RVA: 0x003E5E7C File Offset: 0x003E407C
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

	// Token: 0x0600A78F RID: 42895 RVA: 0x003E5EB4 File Offset: 0x003E40B4
	public bool ContainsKey(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.ContainsKey(text);
	}

	// Token: 0x0600A790 RID: 42896 RVA: 0x003E5ED8 File Offset: 0x003E40D8
	public bool Remove(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.Remove(text);
	}

	// Token: 0x0600A791 RID: 42897 RVA: 0x003E5EFC File Offset: 0x003E40FC
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

	// Token: 0x170013D4 RID: 5076
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

	// Token: 0x0600A794 RID: 42900 RVA: 0x003E5F85 File Offset: 0x003E4185
	public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A795 RID: 42901 RVA: 0x003E5F85 File Offset: 0x003E4185
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A796 RID: 42902 RVA: 0x003E5F92 File Offset: 0x003E4192
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.Add(KeyValuePair<string, T> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A797 RID: 42903 RVA: 0x003E5FA8 File Offset: 0x003E41A8
	public void Clear()
	{
		this.m_dict.Clear();
		this.m_hashToKeys.Clear();
	}

	// Token: 0x0600A798 RID: 42904 RVA: 0x003E5FC0 File Offset: 0x003E41C0
	public bool Contains(KeyValuePair<string, T> item)
	{
		return this.m_dict.Contains(item);
	}

	// Token: 0x0600A799 RID: 42905 RVA: 0x003E5FCE File Offset: 0x003E41CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
	{
		this.m_dict.CopyTo(array, arrayIndex);
	}

	// Token: 0x0600A79A RID: 42906 RVA: 0x003E5FDD File Offset: 0x003E41DD
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

	// Token: 0x170013D5 RID: 5077
	// (get) Token: 0x0600A79B RID: 42907 RVA: 0x003E600C File Offset: 0x003E420C
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x170013D6 RID: 5078
	// (get) Token: 0x0600A79C RID: 42908 RVA: 0x003E6019 File Offset: 0x003E4219
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.IsReadOnly;
		}
	}

	// Token: 0x0600A79D RID: 42909 RVA: 0x003E6026 File Offset: 0x003E4226
	public void Add(string key, T value)
	{
		this.m_dict.Add(key, value);
		this.AddHash(key);
	}

	// Token: 0x0600A79E RID: 42910 RVA: 0x003E603C File Offset: 0x003E423C
	public bool ContainsKey(string key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A79F RID: 42911 RVA: 0x003E604A File Offset: 0x003E424A
	public bool Remove(string key)
	{
		bool flag = this.m_dict.Remove(key);
		if (!this.m_dict.ContainsKey(key))
		{
			this.RemoveHash(key);
		}
		return flag;
	}

	// Token: 0x0600A7A0 RID: 42912 RVA: 0x003E606D File Offset: 0x003E426D
	public bool TryGetValue(string key, out T value)
	{
		return this.m_dict.TryGetValue(key, out value);
	}

	// Token: 0x170013D7 RID: 5079
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

	// Token: 0x170013D8 RID: 5080
	// (get) Token: 0x0600A7A3 RID: 42915 RVA: 0x003E60A0 File Offset: 0x003E42A0
	public ICollection<string> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x170013D9 RID: 5081
	// (get) Token: 0x0600A7A4 RID: 42916 RVA: 0x003E60AD File Offset: 0x003E42AD
	public ICollection<T> Values
	{
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x170013DA RID: 5082
	// (get) Token: 0x0600A7A5 RID: 42917 RVA: 0x003E60A0 File Offset: 0x003E42A0
	public IEnumerable<string> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x170013DB RID: 5083
	// (get) Token: 0x0600A7A6 RID: 42918 RVA: 0x003E60AD File Offset: 0x003E42AD
	public IEnumerable<T> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x04007C5D RID: 31837
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<string, T> m_dict;

	// Token: 0x04007C5E RID: 31838
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, List<string>> m_hashToKeys;
}

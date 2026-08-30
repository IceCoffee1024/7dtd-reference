using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014C0 RID: 5312
public class StringSpanDictionary<T> : IDictionary<string, T>, ICollection<KeyValuePair<string, T>>, IEnumerable<KeyValuePair<string, T>>, IEnumerable, IReadOnlyDictionary<string, T>, IReadOnlyCollection<KeyValuePair<string, T>>
{
	// Token: 0x0600A6D5 RID: 42709 RVA: 0x003E0E9C File Offset: 0x003DF09C
	public StringSpanDictionary()
		: this(new Dictionary<string, T>())
	{
	}

	// Token: 0x0600A6D6 RID: 42710 RVA: 0x003E0EA9 File Offset: 0x003DF0A9
	public StringSpanDictionary(IDictionary<string, T> dict)
	{
		this.m_dict = dict;
		this.m_hashToKeys = new Dictionary<int, List<string>>();
	}

	// Token: 0x0600A6D7 RID: 42711 RVA: 0x003E0EC3 File Offset: 0x003DF0C3
	[PublicizedFrom(EAccessModifier.Private)]
	public static int GenerateHash(StringSpan key)
	{
		return key.GetHashCode();
	}

	// Token: 0x0600A6D8 RID: 42712 RVA: 0x003E0ED4 File Offset: 0x003DF0D4
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

	// Token: 0x0600A6D9 RID: 42713 RVA: 0x003E0F5C File Offset: 0x003DF15C
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

	// Token: 0x0600A6DA RID: 42714 RVA: 0x003E0FA8 File Offset: 0x003DF1A8
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

	// Token: 0x0600A6DB RID: 42715 RVA: 0x003E1028 File Offset: 0x003DF228
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

	// Token: 0x0600A6DC RID: 42716 RVA: 0x003E1060 File Offset: 0x003DF260
	public bool ContainsKey(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.ContainsKey(text);
	}

	// Token: 0x0600A6DD RID: 42717 RVA: 0x003E1084 File Offset: 0x003DF284
	public bool Remove(StringSpan key)
	{
		string text;
		return this.TryGetStringFromHashedKeys(key, out text) && this.Remove(text);
	}

	// Token: 0x0600A6DE RID: 42718 RVA: 0x003E10A8 File Offset: 0x003DF2A8
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

	// Token: 0x1700138F RID: 5007
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

	// Token: 0x0600A6E1 RID: 42721 RVA: 0x003E1131 File Offset: 0x003DF331
	public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A6E2 RID: 42722 RVA: 0x003E1131 File Offset: 0x003DF331
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x0600A6E3 RID: 42723 RVA: 0x003E113E File Offset: 0x003DF33E
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.Add(KeyValuePair<string, T> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x0600A6E4 RID: 42724 RVA: 0x003E1154 File Offset: 0x003DF354
	public void Clear()
	{
		this.m_dict.Clear();
		this.m_hashToKeys.Clear();
	}

	// Token: 0x0600A6E5 RID: 42725 RVA: 0x003E116C File Offset: 0x003DF36C
	public bool Contains(KeyValuePair<string, T> item)
	{
		return this.m_dict.Contains(item);
	}

	// Token: 0x0600A6E6 RID: 42726 RVA: 0x003E117A File Offset: 0x003DF37A
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
	{
		this.m_dict.CopyTo(array, arrayIndex);
	}

	// Token: 0x0600A6E7 RID: 42727 RVA: 0x003E1189 File Offset: 0x003DF389
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

	// Token: 0x17001390 RID: 5008
	// (get) Token: 0x0600A6E8 RID: 42728 RVA: 0x003E11B8 File Offset: 0x003DF3B8
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x17001391 RID: 5009
	// (get) Token: 0x0600A6E9 RID: 42729 RVA: 0x003E11C5 File Offset: 0x003DF3C5
	public bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,T>>.IsReadOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.IsReadOnly;
		}
	}

	// Token: 0x0600A6EA RID: 42730 RVA: 0x003E11D2 File Offset: 0x003DF3D2
	public void Add(string key, T value)
	{
		this.m_dict.Add(key, value);
		this.AddHash(key);
	}

	// Token: 0x0600A6EB RID: 42731 RVA: 0x003E11E8 File Offset: 0x003DF3E8
	public bool ContainsKey(string key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A6EC RID: 42732 RVA: 0x003E11F6 File Offset: 0x003DF3F6
	public bool Remove(string key)
	{
		bool flag = this.m_dict.Remove(key);
		if (!this.m_dict.ContainsKey(key))
		{
			this.RemoveHash(key);
		}
		return flag;
	}

	// Token: 0x0600A6ED RID: 42733 RVA: 0x003E1219 File Offset: 0x003DF419
	public bool TryGetValue(string key, out T value)
	{
		return this.m_dict.TryGetValue(key, out value);
	}

	// Token: 0x17001392 RID: 5010
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

	// Token: 0x17001393 RID: 5011
	// (get) Token: 0x0600A6F0 RID: 42736 RVA: 0x003E124C File Offset: 0x003DF44C
	public ICollection<string> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x0600A6F1 RID: 42737 RVA: 0x003E1259 File Offset: 0x003DF459
	public ICollection<T> Values
	{
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x0600A6F2 RID: 42738 RVA: 0x003E124C File Offset: 0x003DF44C
	public IEnumerable<string> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Keys
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x17001396 RID: 5014
	// (get) Token: 0x0600A6F3 RID: 42739 RVA: 0x003E1259 File Offset: 0x003DF459
	public IEnumerable<T> System.Collections.Generic.IReadOnlyDictionary<System.String,T>.Values
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_dict.Values;
		}
	}

	// Token: 0x04007BDA RID: 31706
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IDictionary<string, T> m_dict;

	// Token: 0x04007BDB RID: 31707
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, List<string>> m_hashToKeys;
}

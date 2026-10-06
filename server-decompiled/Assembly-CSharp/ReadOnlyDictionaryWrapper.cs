using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014A9 RID: 5289
public class ReadOnlyDictionaryWrapper<TKey, TValueIn, TValueOut> : IReadOnlyDictionary<TKey, TValueOut>, IEnumerable<KeyValuePair<TKey, TValueOut>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValueOut>> where TValueIn : TValueOut
{
	// Token: 0x0600A5B4 RID: 42420 RVA: 0x003DB74C File Offset: 0x003D994C
	public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TKey, TValueIn> dict)
	{
		this.m_dict = dict;
	}

	// Token: 0x0600A5B5 RID: 42421 RVA: 0x003DB75B File Offset: 0x003D995B
	public IEnumerator<KeyValuePair<TKey, TValueOut>> GetEnumerator()
	{
		foreach (KeyValuePair<TKey, TValueIn> keyValuePair in this.m_dict)
		{
			TKey tkey;
			TValueIn tvalueIn;
			keyValuePair.Deconstruct(out tkey, out tvalueIn);
			TKey tkey2 = tkey;
			TValueIn tvalueIn2 = tvalueIn;
			yield return new KeyValuePair<TKey, TValueOut>(tkey2, (TValueOut)((object)tvalueIn2));
		}
		IEnumerator<KeyValuePair<TKey, TValueIn>> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x0600A5B6 RID: 42422 RVA: 0x003DB76A File Offset: 0x003D996A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x0600A5B7 RID: 42423 RVA: 0x003DB777 File Offset: 0x003D9977
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x0600A5B8 RID: 42424 RVA: 0x003DB784 File Offset: 0x003D9984
	public bool ContainsKey(TKey key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A5B9 RID: 42425 RVA: 0x003DB794 File Offset: 0x003D9994
	public bool TryGetValue(TKey key, out TValueOut value)
	{
		TValueIn tvalueIn;
		bool flag = this.m_dict.TryGetValue(key, out tvalueIn);
		value = (TValueOut)((object)tvalueIn);
		return flag;
	}

	// Token: 0x170013A3 RID: 5027
	public TValueOut this[TKey key]
	{
		get
		{
			return (TValueOut)((object)this.m_dict[key]);
		}
	}

	// Token: 0x170013A4 RID: 5028
	// (get) Token: 0x0600A5BB RID: 42427 RVA: 0x003DB7D8 File Offset: 0x003D99D8
	public IEnumerable<TKey> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x170013A5 RID: 5029
	// (get) Token: 0x0600A5BC RID: 42428 RVA: 0x003DB7E5 File Offset: 0x003D99E5
	public IEnumerable<TValueOut> Values
	{
		get
		{
			foreach (TValueIn tvalueIn in this.m_dict.Values)
			{
				yield return (TValueOut)((object)tvalueIn);
			}
			IEnumerator<TValueIn> enumerator = null;
			yield break;
			yield break;
		}
	}

	// Token: 0x04007B88 RID: 31624
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IReadOnlyDictionary<TKey, TValueIn> m_dict;
}

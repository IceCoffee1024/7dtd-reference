using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001489 RID: 5257
public class ReadOnlyDictionaryWrapper<TKey, TValueIn, TValueOut> : IReadOnlyDictionary<TKey, TValueOut>, IEnumerable<KeyValuePair<TKey, TValueOut>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValueOut>> where TValueIn : TValueOut
{
	// Token: 0x0600A506 RID: 42246 RVA: 0x003D6AC0 File Offset: 0x003D4CC0
	public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TKey, TValueIn> dict)
	{
		this.m_dict = dict;
	}

	// Token: 0x0600A507 RID: 42247 RVA: 0x003D6ACF File Offset: 0x003D4CCF
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

	// Token: 0x0600A508 RID: 42248 RVA: 0x003D6ADE File Offset: 0x003D4CDE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x1700135D RID: 4957
	// (get) Token: 0x0600A509 RID: 42249 RVA: 0x003D6AEB File Offset: 0x003D4CEB
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x0600A50A RID: 42250 RVA: 0x003D6AF8 File Offset: 0x003D4CF8
	public bool ContainsKey(TKey key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A50B RID: 42251 RVA: 0x003D6B08 File Offset: 0x003D4D08
	public bool TryGetValue(TKey key, out TValueOut value)
	{
		TValueIn tvalueIn;
		bool flag = this.m_dict.TryGetValue(key, out tvalueIn);
		value = (TValueOut)((object)tvalueIn);
		return flag;
	}

	// Token: 0x1700135E RID: 4958
	public TValueOut this[TKey key]
	{
		get
		{
			return (TValueOut)((object)this.m_dict[key]);
		}
	}

	// Token: 0x1700135F RID: 4959
	// (get) Token: 0x0600A50D RID: 42253 RVA: 0x003D6B4C File Offset: 0x003D4D4C
	public IEnumerable<TKey> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x17001360 RID: 4960
	// (get) Token: 0x0600A50E RID: 42254 RVA: 0x003D6B59 File Offset: 0x003D4D59
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

	// Token: 0x04007B0D RID: 31501
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IReadOnlyDictionary<TKey, TValueIn> m_dict;
}

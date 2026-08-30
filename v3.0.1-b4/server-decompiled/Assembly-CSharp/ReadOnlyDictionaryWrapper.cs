using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200146E RID: 5230
public class ReadOnlyDictionaryWrapper<TKey, TValueIn, TValueOut> : IReadOnlyDictionary<TKey, TValueOut>, IEnumerable<KeyValuePair<TKey, TValueOut>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValueOut>> where TValueIn : TValueOut
{
	// Token: 0x0600A409 RID: 41993 RVA: 0x003D177C File Offset: 0x003CF97C
	public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TKey, TValueIn> dict)
	{
		this.m_dict = dict;
	}

	// Token: 0x0600A40A RID: 41994 RVA: 0x003D178B File Offset: 0x003CF98B
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

	// Token: 0x0600A40B RID: 41995 RVA: 0x003D179A File Offset: 0x003CF99A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x17001324 RID: 4900
	// (get) Token: 0x0600A40C RID: 41996 RVA: 0x003D17A7 File Offset: 0x003CF9A7
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x0600A40D RID: 41997 RVA: 0x003D17B4 File Offset: 0x003CF9B4
	public bool ContainsKey(TKey key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A40E RID: 41998 RVA: 0x003D17C4 File Offset: 0x003CF9C4
	public bool TryGetValue(TKey key, out TValueOut value)
	{
		TValueIn tvalueIn;
		bool flag = this.m_dict.TryGetValue(key, out tvalueIn);
		value = (TValueOut)((object)tvalueIn);
		return flag;
	}

	// Token: 0x17001325 RID: 4901
	public TValueOut this[TKey key]
	{
		get
		{
			return (TValueOut)((object)this.m_dict[key]);
		}
	}

	// Token: 0x17001326 RID: 4902
	// (get) Token: 0x0600A410 RID: 42000 RVA: 0x003D1808 File Offset: 0x003CFA08
	public IEnumerable<TKey> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x0600A411 RID: 42001 RVA: 0x003D1815 File Offset: 0x003CFA15
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

	// Token: 0x04007A34 RID: 31284
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IReadOnlyDictionary<TKey, TValueIn> m_dict;
}

using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014BB RID: 5307
public class ReadOnlyDictionaryWrapper<TKey, TValueIn, TValueOut> : IReadOnlyDictionary<TKey, TValueOut>, IEnumerable<KeyValuePair<TKey, TValueOut>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValueOut>> where TValueIn : TValueOut
{
	// Token: 0x0600A60C RID: 42508 RVA: 0x003DDB50 File Offset: 0x003DBD50
	public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TKey, TValueIn> dict)
	{
		this.m_dict = dict;
	}

	// Token: 0x0600A60D RID: 42509 RVA: 0x003DDB5F File Offset: 0x003DBD5F
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

	// Token: 0x0600A60E RID: 42510 RVA: 0x003DDB6E File Offset: 0x003DBD6E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_dict.GetEnumerator();
	}

	// Token: 0x170013AE RID: 5038
	// (get) Token: 0x0600A60F RID: 42511 RVA: 0x003DDB7B File Offset: 0x003DBD7B
	public int Count
	{
		get
		{
			return this.m_dict.Count;
		}
	}

	// Token: 0x0600A610 RID: 42512 RVA: 0x003DDB88 File Offset: 0x003DBD88
	public bool ContainsKey(TKey key)
	{
		return this.m_dict.ContainsKey(key);
	}

	// Token: 0x0600A611 RID: 42513 RVA: 0x003DDB98 File Offset: 0x003DBD98
	public bool TryGetValue(TKey key, out TValueOut value)
	{
		TValueIn tvalueIn;
		bool flag = this.m_dict.TryGetValue(key, out tvalueIn);
		value = (TValueOut)((object)tvalueIn);
		return flag;
	}

	// Token: 0x170013AF RID: 5039
	public TValueOut this[TKey key]
	{
		get
		{
			return (TValueOut)((object)this.m_dict[key]);
		}
	}

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x0600A613 RID: 42515 RVA: 0x003DDBDC File Offset: 0x003DBDDC
	public IEnumerable<TKey> Keys
	{
		get
		{
			return this.m_dict.Keys;
		}
	}

	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x0600A614 RID: 42516 RVA: 0x003DDBE9 File Offset: 0x003DBDE9
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

	// Token: 0x04007BC9 RID: 31689
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IReadOnlyDictionary<TKey, TValueIn> m_dict;
}

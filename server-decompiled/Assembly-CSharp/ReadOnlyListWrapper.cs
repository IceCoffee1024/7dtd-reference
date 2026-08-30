using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200148C RID: 5260
public class ReadOnlyListWrapper<TIn, TOut> : IReadOnlyList<TOut>, IEnumerable<TOut>, IEnumerable, IReadOnlyCollection<TOut> where TIn : TOut
{
	// Token: 0x0600A51F RID: 42271 RVA: 0x003D6E33 File Offset: 0x003D5033
	public ReadOnlyListWrapper(IList<TIn> list)
	{
		this.m_list = list;
	}

	// Token: 0x0600A520 RID: 42272 RVA: 0x003D6E42 File Offset: 0x003D5042
	public IEnumerator<TOut> GetEnumerator()
	{
		return (IEnumerator<TOut>)this.m_list.GetEnumerator();
	}

	// Token: 0x0600A521 RID: 42273 RVA: 0x003D6E54 File Offset: 0x003D5054
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x17001365 RID: 4965
	// (get) Token: 0x0600A522 RID: 42274 RVA: 0x003D6E61 File Offset: 0x003D5061
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x17001366 RID: 4966
	public TOut this[int index]
	{
		get
		{
			return (TOut)((object)this.m_list[index]);
		}
	}

	// Token: 0x04007B17 RID: 31511
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<TIn> m_list;
}

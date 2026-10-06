using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014AC RID: 5292
public class ReadOnlyListWrapper<TIn, TOut> : IReadOnlyList<TOut>, IEnumerable<TOut>, IEnumerable, IReadOnlyCollection<TOut> where TIn : TOut
{
	// Token: 0x0600A5CD RID: 42445 RVA: 0x003DBABF File Offset: 0x003D9CBF
	public ReadOnlyListWrapper(IList<TIn> list)
	{
		this.m_list = list;
	}

	// Token: 0x0600A5CE RID: 42446 RVA: 0x003DBACE File Offset: 0x003D9CCE
	public IEnumerator<TOut> GetEnumerator()
	{
		return (IEnumerator<TOut>)this.m_list.GetEnumerator();
	}

	// Token: 0x0600A5CF RID: 42447 RVA: 0x003DBAE0 File Offset: 0x003D9CE0
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x170013AA RID: 5034
	// (get) Token: 0x0600A5D0 RID: 42448 RVA: 0x003DBAED File Offset: 0x003D9CED
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x170013AB RID: 5035
	public TOut this[int index]
	{
		get
		{
			return (TOut)((object)this.m_list[index]);
		}
	}

	// Token: 0x04007B92 RID: 31634
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<TIn> m_list;
}

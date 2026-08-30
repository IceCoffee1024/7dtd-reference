using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001471 RID: 5233
public class ReadOnlyListWrapper<TIn, TOut> : IReadOnlyList<TOut>, IEnumerable<TOut>, IEnumerable, IReadOnlyCollection<TOut> where TIn : TOut
{
	// Token: 0x0600A422 RID: 42018 RVA: 0x003D1AEF File Offset: 0x003CFCEF
	public ReadOnlyListWrapper(IList<TIn> list)
	{
		this.m_list = list;
	}

	// Token: 0x0600A423 RID: 42019 RVA: 0x003D1AFE File Offset: 0x003CFCFE
	public IEnumerator<TOut> GetEnumerator()
	{
		return (IEnumerator<TOut>)this.m_list.GetEnumerator();
	}

	// Token: 0x0600A424 RID: 42020 RVA: 0x003D1B10 File Offset: 0x003CFD10
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x1700132C RID: 4908
	// (get) Token: 0x0600A425 RID: 42021 RVA: 0x003D1B1D File Offset: 0x003CFD1D
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x1700132D RID: 4909
	public TOut this[int index]
	{
		get
		{
			return (TOut)((object)this.m_list[index]);
		}
	}

	// Token: 0x04007A3E RID: 31294
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<TIn> m_list;
}

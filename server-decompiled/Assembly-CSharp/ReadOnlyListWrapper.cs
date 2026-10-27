using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020014BE RID: 5310
public class ReadOnlyListWrapper<TIn, TOut> : IReadOnlyList<TOut>, IEnumerable<TOut>, IEnumerable, IReadOnlyCollection<TOut> where TIn : TOut
{
	// Token: 0x0600A625 RID: 42533 RVA: 0x003DDEC3 File Offset: 0x003DC0C3
	public ReadOnlyListWrapper(IList<TIn> list)
	{
		this.m_list = list;
	}

	// Token: 0x0600A626 RID: 42534 RVA: 0x003DDED2 File Offset: 0x003DC0D2
	public IEnumerator<TOut> GetEnumerator()
	{
		return (IEnumerator<TOut>)this.m_list.GetEnumerator();
	}

	// Token: 0x0600A627 RID: 42535 RVA: 0x003DDEE4 File Offset: 0x003DC0E4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x170013B6 RID: 5046
	// (get) Token: 0x0600A628 RID: 42536 RVA: 0x003DDEF1 File Offset: 0x003DC0F1
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x170013B7 RID: 5047
	public TOut this[int index]
	{
		get
		{
			return (TOut)((object)this.m_list[index]);
		}
	}

	// Token: 0x04007BD3 RID: 31699
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IList<TIn> m_list;
}

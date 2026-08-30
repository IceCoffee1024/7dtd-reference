using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x0200148E RID: 5262
public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
{
	// Token: 0x0600A528 RID: 42280 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x17001368 RID: 4968
	// (get) Token: 0x0600A529 RID: 42281 RVA: 0x003D6EC8 File Offset: 0x003D50C8
	public static ReferenceEqualityComparer<T> Instance
	{
		get
		{
			if (ReferenceEqualityComparer<T>.instance == null)
			{
				ReferenceEqualityComparer<T> referenceEqualityComparer = new ReferenceEqualityComparer<T>();
				Interlocked.CompareExchange<ReferenceEqualityComparer<T>>(ref ReferenceEqualityComparer<T>.instance, referenceEqualityComparer, null);
			}
			return ReferenceEqualityComparer<T>.instance;
		}
	}

	// Token: 0x0600A52A RID: 42282 RVA: 0x003D6EF4 File Offset: 0x003D50F4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IEqualityComparer<T>.Equals(T _x, T _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A52B RID: 42283 RVA: 0x003D6F04 File Offset: 0x003D5104
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.Generic.IEqualityComparer<T>.GetHashCode(T _obj)
	{
		if (_obj == null)
		{
			return 0;
		}
		return _obj.GetHashCode();
	}

	// Token: 0x04007B19 RID: 31513
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer<T> instance;
}

using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020014C0 RID: 5312
public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
{
	// Token: 0x0600A62E RID: 42542 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x170013B9 RID: 5049
	// (get) Token: 0x0600A62F RID: 42543 RVA: 0x003DDF58 File Offset: 0x003DC158
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

	// Token: 0x0600A630 RID: 42544 RVA: 0x003DDF84 File Offset: 0x003DC184
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IEqualityComparer<T>.Equals(T _x, T _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A631 RID: 42545 RVA: 0x003DDF94 File Offset: 0x003DC194
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.Generic.IEqualityComparer<T>.GetHashCode(T _obj)
	{
		if (_obj == null)
		{
			return 0;
		}
		return _obj.GetHashCode();
	}

	// Token: 0x04007BD5 RID: 31701
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer<T> instance;
}

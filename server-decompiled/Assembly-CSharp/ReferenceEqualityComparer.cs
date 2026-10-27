using System;
using System.Collections;
using System.Threading;

// Token: 0x020014BF RID: 5311
public class ReferenceEqualityComparer : IEqualityComparer
{
	// Token: 0x0600A62A RID: 42538 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x170013B8 RID: 5048
	// (get) Token: 0x0600A62B RID: 42539 RVA: 0x003DDF18 File Offset: 0x003DC118
	public static ReferenceEqualityComparer Instance
	{
		get
		{
			if (ReferenceEqualityComparer.instance == null)
			{
				ReferenceEqualityComparer referenceEqualityComparer = new ReferenceEqualityComparer();
				Interlocked.CompareExchange<ReferenceEqualityComparer>(ref ReferenceEqualityComparer.instance, referenceEqualityComparer, null);
			}
			return ReferenceEqualityComparer.instance;
		}
	}

	// Token: 0x0600A62C RID: 42540 RVA: 0x003DDF44 File Offset: 0x003DC144
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IEqualityComparer.Equals(object _x, object _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A62D RID: 42541 RVA: 0x003DDF4A File Offset: 0x003DC14A
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.IEqualityComparer.GetHashCode(object _obj)
	{
		if (_obj != null)
		{
			return _obj.GetHashCode();
		}
		return 0;
	}

	// Token: 0x04007BD4 RID: 31700
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer instance;
}

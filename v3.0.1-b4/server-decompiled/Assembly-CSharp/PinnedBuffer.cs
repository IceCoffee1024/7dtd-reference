using System;
using System.Runtime.CompilerServices;

// Token: 0x02000B56 RID: 2902
public abstract class PinnedBuffer : IDisposable
{
	// Token: 0x060057DE RID: 22494 RVA: 0x00219900 File Offset: 0x00217B00
	public static PinnedBuffer<T> Create<[IsUnmanaged] T>(ArrayListMP<T> list, PinnedBuffer.Ownership ownership) where T : struct, ValueType
	{
		if (list == null || list.Count <= 0)
		{
			return new PinnedBuffer<T>();
		}
		switch (ownership)
		{
		case PinnedBuffer.Ownership.Pin:
			return new PinnedBuffer<T>(list.Items, list.Count);
		case PinnedBuffer.Ownership.Clone:
		{
			PinnedBuffer<T> pinnedBuffer = new PinnedBuffer<T>(list.pool, list.Count);
			list.Items.AsSpan<T>(0, list.Count).CopyTo(pinnedBuffer.AsSpan());
			return pinnedBuffer;
		}
		case PinnedBuffer.Ownership.Steal:
		{
			T[] items = list.Items;
			int count = list.Count;
			list.Items = null;
			list.Count = 0;
			return new PinnedBuffer<T>(list.pool, items, count);
		}
		default:
			throw new ArgumentOutOfRangeException("ownership", ownership, null);
		}
	}

	// Token: 0x060057DF RID: 22495
	public abstract void Dispose();

	// Token: 0x060057E0 RID: 22496 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public PinnedBuffer()
	{
	}

	// Token: 0x02000B57 RID: 2903
	public enum Ownership
	{
		// Token: 0x040043D7 RID: 17367
		Pin,
		// Token: 0x040043D8 RID: 17368
		Clone,
		// Token: 0x040043D9 RID: 17369
		Steal
	}
}

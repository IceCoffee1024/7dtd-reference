using System;
using System.Runtime.CompilerServices;

// Token: 0x02000B68 RID: 2920
public abstract class PinnedBuffer : IDisposable
{
	// Token: 0x06005866 RID: 22630 RVA: 0x0021C61C File Offset: 0x0021A81C
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

	// Token: 0x06005867 RID: 22631
	public abstract void Dispose();

	// Token: 0x06005868 RID: 22632 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public PinnedBuffer()
	{
	}

	// Token: 0x02000B69 RID: 2921
	public enum Ownership
	{
		// Token: 0x04004472 RID: 17522
		Pin,
		// Token: 0x04004473 RID: 17523
		Clone,
		// Token: 0x04004474 RID: 17524
		Steal
	}
}

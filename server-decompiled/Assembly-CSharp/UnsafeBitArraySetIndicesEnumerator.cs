using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CD0 RID: 3280
public struct UnsafeBitArraySetIndicesEnumerator : IEnumerator<int>, IEnumerator, IDisposable
{
	// Token: 0x0600644A RID: 25674 RVA: 0x00277728 File Offset: 0x00275928
	public unsafe UnsafeBitArraySetIndicesEnumerator(UnsafeBitArray bitArray)
	{
		this.bitArray = bitArray;
		this.currentSlice = *bitArray.Ptr;
		this.sliceIndex = 0;
		this.numSlices = bitArray.Length / 64;
		this.currentIndex = -1;
		this.leadingZeroCount = 0;
		this.numSetBits = bitArray.CountBits(0, bitArray.Length);
		this.foundBits = 0;
	}

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x0600644B RID: 25675 RVA: 0x00277788 File Offset: 0x00275988
	public int Current
	{
		get
		{
			return this.currentIndex;
		}
	}

	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x0600644C RID: 25676 RVA: 0x00277790 File Offset: 0x00275990
	public object System.Collections.IEnumerator.Current
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Current;
		}
	}

	// Token: 0x0600644D RID: 25677 RVA: 0x002777A0 File Offset: 0x002759A0
	public unsafe bool MoveNext()
	{
		while (this.foundBits < this.numSetBits && this.sliceIndex < this.numSlices)
		{
			if (this.currentSlice == 0UL)
			{
				this.sliceIndex++;
				if (this.sliceIndex < this.numSlices)
				{
					this.currentSlice = this.bitArray.Ptr[this.sliceIndex];
					this.leadingZeroCount = 0;
				}
			}
			else
			{
				if ((this.currentSlice & 1UL) != 0UL)
				{
					this.currentIndex = this.leadingZeroCount + this.sliceIndex * 64;
					this.leadingZeroCount++;
					this.currentSlice >>= 1;
					this.foundBits++;
					return true;
				}
				if ((this.currentSlice & (ulong)(-1)) == 0UL)
				{
					this.leadingZeroCount += 32;
					this.currentSlice >>= 32;
				}
				if ((this.currentSlice & 65535UL) == 0UL)
				{
					this.leadingZeroCount += 16;
					this.currentSlice >>= 16;
				}
				if ((this.currentSlice & 255UL) == 0UL)
				{
					this.leadingZeroCount += 8;
					this.currentSlice >>= 8;
				}
				if ((this.currentSlice & 15UL) == 0UL)
				{
					this.leadingZeroCount += 4;
					this.currentSlice >>= 4;
				}
				if ((this.currentSlice & 3UL) == 0UL)
				{
					this.leadingZeroCount += 2;
					this.currentSlice >>= 2;
				}
				if ((this.currentSlice & 1UL) == 0UL)
				{
					this.leadingZeroCount++;
					this.currentSlice >>= 1;
				}
			}
		}
		return false;
	}

	// Token: 0x0600644E RID: 25678 RVA: 0x00277965 File Offset: 0x00275B65
	public unsafe void Reset()
	{
		this.sliceIndex = 0;
		this.currentSlice = *this.bitArray.Ptr;
		this.currentIndex = -1;
		this.leadingZeroCount = 0;
		this.foundBits = 0;
	}

	// Token: 0x0600644F RID: 25679 RVA: 0x000027FC File Offset: 0x000009FC
	public void Dispose()
	{
	}

	// Token: 0x04004E4B RID: 20043
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeBitArray bitArray;

	// Token: 0x04004E4C RID: 20044
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong currentSlice;

	// Token: 0x04004E4D RID: 20045
	[PublicizedFrom(EAccessModifier.Private)]
	public int sliceIndex;

	// Token: 0x04004E4E RID: 20046
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSlices;

	// Token: 0x04004E4F RID: 20047
	[PublicizedFrom(EAccessModifier.Private)]
	public int leadingZeroCount;

	// Token: 0x04004E50 RID: 20048
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x04004E51 RID: 20049
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSetBits;

	// Token: 0x04004E52 RID: 20050
	[PublicizedFrom(EAccessModifier.Private)]
	public int foundBits;
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02000CDB RID: 3291
public struct UnsafeBitArraySetIndicesEnumerator : IEnumerator<int>, IEnumerator, IDisposable
{
	// Token: 0x0600646F RID: 25711 RVA: 0x00278C50 File Offset: 0x00276E50
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

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x06006470 RID: 25712 RVA: 0x00278CB0 File Offset: 0x00276EB0
	public int Current
	{
		get
		{
			return this.currentIndex;
		}
	}

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x06006471 RID: 25713 RVA: 0x00278CB8 File Offset: 0x00276EB8
	public object System.Collections.IEnumerator.Current
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Current;
		}
	}

	// Token: 0x06006472 RID: 25714 RVA: 0x00278CC8 File Offset: 0x00276EC8
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

	// Token: 0x06006473 RID: 25715 RVA: 0x00278E8D File Offset: 0x0027708D
	public unsafe void Reset()
	{
		this.sliceIndex = 0;
		this.currentSlice = *this.bitArray.Ptr;
		this.currentIndex = -1;
		this.leadingZeroCount = 0;
		this.foundBits = 0;
	}

	// Token: 0x06006474 RID: 25716 RVA: 0x000027FC File Offset: 0x000009FC
	public void Dispose()
	{
	}

	// Token: 0x04004E70 RID: 20080
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeBitArray bitArray;

	// Token: 0x04004E71 RID: 20081
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong currentSlice;

	// Token: 0x04004E72 RID: 20082
	[PublicizedFrom(EAccessModifier.Private)]
	public int sliceIndex;

	// Token: 0x04004E73 RID: 20083
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSlices;

	// Token: 0x04004E74 RID: 20084
	[PublicizedFrom(EAccessModifier.Private)]
	public int leadingZeroCount;

	// Token: 0x04004E75 RID: 20085
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x04004E76 RID: 20086
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSetBits;

	// Token: 0x04004E77 RID: 20087
	[PublicizedFrom(EAccessModifier.Private)]
	public int foundBits;
}

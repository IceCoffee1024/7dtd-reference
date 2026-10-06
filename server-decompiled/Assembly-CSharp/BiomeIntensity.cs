using System;
using System.IO;

// Token: 0x02000C53 RID: 3155
public struct BiomeIntensity : IEquatable<BiomeIntensity>
{
	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x06005F90 RID: 24464 RVA: 0x0025F243 File Offset: 0x0025D443
	// (set) Token: 0x06005F91 RID: 24465 RVA: 0x0025F255 File Offset: 0x0025D455
	public float intensity0
	{
		get
		{
			return (float)(this.intensity0and1 & 15) / 15f;
		}
		set
		{
			this.intensity0and1 = (byte)((int)(this.intensity0and1 & 240) | ((int)(value * 15f) & 15));
		}
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x06005F92 RID: 24466 RVA: 0x0025F276 File Offset: 0x0025D476
	// (set) Token: 0x06005F93 RID: 24467 RVA: 0x0025F28A File Offset: 0x0025D48A
	public float intensity1
	{
		get
		{
			return (float)((this.intensity0and1 >> 4) & 15) / 15f;
		}
		set
		{
			this.intensity0and1 = (byte)((int)(this.intensity0and1 & 15) | (((int)(value * 15f) << 4) & 240));
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x06005F94 RID: 24468 RVA: 0x0025F2AD File Offset: 0x0025D4AD
	// (set) Token: 0x06005F95 RID: 24469 RVA: 0x0025F2BF File Offset: 0x0025D4BF
	public float intensity2
	{
		get
		{
			return (float)(this.intensity2and3 & 15) / 15f;
		}
		set
		{
			this.intensity2and3 = (byte)((int)(this.intensity2and3 & 240) | ((int)(value * 15f) & 15));
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x06005F96 RID: 24470 RVA: 0x0025F2E0 File Offset: 0x0025D4E0
	// (set) Token: 0x06005F97 RID: 24471 RVA: 0x0025F2F4 File Offset: 0x0025D4F4
	public float intensity3
	{
		get
		{
			return (float)((this.intensity2and3 >> 4) & 15) / 15f;
		}
		set
		{
			this.intensity2and3 = (byte)((int)(this.intensity2and3 & 15) | (((int)(value * 15f) << 4) & 240));
		}
	}

	// Token: 0x06005F98 RID: 24472 RVA: 0x0025F317 File Offset: 0x0025D517
	public BiomeIntensity(byte _singleBiomeId)
	{
		this.biomeId0 = _singleBiomeId;
		this.biomeId1 = 0;
		this.biomeId2 = 0;
		this.biomeId3 = 0;
		this.intensity0and1 = 15;
		this.intensity2and3 = 0;
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x0025F344 File Offset: 0x0025D544
	public BiomeIntensity(byte[] _chunkBiomeIntensityArray, int _offs)
	{
		this.biomeId0 = _chunkBiomeIntensityArray[_offs];
		this.biomeId1 = _chunkBiomeIntensityArray[_offs + 1];
		this.biomeId2 = _chunkBiomeIntensityArray[_offs + 2];
		this.biomeId3 = _chunkBiomeIntensityArray[_offs + 3];
		this.intensity0and1 = _chunkBiomeIntensityArray[_offs + 4];
		this.intensity2and3 = _chunkBiomeIntensityArray[_offs + 5];
	}

	// Token: 0x06005F9A RID: 24474 RVA: 0x0025F394 File Offset: 0x0025D594
	public static BiomeIntensity FromArray(int[] _unsortedBiomeIdArray)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		BiomeIntensity biomeIntensity = default(BiomeIntensity);
		for (int i = 0; i < _unsortedBiomeIdArray.Length; i++)
		{
			if (num < _unsortedBiomeIdArray[i])
			{
				biomeIntensity.biomeId0 = (byte)i;
				num = _unsortedBiomeIdArray[i];
				if (num5 < num)
				{
					num5 = num;
				}
			}
		}
		_unsortedBiomeIdArray[(int)biomeIntensity.biomeId0] = 0;
		for (int j = 0; j < _unsortedBiomeIdArray.Length; j++)
		{
			if (num2 < _unsortedBiomeIdArray[j])
			{
				biomeIntensity.biomeId1 = (byte)j;
				num2 = _unsortedBiomeIdArray[j];
				if (num5 < num2)
				{
					num5 = num2;
				}
			}
		}
		_unsortedBiomeIdArray[(int)biomeIntensity.biomeId1] = 0;
		for (int k = 0; k < _unsortedBiomeIdArray.Length; k++)
		{
			if (num3 < _unsortedBiomeIdArray[k])
			{
				biomeIntensity.biomeId2 = (byte)k;
				num3 = _unsortedBiomeIdArray[k];
				if (num5 < num3)
				{
					num5 = num3;
				}
			}
		}
		_unsortedBiomeIdArray[(int)biomeIntensity.biomeId2] = 0;
		for (int l = 0; l < _unsortedBiomeIdArray.Length; l++)
		{
			if (num4 < _unsortedBiomeIdArray[l])
			{
				biomeIntensity.biomeId3 = (byte)l;
				num4 = _unsortedBiomeIdArray[l];
				if (num5 < num4)
				{
					num5 = num4;
				}
			}
		}
		_unsortedBiomeIdArray[(int)biomeIntensity.biomeId3] = 0;
		biomeIntensity.intensity0 = (float)num / (float)num5;
		biomeIntensity.intensity1 = (float)num2 / (float)num5;
		biomeIntensity.intensity2 = (float)num3 / (float)num5;
		biomeIntensity.intensity3 = (float)num4 / (float)num5;
		return biomeIntensity;
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x0025F4D4 File Offset: 0x0025D6D4
	public void ToArray(byte[] _array, int offs)
	{
		_array[offs] = this.biomeId0;
		_array[1 + offs] = this.biomeId1;
		_array[2 + offs] = this.biomeId2;
		_array[3 + offs] = this.biomeId3;
		_array[4 + offs] = this.intensity0and1;
		_array[5 + offs] = this.intensity2and3;
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x0025F524 File Offset: 0x0025D724
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.biomeId0);
		_bw.Write((byte)(this.intensity0 * 255f));
		_bw.Write(this.biomeId1);
		_bw.Write((byte)(this.intensity1 * 255f));
		_bw.Write(this.biomeId2);
		_bw.Write((byte)(this.intensity2 * 255f));
		_bw.Write(this.biomeId3);
		_bw.Write((byte)(this.intensity3 * 255f));
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x0025F5B0 File Offset: 0x0025D7B0
	public void Read(BinaryReader _br)
	{
		this.biomeId0 = _br.ReadByte();
		this.intensity0 = (float)_br.ReadByte() / 255f;
		this.biomeId1 = _br.ReadByte();
		this.intensity1 = (float)_br.ReadByte() / 255f;
		this.biomeId2 = _br.ReadByte();
		this.intensity2 = (float)_br.ReadByte() / 255f;
		this.biomeId3 = _br.ReadByte();
		this.intensity3 = (float)_br.ReadByte() / 255f;
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x0025F63C File Offset: 0x0025D83C
	public bool Equals(BiomeIntensity other)
	{
		return this.biomeId0 == other.biomeId0 && this.biomeId1 == other.biomeId1 && this.biomeId2 == other.biomeId2 && this.biomeId3 == other.biomeId3 && this.intensity0and1 == other.intensity0and1 && this.intensity2and3 == other.intensity2and3;
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x0025F69F File Offset: 0x0025D89F
	public override bool Equals(object obj)
	{
		return obj != null && obj is BiomeIntensity && this.Equals((BiomeIntensity)obj);
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x0025F6BC File Offset: 0x0025D8BC
	public override int GetHashCode()
	{
		return (((((((((this.biomeId0.GetHashCode() * 397) ^ this.biomeId1.GetHashCode()) * 397) ^ this.biomeId2.GetHashCode()) * 397) ^ this.biomeId3.GetHashCode()) * 397) ^ this.intensity0and1.GetHashCode()) * 397) ^ this.intensity2and3.GetHashCode();
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x0025F730 File Offset: 0x0025D930
	public override string ToString()
	{
		return string.Format("[b0={0} b1={1} i0={2} i1={3}]", new object[]
		{
			this.biomeId0,
			this.biomeId1,
			this.intensity0.ToCultureInvariantString("0.0"),
			this.intensity1.ToCultureInvariantString("0.0")
		});
	}

	// Token: 0x04004B27 RID: 19239
	public const int cDataSize = 6;

	// Token: 0x04004B28 RID: 19240
	public static BiomeIntensity Default = new BiomeIntensity(0);

	// Token: 0x04004B29 RID: 19241
	public byte biomeId0;

	// Token: 0x04004B2A RID: 19242
	public byte biomeId1;

	// Token: 0x04004B2B RID: 19243
	public byte biomeId2;

	// Token: 0x04004B2C RID: 19244
	public byte biomeId3;

	// Token: 0x04004B2D RID: 19245
	public byte intensity0and1;

	// Token: 0x04004B2E RID: 19246
	public byte intensity2and3;
}

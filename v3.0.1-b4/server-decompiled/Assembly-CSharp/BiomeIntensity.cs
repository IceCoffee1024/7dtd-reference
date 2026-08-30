using System;
using System.IO;

// Token: 0x02000C2F RID: 3119
public struct BiomeIntensity : IEquatable<BiomeIntensity>
{
	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x06005EED RID: 24301 RVA: 0x00259097 File Offset: 0x00257297
	// (set) Token: 0x06005EEE RID: 24302 RVA: 0x002590A9 File Offset: 0x002572A9
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

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x06005EEF RID: 24303 RVA: 0x002590CA File Offset: 0x002572CA
	// (set) Token: 0x06005EF0 RID: 24304 RVA: 0x002590DE File Offset: 0x002572DE
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

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06005EF1 RID: 24305 RVA: 0x00259101 File Offset: 0x00257301
	// (set) Token: 0x06005EF2 RID: 24306 RVA: 0x00259113 File Offset: 0x00257313
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

	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x06005EF3 RID: 24307 RVA: 0x00259134 File Offset: 0x00257334
	// (set) Token: 0x06005EF4 RID: 24308 RVA: 0x00259148 File Offset: 0x00257348
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

	// Token: 0x06005EF5 RID: 24309 RVA: 0x0025916B File Offset: 0x0025736B
	public BiomeIntensity(byte _singleBiomeId)
	{
		this.biomeId0 = _singleBiomeId;
		this.biomeId1 = 0;
		this.biomeId2 = 0;
		this.biomeId3 = 0;
		this.intensity0and1 = 15;
		this.intensity2and3 = 0;
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x00259198 File Offset: 0x00257398
	public BiomeIntensity(byte[] _chunkBiomeIntensityArray, int _offs)
	{
		this.biomeId0 = _chunkBiomeIntensityArray[_offs];
		this.biomeId1 = _chunkBiomeIntensityArray[_offs + 1];
		this.biomeId2 = _chunkBiomeIntensityArray[_offs + 2];
		this.biomeId3 = _chunkBiomeIntensityArray[_offs + 3];
		this.intensity0and1 = _chunkBiomeIntensityArray[_offs + 4];
		this.intensity2and3 = _chunkBiomeIntensityArray[_offs + 5];
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x002591E8 File Offset: 0x002573E8
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

	// Token: 0x06005EF8 RID: 24312 RVA: 0x00259328 File Offset: 0x00257528
	public void ToArray(byte[] _array, int offs)
	{
		_array[offs] = this.biomeId0;
		_array[1 + offs] = this.biomeId1;
		_array[2 + offs] = this.biomeId2;
		_array[3 + offs] = this.biomeId3;
		_array[4 + offs] = this.intensity0and1;
		_array[5 + offs] = this.intensity2and3;
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x00259378 File Offset: 0x00257578
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

	// Token: 0x06005EFA RID: 24314 RVA: 0x00259404 File Offset: 0x00257604
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

	// Token: 0x06005EFB RID: 24315 RVA: 0x00259490 File Offset: 0x00257690
	public bool Equals(BiomeIntensity other)
	{
		return this.biomeId0 == other.biomeId0 && this.biomeId1 == other.biomeId1 && this.biomeId2 == other.biomeId2 && this.biomeId3 == other.biomeId3 && this.intensity0and1 == other.intensity0and1 && this.intensity2and3 == other.intensity2and3;
	}

	// Token: 0x06005EFC RID: 24316 RVA: 0x002594F3 File Offset: 0x002576F3
	public override bool Equals(object obj)
	{
		return obj != null && obj is BiomeIntensity && this.Equals((BiomeIntensity)obj);
	}

	// Token: 0x06005EFD RID: 24317 RVA: 0x00259510 File Offset: 0x00257710
	public override int GetHashCode()
	{
		return (((((((((this.biomeId0.GetHashCode() * 397) ^ this.biomeId1.GetHashCode()) * 397) ^ this.biomeId2.GetHashCode()) * 397) ^ this.biomeId3.GetHashCode()) * 397) ^ this.intensity0and1.GetHashCode()) * 397) ^ this.intensity2and3.GetHashCode();
	}

	// Token: 0x06005EFE RID: 24318 RVA: 0x00259584 File Offset: 0x00257784
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

	// Token: 0x04004A4B RID: 19019
	public const int cDataSize = 6;

	// Token: 0x04004A4C RID: 19020
	public static BiomeIntensity Default = new BiomeIntensity(0);

	// Token: 0x04004A4D RID: 19021
	public byte biomeId0;

	// Token: 0x04004A4E RID: 19022
	public byte biomeId1;

	// Token: 0x04004A4F RID: 19023
	public byte biomeId2;

	// Token: 0x04004A50 RID: 19024
	public byte biomeId3;

	// Token: 0x04004A51 RID: 19025
	public byte intensity0and1;

	// Token: 0x04004A52 RID: 19026
	public byte intensity2and3;
}

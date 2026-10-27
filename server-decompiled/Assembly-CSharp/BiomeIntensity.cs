using System;
using System.IO;

// Token: 0x02000C5E RID: 3166
public struct BiomeIntensity : IEquatable<BiomeIntensity>
{
	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x06005FB5 RID: 24501 RVA: 0x0026076B File Offset: 0x0025E96B
	// (set) Token: 0x06005FB6 RID: 24502 RVA: 0x0026077D File Offset: 0x0025E97D
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

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x06005FB7 RID: 24503 RVA: 0x0026079E File Offset: 0x0025E99E
	// (set) Token: 0x06005FB8 RID: 24504 RVA: 0x002607B2 File Offset: 0x0025E9B2
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

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06005FB9 RID: 24505 RVA: 0x002607D5 File Offset: 0x0025E9D5
	// (set) Token: 0x06005FBA RID: 24506 RVA: 0x002607E7 File Offset: 0x0025E9E7
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

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06005FBB RID: 24507 RVA: 0x00260808 File Offset: 0x0025EA08
	// (set) Token: 0x06005FBC RID: 24508 RVA: 0x0026081C File Offset: 0x0025EA1C
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

	// Token: 0x06005FBD RID: 24509 RVA: 0x0026083F File Offset: 0x0025EA3F
	public BiomeIntensity(byte _singleBiomeId)
	{
		this.biomeId0 = _singleBiomeId;
		this.biomeId1 = 0;
		this.biomeId2 = 0;
		this.biomeId3 = 0;
		this.intensity0and1 = 15;
		this.intensity2and3 = 0;
	}

	// Token: 0x06005FBE RID: 24510 RVA: 0x0026086C File Offset: 0x0025EA6C
	public BiomeIntensity(byte[] _chunkBiomeIntensityArray, int _offs)
	{
		this.biomeId0 = _chunkBiomeIntensityArray[_offs];
		this.biomeId1 = _chunkBiomeIntensityArray[_offs + 1];
		this.biomeId2 = _chunkBiomeIntensityArray[_offs + 2];
		this.biomeId3 = _chunkBiomeIntensityArray[_offs + 3];
		this.intensity0and1 = _chunkBiomeIntensityArray[_offs + 4];
		this.intensity2and3 = _chunkBiomeIntensityArray[_offs + 5];
	}

	// Token: 0x06005FBF RID: 24511 RVA: 0x002608BC File Offset: 0x0025EABC
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

	// Token: 0x06005FC0 RID: 24512 RVA: 0x002609FC File Offset: 0x0025EBFC
	public void ToArray(byte[] _array, int offs)
	{
		_array[offs] = this.biomeId0;
		_array[1 + offs] = this.biomeId1;
		_array[2 + offs] = this.biomeId2;
		_array[3 + offs] = this.biomeId3;
		_array[4 + offs] = this.intensity0and1;
		_array[5 + offs] = this.intensity2and3;
	}

	// Token: 0x06005FC1 RID: 24513 RVA: 0x00260A4C File Offset: 0x0025EC4C
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

	// Token: 0x06005FC2 RID: 24514 RVA: 0x00260AD8 File Offset: 0x0025ECD8
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

	// Token: 0x06005FC3 RID: 24515 RVA: 0x00260B64 File Offset: 0x0025ED64
	public bool Equals(BiomeIntensity other)
	{
		return this.biomeId0 == other.biomeId0 && this.biomeId1 == other.biomeId1 && this.biomeId2 == other.biomeId2 && this.biomeId3 == other.biomeId3 && this.intensity0and1 == other.intensity0and1 && this.intensity2and3 == other.intensity2and3;
	}

	// Token: 0x06005FC4 RID: 24516 RVA: 0x00260BC7 File Offset: 0x0025EDC7
	public override bool Equals(object obj)
	{
		return obj != null && obj is BiomeIntensity && this.Equals((BiomeIntensity)obj);
	}

	// Token: 0x06005FC5 RID: 24517 RVA: 0x00260BE4 File Offset: 0x0025EDE4
	public override int GetHashCode()
	{
		return (((((((((this.biomeId0.GetHashCode() * 397) ^ this.biomeId1.GetHashCode()) * 397) ^ this.biomeId2.GetHashCode()) * 397) ^ this.biomeId3.GetHashCode()) * 397) ^ this.intensity0and1.GetHashCode()) * 397) ^ this.intensity2and3.GetHashCode();
	}

	// Token: 0x06005FC6 RID: 24518 RVA: 0x00260C58 File Offset: 0x0025EE58
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

	// Token: 0x04004B4C RID: 19276
	public const int cDataSize = 6;

	// Token: 0x04004B4D RID: 19277
	public static BiomeIntensity Default = new BiomeIntensity(0);

	// Token: 0x04004B4E RID: 19278
	public byte biomeId0;

	// Token: 0x04004B4F RID: 19279
	public byte biomeId1;

	// Token: 0x04004B50 RID: 19280
	public byte biomeId2;

	// Token: 0x04004B51 RID: 19281
	public byte biomeId3;

	// Token: 0x04004B52 RID: 19282
	public byte intensity0and1;

	// Token: 0x04004B53 RID: 19283
	public byte intensity2and3;
}

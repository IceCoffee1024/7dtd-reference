using System;
using System.IO;

// Token: 0x02000B0F RID: 2831
public class ChunkBlockLayerLegacy : IMemoryPoolableObject
{
	// Token: 0x0600555C RID: 21852 RVA: 0x0020A2C9 File Offset: 0x002084C9
	public ChunkBlockLayerLegacy()
	{
		this.wPow = 4;
		this.hPow = 4;
		this.m_Lower16Bits = new ushort[256];
		this.m_Stability = new SmartArray(this.wPow, 0, this.hPow);
	}

	// Token: 0x0600555D RID: 21853 RVA: 0x0020A308 File Offset: 0x00208508
	public BlockValue GetAt(int _x, int _y, int _z)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_x + (_z << this.wPow)] << 16) | (int)this.m_Lower16Bits[_x + (_z << this.wPow)]) : ((int)this.m_Lower16Bits[_x + (_z << this.wPow)])));
	}

	// Token: 0x0600555E RID: 21854 RVA: 0x0020A364 File Offset: 0x00208564
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue getAt(int _offs)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_offs] << 16) | (int)this.m_Lower16Bits[_offs]) : ((int)this.m_Lower16Bits[_offs])));
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x0020A394 File Offset: 0x00208594
	public void SetAt(int _x, int _y, int _z, uint _fullBlock)
	{
		int num = _x + (_z << this.wPow);
		uint typeMasked = BlockValue.GetTypeMasked((uint)this.m_Lower16Bits[num]);
		this.m_Lower16Bits[num] = (ushort)(_fullBlock & 65535U);
		if ((_fullBlock & 4294901760U) != 0U)
		{
			if (this.m_Upper16Bits == null)
			{
				this.m_Upper16Bits = new ushort[256];
			}
			this.m_Upper16Bits[num] = (ushort)((_fullBlock >> 16) & 65535U);
		}
		else if (this.m_Upper16Bits != null)
		{
			this.m_Upper16Bits[num] = 0;
		}
		if (!Block.BlocksLoaded)
		{
			return;
		}
		uint typeMasked2 = BlockValue.GetTypeMasked(_fullBlock);
		Block block = Block.list[(int)typeMasked];
		Block block2 = Block.list[(int)typeMasked2];
		if (typeMasked == 0U && typeMasked2 != 0U)
		{
			this.blockRefCount++;
			if (block2 != null && block2.IsRandomlyTick)
			{
				this.tickRefCount++;
			}
		}
		else if (typeMasked != 0U && typeMasked2 == 0U)
		{
			this.blockRefCount--;
			if (block != null && block.IsRandomlyTick)
			{
				this.tickRefCount--;
			}
		}
		else if (block != null && block.IsRandomlyTick && block2 != null && !block2.IsRandomlyTick)
		{
			this.tickRefCount--;
		}
		else if (block != null && !block.IsRandomlyTick && block2 != null && block2.IsRandomlyTick)
		{
			this.tickRefCount++;
		}
		if (this.bOnlyTerrain && !block2.shape.IsTerrain())
		{
			this.bOnlyTerrain = false;
		}
	}

	// Token: 0x06005560 RID: 21856 RVA: 0x0020A509 File Offset: 0x00208709
	public byte GetStabilityAt(int _x, int _y)
	{
		return this.m_Stability.get(_x, 0, _y);
	}

	// Token: 0x06005561 RID: 21857 RVA: 0x0020A519 File Offset: 0x00208719
	public void SetStabilityAt(int _x, int _y, byte _v)
	{
		this.m_Stability.set(_x, 0, _y, _v);
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x0020A52A File Offset: 0x0020872A
	public void Reset()
	{
		Array.Clear(this.m_Lower16Bits, 0, this.m_Lower16Bits.Length);
		this.m_Upper16Bits = null;
		if (this.m_Stability != null)
		{
			this.m_Stability.clear();
		}
		this.blockRefCount = 0;
		this.tickRefCount = 0;
	}

	// Token: 0x06005563 RID: 21859 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06005564 RID: 21860 RVA: 0x0020A568 File Offset: 0x00208768
	public static int CalcOffset(int _x, int _z)
	{
		return (_x & 15) + ((_z & 15) << 4);
	}

	// Token: 0x06005565 RID: 21861 RVA: 0x0020A575 File Offset: 0x00208775
	public static int OffsetX(int _offset)
	{
		return _offset & 15;
	}

	// Token: 0x06005566 RID: 21862 RVA: 0x0020A57B File Offset: 0x0020877B
	public static int OffsetY(int _offset)
	{
		return _offset >> 4;
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x0020A580 File Offset: 0x00208780
	public void UpdateRefCounts()
	{
		this.blockRefCount = 0;
		this.tickRefCount = 0;
		for (int i = this.m_Lower16Bits.Length - 1; i >= 0; i--)
		{
			int type = this.getAt(i).type;
			if (type > 0)
			{
				this.blockRefCount++;
				if (Block.list[type].IsRandomlyTick)
				{
					this.tickRefCount++;
				}
			}
		}
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x0020A5EE File Offset: 0x002087EE
	public int GetTickRefCount()
	{
		return this.tickRefCount;
	}

	// Token: 0x06005569 RID: 21865 RVA: 0x0020A5F6 File Offset: 0x002087F6
	public bool IsOnlyTerrain()
	{
		return this.bOnlyTerrain;
	}

	// Token: 0x0600556A RID: 21866 RVA: 0x0020A600 File Offset: 0x00208800
	public void Read(BinaryReader stream, uint _version, bool _bNetworkRead, byte[] _tempReadBuf)
	{
		if (_version >= 19U)
		{
			stream.Read(_tempReadBuf, 0, 512);
			for (int i = 0; i < this.m_Lower16Bits.Length; i++)
			{
				ushort num = (ushort)((int)_tempReadBuf[i * 2] | ((int)_tempReadBuf[i * 2 + 1] << 8));
				this.m_Lower16Bits[i] = num;
			}
			if (stream.ReadBoolean())
			{
				if (this.m_Upper16Bits == null)
				{
					this.m_Upper16Bits = new ushort[256];
				}
				stream.Read(_tempReadBuf, 0, 512);
				for (int j = 0; j < this.m_Upper16Bits.Length; j++)
				{
					ushort num2 = (ushort)((int)_tempReadBuf[j * 2] | ((int)_tempReadBuf[j * 2 + 1] << 8));
					this.m_Upper16Bits[j] = num2;
				}
			}
			else
			{
				this.m_Upper16Bits = null;
			}
		}
		else if (_version >= 5U && _version < 19U)
		{
			stream.Read(_tempReadBuf, 0, 1024);
			for (int k = 0; k < this.m_Lower16Bits.Length; k++)
			{
				uint num3 = (uint)((int)_tempReadBuf[k * 4] | ((int)_tempReadBuf[k * 4 + 1] << 8) | ((int)_tempReadBuf[k * 4 + 2] << 16) | ((int)_tempReadBuf[k * 4 + 3] << 24));
				this.m_Lower16Bits[k] = (ushort)(num3 & 65535U);
				if ((num3 & 4294901760U) != 0U)
				{
					if (this.m_Upper16Bits == null)
					{
						this.m_Upper16Bits = new ushort[256];
					}
					this.m_Upper16Bits[k] = (ushort)((num3 >> 16) & 65535U);
				}
				else if (this.m_Upper16Bits != null)
				{
					this.m_Upper16Bits[k] = 0;
				}
			}
		}
		if (_version > 8U && _version < 18U && !_bNetworkRead)
		{
			byte[] array = new byte[256];
			stream.Read(array, 0, array.Length);
			for (int l = 0; l < 16; l++)
			{
				for (int m = 0; m < 16; m++)
				{
					this.m_Stability.set(l, 0, m, array[l + m * 16]);
				}
			}
		}
		if (_version >= 18U && _version < 28U && !_bNetworkRead)
		{
			this.m_Stability.read(stream);
		}
		this.CheckOnlyTerrain();
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x0020A80C File Offset: 0x00208A0C
	public void Write(BinaryWriter _bw, bool _bNetworkWrite, byte[] _tempSaveBuf)
	{
		for (int i = 0; i < this.m_Lower16Bits.Length; i++)
		{
			uint num = (uint)this.m_Lower16Bits[i];
			_tempSaveBuf[i * 2] = (byte)(num & 255U);
			_tempSaveBuf[i * 2 + 1] = (byte)((num >> 8) & 255U);
		}
		_bw.Write(_tempSaveBuf, 0, this.m_Lower16Bits.Length * 2);
		_bw.Write(this.m_Upper16Bits != null);
		if (this.m_Upper16Bits != null)
		{
			for (int j = 0; j < this.m_Upper16Bits.Length; j++)
			{
				uint num2 = (uint)this.m_Upper16Bits[j];
				_tempSaveBuf[j * 2] = (byte)(num2 & 255U);
				_tempSaveBuf[j * 2 + 1] = (byte)((num2 >> 8) & 255U);
			}
			_bw.Write(_tempSaveBuf, 0, this.m_Upper16Bits.Length * 2);
		}
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x0020A8C8 File Offset: 0x00208AC8
	public void CheckOnlyTerrain()
	{
		this.bOnlyTerrain = true;
		uint num = BlockValue.GetTypeMasked((uint)this.m_Lower16Bits[0]);
		for (int i = 0; i < this.m_Lower16Bits.Length; i++)
		{
			num = BlockValue.GetTypeMasked((uint)this.m_Lower16Bits[i]);
			if (num != 0U && !Block.list[(int)num].shape.IsTerrain())
			{
				this.bOnlyTerrain = false;
				return;
			}
		}
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x0020A929 File Offset: 0x00208B29
	public int GetUsedMem()
	{
		return this.m_Lower16Bits.Length * 2 + ((this.m_Upper16Bits != null) ? (this.m_Upper16Bits.Length * 2) : 0) + 20 + 2;
	}

	// Token: 0x0400423B RID: 16955
	public ushort[] m_Lower16Bits;

	// Token: 0x0400423C RID: 16956
	public ushort[] m_Upper16Bits;

	// Token: 0x0400423D RID: 16957
	[PublicizedFrom(EAccessModifier.Private)]
	public SmartArray m_Stability;

	// Token: 0x0400423E RID: 16958
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x0400423F RID: 16959
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x04004240 RID: 16960
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyTerrain;

	// Token: 0x04004241 RID: 16961
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyBlocks;

	// Token: 0x04004242 RID: 16962
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockRefCount;

	// Token: 0x04004243 RID: 16963
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickRefCount;

	// Token: 0x04004244 RID: 16964
	public static int InstanceCount;

	// Token: 0x04004245 RID: 16965
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLayerHeight = 1;

	// Token: 0x04004246 RID: 16966
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cArrSize = 256;
}

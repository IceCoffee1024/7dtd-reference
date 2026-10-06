using System;
using System.IO;

// Token: 0x02000B21 RID: 2849
public class ChunkBlockLayerLegacy : IMemoryPoolableObject
{
	// Token: 0x06005575 RID: 21877 RVA: 0x0020D539 File Offset: 0x0020B739
	public ChunkBlockLayerLegacy()
	{
		this.wPow = 4;
		this.hPow = 4;
		this.m_Lower16Bits = new ushort[256];
		this.m_Stability = new SmartArray(this.wPow, 0, this.hPow);
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x0020D578 File Offset: 0x0020B778
	public BlockValue GetAt(int _x, int _y, int _z)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_x + (_z << this.wPow)] << 16) | (int)this.m_Lower16Bits[_x + (_z << this.wPow)]) : ((int)this.m_Lower16Bits[_x + (_z << this.wPow)])));
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x0020D5D4 File Offset: 0x0020B7D4
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue getAt(int _offs)
	{
		return new BlockValue((uint)((this.m_Upper16Bits != null) ? (((int)this.m_Upper16Bits[_offs] << 16) | (int)this.m_Lower16Bits[_offs]) : ((int)this.m_Lower16Bits[_offs])));
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x0020D604 File Offset: 0x0020B804
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

	// Token: 0x06005579 RID: 21881 RVA: 0x0020D779 File Offset: 0x0020B979
	public byte GetStabilityAt(int _x, int _y)
	{
		return this.m_Stability.get(_x, 0, _y);
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x0020D789 File Offset: 0x0020B989
	public void SetStabilityAt(int _x, int _y, byte _v)
	{
		this.m_Stability.set(_x, 0, _y, _v);
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x0020D79A File Offset: 0x0020B99A
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

	// Token: 0x0600557C RID: 21884 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x0020D7D8 File Offset: 0x0020B9D8
	public static int CalcOffset(int _x, int _z)
	{
		return (_x & 15) + ((_z & 15) << 4);
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x0020D7E5 File Offset: 0x0020B9E5
	public static int OffsetX(int _offset)
	{
		return _offset & 15;
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x0020D7EB File Offset: 0x0020B9EB
	public static int OffsetY(int _offset)
	{
		return _offset >> 4;
	}

	// Token: 0x06005580 RID: 21888 RVA: 0x0020D7F0 File Offset: 0x0020B9F0
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

	// Token: 0x06005581 RID: 21889 RVA: 0x0020D85E File Offset: 0x0020BA5E
	public int GetTickRefCount()
	{
		return this.tickRefCount;
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x0020D866 File Offset: 0x0020BA66
	public bool IsOnlyTerrain()
	{
		return this.bOnlyTerrain;
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x0020D870 File Offset: 0x0020BA70
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

	// Token: 0x06005584 RID: 21892 RVA: 0x0020DA7C File Offset: 0x0020BC7C
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

	// Token: 0x06005585 RID: 21893 RVA: 0x0020DB38 File Offset: 0x0020BD38
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

	// Token: 0x06005586 RID: 21894 RVA: 0x0020DB99 File Offset: 0x0020BD99
	public int GetUsedMem()
	{
		return this.m_Lower16Bits.Length * 2 + ((this.m_Upper16Bits != null) ? (this.m_Upper16Bits.Length * 2) : 0) + 20 + 2;
	}

	// Token: 0x04004278 RID: 17016
	public ushort[] m_Lower16Bits;

	// Token: 0x04004279 RID: 17017
	public ushort[] m_Upper16Bits;

	// Token: 0x0400427A RID: 17018
	[PublicizedFrom(EAccessModifier.Private)]
	public SmartArray m_Stability;

	// Token: 0x0400427B RID: 17019
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x0400427C RID: 17020
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x0400427D RID: 17021
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyTerrain;

	// Token: 0x0400427E RID: 17022
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyBlocks;

	// Token: 0x0400427F RID: 17023
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockRefCount;

	// Token: 0x04004280 RID: 17024
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickRefCount;

	// Token: 0x04004281 RID: 17025
	public static int InstanceCount;

	// Token: 0x04004282 RID: 17026
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLayerHeight = 1;

	// Token: 0x04004283 RID: 17027
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cArrSize = 256;
}

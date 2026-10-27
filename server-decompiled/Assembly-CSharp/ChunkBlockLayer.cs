using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000B2A RID: 2858
public class ChunkBlockLayer : IMemoryPoolableObject
{
	// Token: 0x06005578 RID: 21880 RVA: 0x0020DA80 File Offset: 0x0020BC80
	[PublicizedFrom(EAccessModifier.Private)]
	public static int EstimateOwnedBytes(object _obj)
	{
		ChunkBlockLayer chunkBlockLayer = (ChunkBlockLayer)_obj;
		int num = 0;
		object obj = chunkBlockLayer.lockObj;
		lock (obj)
		{
			num += MemoryTracker.GetSize<byte>(chunkBlockLayer.m_Lower8Bits);
			num += MemoryTracker.GetSize<byte>(chunkBlockLayer.m_Upper24Bits);
		}
		return num;
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x0020DAE0 File Offset: 0x0020BCE0
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] allocArray8Bit(bool _bClear, byte _val)
	{
		List<byte[]> poolCBLLower8BitArrCache = MemoryPools.poolCBLLower8BitArrCache;
		byte[] array2;
		lock (poolCBLLower8BitArrCache)
		{
			byte[] array;
			if (MemoryPools.poolCBLLower8BitArrCache.Count == 0)
			{
				array = new byte[1024];
			}
			else
			{
				array = MemoryPools.poolCBLLower8BitArrCache[MemoryPools.poolCBLLower8BitArrCache.Count - 1];
				MemoryPools.poolCBLLower8BitArrCache.RemoveAt(MemoryPools.poolCBLLower8BitArrCache.Count - 1);
			}
			if (_bClear)
			{
				Utils.Memset(array, _val, array.Length);
			}
			array2 = array;
		}
		return array2;
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x0020DB74 File Offset: 0x0020BD74
	[PublicizedFrom(EAccessModifier.Private)]
	public void freeArray8Bit(byte[] _array)
	{
		List<byte[]> poolCBLLower8BitArrCache = MemoryPools.poolCBLLower8BitArrCache;
		lock (poolCBLLower8BitArrCache)
		{
			if (_array != null && MemoryPools.poolCBLLower8BitArrCache.Count < 10000)
			{
				MemoryPools.poolCBLLower8BitArrCache.Add(_array);
			}
		}
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x0020DBCC File Offset: 0x0020BDCC
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] allocArray24Bit(bool _bClear)
	{
		List<byte[]> poolCBLUpper24BitArrCache = MemoryPools.poolCBLUpper24BitArrCache;
		byte[] array2;
		lock (poolCBLUpper24BitArrCache)
		{
			byte[] array;
			if (MemoryPools.poolCBLUpper24BitArrCache.Count == 0)
			{
				array = new byte[3072];
			}
			else
			{
				array = MemoryPools.poolCBLUpper24BitArrCache[MemoryPools.poolCBLUpper24BitArrCache.Count - 1];
				MemoryPools.poolCBLUpper24BitArrCache.RemoveAt(MemoryPools.poolCBLUpper24BitArrCache.Count - 1);
			}
			if (_bClear)
			{
				Utils.Memset(array, 0, array.Length);
			}
			array2 = array;
		}
		return array2;
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x0020DC60 File Offset: 0x0020BE60
	[PublicizedFrom(EAccessModifier.Private)]
	public void freeArray24Bit(byte[] _array)
	{
		List<byte[]> poolCBLUpper24BitArrCache = MemoryPools.poolCBLUpper24BitArrCache;
		lock (poolCBLUpper24BitArrCache)
		{
			if (_array != null && MemoryPools.poolCBLUpper24BitArrCache.Count < 10000)
			{
				MemoryPools.poolCBLUpper24BitArrCache.Add(_array);
			}
		}
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x0020DCB8 File Offset: 0x0020BEB8
	public static int GetTempBufSize()
	{
		return 3072;
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x0020DCC0 File Offset: 0x0020BEC0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BlockValue GetAt(int _x, int _y, int _z)
	{
		int num = _x + (_z << 4) + (_y & 3) * 256;
		return this.GetAt(num);
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x0020DCE4 File Offset: 0x0020BEE4
	public BlockValue GetAt(int offs)
	{
		uint num = (uint)this.lower8BitSameValue;
		if (this.m_Lower8Bits != null)
		{
			num = (uint)this.m_Lower8Bits[offs];
		}
		if (this.m_Upper24Bits != null)
		{
			int num2 = offs * 3;
			num |= (uint)(((int)this.m_Upper24Bits[num2] << 8) | ((int)this.m_Upper24Bits[num2 + 1] << 16) | ((int)this.m_Upper24Bits[num2 + 2] << 24));
		}
		BlockValue air = new BlockValue(num);
		if (air.type >= Block.list.Length || air.Block == null)
		{
			air = BlockValue.Air;
		}
		return air;
	}

	// Token: 0x06005580 RID: 21888 RVA: 0x0020DD68 File Offset: 0x0020BF68
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetIdAt(int _x, int _y, int _z)
	{
		int num = _x + (_z << 4) + (_y & 3) * 16 * 16;
		return this.GetIdAt(num);
	}

	// Token: 0x06005581 RID: 21889 RVA: 0x0020DD8C File Offset: 0x0020BF8C
	public int GetIdAt(int offs)
	{
		uint num = (uint)this.lower8BitSameValue;
		if (this.m_Lower8Bits != null)
		{
			num = (uint)this.m_Lower8Bits[offs];
		}
		if (this.m_Upper24Bits != null)
		{
			int num2 = offs * 3;
			num |= (uint)((uint)this.m_Upper24Bits[num2] << 8);
			num &= 65535U;
		}
		return (int)num;
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x0020DDD3 File Offset: 0x0020BFD3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int CalcOffset(int _x, int _y, int _z)
	{
		return _x + (_z << 4) + (_y & 3) * 256;
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x0020DDE4 File Offset: 0x0020BFE4
	public void SetAt(int _x, int _y, int _z, uint _fullBlock)
	{
		int num = _x + (_z << 4) + (_y & 3) * 256;
		this.SetAt(num, _fullBlock);
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x0020DE0C File Offset: 0x0020C00C
	public void SetAt(int offs, uint _fullBlock)
	{
		uint num = (uint)((this.m_Lower8Bits != null) ? this.m_Lower8Bits[offs] : this.lower8BitSameValue);
		if (this.m_Upper24Bits != null)
		{
			num |= (uint)((uint)this.m_Upper24Bits[offs * 3] << 8);
			num &= 65535U;
		}
		byte b = (byte)_fullBlock;
		if (this.m_Lower8Bits == null && this.lower8BitSameValue != b)
		{
			this.m_Lower8Bits = this.allocArray8Bit(true, this.lower8BitSameValue);
		}
		if (this.m_Lower8Bits != null)
		{
			this.m_Lower8Bits[offs] = b;
		}
		if ((_fullBlock & 4294967040U) != 0U)
		{
			if (this.m_Upper24Bits == null)
			{
				this.m_Upper24Bits = this.allocArray24Bit(true);
			}
			this.m_Upper24Bits[offs * 3] = (byte)(_fullBlock >> 8);
			this.m_Upper24Bits[offs * 3 + 1] = (byte)(_fullBlock >> 16);
			this.m_Upper24Bits[offs * 3 + 2] = (byte)(_fullBlock >> 24);
		}
		else if (this.m_Upper24Bits != null)
		{
			this.m_Upper24Bits[offs * 3] = 0;
			this.m_Upper24Bits[offs * 3 + 1] = 0;
			this.m_Upper24Bits[offs * 3 + 2] = 0;
		}
		if (!Block.BlocksLoaded)
		{
			return;
		}
		uint num2 = _fullBlock & 65535U;
		Block block = Block.list[(int)num];
		Block block2 = Block.list[(int)num2];
		if (num == 0U && num2 != 0U)
		{
			this.blockRefCount++;
			if (block2 != null && block2.IsRandomlyTick)
			{
				this.tickRefCount++;
			}
		}
		else if (num != 0U && num2 == 0U)
		{
			this.blockRefCount--;
			if (block != null && block.IsRandomlyTick)
			{
				this.tickRefCount--;
			}
		}
		else if (block != null)
		{
			if (block.IsRandomlyTick && block2 != null && !block2.IsRandomlyTick)
			{
				this.tickRefCount--;
			}
			else if (!block.IsRandomlyTick && block2 != null && block2.IsRandomlyTick)
			{
				this.tickRefCount++;
			}
		}
		if (block2 != null && block2.IsNotifyOnLoadUnload)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.notifyLoadUnloadCallbackBlocks == null)
				{
					this.notifyLoadUnloadCallbackBlocks = new HashSet<int>();
				}
				if (!this.notifyLoadUnloadCallbackBlocks.Contains(offs))
				{
					this.notifyLoadUnloadCallbackBlocks.Add(offs);
				}
				goto IL_025E;
			}
		}
		if (block != null && block.IsNotifyOnLoadUnload && this.notifyLoadUnloadCallbackBlocks != null)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.notifyLoadUnloadCallbackBlocks.Remove(offs);
			}
		}
		IL_025E:
		if (this.bOnlyTerrain && block2 != null && !block2.shape.IsTerrain())
		{
			this.bOnlyTerrain = false;
		}
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x0020E0B4 File Offset: 0x0020C2B4
	public void Fill(uint _fullBlock)
	{
		uint num = _fullBlock & 65535U;
		Block block = Block.list[(int)num];
		this.freeArray8Bit(this.m_Lower8Bits);
		this.m_Lower8Bits = null;
		this.lower8BitSameValue = (byte)_fullBlock;
		if ((_fullBlock & 4294967040U) != 0U)
		{
			if (this.m_Upper24Bits == null)
			{
				this.m_Upper24Bits = this.allocArray24Bit(true);
			}
			byte b = (byte)(_fullBlock >> 8);
			byte b2 = (byte)(_fullBlock >> 16);
			byte b3 = (byte)(_fullBlock >> 24);
			for (int i = 0; i < this.m_Upper24Bits.Length; i += 3)
			{
				this.m_Upper24Bits[i] = b;
				this.m_Upper24Bits[i + 1] = b2;
				this.m_Upper24Bits[i + 2] = b3;
			}
		}
		else
		{
			this.freeArray24Bit(this.m_Upper24Bits);
			this.m_Upper24Bits = null;
		}
		this.bOnlyTerrain = block.shape.IsTerrain();
		object obj = this.lockObj;
		lock (obj)
		{
			if (this.notifyLoadUnloadCallbackBlocks != null)
			{
				this.notifyLoadUnloadCallbackBlocks.Clear();
			}
			else if (block.IsNotifyOnLoadUnload)
			{
				this.notifyLoadUnloadCallbackBlocks = new HashSet<int>();
			}
			if (block.IsNotifyOnLoadUnload)
			{
				for (int j = 0; j < 1024; j++)
				{
					this.notifyLoadUnloadCallbackBlocks.Add(j);
				}
			}
		}
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x0020E200 File Offset: 0x0020C400
	public void Reset()
	{
		this.freeArray8Bit(this.m_Lower8Bits);
		this.m_Lower8Bits = null;
		this.lower8BitSameValue = 0;
		this.freeArray24Bit(this.m_Upper24Bits);
		this.m_Upper24Bits = null;
		this.blockRefCount = 0;
		this.tickRefCount = 0;
		object obj = this.lockObj;
		lock (obj)
		{
			if (this.notifyLoadUnloadCallbackBlocks != null)
			{
				this.notifyLoadUnloadCallbackBlocks.Clear();
			}
		}
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x0020E288 File Offset: 0x0020C488
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRefCounts()
	{
		this.blockRefCount = 0;
		this.tickRefCount = 0;
		for (int i = 1023; i >= 0; i--)
		{
			int idAt = this.GetIdAt(i);
			if (idAt > 0 && idAt < Block.list.Length)
			{
				Block block = Block.list[idAt];
				if (block == null)
				{
					this.SetAt(i, 0U);
				}
				else
				{
					this.blockRefCount++;
					if (block.IsRandomlyTick)
					{
						this.tickRefCount++;
					}
					if (block.IsNotifyOnLoadUnload)
					{
						object obj = this.lockObj;
						lock (obj)
						{
							if (this.notifyLoadUnloadCallbackBlocks == null)
							{
								this.notifyLoadUnloadCallbackBlocks = new HashSet<int>();
							}
							this.notifyLoadUnloadCallbackBlocks.Add(i);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x0020E368 File Offset: 0x0020C568
	public void OnLoad(WorldBase _world, int _x, int _y, int _z)
	{
		if (this.notifyLoadUnloadCallbackBlocks != null)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				foreach (int num in this.notifyLoadUnloadCallbackBlocks)
				{
					BlockValue at = this.GetAt(num);
					int num2 = num / 256 + _y;
					int num3 = num % 256;
					int num4 = num3 % 16 + _x;
					int num5 = num3 / 16 + _z;
					at.Block.OnBlockLoaded(_world, new Vector3i(num4, num2, num5), at);
				}
			}
		}
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x0020E42C File Offset: 0x0020C62C
	public void OnUnload(WorldBase _world, int _x, int _y, int _z)
	{
		if (this.notifyLoadUnloadCallbackBlocks != null)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				foreach (int num in this.notifyLoadUnloadCallbackBlocks)
				{
					BlockValue at = this.GetAt(num);
					int num2 = num / 256 + _y;
					int num3 = num % 256;
					int num4 = num3 % 16 + _x;
					int num5 = num3 / 16 + _z;
					at.Block.OnBlockUnloaded(_world, new Vector3i(num4, num2, num5), at);
				}
			}
		}
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x0020E4F0 File Offset: 0x0020C6F0
	public int GetTickRefCount()
	{
		return this.tickRefCount;
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x0020E4F8 File Offset: 0x0020C6F8
	public bool IsOnlyTerrain()
	{
		return this.bOnlyTerrain;
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x0020E500 File Offset: 0x0020C700
	public void AddIndexedBlocks(int _curLayerIdx, DictionarySave<string, List<Vector3i>> _indexedBlocksDict)
	{
		for (int i = 0; i < 1024; i++)
		{
			int idAt = this.GetIdAt(i);
			Block block = Block.list[idAt];
			if (block != null && block.IndexName != null)
			{
				BlockValue at = this.GetAt(i);
				if (block.FilterIndexType(at))
				{
					if (!_indexedBlocksDict.ContainsKey(block.IndexName))
					{
						_indexedBlocksDict[block.IndexName] = new List<Vector3i>();
					}
					int num = (_curLayerIdx << 2) + i / 256;
					int num2 = i % 256 % 16;
					int num3 = i % 256 / 16;
					_indexedBlocksDict[block.IndexName].Add(new Vector3i(num2, num, num3));
				}
			}
		}
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x0020E5B0 File Offset: 0x0020C7B0
	public void Read(BinaryReader stream, uint _version, bool _bNetworkRead)
	{
		if (_version < 30U)
		{
			throw new Exception("Chunk version " + _version.ToString() + " not supported any more!");
		}
		if (stream.ReadBoolean())
		{
			if (this.m_Lower8Bits == null)
			{
				this.m_Lower8Bits = this.allocArray8Bit(false, 0);
			}
			stream.Read(this.m_Lower8Bits, 0, 1024);
		}
		else
		{
			if (this.m_Lower8Bits != null)
			{
				this.freeArray8Bit(this.m_Lower8Bits);
				this.m_Lower8Bits = null;
			}
			this.lower8BitSameValue = stream.ReadByte();
		}
		if (stream.ReadBoolean())
		{
			if (this.m_Upper24Bits == null)
			{
				this.m_Upper24Bits = this.allocArray24Bit(false);
			}
			stream.Read(this.m_Upper24Bits, 0, 3072);
		}
		else if (this.m_Upper24Bits != null)
		{
			this.freeArray24Bit(this.m_Upper24Bits);
			this.m_Upper24Bits = null;
		}
		this.updateRefCounts();
		this.CheckOnlyTerrain();
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x0020E694 File Offset: 0x0020C894
	public void Write(BinaryWriter _bw, bool _bNetworkWrite)
	{
		_bw.Write(this.m_Lower8Bits != null);
		if (this.m_Lower8Bits != null)
		{
			_bw.Write(this.m_Lower8Bits, 0, 1024);
		}
		else
		{
			_bw.Write(this.lower8BitSameValue);
		}
		_bw.Write(this.m_Upper24Bits != null);
		if (this.m_Upper24Bits != null)
		{
			_bw.Write(this.m_Upper24Bits, 0, 3072);
		}
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x0020E704 File Offset: 0x0020C904
	public void CopyFrom(ChunkBlockLayer _other)
	{
		if (_other.m_Lower8Bits != null)
		{
			if (this.m_Lower8Bits == null)
			{
				this.m_Lower8Bits = this.allocArray8Bit(true, 0);
			}
			Array.Copy(_other.m_Lower8Bits, this.m_Lower8Bits, this.m_Lower8Bits.Length);
		}
		else if (this.m_Lower8Bits != null)
		{
			this.freeArray8Bit(this.m_Lower8Bits);
		}
		if (_other.m_Upper24Bits != null)
		{
			if (this.m_Upper24Bits == null)
			{
				this.m_Upper24Bits = this.allocArray24Bit(true);
			}
			Array.Copy(_other.m_Upper24Bits, this.m_Upper24Bits, this.m_Upper24Bits.Length);
		}
		else if (this.m_Upper24Bits != null)
		{
			this.freeArray24Bit(this.m_Upper24Bits);
		}
		this.bOnlyTerrain = _other.bOnlyTerrain;
		this.blockRefCount = _other.blockRefCount;
		this.tickRefCount = _other.tickRefCount;
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x0020E7D0 File Offset: 0x0020C9D0
	public void CheckOnlyTerrain()
	{
		if (this.m_Upper24Bits != null)
		{
			bool flag = this.m_Upper24Bits[0] == 0;
			int num = 1;
			while (flag && num < this.m_Upper24Bits.Length)
			{
				flag &= this.m_Upper24Bits[num] == 0;
				num++;
			}
			if (flag)
			{
				this.freeArray24Bit(this.m_Upper24Bits);
				this.m_Upper24Bits = null;
			}
		}
		this.bOnlyTerrain = this.m_Upper24Bits == null;
		if (this.m_Lower8Bits == null)
		{
			this.bOnlyTerrain &= this.lower8BitSameValue > 0 && this.lower8BitSameValue <= 128;
			return;
		}
		if (this.bOnlyTerrain)
		{
			for (int i = 0; i < this.m_Lower8Bits.Length; i++)
			{
				uint num2 = (uint)this.m_Lower8Bits[i];
				if (num2 > 128U || num2 == 0U)
				{
					this.bOnlyTerrain = false;
					break;
				}
			}
		}
		bool flag2 = true;
		this.lower8BitSameValue = this.m_Lower8Bits[0];
		for (int j = 1; j < this.m_Lower8Bits.Length; j++)
		{
			if (this.lower8BitSameValue != this.m_Lower8Bits[j])
			{
				flag2 = false;
				this.lower8BitSameValue = 0;
				break;
			}
		}
		if (flag2)
		{
			this.freeArray8Bit(this.m_Lower8Bits);
			this.m_Lower8Bits = null;
			this.bOnlyTerrain &= this.lower8BitSameValue > 0;
		}
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x0020E918 File Offset: 0x0020CB18
	public void LoopOverAllBlocks(Chunk _c, int _yPos, ChunkBlockLayer.LoopBlocksDelegate _delegate, bool _bIncludeChilds = false, bool _bIncludeAirBlocks = false)
	{
		for (int i = 0; i < 1024; i++)
		{
			BlockValue at = this.GetAt(i);
			if ((_bIncludeAirBlocks || !at.isair) && (_bIncludeChilds || !at.ischild))
			{
				int num = i / 256 + _yPos;
				int num2 = i % 256;
				int num3 = num2 % 16;
				int num4 = num2 / 16;
				at.damage = _c.GetDamage(num3, num, num4);
				_delegate(num3, num, num4, at);
			}
		}
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x0020E98E File Offset: 0x0020CB8E
	public int GetUsedMem()
	{
		return ((this.m_Lower8Bits != null) ? this.m_Lower8Bits.Length : 1) + ((this.m_Upper24Bits != null) ? this.m_Upper24Bits.Length : 0) + 20 + 2;
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x0020E9BC File Offset: 0x0020CBBC
	public void SaveBlockMappings(NameIdMapping _mappings)
	{
		if (this.m_Lower8Bits == null && this.m_Upper24Bits == null)
		{
			Block block = this.GetAt(0).Block;
			_mappings.AddMapping(block.blockID, block.GetBlockName(), false);
			return;
		}
		Array.Clear(ChunkBlockLayer.saved, 0, Block.MAX_BLOCKS);
		bool flag = this.m_Lower8Bits != null;
		bool flag2 = this.m_Upper24Bits != null;
		for (int i = 0; i < 1024; i++)
		{
			int num = (int)(flag ? this.m_Lower8Bits[i] : this.lower8BitSameValue);
			num |= (flag2 ? (((int)this.m_Upper24Bits[i * 3] << 8) & 65280) : 0);
			num &= 65535;
			if (!ChunkBlockLayer.saved[num])
			{
				Block block2 = Block.list[num];
				if (block2 != null)
				{
					_mappings.AddMapping(num, block2.GetBlockName(), false);
				}
				ChunkBlockLayer.saved[num] = true;
			}
		}
	}

	// Token: 0x04004290 RID: 17040
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWPow = 4;

	// Token: 0x04004291 RID: 17041
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLayerHeight = 4;

	// Token: 0x04004292 RID: 17042
	public const int cArrSize = 1024;

	// Token: 0x04004293 RID: 17043
	public static int InstanceCount;

	// Token: 0x04004294 RID: 17044
	[PublicizedFrom(EAccessModifier.Private)]
	public byte lower8BitSameValue;

	// Token: 0x04004295 RID: 17045
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_Lower8Bits;

	// Token: 0x04004296 RID: 17046
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_Upper24Bits;

	// Token: 0x04004297 RID: 17047
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOnlyTerrain;

	// Token: 0x04004298 RID: 17048
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockRefCount;

	// Token: 0x04004299 RID: 17049
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickRefCount;

	// Token: 0x0400429A RID: 17050
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();

	// Token: 0x0400429B RID: 17051
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> notifyLoadUnloadCallbackBlocks;

	// Token: 0x0400429C RID: 17052
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool[] saved = new bool[Block.MAX_BLOCKS];

	// Token: 0x02000B2B RID: 2859
	// (Invoke) Token: 0x06005597 RID: 21911
	public delegate void LoopBlocksDelegate(int x, int y, int z, BlockValue bv);
}

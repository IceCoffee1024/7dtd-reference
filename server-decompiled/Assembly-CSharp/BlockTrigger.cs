using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class BlockTrigger
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0005BAE8 File Offset: 0x00059CE8
	// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0005BAF0 File Offset: 0x00059CF0
	public Chunk Chunk
	{
		get
		{
			return this.chunk;
		}
		set
		{
			this.chunk = value;
			long num = 0L;
			if (this.chunk != null)
			{
				num = this.chunk.Key;
			}
			this.chunkKey = num;
		}
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x0005BB22 File Offset: 0x00059D22
	public BlockTrigger(Chunk chunkNew)
	{
		this.Chunk = chunkNew;
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0005BB52 File Offset: 0x00059D52
	public BlockValue BlockValue
	{
		get
		{
			return this.Chunk.GetBlock(this.LocalChunkPos);
		}
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0005BB68 File Offset: 0x00059D68
	public void Refresh(FastTags<TagGroup.Global> questTag)
	{
		this.chunk = (Chunk)GameManager.Instance.World.GetChunkSync(this.chunkKey);
		if (this.chunk == null)
		{
			string text = "BlockTrigger.Refresh: Chunk null. ChunkKey={0}, LocalChunkPos={1}, PrefabInstance={2}. From: {3}";
			object[] array = new object[4];
			array[0] = this.chunkKey;
			array[1] = this.LocalChunkPos;
			int num = 2;
			PrefabTriggerData triggerDataOwner = this.TriggerDataOwner;
			object obj;
			if (triggerDataOwner == null)
			{
				obj = null;
			}
			else
			{
				PrefabInstance prefabInstance = triggerDataOwner.PrefabInstance;
				obj = ((prefabInstance != null) ? prefabInstance.name : null);
			}
			array[num] = obj;
			array[3] = StackTraceUtility.ExtractStackTrace();
			Log.Error(string.Format(text, array));
			return;
		}
		BlockValue blockValue = this.BlockValue;
		blockValue.Block.OnTriggerRefresh(this, blockValue, questTag);
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0005BC10 File Offset: 0x00059E10
	public void Read(PooledBinaryReader _br)
	{
		this.currentVersion = _br.ReadUInt16();
		if (this.currentVersion >= 2)
		{
			this.NeedsTriggered = (BlockTrigger.TriggeredStates)_br.ReadByte();
		}
		int num = (int)_br.ReadByte();
		this.TriggersIndices.Clear();
		for (int i = 0; i < num; i++)
		{
			this.TriggersIndices.Add(_br.ReadByte());
		}
		num = (int)_br.ReadByte();
		this.TriggeredByIndices.Clear();
		for (int j = 0; j < num; j++)
		{
			this.TriggeredByIndices.Add(_br.ReadByte());
		}
		num = (int)_br.ReadByte();
		this.TriggeredValues.Clear();
		for (int k = 0; k < num; k++)
		{
			this.TriggeredValues.Add(_br.ReadByte());
		}
		if (this.currentVersion >= 3)
		{
			this.ExcludeIcon = _br.ReadBoolean();
		}
		if (this.currentVersion >= 4)
		{
			this.UseOrForMultipleTriggers = _br.ReadBoolean();
		}
		if (this.currentVersion >= 5)
		{
			this.Unlock = _br.ReadBoolean();
		}
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0005BD0C File Offset: 0x00059F0C
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(5);
		_bw.Write((byte)this.NeedsTriggered);
		_bw.Write((byte)this.TriggersIndices.Count);
		for (int i = 0; i < this.TriggersIndices.Count; i++)
		{
			_bw.Write(this.TriggersIndices[i]);
		}
		_bw.Write((byte)this.TriggeredByIndices.Count);
		for (int j = 0; j < this.TriggeredByIndices.Count; j++)
		{
			_bw.Write(this.TriggeredByIndices[j]);
		}
		_bw.Write((byte)this.TriggeredValues.Count);
		for (int k = 0; k < this.TriggeredValues.Count; k++)
		{
			_bw.Write(this.TriggeredValues[k]);
		}
		_bw.Write(this.ExcludeIcon);
		_bw.Write(this.UseOrForMultipleTriggers);
		_bw.Write(this.Unlock);
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x0005BE00 File Offset: 0x0005A000
	public BlockTrigger Clone()
	{
		BlockTrigger blockTrigger = new BlockTrigger(this.Chunk);
		blockTrigger.LocalChunkPos = this.LocalChunkPos;
		blockTrigger.TriggersIndices.Clear();
		blockTrigger.TriggeredByIndices.Clear();
		blockTrigger.TriggeredValues.Clear();
		for (int i = 0; i < this.TriggersIndices.Count; i++)
		{
			blockTrigger.TriggersIndices.Add(this.TriggersIndices[i]);
		}
		for (int j = 0; j < this.TriggeredByIndices.Count; j++)
		{
			blockTrigger.TriggeredByIndices.Add(this.TriggeredByIndices[j]);
		}
		for (int k = 0; k < this.TriggeredValues.Count; k++)
		{
			blockTrigger.TriggeredValues.Add(this.TriggeredValues[k]);
		}
		blockTrigger.ExcludeIcon = this.ExcludeIcon;
		blockTrigger.UseOrForMultipleTriggers = this.UseOrForMultipleTriggers;
		blockTrigger.Unlock = this.Unlock;
		return blockTrigger;
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x0005BEF4 File Offset: 0x0005A0F4
	public void CopyFrom(BlockTrigger _other)
	{
		this.LocalChunkPos = _other.LocalChunkPos;
		this.TriggersIndices.Clear();
		this.TriggeredByIndices.Clear();
		this.TriggeredValues.Clear();
		for (int i = 0; i < _other.TriggersIndices.Count; i++)
		{
			this.TriggersIndices.Add(_other.TriggersIndices[i]);
		}
		for (int j = 0; j < _other.TriggeredByIndices.Count; j++)
		{
			this.TriggeredByIndices.Add(_other.TriggeredByIndices[j]);
		}
		for (int k = 0; k < _other.TriggeredValues.Count; k++)
		{
			this.TriggeredValues.Add(_other.TriggeredValues[k]);
		}
		_other.ExcludeIcon = this.ExcludeIcon;
		_other.UseOrForMultipleTriggers = this.UseOrForMultipleTriggers;
		_other.Unlock = this.Unlock;
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0005BFD9 File Offset: 0x0005A1D9
	public void SetTriggersFlag(byte index)
	{
		if (!this.TriggersIndices.Contains(index))
		{
			this.TriggersIndices.Add(index);
		}
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0005BFF5 File Offset: 0x0005A1F5
	public void RemoveTriggersFlag(byte index)
	{
		this.TriggersIndices.Remove(index);
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0005C004 File Offset: 0x0005A204
	public void RemoveAllTriggersFlags()
	{
		this.TriggersIndices.Clear();
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0005C011 File Offset: 0x0005A211
	public bool HasTriggers(byte index)
	{
		return this.TriggersIndices.Contains(index);
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0005C020 File Offset: 0x0005A220
	public void ToggleTriggersFlag(byte layer)
	{
		int num = this.TriggersIndices.IndexOf(layer);
		if (num >= 0)
		{
			this.TriggersIndices.RemoveAt(num);
			return;
		}
		this.TriggersIndices.Add(layer);
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0005C057 File Offset: 0x0005A257
	public bool HasAnyTriggers()
	{
		return this.TriggersIndices.Count > 0;
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0005C067 File Offset: 0x0005A267
	public void SetTriggeredByFlag(byte index)
	{
		if (!this.TriggeredByIndices.Contains(index))
		{
			this.TriggeredByIndices.Add(index);
		}
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x0005C083 File Offset: 0x0005A283
	public void RemoveTriggeredByFlag(byte index)
	{
		this.TriggeredByIndices.Remove(index);
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x0005C092 File Offset: 0x0005A292
	public bool HasTriggeredBy(byte index)
	{
		return this.TriggeredByIndices.Contains(index);
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x0005C0A0 File Offset: 0x0005A2A0
	public void ToggleTriggeredByFlag(byte layer)
	{
		int num = this.TriggeredByIndices.IndexOf(layer);
		if (num >= 0)
		{
			this.TriggeredByIndices.RemoveAt(num);
			return;
		}
		this.TriggeredByIndices.Add(layer);
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x0005C0D7 File Offset: 0x0005A2D7
	public bool HasAnyTriggeredBy()
	{
		return this.TriggeredByIndices.Count > 0;
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x0005C0E7 File Offset: 0x0005A2E7
	public void SetTriggeredValueFlag(byte index)
	{
		if (this.TriggeredValues.Contains(index))
		{
			this.TriggeredValues.Remove(index);
			return;
		}
		this.TriggeredValues.Add(index);
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x0005C114 File Offset: 0x0005A314
	public bool CheckIsTriggered()
	{
		if (this.UseOrForMultipleTriggers)
		{
			using (List<byte>.Enumerator enumerator = this.TriggeredByIndices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = (int)enumerator.Current;
					if (!this.TriggeredValues.Contains((byte)num))
					{
						return true;
					}
				}
			}
			return false;
		}
		using (List<byte>.Enumerator enumerator = this.TriggeredByIndices.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int num2 = (int)enumerator.Current;
				if (!this.TriggeredValues.Contains((byte)num2))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x0005C1D0 File Offset: 0x0005A3D0
	public string TriggerDisplay()
	{
		if (this.TriggeredByIndices.Count != 0 && this.TriggersIndices.Count == 0)
		{
			return string.Format("[0000FF]{0}[-]", string.Join<byte>(",", this.TriggeredByIndices));
		}
		if (this.TriggersIndices.Count != 0 && this.TriggeredByIndices.Count == 0)
		{
			return string.Format("[FF0000]{0}[-][0000FF]{1}[-]", string.Join<byte>(",", this.TriggersIndices), string.Join<byte>(",", this.TriggeredByIndices));
		}
		return string.Format("[FF0000]{0}[-] | [0000FF]{1}[-]", string.Join<byte>(",", this.TriggersIndices), string.Join<byte>(",", this.TriggeredByIndices));
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x0005C284 File Offset: 0x0005A484
	public Vector3i ToWorldPos()
	{
		if (this.Chunk != null)
		{
			return new Vector3i(this.Chunk.X * 16, this.Chunk.Y * 256, this.Chunk.Z * 16) + this.LocalChunkPos;
		}
		return Vector3i.zero;
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x0005C2DC File Offset: 0x0005A4DC
	public void TriggerUpdated(List<BlockChangeInfo> _blockChanges)
	{
		BlockValue block = this.Chunk.GetBlock(this.LocalChunkPos);
		if (_blockChanges != null)
		{
			block.Block.OnTriggerChanged(this, this.Chunk, this.ToWorldPos(), block, _blockChanges);
			return;
		}
		block.Block.OnTriggerChanged(this, this.Chunk, this.ToWorldPos(), block);
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x0005C334 File Offset: 0x0005A534
	public void OnTriggered(EntityPlayer _player, World _world, int index, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy = null)
	{
		this.SetTriggeredValueFlag((byte)index);
		if (this.CheckIsTriggered())
		{
			BlockValue block = this.Chunk.GetBlock(this.LocalChunkPos);
			block.Block.OnTriggered(_player, _world, this.ToWorldPos(), block, _blockChanges, _triggeredBy);
			this.TriggeredValues.Clear();
		}
	}

	// Token: 0x04000BED RID: 3053
	[PublicizedFrom(EAccessModifier.Protected)]
	public const ushort version = 5;

	// Token: 0x04000BEE RID: 3054
	[PublicizedFrom(EAccessModifier.Protected)]
	public ushort currentVersion;

	// Token: 0x04000BEF RID: 3055
	public Vector3i LocalChunkPos;

	// Token: 0x04000BF0 RID: 3056
	[PublicizedFrom(EAccessModifier.Private)]
	public long chunkKey;

	// Token: 0x04000BF1 RID: 3057
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04000BF2 RID: 3058
	public PrefabTriggerData TriggerDataOwner;

	// Token: 0x04000BF3 RID: 3059
	public List<byte> TriggersIndices = new List<byte>();

	// Token: 0x04000BF4 RID: 3060
	public List<byte> TriggeredByIndices = new List<byte>();

	// Token: 0x04000BF5 RID: 3061
	public List<byte> TriggeredValues = new List<byte>();

	// Token: 0x04000BF6 RID: 3062
	public bool ExcludeIcon;

	// Token: 0x04000BF7 RID: 3063
	public bool UseOrForMultipleTriggers;

	// Token: 0x04000BF8 RID: 3064
	public bool Unlock;

	// Token: 0x04000BF9 RID: 3065
	public BlockTrigger.TriggeredStates NeedsTriggered;

	// Token: 0x020001C6 RID: 454
	public enum TriggeredStates
	{
		// Token: 0x04000BFB RID: 3067
		NotTriggered,
		// Token: 0x04000BFC RID: 3068
		NeedsTriggered,
		// Token: 0x04000BFD RID: 3069
		HasTriggered
	}
}

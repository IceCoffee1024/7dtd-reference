using System;
using System.Collections.Generic;

// Token: 0x02000129 RID: 297
public class BlockTracker
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x00037ED3 File Offset: 0x000360D3
	public BlockTracker(int _limit)
	{
		this.limit = _limit;
		this.blockLocations = new List<Vector3i>();
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00037EED File Offset: 0x000360ED
	public bool TryAddBlock(Vector3i _position)
	{
		if (this.blockLocations.Contains(_position))
		{
			return true;
		}
		if (this.blockLocations.Count >= this.limit)
		{
			return false;
		}
		this.blockLocations.Add(_position);
		return true;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00037F21 File Offset: 0x00036121
	public bool RemoveBlock(Vector3i _position)
	{
		if (this.blockLocations.Contains(_position))
		{
			this.blockLocations.Remove(_position);
			return true;
		}
		return false;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00037F41 File Offset: 0x00036141
	public bool CanAdd(Vector3i _position)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return this.blockLocations.Count < this.limit || this.blockLocations.Contains(_position);
		}
		return this.clientAmount < this.limit;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00037F7F File Offset: 0x0003617F
	public void Clear()
	{
		this.blockLocations.Clear();
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00037F8C File Offset: 0x0003618C
	public void Read(PooledBinaryReader _reader)
	{
		this.blockLocations = new List<Vector3i>();
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.blockLocations.Add(new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32()));
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00037FDC File Offset: 0x000361DC
	public void Write(PooledBinaryWriter _writer)
	{
		_writer.Write(this.blockLocations.Count);
		foreach (Vector3i vector3i in this.blockLocations)
		{
			_writer.Write(vector3i.x);
			_writer.Write(vector3i.y);
			_writer.Write(vector3i.z);
		}
	}

	// Token: 0x0400091E RID: 2334
	public int limit;

	// Token: 0x0400091F RID: 2335
	public List<Vector3i> blockLocations;

	// Token: 0x04000920 RID: 2336
	public int clientAmount;
}

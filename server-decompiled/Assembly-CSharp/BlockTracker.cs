using System;
using System.Collections.Generic;

// Token: 0x0200012B RID: 299
public class BlockTracker
{
	// Token: 0x060007E9 RID: 2025 RVA: 0x00037F83 File Offset: 0x00036183
	public BlockTracker(int _limit)
	{
		this.limit = _limit;
		this.blockLocations = new List<Vector3i>();
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00037F9D File Offset: 0x0003619D
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

	// Token: 0x060007EB RID: 2027 RVA: 0x00037FD1 File Offset: 0x000361D1
	public bool RemoveBlock(Vector3i _position)
	{
		if (this.blockLocations.Contains(_position))
		{
			this.blockLocations.Remove(_position);
			return true;
		}
		return false;
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00037FF1 File Offset: 0x000361F1
	public bool CanAdd(Vector3i _position)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return this.blockLocations.Count < this.limit || this.blockLocations.Contains(_position);
		}
		return this.clientAmount < this.limit;
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0003802F File Offset: 0x0003622F
	public void Clear()
	{
		this.blockLocations.Clear();
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0003803C File Offset: 0x0003623C
	public void Read(PooledBinaryReader _reader)
	{
		this.blockLocations = new List<Vector3i>();
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.blockLocations.Add(new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32()));
		}
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0003808C File Offset: 0x0003628C
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

	// Token: 0x04000921 RID: 2337
	public int limit;

	// Token: 0x04000922 RID: 2338
	public List<Vector3i> blockLocations;

	// Token: 0x04000923 RID: 2339
	public int clientAmount;
}

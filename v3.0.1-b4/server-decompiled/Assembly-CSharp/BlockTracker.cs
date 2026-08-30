using System;
using System.Collections.Generic;

// Token: 0x02000128 RID: 296
public class BlockTracker
{
	// Token: 0x060007D9 RID: 2009 RVA: 0x00037C73 File Offset: 0x00035E73
	public BlockTracker(int _limit)
	{
		this.limit = _limit;
		this.blockLocations = new List<Vector3i>();
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00037C8D File Offset: 0x00035E8D
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

	// Token: 0x060007DB RID: 2011 RVA: 0x00037CC1 File Offset: 0x00035EC1
	public bool RemoveBlock(Vector3i _position)
	{
		if (this.blockLocations.Contains(_position))
		{
			this.blockLocations.Remove(_position);
			return true;
		}
		return false;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00037CE1 File Offset: 0x00035EE1
	public bool CanAdd(Vector3i _position)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return this.blockLocations.Count < this.limit || this.blockLocations.Contains(_position);
		}
		return this.clientAmount < this.limit;
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00037D1F File Offset: 0x00035F1F
	public void Clear()
	{
		this.blockLocations.Clear();
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00037D2C File Offset: 0x00035F2C
	public void Read(PooledBinaryReader _reader)
	{
		this.blockLocations = new List<Vector3i>();
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.blockLocations.Add(new Vector3i(_reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32()));
		}
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00037D7C File Offset: 0x00035F7C
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

	// Token: 0x0400091C RID: 2332
	public int limit;

	// Token: 0x0400091D RID: 2333
	public List<Vector3i> blockLocations;

	// Token: 0x0400091E RID: 2334
	public int clientAmount;
}

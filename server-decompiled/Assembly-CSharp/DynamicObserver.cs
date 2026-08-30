using System;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class DynamicObserver
{
	// Token: 0x06001AFF RID: 6911 RVA: 0x000A0720 File Offset: 0x0009E920
	public void Start(Vector3 pos)
	{
		this.Position = pos;
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (this.Observer == null)
		{
			this.Observer = GameManager.Instance.AddChunkObserver(pos, false, DynamicObserver.ViewSize, GameManager.Instance.World.GetPrimaryPlayerId());
		}
		this.Observer.SetPosition(this.Position);
		this.StopTime = float.MaxValue;
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000A078C File Offset: 0x0009E98C
	public bool ContainsPoint(Vector3i pos)
	{
		int num = DynamicObserver.ViewSize * 16;
		return (float)pos.x >= this.Position.x - (float)num && (float)pos.x <= this.Position.x + (float)num && (float)pos.z >= this.Position.z - (float)num && (float)pos.z <= this.Position.z + (float)num;
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000A0804 File Offset: 0x0009EA04
	public bool HasFallingBlocks()
	{
		foreach (long num in this.Observer.chunksLoaded)
		{
			if (GameManager.Instance == null)
			{
				return false;
			}
			if (GameManager.Instance.World == null)
			{
				return false;
			}
			Chunk chunk = (Chunk)GameManager.Instance.World.GetChunkSync(num);
			if (chunk == null)
			{
				DynamicMeshManager.LogMsg("Observer couldn't load chunk so assuming falling");
				return true;
			}
			if (chunk.HasFallingBlocks())
			{
				DynamicMeshManager.Instance.AddUpdateData(chunk.Key, false, true, true, 3);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000A08C4 File Offset: 0x0009EAC4
	public void Stop()
	{
		if (this.Observer == null)
		{
			return;
		}
		try
		{
			GameManager.Instance.RemoveChunkObserver(this.Observer);
		}
		catch (Exception ex)
		{
			if (DynamicMeshManager.DoLog)
			{
				DynamicMeshManager.LogMsg("Observer already destroyed: " + ex.Message);
			}
		}
		this.Observer = null;
	}

	// Token: 0x04001158 RID: 4440
	public static int ViewSize = 3;

	// Token: 0x04001159 RID: 4441
	public Vector3 Position;

	// Token: 0x0400115A RID: 4442
	public ChunkManager.ChunkObserver Observer;

	// Token: 0x0400115B RID: 4443
	public float StopTime = float.MaxValue;
}

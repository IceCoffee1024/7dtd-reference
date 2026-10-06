using System;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class DynamicObserver
{
	// Token: 0x06001B17 RID: 6935 RVA: 0x000A10E0 File Offset: 0x0009F2E0
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

	// Token: 0x06001B18 RID: 6936 RVA: 0x000A114C File Offset: 0x0009F34C
	public bool ContainsPoint(Vector3i pos)
	{
		int num = DynamicObserver.ViewSize * 16;
		return (float)pos.x >= this.Position.x - (float)num && (float)pos.x <= this.Position.x + (float)num && (float)pos.z >= this.Position.z - (float)num && (float)pos.z <= this.Position.z + (float)num;
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000A11C4 File Offset: 0x0009F3C4
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

	// Token: 0x06001B1A RID: 6938 RVA: 0x000A1284 File Offset: 0x0009F484
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

	// Token: 0x04001166 RID: 4454
	public static int ViewSize = 3;

	// Token: 0x04001167 RID: 4455
	public Vector3 Position;

	// Token: 0x04001168 RID: 4456
	public ChunkManager.ChunkObserver Observer;

	// Token: 0x04001169 RID: 4457
	public float StopTime = float.MaxValue;
}

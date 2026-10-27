using System;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class DynamicObserver
{
	// Token: 0x06001B1B RID: 6939 RVA: 0x000A1314 File Offset: 0x0009F514
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

	// Token: 0x06001B1C RID: 6940 RVA: 0x000A1380 File Offset: 0x0009F580
	public bool ContainsPoint(Vector3i pos)
	{
		int num = DynamicObserver.ViewSize * 16;
		return (float)pos.x >= this.Position.x - (float)num && (float)pos.x <= this.Position.x + (float)num && (float)pos.z >= this.Position.z - (float)num && (float)pos.z <= this.Position.z + (float)num;
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x000A13F8 File Offset: 0x0009F5F8
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
			if ((Chunk)GameManager.Instance.World.GetChunkSync(num) == null)
			{
				DynamicMeshManager.LogMsg("Observer couldn't load chunk so assuming falling");
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x000A1494 File Offset: 0x0009F694
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

	// Token: 0x04001176 RID: 4470
	public static int ViewSize = 3;

	// Token: 0x04001177 RID: 4471
	public Vector3 Position;

	// Token: 0x04001178 RID: 4472
	public ChunkManager.ChunkObserver Observer;

	// Token: 0x04001179 RID: 4473
	public float StopTime = float.MaxValue;
}

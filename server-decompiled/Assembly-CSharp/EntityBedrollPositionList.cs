using System;

// Token: 0x0200050D RID: 1293
public class EntityBedrollPositionList
{
	// Token: 0x06002A1C RID: 10780 RVA: 0x00108AC9 File Offset: 0x00106CC9
	public EntityBedrollPositionList(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x00108AD8 File Offset: 0x00106CD8
	public Vector3i GetPos()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			return data.BedrollPos;
		}
		return new Vector3i(0, int.MaxValue, 0);
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x00108B04 File Offset: 0x00106D04
	public void Set(Vector3i _pos)
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.BedrollPos = _pos;
			data.ShowBedrollOnMap();
		}
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x00108B28 File Offset: 0x00106D28
	public void Clear()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.ClearBedroll();
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06002A20 RID: 10784 RVA: 0x00108B45 File Offset: 0x00106D45
	public int Count
	{
		get
		{
			if (this.GetPos().y == 2147483647)
			{
				return 0;
			}
			return 1;
		}
	}

	// Token: 0x17000491 RID: 1169
	public Vector3i this[int _idx]
	{
		get
		{
			return this.GetPos();
		}
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x00108B64 File Offset: 0x00106D64
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData GetData()
	{
		return GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(this.theEntity.entityId);
	}

	// Token: 0x04002009 RID: 8201
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;
}

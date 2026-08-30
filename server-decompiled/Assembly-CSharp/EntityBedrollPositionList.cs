using System;

// Token: 0x02000505 RID: 1285
public class EntityBedrollPositionList
{
	// Token: 0x06002A20 RID: 10784 RVA: 0x00107C3D File Offset: 0x00105E3D
	public EntityBedrollPositionList(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x00107C4C File Offset: 0x00105E4C
	public Vector3i GetPos()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			return data.BedrollPos;
		}
		return new Vector3i(0, int.MaxValue, 0);
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x00107C78 File Offset: 0x00105E78
	public void Set(Vector3i _pos)
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.BedrollPos = _pos;
			data.ShowBedrollOnMap();
		}
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x00107C9C File Offset: 0x00105E9C
	public void Clear()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.ClearBedroll();
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002A24 RID: 10788 RVA: 0x00107CB9 File Offset: 0x00105EB9
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

	// Token: 0x1700049C RID: 1180
	public Vector3i this[int _idx]
	{
		get
		{
			return this.GetPos();
		}
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x00107CD8 File Offset: 0x00105ED8
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData GetData()
	{
		return GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(this.theEntity.entityId);
	}

	// Token: 0x04001FF1 RID: 8177
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;
}

using System;

// Token: 0x02000500 RID: 1280
public class EntityBedrollPositionList
{
	// Token: 0x060029F6 RID: 10742 RVA: 0x00106C01 File Offset: 0x00104E01
	public EntityBedrollPositionList(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x00106C10 File Offset: 0x00104E10
	public Vector3i GetPos()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			return data.BedrollPos;
		}
		return new Vector3i(0, int.MaxValue, 0);
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x00106C3C File Offset: 0x00104E3C
	public void Set(Vector3i _pos)
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.BedrollPos = _pos;
			data.ShowBedrollOnMap();
		}
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x00106C60 File Offset: 0x00104E60
	public void Clear()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.ClearBedroll();
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x060029FA RID: 10746 RVA: 0x00106C7D File Offset: 0x00104E7D
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

	// Token: 0x17000498 RID: 1176
	public Vector3i this[int _idx]
	{
		get
		{
			return this.GetPos();
		}
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x00106C9C File Offset: 0x00104E9C
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData GetData()
	{
		return GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(this.theEntity.entityId);
	}

	// Token: 0x04001FC2 RID: 8130
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;
}

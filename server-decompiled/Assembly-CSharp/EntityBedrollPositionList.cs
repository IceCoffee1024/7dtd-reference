using System;

// Token: 0x0200050B RID: 1291
public class EntityBedrollPositionList
{
	// Token: 0x06002A15 RID: 10773 RVA: 0x00108575 File Offset: 0x00106775
	public EntityBedrollPositionList(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x00108584 File Offset: 0x00106784
	public Vector3i GetPos()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			return data.BedrollPos;
		}
		return new Vector3i(0, int.MaxValue, 0);
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x001085B0 File Offset: 0x001067B0
	public void Set(Vector3i _pos)
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.BedrollPos = _pos;
			data.ShowBedrollOnMap();
		}
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x001085D4 File Offset: 0x001067D4
	public void Clear()
	{
		PersistentPlayerData data = this.GetData();
		if (data != null)
		{
			data.ClearBedroll();
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06002A19 RID: 10777 RVA: 0x001085F1 File Offset: 0x001067F1
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

	// Token: 0x06002A1B RID: 10779 RVA: 0x00108610 File Offset: 0x00106810
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerData GetData()
	{
		return GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(this.theEntity.entityId);
	}

	// Token: 0x04001FF9 RID: 8185
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;
}

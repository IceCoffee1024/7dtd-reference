using System;
using System.Collections.Generic;

// Token: 0x0200097D RID: 2429
public class BaseQuestData
{
	// Token: 0x06004676 RID: 18038 RVA: 0x001BF428 File Offset: 0x001BD628
	public void AddSharedQuester(int _entityID)
	{
		if (!this.entityList.Contains(_entityID))
		{
			this.entityList.Add(_entityID);
			EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(_entityID) as EntityPlayer;
			if (entityPlayer != null)
			{
				this.OnAdd(entityPlayer);
			}
		}
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x001BF470 File Offset: 0x001BD670
	public void RemoveSharedQuester(EntityPlayer _player)
	{
		if (this.entityList.Contains(_player.entityId))
		{
			this.entityList.Remove(_player.entityId);
		}
		if (this.entityList.Count == 0)
		{
			this.OnRemove(_player);
			this.RemoveFromDictionary();
		}
	}

	// Token: 0x06004678 RID: 18040 RVA: 0x001BF4BC File Offset: 0x001BD6BC
	public bool ContainsEntity(int _entityID)
	{
		return this.entityList.Contains(_entityID);
	}

	// Token: 0x06004679 RID: 18041 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetModifier(string _name)
	{
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RemoveFromDictionary()
	{
	}

	// Token: 0x0600467B RID: 18043 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCreated()
	{
	}

	// Token: 0x0600467C RID: 18044 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAdd(EntityPlayer player)
	{
	}

	// Token: 0x0600467D RID: 18045 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnRemove(EntityPlayer player)
	{
	}

	// Token: 0x0600467E RID: 18046 RVA: 0x001BF4CA File Offset: 0x001BD6CA
	public void Remove()
	{
		this.entityList.Clear();
		this.OnRemove(null);
	}

	// Token: 0x040038EB RID: 14571
	[PublicizedFrom(EAccessModifier.Protected)]
	public int questCode;

	// Token: 0x040038EC RID: 14572
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<int> entityList = new List<int>();
}

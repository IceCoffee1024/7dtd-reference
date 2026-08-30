using System;
using System.Collections.Generic;

// Token: 0x02000966 RID: 2406
public class BaseQuestData
{
	// Token: 0x0600464F RID: 17999 RVA: 0x001BC72C File Offset: 0x001BA92C
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

	// Token: 0x06004650 RID: 18000 RVA: 0x001BC774 File Offset: 0x001BA974
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

	// Token: 0x06004651 RID: 18001 RVA: 0x001BC7C0 File Offset: 0x001BA9C0
	public bool ContainsEntity(int _entityID)
	{
		return this.entityList.Contains(_entityID);
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetModifier(string _name)
	{
	}

	// Token: 0x06004653 RID: 18003 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RemoveFromDictionary()
	{
	}

	// Token: 0x06004654 RID: 18004 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCreated()
	{
	}

	// Token: 0x06004655 RID: 18005 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAdd(EntityPlayer player)
	{
	}

	// Token: 0x06004656 RID: 18006 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnRemove(EntityPlayer player)
	{
	}

	// Token: 0x06004657 RID: 18007 RVA: 0x001BC7CE File Offset: 0x001BA9CE
	public void Remove()
	{
		this.entityList.Clear();
		this.OnRemove(null);
	}

	// Token: 0x0400385E RID: 14430
	[PublicizedFrom(EAccessModifier.Protected)]
	public int questCode;

	// Token: 0x0400385F RID: 14431
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<int> entityList = new List<int>();
}

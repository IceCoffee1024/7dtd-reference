using System;
using System.Collections.Generic;

// Token: 0x02000987 RID: 2439
public class BaseQuestData
{
	// Token: 0x06004699 RID: 18073 RVA: 0x001C04AC File Offset: 0x001BE6AC
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

	// Token: 0x0600469A RID: 18074 RVA: 0x001C04F4 File Offset: 0x001BE6F4
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

	// Token: 0x0600469B RID: 18075 RVA: 0x001C0540 File Offset: 0x001BE740
	public bool ContainsEntity(int _entityID)
	{
		return this.entityList.Contains(_entityID);
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetModifier(string _name)
	{
	}

	// Token: 0x0600469D RID: 18077 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RemoveFromDictionary()
	{
	}

	// Token: 0x0600469E RID: 18078 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCreated()
	{
	}

	// Token: 0x0600469F RID: 18079 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAdd(EntityPlayer player)
	{
	}

	// Token: 0x060046A0 RID: 18080 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnRemove(EntityPlayer player)
	{
	}

	// Token: 0x060046A1 RID: 18081 RVA: 0x001C054E File Offset: 0x001BE74E
	public void Remove()
	{
		this.entityList.Clear();
		this.OnRemove(null);
	}

	// Token: 0x0400390B RID: 14603
	[PublicizedFrom(EAccessModifier.Protected)]
	public int questCode;

	// Token: 0x0400390C RID: 14604
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<int> entityList = new List<int>();
}

using System;
using System.Collections.Generic;

// Token: 0x020009EE RID: 2542
public class QuestLockInstance
{
	// Token: 0x06004B25 RID: 19237 RVA: 0x001D4FDD File Offset: 0x001D31DD
	public QuestLockInstance(int lockedByEntityID)
	{
		this.AddQuester(lockedByEntityID);
		this.LockedOutUntil = 0UL;
		this.IsLocked = true;
	}

	// Token: 0x06004B26 RID: 19238 RVA: 0x001D500D File Offset: 0x001D320D
	public void AddQuester(int entityID)
	{
		if (!this.LockedByEntities.Contains(entityID))
		{
			this.LockedByEntities.Add(entityID);
		}
	}

	// Token: 0x06004B27 RID: 19239 RVA: 0x001D502C File Offset: 0x001D322C
	public void AddQuesters(int[] entityIDs)
	{
		for (int i = 0; i < entityIDs.Length; i++)
		{
			if (!this.LockedByEntities.Contains(entityIDs[i]))
			{
				this.LockedByEntities.Add(entityIDs[i]);
			}
		}
	}

	// Token: 0x06004B28 RID: 19240 RVA: 0x001D5065 File Offset: 0x001D3265
	public void RemoveQuester(int entityID)
	{
		if (this.LockedByEntities.Contains(entityID))
		{
			this.LockedByEntities.Remove(entityID);
		}
		if (this.LockedByEntities.Count == 0)
		{
			this.SetUnlocked();
		}
	}

	// Token: 0x06004B29 RID: 19241 RVA: 0x001D5095 File Offset: 0x001D3295
	public void SetUnlocked()
	{
		if (this.IsLocked)
		{
			this.IsLocked = false;
			if (!GameUtils.IsPlaytesting())
			{
				this.LockedOutUntil = GameManager.Instance.World.GetWorldTime() + 2000UL;
			}
		}
	}

	// Token: 0x06004B2A RID: 19242 RVA: 0x001D50C9 File Offset: 0x001D32C9
	public bool CheckQuestLock()
	{
		return !this.IsLocked && GameManager.Instance.World.GetWorldTime() > this.LockedOutUntil;
	}

	// Token: 0x04003B83 RID: 15235
	public bool IsLocked = true;

	// Token: 0x04003B84 RID: 15236
	public List<int> LockedByEntities = new List<int>();

	// Token: 0x04003B85 RID: 15237
	public ulong LockedOutUntil;
}

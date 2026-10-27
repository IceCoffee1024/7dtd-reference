using System;
using System.Collections.Generic;

// Token: 0x020009F8 RID: 2552
public class QuestLockInstance
{
	// Token: 0x06004B49 RID: 19273 RVA: 0x001D60BD File Offset: 0x001D42BD
	public QuestLockInstance(int lockedByEntityID)
	{
		this.AddQuester(lockedByEntityID);
		this.LockedOutUntil = 0UL;
		this.IsLocked = true;
	}

	// Token: 0x06004B4A RID: 19274 RVA: 0x001D60ED File Offset: 0x001D42ED
	public void AddQuester(int entityID)
	{
		if (!this.LockedByEntities.Contains(entityID))
		{
			this.LockedByEntities.Add(entityID);
		}
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x001D610C File Offset: 0x001D430C
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

	// Token: 0x06004B4C RID: 19276 RVA: 0x001D6145 File Offset: 0x001D4345
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

	// Token: 0x06004B4D RID: 19277 RVA: 0x001D6175 File Offset: 0x001D4375
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

	// Token: 0x06004B4E RID: 19278 RVA: 0x001D61A9 File Offset: 0x001D43A9
	public bool CheckQuestLock()
	{
		return !this.IsLocked && GameManager.Instance.World.GetWorldTime() > this.LockedOutUntil;
	}

	// Token: 0x04003BA4 RID: 15268
	public bool IsLocked = true;

	// Token: 0x04003BA5 RID: 15269
	public List<int> LockedByEntities = new List<int>();

	// Token: 0x04003BA6 RID: 15270
	public ulong LockedOutUntil;
}

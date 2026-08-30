using System;
using System.Collections.Generic;

// Token: 0x020009D3 RID: 2515
public class QuestLockInstance
{
	// Token: 0x06004AD0 RID: 19152 RVA: 0x001D15B1 File Offset: 0x001CF7B1
	public QuestLockInstance(int lockedByEntityID)
	{
		this.AddQuester(lockedByEntityID);
		this.LockedOutUntil = 0UL;
		this.IsLocked = true;
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x001D15E1 File Offset: 0x001CF7E1
	public void AddQuester(int entityID)
	{
		if (!this.LockedByEntities.Contains(entityID))
		{
			this.LockedByEntities.Add(entityID);
		}
	}

	// Token: 0x06004AD2 RID: 19154 RVA: 0x001D1600 File Offset: 0x001CF800
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

	// Token: 0x06004AD3 RID: 19155 RVA: 0x001D1639 File Offset: 0x001CF839
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

	// Token: 0x06004AD4 RID: 19156 RVA: 0x001D1669 File Offset: 0x001CF869
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

	// Token: 0x06004AD5 RID: 19157 RVA: 0x001D169D File Offset: 0x001CF89D
	public bool CheckQuestLock()
	{
		return !this.IsLocked && GameManager.Instance.World.GetWorldTime() > this.LockedOutUntil;
	}

	// Token: 0x04003AE6 RID: 15078
	public bool IsLocked = true;

	// Token: 0x04003AE7 RID: 15079
	public List<int> LockedByEntities = new List<int>();

	// Token: 0x04003AE8 RID: 15080
	public ulong LockedOutUntil;
}

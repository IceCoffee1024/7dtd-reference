using System;
using System.Collections.Generic;

// Token: 0x020009E4 RID: 2532
public class QuestLockInstance
{
	// Token: 0x06004B56 RID: 19286 RVA: 0x001D3D99 File Offset: 0x001D1F99
	public QuestLockInstance(int lockedByEntityID)
	{
		this.AddQuester(lockedByEntityID);
		this.LockedOutUntil = 0UL;
		this.IsLocked = true;
	}

	// Token: 0x06004B57 RID: 19287 RVA: 0x001D3DC9 File Offset: 0x001D1FC9
	public void AddQuester(int entityID)
	{
		if (!this.LockedByEntities.Contains(entityID))
		{
			this.LockedByEntities.Add(entityID);
		}
	}

	// Token: 0x06004B58 RID: 19288 RVA: 0x001D3DE8 File Offset: 0x001D1FE8
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

	// Token: 0x06004B59 RID: 19289 RVA: 0x001D3E21 File Offset: 0x001D2021
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

	// Token: 0x06004B5A RID: 19290 RVA: 0x001D3E51 File Offset: 0x001D2051
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

	// Token: 0x06004B5B RID: 19291 RVA: 0x001D3E85 File Offset: 0x001D2085
	public bool CheckQuestLock()
	{
		return !this.IsLocked && GameManager.Instance.World.GetWorldTime() > this.LockedOutUntil;
	}

	// Token: 0x04003B77 RID: 15223
	public bool IsLocked = true;

	// Token: 0x04003B78 RID: 15224
	public List<int> LockedByEntities = new List<int>();

	// Token: 0x04003B79 RID: 15225
	public ulong LockedOutUntil;
}

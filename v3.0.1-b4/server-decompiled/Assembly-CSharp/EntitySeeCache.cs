using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class EntitySeeCache
{
	// Token: 0x06002A2B RID: 10795 RVA: 0x00109C0C File Offset: 0x00107E0C
	public EntitySeeCache(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x00109C34 File Offset: 0x00107E34
	public bool CanSee(Entity _e)
	{
		if (_e == null)
		{
			return false;
		}
		if (this.positiveCache.Contains(_e.entityId))
		{
			return true;
		}
		if (this.negativeCache.Contains(_e.entityId))
		{
			return false;
		}
		bool flag = this.theEntity.CanEntityBeSeen(_e, true);
		if (flag)
		{
			this.positiveCache.Add(_e.entityId);
			if (_e.IsClientControlled())
			{
				this.lastTimeSeenAPlayer = Time.time;
				return flag;
			}
		}
		else
		{
			this.negativeCache.Add(_e.entityId);
		}
		return flag;
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x00109CBE File Offset: 0x00107EBE
	public float GetLastTimePlayerSeen()
	{
		return this.lastTimeSeenAPlayer;
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x00109CC6 File Offset: 0x00107EC6
	public void SetLastTimePlayerSeen()
	{
		this.lastTimeSeenAPlayer = Time.time;
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x00109CD3 File Offset: 0x00107ED3
	public void SetCanSee(Entity _e)
	{
		this.positiveCache.Add(_e.entityId);
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x00109CE7 File Offset: 0x00107EE7
	public void Clear()
	{
		this.positiveCache.Clear();
		this.negativeCache.Clear();
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x00109D00 File Offset: 0x00107F00
	public void ClearIfExpired()
	{
		int num = this.ticksSinceLastClear + 1;
		this.ticksSinceLastClear = num;
		if (num >= 30)
		{
			this.ticksSinceLastClear = 0;
			this.Clear();
		}
	}

	// Token: 0x04002017 RID: 8215
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;

	// Token: 0x04002018 RID: 8216
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> positiveCache = new HashSet<int>();

	// Token: 0x04002019 RID: 8217
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> negativeCache = new HashSet<int>();

	// Token: 0x0400201A RID: 8218
	[PublicizedFrom(EAccessModifier.Private)]
	public int ticksSinceLastClear;

	// Token: 0x0400201B RID: 8219
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeSeenAPlayer;
}

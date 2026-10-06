using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class EntitySeeCache
{
	// Token: 0x06002A4A RID: 10826 RVA: 0x0010B580 File Offset: 0x00109780
	public EntitySeeCache(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x0010B5A8 File Offset: 0x001097A8
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

	// Token: 0x06002A4C RID: 10828 RVA: 0x0010B632 File Offset: 0x00109832
	public float GetLastTimePlayerSeen()
	{
		return this.lastTimeSeenAPlayer;
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x0010B63A File Offset: 0x0010983A
	public void SetLastTimePlayerSeen()
	{
		this.lastTimeSeenAPlayer = Time.time;
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x0010B647 File Offset: 0x00109847
	public void SetCanSee(Entity _e)
	{
		this.positiveCache.Add(_e.entityId);
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x0010B65B File Offset: 0x0010985B
	public void Clear()
	{
		this.positiveCache.Clear();
		this.negativeCache.Clear();
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x0010B674 File Offset: 0x00109874
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

	// Token: 0x0400204E RID: 8270
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;

	// Token: 0x0400204F RID: 8271
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> positiveCache = new HashSet<int>();

	// Token: 0x04002050 RID: 8272
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> negativeCache = new HashSet<int>();

	// Token: 0x04002051 RID: 8273
	[PublicizedFrom(EAccessModifier.Private)]
	public int ticksSinceLastClear;

	// Token: 0x04002052 RID: 8274
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeSeenAPlayer;
}

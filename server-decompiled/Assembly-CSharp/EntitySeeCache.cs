using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public class EntitySeeCache
{
	// Token: 0x06002A55 RID: 10837 RVA: 0x0010AC48 File Offset: 0x00108E48
	public EntitySeeCache(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0010AC70 File Offset: 0x00108E70
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

	// Token: 0x06002A57 RID: 10839 RVA: 0x0010ACFA File Offset: 0x00108EFA
	public float GetLastTimePlayerSeen()
	{
		return this.lastTimeSeenAPlayer;
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x0010AD02 File Offset: 0x00108F02
	public void SetLastTimePlayerSeen()
	{
		this.lastTimeSeenAPlayer = Time.time;
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x0010AD0F File Offset: 0x00108F0F
	public void SetCanSee(Entity _e)
	{
		this.positiveCache.Add(_e.entityId);
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x0010AD23 File Offset: 0x00108F23
	public void Clear()
	{
		this.positiveCache.Clear();
		this.negativeCache.Clear();
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x0010AD3C File Offset: 0x00108F3C
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

	// Token: 0x04002046 RID: 8262
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;

	// Token: 0x04002047 RID: 8263
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> positiveCache = new HashSet<int>();

	// Token: 0x04002048 RID: 8264
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> negativeCache = new HashSet<int>();

	// Token: 0x04002049 RID: 8265
	[PublicizedFrom(EAccessModifier.Private)]
	public int ticksSinceLastClear;

	// Token: 0x0400204A RID: 8266
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeSeenAPlayer;
}

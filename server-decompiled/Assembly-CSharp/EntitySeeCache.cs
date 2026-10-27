using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class EntitySeeCache
{
	// Token: 0x06002A51 RID: 10833 RVA: 0x0010BAD4 File Offset: 0x00109CD4
	public EntitySeeCache(EntityAlive _e)
	{
		this.theEntity = _e;
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x0010BAFC File Offset: 0x00109CFC
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

	// Token: 0x06002A53 RID: 10835 RVA: 0x0010BB86 File Offset: 0x00109D86
	public float GetLastTimePlayerSeen()
	{
		return this.lastTimeSeenAPlayer;
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x0010BB8E File Offset: 0x00109D8E
	public void SetLastTimePlayerSeen()
	{
		this.lastTimeSeenAPlayer = Time.time;
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x0010BB9B File Offset: 0x00109D9B
	public void SetCanSee(Entity _e)
	{
		this.positiveCache.Add(_e.entityId);
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0010BBAF File Offset: 0x00109DAF
	public void Clear()
	{
		this.positiveCache.Clear();
		this.negativeCache.Clear();
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x0010BBC8 File Offset: 0x00109DC8
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

	// Token: 0x0400205E RID: 8286
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive theEntity;

	// Token: 0x0400205F RID: 8287
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> positiveCache = new HashSet<int>();

	// Token: 0x04002060 RID: 8288
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> negativeCache = new HashSet<int>();

	// Token: 0x04002061 RID: 8289
	[PublicizedFrom(EAccessModifier.Private)]
	public int ticksSinceLastClear;

	// Token: 0x04002062 RID: 8290
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeSeenAPlayer;
}

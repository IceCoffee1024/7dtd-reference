using System;
using UnityEngine.Scripting;

// Token: 0x02000423 RID: 1059
[Preserve]
public class AIDirectorZombieState : IMemoryPoolableObject
{
	// Token: 0x06002093 RID: 8339 RVA: 0x000C4CB3 File Offset: 0x000C2EB3
	public AIDirectorZombieState Construct(EntityEnemy zombie)
	{
		this.m_zombie = zombie;
		return this;
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x000C4CBD File Offset: 0x000C2EBD
	public void Reset()
	{
		this.m_zombie = null;
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06002096 RID: 8342 RVA: 0x000C4CC6 File Offset: 0x000C2EC6
	public EntityEnemy Zombie
	{
		get
		{
			return this.m_zombie;
		}
	}

	// Token: 0x0400164E RID: 5710
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityEnemy m_zombie;
}

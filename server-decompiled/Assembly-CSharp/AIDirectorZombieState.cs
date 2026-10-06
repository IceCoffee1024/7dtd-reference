using System;
using UnityEngine.Scripting;

// Token: 0x02000421 RID: 1057
[Preserve]
public class AIDirectorZombieState : IMemoryPoolableObject
{
	// Token: 0x0600208E RID: 8334 RVA: 0x000C4907 File Offset: 0x000C2B07
	public AIDirectorZombieState Construct(EntityEnemy zombie)
	{
		this.m_zombie = zombie;
		return this;
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000C4911 File Offset: 0x000C2B11
	public void Reset()
	{
		this.m_zombie = null;
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06002091 RID: 8337 RVA: 0x000C491A File Offset: 0x000C2B1A
	public EntityEnemy Zombie
	{
		get
		{
			return this.m_zombie;
		}
	}

	// Token: 0x0400163E RID: 5694
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityEnemy m_zombie;
}

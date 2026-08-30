using System;
using UnityEngine.Scripting;

// Token: 0x0200041B RID: 1051
[Preserve]
public class AIDirectorZombieState : IMemoryPoolableObject
{
	// Token: 0x06002082 RID: 8322 RVA: 0x000C40B7 File Offset: 0x000C22B7
	public AIDirectorZombieState Construct(EntityEnemy zombie)
	{
		this.m_zombie = zombie;
		return this;
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x000C40C1 File Offset: 0x000C22C1
	public void Reset()
	{
		this.m_zombie = null;
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06002085 RID: 8325 RVA: 0x000C40CA File Offset: 0x000C22CA
	public EntityEnemy Zombie
	{
		get
		{
			return this.m_zombie;
		}
	}

	// Token: 0x04001632 RID: 5682
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityEnemy m_zombie;
}

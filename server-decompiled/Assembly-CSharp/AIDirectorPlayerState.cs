using System;
using UnityEngine.Scripting;

// Token: 0x0200041C RID: 1052
[Preserve]
public class AIDirectorPlayerState : IMemoryPoolableObject
{
	// Token: 0x0600205E RID: 8286 RVA: 0x000C417C File Offset: 0x000C237C
	public AIDirectorPlayerState Construct(EntityPlayer _player)
	{
		this.Player = _player;
		this.m_dead = false;
		return this;
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000C418D File Offset: 0x000C238D
	public void Reset()
	{
		this.Player = null;
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06002061 RID: 8289 RVA: 0x000C4196 File Offset: 0x000C2396
	// (set) Token: 0x06002062 RID: 8290 RVA: 0x000C419E File Offset: 0x000C239E
	public AIDirectorPlayerInventory Inventory
	{
		get
		{
			return this.m_inventory;
		}
		set
		{
			this.m_inventory = value;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06002063 RID: 8291 RVA: 0x000C41A7 File Offset: 0x000C23A7
	// (set) Token: 0x06002064 RID: 8292 RVA: 0x000C41AF File Offset: 0x000C23AF
	public bool Dead
	{
		get
		{
			return this.m_dead;
		}
		set
		{
			this.m_dead = value;
		}
	}

	// Token: 0x04001622 RID: 5666
	public const float kCheckUndergroundTime = 5f;

	// Token: 0x04001623 RID: 5667
	public const int kNumBlocksUnderground = 10;

	// Token: 0x04001624 RID: 5668
	public EntityPlayer Player;

	// Token: 0x04001625 RID: 5669
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;

	// Token: 0x04001626 RID: 5670
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_dead;
}

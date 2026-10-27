using System;
using UnityEngine.Scripting;

// Token: 0x0200041E RID: 1054
[Preserve]
public class AIDirectorPlayerState : IMemoryPoolableObject
{
	// Token: 0x06002063 RID: 8291 RVA: 0x000C4528 File Offset: 0x000C2728
	public AIDirectorPlayerState Construct(EntityPlayer _player)
	{
		this.Player = _player;
		this.m_dead = false;
		return this;
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000C4539 File Offset: 0x000C2739
	public void Reset()
	{
		this.Player = null;
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x000C4542 File Offset: 0x000C2742
	// (set) Token: 0x06002067 RID: 8295 RVA: 0x000C454A File Offset: 0x000C274A
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
	// (get) Token: 0x06002068 RID: 8296 RVA: 0x000C4553 File Offset: 0x000C2753
	// (set) Token: 0x06002069 RID: 8297 RVA: 0x000C455B File Offset: 0x000C275B
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

	// Token: 0x04001632 RID: 5682
	public const float kCheckUndergroundTime = 5f;

	// Token: 0x04001633 RID: 5683
	public const int kNumBlocksUnderground = 10;

	// Token: 0x04001634 RID: 5684
	public EntityPlayer Player;

	// Token: 0x04001635 RID: 5685
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;

	// Token: 0x04001636 RID: 5686
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_dead;
}

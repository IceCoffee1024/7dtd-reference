using System;
using UnityEngine.Scripting;

// Token: 0x02000416 RID: 1046
[Preserve]
public class AIDirectorPlayerState : IMemoryPoolableObject
{
	// Token: 0x06002052 RID: 8274 RVA: 0x000C392C File Offset: 0x000C1B2C
	public AIDirectorPlayerState Construct(EntityPlayer _player)
	{
		this.Player = _player;
		this.m_dead = false;
		return this;
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000C393D File Offset: 0x000C1B3D
	public void Reset()
	{
		this.Player = null;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06002055 RID: 8277 RVA: 0x000C3946 File Offset: 0x000C1B46
	// (set) Token: 0x06002056 RID: 8278 RVA: 0x000C394E File Offset: 0x000C1B4E
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

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06002057 RID: 8279 RVA: 0x000C3957 File Offset: 0x000C1B57
	// (set) Token: 0x06002058 RID: 8280 RVA: 0x000C395F File Offset: 0x000C1B5F
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

	// Token: 0x04001616 RID: 5654
	public const float kCheckUndergroundTime = 5f;

	// Token: 0x04001617 RID: 5655
	public const int kNumBlocksUnderground = 10;

	// Token: 0x04001618 RID: 5656
	public EntityPlayer Player;

	// Token: 0x04001619 RID: 5657
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;

	// Token: 0x0400161A RID: 5658
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_dead;
}

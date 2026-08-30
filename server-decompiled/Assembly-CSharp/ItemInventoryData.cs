using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class ItemInventoryData
{
	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06002DBC RID: 11708 RVA: 0x00122FC0 File Offset: 0x001211C0
	public Transform model
	{
		get
		{
			return this.holdingEntity.inventory.models[this.slotIdx];
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06002DBD RID: 11709 RVA: 0x00122FD9 File Offset: 0x001211D9
	// (set) Token: 0x06002DBE RID: 11710 RVA: 0x00122FF1 File Offset: 0x001211F1
	public ItemValue itemValue
	{
		get
		{
			return this.holdingEntity.inventory[this.slotIdx];
		}
		set
		{
			this.holdingEntity.inventory[this.slotIdx] = value;
		}
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x0012300A File Offset: 0x0012120A
	public void Changed()
	{
		this.holdingEntity.inventory.Changed();
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x0012301C File Offset: 0x0012121C
	public ItemInventoryData(ItemClass _item, ItemStack _itemStack, IGameManager _gameManager, EntityAlive _holdingEntity, int _slotIdx)
	{
		this.item = _item;
		this.itemStack = _itemStack;
		this.world = _holdingEntity.world;
		this.holdingEntity = _holdingEntity;
		this.gameManager = _gameManager;
		this.slotIdx = _slotIdx;
		this.actionData = new List<ItemActionData>();
		this.actionData.Add(null);
		this.actionData.Add(null);
	}

	// Token: 0x040023B1 RID: 9137
	public ItemClass item;

	// Token: 0x040023B2 RID: 9138
	public ItemStack itemStack;

	// Token: 0x040023B3 RID: 9139
	public readonly EntityAlive holdingEntity;

	// Token: 0x040023B4 RID: 9140
	public int holdingEntitySoundID = -2;

	// Token: 0x040023B5 RID: 9141
	public World world;

	// Token: 0x040023B6 RID: 9142
	public IGameManager gameManager;

	// Token: 0x040023B7 RID: 9143
	public List<ItemActionData> actionData;

	// Token: 0x040023B8 RID: 9144
	public int slotIdx;

	// Token: 0x0200057A RID: 1402
	public enum SoundPlayType
	{
		// Token: 0x040023BA RID: 9146
		None = -2,
		// Token: 0x040023BB RID: 9147
		IdleReady,
		// Token: 0x040023BC RID: 9148
		Idle
	}
}

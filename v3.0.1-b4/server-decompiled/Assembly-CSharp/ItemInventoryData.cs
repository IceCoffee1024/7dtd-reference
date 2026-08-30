using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000574 RID: 1396
public class ItemInventoryData
{
	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06002D91 RID: 11665 RVA: 0x00121F04 File Offset: 0x00120104
	public Transform model
	{
		get
		{
			return this.holdingEntity.inventory.models[this.slotIdx];
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06002D92 RID: 11666 RVA: 0x00121F1D File Offset: 0x0012011D
	// (set) Token: 0x06002D93 RID: 11667 RVA: 0x00121F35 File Offset: 0x00120135
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

	// Token: 0x06002D94 RID: 11668 RVA: 0x00121F4E File Offset: 0x0012014E
	public void Changed()
	{
		this.holdingEntity.inventory.Changed();
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x00121F60 File Offset: 0x00120160
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

	// Token: 0x0400237F RID: 9087
	public ItemClass item;

	// Token: 0x04002380 RID: 9088
	public ItemStack itemStack;

	// Token: 0x04002381 RID: 9089
	public readonly EntityAlive holdingEntity;

	// Token: 0x04002382 RID: 9090
	public int holdingEntitySoundID = -2;

	// Token: 0x04002383 RID: 9091
	public World world;

	// Token: 0x04002384 RID: 9092
	public IGameManager gameManager;

	// Token: 0x04002385 RID: 9093
	public List<ItemActionData> actionData;

	// Token: 0x04002386 RID: 9094
	public int slotIdx;

	// Token: 0x02000575 RID: 1397
	public enum SoundPlayType
	{
		// Token: 0x04002388 RID: 9096
		None = -2,
		// Token: 0x04002389 RID: 9097
		IdleReady,
		// Token: 0x0400238A RID: 9098
		Idle
	}
}

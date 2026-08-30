using System;
using System.IO;

// Token: 0x020002D1 RID: 721
public class CraftingData
{
	// Token: 0x060014DB RID: 5339 RVA: 0x0007E0F6 File Offset: 0x0007C2F6
	public CraftingData()
	{
		this.items = new ItemStack[0];
		this.outputItems = new ItemStack[0];
		this.breakDownType = CraftingData.BreakdownType.None;
		this.RecipeQueueItems = new RecipeQueueItem[0];
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x0007E12C File Offset: 0x0007C32C
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(2);
		int num = this.RecipeQueueItems.Length;
		_bw.Write((byte)num);
		for (int i = 0; i < num; i++)
		{
			if (this.RecipeQueueItems[i] == null)
			{
				this.RecipeQueueItems[i] = new RecipeQueueItem();
			}
			this.RecipeQueueItems[i].Write(_bw);
		}
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x0007E184 File Offset: 0x0007C384
	public void Read(BinaryReader _br)
	{
		_br.ReadUInt16();
		int num = (int)_br.ReadByte();
		this.RecipeQueueItems = new RecipeQueueItem[num];
		for (int i = 0; i < num; i++)
		{
			this.RecipeQueueItems[i] = new RecipeQueueItem();
			this.RecipeQueueItems[i].Read(_br);
		}
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x0007E1D4 File Offset: 0x0007C3D4
	public void ReadLegacy(BinaryReader _br)
	{
		int num = (int)_br.ReadByte();
		this.RecipeQueueItems = new RecipeQueueItem[num];
		for (int i = 0; i < num; i++)
		{
			this.RecipeQueueItems[i] = new RecipeQueueItem();
			this.RecipeQueueItems[i].ReadLegacy(_br);
		}
	}

	// Token: 0x04000DD5 RID: 3541
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04000DD6 RID: 3542
	public ItemStack[] items;

	// Token: 0x04000DD7 RID: 3543
	public ItemStack[] outputItems;

	// Token: 0x04000DD8 RID: 3544
	public bool isCrafting;

	// Token: 0x04000DD9 RID: 3545
	public ulong lastWorldTick;

	// Token: 0x04000DDA RID: 3546
	public int totalLeftToCraft;

	// Token: 0x04000DDB RID: 3547
	public float currentRecipeTimer;

	// Token: 0x04000DDC RID: 3548
	public Recipe currentRecipeToCraft;

	// Token: 0x04000DDD RID: 3549
	public Recipe lastRecipeToCraft;

	// Token: 0x04000DDE RID: 3550
	public ItemValue repairedItem;

	// Token: 0x04000DDF RID: 3551
	public CraftingData.BreakdownType breakDownType;

	// Token: 0x04000DE0 RID: 3552
	public bool isItemPlacedByUser;

	// Token: 0x04000DE1 RID: 3553
	public ulong savedWorldTick;

	// Token: 0x04000DE2 RID: 3554
	public RecipeQueueItem[] RecipeQueueItems;

	// Token: 0x020002D2 RID: 722
	public enum BreakdownType
	{
		// Token: 0x04000DE4 RID: 3556
		None,
		// Token: 0x04000DE5 RID: 3557
		Part,
		// Token: 0x04000DE6 RID: 3558
		Recipe
	}
}

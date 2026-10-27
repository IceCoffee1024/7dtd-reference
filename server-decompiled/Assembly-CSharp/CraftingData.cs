using System;

// Token: 0x020002DA RID: 730
public class CraftingData
{
	// Token: 0x060014EE RID: 5358 RVA: 0x0007EBD2 File Offset: 0x0007CDD2
	public CraftingData()
	{
		this.items = new ItemStack[0];
		this.outputItems = new ItemStack[0];
		this.breakDownType = CraftingData.BreakdownType.None;
		this.RecipeQueueItems = new RecipeQueueItem[0];
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x0007EC08 File Offset: 0x0007CE08
	public void Write(PooledBinaryWriter _bw)
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

	// Token: 0x060014F0 RID: 5360 RVA: 0x0007EC60 File Offset: 0x0007CE60
	public void Read(PooledBinaryReader _br)
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

	// Token: 0x060014F1 RID: 5361 RVA: 0x0007ECB0 File Offset: 0x0007CEB0
	public void ReadLegacy(PooledBinaryReader _br)
	{
		int num = (int)_br.ReadByte();
		this.RecipeQueueItems = new RecipeQueueItem[num];
		for (int i = 0; i < num; i++)
		{
			this.RecipeQueueItems[i] = new RecipeQueueItem();
			this.RecipeQueueItems[i].ReadLegacy(_br);
		}
	}

	// Token: 0x04000DE0 RID: 3552
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04000DE1 RID: 3553
	public ItemStack[] items;

	// Token: 0x04000DE2 RID: 3554
	public ItemStack[] outputItems;

	// Token: 0x04000DE3 RID: 3555
	public bool isCrafting;

	// Token: 0x04000DE4 RID: 3556
	public ulong lastWorldTick;

	// Token: 0x04000DE5 RID: 3557
	public int totalLeftToCraft;

	// Token: 0x04000DE6 RID: 3558
	public float currentRecipeTimer;

	// Token: 0x04000DE7 RID: 3559
	public Recipe currentRecipeToCraft;

	// Token: 0x04000DE8 RID: 3560
	public Recipe lastRecipeToCraft;

	// Token: 0x04000DE9 RID: 3561
	public ItemValue repairedItem;

	// Token: 0x04000DEA RID: 3562
	public CraftingData.BreakdownType breakDownType;

	// Token: 0x04000DEB RID: 3563
	public bool isItemPlacedByUser;

	// Token: 0x04000DEC RID: 3564
	public ulong savedWorldTick;

	// Token: 0x04000DED RID: 3565
	public RecipeQueueItem[] RecipeQueueItems;

	// Token: 0x020002DB RID: 731
	public enum BreakdownType
	{
		// Token: 0x04000DEF RID: 3567
		None,
		// Token: 0x04000DF0 RID: 3568
		Part,
		// Token: 0x04000DF1 RID: 3569
		Recipe
	}
}

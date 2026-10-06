using System;

// Token: 0x020002D8 RID: 728
public class CraftingData
{
	// Token: 0x060014EA RID: 5354 RVA: 0x0007EA02 File Offset: 0x0007CC02
	public CraftingData()
	{
		this.items = new ItemStack[0];
		this.outputItems = new ItemStack[0];
		this.breakDownType = CraftingData.BreakdownType.None;
		this.RecipeQueueItems = new RecipeQueueItem[0];
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x0007EA38 File Offset: 0x0007CC38
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

	// Token: 0x060014EC RID: 5356 RVA: 0x0007EA90 File Offset: 0x0007CC90
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

	// Token: 0x060014ED RID: 5357 RVA: 0x0007EAE0 File Offset: 0x0007CCE0
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

	// Token: 0x04000DDF RID: 3551
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04000DE0 RID: 3552
	public ItemStack[] items;

	// Token: 0x04000DE1 RID: 3553
	public ItemStack[] outputItems;

	// Token: 0x04000DE2 RID: 3554
	public bool isCrafting;

	// Token: 0x04000DE3 RID: 3555
	public ulong lastWorldTick;

	// Token: 0x04000DE4 RID: 3556
	public int totalLeftToCraft;

	// Token: 0x04000DE5 RID: 3557
	public float currentRecipeTimer;

	// Token: 0x04000DE6 RID: 3558
	public Recipe currentRecipeToCraft;

	// Token: 0x04000DE7 RID: 3559
	public Recipe lastRecipeToCraft;

	// Token: 0x04000DE8 RID: 3560
	public ItemValue repairedItem;

	// Token: 0x04000DE9 RID: 3561
	public CraftingData.BreakdownType breakDownType;

	// Token: 0x04000DEA RID: 3562
	public bool isItemPlacedByUser;

	// Token: 0x04000DEB RID: 3563
	public ulong savedWorldTick;

	// Token: 0x04000DEC RID: 3564
	public RecipeQueueItem[] RecipeQueueItems;

	// Token: 0x020002D9 RID: 729
	public enum BreakdownType
	{
		// Token: 0x04000DEE RID: 3566
		None,
		// Token: 0x04000DEF RID: 3567
		Part,
		// Token: 0x04000DF0 RID: 3568
		Recipe
	}
}

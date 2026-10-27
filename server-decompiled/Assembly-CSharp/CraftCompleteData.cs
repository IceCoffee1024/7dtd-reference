using System;

// Token: 0x020002DC RID: 732
public class CraftCompleteData
{
	// Token: 0x060014F2 RID: 5362 RVA: 0x0007ECF7 File Offset: 0x0007CEF7
	public CraftCompleteData()
	{
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x0007ED18 File Offset: 0x0007CF18
	public CraftCompleteData(int crafterEntityID, ItemStack craftedItemStack, string recipeName, string itemScrapped, int craftExpGain, ushort recipeUsedCount)
	{
		this.CrafterEntityID = crafterEntityID;
		this.CraftedItemStack = craftedItemStack;
		this.RecipeName = recipeName;
		this.ItemScrapped = itemScrapped;
		this.RecipeUsedCount = recipeUsedCount;
		this.CraftExpGain = craftExpGain;
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x0007ED70 File Offset: 0x0007CF70
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(1);
		_bw.Write(this.CrafterEntityID);
		this.CraftedItemStack.Write(_bw);
		_bw.Write(this.RecipeName);
		_bw.Write(this.CraftExpGain);
		_bw.Write(this.RecipeUsedCount);
		_bw.Write(this.ItemScrapped);
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x0007EDCC File Offset: 0x0007CFCC
	public void Read(PooledBinaryReader _br)
	{
		_br.ReadUInt16();
		this.CrafterEntityID = _br.ReadInt32();
		this.CraftedItemStack = new ItemStack().Read(_br);
		this.RecipeName = _br.ReadString();
		this.CraftExpGain = _br.ReadInt32();
		this.RecipeUsedCount = _br.ReadUInt16();
		this.ItemScrapped = _br.ReadString();
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x0007EE30 File Offset: 0x0007D030
	public void ReadLegacy(PooledBinaryReader _br)
	{
		this.CrafterEntityID = _br.ReadInt32();
		this.CraftedItemStack = new ItemStack().Read(_br);
		this.RecipeName = _br.ReadString();
		this.CraftExpGain = _br.ReadInt32();
		this.RecipeUsedCount = _br.ReadUInt16();
		this.ItemScrapped = _br.ReadString();
	}

	// Token: 0x04000DF2 RID: 3570
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 1;

	// Token: 0x04000DF3 RID: 3571
	public int CrafterEntityID;

	// Token: 0x04000DF4 RID: 3572
	public ItemStack CraftedItemStack;

	// Token: 0x04000DF5 RID: 3573
	public string RecipeName = "";

	// Token: 0x04000DF6 RID: 3574
	public string ItemScrapped = "";

	// Token: 0x04000DF7 RID: 3575
	public ushort RecipeUsedCount;

	// Token: 0x04000DF8 RID: 3576
	public int CraftExpGain;
}

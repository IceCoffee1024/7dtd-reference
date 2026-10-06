using System;

// Token: 0x020002DA RID: 730
public class CraftCompleteData
{
	// Token: 0x060014EE RID: 5358 RVA: 0x0007EB27 File Offset: 0x0007CD27
	public CraftCompleteData()
	{
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x0007EB48 File Offset: 0x0007CD48
	public CraftCompleteData(int crafterEntityID, ItemStack craftedItemStack, string recipeName, string itemScrapped, int craftExpGain, ushort recipeUsedCount)
	{
		this.CrafterEntityID = crafterEntityID;
		this.CraftedItemStack = craftedItemStack;
		this.RecipeName = recipeName;
		this.ItemScrapped = itemScrapped;
		this.RecipeUsedCount = recipeUsedCount;
		this.CraftExpGain = craftExpGain;
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x0007EBA0 File Offset: 0x0007CDA0
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

	// Token: 0x060014F1 RID: 5361 RVA: 0x0007EBFC File Offset: 0x0007CDFC
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

	// Token: 0x060014F2 RID: 5362 RVA: 0x0007EC60 File Offset: 0x0007CE60
	public void ReadLegacy(PooledBinaryReader _br)
	{
		this.CrafterEntityID = _br.ReadInt32();
		this.CraftedItemStack = new ItemStack().Read(_br);
		this.RecipeName = _br.ReadString();
		this.CraftExpGain = _br.ReadInt32();
		this.RecipeUsedCount = _br.ReadUInt16();
		this.ItemScrapped = _br.ReadString();
	}

	// Token: 0x04000DF1 RID: 3569
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 1;

	// Token: 0x04000DF2 RID: 3570
	public int CrafterEntityID;

	// Token: 0x04000DF3 RID: 3571
	public ItemStack CraftedItemStack;

	// Token: 0x04000DF4 RID: 3572
	public string RecipeName = "";

	// Token: 0x04000DF5 RID: 3573
	public string ItemScrapped = "";

	// Token: 0x04000DF6 RID: 3574
	public ushort RecipeUsedCount;

	// Token: 0x04000DF7 RID: 3575
	public int CraftExpGain;
}

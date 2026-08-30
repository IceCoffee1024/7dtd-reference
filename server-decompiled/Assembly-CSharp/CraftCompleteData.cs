using System;
using System.IO;

// Token: 0x020002D3 RID: 723
public class CraftCompleteData
{
	// Token: 0x060014DF RID: 5343 RVA: 0x0007E21B File Offset: 0x0007C41B
	public CraftCompleteData()
	{
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x0007E23C File Offset: 0x0007C43C
	public CraftCompleteData(int crafterEntityID, ItemStack craftedItemStack, string recipeName, string itemScrapped, int craftExpGain, ushort recipeUsedCount)
	{
		this.CrafterEntityID = crafterEntityID;
		this.CraftedItemStack = craftedItemStack;
		this.RecipeName = recipeName;
		this.ItemScrapped = itemScrapped;
		this.RecipeUsedCount = recipeUsedCount;
		this.CraftExpGain = craftExpGain;
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x0007E294 File Offset: 0x0007C494
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(1);
		_bw.Write(this.CrafterEntityID);
		this.CraftedItemStack.Write(_bw);
		_bw.Write(this.RecipeName);
		_bw.Write(this.CraftExpGain);
		_bw.Write(this.RecipeUsedCount);
		_bw.Write(this.ItemScrapped);
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x0007E2F0 File Offset: 0x0007C4F0
	public void Read(BinaryReader _br)
	{
		_br.ReadUInt16();
		this.CrafterEntityID = _br.ReadInt32();
		this.CraftedItemStack = new ItemStack().Read(_br);
		this.RecipeName = _br.ReadString();
		this.CraftExpGain = _br.ReadInt32();
		this.RecipeUsedCount = _br.ReadUInt16();
		this.ItemScrapped = _br.ReadString();
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x0007E354 File Offset: 0x0007C554
	public void ReadLegacy(BinaryReader _br)
	{
		this.CrafterEntityID = _br.ReadInt32();
		this.CraftedItemStack = new ItemStack().Read(_br);
		this.RecipeName = _br.ReadString();
		this.CraftExpGain = _br.ReadInt32();
		this.RecipeUsedCount = _br.ReadUInt16();
		this.ItemScrapped = _br.ReadString();
	}

	// Token: 0x04000DE7 RID: 3559
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 1;

	// Token: 0x04000DE8 RID: 3560
	public int CrafterEntityID;

	// Token: 0x04000DE9 RID: 3561
	public ItemStack CraftedItemStack;

	// Token: 0x04000DEA RID: 3562
	public string RecipeName = "";

	// Token: 0x04000DEB RID: 3563
	public string ItemScrapped = "";

	// Token: 0x04000DEC RID: 3564
	public ushort RecipeUsedCount;

	// Token: 0x04000DED RID: 3565
	public int CraftExpGain;
}

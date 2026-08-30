using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020002D8 RID: 728
public class RecipeQueueItem
{
	// Token: 0x060014F6 RID: 5366 RVA: 0x0007EA08 File Offset: 0x0007CC08
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(2);
		_bw.Write(this.Multiplier);
		_bw.Write(this.IsCrafting);
		_bw.Write(this.CraftingTimeLeft);
		bool flag = this.RepairItem != null;
		_bw.Write(flag);
		if (flag)
		{
			this.RepairItem.Write(_bw);
			_bw.Write(this.AmountToRepair);
		}
		_bw.Write(this.Quality);
		_bw.Write(this.StartingEntityId);
		_bw.Write(this.OneItemCraftTime);
		bool flag2 = this.Recipe != null;
		_bw.Write(flag2);
		if (flag2)
		{
			this.Recipe.Write(_bw);
		}
		if (flag2)
		{
			ItemClass outputItemClass = this.Recipe.GetOutputItemClass();
			NameIdMapping nameIdMapping = (outputItemClass.IsBlock() ? Block.nameIdMapping : ItemClass.nameIdMapping);
			if (nameIdMapping == null)
			{
				return;
			}
			nameIdMapping.AddMapping(outputItemClass.Id, outputItemClass.Name, false);
		}
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x0007EAEC File Offset: 0x0007CCEC
	public void Read(BinaryReader _br)
	{
		ushort num = _br.ReadUInt16();
		int num2 = 0;
		if (num < 2)
		{
			num2 = _br.ReadInt32();
		}
		this.Multiplier = _br.ReadInt16();
		this.IsCrafting = _br.ReadBoolean();
		this.CraftingTimeLeft = _br.ReadSingle();
		if (_br.ReadBoolean())
		{
			this.RepairItem = ItemValue.ReadOrNull(_br);
			this.AmountToRepair = _br.ReadUInt16();
		}
		this.Quality = _br.ReadByte();
		this.StartingEntityId = _br.ReadInt32();
		this.OneItemCraftTime = _br.ReadSingle();
		if (num >= 2)
		{
			if (_br.ReadBoolean())
			{
				this.Recipe = Recipe.Read(_br);
				return;
			}
		}
		else
		{
			if (_br.ReadBoolean())
			{
				this.Recipe = new Recipe();
				this.Recipe.itemValueType = _br.ReadInt32();
				this.Recipe.count = _br.ReadInt32();
				int num3 = _br.ReadInt32();
				this.Recipe.ingredients = new List<ItemStack>();
				for (int i = 0; i < num3; i++)
				{
					this.Recipe.ingredients.Add(new ItemStack().Read(_br));
				}
				this.Recipe.craftingTime = _br.ReadSingle();
				this.Recipe.craftExpGain = _br.ReadInt32();
				this.Recipe.IsScrap = _br.ReadBoolean();
				return;
			}
			if (num2 != 0)
			{
				Log.Warning("[RecipeQueueItem] In-progress recipe has outdata data and has been discarded.");
			}
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x0007EC48 File Offset: 0x0007CE48
	public void ReadLegacy(BinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.Multiplier = _br.ReadInt16();
		this.IsCrafting = _br.ReadBoolean();
		this.CraftingTimeLeft = _br.ReadSingle();
		if (_br.ReadBoolean())
		{
			this.RepairItem = ItemValue.ReadOrNull(_br);
			this.AmountToRepair = _br.ReadUInt16();
		}
		this.Quality = _br.ReadByte();
		this.StartingEntityId = _br.ReadInt32();
		this.OneItemCraftTime = _br.ReadSingle();
		if (_br.ReadBoolean())
		{
			this.Recipe = new Recipe();
			this.Recipe.itemValueType = _br.ReadInt32();
			this.Recipe.count = _br.ReadInt32();
			int num2 = _br.ReadInt32();
			this.Recipe.ingredients = new List<ItemStack>();
			for (int i = 0; i < num2; i++)
			{
				this.Recipe.ingredients.Add(new ItemStack().Read(_br));
			}
			this.Recipe.craftingTime = _br.ReadSingle();
			this.Recipe.craftExpGain = _br.ReadInt32();
			this.Recipe.IsScrap = _br.ReadBoolean();
			return;
		}
		if (num != 0)
		{
			Log.Warning("[RecipeQueueItem] In-progress recipe has outdata data and has been discarded.");
			this.Clear();
		}
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0007ED84 File Offset: 0x0007CF84
	public void Clear()
	{
		this.Recipe = null;
		this.Multiplier = 0;
		this.CraftingTimeLeft = 0f;
		this.OneItemCraftTime = -1f;
		this.IsCrafting = false;
		ItemValue repairItem = this.RepairItem;
		if (repairItem != null)
		{
			repairItem.Clear();
		}
		this.AmountToRepair = 0;
		this.Quality = 0;
		this.StartingEntityId = -1;
	}

	// Token: 0x04000DFF RID: 3583
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04000E00 RID: 3584
	public Recipe Recipe;

	// Token: 0x04000E01 RID: 3585
	public short Multiplier;

	// Token: 0x04000E02 RID: 3586
	public float CraftingTimeLeft;

	// Token: 0x04000E03 RID: 3587
	public float OneItemCraftTime = -1f;

	// Token: 0x04000E04 RID: 3588
	public bool IsCrafting;

	// Token: 0x04000E05 RID: 3589
	public ItemValue RepairItem;

	// Token: 0x04000E06 RID: 3590
	public ushort AmountToRepair;

	// Token: 0x04000E07 RID: 3591
	public byte Quality;

	// Token: 0x04000E08 RID: 3592
	public int StartingEntityId = -1;
}

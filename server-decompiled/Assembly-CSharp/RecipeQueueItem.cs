using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020002D4 RID: 724
public class RecipeQueueItem
{
	// Token: 0x060014E4 RID: 5348 RVA: 0x0007E3B0 File Offset: 0x0007C5B0
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

	// Token: 0x060014E5 RID: 5349 RVA: 0x0007E494 File Offset: 0x0007C694
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

	// Token: 0x060014E6 RID: 5350 RVA: 0x0007E5F0 File Offset: 0x0007C7F0
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
		}
	}

	// Token: 0x04000DEE RID: 3566
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort Version = 2;

	// Token: 0x04000DEF RID: 3567
	public Recipe Recipe;

	// Token: 0x04000DF0 RID: 3568
	public short Multiplier;

	// Token: 0x04000DF1 RID: 3569
	public float CraftingTimeLeft;

	// Token: 0x04000DF2 RID: 3570
	public float OneItemCraftTime = -1f;

	// Token: 0x04000DF3 RID: 3571
	public bool IsCrafting;

	// Token: 0x04000DF4 RID: 3572
	public ItemValue RepairItem;

	// Token: 0x04000DF5 RID: 3573
	public ushort AmountToRepair;

	// Token: 0x04000DF6 RID: 3574
	public byte Quality;

	// Token: 0x04000DF7 RID: 3575
	public int StartingEntityId = -1;
}

using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010E2 RID: 4322
[Preserve]
public class XUiC_WorkstationMaterialInputGrid : XUiC_WorkstationInputGrid
{
	// Token: 0x0600896B RID: 35179 RVA: 0x00348839 File Offset: 0x00346A39
	public override void Init()
	{
		base.Init();
		this.materialWindow = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputWindow>();
	}

	// Token: 0x0600896C RID: 35180 RVA: 0x00348858 File Offset: 0x00346A58
	public override void Update(float _dt)
	{
		base.Update(_dt);
		int num = 0;
		while (num < this.workstationData.TileEntity.Input.Length && num < this.itemControllers.Length)
		{
			float timerForSlot = this.workstationData.TileEntity.GetTimerForSlot(num);
			if (timerForSlot > 0f)
			{
				this.itemControllers[num].timer.IsVisible = true;
				this.itemControllers[num].timer.Text = string.Format("{0}:{1}", Mathf.Floor((timerForSlot + 0.95f) / 60f).ToCultureInvariantString("00"), Mathf.Floor((timerForSlot + 0.95f) % 60f).ToCultureInvariantString("00"));
			}
			else
			{
				this.itemControllers[num].timer.IsVisible = false;
			}
			num++;
		}
		this.workstationData.GetIsBurning();
	}

	// Token: 0x0600896D RID: 35181 RVA: 0x002F0892 File Offset: 0x002EEA92
	public override ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x0600896E RID: 35182 RVA: 0x002DDF23 File Offset: 0x002DC123
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemStack[] getUISlots()
	{
		return this.items;
	}

	// Token: 0x0600896F RID: 35183 RVA: 0x0034893C File Offset: 0x00346B3C
	public override bool HasRequirement(Recipe recipe)
	{
		return this.materialWindow != null && this.materialWindow.HasRequirement(recipe);
	}

	// Token: 0x06008970 RID: 35184 RVA: 0x00348954 File Offset: 0x00346B54
	public override void SetSlots(ItemStack[] stackList)
	{
		this.items = stackList;
		base.SetSlots(this.items);
		this.materialWindow.SetMaterialWeights(this.items);
	}

	// Token: 0x06008971 RID: 35185 RVA: 0x0034897C File Offset: 0x00346B7C
	public void SetWeight(ItemValue iv, int count)
	{
		ItemClass itemClass = iv.ItemClass;
		if (itemClass == null)
		{
			return;
		}
		string forgeCategory = itemClass.MadeOfMaterial.ForgeCategory;
		if (forgeCategory == null)
		{
			return;
		}
		for (int i = 3; i < this.items.Length; i++)
		{
			ItemClass itemClass2 = this.items[i].itemValue.ItemClass;
			if (itemClass2 == null)
			{
				if (this.materialWindow.MaterialNames[i - 3].EqualsCaseInsensitive(forgeCategory))
				{
					ItemStack itemStack = new ItemStack(iv, count);
					this.items[i] = itemStack;
					break;
				}
			}
			else if (itemClass2.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(forgeCategory))
			{
				ItemStack itemStack2 = this.items[i].Clone();
				itemStack2.count += count;
				if (iv.ItemClass.MaxCount < itemStack2.count)
				{
					itemStack2.count = iv.ItemClass.MaxCount;
				}
				this.items[i] = itemStack2;
				break;
			}
		}
		this.materialWindow.SetMaterialWeights(this.items);
		this.UpdateBackend(this.items);
	}

	// Token: 0x06008972 RID: 35186 RVA: 0x00348A80 File Offset: 0x00346C80
	public int GetWeight(string materialName)
	{
		int num = 0;
		if (materialName == null)
		{
			return num;
		}
		for (int i = 3; i < this.items.Length; i++)
		{
			ItemClass itemClass = this.items[i].itemValue.ItemClass;
			if (itemClass != null && itemClass.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(materialName))
			{
				num = this.items[i].count;
				break;
			}
		}
		return num;
	}

	// Token: 0x04006643 RID: 26179
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputWindow materialWindow;
}

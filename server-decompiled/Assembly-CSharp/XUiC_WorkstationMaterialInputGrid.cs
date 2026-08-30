using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010D1 RID: 4305
[Preserve]
public class XUiC_WorkstationMaterialInputGrid : XUiC_WorkstationInputGrid
{
	// Token: 0x0600891D RID: 35101 RVA: 0x00344A89 File Offset: 0x00342C89
	public override void Init()
	{
		base.Init();
		this.materialWindow = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputWindow>();
	}

	// Token: 0x0600891E RID: 35102 RVA: 0x00344AA8 File Offset: 0x00342CA8
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

	// Token: 0x0600891F RID: 35103 RVA: 0x002ED702 File Offset: 0x002EB902
	public override ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x06008920 RID: 35104 RVA: 0x002C5CCA File Offset: 0x002C3ECA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemStack[] getUISlots()
	{
		return this.items;
	}

	// Token: 0x06008921 RID: 35105 RVA: 0x00344B8C File Offset: 0x00342D8C
	public override bool HasRequirement(Recipe recipe)
	{
		return this.materialWindow != null && this.materialWindow.HasRequirement(recipe);
	}

	// Token: 0x06008922 RID: 35106 RVA: 0x00344BA4 File Offset: 0x00342DA4
	public override void SetSlots(ItemStack[] stackList)
	{
		this.items = stackList;
		base.SetSlots(this.items);
		this.materialWindow.SetMaterialWeights(this.items);
	}

	// Token: 0x06008923 RID: 35107 RVA: 0x00344BCC File Offset: 0x00342DCC
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

	// Token: 0x06008924 RID: 35108 RVA: 0x00344CD0 File Offset: 0x00342ED0
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

	// Token: 0x04006601 RID: 26113
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputWindow materialWindow;
}

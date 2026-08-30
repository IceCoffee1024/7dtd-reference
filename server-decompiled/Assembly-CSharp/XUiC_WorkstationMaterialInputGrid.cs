using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010B8 RID: 4280
[Preserve]
public class XUiC_WorkstationMaterialInputGrid : XUiC_WorkstationInputGrid
{
	// Token: 0x0600882B RID: 34859 RVA: 0x00340259 File Offset: 0x0033E459
	public override void Init()
	{
		base.Init();
		this.materialWindow = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputWindow>();
	}

	// Token: 0x0600882C RID: 34860 RVA: 0x00340278 File Offset: 0x0033E478
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

	// Token: 0x0600882D RID: 34861 RVA: 0x002E9A1E File Offset: 0x002E7C1E
	public override ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x0600882E RID: 34862 RVA: 0x002C27A6 File Offset: 0x002C09A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemStack[] getUISlots()
	{
		return this.items;
	}

	// Token: 0x0600882F RID: 34863 RVA: 0x0034035C File Offset: 0x0033E55C
	public override bool HasRequirement(Recipe recipe)
	{
		return this.materialWindow != null && this.materialWindow.HasRequirement(recipe);
	}

	// Token: 0x06008830 RID: 34864 RVA: 0x00340374 File Offset: 0x0033E574
	public override void SetSlots(ItemStack[] stackList)
	{
		this.items = stackList;
		base.SetSlots(this.items);
		this.materialWindow.SetMaterialWeights(this.items);
	}

	// Token: 0x06008831 RID: 34865 RVA: 0x0034039C File Offset: 0x0033E59C
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
				if (iv.ItemClass.Stacknumber.Value < itemStack2.count)
				{
					itemStack2.count = iv.ItemClass.Stacknumber.Value;
				}
				this.items[i] = itemStack2;
				break;
			}
		}
		this.materialWindow.SetMaterialWeights(this.items);
		this.UpdateBackend(this.items);
	}

	// Token: 0x06008832 RID: 34866 RVA: 0x003404AC File Offset: 0x0033E6AC
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

	// Token: 0x0400653A RID: 25914
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputWindow materialWindow;
}

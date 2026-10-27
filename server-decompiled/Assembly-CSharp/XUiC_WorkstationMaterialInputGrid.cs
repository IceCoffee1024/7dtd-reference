using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010F2 RID: 4338
[Preserve]
public class XUiC_WorkstationMaterialInputGrid : XUiC_WorkstationInputGrid
{
	// Token: 0x060089BE RID: 35262 RVA: 0x0034A8BD File Offset: 0x00348ABD
	public override void Init()
	{
		base.Init();
		this.materialWindow = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputWindow>();
	}

	// Token: 0x060089BF RID: 35263 RVA: 0x0034A8DC File Offset: 0x00348ADC
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

	// Token: 0x060089C0 RID: 35264 RVA: 0x002F2239 File Offset: 0x002F0439
	public override ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x060089C1 RID: 35265 RVA: 0x002DF813 File Offset: 0x002DDA13
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemStack[] getUISlots()
	{
		return this.items;
	}

	// Token: 0x060089C2 RID: 35266 RVA: 0x0034A9C0 File Offset: 0x00348BC0
	public override bool HasRequirement(Recipe recipe)
	{
		return this.materialWindow != null && this.materialWindow.HasRequirement(recipe);
	}

	// Token: 0x060089C3 RID: 35267 RVA: 0x0034A9D8 File Offset: 0x00348BD8
	public override void SetSlots(ItemStack[] stackList)
	{
		this.items = stackList;
		base.SetSlots(this.items);
		this.materialWindow.SetMaterialWeights(this.items);
	}

	// Token: 0x060089C4 RID: 35268 RVA: 0x0034AA00 File Offset: 0x00348C00
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

	// Token: 0x060089C5 RID: 35269 RVA: 0x0034AB04 File Offset: 0x00348D04
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

	// Token: 0x04006682 RID: 26242
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationMaterialInputWindow materialWindow;
}

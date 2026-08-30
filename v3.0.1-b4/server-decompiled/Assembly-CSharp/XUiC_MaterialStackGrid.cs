using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000F7F RID: 3967
[Preserve]
public class XUiC_MaterialStackGrid : XUiController
{
	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x06007D1E RID: 32030 RVA: 0x002F7D12 File Offset: 0x002F5F12
	// (set) Token: 0x06007D1F RID: 32031 RVA: 0x002F7D1A File Offset: 0x002F5F1A
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x06007D20 RID: 32032 RVA: 0x002F7D23 File Offset: 0x002F5F23
	// (set) Token: 0x06007D21 RID: 32033 RVA: 0x002F7D2B File Offset: 0x002F5F2B
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			this.page = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007D22 RID: 32034 RVA: 0x002F7D3C File Offset: 0x002F5F3C
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_MaterialStack>(null);
		this.materialControllers = childrenByType;
		this.Length = this.materialControllers.Length;
		this.bAwakeCalled = true;
	}

	// Token: 0x06007D23 RID: 32035 RVA: 0x002F7D74 File Offset: 0x002F5F74
	public void SetMaterials(List<BlockTextureData> materialIndexList, int newSelectedMaterial = -1)
	{
		bool isCreative = GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) && GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled);
		materialIndexList = (from m in materialIndexList
			orderby m.GetLocked(this.xui.playerUI.entityPlayer), (m.ID != 0) & isCreative, m.Group, m.SortIndex, m.LocalizedName
			select m).ToList<BlockTextureData>();
		XUiC_MaterialInfoWindow childByType = this.xui.GetChildByType<XUiC_MaterialInfoWindow>();
		int count = materialIndexList.Count;
		this.currentList = materialIndexList;
		if (newSelectedMaterial != -1)
		{
			this.selectedMaterial = BlockTextureData.list[newSelectedMaterial];
			if (this.selectedMaterial.Hidden && !isCreative)
			{
				this.selectedMaterial = null;
			}
		}
		for (int i = 0; i < this.Length; i++)
		{
			int num = i + this.Length * this.page;
			XUiC_MaterialStack xuiC_MaterialStack = (XUiC_MaterialStack)this.materialControllers[i];
			xuiC_MaterialStack.InfoWindow = childByType;
			if (num < count)
			{
				xuiC_MaterialStack.TextureData = materialIndexList[num];
				if (xuiC_MaterialStack.TextureData == this.selectedMaterial)
				{
					xuiC_MaterialStack.IsSelected = true;
				}
				if (xuiC_MaterialStack.IsSelected && xuiC_MaterialStack.TextureData != this.selectedMaterial)
				{
					xuiC_MaterialStack.IsSelected = false;
				}
			}
			else
			{
				xuiC_MaterialStack.TextureData = null;
				if (xuiC_MaterialStack.IsSelected)
				{
					xuiC_MaterialStack.IsSelected = false;
				}
			}
		}
		if (this.selectedMaterial == null && newSelectedMaterial != -1)
		{
			for (int j = 0; j < this.materialControllers.Length; j++)
			{
				XUiC_MaterialStack xuiC_MaterialStack2 = this.materialControllers[j] as XUiC_MaterialStack;
				if (xuiC_MaterialStack2.TextureData != null && !xuiC_MaterialStack2.IsLocked)
				{
					xuiC_MaterialStack2.SetSelectedTextureForItem();
					xuiC_MaterialStack2.IsSelected = true;
					return;
				}
			}
		}
	}

	// Token: 0x06007D24 RID: 32036 RVA: 0x002F7F7E File Offset: 0x002F617E
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06007D25 RID: 32037 RVA: 0x002E5E28 File Offset: 0x002E4028
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x06007D26 RID: 32038 RVA: 0x002F7FA4 File Offset: 0x002F61A4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			int num;
			if (this.xui.playerUI.entityPlayer.inventory.holdingItem is ItemClassBlock)
			{
				num = (int)(this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue.TextureFullArray[0] & 255L);
			}
			else
			{
				num = ((ItemActionTextureBlock.ItemActionTextureBlockData)this.xui.playerUI.entityPlayer.inventory.holdingItemData.actionData[1]).idx;
			}
			this.SetMaterials(this.currentList, num);
			this.IsDirty = false;
		}
	}

	// Token: 0x04005CA4 RID: 23716
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005CA5 RID: 23717
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005CA6 RID: 23718
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005CA8 RID: 23720
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] materialControllers;

	// Token: 0x04005CA9 RID: 23721
	[PublicizedFrom(EAccessModifier.Protected)]
	public int[] materialIndices;

	// Token: 0x04005CAA RID: 23722
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04005CAB RID: 23723
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData selectedMaterial;

	// Token: 0x04005CAC RID: 23724
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockTextureData> currentList;
}

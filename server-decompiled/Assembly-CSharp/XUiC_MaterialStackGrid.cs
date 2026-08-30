using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000F98 RID: 3992
[Preserve]
public class XUiC_MaterialStackGrid : XUiController
{
	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x06007E10 RID: 32272 RVA: 0x002FC4E6 File Offset: 0x002FA6E6
	// (set) Token: 0x06007E11 RID: 32273 RVA: 0x002FC4EE File Offset: 0x002FA6EE
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x06007E12 RID: 32274 RVA: 0x002FC4F7 File Offset: 0x002FA6F7
	// (set) Token: 0x06007E13 RID: 32275 RVA: 0x002FC4FF File Offset: 0x002FA6FF
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

	// Token: 0x06007E14 RID: 32276 RVA: 0x002FC510 File Offset: 0x002FA710
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_MaterialStack>(null);
		this.materialControllers = childrenByType;
		this.Length = this.materialControllers.Length;
		this.bAwakeCalled = true;
	}

	// Token: 0x06007E15 RID: 32277 RVA: 0x002FC548 File Offset: 0x002FA748
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

	// Token: 0x06007E16 RID: 32278 RVA: 0x002FC752 File Offset: 0x002FA952
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06007E17 RID: 32279 RVA: 0x002E9CB4 File Offset: 0x002E7EB4
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x06007E18 RID: 32280 RVA: 0x002FC778 File Offset: 0x002FA978
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

	// Token: 0x04005D6B RID: 23915
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005D6C RID: 23916
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005D6D RID: 23917
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005D6F RID: 23919
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] materialControllers;

	// Token: 0x04005D70 RID: 23920
	[PublicizedFrom(EAccessModifier.Protected)]
	public int[] materialIndices;

	// Token: 0x04005D71 RID: 23921
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04005D72 RID: 23922
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData selectedMaterial;

	// Token: 0x04005D73 RID: 23923
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockTextureData> currentList;
}

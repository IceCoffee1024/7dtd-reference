using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000FB7 RID: 4023
[Preserve]
public class XUiC_MaterialStackGrid : XUiController
{
	// Token: 0x17000EEC RID: 3820
	// (get) Token: 0x06007E6F RID: 32367 RVA: 0x00301056 File Offset: 0x002FF256
	// (set) Token: 0x06007E70 RID: 32368 RVA: 0x0030105E File Offset: 0x002FF25E
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EED RID: 3821
	// (get) Token: 0x06007E71 RID: 32369 RVA: 0x00301067 File Offset: 0x002FF267
	// (set) Token: 0x06007E72 RID: 32370 RVA: 0x0030106F File Offset: 0x002FF26F
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

	// Token: 0x06007E73 RID: 32371 RVA: 0x00301080 File Offset: 0x002FF280
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_MaterialStack>(null);
		this.materialControllers = childrenByType;
		this.Length = this.materialControllers.Length;
		this.bAwakeCalled = true;
	}

	// Token: 0x06007E74 RID: 32372 RVA: 0x003010B8 File Offset: 0x002FF2B8
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

	// Token: 0x06007E75 RID: 32373 RVA: 0x003012C2 File Offset: 0x002FF4C2
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06007E76 RID: 32374 RVA: 0x002EE8C1 File Offset: 0x002ECAC1
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x06007E77 RID: 32375 RVA: 0x003012E8 File Offset: 0x002FF4E8
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

	// Token: 0x04005DCF RID: 24015
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005DD0 RID: 24016
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005DD1 RID: 24017
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005DD3 RID: 24019
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] materialControllers;

	// Token: 0x04005DD4 RID: 24020
	[PublicizedFrom(EAccessModifier.Protected)]
	public int[] materialIndices;

	// Token: 0x04005DD5 RID: 24021
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04005DD6 RID: 24022
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData selectedMaterial;

	// Token: 0x04005DD7 RID: 24023
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockTextureData> currentList;
}

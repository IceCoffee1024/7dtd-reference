using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000FA7 RID: 4007
[Preserve]
public class XUiC_MaterialStackGrid : XUiController
{
	// Token: 0x17000EE1 RID: 3809
	// (get) Token: 0x06007E20 RID: 32288 RVA: 0x002FF53A File Offset: 0x002FD73A
	// (set) Token: 0x06007E21 RID: 32289 RVA: 0x002FF542 File Offset: 0x002FD742
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EE2 RID: 3810
	// (get) Token: 0x06007E22 RID: 32290 RVA: 0x002FF54B File Offset: 0x002FD74B
	// (set) Token: 0x06007E23 RID: 32291 RVA: 0x002FF553 File Offset: 0x002FD753
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

	// Token: 0x06007E24 RID: 32292 RVA: 0x002FF564 File Offset: 0x002FD764
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_MaterialStack>(null);
		this.materialControllers = childrenByType;
		this.Length = this.materialControllers.Length;
		this.bAwakeCalled = true;
	}

	// Token: 0x06007E25 RID: 32293 RVA: 0x002FF59C File Offset: 0x002FD79C
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

	// Token: 0x06007E26 RID: 32294 RVA: 0x002FF7A6 File Offset: 0x002FD9A6
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06007E27 RID: 32295 RVA: 0x002ECFC5 File Offset: 0x002EB1C5
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x06007E28 RID: 32296 RVA: 0x002FF7CC File Offset: 0x002FD9CC
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

	// Token: 0x04005D94 RID: 23956
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005D95 RID: 23957
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005D96 RID: 23958
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005D98 RID: 23960
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] materialControllers;

	// Token: 0x04005D99 RID: 23961
	[PublicizedFrom(EAccessModifier.Protected)]
	public int[] materialIndices;

	// Token: 0x04005D9A RID: 23962
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04005D9B RID: 23963
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData selectedMaterial;

	// Token: 0x04005D9C RID: 23964
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockTextureData> currentList;
}

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001026 RID: 4134
[Preserve]
public class XUiC_ShapeStackGrid : XUiController
{
	// Token: 0x17000F3F RID: 3903
	// (get) Token: 0x060082B6 RID: 33462 RVA: 0x00319786 File Offset: 0x00317986
	// (set) Token: 0x060082B7 RID: 33463 RVA: 0x0031978E File Offset: 0x0031798E
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F40 RID: 3904
	// (get) Token: 0x060082B8 RID: 33464 RVA: 0x00319797 File Offset: 0x00317997
	// (set) Token: 0x060082B9 RID: 33465 RVA: 0x0031979F File Offset: 0x0031799F
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

	// Token: 0x060082BA RID: 33466 RVA: 0x003197B0 File Offset: 0x003179B0
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ShapeStack>(null);
		this.shapeControllers = childrenByType;
		this.Length = this.shapeControllers.Length;
		Log.Out("ShapeControllers: " + this.shapeControllers.Length.ToString());
		this.bAwakeCalled = true;
		this.IsDirty = false;
	}

	// Token: 0x060082BB RID: 33467 RVA: 0x00319810 File Offset: 0x00317A10
	public void SetShapes(List<XUiC_ShapeStackGrid.ShapeData> shapeIndexList, int newSelectedBlock = -1)
	{
		if (GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled))
		{
			GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled);
		}
		XUiC_ShapeInfoWindow childByType = this.xui.GetChildByType<XUiC_ShapeInfoWindow>();
		XUiC_ShapeMaterialInfoWindow childByType2 = this.xui.GetChildByType<XUiC_ShapeMaterialInfoWindow>();
		int count = shapeIndexList.Count;
		this.currentList = shapeIndexList;
		for (int i = 0; i < this.Length; i++)
		{
			int num = i + this.Length * this.page;
			XUiC_ShapeStack xuiC_ShapeStack = (XUiC_ShapeStack)this.shapeControllers[i];
			xuiC_ShapeStack.Owner = this;
			xuiC_ShapeStack.InfoWindow = childByType;
			xuiC_ShapeStack.MaterialInfoWindow = childByType2;
			if (num < count)
			{
				xuiC_ShapeStack.BlockData = shapeIndexList[num].Block;
				xuiC_ShapeStack.ShapeIndex = shapeIndexList[num].Index;
				if (xuiC_ShapeStack.BlockData == this.selectedBlock)
				{
					xuiC_ShapeStack.IsSelected = true;
				}
				if (xuiC_ShapeStack.IsSelected && xuiC_ShapeStack.BlockData != this.selectedBlock)
				{
					xuiC_ShapeStack.IsSelected = false;
				}
			}
			else
			{
				xuiC_ShapeStack.BlockData = null;
				xuiC_ShapeStack.ShapeIndex = -1;
				if (xuiC_ShapeStack.IsSelected)
				{
					xuiC_ShapeStack.IsSelected = false;
				}
			}
		}
		if (this.selectedBlock == null && newSelectedBlock != -1)
		{
			for (int j = 0; j < this.shapeControllers.Length; j++)
			{
				XUiC_ShapeStack xuiC_ShapeStack2 = this.shapeControllers[j] as XUiC_ShapeStack;
				if (xuiC_ShapeStack2.BlockData != null && xuiC_ShapeStack2.ShapeIndex == newSelectedBlock)
				{
					xuiC_ShapeStack2.SetSelectedShapeForItem();
					xuiC_ShapeStack2.IsSelected = true;
					return;
				}
			}
		}
		this.IsDirty = false;
	}

	// Token: 0x060082BC RID: 33468 RVA: 0x002FC752 File Offset: 0x002FA952
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x060082BD RID: 33469 RVA: 0x002E9CB4 File Offset: 0x002E7EB4
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x060082BE RID: 33470 RVA: 0x0031998A File Offset: 0x00317B8A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetShapes(this.currentList, this.Owner.ItemValue.Meta);
			this.IsDirty = false;
		}
	}

	// Token: 0x040060D4 RID: 24788
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x040060D5 RID: 24789
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x040060D6 RID: 24790
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040060D8 RID: 24792
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] shapeControllers;

	// Token: 0x040060D9 RID: 24793
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x040060DA RID: 24794
	public XUiC_ShapesWindow Owner;

	// Token: 0x040060DB RID: 24795
	[PublicizedFrom(EAccessModifier.Private)]
	public Block selectedBlock;

	// Token: 0x040060DC RID: 24796
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ShapeStackGrid.ShapeData> currentList;

	// Token: 0x02001027 RID: 4135
	public class ShapeData
	{
		// Token: 0x040060DD RID: 24797
		public Block Block;

		// Token: 0x040060DE RID: 24798
		public int Index;
	}
}

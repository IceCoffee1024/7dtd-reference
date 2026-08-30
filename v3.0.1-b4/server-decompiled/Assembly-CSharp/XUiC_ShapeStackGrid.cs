using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200100D RID: 4109
[Preserve]
public class XUiC_ShapeStackGrid : XUiController
{
	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x060081C4 RID: 33220 RVA: 0x00314F5E File Offset: 0x0031315E
	// (set) Token: 0x060081C5 RID: 33221 RVA: 0x00314F66 File Offset: 0x00313166
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x060081C6 RID: 33222 RVA: 0x00314F6F File Offset: 0x0031316F
	// (set) Token: 0x060081C7 RID: 33223 RVA: 0x00314F77 File Offset: 0x00313177
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

	// Token: 0x060081C8 RID: 33224 RVA: 0x00314F88 File Offset: 0x00313188
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

	// Token: 0x060081C9 RID: 33225 RVA: 0x00314FE8 File Offset: 0x003131E8
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

	// Token: 0x060081CA RID: 33226 RVA: 0x002F7F7E File Offset: 0x002F617E
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x060081CB RID: 33227 RVA: 0x002E5E28 File Offset: 0x002E4028
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x060081CC RID: 33228 RVA: 0x00315162 File Offset: 0x00313362
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetShapes(this.currentList, this.Owner.ItemValue.Meta);
			this.IsDirty = false;
		}
	}

	// Token: 0x0400600D RID: 24589
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x0400600E RID: 24590
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x0400600F RID: 24591
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006011 RID: 24593
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] shapeControllers;

	// Token: 0x04006012 RID: 24594
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04006013 RID: 24595
	public XUiC_ShapesWindow Owner;

	// Token: 0x04006014 RID: 24596
	[PublicizedFrom(EAccessModifier.Private)]
	public Block selectedBlock;

	// Token: 0x04006015 RID: 24597
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ShapeStackGrid.ShapeData> currentList;

	// Token: 0x0200100E RID: 4110
	public class ShapeData
	{
		// Token: 0x04006016 RID: 24598
		public Block Block;

		// Token: 0x04006017 RID: 24599
		public int Index;
	}
}

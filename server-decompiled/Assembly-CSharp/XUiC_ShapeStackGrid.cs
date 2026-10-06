using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001035 RID: 4149
[Preserve]
public class XUiC_ShapeStackGrid : XUiController
{
	// Token: 0x17000F66 RID: 3942
	// (get) Token: 0x060082C5 RID: 33477 RVA: 0x0031C7CA File Offset: 0x0031A9CA
	// (set) Token: 0x060082C6 RID: 33478 RVA: 0x0031C7D2 File Offset: 0x0031A9D2
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F67 RID: 3943
	// (get) Token: 0x060082C7 RID: 33479 RVA: 0x0031C7DB File Offset: 0x0031A9DB
	// (set) Token: 0x060082C8 RID: 33480 RVA: 0x0031C7E3 File Offset: 0x0031A9E3
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

	// Token: 0x060082C9 RID: 33481 RVA: 0x0031C7F4 File Offset: 0x0031A9F4
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

	// Token: 0x060082CA RID: 33482 RVA: 0x0031C854 File Offset: 0x0031AA54
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

	// Token: 0x060082CB RID: 33483 RVA: 0x002FF7A6 File Offset: 0x002FD9A6
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x060082CC RID: 33484 RVA: 0x002ECFC5 File Offset: 0x002EB1C5
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x060082CD RID: 33485 RVA: 0x0031C9CE File Offset: 0x0031ABCE
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.SetShapes(this.currentList, this.Owner.ItemValue.Meta);
			this.IsDirty = false;
		}
	}

	// Token: 0x040060FD RID: 24829
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x040060FE RID: 24830
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x040060FF RID: 24831
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006101 RID: 24833
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] shapeControllers;

	// Token: 0x04006102 RID: 24834
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04006103 RID: 24835
	public XUiC_ShapesWindow Owner;

	// Token: 0x04006104 RID: 24836
	[PublicizedFrom(EAccessModifier.Private)]
	public Block selectedBlock;

	// Token: 0x04006105 RID: 24837
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ShapeStackGrid.ShapeData> currentList;

	// Token: 0x02001036 RID: 4150
	public class ShapeData
	{
		// Token: 0x04006106 RID: 24838
		public Block Block;

		// Token: 0x04006107 RID: 24839
		public int Index;
	}
}

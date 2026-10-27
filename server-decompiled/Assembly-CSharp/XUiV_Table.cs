using System;
using UnityEngine;

// Token: 0x02001189 RID: 4489
public class XUiV_Table : XUiView
{
	// Token: 0x17001131 RID: 4401
	// (get) Token: 0x06008EE7 RID: 36583 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x17001132 RID: 4402
	// (get) Token: 0x06008EE8 RID: 36584 RVA: 0x0035F6D7 File Offset: 0x0035D8D7
	// (set) Token: 0x06008EE9 RID: 36585 RVA: 0x0035F6DF File Offset: 0x0035D8DF
	[XuiXmlAttribute("sorting", false)]
	public UITable.Sorting Sorting { get; set; }

	// Token: 0x17001133 RID: 4403
	// (get) Token: 0x06008EEA RID: 36586 RVA: 0x0035F6E8 File Offset: 0x0035D8E8
	// (set) Token: 0x06008EEB RID: 36587 RVA: 0x0035F6F0 File Offset: 0x0035D8F0
	[XuiXmlAttribute("cols", false)]
	public int Columns { get; set; } = 1;

	// Token: 0x17001134 RID: 4404
	// (get) Token: 0x06008EEC RID: 36588 RVA: 0x0035F6F9 File Offset: 0x0035D8F9
	// (set) Token: 0x06008EED RID: 36589 RVA: 0x0035F701 File Offset: 0x0035D901
	[XuiXmlAttribute("padding", true)]
	public Vector2 Padding { get; set; }

	// Token: 0x17001135 RID: 4405
	// (get) Token: 0x06008EEE RID: 36590 RVA: 0x0035F70A File Offset: 0x0035D90A
	// (set) Token: 0x06008EEF RID: 36591 RVA: 0x0035F712 File Offset: 0x0035D912
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; } = true;

	// Token: 0x17001136 RID: 4406
	// (get) Token: 0x06008EF0 RID: 36592 RVA: 0x0035F71B File Offset: 0x0035D91B
	// (set) Token: 0x06008EF1 RID: 36593 RVA: 0x0035F723 File Offset: 0x0035D923
	[XuiXmlAttribute("always_reposition", false)]
	public bool AlwaysReposition { get; set; }

	// Token: 0x17001137 RID: 4407
	// (get) Token: 0x06008EF2 RID: 36594 RVA: 0x0035F72C File Offset: 0x0035D92C
	// (set) Token: 0x06008EF3 RID: 36595 RVA: 0x0035F734 File Offset: 0x0035D934
	[XuiXmlAttribute("reposition_twice", false)]
	public bool RepositionTwice { get; set; }

	// Token: 0x17001138 RID: 4408
	// (get) Token: 0x06008EF4 RID: 36596 RVA: 0x0035F73D File Offset: 0x0035D93D
	// (set) Token: 0x06008EF5 RID: 36597 RVA: 0x0035F745 File Offset: 0x0035D945
	[XuiXmlAttribute("pivot", false)]
	public UIWidget.Pivot Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			if (this.pivot == value)
			{
				return;
			}
			this.pivot = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001139 RID: 4409
	// (get) Token: 0x06008EF6 RID: 36598 RVA: 0x0035F75E File Offset: 0x0035D95E
	// (set) Token: 0x06008EF7 RID: 36599 RVA: 0x0035F766 File Offset: 0x0035D966
	[XuiXmlAttribute("cell_alignment", false)]
	public UIWidget.Pivot CellAlignment
	{
		get
		{
			return this.cellAlignment;
		}
		set
		{
			if (this.cellAlignment == value)
			{
				return;
			}
			this.cellAlignment = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700113A RID: 4410
	// (get) Token: 0x06008EF8 RID: 36600 RVA: 0x0035CA3D File Offset: 0x0035AC3D
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x06008EF9 RID: 36601 RVA: 0x0035F77F File Offset: 0x0035D97F
	public XUiV_Table(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EFA RID: 36602 RVA: 0x0035F79E File Offset: 0x0035D99E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UITable>();
	}

	// Token: 0x06008EFB RID: 36603 RVA: 0x0035F7A7 File Offset: 0x0035D9A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.table = this.uiTransform.gameObject.GetComponent<UITable>();
	}

	// Token: 0x06008EFC RID: 36604 RVA: 0x0035F7C8 File Offset: 0x0035D9C8
	public override void InitView()
	{
		base.InitView();
		this.table.hideInactive = this.HideInactive;
		this.table.sorting = this.Sorting;
		this.table.direction = UITable.Direction.Down;
		this.table.columns = this.Columns;
		this.table.padding = this.Padding;
		this.table.pivot = this.pivot;
		this.table.cellAlignment = this.cellAlignment;
		this.table.Reposition();
	}

	// Token: 0x06008EFD RID: 36605 RVA: 0x0035F858 File Offset: 0x0035DA58
	public override void OnOpen()
	{
		base.OnOpen();
		this.table.repositionNow = true;
		if (this.RepositionTwice)
		{
			this.repositionNextFrame = true;
		}
	}

	// Token: 0x06008EFE RID: 36606 RVA: 0x0035F87C File Offset: 0x0035DA7C
	public override void Update(float _dt)
	{
		if (this.firstUpdate || this.AlwaysReposition)
		{
			this.firstUpdate = false;
			this.table.Reposition();
		}
		if (this.repositionNextFrame && !this.table.enabled)
		{
			this.table.repositionNow = true;
			this.repositionNextFrame = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06008EFF RID: 36607 RVA: 0x0035F8DA File Offset: 0x0035DADA
	public void Reposition()
	{
		this.repositionNextFrame = true;
	}

	// Token: 0x040068E7 RID: 26855
	[PublicizedFrom(EAccessModifier.Private)]
	public UITable table;

	// Token: 0x040068EE RID: 26862
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;

	// Token: 0x040068EF RID: 26863
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot cellAlignment;

	// Token: 0x040068F0 RID: 26864
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdate = true;

	// Token: 0x040068F1 RID: 26865
	[PublicizedFrom(EAccessModifier.Private)]
	public bool repositionNextFrame;
}

using System;
using UnityEngine;

// Token: 0x02001161 RID: 4449
public class XUiV_Table : XUiView
{
	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06008E08 RID: 36360 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06008E09 RID: 36361 RVA: 0x00358E43 File Offset: 0x00357043
	// (set) Token: 0x06008E0A RID: 36362 RVA: 0x00358E4B File Offset: 0x0035704B
	[XuiXmlAttribute("sorting", false)]
	public UITable.Sorting Sorting { get; set; }

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06008E0B RID: 36363 RVA: 0x00358E54 File Offset: 0x00357054
	// (set) Token: 0x06008E0C RID: 36364 RVA: 0x00358E5C File Offset: 0x0035705C
	[XuiXmlAttribute("cols", false)]
	public int Columns { get; set; } = 1;

	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x06008E0D RID: 36365 RVA: 0x00358E65 File Offset: 0x00357065
	// (set) Token: 0x06008E0E RID: 36366 RVA: 0x00358E6D File Offset: 0x0035706D
	[XuiXmlAttribute("padding", true)]
	public Vector2 Padding { get; set; }

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x06008E0F RID: 36367 RVA: 0x00358E76 File Offset: 0x00357076
	// (set) Token: 0x06008E10 RID: 36368 RVA: 0x00358E7E File Offset: 0x0035707E
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; } = true;

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06008E11 RID: 36369 RVA: 0x00358E87 File Offset: 0x00357087
	// (set) Token: 0x06008E12 RID: 36370 RVA: 0x00358E8F File Offset: 0x0035708F
	[XuiXmlAttribute("always_reposition", false)]
	public bool AlwaysReposition { get; set; }

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06008E13 RID: 36371 RVA: 0x00358E98 File Offset: 0x00357098
	// (set) Token: 0x06008E14 RID: 36372 RVA: 0x00358EA0 File Offset: 0x003570A0
	[XuiXmlAttribute("reposition_twice", false)]
	public bool RepositionTwice { get; set; }

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06008E15 RID: 36373 RVA: 0x00358EA9 File Offset: 0x003570A9
	// (set) Token: 0x06008E16 RID: 36374 RVA: 0x00358EB1 File Offset: 0x003570B1
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

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06008E17 RID: 36375 RVA: 0x00358ECA File Offset: 0x003570CA
	// (set) Token: 0x06008E18 RID: 36376 RVA: 0x00358ED2 File Offset: 0x003570D2
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

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06008E19 RID: 36377 RVA: 0x00356571 File Offset: 0x00354771
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x06008E1A RID: 36378 RVA: 0x00358EEB File Offset: 0x003570EB
	public XUiV_Table(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E1B RID: 36379 RVA: 0x00358F0A File Offset: 0x0035710A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UITable>();
	}

	// Token: 0x06008E1C RID: 36380 RVA: 0x00358F13 File Offset: 0x00357113
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.table = this.uiTransform.gameObject.GetComponent<UITable>();
	}

	// Token: 0x06008E1D RID: 36381 RVA: 0x00358F34 File Offset: 0x00357134
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

	// Token: 0x06008E1E RID: 36382 RVA: 0x00358FC4 File Offset: 0x003571C4
	public override void OnOpen()
	{
		base.OnOpen();
		this.table.repositionNow = true;
		if (this.RepositionTwice)
		{
			this.repositionNextFrame = true;
		}
	}

	// Token: 0x06008E1F RID: 36383 RVA: 0x00358FE8 File Offset: 0x003571E8
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

	// Token: 0x06008E20 RID: 36384 RVA: 0x00359046 File Offset: 0x00357246
	public void Reposition()
	{
		this.repositionNextFrame = true;
	}

	// Token: 0x04006842 RID: 26690
	[PublicizedFrom(EAccessModifier.Private)]
	public UITable table;

	// Token: 0x04006849 RID: 26697
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;

	// Token: 0x0400684A RID: 26698
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot cellAlignment;

	// Token: 0x0400684B RID: 26699
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdate = true;

	// Token: 0x0400684C RID: 26700
	[PublicizedFrom(EAccessModifier.Private)]
	public bool repositionNextFrame;
}

using System;
using UnityEngine;

// Token: 0x02001179 RID: 4473
public class XUiV_Table : XUiView
{
	// Token: 0x17001126 RID: 4390
	// (get) Token: 0x06008E93 RID: 36499 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x17001127 RID: 4391
	// (get) Token: 0x06008E94 RID: 36500 RVA: 0x0035D5E7 File Offset: 0x0035B7E7
	// (set) Token: 0x06008E95 RID: 36501 RVA: 0x0035D5EF File Offset: 0x0035B7EF
	[XuiXmlAttribute("sorting", false)]
	public UITable.Sorting Sorting { get; set; }

	// Token: 0x17001128 RID: 4392
	// (get) Token: 0x06008E96 RID: 36502 RVA: 0x0035D5F8 File Offset: 0x0035B7F8
	// (set) Token: 0x06008E97 RID: 36503 RVA: 0x0035D600 File Offset: 0x0035B800
	[XuiXmlAttribute("cols", false)]
	public int Columns { get; set; } = 1;

	// Token: 0x17001129 RID: 4393
	// (get) Token: 0x06008E98 RID: 36504 RVA: 0x0035D609 File Offset: 0x0035B809
	// (set) Token: 0x06008E99 RID: 36505 RVA: 0x0035D611 File Offset: 0x0035B811
	[XuiXmlAttribute("padding", true)]
	public Vector2 Padding { get; set; }

	// Token: 0x1700112A RID: 4394
	// (get) Token: 0x06008E9A RID: 36506 RVA: 0x0035D61A File Offset: 0x0035B81A
	// (set) Token: 0x06008E9B RID: 36507 RVA: 0x0035D622 File Offset: 0x0035B822
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; } = true;

	// Token: 0x1700112B RID: 4395
	// (get) Token: 0x06008E9C RID: 36508 RVA: 0x0035D62B File Offset: 0x0035B82B
	// (set) Token: 0x06008E9D RID: 36509 RVA: 0x0035D633 File Offset: 0x0035B833
	[XuiXmlAttribute("always_reposition", false)]
	public bool AlwaysReposition { get; set; }

	// Token: 0x1700112C RID: 4396
	// (get) Token: 0x06008E9E RID: 36510 RVA: 0x0035D63C File Offset: 0x0035B83C
	// (set) Token: 0x06008E9F RID: 36511 RVA: 0x0035D644 File Offset: 0x0035B844
	[XuiXmlAttribute("reposition_twice", false)]
	public bool RepositionTwice { get; set; }

	// Token: 0x1700112D RID: 4397
	// (get) Token: 0x06008EA0 RID: 36512 RVA: 0x0035D64D File Offset: 0x0035B84D
	// (set) Token: 0x06008EA1 RID: 36513 RVA: 0x0035D655 File Offset: 0x0035B855
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

	// Token: 0x1700112E RID: 4398
	// (get) Token: 0x06008EA2 RID: 36514 RVA: 0x0035D66E File Offset: 0x0035B86E
	// (set) Token: 0x06008EA3 RID: 36515 RVA: 0x0035D676 File Offset: 0x0035B876
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

	// Token: 0x1700112F RID: 4399
	// (get) Token: 0x06008EA4 RID: 36516 RVA: 0x0035A991 File Offset: 0x00358B91
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x06008EA5 RID: 36517 RVA: 0x0035D68F File Offset: 0x0035B88F
	public XUiV_Table(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EA6 RID: 36518 RVA: 0x0035D6AE File Offset: 0x0035B8AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UITable>();
	}

	// Token: 0x06008EA7 RID: 36519 RVA: 0x0035D6B7 File Offset: 0x0035B8B7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.table = this.uiTransform.gameObject.GetComponent<UITable>();
	}

	// Token: 0x06008EA8 RID: 36520 RVA: 0x0035D6D8 File Offset: 0x0035B8D8
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

	// Token: 0x06008EA9 RID: 36521 RVA: 0x0035D768 File Offset: 0x0035B968
	public override void OnOpen()
	{
		base.OnOpen();
		this.table.repositionNow = true;
		if (this.RepositionTwice)
		{
			this.repositionNextFrame = true;
		}
	}

	// Token: 0x06008EAA RID: 36522 RVA: 0x0035D78C File Offset: 0x0035B98C
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

	// Token: 0x06008EAB RID: 36523 RVA: 0x0035D7EA File Offset: 0x0035B9EA
	public void Reposition()
	{
		this.repositionNextFrame = true;
	}

	// Token: 0x040068A7 RID: 26791
	[PublicizedFrom(EAccessModifier.Private)]
	public UITable table;

	// Token: 0x040068AE RID: 26798
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;

	// Token: 0x040068AF RID: 26799
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot cellAlignment;

	// Token: 0x040068B0 RID: 26800
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdate = true;

	// Token: 0x040068B1 RID: 26801
	[PublicizedFrom(EAccessModifier.Private)]
	public bool repositionNextFrame;
}

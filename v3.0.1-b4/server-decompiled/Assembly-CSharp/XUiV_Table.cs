using System;
using UnityEngine;

// Token: 0x02001148 RID: 4424
public class XUiV_Table : XUiView
{
	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x06008D16 RID: 36118 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x06008D17 RID: 36119 RVA: 0x00354607 File Offset: 0x00352807
	// (set) Token: 0x06008D18 RID: 36120 RVA: 0x0035460F File Offset: 0x0035280F
	[XuiXmlAttribute("sorting", false)]
	public UITable.Sorting Sorting { get; set; }

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x06008D19 RID: 36121 RVA: 0x00354618 File Offset: 0x00352818
	// (set) Token: 0x06008D1A RID: 36122 RVA: 0x00354620 File Offset: 0x00352820
	[XuiXmlAttribute("cols", false)]
	public int Columns { get; set; } = 1;

	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x06008D1B RID: 36123 RVA: 0x00354629 File Offset: 0x00352829
	// (set) Token: 0x06008D1C RID: 36124 RVA: 0x00354631 File Offset: 0x00352831
	[XuiXmlAttribute("padding", true)]
	public Vector2 Padding { get; set; }

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x06008D1D RID: 36125 RVA: 0x0035463A File Offset: 0x0035283A
	// (set) Token: 0x06008D1E RID: 36126 RVA: 0x00354642 File Offset: 0x00352842
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; } = true;

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x06008D1F RID: 36127 RVA: 0x0035464B File Offset: 0x0035284B
	// (set) Token: 0x06008D20 RID: 36128 RVA: 0x00354653 File Offset: 0x00352853
	[XuiXmlAttribute("always_reposition", false)]
	public bool AlwaysReposition { get; set; }

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x06008D21 RID: 36129 RVA: 0x0035465C File Offset: 0x0035285C
	// (set) Token: 0x06008D22 RID: 36130 RVA: 0x00354664 File Offset: 0x00352864
	[XuiXmlAttribute("reposition_twice", false)]
	public bool RepositionTwice { get; set; }

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x06008D23 RID: 36131 RVA: 0x0035466D File Offset: 0x0035286D
	// (set) Token: 0x06008D24 RID: 36132 RVA: 0x00354675 File Offset: 0x00352875
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

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x06008D25 RID: 36133 RVA: 0x0035468E File Offset: 0x0035288E
	// (set) Token: 0x06008D26 RID: 36134 RVA: 0x00354696 File Offset: 0x00352896
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

	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06008D27 RID: 36135 RVA: 0x00351D3D File Offset: 0x0034FF3D
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x06008D28 RID: 36136 RVA: 0x003546AF File Offset: 0x003528AF
	public XUiV_Table(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D29 RID: 36137 RVA: 0x003546CE File Offset: 0x003528CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UITable>();
	}

	// Token: 0x06008D2A RID: 36138 RVA: 0x003546D7 File Offset: 0x003528D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.table = this.uiTransform.gameObject.GetComponent<UITable>();
	}

	// Token: 0x06008D2B RID: 36139 RVA: 0x003546F8 File Offset: 0x003528F8
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

	// Token: 0x06008D2C RID: 36140 RVA: 0x00354788 File Offset: 0x00352988
	public override void OnOpen()
	{
		base.OnOpen();
		this.table.repositionNow = true;
		if (this.RepositionTwice)
		{
			this.repositionNextFrame = true;
		}
	}

	// Token: 0x06008D2D RID: 36141 RVA: 0x003547AC File Offset: 0x003529AC
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

	// Token: 0x06008D2E RID: 36142 RVA: 0x0035480A File Offset: 0x00352A0A
	public void Reposition()
	{
		this.repositionNextFrame = true;
	}

	// Token: 0x04006778 RID: 26488
	[PublicizedFrom(EAccessModifier.Private)]
	public UITable table;

	// Token: 0x0400677F RID: 26495
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;

	// Token: 0x04006780 RID: 26496
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot cellAlignment;

	// Token: 0x04006781 RID: 26497
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstUpdate = true;

	// Token: 0x04006782 RID: 26498
	[PublicizedFrom(EAccessModifier.Private)]
	public bool repositionNextFrame;
}

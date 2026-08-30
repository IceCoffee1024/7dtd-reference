using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200113E RID: 4414
public class XUiV_Grid : XUiView_WidgetBased
{
	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06008C37 RID: 35895 RVA: 0x0035252D File Offset: 0x0035072D
	// (set) Token: 0x06008C38 RID: 35896 RVA: 0x00352535 File Offset: 0x00350735
	[XuiXmlAttribute("arrangement", false)]
	public UIGrid.Arrangement Arrangement { get; set; }

	// Token: 0x140000EB RID: 235
	// (add) Token: 0x06008C39 RID: 35897 RVA: 0x00352540 File Offset: 0x00350740
	// (remove) Token: 0x06008C3A RID: 35898 RVA: 0x00352578 File Offset: 0x00350778
	public event UIGrid.OnSizeChanged OnSizeChanged;

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06008C3B RID: 35899 RVA: 0x003525AD File Offset: 0x003507AD
	// (set) Token: 0x06008C3C RID: 35900 RVA: 0x003525B5 File Offset: 0x003507B5
	[XuiXmlAttribute("cols", false)]
	public int Columns
	{
		get
		{
			return this.columns;
		}
		set
		{
			if (this.columns == value)
			{
				return;
			}
			this.columns = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001066 RID: 4198
	// (get) Token: 0x06008C3D RID: 35901 RVA: 0x003525CE File Offset: 0x003507CE
	// (set) Token: 0x06008C3E RID: 35902 RVA: 0x003525D6 File Offset: 0x003507D6
	[XuiXmlAttribute("rows", false)]
	public int Rows
	{
		get
		{
			return this.rows;
		}
		set
		{
			if (this.rows == value)
			{
				return;
			}
			this.rows = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001067 RID: 4199
	// (get) Token: 0x06008C3F RID: 35903 RVA: 0x003525EF File Offset: 0x003507EF
	// (set) Token: 0x06008C40 RID: 35904 RVA: 0x000027FC File Offset: 0x000009FC
	public override int RepeatCount
	{
		get
		{
			return this.Columns * this.Rows;
		}
		set
		{
		}
	}

	// Token: 0x17001068 RID: 4200
	// (get) Token: 0x06008C41 RID: 35905 RVA: 0x003525FE File Offset: 0x003507FE
	// (set) Token: 0x06008C42 RID: 35906 RVA: 0x00352606 File Offset: 0x00350806
	[XuiXmlAttribute("cell_width", false)]
	public int CellWidth { get; set; }

	// Token: 0x17001069 RID: 4201
	// (get) Token: 0x06008C43 RID: 35907 RVA: 0x0035260F File Offset: 0x0035080F
	// (set) Token: 0x06008C44 RID: 35908 RVA: 0x00352617 File Offset: 0x00350817
	[XuiXmlAttribute("cell_height", false)]
	public int CellHeight { get; set; }

	// Token: 0x1700106A RID: 4202
	// (get) Token: 0x06008C45 RID: 35909 RVA: 0x00352620 File Offset: 0x00350820
	// (set) Token: 0x06008C46 RID: 35910 RVA: 0x00352628 File Offset: 0x00350828
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; }

	// Token: 0x1700106B RID: 4203
	// (get) Token: 0x06008C47 RID: 35911 RVA: 0x00352631 File Offset: 0x00350831
	public override Vector2i InnerSize
	{
		get
		{
			return new Vector2i(this.CellWidth, this.CellHeight);
		}
	}

	// Token: 0x06008C48 RID: 35912 RVA: 0x00352644 File Offset: 0x00350844
	public XUiV_Grid(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008C49 RID: 35913 RVA: 0x0035264E File Offset: 0x0035084E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
		_go.AddComponent<UIGrid>();
	}

	// Token: 0x06008C4A RID: 35914 RVA: 0x0035265E File Offset: 0x0035085E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
		this.grid = this.uiTransform.gameObject.GetComponent<UIGrid>();
	}

	// Token: 0x06008C4B RID: 35915 RVA: 0x00352694 File Offset: 0x00350894
	public override void InitView()
	{
		base.InitView();
		this.widget.autoResizeBoxCollider = true;
		this.grid.hideInactive = this.HideInactive;
		this.grid.arrangement = this.Arrangement;
		this.grid.pivot = this.pivot;
		this.grid.onSizeChanged = new UIGrid.OnSizeChanged(this.OnGridSizeChanged);
		this.grid.maxPerLine = ((this.Arrangement == UIGrid.Arrangement.Horizontal) ? this.Columns : this.Rows);
		this.grid.cellWidth = (float)this.CellWidth;
		this.grid.cellHeight = (float)this.CellHeight;
	}

	// Token: 0x06008C4C RID: 35916 RVA: 0x00352744 File Offset: 0x00350944
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGridSizeChanged(Vector2Int _cells, Vector2 _size)
	{
		this.widget.width = Mathf.RoundToInt(_size.x);
		this.widget.height = Mathf.RoundToInt(_size.y);
		this.size = new Vector2i(this.widget.width, this.widget.height);
		UIGrid.OnSizeChanged onSizeChanged = this.OnSizeChanged;
		if (onSizeChanged == null)
		{
			return;
		}
		onSizeChanged(_cells, _size);
	}

	// Token: 0x06008C4D RID: 35917 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void refreshBoxCollider()
	{
	}

	// Token: 0x06008C4E RID: 35918 RVA: 0x003527B0 File Offset: 0x003509B0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.grid.repositionNow = true;
	}

	// Token: 0x06008C4F RID: 35919 RVA: 0x003527C8 File Offset: 0x003509C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.grid.maxPerLine = ((this.Arrangement == UIGrid.Arrangement.Horizontal) ? this.Columns : this.Rows);
		this.grid.cellWidth = (float)this.CellWidth;
		this.grid.cellHeight = (float)this.CellHeight;
		base.updateData();
	}

	// Token: 0x06008C50 RID: 35920 RVA: 0x00352820 File Offset: 0x00350A20
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.Columns = 0;
		this.Rows = 0;
		this.CellWidth = int.MinValue;
		this.CellHeight = int.MinValue;
		this.Arrangement = UIGrid.Arrangement.Horizontal;
		this.HideInactive = true;
	}

	// Token: 0x06008C51 RID: 35921 RVA: 0x0035285B File Offset: 0x00350A5B
	public override void SetPostParsingDefaults(XUiController _parent)
	{
		base.SetPostParsingDefaults(_parent);
		if (this.CellWidth == -2147483648)
		{
			this.CellWidth = base.Width;
		}
		if (this.CellHeight == -2147483648)
		{
			this.CellHeight = base.Height;
		}
	}

	// Token: 0x06008C52 RID: 35922 RVA: 0x00352898 File Offset: 0x00350A98
	public override void SetRepeatContentTemplateParams(Dictionary<string, object> _templateParams, int _curRepeatNum)
	{
		base.SetRepeatContentTemplateParams(_templateParams, _curRepeatNum);
		int num;
		int num2;
		if (this.Arrangement == UIGrid.Arrangement.Horizontal)
		{
			num = _curRepeatNum % this.Columns;
			num2 = _curRepeatNum / this.Columns;
		}
		else
		{
			num = _curRepeatNum / this.Rows;
			num2 = _curRepeatNum % this.Rows;
		}
		_templateParams["repeat_col"] = num;
		_templateParams["repeat_row"] = num2;
	}

	// Token: 0x04006729 RID: 26409
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x0400672A RID: 26410
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x0400672B RID: 26411
	[PublicizedFrom(EAccessModifier.Private)]
	public UIGrid grid;
}

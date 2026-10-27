using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200117F RID: 4479
public class XUiV_Grid : XUiView_WidgetBased
{
	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06008DE8 RID: 36328 RVA: 0x0035D235 File Offset: 0x0035B435
	// (set) Token: 0x06008DE9 RID: 36329 RVA: 0x0035D23D File Offset: 0x0035B43D
	[XuiXmlAttribute("arrangement", false)]
	public UIGrid.Arrangement Arrangement { get; set; }

	// Token: 0x140000EB RID: 235
	// (add) Token: 0x06008DEA RID: 36330 RVA: 0x0035D248 File Offset: 0x0035B448
	// (remove) Token: 0x06008DEB RID: 36331 RVA: 0x0035D280 File Offset: 0x0035B480
	public event UIGrid.OnSizeChanged OnSizeChanged;

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x06008DEC RID: 36332 RVA: 0x0035D2B5 File Offset: 0x0035B4B5
	// (set) Token: 0x06008DED RID: 36333 RVA: 0x0035D2BD File Offset: 0x0035B4BD
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

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06008DEE RID: 36334 RVA: 0x0035D2D6 File Offset: 0x0035B4D6
	// (set) Token: 0x06008DEF RID: 36335 RVA: 0x0035D2DE File Offset: 0x0035B4DE
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

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x06008DF0 RID: 36336 RVA: 0x0035D2F7 File Offset: 0x0035B4F7
	// (set) Token: 0x06008DF1 RID: 36337 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06008DF2 RID: 36338 RVA: 0x0035D306 File Offset: 0x0035B506
	// (set) Token: 0x06008DF3 RID: 36339 RVA: 0x0035D30E File Offset: 0x0035B50E
	[XuiXmlAttribute("cell_width", false)]
	public int CellWidth { get; set; }

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06008DF4 RID: 36340 RVA: 0x0035D317 File Offset: 0x0035B517
	// (set) Token: 0x06008DF5 RID: 36341 RVA: 0x0035D31F File Offset: 0x0035B51F
	[XuiXmlAttribute("cell_height", false)]
	public int CellHeight { get; set; }

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06008DF6 RID: 36342 RVA: 0x0035D328 File Offset: 0x0035B528
	// (set) Token: 0x06008DF7 RID: 36343 RVA: 0x0035D330 File Offset: 0x0035B530
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; }

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x0035D339 File Offset: 0x0035B539
	public override Vector2i InnerSize
	{
		get
		{
			return new Vector2i(this.CellWidth, this.CellHeight);
		}
	}

	// Token: 0x06008DF9 RID: 36345 RVA: 0x0035D34C File Offset: 0x0035B54C
	public XUiV_Grid(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008DFA RID: 36346 RVA: 0x0035D356 File Offset: 0x0035B556
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
		_go.AddComponent<UIGrid>();
	}

	// Token: 0x06008DFB RID: 36347 RVA: 0x0035D366 File Offset: 0x0035B566
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
		this.grid = this.uiTransform.gameObject.GetComponent<UIGrid>();
	}

	// Token: 0x06008DFC RID: 36348 RVA: 0x0035D39C File Offset: 0x0035B59C
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

	// Token: 0x06008DFD RID: 36349 RVA: 0x0035D44C File Offset: 0x0035B64C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGridSizeChanged(Vector2Int _cells, Vector2 _size)
	{
		this.widget.width = Mathf.RoundToInt(_size.x);
		this.widget.height = Mathf.RoundToInt(_size.y);
		this.size = new Vector2i(this.widget.width, this.widget.height);
		if (this.Arrangement == UIGrid.Arrangement.Horizontal)
		{
			this.Rows = _cells.y;
		}
		else
		{
			this.Columns = _cells.x;
		}
		UIGrid.OnSizeChanged onSizeChanged = this.OnSizeChanged;
		if (onSizeChanged == null)
		{
			return;
		}
		onSizeChanged(_cells, _size);
	}

	// Token: 0x06008DFE RID: 36350 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void refreshBoxCollider()
	{
	}

	// Token: 0x06008DFF RID: 36351 RVA: 0x0035D4DC File Offset: 0x0035B6DC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.grid.repositionNow = true;
	}

	// Token: 0x06008E00 RID: 36352 RVA: 0x0035D4F4 File Offset: 0x0035B6F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.grid.maxPerLine = ((this.Arrangement == UIGrid.Arrangement.Horizontal) ? this.Columns : this.Rows);
		this.grid.cellWidth = (float)this.CellWidth;
		this.grid.cellHeight = (float)this.CellHeight;
		base.updateData();
	}

	// Token: 0x06008E01 RID: 36353 RVA: 0x0035D54C File Offset: 0x0035B74C
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

	// Token: 0x06008E02 RID: 36354 RVA: 0x0035D587 File Offset: 0x0035B787
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

	// Token: 0x06008E03 RID: 36355 RVA: 0x0035D5C4 File Offset: 0x0035B7C4
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

	// Token: 0x04006888 RID: 26760
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x04006889 RID: 26761
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x0400688A RID: 26762
	[PublicizedFrom(EAccessModifier.Private)]
	public UIGrid grid;
}

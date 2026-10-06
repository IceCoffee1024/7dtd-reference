using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200116F RID: 4463
public class XUiV_Grid : XUiView_WidgetBased
{
	// Token: 0x170010D2 RID: 4306
	// (get) Token: 0x06008D94 RID: 36244 RVA: 0x0035B189 File Offset: 0x00359389
	// (set) Token: 0x06008D95 RID: 36245 RVA: 0x0035B191 File Offset: 0x00359391
	[XuiXmlAttribute("arrangement", false)]
	public UIGrid.Arrangement Arrangement { get; set; }

	// Token: 0x140000EB RID: 235
	// (add) Token: 0x06008D96 RID: 36246 RVA: 0x0035B19C File Offset: 0x0035939C
	// (remove) Token: 0x06008D97 RID: 36247 RVA: 0x0035B1D4 File Offset: 0x003593D4
	public event UIGrid.OnSizeChanged OnSizeChanged;

	// Token: 0x170010D3 RID: 4307
	// (get) Token: 0x06008D98 RID: 36248 RVA: 0x0035B209 File Offset: 0x00359409
	// (set) Token: 0x06008D99 RID: 36249 RVA: 0x0035B211 File Offset: 0x00359411
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

	// Token: 0x170010D4 RID: 4308
	// (get) Token: 0x06008D9A RID: 36250 RVA: 0x0035B22A File Offset: 0x0035942A
	// (set) Token: 0x06008D9B RID: 36251 RVA: 0x0035B232 File Offset: 0x00359432
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

	// Token: 0x170010D5 RID: 4309
	// (get) Token: 0x06008D9C RID: 36252 RVA: 0x0035B24B File Offset: 0x0035944B
	// (set) Token: 0x06008D9D RID: 36253 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x06008D9E RID: 36254 RVA: 0x0035B25A File Offset: 0x0035945A
	// (set) Token: 0x06008D9F RID: 36255 RVA: 0x0035B262 File Offset: 0x00359462
	[XuiXmlAttribute("cell_width", false)]
	public int CellWidth { get; set; }

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x06008DA0 RID: 36256 RVA: 0x0035B26B File Offset: 0x0035946B
	// (set) Token: 0x06008DA1 RID: 36257 RVA: 0x0035B273 File Offset: 0x00359473
	[XuiXmlAttribute("cell_height", false)]
	public int CellHeight { get; set; }

	// Token: 0x170010D8 RID: 4312
	// (get) Token: 0x06008DA2 RID: 36258 RVA: 0x0035B27C File Offset: 0x0035947C
	// (set) Token: 0x06008DA3 RID: 36259 RVA: 0x0035B284 File Offset: 0x00359484
	[XuiXmlAttribute("hide_inactive", false)]
	public bool HideInactive { get; set; }

	// Token: 0x170010D9 RID: 4313
	// (get) Token: 0x06008DA4 RID: 36260 RVA: 0x0035B28D File Offset: 0x0035948D
	public override Vector2i InnerSize
	{
		get
		{
			return new Vector2i(this.CellWidth, this.CellHeight);
		}
	}

	// Token: 0x06008DA5 RID: 36261 RVA: 0x0035B2A0 File Offset: 0x003594A0
	public XUiV_Grid(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008DA6 RID: 36262 RVA: 0x0035B2AA File Offset: 0x003594AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
		_go.AddComponent<UIGrid>();
	}

	// Token: 0x06008DA7 RID: 36263 RVA: 0x0035B2BA File Offset: 0x003594BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
		this.grid = this.uiTransform.gameObject.GetComponent<UIGrid>();
	}

	// Token: 0x06008DA8 RID: 36264 RVA: 0x0035B2F0 File Offset: 0x003594F0
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

	// Token: 0x06008DA9 RID: 36265 RVA: 0x0035B3A0 File Offset: 0x003595A0
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

	// Token: 0x06008DAA RID: 36266 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void refreshBoxCollider()
	{
	}

	// Token: 0x06008DAB RID: 36267 RVA: 0x0035B40C File Offset: 0x0035960C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.grid.repositionNow = true;
	}

	// Token: 0x06008DAC RID: 36268 RVA: 0x0035B424 File Offset: 0x00359624
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.grid.maxPerLine = ((this.Arrangement == UIGrid.Arrangement.Horizontal) ? this.Columns : this.Rows);
		this.grid.cellWidth = (float)this.CellWidth;
		this.grid.cellHeight = (float)this.CellHeight;
		base.updateData();
	}

	// Token: 0x06008DAD RID: 36269 RVA: 0x0035B47C File Offset: 0x0035967C
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

	// Token: 0x06008DAE RID: 36270 RVA: 0x0035B4B7 File Offset: 0x003596B7
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

	// Token: 0x06008DAF RID: 36271 RVA: 0x0035B4F4 File Offset: 0x003596F4
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

	// Token: 0x04006848 RID: 26696
	[PublicizedFrom(EAccessModifier.Private)]
	public int columns;

	// Token: 0x04006849 RID: 26697
	[PublicizedFrom(EAccessModifier.Private)]
	public int rows;

	// Token: 0x0400684A RID: 26698
	[PublicizedFrom(EAccessModifier.Private)]
	public UIGrid grid;
}

using System;
using UnityEngine.Scripting;

// Token: 0x02000D5A RID: 3418
[Preserve]
public class XUiC_PopupMenuItem : XUiController
{
	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x060067A6 RID: 26534 RVA: 0x002920CB File Offset: 0x002902CB
	[XuiXmlBinding("menuicon")]
	public string Icon
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_PopupMenuItem.Entry entry = this.itemEntry;
			return ((entry != null) ? entry.IconName : null) ?? "";
		}
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x060067A7 RID: 26535 RVA: 0x002920E8 File Offset: 0x002902E8
	[XuiXmlBinding("menutext")]
	public string Text
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_PopupMenuItem.Entry entry = this.itemEntry;
			return ((entry != null) ? entry.Text : null) ?? "";
		}
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x060067A8 RID: 26536 RVA: 0x00292105 File Offset: 0x00290305
	[XuiXmlBinding("enabled")]
	public bool EntryEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_PopupMenuItem.Entry entry = this.itemEntry;
			return entry != null && entry.IsEnabled;
		}
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x060067A9 RID: 26537 RVA: 0x00292118 File Offset: 0x00290318
	[XuiXmlBinding("hovered")]
	public bool EntryHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x060067AA RID: 26538 RVA: 0x00292120 File Offset: 0x00290320
	[XuiXmlBinding("type")]
	public XUiC_PopupMenuItem.EEntryType EntryType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_PopupMenuItem.Entry entry = this.itemEntry;
			if (entry == null)
			{
				return XUiC_PopupMenuItem.EEntryType.Button;
			}
			return entry.EntryType;
		}
	}

	// Token: 0x060067AB RID: 26539 RVA: 0x00292133 File Offset: 0x00290333
	public override void Init()
	{
		base.Init();
		this.label.Overflow = UILabel.Overflow.ResizeFreely;
	}

	// Token: 0x060067AC RID: 26540 RVA: 0x00292147 File Offset: 0x00290347
	[XuiBindEvent("OnHoveredStateChanged", "slider")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void sliderHoveredChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart)
	{
		this.OnHovered(this, _isOverAnyPart);
	}

	// Token: 0x060067AD RID: 26541 RVA: 0x00292151 File Offset: 0x00290351
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHovered(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
		this.parentPopup.IsOver = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x060067AE RID: 26542 RVA: 0x0029216C File Offset: 0x0029036C
	[XuiBindEvent("OnPress", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_PopupMenuItem.Entry entry = this.itemEntry;
		if (entry == null)
		{
			return;
		}
		if (entry.EntryType != XUiC_PopupMenuItem.EEntryType.Button)
		{
			return;
		}
		this.parentPopup.Close();
		if (entry.IsEnabled)
		{
			entry.HandleItemClicked();
		}
	}

	// Token: 0x060067AF RID: 26543 RVA: 0x002921A6 File Offset: 0x002903A6
	[XuiBindEvent("OnValueChanged", "slider")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		if (this.itemEntry == null)
		{
			return;
		}
		if (this.itemEntry.EntryType != XUiC_PopupMenuItem.EEntryType.Slider)
		{
			return;
		}
		if (this.itemEntry.IsEnabled)
		{
			this.itemEntry.HandleValueChanged(_newValue);
		}
	}

	// Token: 0x060067B0 RID: 26544 RVA: 0x002921DC File Offset: 0x002903DC
	public int SetEntry(XUiC_PopupMenuItem.Entry _entry)
	{
		this.itemEntry = _entry;
		base.RefreshBindings();
		this.label.SetTextImmediately(this.label.Text);
		base.ViewComponent.IsVisible = _entry != null;
		if (_entry == null)
		{
			return 0;
		}
		if (_entry.EntryType == XUiC_PopupMenuItem.EEntryType.Slider)
		{
			this.slider.Value = _entry.Value;
			this.slider.Enabled = _entry.IsEnabled;
		}
		if (_entry.EntryType != XUiC_PopupMenuItem.EEntryType.Button)
		{
			return this.parentPopup.SliderMinWidth;
		}
		return (int)this.label.PrintedSize.x;
	}

	// Token: 0x04004FFF RID: 20479
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005000 RID: 20480
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenu parentPopup;

	// Token: 0x04005001 RID: 20481
	[XuiBindComponent("lblText", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04005002 RID: 20482
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat slider;

	// Token: 0x04005003 RID: 20483
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupMenuItem.Entry itemEntry;

	// Token: 0x02000D5B RID: 3419
	public enum EEntryType
	{
		// Token: 0x04005005 RID: 20485
		Button,
		// Token: 0x04005006 RID: 20486
		Slider
	}

	// Token: 0x02000D5C RID: 3420
	public class Entry
	{
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x060067B2 RID: 26546 RVA: 0x00292270 File Offset: 0x00290470
		// (set) Token: 0x060067B3 RID: 26547 RVA: 0x00292278 File Offset: 0x00290478
		public object Tag { get; set; }

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060067B4 RID: 26548 RVA: 0x00292284 File Offset: 0x00290484
		// (remove) Token: 0x060067B5 RID: 26549 RVA: 0x002922BC File Offset: 0x002904BC
		public event XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate ItemClicked;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060067B6 RID: 26550 RVA: 0x002922F4 File Offset: 0x002904F4
		// (remove) Token: 0x060067B7 RID: 26551 RVA: 0x0029232C File Offset: 0x0029052C
		public event XUiC_PopupMenuItem.Entry.MenuItemValueChangedDelegate ValueChanged;

		// Token: 0x060067B8 RID: 26552 RVA: 0x00292361 File Offset: 0x00290561
		public void HandleItemClicked()
		{
			XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate itemClicked = this.ItemClicked;
			if (itemClicked == null)
			{
				return;
			}
			itemClicked(this);
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x00292374 File Offset: 0x00290574
		public void HandleValueChanged(double _newValue)
		{
			this.Value = _newValue;
			XUiC_PopupMenuItem.Entry.MenuItemValueChangedDelegate valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this, this.Value);
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x00292394 File Offset: 0x00290594
		public Entry(string _text, string _iconName, bool _isEnabled = true, object _tag = null, XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate _handler = null)
		{
			this.Text = _text;
			this.IconName = _iconName;
			this.IsEnabled = _isEnabled;
			this.Tag = _tag;
			this.EntryType = XUiC_PopupMenuItem.EEntryType.Button;
			if (_handler != null)
			{
				this.ItemClicked += _handler;
			}
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x002923CC File Offset: 0x002905CC
		public Entry(string _text, string _iconName, double _initialValue, bool _isEnabled = true, object _tag = null, XUiC_PopupMenuItem.Entry.MenuItemValueChangedDelegate _handler = null)
		{
			this.Text = _text;
			this.IconName = _iconName;
			this.IsEnabled = _isEnabled;
			this.Tag = _tag;
			this.EntryType = XUiC_PopupMenuItem.EEntryType.Slider;
			this.Value = _initialValue;
			if (_handler != null)
			{
				this.ValueChanged += _handler;
			}
		}

		// Token: 0x04005007 RID: 20487
		public readonly string Text;

		// Token: 0x04005008 RID: 20488
		public readonly string IconName;

		// Token: 0x04005009 RID: 20489
		public readonly bool IsEnabled;

		// Token: 0x0400500A RID: 20490
		public double Value;

		// Token: 0x0400500C RID: 20492
		public readonly XUiC_PopupMenuItem.EEntryType EntryType;

		// Token: 0x02000D5D RID: 3421
		// (Invoke) Token: 0x060067BD RID: 26557
		public delegate void MenuItemClickedDelegate(XUiC_PopupMenuItem.Entry _entry);

		// Token: 0x02000D5E RID: 3422
		// (Invoke) Token: 0x060067C1 RID: 26561
		public delegate void MenuItemValueChangedDelegate(XUiC_PopupMenuItem.Entry _entry, double _newValue);
	}
}

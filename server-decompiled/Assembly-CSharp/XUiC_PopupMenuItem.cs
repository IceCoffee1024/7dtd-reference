using System;
using UnityEngine.Scripting;

// Token: 0x02000D4A RID: 3402
[Preserve]
public class XUiC_PopupMenuItem : XUiController
{
	// Token: 0x17000B00 RID: 2816
	// (get) Token: 0x060067AC RID: 26540 RVA: 0x0028EB63 File Offset: 0x0028CD63
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

	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x060067AD RID: 26541 RVA: 0x0028EB80 File Offset: 0x0028CD80
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

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x060067AE RID: 26542 RVA: 0x0028EB9D File Offset: 0x0028CD9D
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

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x060067AF RID: 26543 RVA: 0x0028EBB0 File Offset: 0x0028CDB0
	[XuiXmlBinding("hovered")]
	public bool EntryHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x060067B0 RID: 26544 RVA: 0x0028EBB8 File Offset: 0x0028CDB8
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

	// Token: 0x060067B1 RID: 26545 RVA: 0x0028EBCB File Offset: 0x0028CDCB
	public override void Init()
	{
		base.Init();
		this.label.Overflow = UILabel.Overflow.ResizeFreely;
	}

	// Token: 0x060067B2 RID: 26546 RVA: 0x0028EBDF File Offset: 0x0028CDDF
	[XuiBindEvent("OnHoveredStateChanged", "slider")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void sliderHoveredChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart)
	{
		this.OnHovered(this, _isOverAnyPart);
	}

	// Token: 0x060067B3 RID: 26547 RVA: 0x0028EBE9 File Offset: 0x0028CDE9
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHovered(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
		this.parentPopup.IsOver = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x060067B4 RID: 26548 RVA: 0x0028EC04 File Offset: 0x0028CE04
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

	// Token: 0x060067B5 RID: 26549 RVA: 0x0028EC3E File Offset: 0x0028CE3E
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

	// Token: 0x060067B6 RID: 26550 RVA: 0x0028EC74 File Offset: 0x0028CE74
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

	// Token: 0x04004FC0 RID: 20416
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04004FC1 RID: 20417
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenu parentPopup;

	// Token: 0x04004FC2 RID: 20418
	[XuiBindComponent("lblText", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04004FC3 RID: 20419
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat slider;

	// Token: 0x04004FC4 RID: 20420
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupMenuItem.Entry itemEntry;

	// Token: 0x02000D4B RID: 3403
	public enum EEntryType
	{
		// Token: 0x04004FC6 RID: 20422
		Button,
		// Token: 0x04004FC7 RID: 20423
		Slider
	}

	// Token: 0x02000D4C RID: 3404
	public class Entry
	{
		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060067B8 RID: 26552 RVA: 0x0028ED08 File Offset: 0x0028CF08
		// (set) Token: 0x060067B9 RID: 26553 RVA: 0x0028ED10 File Offset: 0x0028CF10
		public object Tag { get; set; }

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060067BA RID: 26554 RVA: 0x0028ED1C File Offset: 0x0028CF1C
		// (remove) Token: 0x060067BB RID: 26555 RVA: 0x0028ED54 File Offset: 0x0028CF54
		public event XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate ItemClicked;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060067BC RID: 26556 RVA: 0x0028ED8C File Offset: 0x0028CF8C
		// (remove) Token: 0x060067BD RID: 26557 RVA: 0x0028EDC4 File Offset: 0x0028CFC4
		public event XUiC_PopupMenuItem.Entry.MenuItemValueChangedDelegate ValueChanged;

		// Token: 0x060067BE RID: 26558 RVA: 0x0028EDF9 File Offset: 0x0028CFF9
		public void HandleItemClicked()
		{
			XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate itemClicked = this.ItemClicked;
			if (itemClicked == null)
			{
				return;
			}
			itemClicked(this);
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x0028EE0C File Offset: 0x0028D00C
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

		// Token: 0x060067C0 RID: 26560 RVA: 0x0028EE2C File Offset: 0x0028D02C
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

		// Token: 0x060067C1 RID: 26561 RVA: 0x0028EE64 File Offset: 0x0028D064
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

		// Token: 0x04004FC8 RID: 20424
		public readonly string Text;

		// Token: 0x04004FC9 RID: 20425
		public readonly string IconName;

		// Token: 0x04004FCA RID: 20426
		public readonly bool IsEnabled;

		// Token: 0x04004FCB RID: 20427
		public double Value;

		// Token: 0x04004FCD RID: 20429
		public readonly XUiC_PopupMenuItem.EEntryType EntryType;

		// Token: 0x02000D4D RID: 3405
		// (Invoke) Token: 0x060067C3 RID: 26563
		public delegate void MenuItemClickedDelegate(XUiC_PopupMenuItem.Entry _entry);

		// Token: 0x02000D4E RID: 3406
		// (Invoke) Token: 0x060067C7 RID: 26567
		public delegate void MenuItemValueChangedDelegate(XUiC_PopupMenuItem.Entry _entry, double _newValue);
	}
}

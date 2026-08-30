using System;
using UnityEngine.Scripting;

// Token: 0x02000D38 RID: 3384
[Preserve]
public class XUiC_PopupMenuItem : XUiController
{
	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x06006720 RID: 26400 RVA: 0x0028BC7F File Offset: 0x00289E7F
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

	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x06006721 RID: 26401 RVA: 0x0028BC9C File Offset: 0x00289E9C
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

	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x06006722 RID: 26402 RVA: 0x0028BCB9 File Offset: 0x00289EB9
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

	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x06006723 RID: 26403 RVA: 0x0028BCCC File Offset: 0x00289ECC
	[XuiXmlBinding("hovered")]
	public bool EntryHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x06006724 RID: 26404 RVA: 0x0028BCD4 File Offset: 0x00289ED4
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

	// Token: 0x06006725 RID: 26405 RVA: 0x0028BCE7 File Offset: 0x00289EE7
	public override void Init()
	{
		base.Init();
		this.label.Overflow = UILabel.Overflow.ResizeFreely;
	}

	// Token: 0x06006726 RID: 26406 RVA: 0x0028BCFB File Offset: 0x00289EFB
	[XuiBindEvent("OnHoveredStateChanged", "slider")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void sliderHoveredChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart)
	{
		this.OnHovered(this, _isOverAnyPart);
	}

	// Token: 0x06006727 RID: 26407 RVA: 0x0028BD05 File Offset: 0x00289F05
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHovered(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
		this.parentPopup.IsOver = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x06006728 RID: 26408 RVA: 0x0028BD20 File Offset: 0x00289F20
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

	// Token: 0x06006729 RID: 26409 RVA: 0x0028BD5A File Offset: 0x00289F5A
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

	// Token: 0x0600672A RID: 26410 RVA: 0x0028BD90 File Offset: 0x00289F90
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

	// Token: 0x04004F22 RID: 20258
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04004F23 RID: 20259
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenu parentPopup;

	// Token: 0x04004F24 RID: 20260
	[XuiBindComponent("lblText", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04004F25 RID: 20261
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat slider;

	// Token: 0x04004F26 RID: 20262
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupMenuItem.Entry itemEntry;

	// Token: 0x02000D39 RID: 3385
	public enum EEntryType
	{
		// Token: 0x04004F28 RID: 20264
		Button,
		// Token: 0x04004F29 RID: 20265
		Slider
	}

	// Token: 0x02000D3A RID: 3386
	public class Entry
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x0600672C RID: 26412 RVA: 0x0028BE24 File Offset: 0x0028A024
		// (set) Token: 0x0600672D RID: 26413 RVA: 0x0028BE2C File Offset: 0x0028A02C
		public object Tag { get; set; }

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x0600672E RID: 26414 RVA: 0x0028BE38 File Offset: 0x0028A038
		// (remove) Token: 0x0600672F RID: 26415 RVA: 0x0028BE70 File Offset: 0x0028A070
		public event XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate ItemClicked;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06006730 RID: 26416 RVA: 0x0028BEA8 File Offset: 0x0028A0A8
		// (remove) Token: 0x06006731 RID: 26417 RVA: 0x0028BEE0 File Offset: 0x0028A0E0
		public event XUiC_PopupMenuItem.Entry.MenuItemValueChangedDelegate ValueChanged;

		// Token: 0x06006732 RID: 26418 RVA: 0x0028BF15 File Offset: 0x0028A115
		public void HandleItemClicked()
		{
			XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate itemClicked = this.ItemClicked;
			if (itemClicked == null)
			{
				return;
			}
			itemClicked(this);
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x0028BF28 File Offset: 0x0028A128
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

		// Token: 0x06006734 RID: 26420 RVA: 0x0028BF48 File Offset: 0x0028A148
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

		// Token: 0x06006735 RID: 26421 RVA: 0x0028BF80 File Offset: 0x0028A180
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

		// Token: 0x04004F2A RID: 20266
		public readonly string Text;

		// Token: 0x04004F2B RID: 20267
		public readonly string IconName;

		// Token: 0x04004F2C RID: 20268
		public readonly bool IsEnabled;

		// Token: 0x04004F2D RID: 20269
		public double Value;

		// Token: 0x04004F2F RID: 20271
		public readonly XUiC_PopupMenuItem.EEntryType EntryType;

		// Token: 0x02000D3B RID: 3387
		// (Invoke) Token: 0x06006737 RID: 26423
		public delegate void MenuItemClickedDelegate(XUiC_PopupMenuItem.Entry _entry);

		// Token: 0x02000D3C RID: 3388
		// (Invoke) Token: 0x0600673B RID: 26427
		public delegate void MenuItemValueChangedDelegate(XUiC_PopupMenuItem.Entry _entry, double _newValue);
	}
}

using System;
using UnityEngine.Scripting;

// Token: 0x02000D65 RID: 3429
[Preserve]
public class XUiC_PopupMenuItem : XUiController
{
	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x060067CB RID: 26571 RVA: 0x002935F3 File Offset: 0x002917F3
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

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x060067CC RID: 26572 RVA: 0x00293610 File Offset: 0x00291810
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

	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x060067CD RID: 26573 RVA: 0x0029362D File Offset: 0x0029182D
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

	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x060067CE RID: 26574 RVA: 0x00293640 File Offset: 0x00291840
	[XuiXmlBinding("hovered")]
	public bool EntryHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x060067CF RID: 26575 RVA: 0x00293648 File Offset: 0x00291848
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

	// Token: 0x060067D0 RID: 26576 RVA: 0x0029365B File Offset: 0x0029185B
	public override void Init()
	{
		base.Init();
		this.label.Overflow = UILabel.Overflow.ResizeFreely;
	}

	// Token: 0x060067D1 RID: 26577 RVA: 0x0029366F File Offset: 0x0029186F
	[XuiBindEvent("OnHoveredStateChanged", "slider")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void sliderHoveredChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart)
	{
		this.OnHovered(this, _isOverAnyPart);
	}

	// Token: 0x060067D2 RID: 26578 RVA: 0x00293679 File Offset: 0x00291879
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHovered(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
		this.parentPopup.IsOver = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x060067D3 RID: 26579 RVA: 0x00293694 File Offset: 0x00291894
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

	// Token: 0x060067D4 RID: 26580 RVA: 0x002936CE File Offset: 0x002918CE
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

	// Token: 0x060067D5 RID: 26581 RVA: 0x00293704 File Offset: 0x00291904
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

	// Token: 0x04005024 RID: 20516
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005025 RID: 20517
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenu parentPopup;

	// Token: 0x04005026 RID: 20518
	[XuiBindComponent("lblText", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04005027 RID: 20519
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat slider;

	// Token: 0x04005028 RID: 20520
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupMenuItem.Entry itemEntry;

	// Token: 0x02000D66 RID: 3430
	public enum EEntryType
	{
		// Token: 0x0400502A RID: 20522
		Button,
		// Token: 0x0400502B RID: 20523
		Slider
	}

	// Token: 0x02000D67 RID: 3431
	public class Entry
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060067D7 RID: 26583 RVA: 0x00293798 File Offset: 0x00291998
		// (set) Token: 0x060067D8 RID: 26584 RVA: 0x002937A0 File Offset: 0x002919A0
		public object Tag { get; set; }

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060067D9 RID: 26585 RVA: 0x002937AC File Offset: 0x002919AC
		// (remove) Token: 0x060067DA RID: 26586 RVA: 0x002937E4 File Offset: 0x002919E4
		public event XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate ItemClicked;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060067DB RID: 26587 RVA: 0x0029381C File Offset: 0x00291A1C
		// (remove) Token: 0x060067DC RID: 26588 RVA: 0x00293854 File Offset: 0x00291A54
		public event XUiC_PopupMenuItem.Entry.MenuItemValueChangedDelegate ValueChanged;

		// Token: 0x060067DD RID: 26589 RVA: 0x00293889 File Offset: 0x00291A89
		public void HandleItemClicked()
		{
			XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate itemClicked = this.ItemClicked;
			if (itemClicked == null)
			{
				return;
			}
			itemClicked(this);
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x0029389C File Offset: 0x00291A9C
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

		// Token: 0x060067DF RID: 26591 RVA: 0x002938BC File Offset: 0x00291ABC
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

		// Token: 0x060067E0 RID: 26592 RVA: 0x002938F4 File Offset: 0x00291AF4
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

		// Token: 0x0400502C RID: 20524
		public readonly string Text;

		// Token: 0x0400502D RID: 20525
		public readonly string IconName;

		// Token: 0x0400502E RID: 20526
		public readonly bool IsEnabled;

		// Token: 0x0400502F RID: 20527
		public double Value;

		// Token: 0x04005031 RID: 20529
		public readonly XUiC_PopupMenuItem.EEntryType EntryType;

		// Token: 0x02000D68 RID: 3432
		// (Invoke) Token: 0x060067E2 RID: 26594
		public delegate void MenuItemClickedDelegate(XUiC_PopupMenuItem.Entry _entry);

		// Token: 0x02000D69 RID: 3433
		// (Invoke) Token: 0x060067E6 RID: 26598
		public delegate void MenuItemValueChangedDelegate(XUiC_PopupMenuItem.Entry _entry, double _newValue);
	}
}

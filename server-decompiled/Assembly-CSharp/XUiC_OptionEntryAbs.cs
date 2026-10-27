using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000DAD RID: 3501
[Preserve]
public abstract class XUiC_OptionEntryAbs : XUiController
{
	// Token: 0x14000092 RID: 146
	// (add) Token: 0x060069C0 RID: 27072 RVA: 0x0029D8F8 File Offset: 0x0029BAF8
	// (remove) Token: 0x060069C1 RID: 27073 RVA: 0x0029D930 File Offset: 0x0029BB30
	public event Action<XUiC_OptionEntryAbs> ValueChanged;

	// Token: 0x060069C2 RID: 27074 RVA: 0x0029D965 File Offset: 0x0029BB65
	[PublicizedFrom(EAccessModifier.Protected)]
	public void invokeValueChanged()
	{
		Action<XUiC_OptionEntryAbs> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(this);
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x060069C3 RID: 27075 RVA: 0x0029D978 File Offset: 0x0029BB78
	// (set) Token: 0x060069C4 RID: 27076 RVA: 0x0029D980 File Offset: 0x0029BB80
	[XuiXmlAttribute("prefname", false)]
	public string PrefName
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x060069C5 RID: 27077 RVA: 0x0029D989 File Offset: 0x0029BB89
	// (set) Token: 0x060069C6 RID: 27078 RVA: 0x0029D991 File Offset: 0x0029BB91
	[XuiXmlAttribute("apply_immediately", false)]
	public bool ApplyImmediately
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x060069C7 RID: 27079 RVA: 0x0029D99A File Offset: 0x0029BB9A
	// (set) Token: 0x060069C8 RID: 27080 RVA: 0x0029D9A2 File Offset: 0x0029BBA2
	[XuiXmlAttribute("apply_defaults", false)]
	public bool ApplyDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x060069C9 RID: 27081
	[XuiXmlBinding("option_changed")]
	public abstract bool IsChanged { get; }

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x060069CA RID: 27082
	[XuiXmlBinding("option_is_default")]
	public abstract bool IsDefault { get; }

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x060069CB RID: 27083 RVA: 0x0029D9AB File Offset: 0x0029BBAB
	// (set) Token: 0x060069CC RID: 27084 RVA: 0x0029D9B3 File Offset: 0x0029BBB3
	[XuiXmlBinding("option_hovered")]
	public bool OptionHovered
	{
		get
		{
			return this.optionHovered;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.optionHovered == value)
			{
				return;
			}
			this.optionHovered = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x060069CD RID: 27085
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void initCurrentValue();

	// Token: 0x060069CE RID: 27086
	public abstract void DiscardCurrentChange();

	// Token: 0x060069CF RID: 27087
	public abstract void ApplySelection();

	// Token: 0x060069D0 RID: 27088
	public abstract void ResetToDefault();

	// Token: 0x060069D1 RID: 27089 RVA: 0x0029D9CD File Offset: 0x0029BBCD
	public override void OnOpen()
	{
		base.OnOpen();
		this.initHovering();
		this.initCurrentValue();
	}

	// Token: 0x060069D2 RID: 27090 RVA: 0x0029D9E1 File Offset: 0x0029BBE1
	public override void OnClose()
	{
		base.OnClose();
		this.DiscardCurrentChange();
	}

	// Token: 0x060069D3 RID: 27091 RVA: 0x0029D9EF File Offset: 0x0029BBEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void initHovering()
	{
		if (this.hoveringInitialized)
		{
			return;
		}
		this.hoveringInitialized = true;
		this.<initHovering>g__ApplyEventToChildren|33_0(this);
	}

	// Token: 0x060069D4 RID: 27092 RVA: 0x0029DA08 File Offset: 0x0029BC08
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060069D5 RID: 27093 RVA: 0x0029DA26 File Offset: 0x0029BC26
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHoveredElementChanged(XUiController _sender, bool _state)
	{
		this.OptionHovered = _state;
		this.parentOptionsDialog.HoveredOption = (_state ? this : null);
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x000027FC File Offset: 0x000009FC
	public static void DebugLog(string _s)
	{
	}

	// Token: 0x060069D7 RID: 27095 RVA: 0x00089935 File Offset: 0x00087B35
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryAbs()
	{
	}

	// Token: 0x060069D8 RID: 27096 RVA: 0x0029DA44 File Offset: 0x0029BC44
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <initHovering>g__ApplyEventToChildren|33_0(XUiController _controller)
	{
		if (_controller.ViewComponent.HasAnyEvent)
		{
			_controller.ViewComponent.EventOnHover = true;
			_controller.OnHover += this.OnHoveredElementChanged;
		}
		foreach (XUiController xuiController in _controller.Children)
		{
			this.<initHovering>g__ApplyEventToChildren|33_0(xuiController);
		}
	}

	// Token: 0x04005172 RID: 20850
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionsDialogBase parentOptionsDialog;

	// Token: 0x04005173 RID: 20851
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_ComboBoxBase comboGeneric;

	// Token: 0x04005177 RID: 20855
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionHovered;

	// Token: 0x04005178 RID: 20856
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoveringInitialized;
}

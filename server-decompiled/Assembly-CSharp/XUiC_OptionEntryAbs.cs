using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000D7F RID: 3455
[Preserve]
public abstract class XUiC_OptionEntryAbs : XUiController
{
	// Token: 0x14000092 RID: 146
	// (add) Token: 0x06006907 RID: 26887 RVA: 0x00295EF0 File Offset: 0x002940F0
	// (remove) Token: 0x06006908 RID: 26888 RVA: 0x00295F28 File Offset: 0x00294128
	public event Action<XUiC_OptionEntryAbs> ValueChanged;

	// Token: 0x06006909 RID: 26889 RVA: 0x00295F5D File Offset: 0x0029415D
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

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x0600690A RID: 26890 RVA: 0x00295F70 File Offset: 0x00294170
	// (set) Token: 0x0600690B RID: 26891 RVA: 0x00295F78 File Offset: 0x00294178
	[XuiXmlAttribute("prefname", false)]
	public string PrefName
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x0600690C RID: 26892 RVA: 0x00295F81 File Offset: 0x00294181
	// (set) Token: 0x0600690D RID: 26893 RVA: 0x00295F89 File Offset: 0x00294189
	[XuiXmlAttribute("apply_immediately", false)]
	public bool ApplyImmediately
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x0600690E RID: 26894 RVA: 0x00295F92 File Offset: 0x00294192
	// (set) Token: 0x0600690F RID: 26895 RVA: 0x00295F9A File Offset: 0x0029419A
	[XuiXmlAttribute("apply_defaults", false)]
	public bool ApplyDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x06006910 RID: 26896
	[XuiXmlBinding("option_changed")]
	public abstract bool IsChanged { get; }

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x06006911 RID: 26897
	[XuiXmlBinding("option_is_default")]
	public abstract bool IsDefault { get; }

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x06006912 RID: 26898 RVA: 0x00295FA3 File Offset: 0x002941A3
	// (set) Token: 0x06006913 RID: 26899 RVA: 0x00295FAB File Offset: 0x002941AB
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

	// Token: 0x06006914 RID: 26900
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void initCurrentValue();

	// Token: 0x06006915 RID: 26901
	public abstract void DiscardCurrentChange();

	// Token: 0x06006916 RID: 26902
	public abstract void ApplySelection();

	// Token: 0x06006917 RID: 26903
	public abstract void ResetToDefault();

	// Token: 0x06006918 RID: 26904 RVA: 0x00295FC5 File Offset: 0x002941C5
	public override void OnOpen()
	{
		base.OnOpen();
		this.initHovering();
		this.initCurrentValue();
	}

	// Token: 0x06006919 RID: 26905 RVA: 0x00295FD9 File Offset: 0x002941D9
	public override void OnClose()
	{
		base.OnClose();
		this.DiscardCurrentChange();
	}

	// Token: 0x0600691A RID: 26906 RVA: 0x00295FE7 File Offset: 0x002941E7
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

	// Token: 0x0600691B RID: 26907 RVA: 0x00296000 File Offset: 0x00294200
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x0600691C RID: 26908 RVA: 0x0029601E File Offset: 0x0029421E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHoveredElementChanged(XUiController _sender, bool _state)
	{
		this.OptionHovered = _state;
		this.parentOptionsDialog.HoveredOption = (_state ? this : null);
	}

	// Token: 0x0600691D RID: 26909 RVA: 0x000027FC File Offset: 0x000009FC
	public static void DebugLog(string _s)
	{
	}

	// Token: 0x0600691E RID: 26910 RVA: 0x00088DE1 File Offset: 0x00086FE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryAbs()
	{
	}

	// Token: 0x0600691F RID: 26911 RVA: 0x0029603C File Offset: 0x0029423C
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

	// Token: 0x04005074 RID: 20596
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionsDialogBase parentOptionsDialog;

	// Token: 0x04005075 RID: 20597
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_ComboBoxBase comboGeneric;

	// Token: 0x04005079 RID: 20601
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionHovered;

	// Token: 0x0400507A RID: 20602
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoveringInitialized;
}

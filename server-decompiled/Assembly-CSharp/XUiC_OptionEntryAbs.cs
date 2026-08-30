using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000D93 RID: 3475
[Preserve]
public abstract class XUiC_OptionEntryAbs : XUiController
{
	// Token: 0x14000093 RID: 147
	// (add) Token: 0x060069A3 RID: 27043 RVA: 0x00299044 File Offset: 0x00297244
	// (remove) Token: 0x060069A4 RID: 27044 RVA: 0x0029907C File Offset: 0x0029727C
	public event Action<XUiC_OptionEntryAbs> ValueChanged;

	// Token: 0x060069A5 RID: 27045 RVA: 0x002990B1 File Offset: 0x002972B1
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

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x060069A6 RID: 27046 RVA: 0x002990C4 File Offset: 0x002972C4
	// (set) Token: 0x060069A7 RID: 27047 RVA: 0x002990CC File Offset: 0x002972CC
	[XuiXmlAttribute("prefname", false)]
	public string PrefName
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x060069A8 RID: 27048 RVA: 0x002990D5 File Offset: 0x002972D5
	// (set) Token: 0x060069A9 RID: 27049 RVA: 0x002990DD File Offset: 0x002972DD
	[XuiXmlAttribute("apply_immediately", false)]
	public bool ApplyImmediately
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x060069AA RID: 27050 RVA: 0x002990E6 File Offset: 0x002972E6
	// (set) Token: 0x060069AB RID: 27051 RVA: 0x002990EE File Offset: 0x002972EE
	[XuiXmlAttribute("apply_defaults", false)]
	public bool ApplyDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x060069AC RID: 27052
	[XuiXmlBinding("option_changed")]
	public abstract bool IsChanged { get; }

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x060069AD RID: 27053
	[XuiXmlBinding("option_is_default")]
	public abstract bool IsDefault { get; }

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x060069AE RID: 27054 RVA: 0x002990F7 File Offset: 0x002972F7
	// (set) Token: 0x060069AF RID: 27055 RVA: 0x002990FF File Offset: 0x002972FF
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

	// Token: 0x060069B0 RID: 27056
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void initCurrentValue();

	// Token: 0x060069B1 RID: 27057
	public abstract void DiscardCurrentChange();

	// Token: 0x060069B2 RID: 27058
	public abstract void ApplySelection();

	// Token: 0x060069B3 RID: 27059
	public abstract void ResetToDefault();

	// Token: 0x060069B4 RID: 27060 RVA: 0x00299119 File Offset: 0x00297319
	public override void OnOpen()
	{
		base.OnOpen();
		this.initHovering();
		this.initCurrentValue();
	}

	// Token: 0x060069B5 RID: 27061 RVA: 0x0029912D File Offset: 0x0029732D
	public override void OnClose()
	{
		base.OnClose();
		this.DiscardCurrentChange();
	}

	// Token: 0x060069B6 RID: 27062 RVA: 0x0029913B File Offset: 0x0029733B
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

	// Token: 0x060069B7 RID: 27063 RVA: 0x00299154 File Offset: 0x00297354
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060069B8 RID: 27064 RVA: 0x00299172 File Offset: 0x00297372
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHoveredElementChanged(XUiController _sender, bool _state)
	{
		this.OptionHovered = _state;
		this.parentOptionsDialog.HoveredOption = (_state ? this : null);
	}

	// Token: 0x060069B9 RID: 27065 RVA: 0x000027FC File Offset: 0x000009FC
	public static void DebugLog(string _s)
	{
	}

	// Token: 0x060069BA RID: 27066 RVA: 0x00089481 File Offset: 0x00087681
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryAbs()
	{
	}

	// Token: 0x060069BB RID: 27067 RVA: 0x00299190 File Offset: 0x00297390
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

	// Token: 0x04005116 RID: 20758
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionsDialogBase parentOptionsDialog;

	// Token: 0x04005117 RID: 20759
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_ComboBoxBase comboGeneric;

	// Token: 0x0400511B RID: 20763
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionHovered;

	// Token: 0x0400511C RID: 20764
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoveringInitialized;
}

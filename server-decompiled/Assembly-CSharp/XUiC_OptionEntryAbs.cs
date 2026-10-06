using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000DA2 RID: 3490
[Preserve]
public abstract class XUiC_OptionEntryAbs : XUiController
{
	// Token: 0x14000092 RID: 146
	// (add) Token: 0x0600699B RID: 27035 RVA: 0x0029C3D0 File Offset: 0x0029A5D0
	// (remove) Token: 0x0600699C RID: 27036 RVA: 0x0029C408 File Offset: 0x0029A608
	public event Action<XUiC_OptionEntryAbs> ValueChanged;

	// Token: 0x0600699D RID: 27037 RVA: 0x0029C43D File Offset: 0x0029A63D
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

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x0600699E RID: 27038 RVA: 0x0029C450 File Offset: 0x0029A650
	// (set) Token: 0x0600699F RID: 27039 RVA: 0x0029C458 File Offset: 0x0029A658
	[XuiXmlAttribute("prefname", false)]
	public string PrefName
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x060069A0 RID: 27040 RVA: 0x0029C461 File Offset: 0x0029A661
	// (set) Token: 0x060069A1 RID: 27041 RVA: 0x0029C469 File Offset: 0x0029A669
	[XuiXmlAttribute("apply_immediately", false)]
	public bool ApplyImmediately
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x060069A2 RID: 27042 RVA: 0x0029C472 File Offset: 0x0029A672
	// (set) Token: 0x060069A3 RID: 27043 RVA: 0x0029C47A File Offset: 0x0029A67A
	[XuiXmlAttribute("apply_defaults", false)]
	public bool ApplyDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x060069A4 RID: 27044
	[XuiXmlBinding("option_changed")]
	public abstract bool IsChanged { get; }

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x060069A5 RID: 27045
	[XuiXmlBinding("option_is_default")]
	public abstract bool IsDefault { get; }

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x060069A6 RID: 27046 RVA: 0x0029C483 File Offset: 0x0029A683
	// (set) Token: 0x060069A7 RID: 27047 RVA: 0x0029C48B File Offset: 0x0029A68B
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

	// Token: 0x060069A8 RID: 27048
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void initCurrentValue();

	// Token: 0x060069A9 RID: 27049
	public abstract void DiscardCurrentChange();

	// Token: 0x060069AA RID: 27050
	public abstract void ApplySelection();

	// Token: 0x060069AB RID: 27051
	public abstract void ResetToDefault();

	// Token: 0x060069AC RID: 27052 RVA: 0x0029C4A5 File Offset: 0x0029A6A5
	public override void OnOpen()
	{
		base.OnOpen();
		this.initHovering();
		this.initCurrentValue();
	}

	// Token: 0x060069AD RID: 27053 RVA: 0x0029C4B9 File Offset: 0x0029A6B9
	public override void OnClose()
	{
		base.OnClose();
		this.DiscardCurrentChange();
	}

	// Token: 0x060069AE RID: 27054 RVA: 0x0029C4C7 File Offset: 0x0029A6C7
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

	// Token: 0x060069AF RID: 27055 RVA: 0x0029C4E0 File Offset: 0x0029A6E0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060069B0 RID: 27056 RVA: 0x0029C4FE File Offset: 0x0029A6FE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHoveredElementChanged(XUiController _sender, bool _state)
	{
		this.OptionHovered = _state;
		this.parentOptionsDialog.HoveredOption = (_state ? this : null);
	}

	// Token: 0x060069B1 RID: 27057 RVA: 0x000027FC File Offset: 0x000009FC
	public static void DebugLog(string _s)
	{
	}

	// Token: 0x060069B2 RID: 27058 RVA: 0x00089701 File Offset: 0x00087901
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryAbs()
	{
	}

	// Token: 0x060069B3 RID: 27059 RVA: 0x0029C51C File Offset: 0x0029A71C
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

	// Token: 0x0400514D RID: 20813
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionsDialogBase parentOptionsDialog;

	// Token: 0x0400514E RID: 20814
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_ComboBoxBase comboGeneric;

	// Token: 0x04005152 RID: 20818
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionHovered;

	// Token: 0x04005153 RID: 20819
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoveringInitialized;
}

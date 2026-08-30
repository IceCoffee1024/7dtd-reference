using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E6C RID: 3692
[Preserve]
public class XUiC_Button : XUiController
{
	// Token: 0x060071E0 RID: 29152 RVA: 0x002B9FDC File Offset: 0x002B81DC
	public override void Init()
	{
		base.Init();
		if (!base.ViewComponent.HasAnyEvent)
		{
			throw new Exception("[XUi] Button requires the view to listen to events. Hierarchy: " + base.GetXuiHierarchy());
		}
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
	}

	// Token: 0x060071E1 RID: 29153 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071E2 RID: 29154 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071E3 RID: 29155 RVA: 0x002BA029 File Offset: 0x002B8229
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.HoldToActivate && this.isPressed)
		{
			this.HoldToActivateElapsed += Time.deltaTime;
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060071E4 RID: 29156 RVA: 0x002BA05B File Offset: 0x002B825B
	public override void Pressed(int _mouseButton)
	{
		if (this.HoldToActivate)
		{
			return;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x060071E5 RID: 29157 RVA: 0x002BA06D File Offset: 0x002B826D
	[XuiBindEvent("OnVisiblity", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			return;
		}
		this.isPressed = false;
		this.HoldToActivateElapsed = 0f;
	}

	// Token: 0x060071E6 RID: 29158 RVA: 0x002BA085 File Offset: 0x002B8285
	[XuiBindEvent("OnMouseUpDown", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onMouse(XUiController _sender, bool _mouseDown)
	{
		if (!this.HoldToActivate)
		{
			return;
		}
		this.isPressed = _mouseDown;
		if (_mouseDown)
		{
			return;
		}
		if (this.HoldToActivateState >= 1f)
		{
			base.ViewComponent.PlayClickSound();
			base.Pressed(-1);
		}
		this.HoldToActivateElapsed = 0f;
	}

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x060071E7 RID: 29159 RVA: 0x002BA0C5 File Offset: 0x002B82C5
	public bool HoldToActivate
	{
		get
		{
			return this.holdToActivateTime > 0f;
		}
	}

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x060071E8 RID: 29160 RVA: 0x002BA0D4 File Offset: 0x002B82D4
	// (set) Token: 0x060071E9 RID: 29161 RVA: 0x002BA0DC File Offset: 0x002B82DC
	[XuiXmlAttribute("hold_to_activate_time", false)]
	public float HoldToActivateTime
	{
		get
		{
			return this.holdToActivateTime;
		}
		set
		{
			if (Mathf.Approximately(this.holdToActivateTime, value))
			{
				return;
			}
			this.holdToActivateTime = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CF7 RID: 3319
	// (get) Token: 0x060071EA RID: 29162 RVA: 0x002BA0FB File Offset: 0x002B82FB
	// (set) Token: 0x060071EB RID: 29163 RVA: 0x002BA103 File Offset: 0x002B8303
	public float HoldToActivateElapsed
	{
		get
		{
			return this.holdToActivateElapsed;
		}
		set
		{
			if (Mathf.Approximately(this.holdToActivateElapsed, value))
			{
				return;
			}
			this.holdToActivateElapsed = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CF8 RID: 3320
	// (get) Token: 0x060071EC RID: 29164 RVA: 0x002BA122 File Offset: 0x002B8322
	[XuiXmlBinding("hold_to_activate_state")]
	public float HoldToActivateState
	{
		get
		{
			if (!this.HoldToActivate)
			{
				return 0f;
			}
			return Mathf.Clamp01(this.holdToActivateElapsed / this.holdToActivateTime);
		}
	}

	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x060071ED RID: 29165 RVA: 0x002BA144 File Offset: 0x002B8344
	[XuiXmlBinding("btn_enabled")]
	public bool BindingEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.Enabled;
		}
	}

	// Token: 0x17000CFA RID: 3322
	// (get) Token: 0x060071EE RID: 29166 RVA: 0x002BA151 File Offset: 0x002B8351
	[XuiXmlBinding("btn_hovered")]
	public bool BindingHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.IsHovered;
		}
	}

	// Token: 0x17000CFB RID: 3323
	// (get) Token: 0x060071EF RID: 29167 RVA: 0x002BA15E File Offset: 0x002B835E
	[XuiXmlBinding("btn_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x040055CF RID: 21967
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x040055D0 RID: 21968
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateTime;

	// Token: 0x040055D1 RID: 21969
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateElapsed;
}

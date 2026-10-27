using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E86 RID: 3718
[Preserve]
public class XUiC_Button : XUiController
{
	// Token: 0x060071FD RID: 29181 RVA: 0x002BE8F8 File Offset: 0x002BCAF8
	public override void Init()
	{
		base.Init();
		if (!base.ViewComponent.HasAnyEvent)
		{
			throw new Exception("[XUi] Button requires the view to listen to events. Hierarchy: " + base.GetXuiHierarchy());
		}
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
	}

	// Token: 0x060071FE RID: 29182 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071FF RID: 29183 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007200 RID: 29184 RVA: 0x002BE945 File Offset: 0x002BCB45
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.HoldToActivate && this.isPressed)
		{
			this.HoldToActivateElapsed += Time.deltaTime;
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007201 RID: 29185 RVA: 0x002BE977 File Offset: 0x002BCB77
	public override void Pressed(int _mouseButton)
	{
		if (this.HoldToActivate)
		{
			return;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x06007202 RID: 29186 RVA: 0x002BE989 File Offset: 0x002BCB89
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

	// Token: 0x06007203 RID: 29187 RVA: 0x002BE9A1 File Offset: 0x002BCBA1
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

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x06007204 RID: 29188 RVA: 0x002BE9E1 File Offset: 0x002BCBE1
	public bool HoldToActivate
	{
		get
		{
			return this.holdToActivateTime > 0f;
		}
	}

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x06007205 RID: 29189 RVA: 0x002BE9F0 File Offset: 0x002BCBF0
	// (set) Token: 0x06007206 RID: 29190 RVA: 0x002BE9F8 File Offset: 0x002BCBF8
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

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x06007207 RID: 29191 RVA: 0x002BEA17 File Offset: 0x002BCC17
	// (set) Token: 0x06007208 RID: 29192 RVA: 0x002BEA1F File Offset: 0x002BCC1F
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

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x06007209 RID: 29193 RVA: 0x002BEA3E File Offset: 0x002BCC3E
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

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x0600720A RID: 29194 RVA: 0x002BEA60 File Offset: 0x002BCC60
	[XuiXmlBinding("btn_enabled")]
	public bool BindingEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.Enabled;
		}
	}

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x0600720B RID: 29195 RVA: 0x002BEA6D File Offset: 0x002BCC6D
	[XuiXmlBinding("btn_hovered")]
	public bool BindingHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.IsHovered;
		}
	}

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x0600720C RID: 29196 RVA: 0x002BEA7A File Offset: 0x002BCC7A
	[XuiXmlBinding("btn_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x0400562B RID: 22059
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x0400562C RID: 22060
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateTime;

	// Token: 0x0400562D RID: 22061
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateElapsed;
}

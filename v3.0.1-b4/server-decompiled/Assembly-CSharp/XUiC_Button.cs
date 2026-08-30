using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E57 RID: 3671
[Preserve]
public class XUiC_Button : XUiController
{
	// Token: 0x06007140 RID: 28992 RVA: 0x002B6AB8 File Offset: 0x002B4CB8
	public override void Init()
	{
		base.Init();
		if (!base.ViewComponent.HasAnyEvent)
		{
			throw new Exception("[XUi] Button requires the view to listen to events. Hierarchy: " + base.GetXuiHierarchy());
		}
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
	}

	// Token: 0x06007141 RID: 28993 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007142 RID: 28994 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007143 RID: 28995 RVA: 0x002B6B05 File Offset: 0x002B4D05
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.HoldToActivate && this.isPressed)
		{
			this.HoldToActivateElapsed += Time.deltaTime;
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007144 RID: 28996 RVA: 0x002B6B37 File Offset: 0x002B4D37
	public override void Pressed(int _mouseButton)
	{
		if (this.HoldToActivate)
		{
			return;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x06007145 RID: 28997 RVA: 0x002B6B49 File Offset: 0x002B4D49
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

	// Token: 0x06007146 RID: 28998 RVA: 0x002B6B61 File Offset: 0x002B4D61
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

	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x06007147 RID: 28999 RVA: 0x002B6BA1 File Offset: 0x002B4DA1
	public bool HoldToActivate
	{
		get
		{
			return this.holdToActivateTime > 0f;
		}
	}

	// Token: 0x17000CDD RID: 3293
	// (get) Token: 0x06007148 RID: 29000 RVA: 0x002B6BB0 File Offset: 0x002B4DB0
	// (set) Token: 0x06007149 RID: 29001 RVA: 0x002B6BB8 File Offset: 0x002B4DB8
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

	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x0600714A RID: 29002 RVA: 0x002B6BD7 File Offset: 0x002B4DD7
	// (set) Token: 0x0600714B RID: 29003 RVA: 0x002B6BDF File Offset: 0x002B4DDF
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

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x0600714C RID: 29004 RVA: 0x002B6BFE File Offset: 0x002B4DFE
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

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x0600714D RID: 29005 RVA: 0x002B6C20 File Offset: 0x002B4E20
	[XuiXmlBinding("btn_enabled")]
	public bool BindingEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.Enabled;
		}
	}

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x0600714E RID: 29006 RVA: 0x002B6C2D File Offset: 0x002B4E2D
	[XuiXmlBinding("btn_hovered")]
	public bool BindingHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.IsHovered;
		}
	}

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x0600714F RID: 29007 RVA: 0x002B6C3A File Offset: 0x002B4E3A
	[XuiXmlBinding("btn_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x0400552B RID: 21803
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x0400552C RID: 21804
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateTime;

	// Token: 0x0400552D RID: 21805
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateElapsed;
}

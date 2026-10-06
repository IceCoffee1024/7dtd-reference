using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E7B RID: 3707
[Preserve]
public class XUiC_Button : XUiController
{
	// Token: 0x060071D8 RID: 29144 RVA: 0x002BD38C File Offset: 0x002BB58C
	public override void Init()
	{
		base.Init();
		if (!base.ViewComponent.HasAnyEvent)
		{
			throw new Exception("[XUi] Button requires the view to listen to events. Hierarchy: " + base.GetXuiHierarchy());
		}
		this.CustomAttributes.EntryModified += this.OnCustomAttributeModified;
	}

	// Token: 0x060071D9 RID: 29145 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCustomAttributeModified(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071DA RID: 29146 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("OnInteraction", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnyEvent(XUiController _sender, EXUiControllerInteractionType _type)
	{
		this.IsDirty = true;
	}

	// Token: 0x060071DB RID: 29147 RVA: 0x002BD3D9 File Offset: 0x002BB5D9
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.HoldToActivate && this.isPressed)
		{
			this.HoldToActivateElapsed += Time.deltaTime;
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060071DC RID: 29148 RVA: 0x002BD40B File Offset: 0x002BB60B
	public override void Pressed(int _mouseButton)
	{
		if (this.HoldToActivate)
		{
			return;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x060071DD RID: 29149 RVA: 0x002BD41D File Offset: 0x002BB61D
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

	// Token: 0x060071DE RID: 29150 RVA: 0x002BD435 File Offset: 0x002BB635
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

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x060071DF RID: 29151 RVA: 0x002BD475 File Offset: 0x002BB675
	public bool HoldToActivate
	{
		get
		{
			return this.holdToActivateTime > 0f;
		}
	}

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x060071E0 RID: 29152 RVA: 0x002BD484 File Offset: 0x002BB684
	// (set) Token: 0x060071E1 RID: 29153 RVA: 0x002BD48C File Offset: 0x002BB68C
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

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x060071E2 RID: 29154 RVA: 0x002BD4AB File Offset: 0x002BB6AB
	// (set) Token: 0x060071E3 RID: 29155 RVA: 0x002BD4B3 File Offset: 0x002BB6B3
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

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x060071E4 RID: 29156 RVA: 0x002BD4D2 File Offset: 0x002BB6D2
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

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x060071E5 RID: 29157 RVA: 0x002BD4F4 File Offset: 0x002BB6F4
	[XuiXmlBinding("btn_enabled")]
	public bool BindingEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.Enabled;
		}
	}

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x060071E6 RID: 29158 RVA: 0x002BD501 File Offset: 0x002BB701
	[XuiXmlBinding("btn_hovered")]
	public bool BindingHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.viewComponent.IsHovered;
		}
	}

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x060071E7 RID: 29159 RVA: 0x002BD50E File Offset: 0x002BB70E
	[XuiXmlBinding("btn_attributes")]
	public ObservableDictionary<string, object> BindingCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CustomAttributes;
		}
	}

	// Token: 0x04005606 RID: 22022
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x04005607 RID: 22023
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateTime;

	// Token: 0x04005608 RID: 22024
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdToActivateElapsed;
}

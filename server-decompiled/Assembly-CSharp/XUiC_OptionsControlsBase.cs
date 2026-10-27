using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DBF RID: 3519
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_OptionsControlsBase : XUiC_OptionsDialogBase
{
	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06006AB6 RID: 27318 RVA: 0x002A0354 File Offset: 0x0029E554
	// (remove) Token: 0x06006AB7 RID: 27319 RVA: 0x002A0388 File Offset: 0x0029E588
	public static event Action OnSettingsChanged;

	// Token: 0x06006AB8 RID: 27320 RVA: 0x002A03BC File Offset: 0x0029E5BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		XUiC_OptionsControlsBase.XUiC_BindingEntry[] childControllers = this.TabSelector.SelectedTab.GetChildControllers<XUiC_OptionsControlsBase.XUiC_BindingEntry>("", null);
		for (int i = 0; i < childControllers.Length; i++)
		{
			childControllers[i].ResetToDefault();
		}
	}

	// Token: 0x06006AB9 RID: 27321
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void createControlsEntries();

	// Token: 0x06006ABA RID: 27322 RVA: 0x002A03F6 File Offset: 0x0029E5F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		PlayerMoveController.UpdateControlsOptions();
		this.storeCurrentBindings();
		GameOptionsControls.Save();
		Action onSettingsChanged = XUiC_OptionsControlsBase.OnSettingsChanged;
		if (onSettingsChanged == null)
		{
			return;
		}
		onSettingsChanged();
	}

	// Token: 0x06006ABB RID: 27323 RVA: 0x002A0420 File Offset: 0x0029E620
	[PublicizedFrom(EAccessModifier.Protected)]
	public void storeCurrentBindings()
	{
		this.actionBindingsOnOpen.Clear();
		foreach (PlayerActionsBase playerActionsBase in PlatformManager.NativePlatform.Input.ActionSets)
		{
			this.actionBindingsOnOpen.Add(playerActionsBase.Save());
		}
	}

	// Token: 0x06006ABC RID: 27324 RVA: 0x002A048C File Offset: 0x0029E68C
	public override void OnOpen()
	{
		if (!this.initialized)
		{
			this.createControlsEntries();
			this.initialized = true;
		}
		ThreadManager.StartCoroutine(this.storeBindingsLater());
		base.OnOpen();
	}

	// Token: 0x06006ABD RID: 27325 RVA: 0x002A04B5 File Offset: 0x0029E6B5
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator storeBindingsLater()
	{
		yield return null;
		yield return null;
		this.storeCurrentBindings();
		yield break;
	}

	// Token: 0x06006ABE RID: 27326 RVA: 0x002A04C4 File Offset: 0x0029E6C4
	public override void OnClose()
	{
		base.OnClose();
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(this.actionBindingsOnOpen);
	}

	// Token: 0x06006ABF RID: 27327 RVA: 0x002A04E1 File Offset: 0x0029E6E1
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsControlsBase()
	{
	}

	// Token: 0x040051B5 RID: 20917
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool initialized;

	// Token: 0x040051B6 RID: 20918
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> actionBindingsOnOpen = new List<string>();

	// Token: 0x02000DC0 RID: 3520
	[global::UnityEngine.Scripting.Preserve]
	public class XUiC_BindingEntry : XUiController
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06006AC0 RID: 27328 RVA: 0x002A04F4 File Offset: 0x0029E6F4
		// (set) Token: 0x06006AC1 RID: 27329 RVA: 0x002A04FC File Offset: 0x0029E6FC
		public PlayerAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				if (this.action == value)
				{
					return;
				}
				if (this.action != null)
				{
					this.action.OnBindingsChanged -= this.actionBindingsChanged;
				}
				this.action = value;
				this.actionData = (PlayerActionData.ActionUserData)((value != null) ? value.UserData : null);
				if (this.action != null)
				{
					this.action.OnBindingsChanged += this.actionBindingsChanged;
				}
				this.IsDirty = true;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x002A0576 File Offset: 0x0029E776
		// (set) Token: 0x06006AC3 RID: 27331 RVA: 0x002A057E File Offset: 0x0029E77E
		[XuiXmlAttribute("for_controller", false)]
		public bool IsControllerBinding
		{
			get
			{
				return this.isControllerBinding;
			}
			set
			{
				if (value == this.isControllerBinding)
				{
					return;
				}
				this.isControllerBinding = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06006AC4 RID: 27332 RVA: 0x002A0598 File Offset: 0x0029E798
		[XuiXmlBinding("has_action")]
		public bool HasAction
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x002A05A3 File Offset: 0x0029E7A3
		[XuiXmlBinding("allow_rebind")]
		public bool AllowRebind
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return actionUserData != null && actionUserData.allowRebind;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06006AC6 RID: 27334 RVA: 0x002A05B6 File Offset: 0x0029E7B6
		[XuiXmlBinding("action_label")]
		public string ActionLabel
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedName : null) ?? "";
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06006AC7 RID: 27335 RVA: 0x002A05D3 File Offset: 0x0029E7D3
		[XuiXmlBinding("action_description")]
		public string ActionDescription
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedDescription : null) ?? "";
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06006AC8 RID: 27336 RVA: 0x002A05F0 File Offset: 0x0029E7F0
		[XuiXmlBinding("action_binding")]
		public string ActionBinding
		{
			get
			{
				return this.bindingString ?? "";
			}
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x002A0601 File Offset: 0x0029E801
		public override void Init()
		{
			base.Init();
			base.registerForInputStyleChanges();
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x002A060F File Offset: 0x0029E80F
		public override void OnOpen()
		{
			base.OnOpen();
			this.frameOpened = Time.frameCount;
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x002A0624 File Offset: 0x0029E824
		public override void Update(float _dt)
		{
			base.Update(_dt);
			if (this.IsDirty)
			{
				this.IsDirty = false;
				this.bindingString = this.action.GetBindingString(this.isControllerBinding, PlatformManager.NativePlatform.Input.CurrentControllerInputStyle, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null);
				base.RefreshBindings();
			}
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x002A0677 File Offset: 0x0029E877
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.action != null)
			{
				this.action.OnBindingsChanged -= this.actionBindingsChanged;
			}
		}

		// Token: 0x06006ACD RID: 27341 RVA: 0x002A069E File Offset: 0x0029E89E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
		{
			base.inputStyleChanged(_oldStyle, _newStyle);
			this.IsDirty = true;
		}

		// Token: 0x06006ACE RID: 27342 RVA: 0x002A06AF File Offset: 0x0029E8AF
		[PublicizedFrom(EAccessModifier.Private)]
		public void actionBindingsChanged()
		{
			if (Time.frameCount < this.frameOpened + 2)
			{
				return;
			}
			this.IsDirty = true;
			this.parentControls.SetChanged();
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x002A06D3 File Offset: 0x0029E8D3
		public void ResetToDefault()
		{
			PlayerAction playerAction = this.action;
			if (playerAction == null)
			{
				return;
			}
			playerAction.ResetBindings();
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x002A06E5 File Offset: 0x0029E8E5
		[XuiBindEvent("OnPress", "button")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void newBindingClick(XUiController _sender, int _mouseButton)
		{
			if (!this.HasAction || !this.actionData.allowRebind)
			{
				return;
			}
			XUiC_OptionsControlsNewBinding.GetNewBinding(this.xui, this.action, this.isControllerBinding);
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x002A0714 File Offset: 0x0029E914
		[XuiBindEvent("OnPress", "unbind")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void unbindButtonClick(XUiController _sender, int _mouseButton)
		{
			if (!this.HasAction || !this.actionData.allowRebind)
			{
				return;
			}
			this.action.UnbindBindingsOfType(this.isControllerBinding);
		}

		// Token: 0x040051B7 RID: 20919
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_OptionsDialogBase parentControls;

		// Token: 0x040051B8 RID: 20920
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction action;

		// Token: 0x040051B9 RID: 20921
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isControllerBinding;

		// Token: 0x040051BA RID: 20922
		[PublicizedFrom(EAccessModifier.Private)]
		public int frameOpened;

		// Token: 0x040051BB RID: 20923
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerActionData.ActionUserData actionData;

		// Token: 0x040051BC RID: 20924
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingString;

		// Token: 0x040051BD RID: 20925
		[XuiBindComponent("unbind", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button unbind;

		// Token: 0x040051BE RID: 20926
		[XuiBindComponent("background", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D91 RID: 3473
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_OptionsControlsBase : XUiC_OptionsDialogBase
{
	// Token: 0x14000094 RID: 148
	// (add) Token: 0x060069FF RID: 27135 RVA: 0x002989E8 File Offset: 0x00296BE8
	// (remove) Token: 0x06006A00 RID: 27136 RVA: 0x00298A1C File Offset: 0x00296C1C
	public static event Action OnSettingsChanged;

	// Token: 0x06006A01 RID: 27137 RVA: 0x00298A50 File Offset: 0x00296C50
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		XUiC_OptionsControlsBase.XUiC_BindingEntry[] childControllers = this.TabSelector.SelectedTab.GetChildControllers<XUiC_OptionsControlsBase.XUiC_BindingEntry>("", null);
		for (int i = 0; i < childControllers.Length; i++)
		{
			childControllers[i].ResetToDefault();
		}
	}

	// Token: 0x06006A02 RID: 27138
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void createControlsEntries();

	// Token: 0x06006A03 RID: 27139 RVA: 0x00298A8A File Offset: 0x00296C8A
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

	// Token: 0x06006A04 RID: 27140 RVA: 0x00298AB4 File Offset: 0x00296CB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void storeCurrentBindings()
	{
		this.actionBindingsOnOpen.Clear();
		foreach (PlayerActionsBase playerActionsBase in PlatformManager.NativePlatform.Input.ActionSets)
		{
			this.actionBindingsOnOpen.Add(playerActionsBase.Save());
		}
	}

	// Token: 0x06006A05 RID: 27141 RVA: 0x00298B20 File Offset: 0x00296D20
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

	// Token: 0x06006A06 RID: 27142 RVA: 0x00298B49 File Offset: 0x00296D49
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator storeBindingsLater()
	{
		yield return null;
		yield return null;
		this.storeCurrentBindings();
		yield break;
	}

	// Token: 0x06006A07 RID: 27143 RVA: 0x00298B58 File Offset: 0x00296D58
	public override void OnClose()
	{
		base.OnClose();
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(this.actionBindingsOnOpen);
	}

	// Token: 0x06006A08 RID: 27144 RVA: 0x00298B75 File Offset: 0x00296D75
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsControlsBase()
	{
	}

	// Token: 0x040050B8 RID: 20664
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool initialized;

	// Token: 0x040050B9 RID: 20665
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> actionBindingsOnOpen = new List<string>();

	// Token: 0x02000D92 RID: 3474
	[global::UnityEngine.Scripting.Preserve]
	public class XUiC_BindingEntry : XUiController
	{
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06006A09 RID: 27145 RVA: 0x00298B88 File Offset: 0x00296D88
		// (set) Token: 0x06006A0A RID: 27146 RVA: 0x00298B90 File Offset: 0x00296D90
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

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06006A0B RID: 27147 RVA: 0x00298C0A File Offset: 0x00296E0A
		// (set) Token: 0x06006A0C RID: 27148 RVA: 0x00298C12 File Offset: 0x00296E12
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

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06006A0D RID: 27149 RVA: 0x00298C2C File Offset: 0x00296E2C
		[XuiXmlBinding("has_action")]
		public bool HasAction
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06006A0E RID: 27150 RVA: 0x00298C37 File Offset: 0x00296E37
		[XuiXmlBinding("allow_rebind")]
		public bool AllowRebind
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return actionUserData != null && actionUserData.allowRebind;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06006A0F RID: 27151 RVA: 0x00298C4A File Offset: 0x00296E4A
		[XuiXmlBinding("action_label")]
		public string ActionLabel
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedName : null) ?? "";
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06006A10 RID: 27152 RVA: 0x00298C67 File Offset: 0x00296E67
		[XuiXmlBinding("action_description")]
		public string ActionDescription
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedDescription : null) ?? "";
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06006A11 RID: 27153 RVA: 0x00298C84 File Offset: 0x00296E84
		[XuiXmlBinding("action_binding")]
		public string ActionBinding
		{
			get
			{
				return this.bindingString ?? "";
			}
		}

		// Token: 0x06006A12 RID: 27154 RVA: 0x00298C95 File Offset: 0x00296E95
		public override void Init()
		{
			base.Init();
			base.registerForInputStyleChanges();
		}

		// Token: 0x06006A13 RID: 27155 RVA: 0x00298CA3 File Offset: 0x00296EA3
		public override void OnOpen()
		{
			base.OnOpen();
			this.frameOpened = Time.frameCount;
		}

		// Token: 0x06006A14 RID: 27156 RVA: 0x00298CB8 File Offset: 0x00296EB8
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

		// Token: 0x06006A15 RID: 27157 RVA: 0x00298D0B File Offset: 0x00296F0B
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.action != null)
			{
				this.action.OnBindingsChanged -= this.actionBindingsChanged;
			}
		}

		// Token: 0x06006A16 RID: 27158 RVA: 0x00298D32 File Offset: 0x00296F32
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
		{
			base.inputStyleChanged(_oldStyle, _newStyle);
			this.IsDirty = true;
		}

		// Token: 0x06006A17 RID: 27159 RVA: 0x00298D43 File Offset: 0x00296F43
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

		// Token: 0x06006A18 RID: 27160 RVA: 0x00298D67 File Offset: 0x00296F67
		public void ResetToDefault()
		{
			PlayerAction playerAction = this.action;
			if (playerAction == null)
			{
				return;
			}
			playerAction.ResetBindings();
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x00298D79 File Offset: 0x00296F79
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

		// Token: 0x06006A1A RID: 27162 RVA: 0x00298DA8 File Offset: 0x00296FA8
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

		// Token: 0x040050BA RID: 20666
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_OptionsDialogBase parentControls;

		// Token: 0x040050BB RID: 20667
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction action;

		// Token: 0x040050BC RID: 20668
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isControllerBinding;

		// Token: 0x040050BD RID: 20669
		[PublicizedFrom(EAccessModifier.Private)]
		public int frameOpened;

		// Token: 0x040050BE RID: 20670
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerActionData.ActionUserData actionData;

		// Token: 0x040050BF RID: 20671
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingString;

		// Token: 0x040050C0 RID: 20672
		[XuiBindComponent("unbind", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button unbind;

		// Token: 0x040050C1 RID: 20673
		[XuiBindComponent("background", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;
	}
}

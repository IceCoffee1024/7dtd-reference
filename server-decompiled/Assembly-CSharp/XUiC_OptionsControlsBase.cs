using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DA5 RID: 3493
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_OptionsControlsBase : XUiC_OptionsDialogBase
{
	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06006A99 RID: 27289 RVA: 0x0029BA9C File Offset: 0x00299C9C
	// (remove) Token: 0x06006A9A RID: 27290 RVA: 0x0029BAD0 File Offset: 0x00299CD0
	public static event Action OnSettingsChanged;

	// Token: 0x06006A9B RID: 27291 RVA: 0x0029BB04 File Offset: 0x00299D04
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		XUiC_OptionsControlsBase.XUiC_BindingEntry[] childControllers = this.TabSelector.SelectedTab.GetChildControllers<XUiC_OptionsControlsBase.XUiC_BindingEntry>("", null);
		for (int i = 0; i < childControllers.Length; i++)
		{
			childControllers[i].ResetToDefault();
		}
	}

	// Token: 0x06006A9C RID: 27292
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void createControlsEntries();

	// Token: 0x06006A9D RID: 27293 RVA: 0x0029BB3E File Offset: 0x00299D3E
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

	// Token: 0x06006A9E RID: 27294 RVA: 0x0029BB68 File Offset: 0x00299D68
	[PublicizedFrom(EAccessModifier.Protected)]
	public void storeCurrentBindings()
	{
		this.actionBindingsOnOpen.Clear();
		foreach (PlayerActionsBase playerActionsBase in PlatformManager.NativePlatform.Input.ActionSets)
		{
			this.actionBindingsOnOpen.Add(playerActionsBase.Save());
		}
	}

	// Token: 0x06006A9F RID: 27295 RVA: 0x0029BBD4 File Offset: 0x00299DD4
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

	// Token: 0x06006AA0 RID: 27296 RVA: 0x0029BBFD File Offset: 0x00299DFD
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator storeBindingsLater()
	{
		yield return null;
		yield return null;
		this.storeCurrentBindings();
		yield break;
	}

	// Token: 0x06006AA1 RID: 27297 RVA: 0x0029BC0C File Offset: 0x00299E0C
	public override void OnClose()
	{
		base.OnClose();
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(this.actionBindingsOnOpen);
	}

	// Token: 0x06006AA2 RID: 27298 RVA: 0x0029BC29 File Offset: 0x00299E29
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsControlsBase()
	{
	}

	// Token: 0x04005159 RID: 20825
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool initialized;

	// Token: 0x0400515A RID: 20826
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> actionBindingsOnOpen = new List<string>();

	// Token: 0x02000DA6 RID: 3494
	[global::UnityEngine.Scripting.Preserve]
	public class XUiC_BindingEntry : XUiController
	{
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0029BC3C File Offset: 0x00299E3C
		// (set) Token: 0x06006AA4 RID: 27300 RVA: 0x0029BC44 File Offset: 0x00299E44
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

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06006AA5 RID: 27301 RVA: 0x0029BCBE File Offset: 0x00299EBE
		// (set) Token: 0x06006AA6 RID: 27302 RVA: 0x0029BCC6 File Offset: 0x00299EC6
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

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06006AA7 RID: 27303 RVA: 0x0029BCE0 File Offset: 0x00299EE0
		[XuiXmlBinding("has_action")]
		public bool HasAction
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06006AA8 RID: 27304 RVA: 0x0029BCEB File Offset: 0x00299EEB
		[XuiXmlBinding("allow_rebind")]
		public bool AllowRebind
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return actionUserData != null && actionUserData.allowRebind;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x0029BCFE File Offset: 0x00299EFE
		[XuiXmlBinding("action_label")]
		public string ActionLabel
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedName : null) ?? "";
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06006AAA RID: 27306 RVA: 0x0029BD1B File Offset: 0x00299F1B
		[XuiXmlBinding("action_description")]
		public string ActionDescription
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedDescription : null) ?? "";
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06006AAB RID: 27307 RVA: 0x0029BD38 File Offset: 0x00299F38
		[XuiXmlBinding("action_binding")]
		public string ActionBinding
		{
			get
			{
				return this.bindingString ?? "";
			}
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x0029BD49 File Offset: 0x00299F49
		public override void Init()
		{
			base.Init();
			base.registerForInputStyleChanges();
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x0029BD57 File Offset: 0x00299F57
		public override void OnOpen()
		{
			base.OnOpen();
			this.frameOpened = Time.frameCount;
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x0029BD6C File Offset: 0x00299F6C
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

		// Token: 0x06006AAF RID: 27311 RVA: 0x0029BDBF File Offset: 0x00299FBF
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.action != null)
			{
				this.action.OnBindingsChanged -= this.actionBindingsChanged;
			}
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x0029BDE6 File Offset: 0x00299FE6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
		{
			base.inputStyleChanged(_oldStyle, _newStyle);
			this.IsDirty = true;
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x0029BDF7 File Offset: 0x00299FF7
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

		// Token: 0x06006AB2 RID: 27314 RVA: 0x0029BE1B File Offset: 0x0029A01B
		public void ResetToDefault()
		{
			PlayerAction playerAction = this.action;
			if (playerAction == null)
			{
				return;
			}
			playerAction.ResetBindings();
		}

		// Token: 0x06006AB3 RID: 27315 RVA: 0x0029BE2D File Offset: 0x0029A02D
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

		// Token: 0x06006AB4 RID: 27316 RVA: 0x0029BE5C File Offset: 0x0029A05C
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

		// Token: 0x0400515B RID: 20827
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_OptionsDialogBase parentControls;

		// Token: 0x0400515C RID: 20828
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction action;

		// Token: 0x0400515D RID: 20829
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isControllerBinding;

		// Token: 0x0400515E RID: 20830
		[PublicizedFrom(EAccessModifier.Private)]
		public int frameOpened;

		// Token: 0x0400515F RID: 20831
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerActionData.ActionUserData actionData;

		// Token: 0x04005160 RID: 20832
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingString;

		// Token: 0x04005161 RID: 20833
		[XuiBindComponent("unbind", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button unbind;

		// Token: 0x04005162 RID: 20834
		[XuiBindComponent("background", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;
	}
}

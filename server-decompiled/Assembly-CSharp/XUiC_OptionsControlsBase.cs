using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DB4 RID: 3508
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_OptionsControlsBase : XUiC_OptionsDialogBase
{
	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06006A91 RID: 27281 RVA: 0x0029EE2C File Offset: 0x0029D02C
	// (remove) Token: 0x06006A92 RID: 27282 RVA: 0x0029EE60 File Offset: 0x0029D060
	public static event Action OnSettingsChanged;

	// Token: 0x06006A93 RID: 27283 RVA: 0x0029EE94 File Offset: 0x0029D094
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		XUiC_OptionsControlsBase.XUiC_BindingEntry[] childControllers = this.TabSelector.SelectedTab.GetChildControllers<XUiC_OptionsControlsBase.XUiC_BindingEntry>("", null);
		for (int i = 0; i < childControllers.Length; i++)
		{
			childControllers[i].ResetToDefault();
		}
	}

	// Token: 0x06006A94 RID: 27284
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void createControlsEntries();

	// Token: 0x06006A95 RID: 27285 RVA: 0x0029EECE File Offset: 0x0029D0CE
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

	// Token: 0x06006A96 RID: 27286 RVA: 0x0029EEF8 File Offset: 0x0029D0F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void storeCurrentBindings()
	{
		this.actionBindingsOnOpen.Clear();
		foreach (PlayerActionsBase playerActionsBase in PlatformManager.NativePlatform.Input.ActionSets)
		{
			this.actionBindingsOnOpen.Add(playerActionsBase.Save());
		}
	}

	// Token: 0x06006A97 RID: 27287 RVA: 0x0029EF64 File Offset: 0x0029D164
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

	// Token: 0x06006A98 RID: 27288 RVA: 0x0029EF8D File Offset: 0x0029D18D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator storeBindingsLater()
	{
		yield return null;
		yield return null;
		this.storeCurrentBindings();
		yield break;
	}

	// Token: 0x06006A99 RID: 27289 RVA: 0x0029EF9C File Offset: 0x0029D19C
	public override void OnClose()
	{
		base.OnClose();
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(this.actionBindingsOnOpen);
	}

	// Token: 0x06006A9A RID: 27290 RVA: 0x0029EFB9 File Offset: 0x0029D1B9
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsControlsBase()
	{
	}

	// Token: 0x04005190 RID: 20880
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool initialized;

	// Token: 0x04005191 RID: 20881
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<string> actionBindingsOnOpen = new List<string>();

	// Token: 0x02000DB5 RID: 3509
	[global::UnityEngine.Scripting.Preserve]
	public class XUiC_BindingEntry : XUiController
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06006A9B RID: 27291 RVA: 0x0029EFCC File Offset: 0x0029D1CC
		// (set) Token: 0x06006A9C RID: 27292 RVA: 0x0029EFD4 File Offset: 0x0029D1D4
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

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06006A9D RID: 27293 RVA: 0x0029F04E File Offset: 0x0029D24E
		// (set) Token: 0x06006A9E RID: 27294 RVA: 0x0029F056 File Offset: 0x0029D256
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

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06006A9F RID: 27295 RVA: 0x0029F070 File Offset: 0x0029D270
		[XuiXmlBinding("has_action")]
		public bool HasAction
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06006AA0 RID: 27296 RVA: 0x0029F07B File Offset: 0x0029D27B
		[XuiXmlBinding("allow_rebind")]
		public bool AllowRebind
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return actionUserData != null && actionUserData.allowRebind;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06006AA1 RID: 27297 RVA: 0x0029F08E File Offset: 0x0029D28E
		[XuiXmlBinding("action_label")]
		public string ActionLabel
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedName : null) ?? "";
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x0029F0AB File Offset: 0x0029D2AB
		[XuiXmlBinding("action_description")]
		public string ActionDescription
		{
			get
			{
				PlayerActionData.ActionUserData actionUserData = this.actionData;
				return ((actionUserData != null) ? actionUserData.LocalizedDescription : null) ?? "";
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0029F0C8 File Offset: 0x0029D2C8
		[XuiXmlBinding("action_binding")]
		public string ActionBinding
		{
			get
			{
				return this.bindingString ?? "";
			}
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x0029F0D9 File Offset: 0x0029D2D9
		public override void Init()
		{
			base.Init();
			base.registerForInputStyleChanges();
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x0029F0E7 File Offset: 0x0029D2E7
		public override void OnOpen()
		{
			base.OnOpen();
			this.frameOpened = Time.frameCount;
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x0029F0FC File Offset: 0x0029D2FC
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

		// Token: 0x06006AA7 RID: 27303 RVA: 0x0029F14F File Offset: 0x0029D34F
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.action != null)
			{
				this.action.OnBindingsChanged -= this.actionBindingsChanged;
			}
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x0029F176 File Offset: 0x0029D376
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
		{
			base.inputStyleChanged(_oldStyle, _newStyle);
			this.IsDirty = true;
		}

		// Token: 0x06006AA9 RID: 27305 RVA: 0x0029F187 File Offset: 0x0029D387
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

		// Token: 0x06006AAA RID: 27306 RVA: 0x0029F1AB File Offset: 0x0029D3AB
		public void ResetToDefault()
		{
			PlayerAction playerAction = this.action;
			if (playerAction == null)
			{
				return;
			}
			playerAction.ResetBindings();
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x0029F1BD File Offset: 0x0029D3BD
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

		// Token: 0x06006AAC RID: 27308 RVA: 0x0029F1EC File Offset: 0x0029D3EC
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

		// Token: 0x04005192 RID: 20882
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_OptionsDialogBase parentControls;

		// Token: 0x04005193 RID: 20883
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction action;

		// Token: 0x04005194 RID: 20884
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isControllerBinding;

		// Token: 0x04005195 RID: 20885
		[PublicizedFrom(EAccessModifier.Private)]
		public int frameOpened;

		// Token: 0x04005196 RID: 20886
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerActionData.ActionUserData actionData;

		// Token: 0x04005197 RID: 20887
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingString;

		// Token: 0x04005198 RID: 20888
		[XuiBindComponent("unbind", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button unbind;

		// Token: 0x04005199 RID: 20889
		[XuiBindComponent("background", true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;
	}
}

using System;
using System.Collections;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E19 RID: 3609
[Preserve]
public abstract class XUiC_NewContinueBase : XUiC_PlayGameDialogBase
{
	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x06006EB0 RID: 28336 RVA: 0x002ADD75 File Offset: 0x002ABF75
	// (set) Token: 0x06006EB1 RID: 28337 RVA: 0x002ADD7D File Offset: 0x002ABF7D
	[XuiXmlBinding("startable")]
	public bool ValidStartableState
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.validStartableState;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (value == this.validStartableState)
			{
				return;
			}
			this.validStartableState = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x06006EB2 RID: 28338 RVA: 0x00295F26 File Offset: 0x00294126
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06006EB3 RID: 28339 RVA: 0x002ADD97 File Offset: 0x002ABF97
	// (set) Token: 0x06006EB4 RID: 28340 RVA: 0x002ADD9F File Offset: 0x002ABF9F
	[XuiXmlBinding("showbar")]
	public bool DataManagementBarEnabled
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.dataManagementBarEnabled;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (value == this.dataManagementBarEnabled)
			{
				return;
			}
			this.dataManagementBarEnabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06006EB5 RID: 28341
	public abstract bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06006EB6 RID: 28342
	public abstract bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06006EB7 RID: 28343 RVA: 0x002ADDBC File Offset: 0x002ABFBC
	public override void Init()
	{
		base.Init();
		((XUiC_Button)base.GetChildById("btnDataManagement")).OnPress += this.BtnDataManagement_OnPressed;
		this.DataManagementBarEnabled = this.DataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		this.IsDirty = true;
	}

	// Token: 0x06006EB8 RID: 28344 RVA: 0x002ADE0D File Offset: 0x002AC00D
	public override void OnOpen()
	{
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.Settings.ApplyCreativeModeChangeAllowed(this.AllowChangingCreativeMode);
		base.OnOpen();
	}

	// Token: 0x06006EB9 RID: 28345 RVA: 0x002ADE36 File Offset: 0x002AC036
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
	}

	// Token: 0x06006EBA RID: 28346 RVA: 0x002ADE43 File Offset: 0x002AC043
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006EBB RID: 28347 RVA: 0x002ADE61 File Offset: 0x002AC061
	[XuiBindEvent("SettingsChanged", "Settings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSettingsChanged()
	{
		this.validateStartable();
	}

	// Token: 0x06006EBC RID: 28348 RVA: 0x002ADE69 File Offset: 0x002AC069
	[XuiBindEvent("OpenSandboxSettingsRequested", "Settings")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnOpenSandboxSettingsRequested()
	{
		XUiC_SandboxOptions.Open(this.xui, delegate
		{
			this.Settings.UpdateSandboxPresetGroups();
		});
	}

	// Token: 0x06006EBD RID: 28349 RVA: 0x002ADE82 File Offset: 0x002AC082
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06006EBE RID: 28350
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnDataManagementWindowClosed();

	// Token: 0x06006EBF RID: 28351 RVA: 0x002ADE98 File Offset: 0x002AC098
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateBarUsageAndAllowanceValues()
	{
		if (!this.DataManagementBarEnabled)
		{
			return;
		}
		SaveInfoProvider instance = SaveInfoProvider.Instance;
		this.DataManagementBar.SetUsedBytes(instance.TotalUsedBytes);
		this.DataManagementBar.SetAllowanceBytes(instance.TotalAllowanceBytes);
	}

	// Token: 0x06006EC0 RID: 28352 RVA: 0x002ADED8 File Offset: 0x002AC0D8
	[XuiBindEvent("OnPress", "BtnStart")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BtnStart_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.BtnStart.ViewComponent.Enabled)
		{
			return;
		}
		GameManager.Instance.showOpenerMovieOnLoad = this.PlayIntroMovie;
		if (GamePrefs.GetBool(EnumGamePrefs.ServerEnabled))
		{
			EUserPerms perms = EUserPerms.HostMultiplayer;
			if (GamePrefs.GetBool(EnumGamePrefs.ServerAllowCrossplay))
			{
				perms |= EUserPerms.Crossplay;
			}
			XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(perms, delegate(bool _result)
			{
				bool flag = PermissionsManager.CanHostMultiplayer();
				GamePrefs.Set(EnumGamePrefs.ServerEnabled, flag);
				this.Settings.UpdateServerEnabledState();
				bool flag2 = perms.HasCrossplay() && PermissionsManager.IsCrossplayAllowed();
				GamePrefs.Set(EnumGamePrefs.ServerAllowCrossplay, flag2);
				this.Settings.UpdateCrossplayEnabledState();
				if (!_result)
				{
					return;
				}
				ThreadManager.StartCoroutine(this.startGameCo());
			}, -1f, false, null);
			return;
		}
		ThreadManager.StartCoroutine(this.startGameCo());
	}

	// Token: 0x06006EC1 RID: 28353
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract IEnumerator startGameCo();

	// Token: 0x06006EC2 RID: 28354
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void validateStartable();

	// Token: 0x06006EC3 RID: 28355 RVA: 0x002A1FB0 File Offset: 0x002A01B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_NewContinueBase()
	{
	}

	// Token: 0x04005415 RID: 21525
	public static bool LastStartedNewGame;

	// Token: 0x04005416 RID: 21526
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Button BtnStart;

	// Token: 0x04005417 RID: 21527
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_NewContinueGameSettings Settings;

	// Token: 0x04005418 RID: 21528
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validStartableState;

	// Token: 0x04005419 RID: 21529
	[XuiBindComponent("data_bar_controller", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_DataManagementBar DataManagementBar;

	// Token: 0x0400541A RID: 21530
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dataManagementBarEnabled;
}

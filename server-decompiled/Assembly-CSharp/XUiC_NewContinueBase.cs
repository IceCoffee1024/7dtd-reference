using System;
using System.Collections;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E28 RID: 3624
[Preserve]
public abstract class XUiC_NewContinueBase : XUiC_PlayGameDialogBase
{
	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06006EA8 RID: 28328 RVA: 0x002B111D File Offset: 0x002AF31D
	// (set) Token: 0x06006EA9 RID: 28329 RVA: 0x002B1125 File Offset: 0x002AF325
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

	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x06006EAA RID: 28330 RVA: 0x002992B2 File Offset: 0x002974B2
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x06006EAB RID: 28331 RVA: 0x002B113F File Offset: 0x002AF33F
	// (set) Token: 0x06006EAC RID: 28332 RVA: 0x002B1147 File Offset: 0x002AF347
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

	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x06006EAD RID: 28333
	public abstract bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x06006EAE RID: 28334
	public abstract bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06006EAF RID: 28335 RVA: 0x002B1164 File Offset: 0x002AF364
	public override void Init()
	{
		base.Init();
		((XUiC_Button)base.GetChildById("btnDataManagement")).OnPress += this.BtnDataManagement_OnPressed;
		this.DataManagementBarEnabled = this.DataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		this.IsDirty = true;
	}

	// Token: 0x06006EB0 RID: 28336 RVA: 0x002B11B5 File Offset: 0x002AF3B5
	public override void OnOpen()
	{
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.Settings.ApplyCreativeModeChangeAllowed(this.AllowChangingCreativeMode);
		base.OnOpen();
	}

	// Token: 0x06006EB1 RID: 28337 RVA: 0x002B11DE File Offset: 0x002AF3DE
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
	}

	// Token: 0x06006EB2 RID: 28338 RVA: 0x002B11EB File Offset: 0x002AF3EB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006EB3 RID: 28339 RVA: 0x002B1209 File Offset: 0x002AF409
	[XuiBindEvent("SettingsChanged", "Settings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSettingsChanged()
	{
		this.validateStartable();
	}

	// Token: 0x06006EB4 RID: 28340 RVA: 0x002B1211 File Offset: 0x002AF411
	[XuiBindEvent("OpenSandboxSettingsRequested", "Settings")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnOpenSandboxSettingsRequested()
	{
		XUiC_SandboxOptions.Open(this.xui, delegate
		{
			this.Settings.UpdateSandboxPresetGroups();
		});
	}

	// Token: 0x06006EB5 RID: 28341 RVA: 0x002B122A File Offset: 0x002AF42A
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06006EB6 RID: 28342
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnDataManagementWindowClosed();

	// Token: 0x06006EB7 RID: 28343 RVA: 0x002B1240 File Offset: 0x002AF440
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

	// Token: 0x06006EB8 RID: 28344 RVA: 0x002B1280 File Offset: 0x002AF480
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

	// Token: 0x06006EB9 RID: 28345
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract IEnumerator startGameCo();

	// Token: 0x06006EBA RID: 28346
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void validateStartable();

	// Token: 0x06006EBB RID: 28347 RVA: 0x002A5340 File Offset: 0x002A3540
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_NewContinueBase()
	{
	}

	// Token: 0x0400544C RID: 21580
	public static bool LastStartedNewGame;

	// Token: 0x0400544D RID: 21581
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Button BtnStart;

	// Token: 0x0400544E RID: 21582
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_NewContinueGameSettings Settings;

	// Token: 0x0400544F RID: 21583
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validStartableState;

	// Token: 0x04005450 RID: 21584
	[XuiBindComponent("data_bar_controller", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_DataManagementBar DataManagementBar;

	// Token: 0x04005451 RID: 21585
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dataManagementBarEnabled;
}

using System;
using System.Collections;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E33 RID: 3635
[Preserve]
public abstract class XUiC_NewContinueBase : XUiC_PlayGameDialogBase
{
	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x06006ECD RID: 28365 RVA: 0x002B2689 File Offset: 0x002B0889
	// (set) Token: 0x06006ECE RID: 28366 RVA: 0x002B2691 File Offset: 0x002B0891
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

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x06006ECF RID: 28367 RVA: 0x0029A7DA File Offset: 0x002989DA
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x06006ED0 RID: 28368 RVA: 0x002B26AB File Offset: 0x002B08AB
	// (set) Token: 0x06006ED1 RID: 28369 RVA: 0x002B26B3 File Offset: 0x002B08B3
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

	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06006ED2 RID: 28370
	public abstract bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06006ED3 RID: 28371
	public abstract bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06006ED4 RID: 28372 RVA: 0x002B26D0 File Offset: 0x002B08D0
	public override void Init()
	{
		base.Init();
		((XUiC_Button)base.GetChildById("btnDataManagement")).OnPress += this.BtnDataManagement_OnPressed;
		this.DataManagementBarEnabled = this.DataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		this.IsDirty = true;
	}

	// Token: 0x06006ED5 RID: 28373 RVA: 0x002B2721 File Offset: 0x002B0921
	public override void OnOpen()
	{
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.Settings.ApplyCreativeModeChangeAllowed(this.AllowChangingCreativeMode);
		base.OnOpen();
	}

	// Token: 0x06006ED6 RID: 28374 RVA: 0x002B274A File Offset: 0x002B094A
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
	}

	// Token: 0x06006ED7 RID: 28375 RVA: 0x002B2757 File Offset: 0x002B0957
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006ED8 RID: 28376 RVA: 0x002B2775 File Offset: 0x002B0975
	[XuiBindEvent("SettingsChanged", "Settings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSettingsChanged()
	{
		this.validateStartable();
	}

	// Token: 0x06006ED9 RID: 28377 RVA: 0x002B277D File Offset: 0x002B097D
	[XuiBindEvent("OpenSandboxSettingsRequested", "Settings")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnOpenSandboxSettingsRequested()
	{
		XUiC_SandboxOptions.Open(this.xui, delegate
		{
			this.Settings.UpdateSandboxPresetGroups();
		});
	}

	// Token: 0x06006EDA RID: 28378 RVA: 0x002B2796 File Offset: 0x002B0996
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06006EDB RID: 28379
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnDataManagementWindowClosed();

	// Token: 0x06006EDC RID: 28380 RVA: 0x002B27AC File Offset: 0x002B09AC
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

	// Token: 0x06006EDD RID: 28381 RVA: 0x002B27EC File Offset: 0x002B09EC
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

	// Token: 0x06006EDE RID: 28382
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract IEnumerator startGameCo();

	// Token: 0x06006EDF RID: 28383
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void validateStartable();

	// Token: 0x06006EE0 RID: 28384 RVA: 0x002A6868 File Offset: 0x002A4A68
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_NewContinueBase()
	{
	}

	// Token: 0x04005471 RID: 21617
	public static bool LastStartedNewGame;

	// Token: 0x04005472 RID: 21618
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Button BtnStart;

	// Token: 0x04005473 RID: 21619
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_NewContinueGameSettings Settings;

	// Token: 0x04005474 RID: 21620
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validStartableState;

	// Token: 0x04005475 RID: 21621
	[XuiBindComponent("data_bar_controller", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_DataManagementBar DataManagementBar;

	// Token: 0x04005476 RID: 21622
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dataManagementBarEnabled;
}

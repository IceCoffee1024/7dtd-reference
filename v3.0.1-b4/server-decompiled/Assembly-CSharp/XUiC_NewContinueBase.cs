using System;
using System.Collections;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E04 RID: 3588
[Preserve]
public abstract class XUiC_NewContinueBase : XUiC_PlayGameDialogBase
{
	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x06006E10 RID: 28176 RVA: 0x002AA861 File Offset: 0x002A8A61
	// (set) Token: 0x06006E11 RID: 28177 RVA: 0x002AA869 File Offset: 0x002A8A69
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

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06006E12 RID: 28178 RVA: 0x00292DD2 File Offset: 0x00290FD2
	[XuiXmlBinding("isroamingoptional")]
	public bool IsRoamingOptional
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		}
	}

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06006E13 RID: 28179 RVA: 0x002AA883 File Offset: 0x002A8A83
	// (set) Token: 0x06006E14 RID: 28180 RVA: 0x002AA88B File Offset: 0x002A8A8B
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

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06006E15 RID: 28181
	public abstract bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06006E16 RID: 28182
	public abstract bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06006E17 RID: 28183 RVA: 0x002AA8A8 File Offset: 0x002A8AA8
	public override void Init()
	{
		base.Init();
		((XUiC_Button)base.GetChildById("btnDataManagement")).OnPress += this.BtnDataManagement_OnPressed;
		this.DataManagementBarEnabled = this.DataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		this.IsDirty = true;
	}

	// Token: 0x06006E18 RID: 28184 RVA: 0x002AA8F9 File Offset: 0x002A8AF9
	public override void OnOpen()
	{
		this.windowGroup.openWindowOnEsc = XUiC_MainMenu.ID;
		this.Settings.ApplyCreativeModeChangeAllowed(this.AllowChangingCreativeMode);
		base.OnOpen();
	}

	// Token: 0x06006E19 RID: 28185 RVA: 0x002AA922 File Offset: 0x002A8B22
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
	}

	// Token: 0x06006E1A RID: 28186 RVA: 0x002AA92F File Offset: 0x002A8B2F
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006E1B RID: 28187 RVA: 0x002AA94D File Offset: 0x002A8B4D
	[XuiBindEvent("SettingsChanged", "Settings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSettingsChanged()
	{
		this.validateStartable();
	}

	// Token: 0x06006E1C RID: 28188 RVA: 0x002AA955 File Offset: 0x002A8B55
	[XuiBindEvent("OpenSandboxSettingsRequested", "Settings")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnOpenSandboxSettingsRequested()
	{
		XUiC_SandboxOptions.Open(this.xui, delegate
		{
			this.Settings.UpdateSandboxPresetGroups();
		});
	}

	// Token: 0x06006E1D RID: 28189 RVA: 0x002AA96E File Offset: 0x002A8B6E
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06006E1E RID: 28190
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnDataManagementWindowClosed();

	// Token: 0x06006E1F RID: 28191 RVA: 0x002AA984 File Offset: 0x002A8B84
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

	// Token: 0x06006E20 RID: 28192 RVA: 0x002AA9C4 File Offset: 0x002A8BC4
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

	// Token: 0x06006E21 RID: 28193
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract IEnumerator startGameCo();

	// Token: 0x06006E22 RID: 28194
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void validateStartable();

	// Token: 0x06006E23 RID: 28195 RVA: 0x0029EE40 File Offset: 0x0029D040
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_NewContinueBase()
	{
	}

	// Token: 0x04005372 RID: 21362
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_Button BtnStart;

	// Token: 0x04005373 RID: 21363
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_NewContinueGameSettings Settings;

	// Token: 0x04005374 RID: 21364
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validStartableState;

	// Token: 0x04005375 RID: 21365
	[XuiBindComponent("data_bar_controller", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_DataManagementBar DataManagementBar;

	// Token: 0x04005376 RID: 21366
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dataManagementBarEnabled;
}

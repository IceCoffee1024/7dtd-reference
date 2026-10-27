using System;
using System.Collections.Generic;
using System.Globalization;
using Challenges;
using Platform;
using Twitch;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DCC RID: 3532
[Preserve]
public class XUiC_OptionsTwitch : XUiC_OptionsDialogBase
{
	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06006B4E RID: 27470 RVA: 0x002A240C File Offset: 0x002A060C
	// (remove) Token: 0x06006B4F RID: 27471 RVA: 0x002A2440 File Offset: 0x002A0640
	public static event Action OnSettingsChanged;

	// Token: 0x06006B50 RID: 27472 RVA: 0x002A2474 File Offset: 0x002A0674
	public override void Init()
	{
		base.Init();
		this.tabs = base.GetChildByType<XUiC_TabSelector>();
		this.tabs.OnTabChanged += this.Tabs_OnTabChanged;
		this.comboAllowActionsDuringBloodmoon = base.GetChildById("AllowActionsDuringBloodmoon").GetChildByType<XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions>>();
		this.comboAllowActionsDuringQuests = base.GetChildById("AllowActionsDuringQuests").GetChildByType<XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions>>();
		this.comboPointsGeneration = base.GetChildById("PointGeneration").GetChildByType<XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions>>();
		this.comboCooldownPreset = base.GetChildById("CooldownPreset").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboUseProgression = base.GetChildById("UseProgression").GetChildByType<XUiC_ComboBoxBool>();
		this.comboOptOutTwitch = base.GetChildById("OptOutTwitch").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowVisionEffects = base.GetChildById("AllowVisual").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowCrateSharing = base.GetChildById("AllowCrateSharing").GetChildByType<XUiC_ComboBoxBool>();
		this.comboActionSpamDelay = base.GetChildById("ActionSpamDelay").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboActionTwitchIntegrationType = base.GetChildById("ActionTwitchIntegrationType").GetChildByType<XUiC_ComboBoxEnum<TwitchManager.IntegrationSettings>>();
		this.comboAllowActions = base.GetChildById("AllowActions").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboAllowVotes = base.GetChildById("AllowVotes").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboAllowEvents = base.GetChildById("AllowEvents").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboAllowVotesDuringBloodmoon = base.GetChildById("AllowVotesDuringBloodmoon").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowVotesDuringQuests = base.GetChildById("AllowVotesDuringQuests").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowVotesInSafeZone = base.GetChildById("AllowVotesInSafeZone").GetChildByType<XUiC_ComboBoxBool>();
		this.comboBitPrices = base.GetChildById("BitPrices").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboBitPotPercent = base.GetChildById("BitPotPercent").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboViewerStartingPoints = base.GetChildById("ViewerStartingPoints").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboBitPointsAdd = base.GetChildById("BitPointsAdd").GetChildByType<XUiC_ComboBoxEnum<XUiC_OptionsTwitch.BitAddOptions>>();
		this.comboSubPointsAdd = base.GetChildById("SubPointsAdd").GetChildByType<XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions>>();
		this.comboGiftSubPointsAdd = base.GetChildById("GiftSubPointsAdd").GetChildByType<XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions>>();
		this.comboRaidPointsAdd = base.GetChildById("RaidPointsAdd").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboRaidViewerMinimum = base.GetChildById("RaidViewerMinimum").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboHelperRewardAmount = base.GetChildById("HelperRewardAmount").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboHelperRewardTime = base.GetChildById("HelperRewardTime").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboMaxDailyVotes = base.GetChildById("MaxDailyVotes").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboVoteTime = base.GetChildById("VoteTime").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboVoteDayTimeRange = base.GetChildById("VoteDayTimeRange").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboVoteViewerDefeatReward = base.GetChildById("ViewerDefeatReward").GetChildByType<XUiC_ComboBoxList<string>>();
		this.comboAllowBitEvents = base.GetChildById("AllowBitEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowSubEvents = base.GetChildById("AllowSubEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowGiftSubEvents = base.GetChildById("AllowGiftSubEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowCharityEvents = base.GetChildById("AllowCharityEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowRaidEvents = base.GetChildById("AllowRaidEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowHypeTrainEvents = base.GetChildById("AllowHypeTrainEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowCreatorGoalEvents = base.GetChildById("AllowCreatorGoalEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowChannelPointRedeemEvents = base.GetChildById("AllowChannelPointRedeemEvents").GetChildByType<XUiC_ComboBoxBool>();
		this.comboAllowActionsDuringBloodmoon.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowActionsDuringQuests.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboPointsGeneration.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboUseProgression.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboOptOutTwitch.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowVisionEffects.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowCrateSharing.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboActionTwitchIntegrationType.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowActions.OnValueChanged += this.ComboPreset_OnValueChanged;
		this.comboAllowVotes.OnValueChanged += this.ComboPreset_OnValueChanged;
		this.comboAllowEvents.OnValueChanged += this.ComboPreset_OnValueChanged;
		this.comboAllowVotesDuringBloodmoon.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowVotesDuringQuests.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowVotesInSafeZone.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboBitPointsAdd.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboSubPointsAdd.OnValueChanged += this.SubPoints_OnValueChanged;
		this.comboGiftSubPointsAdd.OnValueChanged += this.GiftSubPoints_OnValueChanged;
		this.comboAllowBitEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowSubEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowGiftSubEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowCharityEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowRaidEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowHypeTrainEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowCreatorGoalEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboAllowChannelPointRedeemEvents.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboBitPrices.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		this.comboBitPotPercent.OnValueChangedGeneric += this.Combo_OnValueChangedGeneric;
		XUiC_Button xuiC_Button = base.GetChildById("btnLoginTwitch") as XUiC_Button;
		if (xuiC_Button != null)
		{
			xuiC_Button.OnPress += this.BtnLoginTwitch_OnPressed;
		}
		XUiC_Button xuiC_Button2 = base.GetChildById("btnShowExtras") as XUiC_Button;
		if (xuiC_Button2 != null)
		{
			xuiC_Button2.OnPress += this.BtnShowExtras_OnPressed;
		}
		XUiC_Button xuiC_Button3 = base.GetChildById("btnEnableAllExtras") as XUiC_Button;
		if (xuiC_Button3 != null)
		{
			xuiC_Button3.OnPress += this.BtnEnableAllExtras_OnPressed;
		}
		XUiC_Button xuiC_Button4 = base.GetChildById("btnDisableAllExtras") as XUiC_Button;
		if (xuiC_Button4 != null)
		{
			xuiC_Button4.OnPress += this.BtnDisableAllExtras_OnPressed;
		}
		XUiC_Button xuiC_Button5 = base.GetChildById("btnResetPrices") as XUiC_Button;
		if (xuiC_Button5 != null)
		{
			xuiC_Button5.OnPress += this.BtnResetPrices_OnPressed;
		}
		XUiC_Button xuiC_Button6 = base.GetChildById("btnShowDeviceCode") as XUiC_Button;
		if (xuiC_Button6 != null)
		{
			xuiC_Button6.OnPress += this.BtnShowDeviceCode_OnPressed;
		}
		this.qrCodeTexControl = base.GetChildById("qrCodeTex").ViewComponent as XUiV_Texture;
		TwitchAuthentication.QRCodeGenerated += this.TwitchAuthentication_QRCodeGenerated;
	}

	// Token: 0x06006B51 RID: 27473 RVA: 0x002A2B71 File Offset: 0x002A0D71
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowDeviceCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.showDeviceCode = !this.showDeviceCode;
		base.RefreshBindings();
	}

	// Token: 0x06006B52 RID: 27474 RVA: 0x002A2B88 File Offset: 0x002A0D88
	[PublicizedFrom(EAccessModifier.Private)]
	public void Combo_OnValueChangedGeneric(XUiController _sender)
	{
		base.SetChanged();
	}

	// Token: 0x06006B53 RID: 27475 RVA: 0x002A2B90 File Offset: 0x002A0D90
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tabs_OnTabChanged(int _i, XUiC_TabSelectorTab _tab)
	{
		if (_i != 0 && this.twitchManager.InitState != TwitchManager.InitStates.Ready)
		{
			this.tabs.SelectedTabIndex = 0;
		}
		this.IsDirty = true;
	}

	// Token: 0x06006B54 RID: 27476 RVA: 0x002A2B88 File Offset: 0x002A0D88
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboBoxListString_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
	}

	// Token: 0x06006B55 RID: 27477 RVA: 0x002A2B88 File Offset: 0x002A0D88
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboPreset_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
	}

	// Token: 0x06006B56 RID: 27478 RVA: 0x002A2BB6 File Offset: 0x002A0DB6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboVoteDayTimeRange_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
		this.tempVoteDayTimeRange = this.comboVoteDayTimeRange.SelectedIndex;
	}

	// Token: 0x06006B57 RID: 27479 RVA: 0x002A2BCF File Offset: 0x002A0DCF
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubPoints_OnValueChanged(XUiController _sender, XUiC_OptionsTwitch.PointsGenerationOptions _oldValue, XUiC_OptionsTwitch.PointsGenerationOptions _newValue)
	{
		base.SetChanged();
		this.tempSubModifier = (int)_newValue;
	}

	// Token: 0x06006B58 RID: 27480 RVA: 0x002A2BDE File Offset: 0x002A0DDE
	[PublicizedFrom(EAccessModifier.Private)]
	public void GiftSubPoints_OnValueChanged(XUiController _sender, XUiC_OptionsTwitch.PointsGenerationOptions _oldValue, XUiC_OptionsTwitch.PointsGenerationOptions _newValue)
	{
		base.SetChanged();
		this.IsDirty = true;
		this.tempGiftSubModifier = (int)_newValue;
	}

	// Token: 0x06006B59 RID: 27481 RVA: 0x002A2BF4 File Offset: 0x002A0DF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLoginTwitch_OnPressed(XUiController _sender, int _mouseButton)
	{
		switch (this.twitchManager.InitState)
		{
		case TwitchManager.InitStates.None:
		case TwitchManager.InitStates.PermissionDenied:
		case TwitchManager.InitStates.WaitingForOAuth:
		case TwitchManager.InitStates.ExtensionNotInstalled:
		case TwitchManager.InitStates.Failed:
			this.startedSinceOpened = true;
			this.showDeviceCode = false;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.twitchManager.StartTwitchIntegration();
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTwitchAccess>().Setup(), false);
				this.twitchManager.WaitForPermission();
			}
			if (!this.localPlayer.PlayerUI.windowManager.IsWindowOpen("twitch"))
			{
				this.localPlayer.PlayerUI.windowManager.Open("twitch", false);
			}
			this.origAllowTwitchOptions = true;
			this.localPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Enabled;
			return;
		case TwitchManager.InitStates.WaitingForPermission:
		case TwitchManager.InitStates.Authenticating:
		case TwitchManager.InitStates.Authenticated:
		case TwitchManager.InitStates.CheckingForExtension:
			break;
		case TwitchManager.InitStates.Ready:
			this.twitchManager.StopTwitchIntegration(TwitchManager.InitStates.None);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B5A RID: 27482 RVA: 0x002A2CDC File Offset: 0x002A0EDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowExtras_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.applyChanges();
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (GameManager.Instance.IsPaused() && GameStats.GetInt(EnumGameStats.GameState) == 2)
		{
			GameManager.Instance.Pause(false);
		}
		XUiC_TwitchWindowSelector.OpenSelectorAndWindow(GameManager.Instance.World.GetPrimaryPlayer(), "Actions", true);
	}

	// Token: 0x06006B5B RID: 27483 RVA: 0x002A2D48 File Offset: 0x002A0F48
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEnableAllExtras_OnPressed(XUiController _sender, int _mouseButton)
	{
		List<TwitchAction> list = (from entry in TwitchActionManager.TwitchActions.Values.ToList<TwitchAction>()
			where entry.DisplayCategory.Name == "Extras"
			orderby entry.Title
			select entry).ToList<TwitchAction>();
		TwitchActionPreset currentActionPreset = TwitchManager.Current.CurrentActionPreset;
		foreach (TwitchAction twitchAction in list)
		{
			if (!currentActionPreset.AddedActions.Contains(twitchAction.Name))
			{
				currentActionPreset.AddedActions.Add(twitchAction.Name);
				QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.EnableExtras, twitchAction.DisplayCategory.Name);
			}
		}
	}

	// Token: 0x06006B5C RID: 27484 RVA: 0x002A2E30 File Offset: 0x002A1030
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDisableAllExtras_OnPressed(XUiController _sender, int _mouseButton)
	{
		List<TwitchAction> list = (from entry in TwitchActionManager.TwitchActions.Values.ToList<TwitchAction>()
			where entry.DisplayCategory.Name == "Extras"
			orderby entry.Title
			select entry).ToList<TwitchAction>();
		TwitchActionPreset currentActionPreset = TwitchManager.Current.CurrentActionPreset;
		foreach (TwitchAction twitchAction in list)
		{
			if (currentActionPreset.AddedActions.Contains(twitchAction.Name))
			{
				currentActionPreset.AddedActions.Remove(twitchAction.Name);
			}
		}
	}

	// Token: 0x06006B5D RID: 27485 RVA: 0x002A2F04 File Offset: 0x002A1104
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnResetPrices_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.twitchManager.ResetPricesToDefault();
	}

	// Token: 0x06006B5E RID: 27486 RVA: 0x002A2F11 File Offset: 0x002A1111
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		this.applyChanges();
	}

	// Token: 0x06006B5F RID: 27487 RVA: 0x002A2F20 File Offset: 0x002A1120
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doResetToDefaultsInternal()
	{
		base.doResetToDefaultsInternal();
		this.twitchManager.UseActionsDuringBloodmoon = 2;
		this.twitchManager.UseActionsDuringQuests = 2;
		this.twitchManager.ViewerData.PointRate = 1f;
		this.twitchManager.SetToDefaultCooldown();
		this.twitchManager.SetUseProgression(true);
		this.twitchManager.AllowCrateSharing = false;
		this.twitchManager.ViewerData.ActionSpamDelay = 3f;
		TwitchVotingManager votingManager = this.twitchManager.VotingManager;
		this.twitchManager.SetToDefaultActionPreset();
		this.twitchManager.SetToDefaultVotePreset();
		this.twitchManager.SetToDefaultEventPreset();
		this.twitchManager.ViewerData.StartingPoints = 100;
		this.twitchManager.BitPointModifier = 1;
		this.twitchManager.SubPointModifier = 1;
		this.twitchManager.GiftSubPointModifier = 2;
		this.twitchManager.RaidPointAdd = 1000;
		this.twitchManager.RaidViewerMinimum = 10;
		this.twitchManager.BitPotPercentage = 0.25f;
		TwitchManager.LeaderboardStats.GoodRewardAmount = 1000;
		TwitchManager.LeaderboardStats.GoodRewardTime = 15;
		this.localPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Enabled;
		votingManager.MaxDailyVotes = 4;
		votingManager.VoteTime = 60f;
		votingManager.CurrentVoteDayTimeRange = 2;
		votingManager.ViewerDefeatReward = 250;
		votingManager.AllowVotesDuringBloodmoon = false;
		votingManager.AllowVotesDuringQuests = false;
		votingManager.AllowVotesInSafeZone = false;
		this.updateOptions();
		this.applyChanges();
		base.SetChanged();
	}

	// Token: 0x06006B60 RID: 27488 RVA: 0x002A3098 File Offset: 0x002A1298
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchAuthentication_QRCodeGenerated(Texture2D _qrCodeTex, string _userCode, string _verificationUrl)
	{
		this.qrCodeTexControl.Texture = _qrCodeTex;
		this.authCode = _userCode;
		this.authVerificationUrl = _verificationUrl;
		base.RefreshBindings();
	}

	// Token: 0x06006B61 RID: 27489 RVA: 0x002A30BC File Offset: 0x002A12BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateOptions()
	{
		if (GameStats.GetBool(EnumGameStats.TwitchBloodMoonAllowed))
		{
			this.comboAllowActionsDuringBloodmoon.Value = (this.origAllowActionsDuringBloodmoon = (XUiC_OptionsTwitch.TwitchBloodMoonOptions)TwitchManager.Current.UseActionsDuringBloodmoon);
			this.comboAllowActionsDuringBloodmoon.Enabled = true;
		}
		else
		{
			this.comboAllowActionsDuringBloodmoon.Value = XUiC_OptionsTwitch.TwitchBloodMoonOptions.Disabled;
			this.comboAllowActionsDuringBloodmoon.Enabled = false;
			this.twitchManager.UseActionsDuringBloodmoon = 0;
		}
		this.comboAllowActionsDuringQuests.Value = (this.origAllowActionsDuringQuests = (XUiC_OptionsTwitch.TwitchBloodMoonOptions)TwitchManager.Current.UseActionsDuringQuests);
		this.comboPointsGeneration.Value = (this.origPointsGeneration = (XUiC_OptionsTwitch.PointsGenerationOptions)this.twitchManager.ViewerData.PointRate);
		this.comboCooldownPreset.SelectedIndex = (this.origCooldownPresetIndex = this.twitchManager.CooldownPresetIndex);
		this.comboUseProgression.Value = (this.origUseProgression = this.twitchManager.UseProgression);
		this.comboAllowVisionEffects.Value = (this.origAllowVisionEffects = !this.localPlayer.TwitchVisionDisabled);
		this.comboOptOutTwitch.Value = (this.origAllowTwitchOptions = this.localPlayer.TwitchActionsEnabled > EntityPlayer.TwitchActionsStates.Disabled);
		this.comboAllowCrateSharing.Value = (this.origAllowCrateSharing = this.twitchManager.AllowCrateSharing);
		this.comboActionTwitchIntegrationType.Value = (this.origIntegrationSetting = this.twitchManager.IntegrationSetting);
		TwitchVotingManager votingManager = this.twitchManager.VotingManager;
		this.comboAllowActions.SelectedIndex = (this.origActionPresetIndex = this.twitchManager.ActionPresetIndex);
		this.comboAllowVotes.SelectedIndex = (this.origVotePresetIndex = this.twitchManager.VotePresetIndex);
		this.comboAllowEvents.SelectedIndex = (this.origEventPresetIndex = this.twitchManager.EventPresetIndex);
		this.comboBitPrices.SelectedIndex = (this.origBitPricesIndex = this.getBitPriceModifier(this.twitchManager.BitPriceMultiplier));
		this.comboBitPotPercent.SelectedIndex = (this.origBitPotPercentIndex = Mathf.FloorToInt((1f - this.twitchManager.BitPotPercentage) / 0.05f));
		this.comboViewerStartingPoints.SelectedIndex = (this.origViewerStartingPoints = this.getIndexFromCombobox(this.twitchManager.ViewerData.StartingPoints, this.comboViewerStartingPoints));
		this.comboBitPointsAdd.Value = (this.origBitPointModifier = (XUiC_OptionsTwitch.BitAddOptions)this.twitchManager.BitPointModifier);
		this.comboSubPointsAdd.Value = (this.origSubPointModifier = (XUiC_OptionsTwitch.PointsGenerationOptions)this.twitchManager.SubPointModifier);
		this.comboGiftSubPointsAdd.Value = (this.origGiftSubPointModifier = (XUiC_OptionsTwitch.PointsGenerationOptions)this.twitchManager.GiftSubPointModifier);
		this.comboRaidPointsAdd.SelectedIndex = (this.origRaidPointAmountIndex = this.getIndexFromCombobox(this.twitchManager.RaidPointAdd, this.comboRaidPointsAdd));
		this.comboRaidViewerMinimum.SelectedIndex = (this.origRaidViewerMinimumIndex = this.getIndexFromCombobox(this.twitchManager.RaidViewerMinimum, this.comboRaidViewerMinimum));
		this.comboHelperRewardAmount.SelectedIndex = (this.origHelperRewardAmountIndex = this.getIndexFromCombobox(TwitchManager.LeaderboardStats.GoodRewardAmount, this.comboHelperRewardAmount));
		this.comboHelperRewardTime.SelectedIndex = (this.origHelperRewardTimeIndex = this.getIndexFromCombobox(TwitchManager.LeaderboardStats.GoodRewardTime, this.comboHelperRewardTime));
		this.comboMaxDailyVotes.SelectedIndex = (this.origMaxDailyVotes = this.getIndexFromCombobox(votingManager.MaxDailyVotes, this.comboMaxDailyVotes));
		this.comboVoteTime.SelectedIndex = (this.origVoteTime = this.getIndexFromCombobox((int)votingManager.VoteTime, this.comboVoteTime));
		this.comboVoteDayTimeRange.SelectedIndex = (this.origVoteDayTimeRange = votingManager.CurrentVoteDayTimeRange);
		this.comboVoteViewerDefeatReward.SelectedIndex = (this.origVoteViewerDefeatReward = this.getIndexFromCombobox(votingManager.ViewerDefeatReward, this.comboVoteViewerDefeatReward));
		this.comboActionSpamDelay.SelectedIndex = (this.origActionSpamDelay = this.getActionSpamDelay(this.twitchManager.ViewerData.ActionSpamDelay));
		this.comboAllowVotesDuringBloodmoon.Value = (this.origAllowVotesDuringBloodmoon = votingManager.AllowVotesDuringBloodmoon);
		this.comboAllowVotesDuringQuests.Value = (this.origAllowVotesDuringQuests = votingManager.AllowVotesDuringQuests);
		this.comboAllowVotesInSafeZone.Value = (this.origAllowVotesInSafeZone = votingManager.AllowVotesInSafeZone);
		this.comboAllowBitEvents.Value = (this.origAllowBitEvents = this.twitchManager.AllowBitEvents);
		this.comboAllowSubEvents.Value = (this.origAllowSubEvents = this.twitchManager.AllowSubEvents);
		this.comboAllowGiftSubEvents.Value = (this.origAllowGiftSubEvents = this.twitchManager.AllowGiftSubEvents);
		this.comboAllowCharityEvents.Value = (this.origAllowCharityEvents = this.twitchManager.AllowCharityEvents);
		this.comboAllowRaidEvents.Value = (this.origAllowRaidEvents = this.twitchManager.AllowRaidEvents);
		this.comboAllowHypeTrainEvents.Value = (this.origAllowHypeTrainEvents = this.twitchManager.AllowHypeTrainEvents);
		this.comboAllowCreatorGoalEvents.Value = (this.origAllowCreatorGoalEvents = this.twitchManager.AllowCreatorGoalEvents);
		this.comboAllowChannelPointRedeemEvents.Value = (this.origAllowChannelPointRedemptions = this.twitchManager.AllowChannelPointRedemptions);
		this.tempSubModifier = (int)this.origSubPointModifier;
		this.tempGiftSubModifier = (int)this.origGiftSubPointModifier;
		this.tempVoteDayTimeRange = this.origVoteDayTimeRange;
	}

	// Token: 0x06006B62 RID: 27490 RVA: 0x002A3660 File Offset: 0x002A1860
	[PublicizedFrom(EAccessModifier.Private)]
	public int getIndexFromCombobox(int _value, XUiC_ComboBoxList<string> _cboList)
	{
		string text = _value.ToString();
		for (int i = 0; i < _cboList.Elements.Count; i++)
		{
			if (_cboList.Elements[i] == text)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06006B63 RID: 27491 RVA: 0x002A36A4 File Offset: 0x002A18A4
	[PublicizedFrom(EAccessModifier.Private)]
	public int getActionSpamDelay(float _actionSpamDelay)
	{
		string text = ((int)_actionSpamDelay).ToString();
		for (int i = 0; i < this.comboActionSpamDelay.Elements.Count; i++)
		{
			if (i == 0)
			{
				if (text == "0")
				{
					return i;
				}
			}
			else if (this.comboActionSpamDelay.Elements[i] == text)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06006B64 RID: 27492 RVA: 0x002A3708 File Offset: 0x002A1908
	[PublicizedFrom(EAccessModifier.Private)]
	public int getBitPriceModifier(float _bitPriceModifier)
	{
		if (_bitPriceModifier <= 0.5f)
		{
			if (_bitPriceModifier == 0.25f)
			{
				return 3;
			}
			if (_bitPriceModifier == 0.5f)
			{
				return 2;
			}
		}
		else
		{
			if (_bitPriceModifier == 0.75f)
			{
				return 1;
			}
			if (_bitPriceModifier == 1f)
			{
				return 0;
			}
		}
		return 0;
	}

	// Token: 0x06006B65 RID: 27493 RVA: 0x002A3754 File Offset: 0x002A1954
	[PublicizedFrom(EAccessModifier.Private)]
	public float setBitPriceModifier(int _index)
	{
		float num;
		switch (_index)
		{
		case 0:
			num = 1f;
			break;
		case 1:
			num = 0.75f;
			break;
		case 2:
			num = 0.5f;
			break;
		case 3:
			num = 0.25f;
			break;
		default:
			num = 1f;
			break;
		}
		return num;
	}

	// Token: 0x06006B66 RID: 27494 RVA: 0x002A37A0 File Offset: 0x002A19A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyChanges()
	{
		this.origAllowActionsDuringBloodmoon = this.comboAllowActionsDuringBloodmoon.Value;
		this.origAllowActionsDuringQuests = this.comboAllowActionsDuringQuests.Value;
		this.origPointsGeneration = this.comboPointsGeneration.Value;
		this.origCooldownPresetIndex = this.comboCooldownPreset.SelectedIndex;
		this.origUseProgression = this.comboUseProgression.Value;
		this.origAllowVisionEffects = this.comboAllowVisionEffects.Value;
		this.origAllowTwitchOptions = this.comboOptOutTwitch.Value;
		this.origAllowCrateSharing = this.comboAllowCrateSharing.Value;
		this.origActionSpamDelay = this.comboActionSpamDelay.SelectedIndex;
		this.origIntegrationSetting = this.comboActionTwitchIntegrationType.Value;
		this.origActionPresetIndex = this.comboAllowActions.SelectedIndex;
		this.origVotePresetIndex = this.comboAllowVotes.SelectedIndex;
		this.origEventPresetIndex = this.comboAllowEvents.SelectedIndex;
		this.origViewerStartingPoints = this.comboViewerStartingPoints.SelectedIndex;
		this.origBitPointModifier = this.comboBitPointsAdd.Value;
		this.origSubPointModifier = this.comboSubPointsAdd.Value;
		this.origGiftSubPointModifier = this.comboGiftSubPointsAdd.Value;
		this.origRaidPointAmountIndex = this.comboRaidPointsAdd.SelectedIndex;
		this.origRaidViewerMinimumIndex = this.comboRaidViewerMinimum.SelectedIndex;
		this.origHelperRewardAmountIndex = this.comboHelperRewardAmount.SelectedIndex;
		this.origHelperRewardTimeIndex = this.comboHelperRewardTime.SelectedIndex;
		this.origBitPricesIndex = this.comboBitPrices.SelectedIndex;
		this.origBitPotPercentIndex = this.comboBitPotPercent.SelectedIndex;
		this.origMaxDailyVotes = this.comboMaxDailyVotes.SelectedIndex;
		this.origVoteTime = this.comboVoteTime.SelectedIndex;
		this.origVoteDayTimeRange = this.comboVoteDayTimeRange.SelectedIndex;
		this.origVoteViewerDefeatReward = this.comboVoteViewerDefeatReward.SelectedIndex;
		this.origAllowVotesDuringBloodmoon = this.comboAllowVotesDuringBloodmoon.Value;
		this.origAllowVotesDuringQuests = this.comboAllowVotesDuringQuests.Value;
		this.origAllowVotesInSafeZone = this.comboAllowVotesInSafeZone.Value;
		this.origAllowBitEvents = this.comboAllowBitEvents.Value;
		this.origAllowSubEvents = this.comboAllowSubEvents.Value;
		this.origAllowGiftSubEvents = this.comboAllowGiftSubEvents.Value;
		this.origAllowCharityEvents = this.comboAllowCharityEvents.Value;
		this.origAllowRaidEvents = this.comboAllowRaidEvents.Value;
		this.origAllowHypeTrainEvents = this.comboAllowHypeTrainEvents.Value;
		this.origAllowCreatorGoalEvents = this.comboAllowCreatorGoalEvents.Value;
		this.origAllowChannelPointRedemptions = this.comboAllowChannelPointRedeemEvents.Value;
		Action onSettingsChanged = XUiC_OptionsTwitch.OnSettingsChanged;
		if (onSettingsChanged == null)
		{
			return;
		}
		onSettingsChanged();
	}

	// Token: 0x06006B67 RID: 27495 RVA: 0x002A3A44 File Offset: 0x002A1C44
	public override void OnOpen()
	{
		this.twitchManager = TwitchManager.Current;
		this.twitchManager.ConnectionStateChanged -= this.TwitchManager_ConnectionStateChanged;
		this.twitchManager.ConnectionStateChanged += this.TwitchManager_ConnectionStateChanged;
		this.comboCooldownPreset.OnValueChanged -= this.ComboBoxListString_OnValueChanged;
		this.comboCooldownPreset.Elements.Clear();
		for (int i = 0; i < this.twitchManager.CooldownPresets.Count; i++)
		{
			this.comboCooldownPreset.Elements.Add(this.twitchManager.CooldownPresets[i].Title);
		}
		if (this.twitchManager.CooldownPresetIndex >= this.comboCooldownPreset.Elements.Count)
		{
			this.twitchManager.SetToDefaultCooldown();
		}
		this.comboCooldownPreset.Value = this.comboCooldownPreset.Elements[this.twitchManager.CooldownPresetIndex];
		this.comboCooldownPreset.OnValueChanged += this.ComboBoxListString_OnValueChanged;
		this.localPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		this.comboAllowActions.OnValueChanged -= this.ComboPreset_OnValueChanged;
		this.comboAllowActions.Elements.Clear();
		for (int j = 0; j < this.twitchManager.ActionPresets.Count; j++)
		{
			TwitchActionPreset twitchActionPreset = this.twitchManager.ActionPresets[j];
			if (twitchActionPreset.IsEnabled)
			{
				this.comboAllowActions.Elements.Add(twitchActionPreset.Title);
			}
		}
		if (this.twitchManager.ActionPresetIndex >= this.comboAllowActions.Elements.Count)
		{
			this.twitchManager.SetToDefaultActionPreset();
		}
		this.comboAllowActions.Value = this.comboAllowActions.Elements[this.twitchManager.ActionPresetIndex];
		this.comboAllowActions.OnValueChanged += this.ComboPreset_OnValueChanged;
		this.comboAllowVotes.OnValueChanged -= this.ComboPreset_OnValueChanged;
		this.comboAllowVotes.Elements.Clear();
		for (int k = 0; k < this.twitchManager.VotePresets.Count; k++)
		{
			this.comboAllowVotes.Elements.Add(this.twitchManager.VotePresets[k].Title);
		}
		if (this.twitchManager.VotePresetIndex >= this.comboAllowVotes.Elements.Count)
		{
			this.twitchManager.SetToDefaultVotePreset();
		}
		this.comboAllowVotes.Value = this.comboAllowVotes.Elements[this.twitchManager.VotePresetIndex];
		this.comboAllowVotes.OnValueChanged += this.ComboPreset_OnValueChanged;
		this.comboAllowEvents.OnValueChanged -= this.ComboPreset_OnValueChanged;
		this.comboAllowEvents.Elements.Clear();
		for (int l = 0; l < this.twitchManager.EventPresets.Count; l++)
		{
			this.comboAllowEvents.Elements.Add(this.twitchManager.EventPresets[l].Title);
		}
		if (this.twitchManager.EventPresetIndex >= this.comboAllowEvents.Elements.Count)
		{
			this.twitchManager.SetToDefaultEventPreset();
		}
		this.comboAllowEvents.Value = this.comboAllowEvents.Elements[this.twitchManager.EventPresetIndex];
		this.comboAllowEvents.OnValueChanged += this.ComboPreset_OnValueChanged;
		this.setupComboBoxListString(this.comboViewerStartingPoints, new string[] { "0", "50", "100", "150", "200", "250", "500" }, this.twitchManager.ViewerData.StartingPoints, null);
		this.setupComboBoxListString(this.comboRaidPointsAdd, new string[] { "0", "500", "1000", "1500", "2000", "2500" }, this.twitchManager.RaidPointAdd, null);
		this.setupComboBoxListString(this.comboRaidViewerMinimum, new string[] { "1", "3", "5", "10", "15", "20", "25", "50", "100" }, this.twitchManager.RaidViewerMinimum, null);
		this.setupComboBoxListString(this.comboMaxDailyVotes, new string[]
		{
			"1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
			"11", "12"
		}, this.twitchManager.VotingManager.MaxDailyVotes, null);
		this.setupComboBoxListString(this.comboVoteTime, new string[] { "30", "60", "90", "120" }, (int)this.twitchManager.VotingManager.VoteTime, null);
		this.setupComboBoxListString(this.comboHelperRewardAmount, new string[] { "100", "250", "500", "1000", "1250", "1500", "2000" }, TwitchManager.LeaderboardStats.GoodRewardAmount, null);
		this.setupComboBoxListString(this.comboHelperRewardTime, new string[] { "15", "30", "45", "60" }, TwitchManager.LeaderboardStats.GoodRewardTime, null);
		this.comboVoteDayTimeRange.OnValueChanged -= this.ComboVoteDayTimeRange_OnValueChanged;
		this.comboVoteDayTimeRange.Elements.Clear();
		for (int m = 0; m < this.twitchManager.VotingManager.VoteDayTimeRanges.Count; m++)
		{
			this.comboVoteDayTimeRange.Elements.Add(this.twitchManager.VotingManager.VoteDayTimeRanges[m].Name);
		}
		this.comboVoteDayTimeRange.SelectedIndex = this.twitchManager.VotingManager.CurrentVoteDayTimeRange;
		this.comboVoteDayTimeRange.OnValueChanged += this.ComboVoteDayTimeRange_OnValueChanged;
		this.setupComboBoxListString(this.comboVoteViewerDefeatReward, new string[] { "0", "100", "200", "250", "500", "1000", "2500", "5000" }, this.twitchManager.VotingManager.ViewerDefeatReward, null);
		this.comboActionSpamDelay.OnValueChanged -= this.ComboBoxListString_OnValueChanged;
		this.comboActionSpamDelay.Elements.Clear();
		this.comboActionSpamDelay.Elements.AddRange(new string[]
		{
			Localization.Get("xuiLightPropShadowsNone", false, null),
			"1",
			"2",
			"3",
			"4",
			"5"
		});
		this.comboActionSpamDelay.Value = this.comboActionSpamDelay.Elements[this.getActionSpamDelay(this.twitchManager.ViewerData.ActionSpamDelay)];
		this.comboActionSpamDelay.OnValueChanged += this.ComboBoxListString_OnValueChanged;
		this.comboBitPrices.OnValueChanged -= this.ComboBoxListString_OnValueChanged;
		this.comboBitPrices.Elements.Clear();
		this.comboBitPrices.Elements.AddRange(new string[]
		{
			Localization.Get("xuiDefault", false, null),
			"75%",
			"50%",
			"25%"
		});
		this.comboBitPrices.Value = this.comboBitPrices.Elements[this.getBitPriceModifier(this.twitchManager.BitPriceMultiplier)];
		this.comboBitPrices.OnValueChanged += this.ComboBoxListString_OnValueChanged;
		this.comboBitPotPercent.OnValueChanged -= this.ComboBoxListString_OnValueChanged;
		this.comboBitPotPercent.Elements.Clear();
		this.comboBitPotPercent.Elements.AddRange(new string[]
		{
			"100%", "95%", "90%", "85%", "80%", "75%", "70%", "65%", "60%", "55%",
			"50%", "45%", "40%", "35%", "30%", "25%", "20%", "15%", "10%", "0%"
		});
		this.comboBitPotPercent.Value = this.comboBitPotPercent.Elements[Mathf.FloorToInt((1f - this.twitchManager.BitPotPercentage) / 0.05f)];
		this.comboBitPotPercent.OnValueChanged += this.ComboBoxListString_OnValueChanged;
		if (this.tabs != null)
		{
			this.tabs.SelectedTabIndex = 0;
		}
		this.updateOptions();
		base.OnOpen();
		this.startedSinceOpened = false;
		XUi.InGameMenuOpen = true;
		this.IsDirty = true;
	}

	// Token: 0x06006B68 RID: 27496 RVA: 0x002A442C File Offset: 0x002A262C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupComboBoxListString(XUiC_ComboBoxList<string> _comboBox, string[] _values, int _currentValue, XUiC_ComboBox<string>.XUiEvent_ValueChanged _valueChanged = null)
	{
		if (_valueChanged == null)
		{
			_valueChanged = new XUiC_ComboBox<string>.XUiEvent_ValueChanged(this.ComboBoxListString_OnValueChanged);
		}
		_comboBox.OnValueChanged -= _valueChanged;
		_comboBox.Elements.Clear();
		_comboBox.Elements.AddRange(_values);
		_comboBox.Value = _comboBox.Elements[this.getIndexFromCombobox(_currentValue, _comboBox)];
		_comboBox.OnValueChanged += _valueChanged;
	}

	// Token: 0x06006B69 RID: 27497 RVA: 0x002A448B File Offset: 0x002A268B
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchManager_ConnectionStateChanged(TwitchManager.InitStates _oldState, TwitchManager.InitStates _newState)
	{
		base.RefreshBindings();
		if (_oldState != TwitchManager.InitStates.Ready && _newState == TwitchManager.InitStates.Ready)
		{
			this.localPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Enabled;
			this.comboOptOutTwitch.Value = true;
			this.origAllowTwitchOptions = true;
		}
	}

	// Token: 0x06006B6A RID: 27498 RVA: 0x002A44BC File Offset: 0x002A26BC
	public override void OnClose()
	{
		base.OnClose();
		this.twitchManager.UseActionsDuringBloodmoon = (int)this.origAllowActionsDuringBloodmoon;
		this.twitchManager.UseActionsDuringQuests = (int)this.origAllowActionsDuringQuests;
		this.twitchManager.ViewerData.PointRate = (float)this.origPointsGeneration;
		this.twitchManager.BitPriceMultiplier = this.setBitPriceModifier(this.origBitPricesIndex);
		this.twitchManager.BitPotPercentage = 1f - (float)this.origBitPotPercentIndex * 0.05f;
		this.twitchManager.SetCooldownPreset(this.origCooldownPresetIndex);
		this.twitchManager.SetUseProgression(this.origUseProgression);
		this.twitchManager.AllowCrateSharing = this.origAllowCrateSharing;
		this.twitchManager.ViewerData.ActionSpamDelay = (float)this.origActionSpamDelay;
		this.twitchManager.IntegrationSetting = this.origIntegrationSetting;
		TwitchVotingManager votingManager = this.twitchManager.VotingManager;
		bool allowChannelPointRedemptions = this.twitchManager.AllowChannelPointRedemptions;
		this.twitchManager.AllowBitEvents = this.origAllowBitEvents;
		this.twitchManager.AllowSubEvents = this.origAllowSubEvents;
		this.twitchManager.AllowGiftSubEvents = this.origAllowGiftSubEvents;
		this.twitchManager.AllowCharityEvents = this.origAllowCharityEvents;
		this.twitchManager.AllowRaidEvents = this.origAllowRaidEvents;
		this.twitchManager.AllowHypeTrainEvents = this.origAllowHypeTrainEvents;
		this.twitchManager.AllowCreatorGoalEvents = this.origAllowCreatorGoalEvents;
		this.twitchManager.AllowChannelPointRedemptions = this.origAllowChannelPointRedemptions;
		this.twitchManager.SetTwitchActionPreset(this.origActionPresetIndex);
		this.twitchManager.SetTwitchVotePreset(this.origVotePresetIndex);
		this.twitchManager.SetTwitchEventPreset(this.origEventPresetIndex, allowChannelPointRedemptions);
		this.twitchManager.ViewerData.StartingPoints = StringParsers.ParseSInt32(this.comboViewerStartingPoints.Elements[this.origViewerStartingPoints], 0, -1, NumberStyles.Integer);
		this.twitchManager.BitPointModifier = (int)this.origBitPointModifier;
		this.twitchManager.SubPointModifier = (int)this.origSubPointModifier;
		this.twitchManager.GiftSubPointModifier = (int)this.origGiftSubPointModifier;
		this.twitchManager.RaidPointAdd = StringParsers.ParseSInt32(this.comboRaidPointsAdd.Elements[this.origRaidPointAmountIndex], 0, -1, NumberStyles.Integer);
		this.twitchManager.RaidViewerMinimum = StringParsers.ParseSInt32(this.comboRaidViewerMinimum.Elements[this.origRaidViewerMinimumIndex], 0, -1, NumberStyles.Integer);
		TwitchManager.LeaderboardStats.GoodRewardAmount = StringParsers.ParseSInt32(this.comboHelperRewardAmount.Elements[this.origHelperRewardAmountIndex], 0, -1, NumberStyles.Integer);
		TwitchManager.LeaderboardStats.GoodRewardTime = StringParsers.ParseSInt32(this.comboHelperRewardTime.Elements[this.origHelperRewardTimeIndex], 0, -1, NumberStyles.Integer);
		votingManager.MaxDailyVotes = StringParsers.ParseSInt32(this.comboMaxDailyVotes.Elements[this.origMaxDailyVotes], 0, -1, NumberStyles.Integer);
		votingManager.VoteTime = StringParsers.ParseFloat(this.comboVoteTime.Elements[this.origVoteTime], 0, -1, NumberStyles.Any);
		votingManager.CurrentVoteDayTimeRange = this.origVoteDayTimeRange;
		votingManager.ViewerDefeatReward = StringParsers.ParseSInt32(this.comboVoteViewerDefeatReward.Elements[this.origVoteViewerDefeatReward], 0, -1, NumberStyles.Integer);
		votingManager.AllowVotesDuringBloodmoon = this.origAllowVotesDuringBloodmoon;
		votingManager.AllowVotesDuringQuests = this.origAllowVotesDuringQuests;
		votingManager.AllowVotesInSafeZone = this.origAllowVotesInSafeZone;
		this.twitchManager.HasDataChanges = true;
		if (this.origAllowTwitchOptions)
		{
			if (this.localPlayer.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.Disabled)
			{
				this.localPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Enabled;
			}
		}
		else if (this.localPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Disabled)
		{
			this.localPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Disabled;
		}
		this.localPlayer.TwitchVisionDisabled = !this.origAllowVisionEffects;
		this.twitchManager.ConnectionStateChanged -= this.TwitchManager_ConnectionStateChanged;
		if (this.twitchManager.CurrentCooldownPreset == null)
		{
			this.twitchManager.GetCooldownMax();
		}
		if (this.startedSinceOpened)
		{
			if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Startup && this.twitchManager.CurrentCooldownPreset.StartCooldownTime > 0)
			{
				this.twitchManager.SetCooldown((float)this.twitchManager.CurrentCooldownPreset.StartCooldownTime, TwitchManager.CooldownTypes.Startup, false, false);
			}
			this.twitchManager.InitialCooldownSet = true;
		}
		if (this.twitchManager.InitState == TwitchManager.InitStates.Ready && this.twitchManager.CurrentCooldownPreset.CooldownType != CooldownPreset.CooldownTypes.Fill)
		{
			this.twitchManager.SetCooldown(0f, TwitchManager.CooldownTypes.None, false, false);
		}
		this.twitchManager.ExtensionCheckTime = 0f;
		this.twitchManager = null;
		this.localPlayer = null;
		XUi.InGameMenuOpen = false;
	}

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x06006B6B RID: 27499 RVA: 0x002A4944 File Offset: 0x002A2B44
	public TwitchActionPreset ActionPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.ActionPresets[this.comboAllowActions.SelectedIndex];
		}
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x06006B6C RID: 27500 RVA: 0x002A4961 File Offset: 0x002A2B61
	public TwitchVotePreset VotePreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.VotePresets[this.comboAllowVotes.SelectedIndex];
		}
	}

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x06006B6D RID: 27501 RVA: 0x002A497E File Offset: 0x002A2B7E
	public TwitchEventPreset EventPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.EventPresets[this.comboAllowEvents.SelectedIndex];
		}
	}

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06006B6E RID: 27502 RVA: 0x002A499B File Offset: 0x002A2B9B
	[XuiXmlBinding("twitchstatus")]
	public string TwitchStatus
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return ((twitchManager != null) ? twitchManager.StateText : null) ?? "";
		}
	}

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06006B6F RID: 27503 RVA: 0x002A49B8 File Offset: 0x002A2BB8
	[XuiXmlBinding("twitchbuttontext")]
	public string TwitchButtonText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager == null)
			{
				return "";
			}
			if (this.twitchManager.InitState != TwitchManager.InitStates.Ready)
			{
				return Localization.Get("xuiOptionsTwitchLoginTwitch", false, null);
			}
			return Localization.Get("xuiOptionsTwitchDisconnect", false, null);
		}
	}

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06006B70 RID: 27504 RVA: 0x002A49EF File Offset: 0x002A2BEF
	[XuiXmlBinding("onlyconnected")]
	public bool OnlyConnected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready;
		}
	}

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x06006B71 RID: 27505 RVA: 0x002A4A05 File Offset: 0x002A2C05
	[XuiXmlBinding("notconnected")]
	public bool NotConnected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager == null || twitchManager.InitState != TwitchManager.InitStates.Ready;
		}
	}

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x06006B72 RID: 27506 RVA: 0x002A4A1E File Offset: 0x002A2C1E
	[XuiXmlBinding("notconnecting_console")]
	public bool NotConnectingConsole
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager == null || twitchManager.InitState != TwitchManager.InitStates.WaitingForOAuth || (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x06006B73 RID: 27507 RVA: 0x002A4A3F File Offset: 0x002A2C3F
	[XuiXmlBinding("connecting_console")]
	public bool ConnectingConsole
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.WaitingForOAuth && !(DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x06006B74 RID: 27508 RVA: 0x002A4A63 File Offset: 0x002A2C63
	[XuiXmlBinding("connecting_standalone")]
	public bool ConnectingStandalone
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.WaitingForOAuth && (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x06006B75 RID: 27509 RVA: 0x002A4A84 File Offset: 0x002A2C84
	[XuiXmlBinding("auth_devicecode")]
	public string AuthDeviceCode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.authCode ?? "";
		}
	}

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x06006B76 RID: 27510 RVA: 0x002A4A95 File Offset: 0x002A2C95
	[XuiXmlBinding("auth_verificationUrl")]
	public string AuthVerificationUrl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.authVerificationUrl ?? "";
		}
	}

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x06006B77 RID: 27511 RVA: 0x002A4AA6 File Offset: 0x002A2CA6
	[XuiXmlBinding("show_devicecode")]
	public string ShowDeviceCode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.showDeviceCode)
			{
				return Localization.Get("xuiOptionsTwitchDeviceCodeShow", false, null);
			}
			return this.authCode;
		}
	}

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x06006B78 RID: 27512 RVA: 0x002A4AC3 File Offset: 0x002A2CC3
	[XuiXmlBinding("hascustomevents")]
	public bool HasCustomEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.twitchManager.HasCustomEvents;
		}
	}

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x06006B79 RID: 27513 RVA: 0x002A4AE9 File Offset: 0x002A2CE9
	[XuiXmlBinding("allowactions")]
	public bool AllowActions
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && !this.ActionPreset.IsEmpty;
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x06006B7A RID: 27514 RVA: 0x002A4B12 File Offset: 0x002A2D12
	[XuiXmlBinding("allowvotes")]
	public bool AllowVotes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && !this.VotePreset.IsEmpty;
		}
	}

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x06006B7B RID: 27515 RVA: 0x002A4B3B File Offset: 0x002A2D3B
	[XuiXmlBinding("allowevents")]
	public bool AllowEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && !this.EventPreset.IsEmpty;
		}
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x06006B7C RID: 27516 RVA: 0x002A4B64 File Offset: 0x002A2D64
	[XuiXmlBinding("subvalues")]
	public string SubValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager != null)
			{
				return this.twitchManager.GetSubTierRewards(this.tempSubModifier);
			}
			return "";
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06006B7D RID: 27517 RVA: 0x002A4B85 File Offset: 0x002A2D85
	[XuiXmlBinding("giftsubvalues")]
	public string GiftSubValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager != null)
			{
				return this.twitchManager.GetGiftSubTierRewards(this.tempGiftSubModifier);
			}
			return "";
		}
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06006B7E RID: 27518 RVA: 0x002A4BA6 File Offset: 0x002A2DA6
	[XuiXmlBinding("votedaytimerange")]
	public string VoteDayTimeRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager != null)
			{
				return this.twitchManager.VotingManager.GetDayTimeRange(this.tempVoteDayTimeRange);
			}
			return "";
		}
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06006B7F RID: 27519 RVA: 0x002A4BCC File Offset: 0x002A2DCC
	[XuiXmlBinding("action_description")]
	public string ActionDescription
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager != null)
			{
				return "[DECEA3]" + this.ActionPreset.Title + "[-]\n" + this.ActionPreset.Description;
			}
			return "";
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x06006B80 RID: 27520 RVA: 0x002A4C01 File Offset: 0x002A2E01
	[XuiXmlBinding("vote_description")]
	public string VoteDescription
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager != null)
			{
				return "[DECEA3]" + this.VotePreset.Title + "[-]\n" + this.VotePreset.Description;
			}
			return "";
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06006B81 RID: 27521 RVA: 0x002A4C36 File Offset: 0x002A2E36
	[XuiXmlBinding("event_description")]
	public string EventDescription
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.twitchManager != null)
			{
				return "[DECEA3]" + this.EventPreset.Title + "[-]\n" + this.EventPreset.Description;
			}
			return "";
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06006B82 RID: 27522 RVA: 0x002A4C6B File Offset: 0x002A2E6B
	[XuiXmlBinding("hasbitevents")]
	public bool HasBitEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasBitEvents;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06006B83 RID: 27523 RVA: 0x002A4C91 File Offset: 0x002A2E91
	[XuiXmlBinding("hassubevents")]
	public bool HasSubEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasSubEvents;
		}
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06006B84 RID: 27524 RVA: 0x002A4CB7 File Offset: 0x002A2EB7
	[XuiXmlBinding("hasgiftsubevents")]
	public bool HasGiftSubEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasGiftSubEvents;
		}
	}

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06006B85 RID: 27525 RVA: 0x002A4CDD File Offset: 0x002A2EDD
	[XuiXmlBinding("hascharityevents")]
	public bool HasCharityEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasCharityEvents;
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x06006B86 RID: 27526 RVA: 0x002A4D03 File Offset: 0x002A2F03
	[XuiXmlBinding("hasraidevents")]
	public bool HasRaidEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasRaidEvents;
		}
	}

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x06006B87 RID: 27527 RVA: 0x002A4D29 File Offset: 0x002A2F29
	[XuiXmlBinding("hashypetrainevents")]
	public bool HasHypeTrainEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasHypeTrainEvents;
		}
	}

	// Token: 0x17000BDF RID: 3039
	// (get) Token: 0x06006B88 RID: 27528 RVA: 0x002A4D4F File Offset: 0x002A2F4F
	[XuiXmlBinding("hascreatorgoalevents")]
	public bool HasCreatorGoalEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasCreatorGoalEvents;
		}
	}

	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x06006B89 RID: 27529 RVA: 0x002A4D75 File Offset: 0x002A2F75
	[XuiXmlBinding("haschannelpointevents")]
	public bool HasChannelPointEvents
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TwitchManager twitchManager = this.twitchManager;
			return twitchManager != null && twitchManager.InitState == TwitchManager.InitStates.Ready && this.EventPreset.HasChannelPointEvents;
		}
	}

	// Token: 0x040051F9 RID: 20985
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x040051FA RID: 20986
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector tabs;

	// Token: 0x040051FB RID: 20987
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboOptOutTwitch;

	// Token: 0x040051FC RID: 20988
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboUseProgression;

	// Token: 0x040051FD RID: 20989
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCrateSharing;

	// Token: 0x040051FE RID: 20990
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions> comboAllowActionsDuringBloodmoon;

	// Token: 0x040051FF RID: 20991
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions> comboAllowActionsDuringQuests;

	// Token: 0x04005200 RID: 20992
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesDuringBloodmoon;

	// Token: 0x04005201 RID: 20993
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesDuringQuests;

	// Token: 0x04005202 RID: 20994
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesInSafeZone;

	// Token: 0x04005203 RID: 20995
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboPointsGeneration;

	// Token: 0x04005204 RID: 20996
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboCooldownPreset;

	// Token: 0x04005205 RID: 20997
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVisionEffects;

	// Token: 0x04005206 RID: 20998
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboActionSpamDelay;

	// Token: 0x04005207 RID: 20999
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<TwitchManager.IntegrationSettings> comboActionTwitchIntegrationType;

	// Token: 0x04005208 RID: 21000
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowActions;

	// Token: 0x04005209 RID: 21001
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowVotes;

	// Token: 0x0400520A RID: 21002
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowEvents;

	// Token: 0x0400520B RID: 21003
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboViewerStartingPoints;

	// Token: 0x0400520C RID: 21004
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.BitAddOptions> comboBitPointsAdd;

	// Token: 0x0400520D RID: 21005
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboSubPointsAdd;

	// Token: 0x0400520E RID: 21006
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboGiftSubPointsAdd;

	// Token: 0x0400520F RID: 21007
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboRaidPointsAdd;

	// Token: 0x04005210 RID: 21008
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboRaidViewerMinimum;

	// Token: 0x04005211 RID: 21009
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboMaxDailyVotes;

	// Token: 0x04005212 RID: 21010
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteTime;

	// Token: 0x04005213 RID: 21011
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteViewerDefeatReward;

	// Token: 0x04005214 RID: 21012
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteDayTimeRange;

	// Token: 0x04005215 RID: 21013
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboHelperRewardAmount;

	// Token: 0x04005216 RID: 21014
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboHelperRewardTime;

	// Token: 0x04005217 RID: 21015
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowBitEvents;

	// Token: 0x04005218 RID: 21016
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowSubEvents;

	// Token: 0x04005219 RID: 21017
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowGiftSubEvents;

	// Token: 0x0400521A RID: 21018
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCharityEvents;

	// Token: 0x0400521B RID: 21019
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowRaidEvents;

	// Token: 0x0400521C RID: 21020
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowHypeTrainEvents;

	// Token: 0x0400521D RID: 21021
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCreatorGoalEvents;

	// Token: 0x0400521E RID: 21022
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowChannelPointRedeemEvents;

	// Token: 0x0400521F RID: 21023
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboBitPrices;

	// Token: 0x04005220 RID: 21024
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboBitPotPercent;

	// Token: 0x04005221 RID: 21025
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.TwitchBloodMoonOptions origAllowActionsDuringBloodmoon;

	// Token: 0x04005222 RID: 21026
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.TwitchBloodMoonOptions origAllowActionsDuringQuests;

	// Token: 0x04005223 RID: 21027
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origPointsGeneration;

	// Token: 0x04005224 RID: 21028
	[PublicizedFrom(EAccessModifier.Private)]
	public int origCooldownPresetIndex;

	// Token: 0x04005225 RID: 21029
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origUseProgression;

	// Token: 0x04005226 RID: 21030
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowTwitchOptions;

	// Token: 0x04005227 RID: 21031
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVisionEffects = true;

	// Token: 0x04005228 RID: 21032
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCrateSharing;

	// Token: 0x04005229 RID: 21033
	[PublicizedFrom(EAccessModifier.Private)]
	public int origActionSpamDelay = 1;

	// Token: 0x0400522A RID: 21034
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager.IntegrationSettings origIntegrationSetting;

	// Token: 0x0400522B RID: 21035
	[PublicizedFrom(EAccessModifier.Private)]
	public int origActionPresetIndex;

	// Token: 0x0400522C RID: 21036
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVotePresetIndex;

	// Token: 0x0400522D RID: 21037
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEventPresetIndex;

	// Token: 0x0400522E RID: 21038
	[PublicizedFrom(EAccessModifier.Private)]
	public int origBitPricesIndex;

	// Token: 0x0400522F RID: 21039
	[PublicizedFrom(EAccessModifier.Private)]
	public int origBitPotPercentIndex;

	// Token: 0x04005230 RID: 21040
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.BitAddOptions origBitPointModifier;

	// Token: 0x04005231 RID: 21041
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origSubPointModifier;

	// Token: 0x04005232 RID: 21042
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origGiftSubPointModifier;

	// Token: 0x04005233 RID: 21043
	[PublicizedFrom(EAccessModifier.Private)]
	public int origRaidPointAmountIndex;

	// Token: 0x04005234 RID: 21044
	[PublicizedFrom(EAccessModifier.Private)]
	public int origRaidViewerMinimumIndex;

	// Token: 0x04005235 RID: 21045
	[PublicizedFrom(EAccessModifier.Private)]
	public int origHelperRewardAmountIndex;

	// Token: 0x04005236 RID: 21046
	[PublicizedFrom(EAccessModifier.Private)]
	public int origHelperRewardTimeIndex;

	// Token: 0x04005237 RID: 21047
	[PublicizedFrom(EAccessModifier.Private)]
	public int origViewerStartingPoints;

	// Token: 0x04005238 RID: 21048
	[PublicizedFrom(EAccessModifier.Private)]
	public int origMaxDailyVotes;

	// Token: 0x04005239 RID: 21049
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteTime;

	// Token: 0x0400523A RID: 21050
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteDayTimeRange;

	// Token: 0x0400523B RID: 21051
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteViewerDefeatReward;

	// Token: 0x0400523C RID: 21052
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesDuringBloodmoon;

	// Token: 0x0400523D RID: 21053
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesDuringQuests;

	// Token: 0x0400523E RID: 21054
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesInSafeZone;

	// Token: 0x0400523F RID: 21055
	[PublicizedFrom(EAccessModifier.Private)]
	public bool startedSinceOpened;

	// Token: 0x04005240 RID: 21056
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager twitchManager;

	// Token: 0x04005241 RID: 21057
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempSubModifier = 1;

	// Token: 0x04005242 RID: 21058
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempGiftSubModifier = 1;

	// Token: 0x04005243 RID: 21059
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempVoteDayTimeRange = 2;

	// Token: 0x04005244 RID: 21060
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowBitEvents = true;

	// Token: 0x04005245 RID: 21061
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowSubEvents = true;

	// Token: 0x04005246 RID: 21062
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowGiftSubEvents = true;

	// Token: 0x04005247 RID: 21063
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCharityEvents = true;

	// Token: 0x04005248 RID: 21064
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowRaidEvents = true;

	// Token: 0x04005249 RID: 21065
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowHypeTrainEvents = true;

	// Token: 0x0400524A RID: 21066
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCreatorGoalEvents = true;

	// Token: 0x0400524B RID: 21067
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowChannelPointRedemptions = true;

	// Token: 0x0400524C RID: 21068
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showDeviceCode;

	// Token: 0x0400524D RID: 21069
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture qrCodeTexControl;

	// Token: 0x0400524E RID: 21070
	[PublicizedFrom(EAccessModifier.Private)]
	public string authCode = "";

	// Token: 0x0400524F RID: 21071
	[PublicizedFrom(EAccessModifier.Private)]
	public string authVerificationUrl = "";

	// Token: 0x02000DCD RID: 3533
	public enum TwitchBloodMoonOptions
	{
		// Token: 0x04005251 RID: 21073
		Disabled,
		// Token: 0x04005252 RID: 21074
		Standard,
		// Token: 0x04005253 RID: 21075
		CooldownOnly
	}

	// Token: 0x02000DCE RID: 3534
	public enum PointsGenerationOptions
	{
		// Token: 0x04005255 RID: 21077
		Disabled,
		// Token: 0x04005256 RID: 21078
		Standard,
		// Token: 0x04005257 RID: 21079
		Double,
		// Token: 0x04005258 RID: 21080
		Triple
	}

	// Token: 0x02000DCF RID: 3535
	public enum BitAddOptions
	{
		// Token: 0x0400525A RID: 21082
		Standard = 1,
		// Token: 0x0400525B RID: 21083
		Double,
		// Token: 0x0400525C RID: 21084
		Triple
	}
}

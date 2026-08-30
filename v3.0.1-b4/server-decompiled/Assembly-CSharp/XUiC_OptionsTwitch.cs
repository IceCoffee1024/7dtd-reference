using System;
using System.Collections.Generic;
using System.Globalization;
using Challenges;
using Platform;
using Twitch;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D9E RID: 3486
[Preserve]
public class XUiC_OptionsTwitch : XUiC_OptionsDialogBase
{
	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06006A96 RID: 27286 RVA: 0x0029A9F4 File Offset: 0x00298BF4
	// (remove) Token: 0x06006A97 RID: 27287 RVA: 0x0029AA28 File Offset: 0x00298C28
	public static event Action OnSettingsChanged;

	// Token: 0x06006A98 RID: 27288 RVA: 0x0029AA5C File Offset: 0x00298C5C
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

	// Token: 0x06006A99 RID: 27289 RVA: 0x0029B159 File Offset: 0x00299359
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowDeviceCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.showDeviceCode = !this.showDeviceCode;
		base.RefreshBindings();
	}

	// Token: 0x06006A9A RID: 27290 RVA: 0x0029B170 File Offset: 0x00299370
	[PublicizedFrom(EAccessModifier.Private)]
	public void Combo_OnValueChangedGeneric(XUiController _sender)
	{
		base.SetChanged();
	}

	// Token: 0x06006A9B RID: 27291 RVA: 0x0029B178 File Offset: 0x00299378
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tabs_OnTabChanged(int _i, XUiC_TabSelectorTab _tab)
	{
		if (_i != 0 && this.twitchManager.InitState != TwitchManager.InitStates.Ready)
		{
			this.tabs.SelectedTabIndex = 0;
		}
		this.IsDirty = true;
	}

	// Token: 0x06006A9C RID: 27292 RVA: 0x0029B170 File Offset: 0x00299370
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboBoxListString_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
	}

	// Token: 0x06006A9D RID: 27293 RVA: 0x0029B170 File Offset: 0x00299370
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboPreset_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
	}

	// Token: 0x06006A9E RID: 27294 RVA: 0x0029B19E File Offset: 0x0029939E
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboVoteDayTimeRange_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
		this.tempVoteDayTimeRange = this.comboVoteDayTimeRange.SelectedIndex;
	}

	// Token: 0x06006A9F RID: 27295 RVA: 0x0029B1B7 File Offset: 0x002993B7
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubPoints_OnValueChanged(XUiController _sender, XUiC_OptionsTwitch.PointsGenerationOptions _oldValue, XUiC_OptionsTwitch.PointsGenerationOptions _newValue)
	{
		base.SetChanged();
		this.tempSubModifier = (int)_newValue;
	}

	// Token: 0x06006AA0 RID: 27296 RVA: 0x0029B1C6 File Offset: 0x002993C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void GiftSubPoints_OnValueChanged(XUiController _sender, XUiC_OptionsTwitch.PointsGenerationOptions _oldValue, XUiC_OptionsTwitch.PointsGenerationOptions _newValue)
	{
		base.SetChanged();
		this.IsDirty = true;
		this.tempGiftSubModifier = (int)_newValue;
	}

	// Token: 0x06006AA1 RID: 27297 RVA: 0x0029B1DC File Offset: 0x002993DC
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

	// Token: 0x06006AA2 RID: 27298 RVA: 0x0029B2C4 File Offset: 0x002994C4
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

	// Token: 0x06006AA3 RID: 27299 RVA: 0x0029B330 File Offset: 0x00299530
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

	// Token: 0x06006AA4 RID: 27300 RVA: 0x0029B418 File Offset: 0x00299618
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

	// Token: 0x06006AA5 RID: 27301 RVA: 0x0029B4EC File Offset: 0x002996EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnResetPrices_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.twitchManager.ResetPricesToDefault();
	}

	// Token: 0x06006AA6 RID: 27302 RVA: 0x0029B4F9 File Offset: 0x002996F9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		this.applyChanges();
	}

	// Token: 0x06006AA7 RID: 27303 RVA: 0x0029B508 File Offset: 0x00299708
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

	// Token: 0x06006AA8 RID: 27304 RVA: 0x0029B680 File Offset: 0x00299880
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchAuthentication_QRCodeGenerated(Texture2D _qrCodeTex, string _userCode, string _verificationUrl)
	{
		this.qrCodeTexControl.Texture = _qrCodeTex;
		this.authCode = _userCode;
		this.authVerificationUrl = _verificationUrl;
		base.RefreshBindings();
	}

	// Token: 0x06006AA9 RID: 27305 RVA: 0x0029B6A4 File Offset: 0x002998A4
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

	// Token: 0x06006AAA RID: 27306 RVA: 0x0029BC48 File Offset: 0x00299E48
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

	// Token: 0x06006AAB RID: 27307 RVA: 0x0029BC8C File Offset: 0x00299E8C
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

	// Token: 0x06006AAC RID: 27308 RVA: 0x0029BCF0 File Offset: 0x00299EF0
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

	// Token: 0x06006AAD RID: 27309 RVA: 0x0029BD3C File Offset: 0x00299F3C
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

	// Token: 0x06006AAE RID: 27310 RVA: 0x0029BD88 File Offset: 0x00299F88
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

	// Token: 0x06006AAF RID: 27311 RVA: 0x0029C02C File Offset: 0x0029A22C
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

	// Token: 0x06006AB0 RID: 27312 RVA: 0x0029CA14 File Offset: 0x0029AC14
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

	// Token: 0x06006AB1 RID: 27313 RVA: 0x0029CA73 File Offset: 0x0029AC73
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

	// Token: 0x06006AB2 RID: 27314 RVA: 0x0029CAA4 File Offset: 0x0029ACA4
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

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x06006AB3 RID: 27315 RVA: 0x0029CF2C File Offset: 0x0029B12C
	public TwitchActionPreset ActionPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.ActionPresets[this.comboAllowActions.SelectedIndex];
		}
	}

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x06006AB4 RID: 27316 RVA: 0x0029CF49 File Offset: 0x0029B149
	public TwitchVotePreset VotePreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.VotePresets[this.comboAllowVotes.SelectedIndex];
		}
	}

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x0029CF66 File Offset: 0x0029B166
	public TwitchEventPreset EventPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.EventPresets[this.comboAllowEvents.SelectedIndex];
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x06006AB6 RID: 27318 RVA: 0x0029CF83 File Offset: 0x0029B183
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

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x06006AB7 RID: 27319 RVA: 0x0029CFA0 File Offset: 0x0029B1A0
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

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x06006AB8 RID: 27320 RVA: 0x0029CFD7 File Offset: 0x0029B1D7
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

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x06006AB9 RID: 27321 RVA: 0x0029CFED File Offset: 0x0029B1ED
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

	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x06006ABA RID: 27322 RVA: 0x0029D006 File Offset: 0x0029B206
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

	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x06006ABB RID: 27323 RVA: 0x0029D027 File Offset: 0x0029B227
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

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x06006ABC RID: 27324 RVA: 0x0029D04B File Offset: 0x0029B24B
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

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x06006ABD RID: 27325 RVA: 0x0029D06C File Offset: 0x0029B26C
	[XuiXmlBinding("auth_devicecode")]
	public string AuthDeviceCode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.authCode ?? "";
		}
	}

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06006ABE RID: 27326 RVA: 0x0029D07D File Offset: 0x0029B27D
	[XuiXmlBinding("auth_verificationUrl")]
	public string AuthVerificationUrl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.authVerificationUrl ?? "";
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06006ABF RID: 27327 RVA: 0x0029D08E File Offset: 0x0029B28E
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

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06006AC0 RID: 27328 RVA: 0x0029D0AB File Offset: 0x0029B2AB
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

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06006AC1 RID: 27329 RVA: 0x0029D0D1 File Offset: 0x0029B2D1
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

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x0029D0FA File Offset: 0x0029B2FA
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

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06006AC3 RID: 27331 RVA: 0x0029D123 File Offset: 0x0029B323
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

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x06006AC4 RID: 27332 RVA: 0x0029D14C File Offset: 0x0029B34C
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

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x0029D16D File Offset: 0x0029B36D
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

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x06006AC6 RID: 27334 RVA: 0x0029D18E File Offset: 0x0029B38E
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

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x06006AC7 RID: 27335 RVA: 0x0029D1B4 File Offset: 0x0029B3B4
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

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x06006AC8 RID: 27336 RVA: 0x0029D1E9 File Offset: 0x0029B3E9
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

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x06006AC9 RID: 27337 RVA: 0x0029D21E File Offset: 0x0029B41E
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

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06006ACA RID: 27338 RVA: 0x0029D253 File Offset: 0x0029B453
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

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06006ACB RID: 27339 RVA: 0x0029D279 File Offset: 0x0029B479
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

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06006ACC RID: 27340 RVA: 0x0029D29F File Offset: 0x0029B49F
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

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06006ACD RID: 27341 RVA: 0x0029D2C5 File Offset: 0x0029B4C5
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

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x06006ACE RID: 27342 RVA: 0x0029D2EB File Offset: 0x0029B4EB
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

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06006ACF RID: 27343 RVA: 0x0029D311 File Offset: 0x0029B511
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

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x0029D337 File Offset: 0x0029B537
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

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x0029D35D File Offset: 0x0029B55D
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

	// Token: 0x040050FB RID: 20731
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x040050FC RID: 20732
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector tabs;

	// Token: 0x040050FD RID: 20733
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboOptOutTwitch;

	// Token: 0x040050FE RID: 20734
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboUseProgression;

	// Token: 0x040050FF RID: 20735
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCrateSharing;

	// Token: 0x04005100 RID: 20736
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions> comboAllowActionsDuringBloodmoon;

	// Token: 0x04005101 RID: 20737
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions> comboAllowActionsDuringQuests;

	// Token: 0x04005102 RID: 20738
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesDuringBloodmoon;

	// Token: 0x04005103 RID: 20739
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesDuringQuests;

	// Token: 0x04005104 RID: 20740
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesInSafeZone;

	// Token: 0x04005105 RID: 20741
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboPointsGeneration;

	// Token: 0x04005106 RID: 20742
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboCooldownPreset;

	// Token: 0x04005107 RID: 20743
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVisionEffects;

	// Token: 0x04005108 RID: 20744
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboActionSpamDelay;

	// Token: 0x04005109 RID: 20745
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<TwitchManager.IntegrationSettings> comboActionTwitchIntegrationType;

	// Token: 0x0400510A RID: 20746
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowActions;

	// Token: 0x0400510B RID: 20747
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowVotes;

	// Token: 0x0400510C RID: 20748
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowEvents;

	// Token: 0x0400510D RID: 20749
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboViewerStartingPoints;

	// Token: 0x0400510E RID: 20750
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.BitAddOptions> comboBitPointsAdd;

	// Token: 0x0400510F RID: 20751
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboSubPointsAdd;

	// Token: 0x04005110 RID: 20752
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboGiftSubPointsAdd;

	// Token: 0x04005111 RID: 20753
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboRaidPointsAdd;

	// Token: 0x04005112 RID: 20754
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboRaidViewerMinimum;

	// Token: 0x04005113 RID: 20755
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboMaxDailyVotes;

	// Token: 0x04005114 RID: 20756
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteTime;

	// Token: 0x04005115 RID: 20757
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteViewerDefeatReward;

	// Token: 0x04005116 RID: 20758
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteDayTimeRange;

	// Token: 0x04005117 RID: 20759
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboHelperRewardAmount;

	// Token: 0x04005118 RID: 20760
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboHelperRewardTime;

	// Token: 0x04005119 RID: 20761
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowBitEvents;

	// Token: 0x0400511A RID: 20762
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowSubEvents;

	// Token: 0x0400511B RID: 20763
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowGiftSubEvents;

	// Token: 0x0400511C RID: 20764
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCharityEvents;

	// Token: 0x0400511D RID: 20765
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowRaidEvents;

	// Token: 0x0400511E RID: 20766
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowHypeTrainEvents;

	// Token: 0x0400511F RID: 20767
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCreatorGoalEvents;

	// Token: 0x04005120 RID: 20768
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowChannelPointRedeemEvents;

	// Token: 0x04005121 RID: 20769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboBitPrices;

	// Token: 0x04005122 RID: 20770
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboBitPotPercent;

	// Token: 0x04005123 RID: 20771
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.TwitchBloodMoonOptions origAllowActionsDuringBloodmoon;

	// Token: 0x04005124 RID: 20772
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.TwitchBloodMoonOptions origAllowActionsDuringQuests;

	// Token: 0x04005125 RID: 20773
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origPointsGeneration;

	// Token: 0x04005126 RID: 20774
	[PublicizedFrom(EAccessModifier.Private)]
	public int origCooldownPresetIndex;

	// Token: 0x04005127 RID: 20775
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origUseProgression;

	// Token: 0x04005128 RID: 20776
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowTwitchOptions;

	// Token: 0x04005129 RID: 20777
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVisionEffects = true;

	// Token: 0x0400512A RID: 20778
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCrateSharing;

	// Token: 0x0400512B RID: 20779
	[PublicizedFrom(EAccessModifier.Private)]
	public int origActionSpamDelay = 1;

	// Token: 0x0400512C RID: 20780
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager.IntegrationSettings origIntegrationSetting;

	// Token: 0x0400512D RID: 20781
	[PublicizedFrom(EAccessModifier.Private)]
	public int origActionPresetIndex;

	// Token: 0x0400512E RID: 20782
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVotePresetIndex;

	// Token: 0x0400512F RID: 20783
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEventPresetIndex;

	// Token: 0x04005130 RID: 20784
	[PublicizedFrom(EAccessModifier.Private)]
	public int origBitPricesIndex;

	// Token: 0x04005131 RID: 20785
	[PublicizedFrom(EAccessModifier.Private)]
	public int origBitPotPercentIndex;

	// Token: 0x04005132 RID: 20786
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.BitAddOptions origBitPointModifier;

	// Token: 0x04005133 RID: 20787
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origSubPointModifier;

	// Token: 0x04005134 RID: 20788
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origGiftSubPointModifier;

	// Token: 0x04005135 RID: 20789
	[PublicizedFrom(EAccessModifier.Private)]
	public int origRaidPointAmountIndex;

	// Token: 0x04005136 RID: 20790
	[PublicizedFrom(EAccessModifier.Private)]
	public int origRaidViewerMinimumIndex;

	// Token: 0x04005137 RID: 20791
	[PublicizedFrom(EAccessModifier.Private)]
	public int origHelperRewardAmountIndex;

	// Token: 0x04005138 RID: 20792
	[PublicizedFrom(EAccessModifier.Private)]
	public int origHelperRewardTimeIndex;

	// Token: 0x04005139 RID: 20793
	[PublicizedFrom(EAccessModifier.Private)]
	public int origViewerStartingPoints;

	// Token: 0x0400513A RID: 20794
	[PublicizedFrom(EAccessModifier.Private)]
	public int origMaxDailyVotes;

	// Token: 0x0400513B RID: 20795
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteTime;

	// Token: 0x0400513C RID: 20796
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteDayTimeRange;

	// Token: 0x0400513D RID: 20797
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteViewerDefeatReward;

	// Token: 0x0400513E RID: 20798
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesDuringBloodmoon;

	// Token: 0x0400513F RID: 20799
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesDuringQuests;

	// Token: 0x04005140 RID: 20800
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesInSafeZone;

	// Token: 0x04005141 RID: 20801
	[PublicizedFrom(EAccessModifier.Private)]
	public bool startedSinceOpened;

	// Token: 0x04005142 RID: 20802
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager twitchManager;

	// Token: 0x04005143 RID: 20803
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempSubModifier = 1;

	// Token: 0x04005144 RID: 20804
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempGiftSubModifier = 1;

	// Token: 0x04005145 RID: 20805
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempVoteDayTimeRange = 2;

	// Token: 0x04005146 RID: 20806
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowBitEvents = true;

	// Token: 0x04005147 RID: 20807
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowSubEvents = true;

	// Token: 0x04005148 RID: 20808
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowGiftSubEvents = true;

	// Token: 0x04005149 RID: 20809
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCharityEvents = true;

	// Token: 0x0400514A RID: 20810
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowRaidEvents = true;

	// Token: 0x0400514B RID: 20811
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowHypeTrainEvents = true;

	// Token: 0x0400514C RID: 20812
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCreatorGoalEvents = true;

	// Token: 0x0400514D RID: 20813
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowChannelPointRedemptions = true;

	// Token: 0x0400514E RID: 20814
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showDeviceCode;

	// Token: 0x0400514F RID: 20815
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture qrCodeTexControl;

	// Token: 0x04005150 RID: 20816
	[PublicizedFrom(EAccessModifier.Private)]
	public string authCode = "";

	// Token: 0x04005151 RID: 20817
	[PublicizedFrom(EAccessModifier.Private)]
	public string authVerificationUrl = "";

	// Token: 0x02000D9F RID: 3487
	public enum TwitchBloodMoonOptions
	{
		// Token: 0x04005153 RID: 20819
		Disabled,
		// Token: 0x04005154 RID: 20820
		Standard,
		// Token: 0x04005155 RID: 20821
		CooldownOnly
	}

	// Token: 0x02000DA0 RID: 3488
	public enum PointsGenerationOptions
	{
		// Token: 0x04005157 RID: 20823
		Disabled,
		// Token: 0x04005158 RID: 20824
		Standard,
		// Token: 0x04005159 RID: 20825
		Double,
		// Token: 0x0400515A RID: 20826
		Triple
	}

	// Token: 0x02000DA1 RID: 3489
	public enum BitAddOptions
	{
		// Token: 0x0400515C RID: 20828
		Standard = 1,
		// Token: 0x0400515D RID: 20829
		Double,
		// Token: 0x0400515E RID: 20830
		Triple
	}
}

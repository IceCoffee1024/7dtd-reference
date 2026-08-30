using System;
using System.Collections.Generic;
using System.Globalization;
using Challenges;
using Platform;
using Twitch;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DB2 RID: 3506
[Preserve]
public class XUiC_OptionsTwitch : XUiC_OptionsDialogBase
{
	// Token: 0x14000096 RID: 150
	// (add) Token: 0x06006B31 RID: 27441 RVA: 0x0029DB54 File Offset: 0x0029BD54
	// (remove) Token: 0x06006B32 RID: 27442 RVA: 0x0029DB88 File Offset: 0x0029BD88
	public static event Action OnSettingsChanged;

	// Token: 0x06006B33 RID: 27443 RVA: 0x0029DBBC File Offset: 0x0029BDBC
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

	// Token: 0x06006B34 RID: 27444 RVA: 0x0029E2B9 File Offset: 0x0029C4B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnShowDeviceCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.showDeviceCode = !this.showDeviceCode;
		base.RefreshBindings();
	}

	// Token: 0x06006B35 RID: 27445 RVA: 0x0029E2D0 File Offset: 0x0029C4D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Combo_OnValueChangedGeneric(XUiController _sender)
	{
		base.SetChanged();
	}

	// Token: 0x06006B36 RID: 27446 RVA: 0x0029E2D8 File Offset: 0x0029C4D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tabs_OnTabChanged(int _i, XUiC_TabSelectorTab _tab)
	{
		if (_i != 0 && this.twitchManager.InitState != TwitchManager.InitStates.Ready)
		{
			this.tabs.SelectedTabIndex = 0;
		}
		this.IsDirty = true;
	}

	// Token: 0x06006B37 RID: 27447 RVA: 0x0029E2D0 File Offset: 0x0029C4D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboBoxListString_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
	}

	// Token: 0x06006B38 RID: 27448 RVA: 0x0029E2D0 File Offset: 0x0029C4D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboPreset_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
	}

	// Token: 0x06006B39 RID: 27449 RVA: 0x0029E2FE File Offset: 0x0029C4FE
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboVoteDayTimeRange_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		base.SetChanged();
		this.tempVoteDayTimeRange = this.comboVoteDayTimeRange.SelectedIndex;
	}

	// Token: 0x06006B3A RID: 27450 RVA: 0x0029E317 File Offset: 0x0029C517
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubPoints_OnValueChanged(XUiController _sender, XUiC_OptionsTwitch.PointsGenerationOptions _oldValue, XUiC_OptionsTwitch.PointsGenerationOptions _newValue)
	{
		base.SetChanged();
		this.tempSubModifier = (int)_newValue;
	}

	// Token: 0x06006B3B RID: 27451 RVA: 0x0029E326 File Offset: 0x0029C526
	[PublicizedFrom(EAccessModifier.Private)]
	public void GiftSubPoints_OnValueChanged(XUiController _sender, XUiC_OptionsTwitch.PointsGenerationOptions _oldValue, XUiC_OptionsTwitch.PointsGenerationOptions _newValue)
	{
		base.SetChanged();
		this.IsDirty = true;
		this.tempGiftSubModifier = (int)_newValue;
	}

	// Token: 0x06006B3C RID: 27452 RVA: 0x0029E33C File Offset: 0x0029C53C
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

	// Token: 0x06006B3D RID: 27453 RVA: 0x0029E424 File Offset: 0x0029C624
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

	// Token: 0x06006B3E RID: 27454 RVA: 0x0029E490 File Offset: 0x0029C690
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

	// Token: 0x06006B3F RID: 27455 RVA: 0x0029E578 File Offset: 0x0029C778
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

	// Token: 0x06006B40 RID: 27456 RVA: 0x0029E64C File Offset: 0x0029C84C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnResetPrices_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.twitchManager.ResetPricesToDefault();
	}

	// Token: 0x06006B41 RID: 27457 RVA: 0x0029E659 File Offset: 0x0029C859
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void doSaveChangesInternal()
	{
		base.doSaveChangesInternal();
		this.applyChanges();
	}

	// Token: 0x06006B42 RID: 27458 RVA: 0x0029E668 File Offset: 0x0029C868
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

	// Token: 0x06006B43 RID: 27459 RVA: 0x0029E7E0 File Offset: 0x0029C9E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchAuthentication_QRCodeGenerated(Texture2D _qrCodeTex, string _userCode, string _verificationUrl)
	{
		this.qrCodeTexControl.Texture = _qrCodeTex;
		this.authCode = _userCode;
		this.authVerificationUrl = _verificationUrl;
		base.RefreshBindings();
	}

	// Token: 0x06006B44 RID: 27460 RVA: 0x0029E804 File Offset: 0x0029CA04
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

	// Token: 0x06006B45 RID: 27461 RVA: 0x0029EDA8 File Offset: 0x0029CFA8
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

	// Token: 0x06006B46 RID: 27462 RVA: 0x0029EDEC File Offset: 0x0029CFEC
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

	// Token: 0x06006B47 RID: 27463 RVA: 0x0029EE50 File Offset: 0x0029D050
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

	// Token: 0x06006B48 RID: 27464 RVA: 0x0029EE9C File Offset: 0x0029D09C
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

	// Token: 0x06006B49 RID: 27465 RVA: 0x0029EEE8 File Offset: 0x0029D0E8
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

	// Token: 0x06006B4A RID: 27466 RVA: 0x0029F18C File Offset: 0x0029D38C
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

	// Token: 0x06006B4B RID: 27467 RVA: 0x0029FB74 File Offset: 0x0029DD74
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

	// Token: 0x06006B4C RID: 27468 RVA: 0x0029FBD3 File Offset: 0x0029DDD3
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

	// Token: 0x06006B4D RID: 27469 RVA: 0x0029FC04 File Offset: 0x0029DE04
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

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06006B4E RID: 27470 RVA: 0x002A008C File Offset: 0x0029E28C
	public TwitchActionPreset ActionPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.ActionPresets[this.comboAllowActions.SelectedIndex];
		}
	}

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06006B4F RID: 27471 RVA: 0x002A00A9 File Offset: 0x0029E2A9
	public TwitchVotePreset VotePreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.VotePresets[this.comboAllowVotes.SelectedIndex];
		}
	}

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06006B50 RID: 27472 RVA: 0x002A00C6 File Offset: 0x0029E2C6
	public TwitchEventPreset EventPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.twitchManager.EventPresets[this.comboAllowEvents.SelectedIndex];
		}
	}

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x06006B51 RID: 27473 RVA: 0x002A00E3 File Offset: 0x0029E2E3
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

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06006B52 RID: 27474 RVA: 0x002A0100 File Offset: 0x0029E300
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

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x06006B53 RID: 27475 RVA: 0x002A0137 File Offset: 0x0029E337
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

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x06006B54 RID: 27476 RVA: 0x002A014D File Offset: 0x0029E34D
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

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x06006B55 RID: 27477 RVA: 0x002A0166 File Offset: 0x0029E366
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

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x06006B56 RID: 27478 RVA: 0x002A0187 File Offset: 0x0029E387
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

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x06006B57 RID: 27479 RVA: 0x002A01AB File Offset: 0x0029E3AB
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

	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x06006B58 RID: 27480 RVA: 0x002A01CC File Offset: 0x0029E3CC
	[XuiXmlBinding("auth_devicecode")]
	public string AuthDeviceCode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.authCode ?? "";
		}
	}

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x06006B59 RID: 27481 RVA: 0x002A01DD File Offset: 0x0029E3DD
	[XuiXmlBinding("auth_verificationUrl")]
	public string AuthVerificationUrl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.authVerificationUrl ?? "";
		}
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x06006B5A RID: 27482 RVA: 0x002A01EE File Offset: 0x0029E3EE
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

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x06006B5B RID: 27483 RVA: 0x002A020B File Offset: 0x0029E40B
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

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x06006B5C RID: 27484 RVA: 0x002A0231 File Offset: 0x0029E431
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

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06006B5D RID: 27485 RVA: 0x002A025A File Offset: 0x0029E45A
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

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06006B5E RID: 27486 RVA: 0x002A0283 File Offset: 0x0029E483
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

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06006B5F RID: 27487 RVA: 0x002A02AC File Offset: 0x0029E4AC
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

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06006B60 RID: 27488 RVA: 0x002A02CD File Offset: 0x0029E4CD
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

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06006B61 RID: 27489 RVA: 0x002A02EE File Offset: 0x0029E4EE
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

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06006B62 RID: 27490 RVA: 0x002A0314 File Offset: 0x0029E514
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

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06006B63 RID: 27491 RVA: 0x002A0349 File Offset: 0x0029E549
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

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06006B64 RID: 27492 RVA: 0x002A037E File Offset: 0x0029E57E
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

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06006B65 RID: 27493 RVA: 0x002A03B3 File Offset: 0x0029E5B3
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

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x06006B66 RID: 27494 RVA: 0x002A03D9 File Offset: 0x0029E5D9
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

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x06006B67 RID: 27495 RVA: 0x002A03FF File Offset: 0x0029E5FF
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

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x06006B68 RID: 27496 RVA: 0x002A0425 File Offset: 0x0029E625
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

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x06006B69 RID: 27497 RVA: 0x002A044B File Offset: 0x0029E64B
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

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x06006B6A RID: 27498 RVA: 0x002A0471 File Offset: 0x0029E671
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

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x06006B6B RID: 27499 RVA: 0x002A0497 File Offset: 0x0029E697
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

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x06006B6C RID: 27500 RVA: 0x002A04BD File Offset: 0x0029E6BD
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

	// Token: 0x0400519D RID: 20893
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400519E RID: 20894
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector tabs;

	// Token: 0x0400519F RID: 20895
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboOptOutTwitch;

	// Token: 0x040051A0 RID: 20896
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboUseProgression;

	// Token: 0x040051A1 RID: 20897
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCrateSharing;

	// Token: 0x040051A2 RID: 20898
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions> comboAllowActionsDuringBloodmoon;

	// Token: 0x040051A3 RID: 20899
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.TwitchBloodMoonOptions> comboAllowActionsDuringQuests;

	// Token: 0x040051A4 RID: 20900
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesDuringBloodmoon;

	// Token: 0x040051A5 RID: 20901
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesDuringQuests;

	// Token: 0x040051A6 RID: 20902
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVotesInSafeZone;

	// Token: 0x040051A7 RID: 20903
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboPointsGeneration;

	// Token: 0x040051A8 RID: 20904
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboCooldownPreset;

	// Token: 0x040051A9 RID: 20905
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowVisionEffects;

	// Token: 0x040051AA RID: 20906
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboActionSpamDelay;

	// Token: 0x040051AB RID: 20907
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<TwitchManager.IntegrationSettings> comboActionTwitchIntegrationType;

	// Token: 0x040051AC RID: 20908
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowActions;

	// Token: 0x040051AD RID: 20909
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowVotes;

	// Token: 0x040051AE RID: 20910
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboAllowEvents;

	// Token: 0x040051AF RID: 20911
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboViewerStartingPoints;

	// Token: 0x040051B0 RID: 20912
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.BitAddOptions> comboBitPointsAdd;

	// Token: 0x040051B1 RID: 20913
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboSubPointsAdd;

	// Token: 0x040051B2 RID: 20914
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_OptionsTwitch.PointsGenerationOptions> comboGiftSubPointsAdd;

	// Token: 0x040051B3 RID: 20915
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboRaidPointsAdd;

	// Token: 0x040051B4 RID: 20916
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboRaidViewerMinimum;

	// Token: 0x040051B5 RID: 20917
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboMaxDailyVotes;

	// Token: 0x040051B6 RID: 20918
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteTime;

	// Token: 0x040051B7 RID: 20919
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteViewerDefeatReward;

	// Token: 0x040051B8 RID: 20920
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboVoteDayTimeRange;

	// Token: 0x040051B9 RID: 20921
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboHelperRewardAmount;

	// Token: 0x040051BA RID: 20922
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboHelperRewardTime;

	// Token: 0x040051BB RID: 20923
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowBitEvents;

	// Token: 0x040051BC RID: 20924
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowSubEvents;

	// Token: 0x040051BD RID: 20925
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowGiftSubEvents;

	// Token: 0x040051BE RID: 20926
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCharityEvents;

	// Token: 0x040051BF RID: 20927
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowRaidEvents;

	// Token: 0x040051C0 RID: 20928
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowHypeTrainEvents;

	// Token: 0x040051C1 RID: 20929
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowCreatorGoalEvents;

	// Token: 0x040051C2 RID: 20930
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool comboAllowChannelPointRedeemEvents;

	// Token: 0x040051C3 RID: 20931
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboBitPrices;

	// Token: 0x040051C4 RID: 20932
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> comboBitPotPercent;

	// Token: 0x040051C5 RID: 20933
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.TwitchBloodMoonOptions origAllowActionsDuringBloodmoon;

	// Token: 0x040051C6 RID: 20934
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.TwitchBloodMoonOptions origAllowActionsDuringQuests;

	// Token: 0x040051C7 RID: 20935
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origPointsGeneration;

	// Token: 0x040051C8 RID: 20936
	[PublicizedFrom(EAccessModifier.Private)]
	public int origCooldownPresetIndex;

	// Token: 0x040051C9 RID: 20937
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origUseProgression;

	// Token: 0x040051CA RID: 20938
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowTwitchOptions;

	// Token: 0x040051CB RID: 20939
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVisionEffects = true;

	// Token: 0x040051CC RID: 20940
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCrateSharing;

	// Token: 0x040051CD RID: 20941
	[PublicizedFrom(EAccessModifier.Private)]
	public int origActionSpamDelay = 1;

	// Token: 0x040051CE RID: 20942
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager.IntegrationSettings origIntegrationSetting;

	// Token: 0x040051CF RID: 20943
	[PublicizedFrom(EAccessModifier.Private)]
	public int origActionPresetIndex;

	// Token: 0x040051D0 RID: 20944
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVotePresetIndex;

	// Token: 0x040051D1 RID: 20945
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEventPresetIndex;

	// Token: 0x040051D2 RID: 20946
	[PublicizedFrom(EAccessModifier.Private)]
	public int origBitPricesIndex;

	// Token: 0x040051D3 RID: 20947
	[PublicizedFrom(EAccessModifier.Private)]
	public int origBitPotPercentIndex;

	// Token: 0x040051D4 RID: 20948
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.BitAddOptions origBitPointModifier;

	// Token: 0x040051D5 RID: 20949
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origSubPointModifier;

	// Token: 0x040051D6 RID: 20950
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsTwitch.PointsGenerationOptions origGiftSubPointModifier;

	// Token: 0x040051D7 RID: 20951
	[PublicizedFrom(EAccessModifier.Private)]
	public int origRaidPointAmountIndex;

	// Token: 0x040051D8 RID: 20952
	[PublicizedFrom(EAccessModifier.Private)]
	public int origRaidViewerMinimumIndex;

	// Token: 0x040051D9 RID: 20953
	[PublicizedFrom(EAccessModifier.Private)]
	public int origHelperRewardAmountIndex;

	// Token: 0x040051DA RID: 20954
	[PublicizedFrom(EAccessModifier.Private)]
	public int origHelperRewardTimeIndex;

	// Token: 0x040051DB RID: 20955
	[PublicizedFrom(EAccessModifier.Private)]
	public int origViewerStartingPoints;

	// Token: 0x040051DC RID: 20956
	[PublicizedFrom(EAccessModifier.Private)]
	public int origMaxDailyVotes;

	// Token: 0x040051DD RID: 20957
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteTime;

	// Token: 0x040051DE RID: 20958
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteDayTimeRange;

	// Token: 0x040051DF RID: 20959
	[PublicizedFrom(EAccessModifier.Private)]
	public int origVoteViewerDefeatReward;

	// Token: 0x040051E0 RID: 20960
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesDuringBloodmoon;

	// Token: 0x040051E1 RID: 20961
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesDuringQuests;

	// Token: 0x040051E2 RID: 20962
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowVotesInSafeZone;

	// Token: 0x040051E3 RID: 20963
	[PublicizedFrom(EAccessModifier.Private)]
	public bool startedSinceOpened;

	// Token: 0x040051E4 RID: 20964
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager twitchManager;

	// Token: 0x040051E5 RID: 20965
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempSubModifier = 1;

	// Token: 0x040051E6 RID: 20966
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempGiftSubModifier = 1;

	// Token: 0x040051E7 RID: 20967
	[PublicizedFrom(EAccessModifier.Private)]
	public int tempVoteDayTimeRange = 2;

	// Token: 0x040051E8 RID: 20968
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowBitEvents = true;

	// Token: 0x040051E9 RID: 20969
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowSubEvents = true;

	// Token: 0x040051EA RID: 20970
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowGiftSubEvents = true;

	// Token: 0x040051EB RID: 20971
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCharityEvents = true;

	// Token: 0x040051EC RID: 20972
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowRaidEvents = true;

	// Token: 0x040051ED RID: 20973
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowHypeTrainEvents = true;

	// Token: 0x040051EE RID: 20974
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowCreatorGoalEvents = true;

	// Token: 0x040051EF RID: 20975
	[PublicizedFrom(EAccessModifier.Private)]
	public bool origAllowChannelPointRedemptions = true;

	// Token: 0x040051F0 RID: 20976
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showDeviceCode;

	// Token: 0x040051F1 RID: 20977
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture qrCodeTexControl;

	// Token: 0x040051F2 RID: 20978
	[PublicizedFrom(EAccessModifier.Private)]
	public string authCode = "";

	// Token: 0x040051F3 RID: 20979
	[PublicizedFrom(EAccessModifier.Private)]
	public string authVerificationUrl = "";

	// Token: 0x02000DB3 RID: 3507
	public enum TwitchBloodMoonOptions
	{
		// Token: 0x040051F5 RID: 20981
		Disabled,
		// Token: 0x040051F6 RID: 20982
		Standard,
		// Token: 0x040051F7 RID: 20983
		CooldownOnly
	}

	// Token: 0x02000DB4 RID: 3508
	public enum PointsGenerationOptions
	{
		// Token: 0x040051F9 RID: 20985
		Disabled,
		// Token: 0x040051FA RID: 20986
		Standard,
		// Token: 0x040051FB RID: 20987
		Double,
		// Token: 0x040051FC RID: 20988
		Triple
	}

	// Token: 0x02000DB5 RID: 3509
	public enum BitAddOptions
	{
		// Token: 0x040051FE RID: 20990
		Standard = 1,
		// Token: 0x040051FF RID: 20991
		Double,
		// Token: 0x04005200 RID: 20992
		Triple
	}
}

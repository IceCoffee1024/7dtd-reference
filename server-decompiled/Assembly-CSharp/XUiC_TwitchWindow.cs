using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010CD RID: 4301
[Preserve]
public class XUiC_TwitchWindow : XUiController
{
	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x060088AD RID: 34989 RVA: 0x00344CA1 File Offset: 0x00342EA1
	// (set) Token: 0x060088AE RID: 34990 RVA: 0x00344CA9 File Offset: 0x00342EA9
	public EntityPlayerLocal localPlayer
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x060088AF RID: 34991 RVA: 0x00344CB4 File Offset: 0x00342EB4
	public override void Init()
	{
		base.Init();
		XUiC_TwitchWindow.ID = base.WindowGroup.Id;
		this.twitchManager = TwitchManager.Current;
		this.twitchManager.ConnectionStateChanged -= this.TwitchManager_ConnectionStateChanged;
		this.twitchManager.ConnectionStateChanged += this.TwitchManager_ConnectionStateChanged;
		TwitchVotingManager votingManager = this.twitchManager.VotingManager;
		votingManager.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(votingManager.VoteStarted, new OnGameEventVoteAction(this.TwitchManager_VoteStarted));
		TwitchVotingManager votingManager2 = this.twitchManager.VotingManager;
		votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager2.VoteStarted, new OnGameEventVoteAction(this.TwitchManager_VoteStarted));
		base.OnVisiblity += this.OnVisibilityChanged;
		this.CommandListUI = base.GetChildByType<XUiC_TwitchCommandList>();
		this.CommandListUI.Owner = this;
		XUiC_TwitchVoteList[] childrenByType = base.GetChildrenByType<XUiC_TwitchVoteList>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			childrenByType[i].Owner = this;
		}
		XUiController xuiController = base.GetChildById("leftButton");
		xuiController.OnPress += this.Left_OnPress;
		xuiController = base.GetChildById("rightButton");
		xuiController.OnPress += this.Right_OnPress;
		xuiController = base.GetChildById("pauseButton");
		xuiController.OnPress += this.cooldown_OnPress;
		this.pauseButton = xuiController.ViewComponent as XUiV_Button;
		xuiController = base.GetChildById("optionsButton");
		xuiController.OnPress += this.options_OnPress;
		this.optionsButton = xuiController.ViewComponent as XUiV_Button;
		xuiController = base.GetChildById("historyButton");
		xuiController.OnPress += this.history_OnPress;
		this.historyButton = xuiController.ViewComponent as XUiV_Button;
		this.defaultGearColor = this.optionsButton.CurrentColor;
		this.gearSpriteSize = new Vector2((float)this.optionsButton.Size.x, (float)this.optionsButton.Size.y);
		this.window = base.ViewComponent as XUiV_Window;
		this.lblStatusReady = Localization.Get("TwitchCooldownStatus_Ready", false, null);
		this.lblStatusWaiting = Localization.Get("TwitchCooldownStatus_Waiting", false, null);
		this.lblStatusBloodMoon = Localization.Get("TwitchCooldownStatus_BloodMoon", false, null);
		this.lblStatusCooldown = Localization.Get("TwitchCooldownStatus_Cooldown", false, null);
		this.lblStatusPaused = Localization.Get("TwitchCooldownStatus_Paused", false, null);
		this.lblStatusSafe = Localization.Get("TwitchCooldownStatus_Safe", false, null);
		this.lblStatusVote = Localization.Get("TwitchCooldownStatus_Vote", false, null);
		this.lblStatusQuest = Localization.Get("TwitchCooldownStatus_Quest", false, null);
		this.lblVoteLocked = Localization.Get("TwitchCooldownStatus_VoteLocked", false, null);
		this.lblPointsPP = Localization.Get("TwitchPoints_PP", false, null);
		this.lblPointsSP = Localization.Get("TwitchPoints_SP", false, null);
	}

	// Token: 0x060088B0 RID: 34992 RVA: 0x00344F90 File Offset: 0x00343190
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (this.progressionButton != null)
		{
			this.progressionButton.Selected = this.twitchManager.UseProgression;
		}
		if (this.pauseButton != null)
		{
			this.pauseButton.Selected = !this.twitchManager.TwitchActive;
		}
	}

	// Token: 0x060088B1 RID: 34993 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchManager_VoteStarted()
	{
		this.IsDirty = true;
	}

	// Token: 0x060088B2 RID: 34994 RVA: 0x00344FDC File Offset: 0x003431DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void options_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchWindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "Actions", false);
	}

	// Token: 0x060088B3 RID: 34995 RVA: 0x00344FF9 File Offset: 0x003431F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void history_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchWindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "ActionHistory", false);
	}

	// Token: 0x060088B4 RID: 34996 RVA: 0x00345016 File Offset: 0x00343216
	[PublicizedFrom(EAccessModifier.Private)]
	public void cooldown_OnPress(XUiController _sender, int _mouseButton)
	{
		this.twitchManager.ToggleTwitchActive();
		if (this.pauseButton != null)
		{
			this.pauseButton.Selected = !this.twitchManager.TwitchActive;
		}
	}

	// Token: 0x060088B5 RID: 34997 RVA: 0x00345044 File Offset: 0x00343244
	[PublicizedFrom(EAccessModifier.Private)]
	public void Left_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CommandListUI.MoveBackward();
	}

	// Token: 0x060088B6 RID: 34998 RVA: 0x00345051 File Offset: 0x00343251
	[PublicizedFrom(EAccessModifier.Private)]
	public void Right_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CommandListUI.MoveForward();
	}

	// Token: 0x060088B7 RID: 34999 RVA: 0x0034505E File Offset: 0x0034325E
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchManager_ConnectionStateChanged(TwitchManager.InitStates oldState, TwitchManager.InitStates newState)
	{
		this.viewComponent.IsVisible = newState == TwitchManager.InitStates.Ready;
		this.IsDirty = true;
	}

	// Token: 0x060088B8 RID: 35000 RVA: 0x00345078 File Offset: 0x00343278
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.IsDirty = true;
		if (Time.time - this.lastUpdate >= this.secondRotation)
		{
			if (this.showingBitPot || (!this.showingBitPot && this.twitchManager.BitPot != 0))
			{
				this.IsDirty = true;
				this.showingBitPot = !this.showingBitPot;
			}
			this.lastUpdate = Time.time;
		}
		if (this.showingBitPot && this.twitchManager.BitPot == 0)
		{
			this.showingBitPot = false;
			this.IsDirty = true;
			this.lastUpdate = Time.time;
		}
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		this.window.IsVisible = this.twitchManager.InitState == TwitchManager.InitStates.Ready && !this.localPlayer.IsDead() && !XUi.InGameMenuOpen;
		if (this.twitchManager != null && this.twitchManager.LocalPlayerXUi == null)
		{
			this.twitchManager.LocalPlayerXUi = this.xui;
		}
		if (!this.window.IsVisible)
		{
			return;
		}
		if (!this.twitchManager.HasViewedSettings)
		{
			float num = Mathf.PingPong(Time.time, 0.5f);
			this.optionsButton.DefaultSpriteColor = Color.Lerp(this.defaultGearColor, this.gearBlinkColor, num * 4f);
			float num2 = 1f;
			if (num > 0.25f)
			{
				num2 = 1f + num - 0.25f;
			}
			this.optionsButton.Size = new Vector2i((int)(this.gearSpriteSize.x * num2), (int)(this.gearSpriteSize.y * num2));
		}
		else
		{
			this.optionsButton.DefaultSpriteColor = this.defaultGearColor;
		}
		if (this.CommandListUI.IsDirty)
		{
			this.IsDirty = true;
		}
		if (this.twitchManager.CooldownTime > 0f)
		{
			this.IsDirty = true;
		}
		if (this.twitchManager.UIDirty)
		{
			this.IsDirty = true;
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060088B9 RID: 35001 RVA: 0x00345293 File Offset: 0x00343493
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.progressionButton != null)
		{
			this.progressionButton.Selected = this.twitchManager.UseProgression;
		}
	}

	// Token: 0x060088BA RID: 35002 RVA: 0x003452BC File Offset: 0x003434BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2061600441U)
		{
			if (num <= 1257793062U)
			{
				if (num <= 1195155980U)
				{
					if (num != 1023565667U)
					{
						if (num == 1195155980U)
						{
							if (bindingName == "grouptitlevisible")
							{
								if (this.twitchManager == null)
								{
									value = "false";
									return true;
								}
								if (!this.twitchManager.TwitchActive)
								{
									value = "false";
									return true;
								}
								if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Time || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Startup || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
								{
									value = "false";
									return true;
								}
								if (!this.twitchManager.AllowActions && !this.twitchManager.VotingManager.VotingIsActive && this.twitchManager.VoteLockedLevel != TwitchVoteLockTypes.ActionsLocked)
								{
									value = "false";
									return true;
								}
								value = "true";
								return true;
							}
						}
					}
					else if (bindingName == "show_vote_tip")
					{
						value = "false";
						if (this.twitchManager == null || this.twitchManager.InitState != TwitchManager.InitStates.Ready)
						{
							value = "false";
							return true;
						}
						if (this.twitchManager.VotingManager.WinnerShowing && this.twitchManager.VotingManager.VoteTip != "")
						{
							value = "true";
						}
						return true;
					}
				}
				else if (num != 1215668937U)
				{
					if (num == 1257793062U)
					{
						if (bindingName == "status_title")
						{
							if (this.twitchManager == null)
							{
								value = "";
								return true;
							}
							if (this.twitchManager.InitState == TwitchManager.InitStates.Ready)
							{
								if (!this.twitchManager.TwitchActive)
								{
									value = this.lblStatusPaused;
								}
								else if (this.twitchManager.CooldownTime > 0f && this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Time)
								{
									value = this.lblStatusCooldown;
								}
								else if (this.twitchManager.VotingManager.VotingIsActive)
								{
									value = this.lblStatusVote;
								}
								else if (this.twitchManager.CooldownTime > 0f || this.twitchManager.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Always)
								{
									value = this.lblStatusCooldown;
								}
								else
								{
									value = this.lblStatusReady;
								}
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "showcontent")
				{
					if (this.twitchManager == null)
					{
						value = "false";
						return true;
					}
					if (!this.twitchManager.TwitchActive)
					{
						value = "false";
						return true;
					}
					if (this.twitchManager.InitState != TwitchManager.InitStates.Ready)
					{
						value = "false";
						return true;
					}
					if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Time || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Startup)
					{
						value = "false";
						return true;
					}
					if (this.twitchManager.VotingManager.VotingIsActive)
					{
						value = "true";
						return true;
					}
					if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
					{
						value = "false";
						return true;
					}
					if (this.twitchManager.VoteLockedLevel == TwitchVoteLockTypes.ActionsLocked)
					{
						value = "false";
						return true;
					}
					if (!this.twitchManager.AllowActions)
					{
						value = "false";
						return true;
					}
					if (this.twitchManager.IntegrationSetting == TwitchManager.IntegrationSettings.ExtensionOnly)
					{
						value = "false";
						return true;
					}
					value = "true";
					return true;
				}
			}
			else if (num <= 1875832543U)
			{
				if (num != 1461653202U)
				{
					if (num == 1875832543U)
					{
						if (bindingName == "vote_tip")
						{
							if (this.twitchManager == null || this.twitchManager.InitState != TwitchManager.InitStates.Ready)
							{
								value = "";
								return true;
							}
							value = this.twitchManager.VotingManager.VoteTip;
							return true;
						}
					}
				}
				else if (bindingName == "showcommands")
				{
					value = (this.twitchManager != null && this.twitchManager.InitState == TwitchManager.InitStates.Ready && !this.twitchManager.VotingManager.VotingIsActive).ToString();
					return true;
				}
			}
			else if (num != 1968087740U)
			{
				if (num == 2061600441U)
				{
					if (bindingName == "cooldownfill")
					{
						if (this.twitchManager == null)
						{
							value = "0";
							return true;
						}
						if (this.twitchManager.CurrentCooldownPreset != null && this.twitchManager.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.None)
						{
							value = "0";
							return true;
						}
						if (this.twitchManager.CooldownType != TwitchManager.CooldownTypes.SafeCooldown && this.twitchManager.CooldownType != TwitchManager.CooldownTypes.SafeCooldownExit && (this.twitchManager.CooldownTime > 0f || (this.twitchManager.CurrentCooldownPreset != null && this.twitchManager.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Always)))
						{
							value = "1";
							return true;
						}
						value = (this.twitchManager.CurrentCooldownFill / this.twitchManager.CurrentCooldownPreset.CooldownFillMax).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "commandsheight")
			{
				if (this.CommandListUI != null)
				{
					value = this.CommandListUI.GetHeight().ToString();
				}
				return true;
			}
		}
		else if (num <= 3125508079U)
		{
			if (num <= 2728359946U)
			{
				if (num != 2584460877U)
				{
					if (num == 2728359946U)
					{
						if (bindingName == "grouptitle")
						{
							if (this.twitchManager == null)
							{
								value = "";
								return true;
							}
							if (!this.twitchManager.TwitchActive)
							{
								value = "";
								return true;
							}
							if (this.twitchManager.VotingManager.VotingIsActive)
							{
								value = this.twitchManager.VotingManager.VoteTypeText;
								return true;
							}
							if (this.twitchManager.VoteLockedLevel == TwitchVoteLockTypes.ActionsLocked)
							{
								value = this.lblVoteLocked;
								return true;
							}
							if (this.twitchManager.CooldownTime > 0f && (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Time || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Startup))
							{
								value = "";
								return true;
							}
							if (!this.twitchManager.AllowActions)
							{
								value = "";
								return true;
							}
							if (this.twitchManager.IntegrationSetting == TwitchManager.IntegrationSettings.ExtensionOnly)
							{
								value = Localization.Get("xuiTwitchIntegrationOptionsExtensionOnly", false, null);
								return true;
							}
							if (this.CommandListUI != null)
							{
								value = this.CommandListUI.CurrentTitle;
								return true;
							}
							value = "";
							return true;
						}
					}
				}
				else if (bindingName == "showpotbalance")
				{
					if (this.twitchManager == null || this.twitchManager.InitState != TwitchManager.InitStates.Ready || this.twitchManager.PimpPotType == TwitchManager.PimpPotSettings.Disabled)
					{
						value = "false";
						return true;
					}
					value = "true";
					return true;
				}
			}
			else if (num != 3053359380U)
			{
				if (num == 3125508079U)
				{
					if (bindingName == "status")
					{
						if (this.twitchManager == null)
						{
							value = "";
							return true;
						}
						value = "";
						if (this.twitchManager.InitState == TwitchManager.InitStates.Ready)
						{
							if ((this.twitchManager.CooldownTime > 0f && this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Time) || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.Startup)
							{
								value = XUiM_PlayerBuffs.ConvertToTimeString(this.twitchManager.CooldownTime);
							}
							else if (this.twitchManager.VotingManager.VotingIsActive && this.twitchManager.VotingManager.CurrentVoteState != TwitchVotingManager.VoteStateTypes.WaitingForActive && this.twitchManager.VotingManager.CurrentVoteState != TwitchVotingManager.VoteStateTypes.EventActive)
							{
								if (this.twitchManager.VotingManager.VoteTimeRemaining > 0f)
								{
									value = XUiM_PlayerBuffs.GetCVarValueAsTimeString(this.twitchManager.VotingManager.VoteTimeRemaining);
								}
							}
							else if (this.twitchManager.CooldownTime > 0f)
							{
								if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting)
								{
									value = this.lblStatusWaiting;
								}
								else if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown)
								{
									value = this.lblStatusBloodMoon;
								}
								else if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.QuestDisabled || this.twitchManager.CooldownType == TwitchManager.CooldownTypes.QuestCooldown)
								{
									value = this.lblStatusQuest;
								}
								else if (this.twitchManager.CooldownType == TwitchManager.CooldownTypes.SafeCooldown)
								{
									value = this.lblStatusSafe;
								}
								else
								{
									value = XUiM_PlayerBuffs.ConvertToTimeString(this.twitchManager.CooldownTime);
								}
							}
						}
						return true;
					}
				}
			}
			else if (bindingName == "tip_offset")
			{
				if (this.twitchManager == null || this.twitchManager.InitState != TwitchManager.InitStates.Ready)
				{
					value = "0";
					return true;
				}
				value = this.twitchManager.VotingManager.VoteOffset;
				return true;
			}
		}
		else if (num <= 3380638462U)
		{
			if (num != 3220589183U)
			{
				if (num == 3380638462U)
				{
					if (bindingName == "statuscolor")
					{
						value = ((this.twitchManager != null) ? "[ffffff]" : "[ffffff]");
						return true;
					}
				}
			}
			else if (bindingName == "voteitems")
			{
				value = "3";
				return true;
			}
		}
		else if (num != 3759131736U)
		{
			if (num != 3811588448U)
			{
				if (num == 4228112153U)
				{
					if (bindingName == "showvotes")
					{
						value = (this.twitchManager != null && this.twitchManager.InitState == TwitchManager.InitStates.Ready && this.twitchManager.VotingManager.VotingIsActive).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "potbalance")
			{
				if (this.twitchManager == null || this.twitchManager.InitState != TwitchManager.InitStates.Ready)
				{
					value = "";
					return true;
				}
				if (this.showingBitPot)
				{
					value = string.Format("[FFB400]{0} BC[-]", this.twitchManager.BitPot);
				}
				else
				{
					value = string.Format("{0} {1}", this.twitchManager.RewardPot, (this.twitchManager.PimpPotType == TwitchManager.PimpPotSettings.EnabledSP) ? this.lblPointsSP : this.lblPointsPP);
				}
				return true;
			}
		}
		else if (bindingName == "arrowvisible")
		{
			if (this.twitchManager == null || this.twitchManager.InitState != TwitchManager.InitStates.Ready)
			{
				value = "true";
				return true;
			}
			if (this.twitchManager.VotingManager.VotingIsActive || this.twitchManager.VoteLockedLevel == TwitchVoteLockTypes.ActionsLocked)
			{
				value = "false";
				return true;
			}
			if (!this.twitchManager.AllowActions)
			{
				value = "false";
				return true;
			}
			if (this.twitchManager.IntegrationSetting == TwitchManager.IntegrationSettings.ExtensionOnly)
			{
				value = "false";
				return true;
			}
			value = "true";
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060088BB RID: 35003 RVA: 0x00345D77 File Offset: 0x00343F77
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
		if (this.twitchManager != null)
		{
			this.twitchManager.ConnectionStateChanged -= this.TwitchManager_ConnectionStateChanged;
		}
	}

	// Token: 0x040065D6 RID: 26070
	public static string ID = "";

	// Token: 0x040065D7 RID: 26071
	public TwitchManager twitchManager;

	// Token: 0x040065D8 RID: 26072
	[PublicizedFrom(EAccessModifier.Private)]
	public Color defaultGearColor;

	// Token: 0x040065D9 RID: 26073
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 gearSpriteSize;

	// Token: 0x040065DA RID: 26074
	[PublicizedFrom(EAccessModifier.Private)]
	public Color gearBlinkColor = new Color32(byte.MaxValue, 180, 0, byte.MaxValue);

	// Token: 0x040065DB RID: 26075
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdate;

	// Token: 0x040065DC RID: 26076
	[PublicizedFrom(EAccessModifier.Private)]
	public float secondRotation = 5f;

	// Token: 0x040065DD RID: 26077
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchCommandList CommandListUI;

	// Token: 0x040065DE RID: 26078
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button progressionButton;

	// Token: 0x040065DF RID: 26079
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button optionsButton;

	// Token: 0x040065E0 RID: 26080
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button historyButton;

	// Token: 0x040065E1 RID: 26081
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button pauseButton;

	// Token: 0x040065E2 RID: 26082
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Window window;

	// Token: 0x040065E3 RID: 26083
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingBitPot;

	// Token: 0x040065E5 RID: 26085
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusReady;

	// Token: 0x040065E6 RID: 26086
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusWaiting;

	// Token: 0x040065E7 RID: 26087
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusBloodMoon;

	// Token: 0x040065E8 RID: 26088
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusCooldown;

	// Token: 0x040065E9 RID: 26089
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusPaused;

	// Token: 0x040065EA RID: 26090
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusSafe;

	// Token: 0x040065EB RID: 26091
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusVote;

	// Token: 0x040065EC RID: 26092
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusQuest;

	// Token: 0x040065ED RID: 26093
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblVoteLocked;

	// Token: 0x040065EE RID: 26094
	public string lblPointsPP;

	// Token: 0x040065EF RID: 26095
	public string lblPointsSP;

	// Token: 0x040065F0 RID: 26096
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}

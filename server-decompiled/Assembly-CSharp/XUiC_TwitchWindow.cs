using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010AC RID: 4268
[Preserve]
public class XUiC_TwitchWindow : XUiController
{
	// Token: 0x17000FDE RID: 4062
	// (get) Token: 0x0600880B RID: 34827 RVA: 0x0033EE05 File Offset: 0x0033D005
	// (set) Token: 0x0600880C RID: 34828 RVA: 0x0033EE0D File Offset: 0x0033D00D
	public EntityPlayerLocal localPlayer
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x0600880D RID: 34829 RVA: 0x0033EE18 File Offset: 0x0033D018
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

	// Token: 0x0600880E RID: 34830 RVA: 0x0033F0F4 File Offset: 0x0033D2F4
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

	// Token: 0x0600880F RID: 34831 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchManager_VoteStarted()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008810 RID: 34832 RVA: 0x0033F140 File Offset: 0x0033D340
	[PublicizedFrom(EAccessModifier.Private)]
	public void options_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchWindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "Actions", false);
	}

	// Token: 0x06008811 RID: 34833 RVA: 0x0033F15D File Offset: 0x0033D35D
	[PublicizedFrom(EAccessModifier.Private)]
	public void history_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchWindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "ActionHistory", false);
	}

	// Token: 0x06008812 RID: 34834 RVA: 0x0033F17A File Offset: 0x0033D37A
	[PublicizedFrom(EAccessModifier.Private)]
	public void cooldown_OnPress(XUiController _sender, int _mouseButton)
	{
		this.twitchManager.ToggleTwitchActive();
		if (this.pauseButton != null)
		{
			this.pauseButton.Selected = !this.twitchManager.TwitchActive;
		}
	}

	// Token: 0x06008813 RID: 34835 RVA: 0x0033F1A8 File Offset: 0x0033D3A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Left_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CommandListUI.MoveBackward();
	}

	// Token: 0x06008814 RID: 34836 RVA: 0x0033F1B5 File Offset: 0x0033D3B5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Right_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CommandListUI.MoveForward();
	}

	// Token: 0x06008815 RID: 34837 RVA: 0x0033F1C2 File Offset: 0x0033D3C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchManager_ConnectionStateChanged(TwitchManager.InitStates oldState, TwitchManager.InitStates newState)
	{
		this.viewComponent.IsVisible = newState == TwitchManager.InitStates.Ready;
		this.IsDirty = true;
	}

	// Token: 0x06008816 RID: 34838 RVA: 0x0033F1DC File Offset: 0x0033D3DC
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

	// Token: 0x06008817 RID: 34839 RVA: 0x0033F3F7 File Offset: 0x0033D5F7
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.progressionButton != null)
		{
			this.progressionButton.Selected = this.twitchManager.UseProgression;
		}
	}

	// Token: 0x06008818 RID: 34840 RVA: 0x0033F420 File Offset: 0x0033D620
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

	// Token: 0x06008819 RID: 34841 RVA: 0x0033FEDB File Offset: 0x0033E0DB
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
		if (this.twitchManager != null)
		{
			this.twitchManager.ConnectionStateChanged -= this.TwitchManager_ConnectionStateChanged;
		}
	}

	// Token: 0x04006555 RID: 25941
	public static string ID = "";

	// Token: 0x04006556 RID: 25942
	public TwitchManager twitchManager;

	// Token: 0x04006557 RID: 25943
	[PublicizedFrom(EAccessModifier.Private)]
	public Color defaultGearColor;

	// Token: 0x04006558 RID: 25944
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 gearSpriteSize;

	// Token: 0x04006559 RID: 25945
	[PublicizedFrom(EAccessModifier.Private)]
	public Color gearBlinkColor = new Color32(byte.MaxValue, 180, 0, byte.MaxValue);

	// Token: 0x0400655A RID: 25946
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdate;

	// Token: 0x0400655B RID: 25947
	[PublicizedFrom(EAccessModifier.Private)]
	public float secondRotation = 5f;

	// Token: 0x0400655C RID: 25948
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchCommandList CommandListUI;

	// Token: 0x0400655D RID: 25949
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button progressionButton;

	// Token: 0x0400655E RID: 25950
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button optionsButton;

	// Token: 0x0400655F RID: 25951
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button historyButton;

	// Token: 0x04006560 RID: 25952
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button pauseButton;

	// Token: 0x04006561 RID: 25953
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Window window;

	// Token: 0x04006562 RID: 25954
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingBitPot;

	// Token: 0x04006564 RID: 25956
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusReady;

	// Token: 0x04006565 RID: 25957
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusWaiting;

	// Token: 0x04006566 RID: 25958
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusBloodMoon;

	// Token: 0x04006567 RID: 25959
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusCooldown;

	// Token: 0x04006568 RID: 25960
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusPaused;

	// Token: 0x04006569 RID: 25961
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusSafe;

	// Token: 0x0400656A RID: 25962
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusVote;

	// Token: 0x0400656B RID: 25963
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStatusQuest;

	// Token: 0x0400656C RID: 25964
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblVoteLocked;

	// Token: 0x0400656D RID: 25965
	public string lblPointsPP;

	// Token: 0x0400656E RID: 25966
	public string lblPointsSP;

	// Token: 0x0400656F RID: 25967
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}

using System;
using Audio;
using Challenges;
using Platform;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200109D RID: 4253
[Preserve]
public class XUiC_TwitchEntryDescriptionWindow : XUiController
{
	// Token: 0x17000FCC RID: 4044
	// (get) Token: 0x0600875B RID: 34651 RVA: 0x0033997E File Offset: 0x00337B7E
	// (set) Token: 0x0600875C RID: 34652 RVA: 0x00339986 File Offset: 0x00337B86
	public TwitchAction CurrentTwitchActionEntry
	{
		get
		{
			return this.currentTwitchActionEntry;
		}
		set
		{
			this.currentTwitchActionEntry = value;
			base.RefreshBindings();
		}
	}

	// Token: 0x17000FCD RID: 4045
	// (get) Token: 0x0600875D RID: 34653 RVA: 0x00339995 File Offset: 0x00337B95
	// (set) Token: 0x0600875E RID: 34654 RVA: 0x0033999D File Offset: 0x00337B9D
	public TwitchVote CurrentTwitchVoteEntry
	{
		get
		{
			return this.currentTwitchVoteEntry;
		}
		set
		{
			this.currentTwitchVoteEntry = value;
			base.RefreshBindings();
		}
	}

	// Token: 0x17000FCE RID: 4046
	// (get) Token: 0x0600875F RID: 34655 RVA: 0x003399AC File Offset: 0x00337BAC
	// (set) Token: 0x06008760 RID: 34656 RVA: 0x003399B4 File Offset: 0x00337BB4
	public TwitchActionHistoryEntry CurrentTwitchActionHistoryEntry
	{
		get
		{
			return this.currentTwitchActionHistoryEntry;
		}
		set
		{
			this.currentTwitchActionHistoryEntry = value;
			base.RefreshBindings();
		}
	}

	// Token: 0x06008761 RID: 34657 RVA: 0x003399C4 File Offset: 0x00337BC4
	public override void Init()
	{
		base.Init();
		XUiController childById = ((XUiC_TwitchInfoWindowGroup)this.windowGroup.Controller).GetChildByType<XUiC_TwitchHowToWindow>().GetChildById("leftButton");
		XUiController childById2 = this.windowGroup.Controller.GetChildById("windowTwitchInfoDescription");
		XUiController xuiController = childById2.GetChildById("btnEnable");
		XUiController childById3 = childById2.GetChildById("statClick");
		this.btnEnable = (XUiV_Button)xuiController.GetChildById("clickable").ViewComponent;
		this.btnEnable.Controller.OnPress += this.btnEnable_OnPress;
		this.btnEnable.NavDownTarget = childById.ViewComponent;
		xuiController = childById2.GetChildById("btnRefund");
		this.btnRefund = (XUiV_Button)xuiController.GetChildById("clickable").ViewComponent;
		this.btnRefund.Controller.OnPress += this.btnRefund_OnPress;
		this.btnRetry = childById2.GetChildById("btnRetry").GetChildByType<XUiC_SimpleButton>();
		this.btnRetry.OnPressed += this.btnRetry_OnPress;
		this.btnRetry.ViewComponent.NavDownTarget = childById.ViewComponent;
		childById2.GetChildById("btnIncrease").GetChildByType<XUiC_SimpleButton>().OnPressed += this.BtnIncrease_OnPressed;
		childById2.GetChildById("btnDecrease").GetChildByType<XUiC_SimpleButton>().OnPressed += this.BtnDecrease_OnPressed;
		childById3.OnPress += this.RectStat_OnPress;
		this.lblStartGamestage = Localization.Get("TwitchInfo_ActionStartGamestage", false, null);
		this.lblEndGamestage = Localization.Get("TwitchInfo_ActionEndGamestage", false, null);
		this.lblPointCost = Localization.Get("TwitchInfo_ActionPointCost", false, null);
		this.lblDiscountCost = Localization.Get("TwitchInfo_ActionDiscountCost", false, null);
		this.lblCooldown = Localization.Get("TwitchInfo_ActionCooldown", false, null);
		this.lblRandomDaily = Localization.Get("TwitchInfo_ActionRandomDaily", false, null);
		this.lblIsPositive = Localization.Get("TwitchInfo_ActionIsPositive", false, null);
		this.lblPointType = Localization.Get("TwitchInfo_ActionPointType", false, null);
		this.lblEnableAction = Localization.Get("TwitchInfo_ActionEnableAction", false, null) + " ([action:gui:GUI D-Pad Up])";
		this.lblDisableAction = Localization.Get("TwitchInfo_ActionDisableAction", false, null) + " ([action:gui:GUI D-Pad Up])";
		this.lblIncreasePrice = Localization.Get("TwitchInfo_IncreasePriceButton", false, null) + " ([action:gui:GUI D-Pad Right])";
		this.lblDecreasePrice = Localization.Get("TwitchInfo_DecreasePriceButton", false, null) + " ([action:gui:GUI D-Pad Left])";
		this.lblEnableVote = Localization.Get("TwitchInfo_ActionEnableVote", false, null) + " ([action:gui:GUI D-Pad Up])";
		this.lblDisableVote = Localization.Get("TwitchInfo_ActionDisableVote", false, null) + " ([action:gui:GUI D-Pad Up])";
		this.lblEnableAction_Controller = Localization.Get("TwitchInfo_ActionEnableAction", false, null) + " [action:gui:GUI HalfStack]";
		this.lblDisableAction_Controller = Localization.Get("TwitchInfo_ActionDisableAction", false, null) + " [action:gui:GUI HalfStack]";
		this.lblIncreasePrice_Controller = Localization.Get("TwitchInfo_IncreasePriceButton", false, null) + " [action:gui:GUI Inspect] + [action:gui:GUI D-Pad Right]";
		this.lblDecreasePrice_Controller = Localization.Get("TwitchInfo_DecreasePriceButton", false, null) + " [action:gui:GUI Inspect] + [action:gui:GUI D-Pad Left]";
		this.lblEnableVote_Controller = Localization.Get("TwitchInfo_ActionEnableVote", false, null) + " [action:gui:GUI HalfStack]";
		this.lblDisableVote_Controller = Localization.Get("TwitchInfo_ActionDisableVote", false, null) + " [action:gui:GUI HalfStack]";
		this.lblActionEmpty = Localization.Get("TwitchInfo_ActionEmpty", false, null);
		this.lblVoteEmpty = Localization.Get("TwitchInfo_VoteEmpty", false, null);
		this.lblActionHistoryEmpty = Localization.Get("TwitchInfo_ActionHistoryEmpty", false, null);
		this.lblLeaderboardEmpty = Localization.Get("TwitchInfo_LeaderboardEmpty", false, null);
		this.lblHistoryTargetTitle = Localization.Get("TwitchInfo_ActionHistoryTarget", false, null);
		this.lblHistoryStateTitle = Localization.Get("xuiLightPropState", false, null);
		this.lblHistoryTimeStampTitle = Localization.Get("ObjectiveTime_keyword", false, null);
		this.lblRefund = Localization.Get("TwitchInfo_ActionHistoryRefund", false, null);
		this.lblNoRefund = Localization.Get("TwitchInfo_ActionHistoryRefundNotAvailable", false, null);
		this.lblRetry = Localization.Get("TwitchInfo_ActionHistoryRetry", false, null);
		this.lblNoRetry = Localization.Get("TwitchInfo_ActionHistoryRetryNotAvailable", false, null);
		this.lblRetryActionUnavailable = Localization.Get("TwitchInfo_ActionHistoryRetryActionUnavailable", false, null);
		this.lblLeaderboardStats = Localization.Get("TwitchInfo_LeaderboardStats", false, null);
		this.lblShowBitTotal = Localization.Get("TwitchInfo_LeaderboardShowBitTotal", false, null);
		this.lblTopKiller = Localization.Get("TwitchInfo_TopKiller", false, null);
		this.lblTopGood = Localization.Get("TwitchInfo_TopGood", false, null);
		this.lblTopEvil = Localization.Get("TwitchInfo_TopEvil", false, null);
		this.lblCurrentGood = Localization.Get("TwitchInfo_CurrentGood", false, null);
		this.lblMostBits = Localization.Get("TwitchInfo_MostBits", false, null);
		this.lblTotalBits = Localization.Get("TwitchInfo_TotalBits", false, null);
		this.lblTotalBad = Localization.Get("TwitchInfo_TotalBad", false, null);
		this.lblTotalGood = Localization.Get("TwitchInfo_TotalGood", false, null);
		this.lblTotalActions = Localization.Get("TwitchInfo_TotalActions", false, null);
		this.lblLargestPimpPot = Localization.Get("TwitchInfo_LargestPimpPot", false, null);
		this.lblTrue = Localization.Get("statTrue", false, null);
		this.lblFalse = Localization.Get("statFalse", false, null);
		this.lblPointsPP = Localization.Get("TwitchPoints_PP", false, null);
		this.lblPointsSP = Localization.Get("TwitchPoints_SP", false, null);
		this.lblPointsBits = Localization.Get("TwitchPoints_Bits", false, null);
		base.registerForInputStyleChanges();
	}

	// Token: 0x06008762 RID: 34658 RVA: 0x00339F38 File Offset: 0x00338138
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.ViewComponent.IsActiveInHierarchy && (this.actionEntry != null || this.voteEntry != null))
		{
			PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && !this.xui.playerUI.windowManager.IsInputActive())
			{
				if (this.actionEntry != null)
				{
					if (guiactions.DPad_Up.WasPressed)
					{
						this.btnEnable_OnPress(this.btnEnable.Controller, -1);
					}
					if (guiactions.DPad_Left.WasPressed)
					{
						this.BtnDecrease_OnPressed(null, -1);
					}
					if (guiactions.DPad_Right.WasPressed)
					{
						this.BtnIncrease_OnPressed(null, -1);
						return;
					}
				}
				else if (this.voteEntry != null && guiactions.DPad_Up.WasPressed)
				{
					this.btnEnable_OnPress(this.btnEnable.Controller, -1);
					return;
				}
			}
			else if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
			{
				if (this.actionEntry != null)
				{
					if (guiactions.HalfStack.WasPressed)
					{
						this.btnEnable_OnPress(this.btnEnable.Controller, -1);
					}
					if (guiactions.Inspect.IsPressed)
					{
						if (guiactions.DPad_Left.WasPressed)
						{
							this.BtnDecrease_OnPressed(null, -1);
						}
						if (guiactions.DPad_Right.WasPressed)
						{
							this.BtnIncrease_OnPressed(null, -1);
							return;
						}
					}
				}
				else if (this.voteEntry != null && guiactions.HalfStack.WasPressed)
				{
					this.btnEnable_OnPress(this.btnEnable.Controller, -1);
				}
			}
		}
	}

	// Token: 0x06008763 RID: 34659 RVA: 0x0033A0D2 File Offset: 0x003382D2
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDecrease_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.currentTwitchActionEntry != null)
		{
			this.currentTwitchActionEntry.DecreaseCost();
			base.RefreshBindings();
		}
	}

	// Token: 0x06008764 RID: 34660 RVA: 0x0033A0ED File Offset: 0x003382ED
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnIncrease_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.currentTwitchActionEntry != null)
		{
			this.currentTwitchActionEntry.IncreaseCost();
			base.RefreshBindings();
		}
	}

	// Token: 0x06008765 RID: 34661 RVA: 0x0033A108 File Offset: 0x00338308
	[PublicizedFrom(EAccessModifier.Private)]
	public void RectStat_OnPress(XUiController _sender, int _mouseButton)
	{
		if (!this.showBitTotal)
		{
			this.showBitTotal = true;
			base.RefreshBindings();
			_sender.ViewComponent.EventOnPress = false;
		}
	}

	// Token: 0x06008766 RID: 34662 RVA: 0x0033A12B File Offset: 0x0033832B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRefund_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.currentTwitchActionHistoryEntry != null)
		{
			this.currentTwitchActionHistoryEntry.Refund();
			base.RefreshBindings();
		}
	}

	// Token: 0x06008767 RID: 34663 RVA: 0x0033A146 File Offset: 0x00338346
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRetry_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.currentTwitchActionHistoryEntry != null)
		{
			this.currentTwitchActionHistoryEntry.Retry();
			base.RefreshBindings();
		}
	}

	// Token: 0x06008768 RID: 34664 RVA: 0x0033A164 File Offset: 0x00338364
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnEnable_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.currentTwitchActionEntry != null)
		{
			TwitchManager twitchManager = TwitchManager.Current;
			TwitchActionPreset currentActionPreset = twitchManager.CurrentActionPreset;
			if (this.currentTwitchActionEntry.IsInPresetDefault(currentActionPreset))
			{
				if (currentActionPreset.RemovedActions.Contains(this.currentTwitchActionEntry.Name))
				{
					currentActionPreset.RemovedActions.Remove(this.currentTwitchActionEntry.Name);
				}
				else
				{
					currentActionPreset.RemovedActions.Add(this.currentTwitchActionEntry.Name);
				}
			}
			else if (currentActionPreset.AddedActions.Contains(this.currentTwitchActionEntry.Name))
			{
				currentActionPreset.AddedActions.Remove(this.currentTwitchActionEntry.Name);
			}
			else
			{
				currentActionPreset.AddedActions.Add(this.currentTwitchActionEntry.Name);
				if (this.currentTwitchActionEntry.DisplayCategory.Name == "Extras")
				{
					QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.EnableExtras, this.currentTwitchActionEntry.DisplayCategory.Name);
				}
			}
			twitchManager.HandleChangedPropertyList();
			Manager.PlayInsidePlayerHead("craft_click_craft", -1, 0f, false, false);
			twitchManager.SetupAvailableCommands();
			twitchManager.HandleCooldownActionLocking();
			base.RefreshBindings();
			this.actionEntry.RefreshBindings();
			return;
		}
		if (this.currentTwitchVoteEntry != null)
		{
			this.currentTwitchVoteEntry.Enabled = !this.currentTwitchVoteEntry.Enabled;
			TwitchManager.Current.HandleChangedPropertyList();
			Manager.PlayInsidePlayerHead("craft_click_craft", -1, 0f, false, false);
			base.RefreshBindings();
		}
	}

	// Token: 0x06008769 RID: 34665 RVA: 0x0033A2D8 File Offset: 0x003384D8
	public override void OnOpen()
	{
		base.OnOpen();
		this.stats = TwitchManager.LeaderboardStats;
		this.stats.StatsChanged += this.Stats_StatsChanged;
		base.RefreshBindings();
	}

	// Token: 0x0600876A RID: 34666 RVA: 0x0033A308 File Offset: 0x00338508
	public override void OnClose()
	{
		base.OnClose();
		this.stats.StatsChanged -= this.Stats_StatsChanged;
		TwitchManager.Current.HandleChangedPropertyList();
		TwitchManager.Current.ResetPrices();
	}

	// Token: 0x0600876B RID: 34667 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Stats_StatsChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x0600876C RID: 34668 RVA: 0x0033A33C File Offset: 0x0033853C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2236179277U)
		{
			if (num <= 1143702022U)
			{
				if (num <= 750543317U)
				{
					if (num <= 337301254U)
					{
						if (num <= 118022627U)
						{
							if (num != 59652435U)
							{
								if (num == 118022627U)
								{
									if (bindingName == "showhistory_event")
									{
										value = (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.EventEntry != null).ToString();
										return true;
									}
								}
							}
							else if (bindingName == "actionrandomgroup")
							{
								value = ((this.currentTwitchActionEntry != null) ? ((this.currentTwitchActionEntry.RandomGroup != "") ? this.lblTrue : this.lblFalse) : "");
								return true;
							}
						}
						else if (num != 140690669U)
						{
							if (num == 337301254U)
							{
								if (bindingName == "voteendgamestage")
								{
									if (this.currentTwitchVoteEntry != null && this.currentTwitchVoteEntry.EndGameStage > 0)
									{
										value = this.currentTwitchVoteEntry.EndGameStage.ToString();
									}
									else
									{
										value = "";
									}
									return true;
								}
							}
						}
						else if (bindingName == "actiondefaultcost")
						{
							if (this.currentTwitchActionEntry != null)
							{
								value = this.currentTwitchActionEntry.ModifiedCost.ToString();
							}
							else if (this.currentTwitchActionHistoryEntry != null)
							{
								value = this.currentTwitchActionHistoryEntry.PointsSpent.ToString();
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
					else if (num <= 422696738U)
					{
						if (num != 362072451U)
						{
							if (num == 422696738U)
							{
								if (bindingName == "currentgood_title")
								{
									value = ((this.stats != null) ? string.Format(this.lblCurrentGood, this.stats.GoodRewardTime) : "");
									return true;
								}
							}
						}
						else if (bindingName == "sessiontotalgood_title")
						{
							value = this.lblTotalGood;
							return true;
						}
					}
					else if (num != 718761959U)
					{
						if (num == 750543317U)
						{
							if (bindingName == "decreasepricetext")
							{
								value = "";
								if (this.currentTwitchActionEntry != null)
								{
									value = ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard) ? this.lblDecreasePrice : this.lblDecreasePrice_Controller);
								}
								return true;
							}
						}
					}
					else if (bindingName == "entrydescription")
					{
						if (this.currentTwitchActionEntry != null)
						{
							value = this.currentTwitchActionEntry.Description;
						}
						else if (this.currentTwitchVoteEntry != null)
						{
							value = this.currentTwitchVoteEntry.Description;
						}
						else if (this.currentTwitchActionHistoryEntry != null)
						{
							value = this.currentTwitchActionHistoryEntry.Description;
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
				else if (num <= 853102448U)
				{
					if (num <= 825770997U)
					{
						if (num != 782028412U)
						{
							if (num == 825770997U)
							{
								if (bindingName == "sessionkiller_title")
								{
									value = this.lblTopKiller;
									return true;
								}
							}
						}
						else if (bindingName == "actioncommand")
						{
							if (this.currentTwitchActionEntry != null)
							{
								value = this.currentTwitchActionEntry.Command;
							}
							else if (this.currentTwitchActionHistoryEntry != null)
							{
								value = this.currentTwitchActionHistoryEntry.UserName;
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
					else if (num != 831944002U)
					{
						if (num == 853102448U)
						{
							if (bindingName == "historystate")
							{
								value = ((this.currentTwitchActionHistoryEntry != null) ? this.currentTwitchActionHistoryEntry.EntryState.ToString() : "");
								return true;
							}
						}
					}
					else if (bindingName == "historytarget")
					{
						if (this.currentTwitchActionHistoryEntry != null)
						{
							value = this.currentTwitchActionHistoryEntry.Target;
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
				else if (num <= 900437321U)
				{
					if (num != 884373603U)
					{
						if (num == 900437321U)
						{
							if (bindingName == "leaderboard_goodperson")
							{
								value = ((this.stats != null && this.stats.TopGoodViewer != null) ? string.Format("[{0}]{1}[-] ({2})", this.stats.TopGoodViewer.UserColor, this.stats.TopGoodViewer.Name, this.stats.TopGoodViewer.GoodActions) : "--");
								return true;
							}
						}
					}
					else if (bindingName == "showleaderboard")
					{
						value = (this.OwnerList != null && this.OwnerList.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Leaderboard).ToString();
						return true;
					}
				}
				else if (num != 930150597U)
				{
					if (num != 1079842190U)
					{
						if (num == 1143702022U)
						{
							if (bindingName == "historystatetitle")
							{
								value = this.lblHistoryStateTitle;
								return true;
							}
						}
					}
					else if (bindingName == "leaderboard_mostbits")
					{
						value = ((this.stats != null && this.stats.MostBitsSpentViewer != null) ? string.Format("[{0}]{1}[-] ({2})", this.stats.MostBitsSpentViewer.UserColor, this.stats.MostBitsSpentViewer.Name, this.stats.MostBitsSpentViewer.BitsUsed) : "--");
						return true;
					}
				}
				else if (bindingName == "leaderboard_totalbits")
				{
					if (this.showBitTotal)
					{
						value = ((this.stats != null) ? this.stats.TotalBits.ToString() : "0");
					}
					else
					{
						value = string.Format("<{0}>", this.lblShowBitTotal);
					}
					return true;
				}
			}
			else if (num <= 1548958396U)
			{
				if (num <= 1300396129U)
				{
					if (num <= 1182727743U)
					{
						if (num != 1152134642U)
						{
							if (num == 1182727743U)
							{
								if (bindingName == "leaderboard_currentgood")
								{
									value = ((this.stats != null && this.stats.CurrentGoodViewer != null) ? string.Format("[{0}]{1}[-] ({2})", this.stats.CurrentGoodViewer.UserColor, this.stats.CurrentGoodViewer.Name, this.stats.CurrentGoodViewer.CurrentGoodActions) : "--");
									return true;
								}
							}
						}
						else if (bindingName == "entryicon")
						{
							value = "";
							if (this.currentTwitchActionEntry != null && this.currentTwitchActionEntry.MainCategory != null)
							{
								value = this.currentTwitchActionEntry.MainCategory.Icon;
							}
							else if (this.currentTwitchVoteEntry != null && this.currentTwitchVoteEntry.MainVoteType != null)
							{
								value = this.currentTwitchVoteEntry.MainVoteType.Icon;
							}
							else if (this.currentTwitchActionHistoryEntry != null)
							{
								if (this.currentTwitchActionHistoryEntry.Action != null)
								{
									TwitchAction action = this.currentTwitchActionHistoryEntry.Action;
									if (action.MainCategory != null)
									{
										value = action.MainCategory.Icon;
									}
								}
								else if (this.currentTwitchActionHistoryEntry.Vote != null)
								{
									TwitchVoteType mainVoteType = this.currentTwitchActionHistoryEntry.Vote.MainVoteType;
									if (mainVoteType != null)
									{
										value = mainVoteType.Icon;
									}
								}
							}
							return true;
						}
					}
					else if (num != 1187804961U)
					{
						if (num == 1300396129U)
						{
							if (bindingName == "votestartgamestagetitle")
							{
								value = this.lblStartGamestage;
								return true;
							}
						}
					}
					else if (bindingName == "historytimestamptitle")
					{
						value = this.lblHistoryTimeStampTitle;
						return true;
					}
				}
				else if (num <= 1413159236U)
				{
					if (num != 1383999083U)
					{
						if (num == 1413159236U)
						{
							if (bindingName == "actionispositive")
							{
								value = ((this.currentTwitchActionEntry != null) ? (this.currentTwitchActionEntry.IsPositive ? this.lblTrue : this.lblFalse) : "");
								return true;
							}
						}
					}
					else if (bindingName == "showhistory_action")
					{
						value = (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.Action != null).ToString();
						return true;
					}
				}
				else if (num != 1453562025U)
				{
					if (num == 1548958396U)
					{
						if (bindingName == "actioncooldowntitle")
						{
							value = this.lblCooldown;
							return true;
						}
					}
				}
				else if (bindingName == "actioncostcolor")
				{
					value = "222,206,163,255";
					if (this.currentTwitchActionEntry != null)
					{
						int num2 = this.currentTwitchActionEntry.ModifiedCost - this.currentTwitchActionEntry.DefaultCost;
						if (num2 != 0)
						{
							if (num2 > 0)
							{
								value = "255,0,0,255";
							}
							else if (num2 < 0)
							{
								value = "0,255,0,255";
							}
						}
					}
					return true;
				}
			}
			else if (num <= 1835511105U)
			{
				if (num <= 1715807981U)
				{
					if (num != 1589536157U)
					{
						if (num == 1715807981U)
						{
							if (bindingName == "actiondiscountcost")
							{
								if (this.currentTwitchActionEntry != null && TwitchManager.Current.BitPriceMultiplier != 1f && this.currentTwitchActionEntry.PointType == TwitchAction.PointTypes.Bits && !this.currentTwitchActionEntry.IgnoreDiscount)
								{
									value = string.Format("{0} {1}", this.currentTwitchActionEntry.GetModifiedDiscountCost(), this.lblPointsBits);
								}
								else
								{
									value = "";
								}
								return true;
							}
						}
					}
					else if (bindingName == "actiongamestage")
					{
						value = ((this.currentTwitchActionEntry != null) ? this.currentTwitchActionEntry.StartGameStage.ToString() : "");
						return true;
					}
				}
				else if (num != 1780150248U)
				{
					if (num == 1835511105U)
					{
						if (bindingName == "actiondefaultcosttitle")
						{
							value = this.lblPointCost;
							return true;
						}
					}
				}
				else if (bindingName == "sessionevil_title")
				{
					value = this.lblTopEvil;
					return true;
				}
			}
			else if (num <= 1882578559U)
			{
				if (num != 1841054916U)
				{
					if (num == 1882578559U)
					{
						if (bindingName == "sessiontotalactions_title")
						{
							value = this.lblTotalActions;
							return true;
						}
					}
				}
				else if (bindingName == "historytargettitle")
				{
					value = this.lblHistoryTargetTitle;
					return true;
				}
			}
			else if (num != 2003818719U)
			{
				if (num != 2170302190U)
				{
					if (num == 2236179277U)
					{
						if (bindingName == "historytimestamp")
						{
							value = ((this.currentTwitchActionHistoryEntry != null) ? this.currentTwitchActionHistoryEntry.ActionTime : "");
							return true;
						}
					}
				}
				else if (bindingName == "showaction")
				{
					value = (this.currentTwitchActionEntry != null).ToString();
					return true;
				}
			}
			else if (bindingName == "sessiontotalbad_title")
			{
				value = this.lblTotalBad;
				return true;
			}
		}
		else if (num <= 3257770903U)
		{
			if (num <= 2818986817U)
			{
				if (num <= 2493832296U)
				{
					if (num <= 2415022578U)
					{
						if (num != 2373030536U)
						{
							if (num == 2415022578U)
							{
								if (bindingName == "actionispositivetitle")
								{
									value = this.lblIsPositive;
									return true;
								}
							}
						}
						else if (bindingName == "leaderboard_sessionkiller")
						{
							value = ((this.stats != null && this.stats.TopKillerViewer != null) ? string.Format("[{0}]{1}[-] ({2})", this.stats.TopKillerViewer.UserColor, this.stats.TopKillerViewer.Name, this.stats.TopKillerViewer.Kills) : "--");
							return true;
						}
					}
					else if (num != 2476815079U)
					{
						if (num == 2493832296U)
						{
							if (bindingName == "refundtext")
							{
								if (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.CanRefund())
								{
									value = string.Format(this.lblRefund, this.GetHistoryPointCost());
								}
								else
								{
									value = this.lblNoRefund;
								}
								return true;
							}
						}
					}
					else if (bindingName == "actionpointtypetitle")
					{
						value = this.lblPointType;
						return true;
					}
				}
				else if (num <= 2646313357U)
				{
					if (num != 2493865531U)
					{
						if (num == 2646313357U)
						{
							if (bindingName == "votestartgamestage")
							{
								if (this.currentTwitchVoteEntry != null && this.currentTwitchVoteEntry.StartGameStage > 0)
								{
									value = this.currentTwitchVoteEntry.StartGameStage.ToString();
								}
								else
								{
									value = "";
								}
								return true;
							}
						}
					}
					else if (bindingName == "sessiongood_title")
					{
						value = this.lblTopGood;
						return true;
					}
				}
				else if (num != 2658197062U)
				{
					if (num == 2818986817U)
					{
						if (bindingName == "actiondiscountcosttitle")
						{
							if (this.currentTwitchActionEntry != null && TwitchManager.Current.BitPriceMultiplier != 1f && this.currentTwitchActionEntry.PointType == TwitchAction.PointTypes.Bits && !this.currentTwitchActionEntry.IgnoreDiscount)
							{
								value = this.lblDiscountCost;
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "leaderboard_largestpot")
				{
					string text = ((TwitchManager.Current.PimpPotType == TwitchManager.PimpPotSettings.EnabledSP) ? Localization.Get("TwitchPoints_SP", false, null) : Localization.Get("TwitchPoints_PP", false, null));
					value = string.Format("{0} {1}", (this.stats != null) ? this.stats.LargestPimpPot : 0, text);
					return true;
				}
			}
			else if (num <= 3051633273U)
			{
				if (num <= 2958318179U)
				{
					if (num != 2846886419U)
					{
						if (num == 2958318179U)
						{
							if (bindingName == "entrytitle")
							{
								if (this.currentTwitchActionEntry != null)
								{
									value = this.currentTwitchActionEntry.Title;
								}
								else if (this.currentTwitchVoteEntry != null)
								{
									value = this.currentTwitchVoteEntry.VoteDescription;
								}
								else if (this.currentTwitchActionHistoryEntry != null)
								{
									value = this.currentTwitchActionHistoryEntry.Title;
								}
								else if (this.OwnerList != null && this.OwnerList.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Leaderboard)
								{
									value = this.lblLeaderboardStats;
								}
								else
								{
									value = "";
								}
								return true;
							}
						}
					}
					else if (bindingName == "sessionlargestpimppot_title")
					{
						value = this.lblLargestPimpPot;
						return true;
					}
				}
				else if (num != 3051255413U)
				{
					if (num == 3051633273U)
					{
						if (bindingName == "increasepricetext")
						{
							value = "";
							if (this.currentTwitchActionEntry != null)
							{
								value = ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard) ? this.lblIncreasePrice : this.lblIncreasePrice_Controller);
							}
							return true;
						}
					}
				}
				else if (bindingName == "sessionmostbits_title")
				{
					value = this.lblMostBits;
					return true;
				}
			}
			else if (num <= 3184965712U)
			{
				if (num != 3144329307U)
				{
					if (num == 3184965712U)
					{
						if (bindingName == "showvote")
						{
							value = (this.currentTwitchVoteEntry != null).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "showenable")
				{
					value = (this.currentTwitchActionEntry != null || this.currentTwitchVoteEntry != null).ToString();
					return true;
				}
			}
			else if (num != 3213443644U)
			{
				if (num != 3214603061U)
				{
					if (num == 3257770903U)
					{
						if (bindingName == "showstats")
						{
							value = (this.currentTwitchActionEntry != null || this.currentTwitchVoteEntry != null || this.currentTwitchActionHistoryEntry != null || (this.OwnerList != null && this.OwnerList.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Leaderboard)).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "showhistory_retry")
				{
					value = (this.currentTwitchActionHistoryEntry != null && (this.currentTwitchActionHistoryEntry.Action != null || this.currentTwitchActionHistoryEntry.EventEntry != null)).ToString();
					return true;
				}
			}
			else if (bindingName == "enablerefund")
			{
				value = (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.CanRefund()).ToString();
				return true;
			}
		}
		else if (num <= 3639271085U)
		{
			if (num <= 3487000492U)
			{
				if (num <= 3314030970U)
				{
					if (num != 3275561864U)
					{
						if (num == 3314030970U)
						{
							if (bindingName == "retrytext")
							{
								if (this.currentTwitchActionHistoryEntry != null)
								{
									if (this.currentTwitchActionHistoryEntry.CanRetry())
									{
										value = this.lblRetry;
									}
									else
									{
										value = (this.currentTwitchActionHistoryEntry.HasRetried ? this.lblNoRetry : this.lblRetryActionUnavailable);
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
					else if (bindingName == "leaderboard_totalactions")
					{
						value = ((this.stats != null) ? this.stats.TotalActions.ToString() : "0");
						return true;
					}
				}
				else if (num != 3387479438U)
				{
					if (num == 3487000492U)
					{
						if (bindingName == "showhistory")
						{
							value = (this.currentTwitchActionHistoryEntry != null).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "actionpointcost")
				{
					if (this.currentTwitchActionEntry != null)
					{
						switch (this.currentTwitchActionEntry.PointType)
						{
						case TwitchAction.PointTypes.PP:
							value = string.Format("{0} {1}", this.currentTwitchActionEntry.ModifiedCost, this.lblPointsPP);
							break;
						case TwitchAction.PointTypes.SP:
							value = string.Format("{0} {1}", this.currentTwitchActionEntry.ModifiedCost, this.lblPointsSP);
							break;
						case TwitchAction.PointTypes.Bits:
							value = string.Format("{0} {1}", this.currentTwitchActionEntry.ModifiedCost, this.lblPointsBits);
							break;
						}
					}
					else if (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.Action != null)
					{
						value = this.GetHistoryPointCost();
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num <= 3526138964U)
			{
				if (num != 3500355537U)
				{
					if (num == 3526138964U)
					{
						if (bindingName == "enableretry")
						{
							value = (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.CanRetry()).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "actiongamestagetitle")
				{
					value = this.lblStartGamestage;
					return true;
				}
			}
			else if (num != 3616278858U)
			{
				if (num == 3639271085U)
				{
					if (bindingName == "enablebuttontext")
					{
						value = "";
						if (this.currentTwitchActionEntry != null)
						{
							if (this.currentTwitchActionEntry.IsInPreset(TwitchManager.Current.CurrentActionPreset))
							{
								value = ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard) ? this.lblDisableAction : this.lblDisableAction_Controller);
							}
							else
							{
								value = ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard) ? this.lblEnableAction : this.lblEnableAction_Controller);
							}
						}
						else if (this.currentTwitchVoteEntry != null)
						{
							if (this.currentTwitchVoteEntry.Enabled)
							{
								value = ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard) ? this.lblDisableVote : this.lblDisableVote_Controller);
							}
							else
							{
								value = ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard) ? this.lblEnableVote : this.lblEnableVote_Controller);
							}
						}
						return true;
					}
				}
			}
			else if (bindingName == "actioncooldown")
			{
				value = ((this.currentTwitchActionEntry != null) ? XUiM_PlayerBuffs.ConvertToTimeString(this.currentTwitchActionEntry.Cooldown) : "");
				return true;
			}
		}
		else if (num <= 4060322893U)
		{
			if (num <= 3745003420U)
			{
				if (num != 3649566128U)
				{
					if (num == 3745003420U)
					{
						if (bindingName == "leaderboard_totalgood")
						{
							value = string.Format("[AFAFFF]{0}[-]", (this.stats != null) ? this.stats.TotalGood.ToString() : "0");
							return true;
						}
					}
				}
				else if (bindingName == "voteendgamestagetitle")
				{
					value = this.lblEndGamestage;
					return true;
				}
			}
			else if (num != 3755168009U)
			{
				if (num == 4060322893U)
				{
					if (bindingName == "showempty")
					{
						value = (this.currentTwitchActionEntry == null && this.currentTwitchVoteEntry == null && this.currentTwitchActionHistoryEntry == null && this.OwnerList != null && this.OwnerList.CurrentType != XUiC_TwitchEntryListWindow.ListTypes.Leaderboard).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "showhistory_vote")
			{
				value = (this.currentTwitchActionHistoryEntry != null && this.currentTwitchActionHistoryEntry.Vote != null).ToString();
				return true;
			}
		}
		else if (num <= 4142793257U)
		{
			if (num != 4101101692U)
			{
				if (num == 4142793257U)
				{
					if (bindingName == "leaderboard_badperson")
					{
						value = ((this.stats != null && this.stats.TopBadViewer != null) ? string.Format("[{0}]{1}[-] ({2})", this.stats.TopBadViewer.UserColor, this.stats.TopBadViewer.Name, this.stats.TopBadViewer.BadActions) : "--");
						return true;
					}
				}
			}
			else if (bindingName == "leaderboard_totalbad")
			{
				value = string.Format("[FFAFAF]{0}[-]", (this.stats != null) ? this.stats.TotalBad.ToString() : "0");
				return true;
			}
		}
		else if (num != 4227361571U)
		{
			if (num != 4259958381U)
			{
				if (num == 4292913866U)
				{
					if (bindingName == "sessiontotalbits_title")
					{
						value = this.lblTotalBits;
						return true;
					}
				}
			}
			else if (bindingName == "emptytext")
			{
				value = "";
				if (this.OwnerList != null)
				{
					switch (this.OwnerList.CurrentType)
					{
					case XUiC_TwitchEntryListWindow.ListTypes.Actions:
						value = this.lblActionEmpty;
						break;
					case XUiC_TwitchEntryListWindow.ListTypes.Votes:
						value = this.lblVoteEmpty;
						break;
					case XUiC_TwitchEntryListWindow.ListTypes.ActionHistory:
						value = this.lblActionHistoryEmpty;
						break;
					case XUiC_TwitchEntryListWindow.ListTypes.Leaderboard:
						value = this.lblLeaderboardEmpty;
						break;
					}
				}
				return true;
			}
		}
		else if (bindingName == "actionrandomgrouptitle")
		{
			value = this.lblRandomDaily;
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600876D RID: 34669 RVA: 0x0033BA7C File Offset: 0x00339C7C
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetHistoryPointCost()
	{
		if (this.currentTwitchActionHistoryEntry == null || this.currentTwitchActionHistoryEntry.Action == null)
		{
			return "";
		}
		switch (this.currentTwitchActionHistoryEntry.Action.PointType)
		{
		case TwitchAction.PointTypes.PP:
			return string.Format("{0} {1}", this.currentTwitchActionHistoryEntry.PointsSpent, this.lblPointsPP);
		case TwitchAction.PointTypes.SP:
			return string.Format("{0} {1}", this.currentTwitchActionHistoryEntry.PointsSpent, this.lblPointsSP);
		case TwitchAction.PointTypes.Bits:
			return string.Format("{0} {1}", this.currentTwitchActionHistoryEntry.PointsSpent, this.lblPointsBits);
		default:
			return "";
		}
	}

	// Token: 0x0600876E RID: 34670 RVA: 0x0033BB34 File Offset: 0x00339D34
	public void SetTwitchAction(XUiC_TwitchActionEntry twitchInfoEntry)
	{
		this.actionEntry = twitchInfoEntry;
		this.voteEntry = null;
		this.CurrentTwitchVoteEntry = null;
		this.historyEntry = null;
		this.CurrentTwitchActionHistoryEntry = null;
		if (this.actionEntry != null)
		{
			this.CurrentTwitchActionEntry = this.actionEntry.Action;
			return;
		}
		this.CurrentTwitchActionEntry = null;
	}

	// Token: 0x0600876F RID: 34671 RVA: 0x0033BB88 File Offset: 0x00339D88
	public void SetTwitchVote(XUiC_TwitchVoteInfoEntry twitchInfoEntry)
	{
		this.voteEntry = twitchInfoEntry;
		this.actionEntry = null;
		this.CurrentTwitchActionEntry = null;
		this.historyEntry = null;
		this.CurrentTwitchActionHistoryEntry = null;
		if (this.voteEntry != null)
		{
			this.CurrentTwitchVoteEntry = this.voteEntry.Vote;
			return;
		}
		this.CurrentTwitchVoteEntry = null;
	}

	// Token: 0x06008770 RID: 34672 RVA: 0x0033BBDC File Offset: 0x00339DDC
	public void SetTwitchHistory(XUiC_TwitchActionHistoryEntry twitchInfoEntry)
	{
		this.historyEntry = twitchInfoEntry;
		this.actionEntry = null;
		this.CurrentTwitchActionEntry = null;
		this.voteEntry = null;
		this.CurrentTwitchVoteEntry = null;
		if (this.historyEntry != null)
		{
			this.CurrentTwitchActionHistoryEntry = this.historyEntry.HistoryItem;
			return;
		}
		this.CurrentTwitchActionHistoryEntry = null;
	}

	// Token: 0x06008771 RID: 34673 RVA: 0x0033BC2D File Offset: 0x00339E2D
	public void ClearEntries()
	{
		this.actionEntry = null;
		this.CurrentTwitchActionEntry = null;
		this.voteEntry = null;
		this.CurrentTwitchVoteEntry = null;
		this.historyEntry = null;
		this.CurrentTwitchActionHistoryEntry = null;
	}

	// Token: 0x040064A1 RID: 25761
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionEntry actionEntry;

	// Token: 0x040064A2 RID: 25762
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchVoteInfoEntry voteEntry;

	// Token: 0x040064A3 RID: 25763
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionHistoryEntry historyEntry;

	// Token: 0x040064A4 RID: 25764
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnEnable;

	// Token: 0x040064A5 RID: 25765
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRefund;

	// Token: 0x040064A6 RID: 25766
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnRetry;

	// Token: 0x040064A7 RID: 25767
	public XUiC_TwitchEntryListWindow OwnerList;

	// Token: 0x040064A8 RID: 25768
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction currentTwitchActionEntry;

	// Token: 0x040064A9 RID: 25769
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVote currentTwitchVoteEntry;

	// Token: 0x040064AA RID: 25770
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchActionHistoryEntry currentTwitchActionHistoryEntry;

	// Token: 0x040064AB RID: 25771
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStartGamestage;

	// Token: 0x040064AC RID: 25772
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblEndGamestage;

	// Token: 0x040064AD RID: 25773
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPointCost;

	// Token: 0x040064AE RID: 25774
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCooldown;

	// Token: 0x040064AF RID: 25775
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblRandomDaily;

	// Token: 0x040064B0 RID: 25776
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblIsPositive;

	// Token: 0x040064B1 RID: 25777
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPointType;

	// Token: 0x040064B2 RID: 25778
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblEnableAction;

	// Token: 0x040064B3 RID: 25779
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDisableAction;

	// Token: 0x040064B4 RID: 25780
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblEnableVote;

	// Token: 0x040064B5 RID: 25781
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDisableVote;

	// Token: 0x040064B6 RID: 25782
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblActionEmpty;

	// Token: 0x040064B7 RID: 25783
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblVoteEmpty;

	// Token: 0x040064B8 RID: 25784
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblActionHistoryEmpty;

	// Token: 0x040064B9 RID: 25785
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblLeaderboardEmpty;

	// Token: 0x040064BA RID: 25786
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblHistoryTargetTitle;

	// Token: 0x040064BB RID: 25787
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblHistoryStateTitle;

	// Token: 0x040064BC RID: 25788
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblHistoryTimeStampTitle;

	// Token: 0x040064BD RID: 25789
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblRefund;

	// Token: 0x040064BE RID: 25790
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblNoRefund;

	// Token: 0x040064BF RID: 25791
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblRetry;

	// Token: 0x040064C0 RID: 25792
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblNoRetry;

	// Token: 0x040064C1 RID: 25793
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblRetryActionUnavailable;

	// Token: 0x040064C2 RID: 25794
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblLeaderboardStats;

	// Token: 0x040064C3 RID: 25795
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblShowBitTotal;

	// Token: 0x040064C4 RID: 25796
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDiscountCost;

	// Token: 0x040064C5 RID: 25797
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblIncreasePrice;

	// Token: 0x040064C6 RID: 25798
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDecreasePrice;

	// Token: 0x040064C7 RID: 25799
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblEnableAction_Controller;

	// Token: 0x040064C8 RID: 25800
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDisableAction_Controller;

	// Token: 0x040064C9 RID: 25801
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblEnableVote_Controller;

	// Token: 0x040064CA RID: 25802
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDisableVote_Controller;

	// Token: 0x040064CB RID: 25803
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblIncreasePrice_Controller;

	// Token: 0x040064CC RID: 25804
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblDecreasePrice_Controller;

	// Token: 0x040064CD RID: 25805
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTopKiller;

	// Token: 0x040064CE RID: 25806
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTopGood;

	// Token: 0x040064CF RID: 25807
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTopEvil;

	// Token: 0x040064D0 RID: 25808
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCurrentGood;

	// Token: 0x040064D1 RID: 25809
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblMostBits;

	// Token: 0x040064D2 RID: 25810
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotalBits;

	// Token: 0x040064D3 RID: 25811
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotalBad;

	// Token: 0x040064D4 RID: 25812
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotalGood;

	// Token: 0x040064D5 RID: 25813
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotalActions;

	// Token: 0x040064D6 RID: 25814
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblLargestPimpPot;

	// Token: 0x040064D7 RID: 25815
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTrue;

	// Token: 0x040064D8 RID: 25816
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblFalse;

	// Token: 0x040064D9 RID: 25817
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPointsPP;

	// Token: 0x040064DA RID: 25818
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPointsSP;

	// Token: 0x040064DB RID: 25819
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPointsBits;

	// Token: 0x040064DC RID: 25820
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchLeaderboardStats stats;

	// Token: 0x040064DD RID: 25821
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBitTotal;

	// Token: 0x040064DE RID: 25822
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string DayTimeFormatString = Localization.Get("xuiDay", false, null) + "{0}, {1:00}:{2:00}";

	// Token: 0x040064DF RID: 25823
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<ulong> daytimeFormatter = new CachedStringFormatter<ulong>((ulong _worldTime) => ValueDisplayFormatters.WorldTime(_worldTime, XUiC_TwitchEntryDescriptionWindow.DayTimeFormatString));
}

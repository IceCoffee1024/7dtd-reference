using System;
using Audio;
using Challenges;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000EE6 RID: 3814
[Preserve]
public class XUiC_ChallengeEntryDescriptionWindow : XUiController
{
	// Token: 0x17000E23 RID: 3619
	// (get) Token: 0x06007720 RID: 30496 RVA: 0x002CEB3B File Offset: 0x002CCD3B
	// (set) Token: 0x06007721 RID: 30497 RVA: 0x002CEB43 File Offset: 0x002CCD43
	public Challenge CurrentChallengeEntry
	{
		get
		{
			return this.currentChallenge;
		}
		set
		{
			this.currentChallenge = value;
			this.challengeClass = ((this.currentChallenge != null) ? this.currentChallenge.ChallengeClass : null);
			base.RefreshBindings();
		}
	}

	// Token: 0x06007722 RID: 30498 RVA: 0x002CEB70 File Offset: 0x002CCD70
	public override void Init()
	{
		base.Init();
		this.btnTrack = base.GetChildById("btnTrack").GetChildByType<XUiC_SimpleButton>();
		this.btnTrack.OnPressed += this.BtnTrack_OnPressed;
		this.btnComplete = base.GetChildById("btnComplete").GetChildByType<XUiC_SimpleButton>();
		this.btnComplete.OnPressed += this.BtnComplete_OnPressed;
		this.gotoButton = base.GetChildById("gotoButton");
		if (this.gotoButton != null)
		{
			this.gotoButton.OnPress += this.GotoButton_OnPress;
		}
		base.registerForInputStyleChanges();
	}

	// Token: 0x06007723 RID: 30499 RVA: 0x002CEC14 File Offset: 0x002CCE14
	[PublicizedFrom(EAccessModifier.Private)]
	public void GotoButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.currentChallenge != null)
		{
			BaseChallengeObjective navObjective = this.currentChallenge.GetNavObjective();
			ChallengeObjectiveCraft challengeObjectiveCraft = navObjective as ChallengeObjectiveCraft;
			if (challengeObjectiveCraft != null)
			{
				Recipe itemRecipe = challengeObjectiveCraft.itemRecipe;
				XUiC_WindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "crafting");
				XUiC_RecipeList childByType = this.xui.GetChildByType<XUiC_RecipeList>();
				if (childByType != null)
				{
					childByType.SetRecipeDataByItem(challengeObjectiveCraft.itemRecipe.itemValueType);
					return;
				}
			}
			else if (navObjective is ChallengeObjectiveTwitch)
			{
				XUiC_TwitchWindowSelector.OpenSelectorAndWindow(GameManager.Instance.World.GetPrimaryPlayer(), "Actions", true);
			}
		}
	}

	// Token: 0x06007724 RID: 30500 RVA: 0x0029F176 File Offset: 0x0029D376
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.IsDirty = true;
	}

	// Token: 0x06007725 RID: 30501 RVA: 0x002CECA8 File Offset: 0x002CCEA8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty || (this.currentChallenge != null && this.currentChallenge.NeedsUIUpdate))
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		if (base.ViewComponent.IsActiveInHierarchy && this.currentChallenge != null)
		{
			PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && !this.xui.playerUI.windowManager.IsInputActive())
			{
				if (guiactions.DPad_Up.WasPressed)
				{
					this.BtnComplete_OnPressed(this.btnComplete, -1);
				}
				if (guiactions.DPad_Left.WasPressed)
				{
					this.BtnTrack_OnPressed(this.btnTrack, -1);
				}
			}
		}
	}

	// Token: 0x06007726 RID: 30502 RVA: 0x002CED70 File Offset: 0x002CCF70
	public void TrackCurrentChallenege()
	{
		if (this.currentChallenge != null && this.currentChallenge.IsActive)
		{
			if (this.xui.QuestTracker.TrackedChallenge == this.currentChallenge)
			{
				this.xui.QuestTracker.TrackedChallenge = null;
			}
			else
			{
				this.xui.QuestTracker.TrackedChallenge = this.currentChallenge;
				Manager.PlayInsidePlayerHead("ui_challenge_track", -1, 0f, false, false);
			}
			this.entry.Owner.MarkDirty();
		}
	}

	// Token: 0x06007727 RID: 30503 RVA: 0x002CEDF8 File Offset: 0x002CCFF8
	public void CompleteCurrentChallenege()
	{
		if (this.currentChallenge != null && this.currentChallenge.ReadyToComplete)
		{
			this.currentChallenge.ChallengeState = Challenge.ChallengeStates.Redeemed;
			this.currentChallenge.Redeem();
			QuestEventManager.Current.ChallengeCompleted(this.challengeClass, true);
			this.currentChallenge = this.currentChallenge.Owner.GetNextRedeemableChallenge(this.currentChallenge);
			XUiC_ChallengeEntry selectedEntry = XUiC_ChallengeEntryList.SelectedEntry;
			if (selectedEntry != null && selectedEntry.Entry != this.currentChallenge)
			{
				this.entry.Owner.SetEntryByChallenge(this.currentChallenge);
			}
		}
	}

	// Token: 0x06007728 RID: 30504 RVA: 0x002CEE8B File Offset: 0x002CD08B
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnTrack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.TrackCurrentChallenege();
	}

	// Token: 0x06007729 RID: 30505 RVA: 0x002CEE93 File Offset: 0x002CD093
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnComplete_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CompleteCurrentChallenege();
	}

	// Token: 0x0600772A RID: 30506 RVA: 0x002CEE9B File Offset: 0x002CD09B
	public override void OnOpen()
	{
		base.OnOpen();
		this.RefreshButtonLabels(PlatformManager.NativePlatform.Input.CurrentInputStyle);
		QuestEventManager.Current.ChallengeComplete += this.Current_ChallengeComplete;
	}

	// Token: 0x0600772B RID: 30507 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ChallengeComplete(ChallengeClass challenge, bool isRedeemed)
	{
		base.RefreshBindings();
	}

	// Token: 0x0600772C RID: 30508 RVA: 0x002CEECE File Offset: 0x002CD0CE
	public override void OnClose()
	{
		base.OnClose();
		QuestEventManager.Current.ChallengeComplete -= this.Current_ChallengeComplete;
	}

	// Token: 0x0600772D RID: 30509 RVA: 0x002CEEEC File Offset: 0x002CD0EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2281937273U)
		{
			if (num <= 908978632U)
			{
				if (num <= 227073911U)
				{
					if (num <= 193518673U)
					{
						if (num != 176741054U)
						{
							if (num == 193518673U)
							{
								if (bindingName == "hasobjective5")
								{
									if (this.currentChallenge != null)
									{
										value = (this.currentChallenge.ObjectiveList.Count > 4).ToString();
									}
									else
									{
										value = "false";
									}
									return true;
								}
							}
						}
						else if (bindingName == "hasobjective4")
						{
							if (this.currentChallenge != null)
							{
								value = (this.currentChallenge.ObjectiveList.Count > 3).ToString();
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
					else if (num != 210296292U)
					{
						if (num == 227073911U)
						{
							if (bindingName == "hasobjective3")
							{
								if (this.currentChallenge != null)
								{
									value = (this.currentChallenge.ObjectiveList.Count > 2).ToString();
								}
								else
								{
									value = "false";
								}
								return true;
							}
						}
					}
					else if (bindingName == "hasobjective2")
					{
						if (this.currentChallenge != null)
						{
							value = (this.currentChallenge.ObjectiveList.Count > 1).ToString();
						}
						else
						{
							value = "false";
						}
						return true;
					}
				}
				else if (num <= 338523197U)
				{
					if (num != 260629149U)
					{
						if (num == 338523197U)
						{
							if (bindingName == "enabletrack")
							{
								value = (this.currentChallenge != null && this.currentChallenge.ChallengeState == Challenge.ChallengeStates.Active).ToString();
								return true;
							}
						}
					}
					else if (bindingName == "hasobjective1")
					{
						if (this.currentChallenge != null)
						{
							value = (this.currentChallenge.ObjectiveList.Count > 0).ToString();
						}
						else
						{
							value = "false";
						}
						return true;
					}
				}
				else if (num != 718761959U)
				{
					if (num == 908978632U)
					{
						if (bindingName == "has4objective")
						{
							if (this.currentChallenge != null)
							{
								value = (this.currentChallenge.ObjectiveList.Count == 4).ToString();
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "entrydescription")
				{
					value = ((this.currentChallenge != null) ? this.challengeClass.GetDescription() : "");
					return true;
				}
			}
			else if (num <= 1507958455U)
			{
				if (num <= 1152134642U)
				{
					if (num != 1003914668U)
					{
						if (num == 1152134642U)
						{
							if (bindingName == "entryicon")
							{
								value = ((this.currentChallenge != null) ? this.challengeClass.Icon : "");
								return true;
							}
						}
					}
					else if (bindingName == "rewardtitle")
					{
						value = Localization.Get("xuiRewards", false, null);
						return true;
					}
				}
				else if (num != 1498809836U)
				{
					if (num == 1507958455U)
					{
						if (bindingName == "entryshortdescription")
						{
							value = ((this.currentChallenge != null) ? this.challengeClass.ShortDescription : "");
							return true;
						}
					}
				}
				else if (bindingName == "haschallenge")
				{
					value = (this.currentChallenge != null).ToString();
					return true;
				}
			}
			else if (num <= 1833103823U)
			{
				if (num != 1518662865U)
				{
					if (num == 1833103823U)
					{
						if (bindingName == "showgoto")
						{
							value = (this.currentChallenge != null && this.currentChallenge.ChallengeClass.HasNavType).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "rewardtext")
				{
					if (this.currentChallenge != null)
					{
						value = (this.currentChallenge.AutoCompleted ? "--" : this.challengeClass.RewardText);
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num != 1960932512U)
			{
				if (num != 2048631988U)
				{
					if (num == 2281937273U)
					{
						if (bindingName == "has1objective")
						{
							if (this.currentChallenge != null)
							{
								value = (this.currentChallenge.ObjectiveList.Count == 1).ToString();
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "hasreward")
				{
					value = ((this.currentChallenge != null) ? (this.challengeClass.RewardEvent != "").ToString() : "false");
					return true;
				}
			}
			else if (bindingName == "adjustedheight")
			{
				if (this.currentChallenge != null)
				{
					switch (this.currentChallenge.ObjectiveList.Count)
					{
					case 1:
						value = "276";
						return true;
					case 2:
						value = "236";
						return true;
					case 3:
						value = "196";
						return true;
					case 4:
						value = "156";
						return true;
					case 5:
						value = "116";
						return true;
					}
				}
				value = "196";
				return true;
			}
		}
		else if (num <= 3256735460U)
		{
			if (num <= 2958318179U)
			{
				if (num <= 2675497066U)
				{
					if (num != 2650623958U)
					{
						if (num == 2675497066U)
						{
							if (bindingName == "entrygroup")
							{
								value = ((this.currentChallenge != null) ? this.currentChallenge.ChallengeGroup.Title : "");
								return true;
							}
						}
					}
					else if (bindingName == "enableredeem")
					{
						value = (this.currentChallenge != null && this.currentChallenge.ReadyToComplete).ToString();
						return true;
					}
				}
				else if (num != 2725401013U)
				{
					if (num == 2958318179U)
					{
						if (bindingName == "entrytitle")
						{
							value = ((this.currentChallenge != null) ? this.challengeClass.Title : "");
							return true;
						}
					}
				}
				else if (bindingName == "has5objective")
				{
					if (this.currentChallenge != null)
					{
						value = (this.currentChallenge.ObjectiveList.Count == 5).ToString();
					}
					else
					{
						value = "false";
					}
					return true;
				}
			}
			else if (num <= 3223180222U)
			{
				if (num != 3206402603U)
				{
					if (num == 3223180222U)
					{
						if (bindingName == "objective2")
						{
							if (this.currentChallenge != null)
							{
								value = ((this.currentChallenge.ObjectiveList.Count > 1) ? this.currentChallenge.ObjectiveList[1].ObjectiveText : "");
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "objective1")
				{
					if (this.currentChallenge != null)
					{
						value = ((this.currentChallenge.ObjectiveList.Count > 0) ? this.currentChallenge.ObjectiveList[0].ObjectiveText : "");
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num != 3239957841U)
			{
				if (num == 3256735460U)
				{
					if (bindingName == "objective4")
					{
						if (this.currentChallenge != null)
						{
							value = ((this.currentChallenge.ObjectiveList.Count > 3) ? this.currentChallenge.ObjectiveList[3].ObjectiveText : "");
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "objective3")
			{
				if (this.currentChallenge != null)
				{
					value = ((this.currentChallenge.ObjectiveList.Count > 2) ? this.currentChallenge.ObjectiveList[2].ObjectiveText : "");
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (num <= 3746182248U)
		{
			if (num <= 3279637982U)
			{
				if (num != 3273513079U)
				{
					if (num == 3279637982U)
					{
						if (bindingName == "has2objective")
						{
							if (this.currentChallenge != null)
							{
								value = (this.currentChallenge.ObjectiveList.Count == 2).ToString();
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "objective5")
				{
					if (this.currentChallenge != null)
					{
						value = ((this.currentChallenge.ObjectiveList.Count > 4) ? this.currentChallenge.ObjectiveList[4].ObjectiveText : "");
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num != 3479566019U)
			{
				if (num == 3746182248U)
				{
					if (bindingName == "objectivefill1")
					{
						if (this.currentChallenge != null)
						{
							value = ((this.currentChallenge.ObjectiveList.Count > 0) ? this.currentChallenge.ObjectiveList[0].FillAmount.ToString() : "0");
						}
						else
						{
							value = "0";
						}
						return true;
					}
				}
			}
			else if (bindingName == "has3objective")
			{
				if (this.currentChallenge != null)
				{
					value = (this.currentChallenge.ObjectiveList.Count == 3).ToString();
				}
				else
				{
					value = "false";
				}
				return true;
			}
		}
		else if (num <= 3796515105U)
		{
			if (num != 3779737486U)
			{
				if (num == 3796515105U)
				{
					if (bindingName == "objectivefill2")
					{
						if (this.currentChallenge != null)
						{
							value = ((this.currentChallenge.ObjectiveList.Count > 1) ? this.currentChallenge.ObjectiveList[1].FillAmount.ToString() : "0");
						}
						else
						{
							value = "0";
						}
						return true;
					}
				}
			}
			else if (bindingName == "objectivefill3")
			{
				if (this.currentChallenge != null)
				{
					value = ((this.currentChallenge.ObjectiveList.Count > 2) ? this.currentChallenge.ObjectiveList[2].FillAmount.ToString() : "0");
				}
				else
				{
					value = "0";
				}
				return true;
			}
		}
		else if (num != 3813292724U)
		{
			if (num != 3830070343U)
			{
				if (num == 4060322893U)
				{
					if (bindingName == "showempty")
					{
						value = (this.currentChallenge == null).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "objectivefill4")
			{
				if (this.currentChallenge != null)
				{
					value = ((this.currentChallenge.ObjectiveList.Count > 3) ? this.currentChallenge.ObjectiveList[3].FillAmount.ToString() : "0");
				}
				else
				{
					value = "0";
				}
				return true;
			}
		}
		else if (bindingName == "objectivefill5")
		{
			if (this.currentChallenge != null)
			{
				value = ((this.currentChallenge.ObjectiveList.Count > 4) ? this.currentChallenge.ObjectiveList[4].FillAmount.ToString() : "0");
			}
			else
			{
				value = "0";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600772E RID: 30510 RVA: 0x002CFAE7 File Offset: 0x002CDCE7
	public void SetChallenge(XUiC_ChallengeEntry challengeEntry)
	{
		this.entry = challengeEntry;
		if (this.entry != null)
		{
			this.CurrentChallengeEntry = this.entry.Entry;
			return;
		}
		this.CurrentChallengeEntry = null;
	}

	// Token: 0x0600772F RID: 30511 RVA: 0x002CFB11 File Offset: 0x002CDD11
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.RefreshButtonLabels(_style);
	}

	// Token: 0x06007730 RID: 30512 RVA: 0x002CFB1C File Offset: 0x002CDD1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshButtonLabels(PlayerInputManager.InputStyle _style)
	{
		if (_style == PlayerInputManager.InputStyle.Keyboard)
		{
			(this.btnTrack.GetChildById("btnLabel").ViewComponent as XUiV_Label).Text = string.Format(Localization.Get("journalTrack", false, null), LocalPlayerUI.primaryUI.playerInput.GUIActions.DPad_Left.GetBindingString(false, PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.KeyboardWithAngleBrackets, false, null));
			(this.btnComplete.GetChildById("btnLabel").ViewComponent as XUiV_Label).Text = string.Format(Localization.Get("journalComplete", false, null), LocalPlayerUI.primaryUI.playerInput.GUIActions.DPad_Up.GetBindingString(false, PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.KeyboardWithAngleBrackets, false, null));
			return;
		}
		(this.btnTrack.GetChildById("btnLabel").ViewComponent as XUiV_Label).Text = string.Format(Localization.Get("journalTrack", false, null), LocalPlayerUI.primaryUI.playerInput.GUIActions.HalfStack.GetBindingString(true, PlatformManager.NativePlatform.Input.CurrentControllerInputStyle, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null));
		(this.btnComplete.GetChildById("btnLabel").ViewComponent as XUiV_Label).Text = string.Format(Localization.Get("journalComplete", false, null), LocalPlayerUI.primaryUI.playerInput.GUIActions.Inspect.GetBindingString(true, PlatformManager.NativePlatform.Input.CurrentControllerInputStyle, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null));
	}

	// Token: 0x04005830 RID: 22576
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeEntry entry;

	// Token: 0x04005831 RID: 22577
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge currentChallenge;

	// Token: 0x04005832 RID: 22578
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x04005833 RID: 22579
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnTrack;

	// Token: 0x04005834 RID: 22580
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnComplete;

	// Token: 0x04005835 RID: 22581
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController gotoButton;

	// Token: 0x04005836 RID: 22582
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string DayTimeFormatString = Localization.Get("xuiDay", false, null) + " {0}, {1:00}:{2:00}";

	// Token: 0x04005837 RID: 22583
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<ulong> daytimeFormatter = new CachedStringFormatter<ulong>((ulong _worldTime) => ValueDisplayFormatters.WorldTime(_worldTime, XUiC_ChallengeEntryDescriptionWindow.DayTimeFormatString));
}

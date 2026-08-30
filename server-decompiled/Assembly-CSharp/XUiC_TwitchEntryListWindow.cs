using System;
using System.Collections.Generic;
using Twitch;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x0200109F RID: 4255
[Preserve]
public class XUiC_TwitchEntryListWindow : XUiController
{
	// Token: 0x17000FCF RID: 4047
	// (get) Token: 0x06008777 RID: 34679 RVA: 0x0033BCC1 File Offset: 0x00339EC1
	// (set) Token: 0x06008778 RID: 34680 RVA: 0x0033BCC9 File Offset: 0x00339EC9
	public string ActionCategory
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FD0 RID: 4048
	// (get) Token: 0x06008779 RID: 34681 RVA: 0x0033BCD2 File Offset: 0x00339ED2
	// (set) Token: 0x0600877A RID: 34682 RVA: 0x0033BCDA File Offset: 0x00339EDA
	public string VoteCategory
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FD1 RID: 4049
	// (get) Token: 0x0600877B RID: 34683 RVA: 0x0033BCE3 File Offset: 0x00339EE3
	// (set) Token: 0x0600877C RID: 34684 RVA: 0x0033BCEB File Offset: 0x00339EEB
	public string ActionHistoryCategory
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FD2 RID: 4050
	// (get) Token: 0x0600877D RID: 34685 RVA: 0x0033BCF4 File Offset: 0x00339EF4
	// (set) Token: 0x0600877E RID: 34686 RVA: 0x0033BCFC File Offset: 0x00339EFC
	public string LeaderboardCategory
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600877F RID: 34687 RVA: 0x0033BD08 File Offset: 0x00339F08
	public override void Init()
	{
		base.Init();
		this.twitchActionEntryList = base.GetChildByType<XUiC_TwitchActionEntryList>();
		this.twitchActionEntryList.TwitchEntryListWindow = this;
		this.twitchVoteInfoEntryList = base.GetChildByType<XUiC_TwitchVoteInfoEntryList>();
		this.twitchVoteInfoEntryList.TwitchEntryListWindow = this;
		this.twitchActionHistoryEntryList = base.GetChildByType<XUiC_TwitchActionHistoryEntryList>();
		this.twitchActionHistoryEntryList.TwitchEntryListWindow = this;
		this.twitchLeaderboardEntryList = base.GetChildByType<XUiC_TwitchLeaderboardEntryList>();
		this.twitchLeaderboardEntryList.TwitchEntryListWindow = this;
		this.categoryList = (XUiC_CategoryList)this.windowGroup.Controller.GetChildById("actioncategories");
		this.categoryList.CategoryChanged += this.HandleCategoryChanged;
		this.voteCategoryList = (XUiC_CategoryList)this.windowGroup.Controller.GetChildById("votecategories");
		this.voteCategoryList.CategoryChanged += this.HandleVoteCategoryChanged;
		this.actionHistoryCategoryList = (XUiC_CategoryList)this.windowGroup.Controller.GetChildById("actionHistoryCategories");
		this.actionHistoryCategoryList.CategoryChanged += this.HandleActionHistoryCategoryChanged;
		this.leaderboardCategoryList = (XUiC_CategoryList)this.windowGroup.Controller.GetChildById("leaderboardCategories");
		this.leaderboardCategoryList.CategoryChanged += this.HandleLeaderboardCategoryChanged;
		this.txtInput = (XUiC_TextInput)this.windowGroup.Controller.GetChildById("searchInput");
		XUiController childById = base.GetChildById("allactions");
		if (childById != null)
		{
			this.allActionsButton = childById.ViewComponent as XUiV_Button;
			if (this.allActionsButton != null)
			{
				childById.OnPress += this.AllActionsButtonCtrl_OnPress;
			}
		}
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
		this.ActionCategory = "";
		this.lblActions = Localization.Get("TwitchInfo_Actions", false, null);
		this.lblVotes = Localization.Get("TwitchInfo_Votes", false, null);
		this.lblActionHistory = Localization.Get("TwitchInfo_ActionHistory", false, null);
		this.lblLeaderboard = Localization.Get("TwitchInfo_Leaderboard", false, null);
	}

	// Token: 0x06008780 RID: 34688 RVA: 0x0033BF33 File Offset: 0x0033A133
	[PublicizedFrom(EAccessModifier.Private)]
	public void AllActionsButtonCtrl_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showAllActions = !this.showAllActions;
		this.allActionsButton.Selected = this.showAllActions;
		this.IsDirty = true;
	}

	// Token: 0x06008781 RID: 34689 RVA: 0x0033BF5C File Offset: 0x0033A15C
	public void SetOpenToActions(bool openExtras = false)
	{
		this.CurrentType = XUiC_TwitchEntryListWindow.ListTypes.Actions;
		this.IsDirty = true;
		this.forceExtras = openExtras;
	}

	// Token: 0x06008782 RID: 34690 RVA: 0x0033BF73 File Offset: 0x0033A173
	public void SetOpenToLeaderboard()
	{
		this.CurrentType = XUiC_TwitchEntryListWindow.ListTypes.Leaderboard;
		this.IsDirty = true;
	}

	// Token: 0x06008783 RID: 34691 RVA: 0x0033BF83 File Offset: 0x0033A183
	public void SetOpenToVotes()
	{
		this.CurrentType = XUiC_TwitchEntryListWindow.ListTypes.Votes;
		this.IsDirty = true;
	}

	// Token: 0x06008784 RID: 34692 RVA: 0x0033BF93 File Offset: 0x0033A193
	public void SetOpenToHistory()
	{
		this.CurrentType = XUiC_TwitchEntryListWindow.ListTypes.ActionHistory;
		this.IsDirty = true;
	}

	// Token: 0x06008785 RID: 34693 RVA: 0x0033BFA3 File Offset: 0x0033A1A3
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.ActionCategory = _categoryEntry.CategoryName;
		this.HandleFilterActions();
	}

	// Token: 0x06008786 RID: 34694 RVA: 0x0033BFD0 File Offset: 0x0033A1D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFilterActions()
	{
		if (this.currentActions != null && this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Actions)
		{
			TwitchActionPreset currentPreset = this.tm.CurrentActionPreset;
			List<TwitchAction> list = (from entry in this.currentActions
				where (this.filterText == "" || entry.Title.ContainsCaseInsensitive(this.filterText) || entry.Command.ContainsCaseInsensitive(this.filterText)) && ((this.ActionCategory == "" && entry.DisplayCategory == entry.MainCategory) || entry.DisplayCategory.Name == this.ActionCategory) && entry.ShowInActionList && (currentPreset.AllowPointGeneration || entry.PointType == TwitchAction.PointTypes.Bits)
				orderby entry.Title
				select entry).ToList<TwitchAction>();
			this.twitchActionEntryList.SetTwitchActionList(list, this.tm.CurrentActionPreset);
			base.RefreshBindings();
			return;
		}
		this.IsDirty = true;
	}

	// Token: 0x06008787 RID: 34695 RVA: 0x0033C071 File Offset: 0x0033A271
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleVoteCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.VoteCategory = _categoryEntry.CategoryName;
		this.IsDirty = true;
	}

	// Token: 0x06008788 RID: 34696 RVA: 0x0033C0A0 File Offset: 0x0033A2A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleActionHistoryCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		if (_categoryEntry.CategoryName != this.ActionHistoryCategory)
		{
			this.ActionHistoryCategory = _categoryEntry.CategoryName;
			this.twitchActionHistoryEntryList.SelectedEntry = null;
			this.twitchActionHistoryEntryList.setFirstEntry = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06008789 RID: 34697 RVA: 0x0033C103 File Offset: 0x0033A303
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLeaderboardCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.LeaderboardCategory = _categoryEntry.CategoryName;
		this.IsDirty = true;
		this.UpdateDelay = 0f;
	}

	// Token: 0x0600878A RID: 34698 RVA: 0x0033C13B File Offset: 0x0033A33B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		if (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Actions)
		{
			this.HandleFilterActions();
			return;
		}
		this.IsDirty = true;
	}

	// Token: 0x0600878B RID: 34699 RVA: 0x0033C15C File Offset: 0x0033A35C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.UpdateDelay > 0f)
		{
			this.UpdateDelay -= _dt;
			return;
		}
		if (this.IsDirty)
		{
			if (this.tm == null)
			{
				this.tm = TwitchManager.Current;
			}
			switch (this.CurrentType)
			{
			case XUiC_TwitchEntryListWindow.ListTypes.Actions:
			{
				this.currentActions = (from entry in TwitchActionManager.TwitchActions.Values.ToList<TwitchAction>()
					where entry != null && entry.ShowInActionList && (entry.IsInPresetForList(this.tm.CurrentActionPreset) || (this.showAllActions && !entry.OnlyShowInPreset) || (entry.DisplayCategory != null && entry.DisplayCategory.AlwaysShowInMenu))
					orderby entry.Title
					select entry).ToList<TwitchAction>();
				this.twitchActionEntryList.SetTwitchActionList(this.currentActions, this.tm.CurrentActionPreset);
				string text = ((this.categoryList.CurrentCategory != null) ? this.categoryList.CurrentCategory.CategoryName : "");
				this.categoryList.SetupCategoriesBasedOnTwitchActions(this.currentActions);
				if (this.forceExtras)
				{
					text = "Extras";
					this.forceExtras = false;
				}
				if (text == "")
				{
					this.categoryList.SetCategoryToFirst();
				}
				else
				{
					this.categoryList.SetCategory(text);
					if (this.categoryList.CurrentCategory == null || this.categoryList.CurrentCategory.CategoryName == "")
					{
						this.categoryList.SetCategoryToFirst();
					}
				}
				break;
			}
			case XUiC_TwitchEntryListWindow.ListTypes.Votes:
				this.currentVotes = (from entry in TwitchActionManager.TwitchVotes.Values.ToList<TwitchVote>()
					where (this.filterText == "" || entry.Display.ContainsCaseInsensitive(this.filterText)) && (this.VoteCategory == "" || (entry.MainVoteType != null && entry.MainVoteType.Name == this.VoteCategory && entry.MainVoteType.IsInPreset(this.tm.CurrentVotePreset.Name))) && entry.IsInPreset(this.tm.CurrentVotePreset)
					orderby entry.VoteDescription
					select entry).ToList<TwitchVote>();
				this.twitchVoteInfoEntryList.SetTwitchVoteList(this.currentVotes);
				break;
			case XUiC_TwitchEntryListWindow.ListTypes.ActionHistory:
			{
				List<TwitchActionHistoryEntry> list = null;
				string actionHistoryCategory = this.ActionHistoryCategory;
				if (!(actionHistoryCategory == "action"))
				{
					if (!(actionHistoryCategory == "vote"))
					{
						if (actionHistoryCategory == "event")
						{
							list = this.tm.EventHistory;
						}
					}
					else
					{
						list = this.tm.VoteHistory;
					}
				}
				else
				{
					list = this.tm.ActionHistory;
				}
				if (list != null)
				{
					this.currentRedemptions = list.Where<TwitchActionHistoryEntry>((TwitchActionHistoryEntry entry) => entry != null && entry.IsValid() && (this.filterText == "" || entry.UserName.ContainsCaseInsensitive(this.filterText) || entry.Action.Command.ContainsCaseInsensitive(this.filterText)) && (this.ActionHistoryCategory == "" || this.ActionHistoryCategory == entry.HistoryType)).ToList<TwitchActionHistoryEntry>();
					this.twitchActionHistoryEntryList.SetTwitchActionHistoryList(this.currentRedemptions);
				}
				break;
			}
			case XUiC_TwitchEntryListWindow.ListTypes.Leaderboard:
			{
				string text2 = ((this.leaderboardCategoryList == null) ? "" : this.leaderboardCategoryList.CurrentCategory.CategoryName);
				if (!(text2 == "global_kills"))
				{
					if (!(text2 == "session_kills"))
					{
						if (!(text2 == "session_good"))
						{
							if (!(text2 == "session_bad"))
							{
								if (!(text2 == "session_bits"))
								{
									if (text2 == "current_good")
									{
										Dictionary<string, TwitchLeaderboardStats.StatEntry> statEntries = TwitchManager.LeaderboardStats.StatEntries;
										this.currentLeaderboard = (from viewer in statEntries.Values
											where (this.filterText == "" || viewer.Name.ContainsCaseInsensitive(this.filterText)) && viewer.CurrentActions > 0
											orderby viewer.CurrentGoodActions descending
											select new TwitchLeaderboardEntry(viewer.Name, viewer.UserColor, viewer.CurrentGoodActions)).ToList<TwitchLeaderboardEntry>();
										this.twitchLeaderboardEntryList.SetTwitchLeaderboardList(this.currentLeaderboard);
									}
								}
								else
								{
									Dictionary<string, TwitchLeaderboardStats.StatEntry> statEntries2 = TwitchManager.LeaderboardStats.StatEntries;
									this.currentLeaderboard = (from viewer in statEntries2.Values
										where (this.filterText == "" || viewer.Name.ContainsCaseInsensitive(this.filterText)) && viewer.BitsUsed > 0
										orderby viewer.BitsUsed descending
										select new TwitchLeaderboardEntry(viewer.Name, viewer.UserColor, viewer.BitsUsed)).ToList<TwitchLeaderboardEntry>();
									this.twitchLeaderboardEntryList.SetTwitchLeaderboardList(this.currentLeaderboard);
								}
							}
							else
							{
								Dictionary<string, TwitchLeaderboardStats.StatEntry> statEntries3 = TwitchManager.LeaderboardStats.StatEntries;
								this.currentLeaderboard = (from viewer in statEntries3.Values
									where (this.filterText == "" || viewer.Name.ContainsCaseInsensitive(this.filterText)) && viewer.BadActions > 0
									orderby viewer.BadActions descending
									select new TwitchLeaderboardEntry(viewer.Name, viewer.UserColor, viewer.BadActions)).ToList<TwitchLeaderboardEntry>();
								this.twitchLeaderboardEntryList.SetTwitchLeaderboardList(this.currentLeaderboard);
							}
						}
						else
						{
							Dictionary<string, TwitchLeaderboardStats.StatEntry> statEntries4 = TwitchManager.LeaderboardStats.StatEntries;
							this.currentLeaderboard = (from viewer in statEntries4.Values
								where (this.filterText == "" || viewer.Name.ContainsCaseInsensitive(this.filterText)) && viewer.GoodActions > 0
								orderby viewer.GoodActions descending
								select new TwitchLeaderboardEntry(viewer.Name, viewer.UserColor, viewer.GoodActions)).ToList<TwitchLeaderboardEntry>();
							this.twitchLeaderboardEntryList.SetTwitchLeaderboardList(this.currentLeaderboard);
						}
					}
					else
					{
						Dictionary<string, TwitchLeaderboardStats.StatEntry> statEntries5 = TwitchManager.LeaderboardStats.StatEntries;
						this.currentLeaderboard = (from viewer in statEntries5.Values
							where (this.filterText == "" || viewer.Name.ContainsCaseInsensitive(this.filterText)) && viewer.Kills > 0
							orderby viewer.Kills descending
							select new TwitchLeaderboardEntry(viewer.Name, viewer.UserColor, viewer.Kills)).ToList<TwitchLeaderboardEntry>();
						this.twitchLeaderboardEntryList.SetTwitchLeaderboardList(this.currentLeaderboard);
					}
				}
				else
				{
					this.currentLeaderboard = (from entry in this.tm.Leaderboard
						where this.filterText == "" || entry.UserName.ContainsCaseInsensitive(this.filterText)
						orderby entry.Kills descending
						select entry).ToList<TwitchLeaderboardEntry>();
					this.twitchLeaderboardEntryList.SetTwitchLeaderboardList(this.currentLeaderboard);
				}
				this.UpdateDelay = 1f;
				break;
			}
			}
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600878C RID: 34700 RVA: 0x0033C7A0 File Offset: 0x0033A9A0
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.isInitialized)
		{
			this.player = this.xui.playerUI.entityPlayer;
			this.categoryList.SetupCategoriesBasedOnTwitchCategories(TwitchActionManager.Current.CategoryList);
			this.categoryList.SetCategoryToFirst();
			this.voteCategoryList.SetupCategoriesBasedOnTwitchVoteCategories(TwitchManager.Current.VotingManager.VoteTypes.Values.Where<TwitchVoteType>((TwitchVoteType v) => v.IsInPreset(TwitchManager.Current.CurrentVotePreset.Name)).ToList<TwitchVoteType>());
			this.voteCategoryList.SetCategoryToFirst();
			this.actionHistoryCategoryList.SetCategoryEntry(0, "action", "ui_game_symbol_twitch_actions", Localization.Get("TwitchInfo_Actions", false, null));
			this.actionHistoryCategoryList.SetCategoryEntry(1, "vote", "ui_game_symbol_twitch_vote", Localization.Get("TwitchInfo_Votes", false, null));
			this.actionHistoryCategoryList.SetCategoryEntry(2, "event", "ui_game_symbol_twitch_custom_actions", Localization.Get("xuiOptionsTwitchEvents", false, null));
			this.actionHistoryCategoryList.SetCategoryToFirst();
			this.leaderboardCategoryList.SetCategoryEntry(0, "global_kills", "ui_game_symbol_twitch_top_killer", Localization.Get("TwitchInfo_LeaderboardGlobalKills", false, null));
			this.leaderboardCategoryList.SetCategoryEntry(1, "session_kills", "ui_game_symbol_skull", Localization.Get("TwitchInfo_LeaderboardSessionKills", false, null));
			this.leaderboardCategoryList.SetCategoryEntry(2, "session_good", "ui_game_symbol_twitch_top_good", Localization.Get("TwitchInfo_LeaderboardSessionGood", false, null));
			this.leaderboardCategoryList.SetCategoryEntry(3, "session_bad", "ui_game_symbol_twitch_top_bad", Localization.Get("TwitchInfo_LeaderboardSessionBad", false, null));
			this.leaderboardCategoryList.SetCategoryEntry(4, "session_bits", "ui_game_symbol_twitch_bits", Localization.Get("TwitchInfo_LeaderboardSessionBits", false, null));
			this.leaderboardCategoryList.SetCategoryEntry(5, "current_good", "ui_game_symbol_twitch_best_helper", Localization.Get("TwitchInfo_LeaderboardCurrentGood", false, null));
			this.leaderboardCategoryList.SetCategoryToFirst();
			this.IsDirty = true;
			this.isInitialized = true;
		}
		this.tm = TwitchManager.Current;
		this.tm.ActionHistoryAdded += this.Tm_ActionHistoryAdded;
		this.tm.VoteHistoryAdded += this.Tm_VoteHistoryAdded;
		this.tm.EventHistoryAdded += this.Tm_EventHistoryAdded;
		TwitchManager.LeaderboardStats.LeaderboardChanged += this.LeaderboardStats_LeaderboardChanged;
	}

	// Token: 0x0600878D RID: 34701 RVA: 0x0033CA05 File Offset: 0x0033AC05
	[PublicizedFrom(EAccessModifier.Private)]
	public void LeaderboardStats_LeaderboardChanged()
	{
		if (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Leaderboard)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x0600878E RID: 34702 RVA: 0x0033CA17 File Offset: 0x0033AC17
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tm_ActionHistoryAdded()
	{
		if (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.ActionHistory && this.ActionHistoryCategory == "action")
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x0600878F RID: 34703 RVA: 0x0033CA3B File Offset: 0x0033AC3B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tm_VoteHistoryAdded()
	{
		if (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.ActionHistory && this.ActionHistoryCategory == "vote")
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06008790 RID: 34704 RVA: 0x0033CA5F File Offset: 0x0033AC5F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tm_EventHistoryAdded()
	{
		if (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.ActionHistory && this.ActionHistoryCategory == "event")
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06008791 RID: 34705 RVA: 0x0033CA84 File Offset: 0x0033AC84
	public override void OnClose()
	{
		base.OnClose();
		TwitchManager twitchManager = TwitchManager.Current;
		twitchManager.ActionHistoryAdded -= this.Tm_ActionHistoryAdded;
		twitchManager.VoteHistoryAdded -= this.Tm_VoteHistoryAdded;
		twitchManager.EventHistoryAdded -= this.Tm_EventHistoryAdded;
		TwitchManager.LeaderboardStats.LeaderboardChanged -= this.LeaderboardStats_LeaderboardChanged;
	}

	// Token: 0x06008792 RID: 34706 RVA: 0x0033CAE8 File Offset: 0x0033ACE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2855073766U)
		{
			if (num <= 956754208U)
			{
				if (num != 884373603U)
				{
					if (num == 956754208U)
					{
						if (bindingName == "showcategories")
						{
							value = (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Actions).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "showleaderboard")
				{
					value = (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Leaderboard).ToString();
					return true;
				}
			}
			else if (num != 1092444455U)
			{
				if (num == 2855073766U)
				{
					if (bindingName == "leaderboard_header_value")
					{
						if (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Leaderboard)
						{
							string text;
							if (this.leaderboardCategoryList != null)
							{
								XUiC_CategoryEntry currentCategory = this.leaderboardCategoryList.CurrentCategory;
								text = ((currentCategory != null) ? currentCategory.CategoryName : null) ?? "";
							}
							else
							{
								text = "";
							}
							string text2 = text;
							if (!(text2 == "global_kills") && !(text2 == "session_kills"))
							{
								if (!(text2 == "session_good") && !(text2 == "session_bad") && !(text2 == "current_good"))
								{
									if (text2 == "session_bits")
									{
										value = Localization.Get("TwitchPoints_Bits", false, null);
									}
								}
								else
								{
									value = Localization.Get("TwitchInfo_Actions", false, null);
								}
							}
							else
							{
								value = Localization.Get("TwitchInfo_KillsHeader", false, null);
							}
						}
						return true;
					}
				}
			}
			else if (bindingName == "showactions")
			{
				value = (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Actions).ToString();
				return true;
			}
		}
		else if (num <= 3272249238U)
		{
			if (num != 3131143201U)
			{
				if (num == 3272249238U)
				{
					if (bindingName == "headertitle")
					{
						switch (this.CurrentType)
						{
						case XUiC_TwitchEntryListWindow.ListTypes.Actions:
						{
							string text3;
							if (this.categoryList != null)
							{
								XUiC_CategoryEntry currentCategory2 = this.categoryList.CurrentCategory;
								text3 = ((currentCategory2 != null) ? currentCategory2.CategoryDisplayName : null) ?? "";
							}
							else
							{
								text3 = "";
							}
							string text4 = text3;
							value = ((text4 != "") ? text4 : Localization.Get("lblAll", false, null));
							break;
						}
						case XUiC_TwitchEntryListWindow.ListTypes.Votes:
						{
							string text5;
							if (this.voteCategoryList != null)
							{
								XUiC_CategoryEntry currentCategory3 = this.voteCategoryList.CurrentCategory;
								text5 = ((currentCategory3 != null) ? currentCategory3.CategoryDisplayName : null) ?? "";
							}
							else
							{
								text5 = "";
							}
							string text6 = text5;
							value = ((text6 != "") ? text6 : Localization.Get("lblAll", false, null));
							break;
						}
						case XUiC_TwitchEntryListWindow.ListTypes.ActionHistory:
						{
							string text7;
							if (this.actionHistoryCategoryList != null)
							{
								XUiC_CategoryEntry currentCategory4 = this.actionHistoryCategoryList.CurrentCategory;
								text7 = ((currentCategory4 != null) ? currentCategory4.CategoryDisplayName : null) ?? "";
							}
							else
							{
								text7 = "";
							}
							string text8 = text7;
							value = ((text8 != "") ? text8 : Localization.Get("lblAll", false, null));
							break;
						}
						case XUiC_TwitchEntryListWindow.ListTypes.Leaderboard:
						{
							string text9;
							if (this.leaderboardCategoryList != null)
							{
								XUiC_CategoryEntry currentCategory5 = this.leaderboardCategoryList.CurrentCategory;
								text9 = ((currentCategory5 != null) ? currentCategory5.CategoryDisplayName : null) ?? "";
							}
							else
							{
								text9 = "";
							}
							string text10 = text9;
							value = ((text10 != "") ? text10 : Localization.Get("lblAll", false, null));
							break;
						}
						}
						return true;
					}
				}
			}
			else if (bindingName == "headericon")
			{
				switch (this.CurrentType)
				{
				case XUiC_TwitchEntryListWindow.ListTypes.Actions:
					value = "ui_game_symbol_twitch_actions";
					break;
				case XUiC_TwitchEntryListWindow.ListTypes.Votes:
					value = "ui_game_symbol_twitch_vote";
					break;
				case XUiC_TwitchEntryListWindow.ListTypes.ActionHistory:
					value = "ui_game_symbol_twitch_history";
					break;
				case XUiC_TwitchEntryListWindow.ListTypes.Leaderboard:
					value = "ui_game_symbol_twitch_leaderboard_1";
					break;
				}
				return true;
			}
		}
		else if (num != 4008283398U)
		{
			if (num == 4228112153U)
			{
				if (bindingName == "showvotes")
				{
					value = (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.Votes).ToString();
					return true;
				}
			}
		}
		else if (bindingName == "showactionhistory")
		{
			value = (this.CurrentType == XUiC_TwitchEntryListWindow.ListTypes.ActionHistory).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x040064E2 RID: 25826
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040064E3 RID: 25827
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionEntryList twitchActionEntryList;

	// Token: 0x040064E4 RID: 25828
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchVoteInfoEntryList twitchVoteInfoEntryList;

	// Token: 0x040064E5 RID: 25829
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionHistoryEntryList twitchActionHistoryEntryList;

	// Token: 0x040064E6 RID: 25830
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchLeaderboardEntryList twitchLeaderboardEntryList;

	// Token: 0x040064E7 RID: 25831
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040064E8 RID: 25832
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchAction> currentActions;

	// Token: 0x040064E9 RID: 25833
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchVote> currentVotes;

	// Token: 0x040064EA RID: 25834
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchActionHistoryEntry> currentRedemptions;

	// Token: 0x040064EB RID: 25835
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchLeaderboardEntry> currentLeaderboard;

	// Token: 0x040064EC RID: 25836
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x040064ED RID: 25837
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040064EE RID: 25838
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList voteCategoryList;

	// Token: 0x040064EF RID: 25839
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList actionHistoryCategoryList;

	// Token: 0x040064F0 RID: 25840
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList leaderboardCategoryList;

	// Token: 0x040064F5 RID: 25845
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInitialized;

	// Token: 0x040064F6 RID: 25846
	public XUiC_TwitchEntryListWindow.ListTypes CurrentType;

	// Token: 0x040064F7 RID: 25847
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblActions;

	// Token: 0x040064F8 RID: 25848
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblVotes;

	// Token: 0x040064F9 RID: 25849
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblActionHistory;

	// Token: 0x040064FA RID: 25850
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblLeaderboard;

	// Token: 0x040064FB RID: 25851
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAllActions;

	// Token: 0x040064FC RID: 25852
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button allActionsButton;

	// Token: 0x040064FD RID: 25853
	[PublicizedFrom(EAccessModifier.Private)]
	public float UpdateDelay;

	// Token: 0x040064FE RID: 25854
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager tm;

	// Token: 0x040064FF RID: 25855
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceExtras;

	// Token: 0x020010A0 RID: 4256
	public enum ListTypes
	{
		// Token: 0x04006501 RID: 25857
		Actions,
		// Token: 0x04006502 RID: 25858
		Votes,
		// Token: 0x04006503 RID: 25859
		ActionHistory,
		// Token: 0x04006504 RID: 25860
		Leaderboard
	}
}

using System;
using Challenges;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000FF6 RID: 4086
[Preserve]
public class XUiC_QuestTrackerWindow : XUiController
{
	// Token: 0x17000F0C RID: 3852
	// (get) Token: 0x060080F5 RID: 33013 RVA: 0x0030D898 File Offset: 0x0030BA98
	// (set) Token: 0x060080F6 RID: 33014 RVA: 0x0030D8A0 File Offset: 0x0030BAA0
	public Quest CurrentQuest
	{
		get
		{
			return this.currentQuest;
		}
		set
		{
			this.currentQuest = value;
			this.questClass = ((value != null) ? QuestClass.GetQuest(this.currentQuest.ID) : null);
			if (value != null)
			{
				if (this.currentChallenge != null)
				{
					this.currentChallenge.OnChallengeStateChanged -= this.CurrentChallenge_OnChallengeStateChanged;
				}
				this.currentChallenge = null;
				this.challengeClass = null;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x060080F7 RID: 33015 RVA: 0x0030D906 File Offset: 0x0030BB06
	// (set) Token: 0x060080F8 RID: 33016 RVA: 0x0030D910 File Offset: 0x0030BB10
	public Challenge CurrentChallenge
	{
		get
		{
			return this.currentChallenge;
		}
		set
		{
			if (this.currentChallenge != null)
			{
				this.currentChallenge.OnChallengeStateChanged -= this.CurrentChallenge_OnChallengeStateChanged;
			}
			this.currentChallenge = value;
			this.challengeClass = ((value != null) ? this.currentChallenge.ChallengeClass : null);
			if (value != null)
			{
				this.currentQuest = null;
				this.questClass = null;
				this.currentChallenge.OnChallengeStateChanged += this.CurrentChallenge_OnChallengeStateChanged;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x060080F9 RID: 33017 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void CurrentChallenge_OnChallengeStateChanged(Challenge challenge)
	{
		base.RefreshBindings();
	}

	// Token: 0x060080FA RID: 33018 RVA: 0x0030D988 File Offset: 0x0030BB88
	public override void Init()
	{
		base.Init();
		XUiC_QuestTrackerWindow.ID = base.WindowGroup.Id;
		this.objectiveList = base.GetChildByType<XUiC_QuestTrackerObjectiveList>();
		base.registerForInputStyleChanges();
	}

	// Token: 0x060080FB RID: 33019 RVA: 0x0029BDE6 File Offset: 0x00299FE6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.IsDirty = true;
	}

	// Token: 0x060080FC RID: 33020 RVA: 0x0030D9B4 File Offset: 0x0030BBB4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (windowManager.IsHUDEnabled() || (this.xui.DragAndDropWindow.InMenu && windowManager.IsHUDPartialHidden()))
		{
			if (base.ViewComponent.IsVisible && this.localPlayer.IsDead())
			{
				this.IsDirty = true;
			}
			else if (!base.ViewComponent.IsVisible && !this.localPlayer.IsDead())
			{
				this.IsDirty = true;
			}
			if (this.currentChallenge != null && this.currentChallenge.UIDirty)
			{
				this.IsDirty = true;
				this.currentChallenge.UIDirty = false;
			}
			if (this.IsDirty)
			{
				this.objectiveList.Quest = this.currentQuest;
				this.objectiveList.Challenge = this.currentChallenge;
				base.RefreshBindings();
				this.IsDirty = false;
			}
			return;
		}
		base.ViewComponent.IsVisible = false;
	}

	// Token: 0x060080FD RID: 33021 RVA: 0x0030DAD4 File Offset: 0x0030BCD4
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
		this.xui.playerUI.entityPlayer.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedChallengeChanged += this.QuestTracker_OnTrackedChallengeChanged;
		this.xui.playerUI.entityPlayer.QuestJournal.RefreshTracked();
		if (this.xui.QuestTracker.TrackedQuest != null)
		{
			this.CurrentQuest = this.xui.QuestTracker.TrackedQuest;
			return;
		}
		if (this.xui.QuestTracker.TrackedChallenge != null)
		{
			this.CurrentChallenge = this.xui.QuestTracker.TrackedChallenge;
		}
	}

	// Token: 0x060080FE RID: 33022 RVA: 0x0030DBAC File Offset: 0x0030BDAC
	public override void OnClose()
	{
		base.OnClose();
		if (XUi.IsGameRunning())
		{
			this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
			this.xui.playerUI.entityPlayer.QuestChanged -= this.QuestJournal_QuestChanged;
			this.xui.QuestTracker.OnTrackedChallengeChanged -= this.QuestTracker_OnTrackedChallengeChanged;
		}
	}

	// Token: 0x060080FF RID: 33023 RVA: 0x0030DC1F File Offset: 0x0030BE1F
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		this.CurrentQuest = this.xui.QuestTracker.TrackedQuest;
		this.IsDirty = true;
	}

	// Token: 0x06008100 RID: 33024 RVA: 0x0030DC3E File Offset: 0x0030BE3E
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedChallengeChanged()
	{
		this.CurrentChallenge = this.xui.QuestTracker.TrackedChallenge;
		this.IsDirty = true;
	}

	// Token: 0x06008101 RID: 33025 RVA: 0x0030DC5D File Offset: 0x0030BE5D
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.CurrentQuest == q)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06008102 RID: 33026 RVA: 0x0030DC70 File Offset: 0x0030BE70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2783733891U)
		{
			if (num <= 1901444352U)
			{
				if (num != 112224632U)
				{
					if (num == 1901444352U)
					{
						if (bindingName == "questhint")
						{
							if (this.currentQuest != null)
							{
								value = this.questClass.GetCurrentHint((int)this.currentQuest.CurrentPhase);
							}
							else if (this.currentChallenge != null)
							{
								value = this.challengeClass.GetHint(this.currentChallenge.NeedsPreRequisites);
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "questicon")
				{
					if (this.currentQuest != null)
					{
						value = this.questClass.Icon;
					}
					else if (this.currentChallenge != null)
					{
						value = this.challengeClass.Icon;
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num != 2730462270U)
			{
				if (num == 2783733891U)
				{
					if (bindingName == "questhintposition")
					{
						if (this.currentQuest != null)
						{
							value = string.Format("0,{0}", -50 + this.currentQuest.ActiveObjectives * -27);
						}
						else if (this.currentChallenge != null)
						{
							value = string.Format("0,{0}", -50 + this.currentChallenge.ActiveObjectives * -27);
						}
						else
						{
							value = "0,0";
						}
						return true;
					}
				}
			}
			else if (bindingName == "questname")
			{
				if (this.currentQuest != null)
				{
					value = this.questClass.Name;
				}
				else if (this.currentChallenge != null)
				{
					value = this.challengeClass.Title;
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (num <= 3047389681U)
		{
			if (num != 2823605611U)
			{
				if (num == 3047389681U)
				{
					if (bindingName == "questtitle")
					{
						if (this.currentQuest != null)
						{
							value = this.questClass.SubTitle;
						}
						else if (this.currentChallenge != null)
						{
							value = this.challengeClass.Title;
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "questhintavailable")
			{
				if (this.currentQuest != null)
				{
					value = (this.questClass.GetCurrentHint((int)this.currentQuest.CurrentPhase) != "").ToString();
				}
				else if (this.currentChallenge != null)
				{
					value = (this.challengeClass.GetHint(this.currentChallenge.NeedsPreRequisites) != "").ToString();
				}
				else
				{
					value = "false";
				}
				return true;
			}
		}
		else if (num != 3231221182U)
		{
			if (num != 4060322893U)
			{
				if (num == 4116915492U)
				{
					if (bindingName == "trackerheight")
					{
						if (this.currentQuest != null)
						{
							value = this.trackerheightFormatter.Format(this.currentQuest.ActiveObjectives * 27);
						}
						else if (this.currentChallenge != null)
						{
							value = this.trackerheightFormatter.Format(this.currentChallenge.ActiveObjectives * 27);
						}
						else
						{
							value = "0";
						}
						return true;
					}
				}
			}
			else if (bindingName == "showempty")
			{
				value = (this.currentQuest == null && this.currentChallenge == null).ToString();
				return true;
			}
		}
		else if (bindingName == "showquest")
		{
			value = ((this.currentQuest != null || this.currentChallenge != null) && XUi.IsGameRunning() && this.localPlayer != null && !this.localPlayer.IsDead()).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x04005F95 RID: 24469
	public static string ID = "";

	// Token: 0x04005F96 RID: 24470
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTrackerObjectiveList objectiveList;

	// Token: 0x04005F97 RID: 24471
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04005F98 RID: 24472
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F99 RID: 24473
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x04005F9A RID: 24474
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005F9B RID: 24475
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge currentChallenge;

	// Token: 0x04005F9C RID: 24476
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}

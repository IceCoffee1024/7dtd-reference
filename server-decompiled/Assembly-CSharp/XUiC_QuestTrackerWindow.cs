using System;
using Challenges;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001005 RID: 4101
[Preserve]
public class XUiC_QuestTrackerWindow : XUiController
{
	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x06008104 RID: 33028 RVA: 0x00310880 File Offset: 0x0030EA80
	// (set) Token: 0x06008105 RID: 33029 RVA: 0x00310888 File Offset: 0x0030EA88
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

	// Token: 0x17000F34 RID: 3892
	// (get) Token: 0x06008106 RID: 33030 RVA: 0x003108EE File Offset: 0x0030EAEE
	// (set) Token: 0x06008107 RID: 33031 RVA: 0x003108F8 File Offset: 0x0030EAF8
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

	// Token: 0x06008108 RID: 33032 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void CurrentChallenge_OnChallengeStateChanged(Challenge challenge)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008109 RID: 33033 RVA: 0x00310970 File Offset: 0x0030EB70
	public override void Init()
	{
		base.Init();
		XUiC_QuestTrackerWindow.ID = base.WindowGroup.Id;
		this.objectiveList = base.GetChildByType<XUiC_QuestTrackerObjectiveList>();
		base.registerForInputStyleChanges();
	}

	// Token: 0x0600810A RID: 33034 RVA: 0x0029F176 File Offset: 0x0029D376
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.IsDirty = true;
	}

	// Token: 0x0600810B RID: 33035 RVA: 0x0031099C File Offset: 0x0030EB9C
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

	// Token: 0x0600810C RID: 33036 RVA: 0x00310ABC File Offset: 0x0030ECBC
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

	// Token: 0x0600810D RID: 33037 RVA: 0x00310B94 File Offset: 0x0030ED94
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

	// Token: 0x0600810E RID: 33038 RVA: 0x00310C07 File Offset: 0x0030EE07
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		this.CurrentQuest = this.xui.QuestTracker.TrackedQuest;
		this.IsDirty = true;
	}

	// Token: 0x0600810F RID: 33039 RVA: 0x00310C26 File Offset: 0x0030EE26
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedChallengeChanged()
	{
		this.CurrentChallenge = this.xui.QuestTracker.TrackedChallenge;
		this.IsDirty = true;
	}

	// Token: 0x06008110 RID: 33040 RVA: 0x00310C45 File Offset: 0x0030EE45
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.CurrentQuest == q)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06008111 RID: 33041 RVA: 0x00310C58 File Offset: 0x0030EE58
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

	// Token: 0x04005FBE RID: 24510
	public static string ID = "";

	// Token: 0x04005FBF RID: 24511
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTrackerObjectiveList objectiveList;

	// Token: 0x04005FC0 RID: 24512
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04005FC1 RID: 24513
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005FC2 RID: 24514
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x04005FC3 RID: 24515
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005FC4 RID: 24516
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge currentChallenge;

	// Token: 0x04005FC5 RID: 24517
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}

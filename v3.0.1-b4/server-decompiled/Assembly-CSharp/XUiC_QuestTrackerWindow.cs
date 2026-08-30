using System;
using Challenges;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000FDD RID: 4061
[Preserve]
public class XUiC_QuestTrackerWindow : XUiController
{
	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x06008003 RID: 32771 RVA: 0x00309064 File Offset: 0x00307264
	// (set) Token: 0x06008004 RID: 32772 RVA: 0x0030906C File Offset: 0x0030726C
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

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x06008005 RID: 32773 RVA: 0x003090D2 File Offset: 0x003072D2
	// (set) Token: 0x06008006 RID: 32774 RVA: 0x003090DC File Offset: 0x003072DC
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

	// Token: 0x06008007 RID: 32775 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void CurrentChallenge_OnChallengeStateChanged(Challenge challenge)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008008 RID: 32776 RVA: 0x00309154 File Offset: 0x00307354
	public override void Init()
	{
		base.Init();
		XUiC_QuestTrackerWindow.ID = base.WindowGroup.Id;
		this.objectiveList = base.GetChildByType<XUiC_QuestTrackerObjectiveList>();
		base.registerForInputStyleChanges();
	}

	// Token: 0x06008009 RID: 32777 RVA: 0x00298D32 File Offset: 0x00296F32
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.IsDirty = true;
	}

	// Token: 0x0600800A RID: 32778 RVA: 0x00309180 File Offset: 0x00307380
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

	// Token: 0x0600800B RID: 32779 RVA: 0x003092A0 File Offset: 0x003074A0
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

	// Token: 0x0600800C RID: 32780 RVA: 0x00309378 File Offset: 0x00307578
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

	// Token: 0x0600800D RID: 32781 RVA: 0x003093EB File Offset: 0x003075EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		this.CurrentQuest = this.xui.QuestTracker.TrackedQuest;
		this.IsDirty = true;
	}

	// Token: 0x0600800E RID: 32782 RVA: 0x0030940A File Offset: 0x0030760A
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedChallengeChanged()
	{
		this.CurrentChallenge = this.xui.QuestTracker.TrackedChallenge;
		this.IsDirty = true;
	}

	// Token: 0x0600800F RID: 32783 RVA: 0x00309429 File Offset: 0x00307629
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.CurrentQuest == q)
		{
			this.IsDirty = true;
		}
	}

	// Token: 0x06008010 RID: 32784 RVA: 0x0030943C File Offset: 0x0030763C
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

	// Token: 0x04005ECE RID: 24270
	public static string ID = "";

	// Token: 0x04005ECF RID: 24271
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTrackerObjectiveList objectiveList;

	// Token: 0x04005ED0 RID: 24272
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04005ED1 RID: 24273
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005ED2 RID: 24274
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x04005ED3 RID: 24275
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005ED4 RID: 24276
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge currentChallenge;

	// Token: 0x04005ED5 RID: 24277
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt trackerheightFormatter = new CachedStringFormatterInt();
}

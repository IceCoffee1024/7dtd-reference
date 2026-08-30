using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ED9 RID: 3801
[Preserve]
public class XUiC_ChallengeEntryList : XUiController
{
	// Token: 0x17000E07 RID: 3591
	// (get) Token: 0x0600773B RID: 30523 RVA: 0x002CC919 File Offset: 0x002CAB19
	// (set) Token: 0x0600773C RID: 30524 RVA: 0x002CC920 File Offset: 0x002CAB20
	public static XUiC_ChallengeEntry SelectedEntry
	{
		get
		{
			return XUiC_ChallengeEntryList.selectedEntry;
		}
		set
		{
			if (value != null && !value.IsChallengeVisible)
			{
				Debug.LogWarning("Here it is!");
				return;
			}
			if (XUiC_ChallengeEntryList.selectedEntry != null)
			{
				XUiC_ChallengeEntryList.selectedEntry.Selected = false;
			}
			XUiC_ChallengeEntryList.selectedEntry = value;
			if (XUiC_ChallengeEntryList.selectedEntry != null)
			{
				XUiC_ChallengeEntryList.selectedEntry.Selected = true;
			}
			XUiC_ChallengeEntryList.journalWindowGroup.SetEntry(XUiC_ChallengeEntryList.selectedEntry);
		}
	}

	// Token: 0x0600773D RID: 30525 RVA: 0x002CC97C File Offset: 0x002CAB7C
	public override void Init()
	{
		base.Init();
		XUiC_ChallengeEntryList.journalWindowGroup = (XUiC_ChallengeWindowGroup)base.WindowGroup.Controller;
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_ChallengeEntry)
			{
				XUiC_ChallengeEntry xuiC_ChallengeEntry = (XUiC_ChallengeEntry)this.children[i];
				xuiC_ChallengeEntry.Owner = this;
				xuiC_ChallengeEntry.JournalUIHandler = XUiC_ChallengeEntryList.journalWindowGroup;
				this.entryList.Add(xuiC_ChallengeEntry);
			}
		}
	}

	// Token: 0x0600773E RID: 30526 RVA: 0x002CCA00 File Offset: 0x002CAC00
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			ChallengeObjectiveChallengeComplete challengeObjectiveChallengeComplete = null;
			string text = "";
			string text2 = "";
			bool flag = this.Owner != null && this.Owner.Entry != null && this.Owner.Entry.ChallengeGroup.IsVisible(this.xui.playerUI.entityPlayer);
			if (this.xui.QuestTracker.TrackedChallenge != null)
			{
				challengeObjectiveChallengeComplete = this.xui.QuestTracker.TrackedChallenge.GetChallengeCompleteObjective();
				if (challengeObjectiveChallengeComplete != null && challengeObjectiveChallengeComplete.IsRedeemed)
				{
					if (challengeObjectiveChallengeComplete.IsGroup)
					{
						text2 = challengeObjectiveChallengeComplete.ChallengeName;
					}
					else
					{
						text = challengeObjectiveChallengeComplete.ChallengeName;
					}
				}
				else
				{
					challengeObjectiveChallengeComplete = null;
				}
			}
			for (int i = 0; i < this.entryList.Count; i++)
			{
				XUiC_ChallengeEntry xuiC_ChallengeEntry = this.entryList[i];
				if (xuiC_ChallengeEntry != null)
				{
					xuiC_ChallengeEntry.OnPress -= this.OnPressEntry;
					xuiC_ChallengeEntry.Selected = XUiC_ChallengeEntryList.selectedEntry == xuiC_ChallengeEntry || (XUiC_ChallengeEntryList.selectedEntry == null && xuiC_ChallengeEntry.Tracked);
					if (i < this.challengeList.Count)
					{
						xuiC_ChallengeEntry.Entry = this.challengeList[i];
						if (xuiC_ChallengeEntry.IsChallengeVisible)
						{
							xuiC_ChallengeEntry.OnPress += this.OnPressEntry;
							xuiC_ChallengeEntry.ViewComponent.SoundPlayOnClick = true;
							xuiC_ChallengeEntry.ViewComponent.SoundPlayOnHover = true;
						}
						else
						{
							xuiC_ChallengeEntry.ViewComponent.SoundPlayOnClick = false;
							xuiC_ChallengeEntry.ViewComponent.SoundPlayOnHover = false;
						}
						xuiC_ChallengeEntry.IsRedeemBlinking = false;
						if (xuiC_ChallengeEntry.Entry.ChallengeState == Challenge.ChallengeStates.Completed && challengeObjectiveChallengeComplete != null && (xuiC_ChallengeEntry.Entry.ChallengeClass.Name.EqualsCaseInsensitive(text) || xuiC_ChallengeEntry.Entry.ChallengeGroup.Name.EqualsCaseInsensitive(text2)))
						{
							xuiC_ChallengeEntry.IsRedeemBlinking = true;
						}
						if (flag && XUiC_ChallengeEntryList.selectedEntry == null)
						{
							if (xuiC_ChallengeEntry.Entry.IsTracked)
							{
								if (XUiC_ChallengeEntryList.selectedEntry == null)
								{
									this.Owner.Select();
									XUiC_ChallengeEntryList.SelectedEntry = xuiC_ChallengeEntry;
									if (XUiC_ChallengeEntryList.selectedEntry != null)
									{
										XUiC_ChallengeEntryList.journalWindowGroup.SetEntry(XUiC_ChallengeEntryList.selectedEntry);
										xuiC_ChallengeEntry.SelectCursorElement(true, false);
									}
									else
									{
										xuiC_ChallengeEntry.Entry.IsTracked = false;
									}
								}
								else if (!xuiC_ChallengeEntry.IsChallengeVisible)
								{
									xuiC_ChallengeEntry.Tracked = false;
									XUiC_ChallengeEntryList.SelectedEntry = null;
								}
							}
							else
							{
								this.Owner.Select();
								XUiC_ChallengeEntryList.SelectedEntry = xuiC_ChallengeEntry;
								if (XUiC_ChallengeEntryList.selectedEntry != null)
								{
									XUiC_ChallengeEntryList.journalWindowGroup.SetEntry(XUiC_ChallengeEntryList.selectedEntry);
									xuiC_ChallengeEntry.SelectCursorElement(true, false);
								}
							}
						}
					}
					else
					{
						xuiC_ChallengeEntry.Entry = null;
						xuiC_ChallengeEntry.ViewComponent.SoundPlayOnClick = false;
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0600773F RID: 30527 RVA: 0x0008A715 File Offset: 0x00088915
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007740 RID: 30528 RVA: 0x002CCCC4 File Offset: 0x002CAEC4
	public void UnSelect()
	{
		XUiC_ChallengeEntryList.SelectedEntry = null;
	}

	// Token: 0x06007741 RID: 30529 RVA: 0x002CCCCC File Offset: 0x002CAECC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_ChallengeEntry xuiC_ChallengeEntry = _sender as XUiC_ChallengeEntry;
		if (xuiC_ChallengeEntry != null)
		{
			this.Owner.Select();
			XUiC_ChallengeEntryList.SelectedEntry = xuiC_ChallengeEntry;
			XUiC_ChallengeEntryList.SelectedEntry.JournalUIHandler.SetEntry(XUiC_ChallengeEntryList.SelectedEntry);
			if (InputUtils.ShiftKeyPressed)
			{
				Challenge entry = xuiC_ChallengeEntry.Entry;
				if (entry.IsActive && !entry.IsTracked)
				{
					entry.IsTracked = true;
					this.xui.QuestTracker.TrackedChallenge = entry;
				}
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x06007742 RID: 30530 RVA: 0x002CCD45 File Offset: 0x002CAF45
	public void SetChallengeEntryList(List<Challenge> newChallengeList)
	{
		this.challengeList = newChallengeList;
		this.IsDirty = true;
	}

	// Token: 0x06007743 RID: 30531 RVA: 0x002CCD58 File Offset: 0x002CAF58
	public void SetEntryByChallenge(Challenge newChallenge)
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			XUiC_ChallengeEntry xuiC_ChallengeEntry = this.entryList[i];
			if (xuiC_ChallengeEntry != null && xuiC_ChallengeEntry.Entry == newChallenge)
			{
				XUiC_ChallengeEntryList.SelectedEntry = xuiC_ChallengeEntry;
				return;
			}
		}
	}

	// Token: 0x06007744 RID: 30532 RVA: 0x002CCD9B File Offset: 0x002CAF9B
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06007745 RID: 30533 RVA: 0x002CCDB9 File Offset: 0x002CAFB9
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x04005802 RID: 22530
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005803 RID: 22531
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeEntry> entryList = new List<XUiC_ChallengeEntry>();

	// Token: 0x04005804 RID: 22532
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_ChallengeEntry selectedEntry;

	// Token: 0x04005805 RID: 22533
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> challengeList;

	// Token: 0x04005806 RID: 22534
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;

	// Token: 0x04005807 RID: 22535
	public XUiC_ChallengeGroupEntry Owner;

	// Token: 0x04005808 RID: 22536
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_ChallengeWindowGroup journalWindowGroup;
}

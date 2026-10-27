using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF3 RID: 3827
[Preserve]
public class XUiC_ChallengeEntryList : XUiController
{
	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x0600775C RID: 30556 RVA: 0x002D1249 File Offset: 0x002CF449
	// (set) Token: 0x0600775D RID: 30557 RVA: 0x002D1250 File Offset: 0x002CF450
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

	// Token: 0x0600775E RID: 30558 RVA: 0x002D12AC File Offset: 0x002CF4AC
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

	// Token: 0x0600775F RID: 30559 RVA: 0x002D1330 File Offset: 0x002CF530
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

	// Token: 0x06007760 RID: 30560 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007761 RID: 30561 RVA: 0x002D15F4 File Offset: 0x002CF7F4
	public void UnSelect()
	{
		XUiC_ChallengeEntryList.SelectedEntry = null;
	}

	// Token: 0x06007762 RID: 30562 RVA: 0x002D15FC File Offset: 0x002CF7FC
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

	// Token: 0x06007763 RID: 30563 RVA: 0x002D1675 File Offset: 0x002CF875
	public void SetChallengeEntryList(List<Challenge> newChallengeList)
	{
		this.challengeList = newChallengeList;
		this.IsDirty = true;
	}

	// Token: 0x06007764 RID: 30564 RVA: 0x002D1688 File Offset: 0x002CF888
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

	// Token: 0x06007765 RID: 30565 RVA: 0x002D16CB File Offset: 0x002CF8CB
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06007766 RID: 30566 RVA: 0x002D16E9 File Offset: 0x002CF8E9
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x0400585F RID: 22623
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005860 RID: 22624
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeEntry> entryList = new List<XUiC_ChallengeEntry>();

	// Token: 0x04005861 RID: 22625
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_ChallengeEntry selectedEntry;

	// Token: 0x04005862 RID: 22626
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> challengeList;

	// Token: 0x04005863 RID: 22627
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;

	// Token: 0x04005864 RID: 22628
	public XUiC_ChallengeGroupEntry Owner;

	// Token: 0x04005865 RID: 22629
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_ChallengeWindowGroup journalWindowGroup;
}

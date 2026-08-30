using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC4 RID: 3780
[Preserve]
public class XUiC_ChallengeEntryList : XUiController
{
	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x0600769A RID: 30362 RVA: 0x002C93F5 File Offset: 0x002C75F5
	// (set) Token: 0x0600769B RID: 30363 RVA: 0x002C93FC File Offset: 0x002C75FC
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

	// Token: 0x0600769C RID: 30364 RVA: 0x002C9458 File Offset: 0x002C7658
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

	// Token: 0x0600769D RID: 30365 RVA: 0x002C94DC File Offset: 0x002C76DC
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

	// Token: 0x0600769E RID: 30366 RVA: 0x0008A069 File Offset: 0x00088269
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600769F RID: 30367 RVA: 0x002C97A0 File Offset: 0x002C79A0
	public void UnSelect()
	{
		XUiC_ChallengeEntryList.SelectedEntry = null;
	}

	// Token: 0x060076A0 RID: 30368 RVA: 0x002C97A8 File Offset: 0x002C79A8
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

	// Token: 0x060076A1 RID: 30369 RVA: 0x002C9821 File Offset: 0x002C7A21
	public void SetChallengeEntryList(List<Challenge> newChallengeList)
	{
		this.challengeList = newChallengeList;
		this.IsDirty = true;
	}

	// Token: 0x060076A2 RID: 30370 RVA: 0x002C9834 File Offset: 0x002C7A34
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

	// Token: 0x060076A3 RID: 30371 RVA: 0x002C9877 File Offset: 0x002C7A77
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x060076A4 RID: 30372 RVA: 0x002C9895 File Offset: 0x002C7A95
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x0400575D RID: 22365
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400575E RID: 22366
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeEntry> entryList = new List<XUiC_ChallengeEntry>();

	// Token: 0x0400575F RID: 22367
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_ChallengeEntry selectedEntry;

	// Token: 0x04005760 RID: 22368
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> challengeList;

	// Token: 0x04005761 RID: 22369
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;

	// Token: 0x04005762 RID: 22370
	public XUiC_ChallengeGroupEntry Owner;

	// Token: 0x04005763 RID: 22371
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_ChallengeWindowGroup journalWindowGroup;
}

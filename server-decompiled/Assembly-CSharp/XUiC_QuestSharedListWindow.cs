using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001010 RID: 4112
[Preserve]
public class XUiC_QuestSharedListWindow : XUiController
{
	// Token: 0x0600812D RID: 33069 RVA: 0x0031136C File Offset: 0x0030F56C
	public override void Init()
	{
		base.Init();
		this.questList = base.GetChildByType<XUiC_QuestSharedList>();
		this.acceptBtn = (XUiV_Button)base.GetChildById("acceptBtn").ViewComponent;
		this.acceptBtn.Controller.OnPress += this.acceptBtn_OnPress;
		this.showOnMapBtn = (XUiV_Button)base.GetChildById("showOnMapBtn").ViewComponent;
		this.showOnMapBtn.Controller.OnPress += this.showOnMapBtn_OnPress;
		this.questRemoveBtn = (XUiV_Button)base.GetChildById("questRemoveBtn").ViewComponent;
		this.questRemoveBtn.Controller.OnPress += this.questRemoveBtn_OnPress;
		this.buttonSpacing = this.showOnMapBtn.Position.x - this.acceptBtn.Position.x;
		this.txtInput = (XUiC_TextInput)base.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
	}

	// Token: 0x0600812E RID: 33070 RVA: 0x0031149C File Offset: 0x0030F69C
	[PublicizedFrom(EAccessModifier.Private)]
	public void showOnMapBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			Quest quest = this.questList.SelectedEntry.Quest;
			if (quest.HasPosition)
			{
				XUiC_WindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "map");
				((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).PositionMapAt(quest.Position);
				return;
			}
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, Localization.Get("ttQuestNoLocation", false, null), false, false, 0f);
		}
	}

	// Token: 0x0600812F RID: 33071 RVA: 0x0031153C File Offset: 0x0030F73C
	[PublicizedFrom(EAccessModifier.Private)]
	public void acceptBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		SharedQuestEntry sharedQuestEntry = ((selectedEntry != null) ? selectedEntry.SharedQuestEntry : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.AcceptSharedQuest(sharedQuestEntry, entityPlayer);
	}

	// Token: 0x06008130 RID: 33072 RVA: 0x00311577 File Offset: 0x0030F777
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x06008131 RID: 33073 RVA: 0x00311587 File Offset: 0x0030F787
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveSharedQuestEntry(this.questList.SelectedEntry.SharedQuestEntry);
		}
	}

	// Token: 0x06008132 RID: 33074 RVA: 0x003115C0 File Offset: 0x0030F7C0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.filterText = this.filterText.ToLower();
			this.currentItems = this.player.QuestJournal.sharedQuestEntries.Where<SharedQuestEntry>((SharedQuestEntry quest) => this.filterText == "" || quest.QuestClass.Name.ToLower().Contains(this.filterText)).ToList<SharedQuestEntry>();
			this.questList.SetSharedQuestList(this.currentItems);
			this.IsDirty = false;
		}
	}

	// Token: 0x06008133 RID: 33075 RVA: 0x00311634 File Offset: 0x0030F834
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.SharedQuestAdded += this.QuestJournal_SharedQuestAdded;
		this.player.SharedQuestRemoved += this.QuestJournal_SharedQuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x06008134 RID: 33076 RVA: 0x00311692 File Offset: 0x0030F892
	public override void OnClose()
	{
		base.OnClose();
		this.player.SharedQuestAdded -= this.QuestJournal_SharedQuestAdded;
		this.player.SharedQuestRemoved -= this.QuestJournal_SharedQuestRemoved;
	}

	// Token: 0x06008135 RID: 33077 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_SharedQuestAdded(SharedQuestEntry entry)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008136 RID: 33078 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_SharedQuestRemoved(SharedQuestEntry entry)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008137 RID: 33079 RVA: 0x003116C8 File Offset: 0x0030F8C8
	public void ShowTrackButton(bool _show)
	{
		this.acceptBtn.IsVisible = _show;
		if (this.showingTrackButton != _show)
		{
			this.showingTrackButton = _show;
			this.acceptBtn.Enabled = _show;
			Vector3 localPosition = this.showOnMapBtn.UiTransform.localPosition;
			Vector3 localPosition2 = this.questRemoveBtn.UiTransform.localPosition;
			if (_show)
			{
				this.showOnMapBtn.UiTransform.localPosition = new Vector3(localPosition.x + (float)this.buttonSpacing, localPosition.y, localPosition.z);
				this.questRemoveBtn.UiTransform.localPosition = new Vector3(localPosition2.x + (float)this.buttonSpacing, localPosition2.y, localPosition2.z);
				return;
			}
			this.showOnMapBtn.UiTransform.localPosition = new Vector3(localPosition.x - (float)this.buttonSpacing, localPosition.y, localPosition.z);
			this.questRemoveBtn.UiTransform.localPosition = new Vector3(localPosition2.x - (float)this.buttonSpacing, localPosition2.y, localPosition2.z);
		}
	}

	// Token: 0x04005FD5 RID: 24533
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005FD6 RID: 24534
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedListWindow.SearchTypes searchType;

	// Token: 0x04005FD7 RID: 24535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedList questList;

	// Token: 0x04005FD8 RID: 24536
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button acceptBtn;

	// Token: 0x04005FD9 RID: 24537
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005FDA RID: 24538
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005FDB RID: 24539
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005FDC RID: 24540
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> currentItems;

	// Token: 0x04005FDD RID: 24541
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005FDE RID: 24542
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005FDF RID: 24543
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02001011 RID: 4113
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005FE1 RID: 24545
		All,
		// Token: 0x04005FE2 RID: 24546
		Active,
		// Token: 0x04005FE3 RID: 24547
		Completed
	}
}

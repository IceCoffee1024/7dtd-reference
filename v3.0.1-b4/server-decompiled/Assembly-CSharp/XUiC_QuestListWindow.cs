using System;
using System.Collections.Generic;
using System.Globalization;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC7 RID: 4039
[Preserve]
public class XUiC_QuestListWindow : XUiController
{
	// Token: 0x06007F54 RID: 32596 RVA: 0x00304C24 File Offset: 0x00302E24
	public override void Init()
	{
		base.Init();
		this.questList = base.GetChildByType<XUiC_QuestList>();
		this.questList.QuestListWindow = this;
		this.trackBtn = (XUiV_Button)base.GetChildById("trackBtn").ViewComponent;
		this.trackBtn.Controller.OnPress += this.trackBtn_OnPress;
		this.showOnMapBtn = (XUiV_Button)base.GetChildById("showOnMapBtn").ViewComponent;
		this.showOnMapBtn.Controller.OnPress += this.showOnMapBtn_OnPress;
		this.questRemoveBtn = (XUiV_Button)base.GetChildById("questRemoveBtn").ViewComponent;
		this.questRemoveBtn.Controller.OnPress += this.questRemoveBtn_OnPress;
		this.questShareBtn = (XUiV_Button)base.GetChildById("questShareBtn").ViewComponent;
		this.questShareBtn.Controller.OnPress += this.questShareBtn_OnPress;
		this.buttonSpacing = this.showOnMapBtn.Position.x - this.trackBtn.Position.x;
		this.txtInput = (XUiC_TextInput)base.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
	}

	// Token: 0x06007F55 RID: 32597 RVA: 0x00304D98 File Offset: 0x00302F98
	[PublicizedFrom(EAccessModifier.Private)]
	public void questShareBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		Quest quest = ((selectedEntry != null) ? selectedEntry.Quest : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.ShareQuestWithParty(quest, entityPlayer, true);
	}

	// Token: 0x06007F56 RID: 32598 RVA: 0x00304DD4 File Offset: 0x00302FD4
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

	// Token: 0x06007F57 RID: 32599 RVA: 0x00304E74 File Offset: 0x00303074
	[PublicizedFrom(EAccessModifier.Private)]
	public void trackBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiM_Quest questTracker = this.xui.QuestTracker;
		if (this.questList.SelectedEntry != null)
		{
			Quest quest = this.questList.SelectedEntry.Quest;
			if (quest.Active)
			{
				quest.Tracked = !quest.Tracked;
				this.xui.playerUI.entityPlayer.QuestJournal.TrackedQuest = (quest.Tracked ? quest : null);
				this.xui.playerUI.entityPlayer.QuestJournal.RefreshTracked();
			}
		}
	}

	// Token: 0x06007F58 RID: 32600 RVA: 0x00304F02 File Offset: 0x00303102
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x06007F59 RID: 32601 RVA: 0x00304F14 File Offset: 0x00303114
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveQuest(this.questList.SelectedEntry.Quest);
			this.questList.SelectedEntry = null;
		}
	}

	// Token: 0x06007F5A RID: 32602 RVA: 0x00304F64 File Offset: 0x00303164
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.filterText = this.filterText.ToLower();
			this.currentItems = (from quest in this.player.QuestJournal.quests
				where this.filterText == "" || QuestClass.GetQuest(quest.ID).Name.ToLower().Contains(this.filterText)
				orderby !quest.Active, quest.FinishTime descending, QuestClass.GetQuest(quest.ID).Name
				select quest).ToList<Quest>();
			this.questList.SetQuestList(this.currentItems);
			this.IsDirty = false;
		}
	}

	// Token: 0x06007F5B RID: 32603 RVA: 0x00305044 File Offset: 0x00303244
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestAccepted += this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved += this.QuestJournal_QuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x06007F5C RID: 32604 RVA: 0x003050A2 File Offset: 0x003032A2
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestAccepted -= this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved -= this.QuestJournal_QuestRemoved;
	}

	// Token: 0x06007F5D RID: 32605 RVA: 0x003050D8 File Offset: 0x003032D8
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "visible_quest_count")
		{
			this.questList.VisibleEntries = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007F5E RID: 32606 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestAccepted(Quest q)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007F5F RID: 32607 RVA: 0x00305105 File Offset: 0x00303305
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestRemoved(Quest q)
	{
		this.IsDirty = true;
		if (q == this.xui.QuestTracker.TrackedQuest)
		{
			this.xui.QuestTracker.TrackedQuest = null;
		}
	}

	// Token: 0x06007F60 RID: 32608 RVA: 0x00305134 File Offset: 0x00303334
	public void ShowTrackButton(bool _show)
	{
		this.trackBtn.IsVisible = _show;
		if (this.showingTrackButton != _show)
		{
			this.showingTrackButton = _show;
			this.trackBtn.Enabled = _show;
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

	// Token: 0x06007F61 RID: 32609 RVA: 0x0030524E File Offset: 0x0030344E
	public void ShowShareQuest(bool _show)
	{
		this.questShareBtn.IsVisible = _show && !PartyQuests.AutoShare;
	}

	// Token: 0x06007F62 RID: 32610 RVA: 0x00305269 File Offset: 0x00303469
	public void ShowRemoveQuest(bool _show)
	{
		this.questRemoveBtn.IsVisible = _show;
	}

	// Token: 0x04005E33 RID: 24115
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005E34 RID: 24116
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestListWindow.SearchTypes searchType;

	// Token: 0x04005E35 RID: 24117
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestList questList;

	// Token: 0x04005E36 RID: 24118
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button trackBtn;

	// Token: 0x04005E37 RID: 24119
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005E38 RID: 24120
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005E39 RID: 24121
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questShareBtn;

	// Token: 0x04005E3A RID: 24122
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005E3B RID: 24123
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> currentItems;

	// Token: 0x04005E3C RID: 24124
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005E3D RID: 24125
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005E3E RID: 24126
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02000FC8 RID: 4040
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005E40 RID: 24128
		All,
		// Token: 0x04005E41 RID: 24129
		Active,
		// Token: 0x04005E42 RID: 24130
		Completed
	}
}

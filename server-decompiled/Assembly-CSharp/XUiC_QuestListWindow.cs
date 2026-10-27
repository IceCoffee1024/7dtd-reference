using System;
using System.Collections.Generic;
using System.Globalization;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FFF RID: 4095
[Preserve]
public class XUiC_QuestListWindow : XUiController
{
	// Token: 0x060080A4 RID: 32932 RVA: 0x0030DEEC File Offset: 0x0030C0EC
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

	// Token: 0x060080A5 RID: 32933 RVA: 0x0030E060 File Offset: 0x0030C260
	[PublicizedFrom(EAccessModifier.Private)]
	public void questShareBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		Quest quest = ((selectedEntry != null) ? selectedEntry.Quest : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.ShareQuestWithParty(quest, entityPlayer, true);
	}

	// Token: 0x060080A6 RID: 32934 RVA: 0x0030E09C File Offset: 0x0030C29C
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

	// Token: 0x060080A7 RID: 32935 RVA: 0x0030E13C File Offset: 0x0030C33C
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

	// Token: 0x060080A8 RID: 32936 RVA: 0x0030E1CA File Offset: 0x0030C3CA
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x060080A9 RID: 32937 RVA: 0x0030E1DC File Offset: 0x0030C3DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveQuest(this.questList.SelectedEntry.Quest);
			this.questList.SelectedEntry = null;
		}
	}

	// Token: 0x060080AA RID: 32938 RVA: 0x0030E22C File Offset: 0x0030C42C
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

	// Token: 0x060080AB RID: 32939 RVA: 0x0030E30C File Offset: 0x0030C50C
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestAccepted += this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved += this.QuestJournal_QuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x060080AC RID: 32940 RVA: 0x0030E36A File Offset: 0x0030C56A
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestAccepted -= this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved -= this.QuestJournal_QuestRemoved;
	}

	// Token: 0x060080AD RID: 32941 RVA: 0x0030E3A0 File Offset: 0x0030C5A0
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "visible_quest_count")
		{
			this.questList.VisibleEntries = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060080AE RID: 32942 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestAccepted(Quest q)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080AF RID: 32943 RVA: 0x0030E3CD File Offset: 0x0030C5CD
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestRemoved(Quest q)
	{
		this.IsDirty = true;
		if (q == this.xui.QuestTracker.TrackedQuest)
		{
			this.xui.QuestTracker.TrackedQuest = null;
		}
	}

	// Token: 0x060080B0 RID: 32944 RVA: 0x0030E3FC File Offset: 0x0030C5FC
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

	// Token: 0x060080B1 RID: 32945 RVA: 0x0030E516 File Offset: 0x0030C716
	public void ShowShareQuest(bool _show)
	{
		this.questShareBtn.IsVisible = _show && !PartyQuests.AutoShare;
	}

	// Token: 0x060080B2 RID: 32946 RVA: 0x0030E531 File Offset: 0x0030C731
	public void ShowRemoveQuest(bool _show)
	{
		this.questRemoveBtn.IsVisible = _show;
	}

	// Token: 0x04005F5E RID: 24414
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F5F RID: 24415
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestListWindow.SearchTypes searchType;

	// Token: 0x04005F60 RID: 24416
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestList questList;

	// Token: 0x04005F61 RID: 24417
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button trackBtn;

	// Token: 0x04005F62 RID: 24418
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005F63 RID: 24419
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005F64 RID: 24420
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questShareBtn;

	// Token: 0x04005F65 RID: 24421
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005F66 RID: 24422
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> currentItems;

	// Token: 0x04005F67 RID: 24423
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005F68 RID: 24424
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005F69 RID: 24425
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02001000 RID: 4096
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005F6B RID: 24427
		All,
		// Token: 0x04005F6C RID: 24428
		Active,
		// Token: 0x04005F6D RID: 24429
		Completed
	}
}

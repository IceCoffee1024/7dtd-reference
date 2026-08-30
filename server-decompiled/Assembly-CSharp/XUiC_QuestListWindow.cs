using System;
using System.Collections.Generic;
using System.Globalization;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FE0 RID: 4064
[Preserve]
public class XUiC_QuestListWindow : XUiController
{
	// Token: 0x06008046 RID: 32838 RVA: 0x003093F8 File Offset: 0x003075F8
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

	// Token: 0x06008047 RID: 32839 RVA: 0x0030956C File Offset: 0x0030776C
	[PublicizedFrom(EAccessModifier.Private)]
	public void questShareBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		Quest quest = ((selectedEntry != null) ? selectedEntry.Quest : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.ShareQuestWithParty(quest, entityPlayer, true);
	}

	// Token: 0x06008048 RID: 32840 RVA: 0x003095A8 File Offset: 0x003077A8
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

	// Token: 0x06008049 RID: 32841 RVA: 0x00309648 File Offset: 0x00307848
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

	// Token: 0x0600804A RID: 32842 RVA: 0x003096D6 File Offset: 0x003078D6
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x0600804B RID: 32843 RVA: 0x003096E8 File Offset: 0x003078E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveQuest(this.questList.SelectedEntry.Quest);
			this.questList.SelectedEntry = null;
		}
	}

	// Token: 0x0600804C RID: 32844 RVA: 0x00309738 File Offset: 0x00307938
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

	// Token: 0x0600804D RID: 32845 RVA: 0x00309818 File Offset: 0x00307A18
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestAccepted += this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved += this.QuestJournal_QuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x0600804E RID: 32846 RVA: 0x00309876 File Offset: 0x00307A76
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestAccepted -= this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved -= this.QuestJournal_QuestRemoved;
	}

	// Token: 0x0600804F RID: 32847 RVA: 0x003098AC File Offset: 0x00307AAC
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "visible_quest_count")
		{
			this.questList.VisibleEntries = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06008050 RID: 32848 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestAccepted(Quest q)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008051 RID: 32849 RVA: 0x003098D9 File Offset: 0x00307AD9
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestRemoved(Quest q)
	{
		this.IsDirty = true;
		if (q == this.xui.QuestTracker.TrackedQuest)
		{
			this.xui.QuestTracker.TrackedQuest = null;
		}
	}

	// Token: 0x06008052 RID: 32850 RVA: 0x00309908 File Offset: 0x00307B08
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

	// Token: 0x06008053 RID: 32851 RVA: 0x00309A22 File Offset: 0x00307C22
	public void ShowShareQuest(bool _show)
	{
		this.questShareBtn.IsVisible = _show && !PartyQuests.AutoShare;
	}

	// Token: 0x06008054 RID: 32852 RVA: 0x00309A3D File Offset: 0x00307C3D
	public void ShowRemoveQuest(bool _show)
	{
		this.questRemoveBtn.IsVisible = _show;
	}

	// Token: 0x04005EFA RID: 24314
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005EFB RID: 24315
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestListWindow.SearchTypes searchType;

	// Token: 0x04005EFC RID: 24316
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestList questList;

	// Token: 0x04005EFD RID: 24317
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button trackBtn;

	// Token: 0x04005EFE RID: 24318
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005EFF RID: 24319
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005F00 RID: 24320
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questShareBtn;

	// Token: 0x04005F01 RID: 24321
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005F02 RID: 24322
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> currentItems;

	// Token: 0x04005F03 RID: 24323
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005F04 RID: 24324
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005F05 RID: 24325
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02000FE1 RID: 4065
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005F07 RID: 24327
		All,
		// Token: 0x04005F08 RID: 24328
		Active,
		// Token: 0x04005F09 RID: 24329
		Completed
	}
}

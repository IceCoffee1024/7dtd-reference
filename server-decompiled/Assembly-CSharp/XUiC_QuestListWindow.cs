using System;
using System.Collections.Generic;
using System.Globalization;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FEF RID: 4079
[Preserve]
public class XUiC_QuestListWindow : XUiController
{
	// Token: 0x06008055 RID: 32853 RVA: 0x0030C3D0 File Offset: 0x0030A5D0
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

	// Token: 0x06008056 RID: 32854 RVA: 0x0030C544 File Offset: 0x0030A744
	[PublicizedFrom(EAccessModifier.Private)]
	public void questShareBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		Quest quest = ((selectedEntry != null) ? selectedEntry.Quest : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.ShareQuestWithParty(quest, entityPlayer, true);
	}

	// Token: 0x06008057 RID: 32855 RVA: 0x0030C580 File Offset: 0x0030A780
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

	// Token: 0x06008058 RID: 32856 RVA: 0x0030C620 File Offset: 0x0030A820
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

	// Token: 0x06008059 RID: 32857 RVA: 0x0030C6AE File Offset: 0x0030A8AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x0600805A RID: 32858 RVA: 0x0030C6C0 File Offset: 0x0030A8C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveQuest(this.questList.SelectedEntry.Quest);
			this.questList.SelectedEntry = null;
		}
	}

	// Token: 0x0600805B RID: 32859 RVA: 0x0030C710 File Offset: 0x0030A910
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

	// Token: 0x0600805C RID: 32860 RVA: 0x0030C7F0 File Offset: 0x0030A9F0
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestAccepted += this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved += this.QuestJournal_QuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x0600805D RID: 32861 RVA: 0x0030C84E File Offset: 0x0030AA4E
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestAccepted -= this.QuestJournal_QuestAccepted;
		this.player.QuestRemoved -= this.QuestJournal_QuestRemoved;
	}

	// Token: 0x0600805E RID: 32862 RVA: 0x0030C884 File Offset: 0x0030AA84
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "visible_quest_count")
		{
			this.questList.VisibleEntries = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0600805F RID: 32863 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestAccepted(Quest q)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008060 RID: 32864 RVA: 0x0030C8B1 File Offset: 0x0030AAB1
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestRemoved(Quest q)
	{
		this.IsDirty = true;
		if (q == this.xui.QuestTracker.TrackedQuest)
		{
			this.xui.QuestTracker.TrackedQuest = null;
		}
	}

	// Token: 0x06008061 RID: 32865 RVA: 0x0030C8E0 File Offset: 0x0030AAE0
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

	// Token: 0x06008062 RID: 32866 RVA: 0x0030C9FA File Offset: 0x0030ABFA
	public void ShowShareQuest(bool _show)
	{
		this.questShareBtn.IsVisible = _show && !PartyQuests.AutoShare;
	}

	// Token: 0x06008063 RID: 32867 RVA: 0x0030CA15 File Offset: 0x0030AC15
	public void ShowRemoveQuest(bool _show)
	{
		this.questRemoveBtn.IsVisible = _show;
	}

	// Token: 0x04005F23 RID: 24355
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F24 RID: 24356
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestListWindow.SearchTypes searchType;

	// Token: 0x04005F25 RID: 24357
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestList questList;

	// Token: 0x04005F26 RID: 24358
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button trackBtn;

	// Token: 0x04005F27 RID: 24359
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005F28 RID: 24360
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005F29 RID: 24361
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questShareBtn;

	// Token: 0x04005F2A RID: 24362
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005F2B RID: 24363
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> currentItems;

	// Token: 0x04005F2C RID: 24364
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005F2D RID: 24365
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005F2E RID: 24366
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02000FF0 RID: 4080
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005F30 RID: 24368
		All,
		// Token: 0x04005F31 RID: 24369
		Active,
		// Token: 0x04005F32 RID: 24370
		Completed
	}
}

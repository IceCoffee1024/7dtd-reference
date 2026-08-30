using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FF1 RID: 4081
[Preserve]
public class XUiC_QuestSharedListWindow : XUiController
{
	// Token: 0x060080CF RID: 32975 RVA: 0x0030C868 File Offset: 0x0030AA68
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

	// Token: 0x060080D0 RID: 32976 RVA: 0x0030C998 File Offset: 0x0030AB98
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

	// Token: 0x060080D1 RID: 32977 RVA: 0x0030CA38 File Offset: 0x0030AC38
	[PublicizedFrom(EAccessModifier.Private)]
	public void acceptBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		SharedQuestEntry sharedQuestEntry = ((selectedEntry != null) ? selectedEntry.SharedQuestEntry : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.AcceptSharedQuest(sharedQuestEntry, entityPlayer);
	}

	// Token: 0x060080D2 RID: 32978 RVA: 0x0030CA73 File Offset: 0x0030AC73
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x060080D3 RID: 32979 RVA: 0x0030CA83 File Offset: 0x0030AC83
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveSharedQuestEntry(this.questList.SelectedEntry.SharedQuestEntry);
		}
	}

	// Token: 0x060080D4 RID: 32980 RVA: 0x0030CABC File Offset: 0x0030ACBC
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

	// Token: 0x060080D5 RID: 32981 RVA: 0x0030CB30 File Offset: 0x0030AD30
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.SharedQuestAdded += this.QuestJournal_SharedQuestAdded;
		this.player.SharedQuestRemoved += this.QuestJournal_SharedQuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x060080D6 RID: 32982 RVA: 0x0030CB8E File Offset: 0x0030AD8E
	public override void OnClose()
	{
		base.OnClose();
		this.player.SharedQuestAdded -= this.QuestJournal_SharedQuestAdded;
		this.player.SharedQuestRemoved -= this.QuestJournal_SharedQuestRemoved;
	}

	// Token: 0x060080D7 RID: 32983 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_SharedQuestAdded(SharedQuestEntry entry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080D8 RID: 32984 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_SharedQuestRemoved(SharedQuestEntry entry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080D9 RID: 32985 RVA: 0x0030CBC4 File Offset: 0x0030ADC4
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

	// Token: 0x04005F71 RID: 24433
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F72 RID: 24434
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedListWindow.SearchTypes searchType;

	// Token: 0x04005F73 RID: 24435
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedList questList;

	// Token: 0x04005F74 RID: 24436
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button acceptBtn;

	// Token: 0x04005F75 RID: 24437
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005F76 RID: 24438
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005F77 RID: 24439
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005F78 RID: 24440
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> currentItems;

	// Token: 0x04005F79 RID: 24441
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005F7A RID: 24442
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005F7B RID: 24443
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02000FF2 RID: 4082
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005F7D RID: 24445
		All,
		// Token: 0x04005F7E RID: 24446
		Active,
		// Token: 0x04005F7F RID: 24447
		Completed
	}
}

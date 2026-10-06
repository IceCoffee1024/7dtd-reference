using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001000 RID: 4096
[Preserve]
public class XUiC_QuestSharedListWindow : XUiController
{
	// Token: 0x060080DE RID: 32990 RVA: 0x0030F850 File Offset: 0x0030DA50
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

	// Token: 0x060080DF RID: 32991 RVA: 0x0030F980 File Offset: 0x0030DB80
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

	// Token: 0x060080E0 RID: 32992 RVA: 0x0030FA20 File Offset: 0x0030DC20
	[PublicizedFrom(EAccessModifier.Private)]
	public void acceptBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry selectedEntry = this.questList.SelectedEntry;
		SharedQuestEntry sharedQuestEntry = ((selectedEntry != null) ? selectedEntry.SharedQuestEntry : null);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		PartyQuests.AcceptSharedQuest(sharedQuestEntry, entityPlayer);
	}

	// Token: 0x060080E1 RID: 32993 RVA: 0x0030FA5B File Offset: 0x0030DC5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x060080E2 RID: 32994 RVA: 0x0030FA6B File Offset: 0x0030DC6B
	[PublicizedFrom(EAccessModifier.Private)]
	public void questRemoveBtn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.questList.SelectedEntry != null)
		{
			this.xui.playerUI.entityPlayer.QuestJournal.RemoveSharedQuestEntry(this.questList.SelectedEntry.SharedQuestEntry);
		}
	}

	// Token: 0x060080E3 RID: 32995 RVA: 0x0030FAA4 File Offset: 0x0030DCA4
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

	// Token: 0x060080E4 RID: 32996 RVA: 0x0030FB18 File Offset: 0x0030DD18
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.SharedQuestAdded += this.QuestJournal_SharedQuestAdded;
		this.player.SharedQuestRemoved += this.QuestJournal_SharedQuestRemoved;
		this.IsDirty = true;
	}

	// Token: 0x060080E5 RID: 32997 RVA: 0x0030FB76 File Offset: 0x0030DD76
	public override void OnClose()
	{
		base.OnClose();
		this.player.SharedQuestAdded -= this.QuestJournal_SharedQuestAdded;
		this.player.SharedQuestRemoved -= this.QuestJournal_SharedQuestRemoved;
	}

	// Token: 0x060080E6 RID: 32998 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_SharedQuestAdded(SharedQuestEntry entry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080E7 RID: 32999 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_SharedQuestRemoved(SharedQuestEntry entry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080E8 RID: 33000 RVA: 0x0030FBAC File Offset: 0x0030DDAC
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

	// Token: 0x04005F9A RID: 24474
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F9B RID: 24475
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedListWindow.SearchTypes searchType;

	// Token: 0x04005F9C RID: 24476
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedList questList;

	// Token: 0x04005F9D RID: 24477
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button acceptBtn;

	// Token: 0x04005F9E RID: 24478
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button questRemoveBtn;

	// Token: 0x04005F9F RID: 24479
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showOnMapBtn;

	// Token: 0x04005FA0 RID: 24480
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005FA1 RID: 24481
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> currentItems;

	// Token: 0x04005FA2 RID: 24482
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x04005FA3 RID: 24483
	[PublicizedFrom(EAccessModifier.Private)]
	public int buttonSpacing;

	// Token: 0x04005FA4 RID: 24484
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingTrackButton = true;

	// Token: 0x02001001 RID: 4097
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchTypes
	{
		// Token: 0x04005FA6 RID: 24486
		All,
		// Token: 0x04005FA7 RID: 24487
		Active,
		// Token: 0x04005FA8 RID: 24488
		Completed
	}
}

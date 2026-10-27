using System;
using System.Threading.Tasks;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x0200101C RID: 4124
[Preserve]
public class XUiC_QuestWindowGroup : XUiController
{
	// Token: 0x06008197 RID: 33175 RVA: 0x00314CD4 File Offset: 0x00312ED4
	public override void Init()
	{
		base.Init();
		this.objectivesWindow = base.GetChildByType<XUiC_QuestObjectivesWindow>();
		this.rewardsWindow = base.GetChildByType<XUiC_QuestRewardsWindow>();
		this.descriptionWindow = base.GetChildByType<XUiC_QuestDescriptionWindow>();
		XUiC_QuestListWindow childByType = base.GetChildByType<XUiC_QuestListWindow>();
		this.questList = ((childByType != null) ? childByType.GetChildByType<XUiC_QuestList>() : null);
		XUiC_QuestSharedListWindow childByType2 = base.GetChildByType<XUiC_QuestSharedListWindow>();
		this.sharedList = ((childByType2 != null) ? childByType2.GetChildByType<XUiC_QuestSharedList>() : null);
		if (this.questList != null)
		{
			this.questList.SharedList = this.sharedList;
		}
		if (this.sharedList != null)
		{
			this.sharedList.QuestList = this.questList;
		}
	}

	// Token: 0x06008198 RID: 33176 RVA: 0x00314D6D File Offset: 0x00312F6D
	public void SetQuest(XUiC_QuestEntry q)
	{
		this.objectivesWindow.SetQuest(q);
		this.rewardsWindow.SetQuest(q);
		this.descriptionWindow.SetQuest(q);
	}

	// Token: 0x06008199 RID: 33177 RVA: 0x00314D94 File Offset: 0x00312F94
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		XUiController xuiController = this.xui.FindWindowGroupByName("windowpaging");
		if (xuiController != null)
		{
			XUiC_WindowSelector childByType = xuiController.GetChildByType<XUiC_WindowSelector>();
			if (childByType != null)
			{
				childByType.SetSelected("quests");
			}
		}
		base.RefreshBindings();
		this.AsyncUISelectionOnOpen();
	}

	// Token: 0x0600819A RID: 33178 RVA: 0x00314E50 File Offset: 0x00313050
	[PublicizedFrom(EAccessModifier.Private)]
	public async void AsyncUISelectionOnOpen()
	{
		this.xui.playerUI.CursorController.Locked = true;
		for (int i = 0; i < 3; i++)
		{
			await Task.Yield();
		}
		this.xui.playerUI.CursorController.Locked = false;
		if (this.questList.HasQuests())
		{
			this.questList.SelectCursorElement(true, false);
		}
		else if (this.sharedList.HasQuests())
		{
			this.sharedList.SelectCursorElement(true, false);
		}
		else
		{
			base.GetChildById("content").SelectCursorElement(true, false);
		}
	}

	// Token: 0x0600819B RID: 33179 RVA: 0x002D215D File Offset: 0x002D035D
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x0600819C RID: 33180 RVA: 0x00314E88 File Offset: 0x00313088
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "questsautoshare")
		{
			_value = PartyQuests.AutoShare.ToString();
			return true;
		}
		if (_bindingName == "questsautoaccept")
		{
			_value = PartyQuests.AutoAccept.ToString();
			return true;
		}
		if (!(_bindingName == "queststier"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (entityPlayer != null)
		{
			int currentFactionTier = entityPlayer.QuestJournal.GetCurrentFactionTier(1, 0, false);
			_value = string.Format(Localization.Get("xuiQuestTierDescription", false, null), ValueDisplayFormatters.RomanNumber(entityPlayer.QuestJournal.GetCurrentFactionTier(1, 0, false)), entityPlayer.QuestJournal.GetQuestFactionPoints(1), entityPlayer.QuestJournal.GetQuestFactionMax(1, currentFactionTier));
		}
		return true;
	}

	// Token: 0x04006034 RID: 24628
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestList questList;

	// Token: 0x04006035 RID: 24629
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestSharedList sharedList;

	// Token: 0x04006036 RID: 24630
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestObjectivesWindow objectivesWindow;

	// Token: 0x04006037 RID: 24631
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestRewardsWindow rewardsWindow;

	// Token: 0x04006038 RID: 24632
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestDescriptionWindow descriptionWindow;
}

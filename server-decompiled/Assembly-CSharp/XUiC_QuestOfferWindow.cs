using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FF6 RID: 4086
[Preserve]
public class XUiC_QuestOfferWindow : XUiController
{
	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x06008089 RID: 32905 RVA: 0x0030D9B2 File Offset: 0x0030BBB2
	// (set) Token: 0x0600808A RID: 32906 RVA: 0x0030D9BA File Offset: 0x0030BBBA
	public XUiC_QuestOfferWindow.OfferTypes OfferType
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x0600808B RID: 32907 RVA: 0x0030D9C3 File Offset: 0x0030BBC3
	// (set) Token: 0x0600808C RID: 32908 RVA: 0x0030D9CB File Offset: 0x0030BBCB
	public Quest Quest
	{
		get
		{
			return this.quest;
		}
		set
		{
			this.quest = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000F1D RID: 3869
	// (get) Token: 0x0600808D RID: 32909 RVA: 0x0030D9DB File Offset: 0x0030BBDB
	// (set) Token: 0x0600808E RID: 32910 RVA: 0x0030D9E3 File Offset: 0x0030BBE3
	public int Variation
	{
		get
		{
			return this.variation;
		}
		set
		{
			this.variation = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x0600808F RID: 32911 RVA: 0x0030D9F3 File Offset: 0x0030BBF3
	// (set) Token: 0x06008090 RID: 32912 RVA: 0x0030D9FB File Offset: 0x0030BBFB
	public XUiC_ItemStack ItemStackController { get; set; }

	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x06008091 RID: 32913 RVA: 0x0030DA04 File Offset: 0x0030BC04
	// (set) Token: 0x06008092 RID: 32914 RVA: 0x0030DA0C File Offset: 0x0030BC0C
	public int QuestGiverID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06008093 RID: 32915 RVA: 0x0030DA18 File Offset: 0x0030BC18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "questname")
		{
			value = ((this.Quest != null) ? this.Quest.GetParsedText(this.Quest.QuestClass.Name) : "");
			return true;
		}
		if (bindingName == "questicon")
		{
			value = ((this.Quest != null) ? this.Quest.QuestClass.Icon : "");
			return true;
		}
		if (bindingName == "questoffer")
		{
			value = ((this.Quest != null) ? this.Quest.GetParsedText(this.Quest.QuestClass.Offer) : "");
			return true;
		}
		if (bindingName == "questdifficulty")
		{
			value = ((this.Quest != null) ? this.Quest.QuestClass.Difficulty : "");
			return true;
		}
		if (bindingName == "tieradd")
		{
			if (this.Quest != null && this.Quest.QuestClass.AddsToTierComplete)
			{
				if (!this.xui.playerUI.entityPlayer.QuestJournal.CanAddProgression)
				{
					value = "";
				}
				else
				{
					string text = ((this.Quest.QuestClass.DifficultyTier > 0) ? "+" : "-") + this.Quest.QuestClass.DifficultyTier.ToString();
					value = string.Format(Localization.Get("xuiQuestTierAdd", false, null), text);
				}
			}
			else
			{
				value = "";
			}
			return true;
		}
		if (!(bindingName == "tieraddlimited"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		if (this.Quest != null && this.Quest.QuestClass.AddsToTierComplete && !this.xui.playerUI.entityPlayer.QuestJournal.CanAddProgression)
		{
			value = "true";
		}
		else
		{
			value = "false";
		}
		return true;
	}

	// Token: 0x06008094 RID: 32916 RVA: 0x0030DC14 File Offset: 0x0030BE14
	public override void Init()
	{
		base.Init();
		this.btnAccept = base.GetChildById("btnAccept");
		this.btnAccept_Background = (XUiV_Button)this.btnAccept.GetChildById("clickable").ViewComponent;
		this.btnAccept_Background.Controller.OnPress += this.btnAccept_OnPress;
		this.btnAccept_Background.Controller.OnHover += this.btnAccept_OnHover;
		this.btnDecline = base.GetChildById("btnDecline");
		this.btnDecline_Background = (XUiV_Button)this.btnDecline.GetChildById("clickable").ViewComponent;
		this.btnDecline_Background.Controller.OnPress += this.btnDecline_OnPress;
		this.btnDecline_Background.Controller.OnHover += this.btnDecline_OnHover;
	}

	// Token: 0x06008095 RID: 32917 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008096 RID: 32918 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008097 RID: 32919 RVA: 0x0030DCFC File Offset: 0x0030BEFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnPress(XUiController _sender, int _mouseButton)
	{
		Quest quest = this.Quest;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			quest = this.Quest.Clone();
		}
		else if (quest.CurrentState != Quest.QuestState.InProgress)
		{
			quest.ResetQuest();
		}
		quest.QuestGiverID = this.QuestGiverID;
		if (this.OfferType == XUiC_QuestOfferWindow.OfferTypes.Item)
		{
			ItemStack itemStack = this.ItemStackController.ItemStack;
			if (itemStack.count > 1)
			{
				ItemStack itemStack2 = itemStack;
				int count = itemStack2.count;
				itemStack2.count = count - 1;
				this.ItemStackController.ForceSetItemStack(itemStack.Clone());
				this.ItemStackController.WindowGroup.Controller.SetAllChildrenDirty();
			}
			else
			{
				this.ItemStackController.ItemStack = ItemStack.Empty;
				this.ItemStackController.WindowGroup.Controller.SetAllChildrenDirty();
			}
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.QuestGiverID != -1)
		{
			this.xui.Dialog.Respondent.PlayVoiceSetEntry("quest_accepted", entityPlayer, true, true);
		}
		this.questAccepted = true;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		entityPlayer.QuestJournal.AddQuest(quest, (quest.QuestGiverID != -1) ? Quest.QuestSource.Trader : Quest.QuestSource.TreasureMap, true);
	}

	// Token: 0x06008098 RID: 32920 RVA: 0x0030DE34 File Offset: 0x0030C034
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.QuestGiverID != -1)
		{
			this.xui.Dialog.Respondent.PlayVoiceSetEntry("quest_declined", this.xui.playerUI.entityPlayer, true, true);
		}
		this.questAccepted = false;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06008099 RID: 32921 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600809A RID: 32922 RVA: 0x0030DE9C File Offset: 0x0030C09C
	public static XUiC_QuestOfferWindow OpenQuestOfferWindow(XUi xui, Quest q, int listIndex = -1, XUiC_QuestOfferWindow.OfferTypes offerType = XUiC_QuestOfferWindow.OfferTypes.Item, int questGiverID = -1, Action<EntityNPC> onDenied = null)
	{
		bool flag = offerType == XUiC_QuestOfferWindow.OfferTypes.Item;
		XUiC_QuestOfferWindow childByType = xui.FindWindowGroupByName("questOffer").GetChildByType<XUiC_QuestOfferWindow>();
		childByType.Quest = q;
		childByType.Variation = -1;
		childByType.listIndex = listIndex;
		childByType.QuestGiverID = questGiverID;
		childByType.OfferType = offerType;
		childByType.OnDenied = onDenied;
		xui.playerUI.windowManager.Open("questOffer", flag);
		return childByType;
	}

	// Token: 0x0600809B RID: 32923 RVA: 0x0030DF00 File Offset: 0x0030C100
	public override void OnOpen()
	{
		base.OnOpen();
		this.questAccepted = false;
		Manager.PlayInsidePlayerHead("quest_note_offer", -1, 0f, false, false);
		this.xui.playerUI.windowManager.Close("windowpaging");
		this.xui.playerUI.windowManager.Close("toolbelt");
		if (this.OfferType == XUiC_QuestOfferWindow.OfferTypes.Dialog)
		{
			Dialog currentDialog = this.xui.Dialog.DialogWindowGroup.CurrentDialog;
			this.xui.Dialog.DialogWindowGroup.RefreshDialog();
			this.xui.Dialog.DialogWindowGroup.ShowResponseWindow(false);
			if (this.QuestGiverID != -1)
			{
				this.xui.Dialog.Respondent.PlayVoiceSetEntry("quest_offer", this.xui.playerUI.entityPlayer, true, true);
			}
		}
	}

	// Token: 0x0600809C RID: 32924 RVA: 0x0030DFE0 File Offset: 0x0030C1E0
	public override void OnClose()
	{
		base.OnClose();
		if (!this.questAccepted)
		{
			Manager.PlayInsidePlayerHead("quest_note_decline", -1, 0f, false, false);
			if (this.OnDenied != null)
			{
				this.OnDenied(this.xui.Dialog.Respondent);
			}
		}
		if (this.ItemStackController != null)
		{
			this.ItemStackController.QuestLock = false;
			this.ItemStackController = null;
		}
		if (this.OfferType == XUiC_QuestOfferWindow.OfferTypes.Dialog)
		{
			if (this.questAccepted)
			{
				EntityTrader entityTrader = this.xui.Dialog.Respondent as EntityTrader;
				if (entityTrader != null && entityTrader.activeQuests != null && this.Quest.QuestClass.QuestType != "special")
				{
					entityTrader.activeQuests.Remove(this.Quest);
				}
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.Quest.QuestClass.QuestType != "special")
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageNPCQuestList>().Setup(this.QuestGiverID, this.xui.playerUI.entityPlayer.entityId, (int)this.Quest.QuestClass.DifficultyTier, (byte)this.listIndex), false);
				}
				if (this.Quest.QuestTags.Test_AnySet(QuestEventManager.treasureTag) && GameSparksCollector.CollectGamePlayData)
				{
					GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.QuestAcceptedDistance, ((int)Vector3.Distance(this.Quest.Position, this.xui.Dialog.Respondent.position) / 50 * 50).ToString(), 1, true, GameSparksCollector.GSDataCollection.SessionUpdates);
				}
			}
			Dialog currentDialog = this.xui.Dialog.DialogWindowGroup.CurrentDialog;
			if (string.IsNullOrEmpty(currentDialog.CurrentStatement.NextStatementID) && string.IsNullOrEmpty(this.xui.Dialog.ReturnStatement))
			{
				this.xui.playerUI.windowManager.Close("dialog");
				return;
			}
			if (!string.IsNullOrEmpty(this.xui.Dialog.ReturnStatement))
			{
				currentDialog.CurrentStatement.NextStatementID = this.xui.Dialog.ReturnStatement;
				this.xui.Dialog.ReturnStatement = "";
			}
			currentDialog.CurrentStatement = currentDialog.GetStatement(currentDialog.CurrentStatement.NextStatementID);
			this.xui.Dialog.DialogWindowGroup.RefreshDialog();
			this.xui.Dialog.DialogWindowGroup.ShowResponseWindow(true);
		}
	}

	// Token: 0x04005F58 RID: 24408
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnAccept;

	// Token: 0x04005F59 RID: 24409
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnDecline;

	// Token: 0x04005F5A RID: 24410
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnAccept_Background;

	// Token: 0x04005F5B RID: 24411
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnDecline_Background;

	// Token: 0x04005F5C RID: 24412
	[PublicizedFrom(EAccessModifier.Private)]
	public bool questAccepted;

	// Token: 0x04005F5D RID: 24413
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<EntityNPC> OnDenied;

	// Token: 0x04005F5E RID: 24414
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F5F RID: 24415
	[PublicizedFrom(EAccessModifier.Private)]
	public int variation = -1;

	// Token: 0x04005F60 RID: 24416
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex = -1;

	// Token: 0x02000FF7 RID: 4087
	public enum OfferTypes
	{
		// Token: 0x04005F64 RID: 24420
		Item,
		// Token: 0x04005F65 RID: 24421
		Dialog
	}
}

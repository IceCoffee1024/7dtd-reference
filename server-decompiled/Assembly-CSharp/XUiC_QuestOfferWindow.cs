using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001006 RID: 4102
[Preserve]
public class XUiC_QuestOfferWindow : XUiController
{
	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x060080D8 RID: 32984 RVA: 0x0030F4CE File Offset: 0x0030D6CE
	// (set) Token: 0x060080D9 RID: 32985 RVA: 0x0030F4D6 File Offset: 0x0030D6D6
	public XUiC_QuestOfferWindow.OfferTypes OfferType
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x060080DA RID: 32986 RVA: 0x0030F4DF File Offset: 0x0030D6DF
	// (set) Token: 0x060080DB RID: 32987 RVA: 0x0030F4E7 File Offset: 0x0030D6E7
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

	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x060080DC RID: 32988 RVA: 0x0030F4F7 File Offset: 0x0030D6F7
	// (set) Token: 0x060080DD RID: 32989 RVA: 0x0030F4FF File Offset: 0x0030D6FF
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

	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x060080DE RID: 32990 RVA: 0x0030F50F File Offset: 0x0030D70F
	// (set) Token: 0x060080DF RID: 32991 RVA: 0x0030F517 File Offset: 0x0030D717
	public XUiC_ItemStack ItemStackController { get; set; }

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x060080E0 RID: 32992 RVA: 0x0030F520 File Offset: 0x0030D720
	// (set) Token: 0x060080E1 RID: 32993 RVA: 0x0030F528 File Offset: 0x0030D728
	public int QuestGiverID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060080E2 RID: 32994 RVA: 0x0030F534 File Offset: 0x0030D734
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

	// Token: 0x060080E3 RID: 32995 RVA: 0x0030F730 File Offset: 0x0030D930
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

	// Token: 0x060080E4 RID: 32996 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x060080E5 RID: 32997 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x060080E6 RID: 32998 RVA: 0x0030F818 File Offset: 0x0030DA18
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

	// Token: 0x060080E7 RID: 32999 RVA: 0x0030F950 File Offset: 0x0030DB50
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

	// Token: 0x060080E8 RID: 33000 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060080E9 RID: 33001 RVA: 0x0030F9B8 File Offset: 0x0030DBB8
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

	// Token: 0x060080EA RID: 33002 RVA: 0x0030FA1C File Offset: 0x0030DC1C
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

	// Token: 0x060080EB RID: 33003 RVA: 0x0030FAFC File Offset: 0x0030DCFC
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

	// Token: 0x04005F93 RID: 24467
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnAccept;

	// Token: 0x04005F94 RID: 24468
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnDecline;

	// Token: 0x04005F95 RID: 24469
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnAccept_Background;

	// Token: 0x04005F96 RID: 24470
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnDecline_Background;

	// Token: 0x04005F97 RID: 24471
	[PublicizedFrom(EAccessModifier.Private)]
	public bool questAccepted;

	// Token: 0x04005F98 RID: 24472
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<EntityNPC> OnDenied;

	// Token: 0x04005F99 RID: 24473
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F9A RID: 24474
	[PublicizedFrom(EAccessModifier.Private)]
	public int variation = -1;

	// Token: 0x04005F9B RID: 24475
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex = -1;

	// Token: 0x02001007 RID: 4103
	public enum OfferTypes
	{
		// Token: 0x04005F9F RID: 24479
		Item,
		// Token: 0x04005FA0 RID: 24480
		Dialog
	}
}

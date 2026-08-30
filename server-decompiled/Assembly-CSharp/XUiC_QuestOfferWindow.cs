using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FE7 RID: 4071
[Preserve]
public class XUiC_QuestOfferWindow : XUiController
{
	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x0600807A RID: 32890 RVA: 0x0030A9DA File Offset: 0x00308BDA
	// (set) Token: 0x0600807B RID: 32891 RVA: 0x0030A9E2 File Offset: 0x00308BE2
	public XUiC_QuestOfferWindow.OfferTypes OfferType
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x0600807C RID: 32892 RVA: 0x0030A9EB File Offset: 0x00308BEB
	// (set) Token: 0x0600807D RID: 32893 RVA: 0x0030A9F3 File Offset: 0x00308BF3
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

	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x0600807E RID: 32894 RVA: 0x0030AA03 File Offset: 0x00308C03
	// (set) Token: 0x0600807F RID: 32895 RVA: 0x0030AA0B File Offset: 0x00308C0B
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

	// Token: 0x17000EF7 RID: 3831
	// (get) Token: 0x06008080 RID: 32896 RVA: 0x0030AA1B File Offset: 0x00308C1B
	// (set) Token: 0x06008081 RID: 32897 RVA: 0x0030AA23 File Offset: 0x00308C23
	public XUiC_ItemStack ItemStackController { get; set; }

	// Token: 0x17000EF8 RID: 3832
	// (get) Token: 0x06008082 RID: 32898 RVA: 0x0030AA2C File Offset: 0x00308C2C
	// (set) Token: 0x06008083 RID: 32899 RVA: 0x0030AA34 File Offset: 0x00308C34
	public int QuestGiverID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06008084 RID: 32900 RVA: 0x0030AA40 File Offset: 0x00308C40
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

	// Token: 0x06008085 RID: 32901 RVA: 0x0030AC3C File Offset: 0x00308E3C
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

	// Token: 0x06008086 RID: 32902 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008087 RID: 32903 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x06008088 RID: 32904 RVA: 0x0030AD24 File Offset: 0x00308F24
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
				itemStack.count--;
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
		entityPlayer.QuestJournal.AddQuest(quest, true);
	}

	// Token: 0x06008089 RID: 32905 RVA: 0x0030AE4C File Offset: 0x0030904C
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

	// Token: 0x0600808A RID: 32906 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600808B RID: 32907 RVA: 0x0030AEB4 File Offset: 0x003090B4
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

	// Token: 0x0600808C RID: 32908 RVA: 0x0030AF18 File Offset: 0x00309118
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

	// Token: 0x0600808D RID: 32909 RVA: 0x0030AFF8 File Offset: 0x003091F8
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

	// Token: 0x04005F2F RID: 24367
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnAccept;

	// Token: 0x04005F30 RID: 24368
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnDecline;

	// Token: 0x04005F31 RID: 24369
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnAccept_Background;

	// Token: 0x04005F32 RID: 24370
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnDecline_Background;

	// Token: 0x04005F33 RID: 24371
	[PublicizedFrom(EAccessModifier.Private)]
	public bool questAccepted;

	// Token: 0x04005F34 RID: 24372
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<EntityNPC> OnDenied;

	// Token: 0x04005F35 RID: 24373
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F36 RID: 24374
	[PublicizedFrom(EAccessModifier.Private)]
	public int variation = -1;

	// Token: 0x04005F37 RID: 24375
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex = -1;

	// Token: 0x02000FE8 RID: 4072
	public enum OfferTypes
	{
		// Token: 0x04005F3B RID: 24379
		Item,
		// Token: 0x04005F3C RID: 24380
		Dialog
	}
}

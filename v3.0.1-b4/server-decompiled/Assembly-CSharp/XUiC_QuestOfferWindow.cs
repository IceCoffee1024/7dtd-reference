using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCE RID: 4046
[Preserve]
public class XUiC_QuestOfferWindow : XUiController
{
	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x06007F88 RID: 32648 RVA: 0x00306206 File Offset: 0x00304406
	// (set) Token: 0x06007F89 RID: 32649 RVA: 0x0030620E File Offset: 0x0030440E
	public XUiC_QuestOfferWindow.OfferTypes OfferType
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x06007F8A RID: 32650 RVA: 0x00306217 File Offset: 0x00304417
	// (set) Token: 0x06007F8B RID: 32651 RVA: 0x0030621F File Offset: 0x0030441F
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

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x06007F8C RID: 32652 RVA: 0x0030622F File Offset: 0x0030442F
	// (set) Token: 0x06007F8D RID: 32653 RVA: 0x00306237 File Offset: 0x00304437
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

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x06007F8E RID: 32654 RVA: 0x00306247 File Offset: 0x00304447
	// (set) Token: 0x06007F8F RID: 32655 RVA: 0x0030624F File Offset: 0x0030444F
	public XUiC_ItemStack ItemStackController { get; set; }

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x06007F90 RID: 32656 RVA: 0x00306258 File Offset: 0x00304458
	// (set) Token: 0x06007F91 RID: 32657 RVA: 0x00306260 File Offset: 0x00304460
	public int QuestGiverID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007F92 RID: 32658 RVA: 0x0030626C File Offset: 0x0030446C
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

	// Token: 0x06007F93 RID: 32659 RVA: 0x00306468 File Offset: 0x00304668
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

	// Token: 0x06007F94 RID: 32660 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007F95 RID: 32661 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnHover(XUiController _sender, bool _isOver)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007F96 RID: 32662 RVA: 0x00306550 File Offset: 0x00304750
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnPress(XUiController _sender, int _mouseButton)
	{
		Quest quest = this.Quest;
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

	// Token: 0x06007F97 RID: 32663 RVA: 0x00306650 File Offset: 0x00304850
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

	// Token: 0x06007F98 RID: 32664 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007F99 RID: 32665 RVA: 0x003066B8 File Offset: 0x003048B8
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

	// Token: 0x06007F9A RID: 32666 RVA: 0x0030671C File Offset: 0x0030491C
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

	// Token: 0x06007F9B RID: 32667 RVA: 0x003067FC File Offset: 0x003049FC
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
				if (entityTrader != null && entityTrader.activeQuests != null)
				{
					entityTrader.activeQuests.Remove(this.Quest);
				}
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
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

	// Token: 0x04005E68 RID: 24168
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnAccept;

	// Token: 0x04005E69 RID: 24169
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnDecline;

	// Token: 0x04005E6A RID: 24170
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnAccept_Background;

	// Token: 0x04005E6B RID: 24171
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnDecline_Background;

	// Token: 0x04005E6C RID: 24172
	[PublicizedFrom(EAccessModifier.Private)]
	public bool questAccepted;

	// Token: 0x04005E6D RID: 24173
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<EntityNPC> OnDenied;

	// Token: 0x04005E6E RID: 24174
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005E6F RID: 24175
	[PublicizedFrom(EAccessModifier.Private)]
	public int variation = -1;

	// Token: 0x04005E70 RID: 24176
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex = -1;

	// Token: 0x02000FCF RID: 4047
	public enum OfferTypes
	{
		// Token: 0x04005E74 RID: 24180
		Item,
		// Token: 0x04005E75 RID: 24181
		Dialog
	}
}

using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005C2 RID: 1474
[Preserve]
public class ItemActionQuest : ItemAction
{
	// Token: 0x0600302B RID: 12331 RVA: 0x0013409B File Offset: 0x0013229B
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionQuest.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x001340A4 File Offset: 0x001322A4
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("QuestGiven"))
		{
			this.QuestGiven = "";
		}
		else
		{
			this.QuestGiven = _props.Values["QuestGiven"];
		}
		if (!_props.Values.ContainsKey("Title"))
		{
			this.Title = "The title is impossible to read.";
		}
		else
		{
			this.Title = _props.Values["Title"];
		}
		if (!_props.Values.ContainsKey("Description"))
		{
			this.Description = "The description is impossible to read.";
			return;
		}
		this.Description = _props.Values["Description"];
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x00134156 File Offset: 0x00132356
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionQuest.MyInventoryData)_data).bQuestAccept = false;
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x0013416C File Offset: 0x0013236C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		if (EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			_actionData.lastUseTime = Time.time + 1f;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return;
		}
		_actionData.lastUseTime = Time.time;
		if (this.UseAnimation)
		{
			_actionData.invData.holdingEntity.RightArmAnimationUse = true;
			if (this.soundStart != null)
			{
				_actionData.invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
			}
			((ItemActionQuest.MyInventoryData)_actionData).bQuestAccept = true;
			return;
		}
		XUiController xuiController = LocalPlayerUI.GetUIForPlayer(_actionData.invData.holdingEntity as EntityPlayerLocal).xui.FindWindowGroupByName("toolbelt");
		XUiC_Toolbelt xuiC_Toolbelt = ((xuiController != null) ? xuiController.GetChildByType<XUiC_Toolbelt>() : null);
		XUiC_ItemStack xuiC_ItemStack;
		if (xuiC_Toolbelt != null && xuiC_Toolbelt.TryGetSlotControl(_actionData.invData.slotIdx, out xuiC_ItemStack))
		{
			this.ExecuteInstantAction(_actionData.invData.holdingEntity, _actionData.invData.itemStack, true, xuiC_ItemStack);
		}
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x001342C0 File Offset: 0x001324C0
	public override bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		if (this.soundStart != null)
		{
			ent.PlayOneShot(this.soundStart, false, false, false, null, 1f);
		}
		EntityPlayerLocal entityPlayerLocal = ent as EntityPlayerLocal;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		if (this.QuestGiven != "")
		{
			QuestClass quest = QuestClass.GetQuest(this.QuestGiven);
			if (quest != null)
			{
				Quest quest2 = entityPlayerLocal.QuestJournal.FindQuest(this.QuestGiven, -1);
				if (quest2 == null || (quest.Repeatable && !quest2.Active))
				{
					if (!quest.CanActivate())
					{
						GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("questunavailable", false, null), false, false, 0f);
						return false;
					}
					Quest quest3 = quest.CreateQuest();
					XUiC_QuestOfferWindow xuiC_QuestOfferWindow = XUiC_QuestOfferWindow.OpenQuestOfferWindow(uiforPlayer.xui, quest3, -1, XUiC_QuestOfferWindow.OfferTypes.Item, -1, null);
					xuiC_QuestOfferWindow.ItemStackController = stackController;
					xuiC_QuestOfferWindow.ItemStackController.QuestLock = true;
				}
				else
				{
					GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("questunavailable", false, null), false, false, 0f);
				}
			}
		}
		return true;
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x001343B0 File Offset: 0x001325B0
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionQuest.MyInventoryData myInventoryData = (ItemActionQuest.MyInventoryData)_actionData;
		return myInventoryData.bQuestAccept && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x00134408 File Offset: 0x00132608
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionQuest.MyInventoryData myInventoryData = (ItemActionQuest.MyInventoryData)_actionData;
		if (!myInventoryData.bQuestAccept || Time.time - myInventoryData.lastUseTime < AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast)
		{
			return;
		}
		myInventoryData.bQuestAccept = false;
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		if (this.QuestGiven != "")
		{
			QuestClass quest = QuestClass.GetQuest(this.QuestGiven);
			if (quest != null)
			{
				Quest quest2 = entityPlayerLocal.QuestJournal.FindQuest(this.QuestGiven, -1);
				if (quest2 == null || (quest.Repeatable && !quest2.Active))
				{
					Quest quest3 = quest.CreateQuest();
					XUiC_QuestOfferWindow xuiC_QuestOfferWindow = XUiC_QuestOfferWindow.OpenQuestOfferWindow(uiforPlayer.xui, quest3, -1, XUiC_QuestOfferWindow.OfferTypes.Item, -1, null);
					if (myInventoryData.invData.slotIdx >= 0 && myInventoryData.invData.slotIdx < uiforPlayer.entityPlayer.inventory.Length)
					{
						XUiController xuiController = uiforPlayer.xui.FindWindowGroupByName("toolbelt");
						XUiC_Toolbelt xuiC_Toolbelt = ((xuiController != null) ? xuiController.GetChildByType<XUiC_Toolbelt>() : null);
						XUiC_ItemStack xuiC_ItemStack;
						if (xuiC_Toolbelt != null && xuiC_Toolbelt.TryGetSlotControl(_actionData.invData.slotIdx, out xuiC_ItemStack))
						{
							xuiC_QuestOfferWindow.ItemStackController = xuiC_ItemStack;
							xuiC_QuestOfferWindow.ItemStackController.QuestLock = true;
							return;
						}
					}
				}
				else
				{
					GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("questunavailable", false, null), false, false, 0f);
				}
			}
		}
	}

	// Token: 0x04002511 RID: 9489
	public string QuestGiven;

	// Token: 0x04002512 RID: 9490
	public new string Title;

	// Token: 0x04002513 RID: 9491
	public new string Description;

	// Token: 0x020005C3 RID: 1475
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06003033 RID: 12339 RVA: 0x0012B224 File Offset: 0x00129424
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002514 RID: 9492
		public bool bQuestAccept;
	}
}

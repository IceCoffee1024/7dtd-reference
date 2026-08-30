using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005BC RID: 1468
[Preserve]
public class ItemActionQuest : ItemAction
{
	// Token: 0x06002FBA RID: 12218 RVA: 0x0013258F File Offset: 0x0013078F
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionQuest.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FBB RID: 12219 RVA: 0x00132598 File Offset: 0x00130798
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

	// Token: 0x06002FBC RID: 12220 RVA: 0x0013264A File Offset: 0x0013084A
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionQuest.MyInventoryData)_data).bQuestAccept = false;
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x00132660 File Offset: 0x00130860
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
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_actionData.invData.holdingEntity as EntityPlayerLocal);
		if (_actionData.invData.slotIdx < uiforPlayer.entityPlayer.inventory.PUBLIC_SLOTS)
		{
			XUiC_Toolbelt childByType = uiforPlayer.xui.FindWindowGroupByName("toolbelt").GetChildByType<XUiC_Toolbelt>();
			this.ExecuteInstantAction(_actionData.invData.holdingEntity, _actionData.invData.itemStack, true, childByType.GetSlotControl(_actionData.invData.slotIdx));
		}
	}

	// Token: 0x06002FBE RID: 12222 RVA: 0x001327C4 File Offset: 0x001309C4
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

	// Token: 0x06002FBF RID: 12223 RVA: 0x001328B4 File Offset: 0x00130AB4
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionQuest.MyInventoryData myInventoryData = (ItemActionQuest.MyInventoryData)_actionData;
		return myInventoryData.bQuestAccept && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x0013290C File Offset: 0x00130B0C
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
					if (myInventoryData.invData.slotIdx < uiforPlayer.entityPlayer.inventory.PUBLIC_SLOTS)
					{
						XUiC_Toolbelt childByType = uiforPlayer.xui.FindWindowGroupByName("toolbelt").GetChildByType<XUiC_Toolbelt>();
						xuiC_QuestOfferWindow.ItemStackController = childByType.GetSlotControl(myInventoryData.invData.slotIdx);
						xuiC_QuestOfferWindow.ItemStackController.QuestLock = true;
						return;
					}
				}
				else
				{
					GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("questunavailable", false, null), false, false, 0f);
				}
			}
		}
	}

	// Token: 0x040024E4 RID: 9444
	public string QuestGiven;

	// Token: 0x040024E5 RID: 9445
	public new string Title;

	// Token: 0x040024E6 RID: 9446
	public new string Description;

	// Token: 0x020005BD RID: 1469
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FC2 RID: 12226 RVA: 0x00129694 File Offset: 0x00127894
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024E7 RID: 9447
		public bool bQuestAccept;
	}
}

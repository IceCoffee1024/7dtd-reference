using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E3 RID: 1507
[Preserve]
public class ItemActionUseOther : ItemAction
{
	// Token: 0x06003101 RID: 12545 RVA: 0x0013E1E2 File Offset: 0x0013C3E2
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionUseOther.FeedInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x0013E1EC File Offset: 0x0013C3EC
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Consume"))
		{
			this.Consume = StringParsers.ParseBool(_props.Values["Consume"], 0, -1, true);
		}
		else
		{
			this.Consume = true;
		}
		if (!_props.Values.ContainsKey("Create_item"))
		{
			this.CreateItem = null;
			this.CreateItemCount = 0;
			return;
		}
		this.CreateItem = _props.Values["Create_item"];
		if (_props.Values.ContainsKey("Create_item_count"))
		{
			this.CreateItemCount = int.Parse(_props.Values["Create_item_count"]);
			return;
		}
		this.CreateItemCount = 1;
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x0013E2A8 File Offset: 0x0013C4A8
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_data;
		feedInventoryData.bFeedingStarted = false;
		feedInventoryData.TargetEntity = null;
		if (_data.invData.holdingEntity is EntityPlayerLocal)
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_data.invData.holdingEntity as EntityPlayerLocal);
			NGUIWindowManager nguiWindowManager = uiforPlayer.nguiWindowManager;
			XUiC_FocusedBlockHealth.SetData(uiforPlayer, null, 0f);
		}
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x0013E308 File Offset: 0x0013C508
	public override bool CanExecute(ItemActionData _actionData)
	{
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return false;
		}
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
		EntityAlive holdingEntity = feedInventoryData.invData.holdingEntity;
		int modelLayer = holdingEntity.GetModelLayer();
		holdingEntity.SetModelLayer(2, false, null);
		float num = 4f;
		feedInventoryData.ray = holdingEntity.GetLookRay();
		EntityAlive entityAlive = null;
		if (Voxel.Raycast(feedInventoryData.invData.world, feedInventoryData.ray, num, -538750989, 128, this.SphereRadius))
		{
			entityAlive = ItemActionUseOther.GetEntityFromHit(Voxel.voxelRayHitInfo) as EntityAlive;
		}
		if (entityAlive == null || !entityAlive.IsAlive() || !(entityAlive is EntityPlayer))
		{
			Voxel.Raycast(feedInventoryData.invData.world, feedInventoryData.ray, num, -538488845, 128, this.SphereRadius);
		}
		holdingEntity.SetModelLayer(modelLayer, false, null);
		if (feedInventoryData.TargetEntity == null)
		{
			feedInventoryData.TargetEntity = entityAlive;
		}
		_actionData.invData.holdingEntity.MinEventContext.Other = feedInventoryData.TargetEntity;
		_actionData.invData.holdingEntity.MinEventContext.ItemValue = _actionData.invData.itemValue;
		return base.CanExecute(_actionData);
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x0013E444 File Offset: 0x0013C644
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
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
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
		_actionData.lastUseTime = Time.time;
		feedInventoryData.bFeedingStarted = true;
		if (feedInventoryData.TargetEntity == null)
		{
			return;
		}
		if (feedInventoryData.invData.item.HasAnyTags(this.medicalItemTag) && feedInventoryData.TargetEntity as EntityPlayer == null)
		{
			feedInventoryData.bFeedingStarted = false;
			feedInventoryData.TargetEntity = null;
			return;
		}
		if (feedInventoryData.invData.item.HasAnyTags(this.medicalItemTag) && feedInventoryData.TargetEntity.HasAnyTags(this.noMedBuffsTag))
		{
			feedInventoryData.bFeedingStarted = false;
			feedInventoryData.TargetEntity = null;
			return;
		}
		holdingEntity.RightArmAnimationUse = true;
		if (this.soundStart != null)
		{
			holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
		}
		holdingEntity.MinEventContext.Other = feedInventoryData.TargetEntity;
		holdingEntity.MinEventContext.ItemValue = _actionData.invData.itemValue;
		holdingEntity.FireEvent(MinEventTypes.onSelfHealedOther, true);
		holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionEnd : MinEventTypes.onSelfSecondaryActionEnd, true);
		if (_actionData.invData.itemValue.ItemClass.HasAnyTags(this.stopBleed) && feedInventoryData.TargetEntity.entityType == EntityType.Player && feedInventoryData.TargetEntity.Buffs.HasBuff("buffInjuryBleeding"))
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			if (achievementManager != null)
			{
				achievementManager.SetAchievementStat(EnumAchievementDataStat.BleedOutStopped, 1);
			}
		}
		ItemAction.ExecuteBuffActions(this.getBuffActions(_actionData), holdingEntity.entityId, feedInventoryData.TargetEntity, false, EnumBodyPartHit.None, null);
		EntityPlayer entityPlayer = holdingEntity as EntityPlayer;
		if (this.Consume)
		{
			if (_actionData.invData.itemValue.MaxUseTimes > 0 && _actionData.invData.itemValue.UseTimes + 1f < (float)_actionData.invData.itemValue.MaxUseTimes)
			{
				ItemValue itemValue = _actionData.invData.itemValue;
				itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, feedInventoryData.invData.itemValue, 1f, holdingEntity, null, _actionData.invData.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
				feedInventoryData.invData.itemValue = itemValue;
				return;
			}
			holdingEntity.inventory.DecHoldingItem(1);
		}
		if (this.CreateItem != null && this.CreateItemCount > 0)
		{
			ItemStack itemStack = new ItemStack(ItemClass.GetItem(this.CreateItem, false), this.CreateItemCount);
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayer as EntityPlayerLocal);
			if (null != uiforPlayer && !uiforPlayer.xui.PlayerInventory.AddItem(itemStack))
			{
				holdingEntity.world.gameManager.ItemDropServer(itemStack, holdingEntity.GetPosition(), Vector3.zero, -1, 60f, false);
			}
		}
		feedInventoryData.bFeedingStarted = false;
		feedInventoryData.TargetEntity = null;
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x0013E780 File Offset: 0x0013C980
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
		return feedInventoryData.bFeedingStarted && Time.time - feedInventoryData.lastUseTime < this.Delay;
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x0013E7B3 File Offset: 0x0013C9B3
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x0013E7BC File Offset: 0x0013C9BC
	public static Entity GetEntityFromHit(WorldRayHitInfo hitInfo)
	{
		Transform hitRootTransform = GameUtils.GetHitRootTransform(hitInfo.tag, hitInfo.transform);
		if (hitRootTransform != null)
		{
			return hitRootTransform.GetComponent<Entity>();
		}
		return null;
	}

	// Token: 0x06003109 RID: 12553 RVA: 0x0013E7EC File Offset: 0x0013C9EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canShowOverlay(ItemActionData _actionData)
	{
		return this.isValidEntityToHeal((ItemActionUseOther.FeedInventoryData)_actionData);
	}

	// Token: 0x0600310A RID: 12554 RVA: 0x0013E7FA File Offset: 0x0013C9FA
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemActionData _actionData)
	{
		if (this.isValidEntityToHeal((ItemActionUseOther.FeedInventoryData)_actionData))
		{
			return ItemClass.EnumCrosshairType.Heal;
		}
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x0013E7EC File Offset: 0x0013C9EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isShowOverlay(ItemActionData _actionData)
	{
		return this.isValidEntityToHeal((ItemActionUseOther.FeedInventoryData)_actionData);
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x0013E810 File Offset: 0x0013CA10
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void getOverlayData(ItemActionData _actionData, out float _perc, out string _text)
	{
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
		if (!this.isValidEntityToHeal(feedInventoryData))
		{
			base.getOverlayData(_actionData, out _perc, out _text);
			return;
		}
		_perc = feedInventoryData.TargetEntity.Stats.Health.Value / feedInventoryData.TargetEntity.Stats.Health.Max;
		_text = string.Format("{0}/{1}", feedInventoryData.TargetEntity.Stats.Health.Value.ToCultureInvariantString(), feedInventoryData.TargetEntity.Stats.Health.Max.ToCultureInvariantString());
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x0013E8A4 File Offset: 0x0013CAA4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isValidEntityToHeal(ItemActionUseOther.FeedInventoryData _actionData)
	{
		return _actionData.TargetEntity != null;
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x0013E8B4 File Offset: 0x0013CAB4
	public override void OnHUD(ItemActionData _actionData, int _x, int _y)
	{
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
		if (feedInventoryData == null)
		{
			return;
		}
		if (!this.canShowOverlay(feedInventoryData))
		{
			XUiC_FocusedBlockHealth.SetData(LocalPlayerUI.GetUIForPrimaryPlayer(), null, 0f);
			return;
		}
		if (!(feedInventoryData.invData.holdingEntity is EntityPlayerLocal))
		{
			return;
		}
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer((EntityPlayerLocal)feedInventoryData.invData.holdingEntity);
		if (!this.isShowOverlay(feedInventoryData))
		{
			if (feedInventoryData.uiOpenedByMe && XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
			{
				XUiC_FocusedBlockHealth.SetData(uiforPlayer, null, 0f);
				feedInventoryData.uiOpenedByMe = false;
				return;
			}
		}
		else
		{
			if (!XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
			{
				feedInventoryData.uiOpenedByMe = true;
			}
			float num;
			string text;
			this.getOverlayData(feedInventoryData, out num, out text);
			XUiC_FocusedBlockHealth.SetData(uiforPlayer, text, num);
		}
	}

	// Token: 0x0400260D RID: 9741
	public new string CreateItem;

	// Token: 0x0400260E RID: 9742
	public int CreateItemCount;

	// Token: 0x0400260F RID: 9743
	public new bool Consume;

	// Token: 0x04002610 RID: 9744
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> noMedBuffsTag = FastTags<TagGroup.Global>.Parse("noMedBuffs");

	// Token: 0x04002611 RID: 9745
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> medicalItemTag = FastTags<TagGroup.Global>.Parse("medical");

	// Token: 0x04002612 RID: 9746
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> stopBleed = FastTags<TagGroup.Global>.Parse("stopsBleeding");

	// Token: 0x020005E4 RID: 1508
	public class FeedInventoryData : ItemActionAttackData
	{
		// Token: 0x06003110 RID: 12560 RVA: 0x0012A704 File Offset: 0x00128904
		public FeedInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002613 RID: 9747
		public bool bFeedingStarted;

		// Token: 0x04002614 RID: 9748
		public EntityAlive TargetEntity;

		// Token: 0x04002615 RID: 9749
		public Ray ray;
	}
}

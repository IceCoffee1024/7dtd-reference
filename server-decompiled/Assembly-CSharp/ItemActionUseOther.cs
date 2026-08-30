using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005DE RID: 1502
[Preserve]
public class ItemActionUseOther : ItemAction
{
	// Token: 0x060030D3 RID: 12499 RVA: 0x0013D172 File Offset: 0x0013B372
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionUseOther.FeedInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x0013D17C File Offset: 0x0013B37C
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

	// Token: 0x060030D5 RID: 12501 RVA: 0x0013D238 File Offset: 0x0013B438
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

	// Token: 0x060030D6 RID: 12502 RVA: 0x0013D298 File Offset: 0x0013B498
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

	// Token: 0x060030D7 RID: 12503 RVA: 0x0013D3D4 File Offset: 0x0013B5D4
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

	// Token: 0x060030D8 RID: 12504 RVA: 0x0013D710 File Offset: 0x0013B910
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
		return feedInventoryData.bFeedingStarted && Time.time - feedInventoryData.lastUseTime < this.Delay;
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x0013D743 File Offset: 0x0013B943
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionUseOther.FeedInventoryData feedInventoryData = (ItemActionUseOther.FeedInventoryData)_actionData;
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x0013D74C File Offset: 0x0013B94C
	public static Entity GetEntityFromHit(WorldRayHitInfo hitInfo)
	{
		Transform hitRootTransform = GameUtils.GetHitRootTransform(hitInfo.tag, hitInfo.transform);
		if (hitRootTransform != null)
		{
			return hitRootTransform.GetComponent<Entity>();
		}
		return null;
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x0013D77C File Offset: 0x0013B97C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canShowOverlay(ItemActionData _actionData)
	{
		return this.isValidEntityToHeal((ItemActionUseOther.FeedInventoryData)_actionData);
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x0013D78A File Offset: 0x0013B98A
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemActionData _actionData)
	{
		if (this.isValidEntityToHeal((ItemActionUseOther.FeedInventoryData)_actionData))
		{
			return ItemClass.EnumCrosshairType.Heal;
		}
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x0013D77C File Offset: 0x0013B97C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isShowOverlay(ItemActionData _actionData)
	{
		return this.isValidEntityToHeal((ItemActionUseOther.FeedInventoryData)_actionData);
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x0013D7A0 File Offset: 0x0013B9A0
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

	// Token: 0x060030DF RID: 12511 RVA: 0x0013D834 File Offset: 0x0013BA34
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isValidEntityToHeal(ItemActionUseOther.FeedInventoryData _actionData)
	{
		return _actionData.TargetEntity != null;
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x0013D844 File Offset: 0x0013BA44
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

	// Token: 0x040025D9 RID: 9689
	public new string CreateItem;

	// Token: 0x040025DA RID: 9690
	public int CreateItemCount;

	// Token: 0x040025DB RID: 9691
	public new bool Consume;

	// Token: 0x040025DC RID: 9692
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> noMedBuffsTag = FastTags<TagGroup.Global>.Parse("noMedBuffs");

	// Token: 0x040025DD RID: 9693
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> medicalItemTag = FastTags<TagGroup.Global>.Parse("medical");

	// Token: 0x040025DE RID: 9694
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> stopBleed = FastTags<TagGroup.Global>.Parse("stopsBleeding");

	// Token: 0x020005DF RID: 1503
	public class FeedInventoryData : ItemActionAttackData
	{
		// Token: 0x060030E2 RID: 12514 RVA: 0x00129694 File Offset: 0x00127894
		public FeedInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025DF RID: 9695
		public bool bFeedingStarted;

		// Token: 0x040025E0 RID: 9696
		public EntityAlive TargetEntity;

		// Token: 0x040025E1 RID: 9697
		public Ray ray;
	}
}

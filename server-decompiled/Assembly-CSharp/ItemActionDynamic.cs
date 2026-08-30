using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005A0 RID: 1440
[Preserve]
public class ItemActionDynamic : ItemAction
{
	// Token: 0x06002F37 RID: 12087 RVA: 0x0012C398 File Offset: 0x0012A598
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Classes.ContainsKey("ToolCategory"))
		{
			DynamicProperties dynamicProperties = _props.Classes["ToolCategory"];
			foreach (KeyValuePair<string, string> keyValuePair in dynamicProperties.Values)
			{
				this.ToolBonuses[keyValuePair.Key] = new ItemActionAttack.Bonuses(StringParsers.ParseFloat(keyValuePair.Value, 0, -1, NumberStyles.Any), dynamicProperties.Params1.ContainsKey(keyValuePair.Key) ? StringParsers.ParseFloat(dynamicProperties.Params1[keyValuePair.Key], 0, -1, NumberStyles.Any) : 2f);
			}
		}
		if (_props.Values.ContainsKey("Damage_type"))
		{
			this.DamageType = EnumUtils.Parse<EnumDamageTypes>(_props.Values["Damage_type"], false);
		}
		else if (_props.Values.ContainsKey("DamageType"))
		{
			this.DamageType = EnumUtils.Parse<EnumDamageTypes>(_props.Values["DamageType"], false);
		}
		else
		{
			this.DamageType = EnumDamageTypes.Bashing;
		}
		this.RangeDefault = 2f;
		_props.ParseFloat("Range", ref this.RangeDefault);
		this.UsePowerAttackAnimation = this.ActionIndex == 1;
		this.UsePowerAttackTriggers = this.ActionIndex == 1;
		if (_props.Values.ContainsKey("UsePowerAttackAnimation"))
		{
			this.UsePowerAttackAnimation = StringParsers.ParseBool(_props.Values["UsePowerAttackAnimation"], 0, -1, true);
		}
		if (_props.Values.ContainsKey("UsePowerAttackTriggers"))
		{
			this.UsePowerAttackTriggers = StringParsers.ParseBool(_props.Values["UsePowerAttackTriggers"], 0, -1, true);
		}
		if (_props.Values.ContainsKey("UseGrazingHits"))
		{
			this.UseGrazingHits = StringParsers.ParseBool(_props.Values["UseGrazingHits"], 0, -1, true);
		}
		else
		{
			this.UseGrazingHits = false;
		}
		if (_props.Values.ContainsKey("GrazeDamagePercentage"))
		{
			this.GrazeDamagePercentage = StringParsers.ParseFloat(_props.Values["GrazeDamagePercentage"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.GrazeDamagePercentage = 0.1f;
		}
		if (_props.Values.ContainsKey("GrazeStaminaPercentage"))
		{
			this.GrazeStaminaPercentage = StringParsers.ParseFloat(_props.Values["GrazeStaminaPercentage"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.GrazeStaminaPercentage = 0.01f;
		}
		if (_props.Values.ContainsKey("Sphere"))
		{
			this.SphereRadius = StringParsers.ParseFloat(_props.Values["Sphere"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.SphereRadius = 0f;
		}
		if (_props.Values.ContainsKey("GrazeStart"))
		{
			this.GrazeStart = StringParsers.ParseFloat(_props.Values["GrazeStart"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.GrazeStart = -0.15f;
		}
		if (_props.Values.ContainsKey("GrazeEnd"))
		{
			this.GrazeEnd = StringParsers.ParseFloat(_props.Values["GrazeEnd"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.GrazeEnd = 0.15f;
		}
		if (_props.Values.ContainsKey("IsVerticalSwing"))
		{
			this.IsVerticalSwing = StringParsers.ParseBool(_props.Values["IsVerticalSwing"], 0, -1, true);
		}
		else
		{
			this.IsVerticalSwing = false;
		}
		if (_props.Values.ContainsKey("IsHorizontalSwing"))
		{
			this.IsHorizontalSwing = StringParsers.ParseBool(_props.Values["IsHorizontalSwing"], 0, -1, true);
		}
		else
		{
			this.IsHorizontalSwing = false;
		}
		if (_props.Values.ContainsKey("InvertSwing"))
		{
			this.InvertSwing = StringParsers.ParseBool(_props.Values["InvertSwing"], 0, -1, true);
		}
		else
		{
			this.InvertSwing = false;
		}
		if (_props.Values.ContainsKey("SwingDegrees"))
		{
			this.SwingDegrees = StringParsers.ParseFloat(_props.Values["SwingDegrees"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.SwingDegrees = 65f;
		}
		if (_props.Values.ContainsKey("SwingAngle"))
		{
			this.SwingAngle = StringParsers.ParseFloat(_props.Values["SwingAngle"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.SwingAngle = 0f;
		}
		_props.ParseInt("EntityPenetrationCount", ref this.EntityPenetrationCount);
		this.harvestHitEffectOn = true;
		_props.ParseBool("HarvestHitEffectOn", ref this.harvestHitEffectOn);
		if (_props.Classes.ContainsKey("HitSounds"))
		{
			this.HitSoundOverrides = new Dictionary<string, string>();
			for (int i = 0; i < 10; i++)
			{
				string text = "Override" + i.ToString();
				if (_props.Classes["HitSounds"].Values.ContainsKey(text) && _props.Classes["HitSounds"].Params1.ContainsKey(text))
				{
					this.HitSoundOverrides[_props.Classes["HitSounds"].Values[text]] = _props.Classes["HitSounds"].Params1[text];
				}
			}
		}
		if (_props.Classes.ContainsKey("GrazeSounds"))
		{
			this.GrazeSoundOverrides = new Dictionary<string, string>();
			for (int j = 0; j < 10; j++)
			{
				string text2 = "Override" + j.ToString();
				if (_props.Classes["GrazeSounds"].Values.ContainsKey(text2) && _props.Classes["GrazeSounds"].Params1.ContainsKey(text2))
				{
					this.GrazeSoundOverrides[_props.Classes["GrazeSounds"].Values[text2]] = _props.Classes["GrazeSounds"].Params1[text2];
				}
			}
		}
		if (_props.Values.ContainsKey("HarvestLength"))
		{
			this.HarvestLength = StringParsers.ParseFloat(_props.Values["HarvestLength"], 0, -1, NumberStyles.Any);
		}
		this.hitmaskOverride = Voxel.ToHitMask(_props.GetString("Hitmask_override"));
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x0012CA24 File Offset: 0x0012AC24
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator harvestOnCompletion(ItemActionDynamicMelee.ItemActionDynamicMeleeData dmd, Action callback)
	{
		if (dmd != null)
		{
			while (!dmd.HasFinished)
			{
				yield return null;
			}
		}
		callback();
		yield break;
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x0012CA3A File Offset: 0x0012AC3A
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionDynamic.ItemActionDynamicData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool GrazeCast(ItemActionDynamic.ItemActionDynamicData _actionData, float normalizedClipTime = -1f)
	{
		return false;
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Raycast(ItemActionDynamic.ItemActionDynamicData _actionData)
	{
		return false;
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x0012CA44 File Offset: 0x0012AC44
	public override WorldRayHitInfo GetExecuteActionTarget(ItemActionData _actionData)
	{
		float sphereRadius = this.SphereRadius;
		ItemActionDynamic.ItemActionDynamicData itemActionDynamicData = (ItemActionDynamic.ItemActionDynamicData)_actionData;
		EntityAlive holdingEntity = itemActionDynamicData.invData.holdingEntity;
		if (!itemActionDynamicData.useExistingRay)
		{
			itemActionDynamicData.ray = holdingEntity.GetMeleeRay();
			if (holdingEntity.IsBreakingBlocks)
			{
				if (itemActionDynamicData.ray.direction.y < 0f)
				{
					itemActionDynamicData.ray.direction = new Vector3(itemActionDynamicData.ray.direction.x, 0f, itemActionDynamicData.ray.direction.z);
					ItemActionDynamic.ItemActionDynamicData itemActionDynamicData2 = itemActionDynamicData;
					itemActionDynamicData2.ray.origin = itemActionDynamicData2.ray.origin + new Vector3(0f, -0.7f, 0f);
				}
			}
			else if (holdingEntity.GetAttackTarget() != null)
			{
				Vector3 vector = holdingEntity.GetAttackTargetHitPosition() - itemActionDynamicData.ray.origin;
				itemActionDynamicData.ray = new Ray(itemActionDynamicData.ray.origin, vector);
			}
			ItemActionDynamic.ItemActionDynamicData itemActionDynamicData3 = itemActionDynamicData;
			itemActionDynamicData3.ray.origin = itemActionDynamicData3.ray.origin - sphereRadius * itemActionDynamicData.ray.direction;
			itemActionDynamicData.rayStartPos = itemActionDynamicData.ray.origin;
		}
		itemActionDynamicData.useExistingRay = false;
		this.lastModelLayer = holdingEntity.GetModelLayer();
		holdingEntity.SetModelLayer(2, false, null);
		ItemValue itemValue = itemActionDynamicData.invData.itemValue;
		ItemClass holdingItem = holdingEntity.inventory.holdingItem;
		FastTags<TagGroup.Global> fastTags = _actionData.ActionTags;
		fastTags |= ((holdingItem == null) ? ItemActionAttack.MeleeTag : holdingItem.ItemTags);
		this.Range = EffectManager.GetValue(PassiveEffects.MaxRange, itemValue, this.RangeDefault, holdingEntity, null, fastTags, true, true, true, true, true, 1, true, false);
		this.BlockRange = EffectManager.GetValue(PassiveEffects.BlockRange, itemValue, this.Range, holdingEntity, null, fastTags, true, true, true, true, true, 1, true, false);
		float num = Utils.FastMax(this.Range, this.BlockRange) + sphereRadius;
		num -= (itemActionDynamicData.ray.origin - itemActionDynamicData.rayStartPos).magnitude;
		if (holdingEntity is EntityEnemy && holdingEntity.IsBreakingBlocks)
		{
			Voxel.Raycast(itemActionDynamicData.invData.world, itemActionDynamicData.ray, num, 1073807360, (this.hitmaskOverride == 0) ? 128 : this.hitmaskOverride, 0.4f);
		}
		else
		{
			EntityAlive entityAlive = null;
			int num2 = -538767389;
			if (Voxel.Raycast(itemActionDynamicData.invData.world, itemActionDynamicData.ray, num, num2, (this.hitmaskOverride == 0) ? 128 : this.hitmaskOverride, this.SphereRadius))
			{
				entityAlive = ItemActionAttack.GetEntityFromHit(Voxel.voxelRayHitInfo) as EntityAlive;
			}
			if (entityAlive == null)
			{
				Voxel.Raycast(itemActionDynamicData.invData.world, itemActionDynamicData.ray, num, -538488845, (this.hitmaskOverride == 0) ? 128 : this.hitmaskOverride, this.SphereRadius);
			}
		}
		holdingEntity.SetModelLayer(this.lastModelLayer, false, null);
		return _actionData.GetUpdatedHitInfo();
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x0012CD44 File Offset: 0x0012AF44
	public WorldRayHitInfo[] GetExecuteActionGrazeTarget(ItemActionData _actionData, float normalizedClipTime = -1f)
	{
		List<WorldRayHitInfo> list = new List<WorldRayHitInfo>();
		normalizedClipTime = Mathf.Clamp((normalizedClipTime - this.GrazeStart) / (this.GrazeEnd - this.GrazeStart), 0f, 1f);
		float num = this.SphereRadius * 1.25f;
		if (num == 0f)
		{
			num = 0.15f;
		}
		float num2 = num;
		float num3 = -(this.SwingDegrees * 0.5f);
		float num4 = this.SwingDegrees * 0.5f;
		ItemActionDynamic.ItemActionDynamicData itemActionDynamicData = (ItemActionDynamic.ItemActionDynamicData)_actionData;
		EntityAlive holdingEntity = itemActionDynamicData.invData.holdingEntity;
		EntityPlayerLocal entityPlayerLocal = holdingEntity as EntityPlayerLocal;
		Ray meleeRay = itemActionDynamicData.invData.holdingEntity.GetMeleeRay();
		meleeRay.direction = Quaternion.AngleAxis(Mathf.Lerp(num4, num3, normalizedClipTime), (holdingEntity as EntityPlayerLocal).cameraTransform.right) * meleeRay.direction;
		if (this.SwingAngle != 0f)
		{
			meleeRay.direction = Quaternion.AngleAxis(this.SwingAngle, (holdingEntity as EntityPlayerLocal).cameraTransform.forward) * meleeRay.direction;
		}
		float num5 = EffectManager.GetValue(PassiveEffects.MaxRange, itemActionDynamicData.invData.itemValue, 2f, holdingEntity, null, itemActionDynamicData.ActionTags, true, true, true, true, true, 1, true, false) + num2;
		meleeRay.origin -= meleeRay.direction * num2;
		Vector3 vector = meleeRay.origin + meleeRay.direction * num5;
		if (itemActionDynamicData.lastWeaponHeadPosition == Vector3.zero)
		{
			itemActionDynamicData.lastWeaponHeadPosition = vector;
			return list.ToArray();
		}
		float num6 = Vector3.Distance(vector, itemActionDynamicData.lastWeaponHeadPosition);
		Vector3 normalized = (vector - itemActionDynamicData.lastWeaponHeadPosition).normalized;
		Vector3 direction = itemActionDynamicData.invData.holdingEntity.GetMeleeRay().direction;
		this.lastModelLayer = holdingEntity.GetModelLayer();
		holdingEntity.SetModelLayer(2, false, null);
		Entity entity = null;
		Ray meleeRay2 = itemActionDynamicData.invData.holdingEntity.GetMeleeRay();
		meleeRay2.origin -= meleeRay2.direction * this.SphereRadius;
		if (Voxel.Raycast(itemActionDynamicData.invData.world, meleeRay2, num5, -538750989, (this.hitmaskOverride == 0) ? 128 : this.hitmaskOverride, this.SphereRadius))
		{
			WorldRayHitInfo updatedHitInfo = _actionData.GetUpdatedHitInfo();
			if (updatedHitInfo.tag != null && updatedHitInfo.tag.StartsWith("E_"))
			{
				entity = ItemActionAttack.GetEntityFromHit(Voxel.voxelRayHitInfo) as EntityAlive;
				if (entity != null && !this.shouldIgnoreTarget(entity, _actionData.invData.holdingEntity, true))
				{
					itemActionDynamicData.alreadyHitEnts.Add(entity.entityId);
				}
			}
		}
		Debug.DrawLine(meleeRay2.origin, meleeRay2.origin + meleeRay2.direction * num5, Color.green, Time.deltaTime);
		float num7 = -num;
		while (num7 < num6)
		{
			num7 += num;
			num7 = Mathf.Clamp(num7, 0f, num6);
			meleeRay.direction = (itemActionDynamicData.lastWeaponHeadPosition + normalized * num7 - meleeRay.origin).normalized;
			meleeRay.origin = itemActionDynamicData.invData.holdingEntity.GetMeleeRay().origin - num2 * meleeRay.direction;
			meleeRay.origin + meleeRay.direction * (num5 + num2);
			EntityAlive entityAlive = null;
			Color color = Color.red;
			if (Voxel.Raycast(itemActionDynamicData.invData.world, meleeRay, num5, -538750989, (this.hitmaskOverride == 0) ? 128 : this.hitmaskOverride, num))
			{
				Vector3.Distance(Voxel.voxelRayHitInfo.hit.pos, meleeRay.origin);
				entityAlive = ItemActionAttack.GetEntityFromHit(Voxel.voxelRayHitInfo) as EntityAlive;
				if (entityAlive == null)
				{
					entityAlive = Voxel.voxelRayHitInfo.hitCollider.GetComponentInParent<EntityAlive>();
				}
				if (this.shouldIgnoreTarget(entityAlive, _actionData.invData.holdingEntity, true))
				{
					entityAlive = null;
				}
				if (entityAlive != null && entityAlive.IsAlive() && !itemActionDynamicData.alreadyHitEnts.Contains(entityAlive.entityId))
				{
					color = Color.green;
				}
				else
				{
					_actionData.invData.holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionGrazeMiss : MinEventTypes.onSelfSecondaryActionGrazeMiss, true);
				}
				if (entityAlive != null && entityAlive.IsAlive() && !(entityAlive is EntityVehicle) && entityPlayerLocal != null && (entityPlayerLocal.HitInfo.transform == null || !entityPlayerLocal.HitInfo.transform.IsChildOf(entityAlive.ModelTransform)) && Vector3.Angle(holdingEntity.transform.forward, entityAlive.transform.position - holdingEntity.transform.position) <= 30f)
				{
					entityPlayerLocal.MoveController.SetCameraSnapEntity(entityAlive, eCameraSnapMode.MeleeAttack);
				}
			}
			Debug.DrawLine(meleeRay.origin, meleeRay.origin + meleeRay.direction * num5, color, 2f);
			if (ItemActionDynamic.ShowDebugSwing)
			{
				ItemActionDynamic.DebugDisplayHits.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
				ItemActionDynamic.DebugDisplayHits[ItemActionDynamic.DebugDisplayHits.Count - 1].transform.position = meleeRay.origin + meleeRay.direction * (num2 + 0.2f) + meleeRay.direction * ((num5 - (num2 + 0.2f)) * 0.5f);
				ItemActionDynamic.DebugDisplayHits[ItemActionDynamic.DebugDisplayHits.Count - 1].transform.LookAt(meleeRay.origin);
				ItemActionDynamic.DebugDisplayHits[ItemActionDynamic.DebugDisplayHits.Count - 1].transform.localScale = Vector3.right * num + Vector3.up * num + Vector3.forward * (num5 - (num2 + 0.2f));
				ItemActionDynamic.DebugDisplayHits[ItemActionDynamic.DebugDisplayHits.Count - 1].layer = 2;
				ItemActionDynamic.DebugDisplayHits[ItemActionDynamic.DebugDisplayHits.Count - 1].transform.position -= Origin.position;
				ItemActionDynamic.DebugDisplayHits[ItemActionDynamic.DebugDisplayHits.Count - 1].GetComponent<MeshRenderer>().material.SetColor("_Color", color);
			}
			if (this.isValidTarget(entityAlive, _actionData.invData.holdingEntity, true) && (entity == null || entityAlive.entityId != entity.entityId) && !itemActionDynamicData.alreadyHitEnts.Contains(entityAlive.entityId))
			{
				list.Add(Voxel.voxelRayHitInfo.Clone());
				itemActionDynamicData.alreadyHitEnts.Add(entityAlive.entityId);
			}
		}
		holdingEntity.SetModelLayer(this.lastModelLayer, false, null);
		itemActionDynamicData.lastWeaponHeadPosition = vector;
		return list.ToArray();
	}

	// Token: 0x06002F41 RID: 12097 RVA: 0x0012D4A0 File Offset: 0x0012B6A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void hitTarget(ItemActionData _actionData, WorldRayHitInfo hitInfo, bool _isGrazingHit = false)
	{
		ItemActionDynamic.ItemActionDynamicData itemActionDynamicData = (ItemActionDynamic.ItemActionDynamicData)_actionData;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		this.tmpTag = _actionData.ActionTags;
		this.tmpTag |= ((holdingEntity.inventory.holdingItem == null) ? ItemActionAttack.MeleeTag : holdingEntity.inventory.holdingItem.ItemTags);
		this.tmpTag = this.tmpTag | holdingEntity.CurrentStanceTag | holdingEntity.CurrentMovementTag;
		if (!_isGrazingHit && _actionData.invData.itemValue.MaxUseTimes > 0)
		{
			_actionData.invData.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, _actionData.invData.itemValue, 1f, holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
			base.HandleItemBreak(_actionData);
		}
		_actionData.attackDetails.isCriticalHit = false;
		MinEventParams minEventContext = holdingEntity.MinEventContext;
		minEventContext.StartPosition = hitInfo.ray.origin;
		BlockValue blockValue = BlockValue.Air;
		if (hitInfo.tag != null)
		{
			if (GameUtils.IsBlockOrTerrain(hitInfo.tag))
			{
				minEventContext.Other = null;
				minEventContext.Position = hitInfo.hit.pos;
				PropValue propValue2;
				PropValue propValue = (propValue2 = hitInfo.hit.propValue);
				if (!propValue2.IsAir)
				{
					minEventContext.PropRef = hitInfo.hit.propData.PropRef;
					minEventContext.PropValue = propValue;
					BlockValue blockValue2 = propValue.blockValue;
					minEventContext.BlockValue = blockValue2;
					blockValue = blockValue2;
					itemActionDynamicData.alreadyHitProps.Add(hitInfo.hit.propData.PropRef);
				}
				else
				{
					BlockValue blockValue3;
					BlockValue blockValue2 = (blockValue3 = hitInfo.fmcHit.blockValue);
					if (!blockValue3.isair)
					{
						minEventContext.BlockValue = blockValue2;
						blockValue = blockValue2;
						minEventContext.PropValue = PropValue.AIR;
						itemActionDynamicData.alreadyHitBlocks.Add(hitInfo.fmcHit.blockPos);
					}
					else
					{
						minEventContext.BlockValue = BlockValue.Air;
						minEventContext.PropValue = PropValue.AIR;
						blockValue = BlockValue.Air;
					}
				}
				if (_isGrazingHit)
				{
					return;
				}
			}
			else if (hitInfo.tag.StartsWith("E_"))
			{
				minEventContext.Other = hitInfo.hitCollider.transform.GetComponentInParent<EntityAlive>();
				minEventContext.BlockValue = BlockValue.Air;
				minEventContext.PropValue = PropValue.AIR;
				blockValue = BlockValue.Air;
				minEventContext.Position = hitInfo.hit.pos;
			}
		}
		if (!_isGrazingHit)
		{
			holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionRayHit : MinEventTypes.onSelfSecondaryActionRayHit, true);
			_actionData.attackDetails.isCriticalHit = _actionData.indexInEntityOfAction == 1;
		}
		else
		{
			holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionGrazeHit : MinEventTypes.onSelfSecondaryActionGrazeHit, true);
			_actionData.attackDetails.isCriticalHit = false;
		}
		_actionData.attackDetails.WeaponTypeTag = ItemActionAttack.MeleeTag;
		if (blockValue.isair)
		{
			blockValue = ItemActionAttack.GetBlockHit(itemActionDynamicData.invData.world, hitInfo);
		}
		float num = this.GetDamageBlock(_actionData.invData.itemValue, blockValue, holdingEntity, _actionData.indexInEntityOfAction);
		float num2 = this.GetDamageEntity(_actionData.invData.itemValue, holdingEntity, _actionData.indexInEntityOfAction);
		if (_isGrazingHit)
		{
			num = 0f;
			num2 *= EffectManager.GetValue(PassiveEffects.GrazeDamageMultiplier, _actionData.invData.itemValue, this.GrazeDamagePercentage, holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
		}
		int num3 = 1;
		if (this.bUseParticleHarvesting && (this.particleHarvestingCategory == null || this.particleHarvestingCategory == this.item.MadeOfMaterial.id))
		{
			num3 |= 4;
		}
		if (!this.harvestHitEffectOn && itemActionDynamicData.IsHarvesting)
		{
			num3 |= 8;
		}
		bool flag;
		if (holdingEntity is EntityPlayer)
		{
			num2 *= ItemActionAttack.MeleeDamagePercent;
			flag = ItemActionAttack.MeleeDamagePercent == 0f;
		}
		else
		{
			num2 *= ItemActionAttack.EntityPlayerDamagePercent;
			flag = ItemActionAttack.EntityPlayerDamagePercent == 0f;
		}
		int entityId = holdingEntity.entityId;
		EnumDamageTypes damageType = this.DamageType;
		float num4 = num;
		float num5 = num2;
		float num6 = 1f;
		float num7 = 1f;
		float criticalChance = this.getCriticalChance(_actionData);
		float dismemberChance = ItemAction.GetDismemberChance(_actionData, hitInfo);
		string surfaceCategory = this.item.MadeOfMaterial.SurfaceCategory;
		DamageMultiplier damageMultiplier = new DamageMultiplier();
		List<string> list = new List<string>();
		ItemActionAttack.AttackHitInfo attackDetails = _actionData.attackDetails;
		int num8 = num3;
		int actionExp = this.ActionExp;
		float actionExpBonusMultiplier = this.ActionExpBonusMultiplier;
		ItemActionAttack itemActionAttack = null;
		Dictionary<string, ItemActionAttack.Bonuses> toolBonuses = this.ToolBonuses;
		ItemActionAttack.EnumAttackMode enumAttackMode = (itemActionDynamicData.IsHarvesting ? ItemActionAttack.EnumAttackMode.RealAndHarvesting : ItemActionAttack.EnumAttackMode.RealNoHarvesting);
		Dictionary<string, string> dictionary = (_isGrazingHit ? this.GrazeSoundOverrides : this.HitSoundOverrides);
		int num9 = -1;
		bool flag2 = flag;
		ItemActionAttack.Hit(hitInfo, entityId, damageType, num4, num5, num6, num7, criticalChance, dismemberChance, surfaceCategory, damageMultiplier, list, attackDetails, num8, actionExp, actionExpBonusMultiplier, itemActionAttack, toolBonuses, enumAttackMode, dictionary, num9, itemActionDynamicData.invData.itemStack.itemValue, _isGrazingHit, flag2, true, null);
		if (!_isGrazingHit)
		{
			ItemActionDynamicMelee.ItemActionDynamicMeleeData itemActionDynamicMeleeData = _actionData as ItemActionDynamicMelee.ItemActionDynamicMeleeData;
			if (_actionData.invData.itemValue.IsEmpty())
			{
				GameManager.Instance.StartCoroutine(this.harvestOnCompletion(itemActionDynamicMeleeData, delegate
				{
					GameUtils.HarvestOnAttack(_actionData, this.ToolBonuses);
				}));
				return;
			}
			GameUtils.HarvestOnAttack(_actionData, this.ToolBonuses);
		}
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x0012DA00 File Offset: 0x0012BC00
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getCriticalChance(ItemActionData _actionData)
	{
		return EffectManager.GetValue(PassiveEffects.CriticalChance, _actionData.invData.itemValue, 0f, _actionData.invData.holdingEntity, null, _actionData.ActionTags, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x0012DA44 File Offset: 0x0012BC44
	public float GetDamageEntity(ItemValue _itemValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
	{
		this.tmpTag = ((actionIndex == 0) ? ItemActionAttack.PrimaryTag : ItemActionAttack.SecondaryTag);
		this.tmpTag |= ((_itemValue.ItemClass == null) ? ItemActionAttack.MeleeTag : _itemValue.ItemClass.ItemTags);
		if (_holdingEntity != null)
		{
			this.tmpTag = this.tmpTag | _holdingEntity.CurrentStanceTag | _holdingEntity.CurrentMovementTag;
		}
		return EffectManager.GetValue(PassiveEffects.EntityDamage, _itemValue, 0f, _holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x0012DAD8 File Offset: 0x0012BCD8
	public float GetDamageBlock(ItemValue _itemValue, BlockValue _blockValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
	{
		this.tmpTag = ((actionIndex == 0) ? ItemActionAttack.PrimaryTag : ItemActionAttack.SecondaryTag);
		this.tmpTag |= ((_itemValue.ItemClass == null) ? ItemActionAttack.MeleeTag : _itemValue.ItemClass.ItemTags);
		if (_holdingEntity != null)
		{
			this.tmpTag = this.tmpTag | _holdingEntity.CurrentStanceTag | _holdingEntity.CurrentMovementTag;
		}
		this.tmpTag |= _blockValue.Block.Tags;
		float value = EffectManager.GetValue(PassiveEffects.BlockDamage, _itemValue, 0f, _holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
		return Utils.FastMin((float)_blockValue.Block.blockMaterial.MaxIncomingDamage, value);
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x0012DBA8 File Offset: 0x0012BDA8
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isHitValid(WorldRayHitInfo _hitInfo, ItemActionData _actionData, out EntityAlive _hitEntity)
	{
		_hitEntity = null;
		if (_hitInfo == null)
		{
			return false;
		}
		if (!_hitInfo.bHitValid)
		{
			return false;
		}
		ItemActionDynamic.ItemActionDynamicData itemActionDynamicData = (ItemActionDynamic.ItemActionDynamicData)_actionData;
		float sqrMagnitude = (_hitInfo.hit.pos - itemActionDynamicData.rayStartPos).sqrMagnitude;
		if (_hitInfo.tag != null && GameUtils.IsBlockOrTerrain(_hitInfo.tag) && sqrMagnitude > this.BlockRange * this.BlockRange)
		{
			return false;
		}
		if (_hitInfo.tag != null && _hitInfo.tag.StartsWith("E_"))
		{
			EntityAlive entityAlive = GameUtils.GetHitRootEntity(_hitInfo.tag, _hitInfo.transform) as EntityAlive;
			if (entityAlive == null)
			{
				return false;
			}
			if (this.shouldIgnoreTarget(entityAlive, _actionData.invData.holdingEntity, false))
			{
				return false;
			}
			if (sqrMagnitude > this.Range * this.Range)
			{
				return false;
			}
			_hitEntity = entityAlive;
		}
		return true;
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x0012DC7D File Offset: 0x0012BE7D
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isValidTarget(Entity _target, Entity _self, bool _deadInvalid = true)
	{
		return !this.shouldIgnoreTarget(_target, _self, _deadInvalid);
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x0012DC8C File Offset: 0x0012BE8C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldIgnoreTarget(Entity _target, Entity _self, bool _ignoreDead = true)
	{
		if (_target == null)
		{
			return true;
		}
		if (_ignoreDead && !_target.IsAlive())
		{
			return true;
		}
		if (_target.entityId == _self.entityId)
		{
			return true;
		}
		if (_target is EntityDrone)
		{
			return (_target as EntityDrone).isAlly(_self as EntityPlayer);
		}
		EntityPlayer entityPlayer = _self as EntityPlayer;
		EntityPlayer entityPlayer2 = _target as EntityPlayer;
		return entityPlayer != null && entityPlayer2 != null && !entityPlayer.FriendlyFireCheck(entityPlayer2);
	}

	// Token: 0x06002F48 RID: 12104 RVA: 0x0012DD08 File Offset: 0x0012BF08
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && !entityPlayerLocal.TPCameraCheckPassed)
		{
			return ItemClass.EnumCrosshairType.None;
		}
		if (this.isShowOverlay(_actionData))
		{
			return ItemClass.EnumCrosshairType.Damage;
		}
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x06002F49 RID: 12105 RVA: 0x0012DD40 File Offset: 0x0012BF40
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isShowOverlay(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal == null)
		{
			return false;
		}
		EntityAlive entityAlive = _actionData.attackDetails.entityHit as EntityAlive;
		if (entityAlive is EntityDrone && (entityPlayerLocal.PlayerUI.xui.Dialog.Respondent != null || (float)entityAlive.Health == entityAlive.Stats.Health.Max))
		{
			if (_actionData.uiOpenedByMe && XUiC_FocusedBlockHealth.IsWindowOpen(entityPlayerLocal.PlayerUI))
			{
				XUiC_FocusedBlockHealth.SetData(entityPlayerLocal.PlayerUI, null, 0f);
				_actionData.uiOpenedByMe = false;
			}
			return false;
		}
		if (!this.isShowOverlayInternal(_actionData))
		{
			return false;
		}
		WorldRayHitInfo hitInfo = _actionData.hitInfo;
		if (!hitInfo.bHitValid)
		{
			return false;
		}
		if (hitInfo.tag != null)
		{
			if (GameUtils.IsBlockOrTerrain(hitInfo.tag))
			{
				if (hitInfo.hit.distanceSq > this.BlockRange * this.BlockRange)
				{
					return false;
				}
				if (!hitInfo.hit.blockValue.Block.IsHealthShownInUI(hitInfo.hit, hitInfo.hit.blockValue))
				{
					return false;
				}
			}
			if (hitInfo.tag.StartsWith("E_") && hitInfo.hit.distanceSq > this.Range * this.Range)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002F4A RID: 12106 RVA: 0x0012DE94 File Offset: 0x0012C094
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isShowOverlayInternal(ItemActionData actionData)
	{
		WorldRayHitInfo executeActionTarget = this.GetExecuteActionTarget(actionData);
		if (executeActionTarget == null)
		{
			return false;
		}
		if (!executeActionTarget.bHitValid)
		{
			return false;
		}
		bool flag = actionData.attackDetails.entityHit is EntityDrone;
		if (actionData.attackDetails.itemsToDrop == null && !flag)
		{
			return false;
		}
		if (actionData.attackDetails.bBlockHit)
		{
			return actionData.attackDetails.raycastHitPosition == executeActionTarget.hit.blockPos;
		}
		Entity hitRootEntity = GameUtils.GetHitRootEntity(executeActionTarget.tag, executeActionTarget.transform);
		return hitRootEntity && hitRootEntity == actionData.attackDetails.entityHit;
	}

	// Token: 0x06002F4B RID: 12107 RVA: 0x0012DF34 File Offset: 0x0012C134
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void getOverlayData(ItemActionData actionData, out float _perc, out string _text)
	{
		float num = actionData.attackDetails.damageTotalOfTarget;
		float num2 = (float)actionData.attackDetails.damageMax;
		if (actionData.attackDetails.bBlockHit)
		{
			BlockValue block = actionData.invData.world.GetBlock(actionData.attackDetails.hitRef);
			num = (float)block.damage;
			num2 = (float)block.Block.GetShownMaxDamage();
		}
		else
		{
			EntityAlive entityAlive = actionData.attackDetails.entityHit as EntityAlive;
			if (entityAlive != null)
			{
				if (entityAlive is EntityDrone)
				{
					float num3 = (float)entityAlive.Health;
					float max = entityAlive.Stats.Health.Max;
					_perc = num3 / max;
					_text = string.Format("{0}/{1}", Utils.FastMax(0f, num3).ToCultureInvariantString("0"), max.ToCultureInvariantString());
					return;
				}
				num = (float)(-(float)entityAlive.DeathHealth);
				num2 = (float)EntityClass.list[entityAlive.entityClass].DeadBodyHitPoints;
			}
		}
		_perc = (num2 - num) / num2;
		_text = string.Format("{0}/{1}", Utils.FastMax(0f, num2 - num).ToCultureInvariantString("0"), num2.ToCultureInvariantString());
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x0012E05C File Offset: 0x0012C25C
	public override void OnHUD(ItemActionData actionData, int _x, int _y)
	{
		if (actionData == null)
		{
			return;
		}
		if (!this.canShowOverlay(actionData))
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = actionData.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		if (!this.isShowOverlay(actionData))
		{
			if (actionData.uiOpenedByMe && XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
			{
				XUiC_FocusedBlockHealth.SetData(uiforPlayer, null, 0f);
				actionData.uiOpenedByMe = false;
				return;
			}
		}
		else
		{
			if (!XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
			{
				actionData.uiOpenedByMe = true;
			}
			float num;
			string text;
			this.getOverlayData(actionData, out num, out text);
			if (num < 1f)
			{
				XUiC_FocusedBlockHealth.SetData(uiforPlayer, text, num);
			}
		}
	}

	// Token: 0x06002F4D RID: 12109 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canShowOverlay(ItemActionData actionData)
	{
		return true;
	}

	// Token: 0x04002495 RID: 9365
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumDamageTypes DamageType;

	// Token: 0x04002496 RID: 9366
	[PublicizedFrom(EAccessModifier.Protected)]
	public Dictionary<string, ItemActionAttack.Bonuses> ToolBonuses = new Dictionary<string, ItemActionAttack.Bonuses>();

	// Token: 0x04002497 RID: 9367
	[PublicizedFrom(EAccessModifier.Protected)]
	public Dictionary<string, string> HitSoundOverrides;

	// Token: 0x04002498 RID: 9368
	[PublicizedFrom(EAccessModifier.Protected)]
	public Dictionary<string, string> GrazeSoundOverrides;

	// Token: 0x04002499 RID: 9369
	[PublicizedFrom(EAccessModifier.Protected)]
	public int lastModelLayer;

	// Token: 0x0400249A RID: 9370
	[PublicizedFrom(EAccessModifier.Protected)]
	public float RangeDefault;

	// Token: 0x0400249B RID: 9371
	[PublicizedFrom(EAccessModifier.Protected)]
	public new float Range;

	// Token: 0x0400249C RID: 9372
	[PublicizedFrom(EAccessModifier.Protected)]
	public new float BlockRange;

	// Token: 0x0400249D RID: 9373
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool UsePowerAttackAnimation;

	// Token: 0x0400249E RID: 9374
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool UsePowerAttackTriggers;

	// Token: 0x0400249F RID: 9375
	[PublicizedFrom(EAccessModifier.Protected)]
	public int hitmaskOverride;

	// Token: 0x040024A0 RID: 9376
	public bool UseGrazingHits;

	// Token: 0x040024A1 RID: 9377
	public float GrazeStart;

	// Token: 0x040024A2 RID: 9378
	public float GrazeEnd;

	// Token: 0x040024A3 RID: 9379
	public float GrazeDamagePercentage;

	// Token: 0x040024A4 RID: 9380
	public float GrazeStaminaPercentage;

	// Token: 0x040024A5 RID: 9381
	public bool IsVerticalSwing;

	// Token: 0x040024A6 RID: 9382
	public bool IsHorizontalSwing;

	// Token: 0x040024A7 RID: 9383
	public bool InvertSwing;

	// Token: 0x040024A8 RID: 9384
	public float SwingDegrees;

	// Token: 0x040024A9 RID: 9385
	public float SwingAngle;

	// Token: 0x040024AA RID: 9386
	public int EntityPenetrationCount;

	// Token: 0x040024AB RID: 9387
	[PublicizedFrom(EAccessModifier.Private)]
	public bool harvestHitEffectOn;

	// Token: 0x040024AC RID: 9388
	public float HarvestLength = 0.3f;

	// Token: 0x040024AD RID: 9389
	public static bool ShowDebugSwing = false;

	// Token: 0x040024AE RID: 9390
	public static List<GameObject> DebugDisplayHits = new List<GameObject>();

	// Token: 0x040024AF RID: 9391
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tmpTag;

	// Token: 0x020005A1 RID: 1441
	public class ItemActionDynamicData : ItemActionAttackData
	{
		// Token: 0x06002F50 RID: 12112 RVA: 0x0012E120 File Offset: 0x0012C320
		public ItemActionDynamicData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
			this.alreadyHitEnts = new List<int>();
			this.alreadyHitBlocks = new List<Vector3i>();
			this.alreadyHitProps = new List<PropRef>();
			this.waterCollisionParticles.Init(_invData.holdingEntity.entityId, _invData.item.MadeOfMaterial.SurfaceCategory, "water", 16);
		}

		// Token: 0x040024B0 RID: 9392
		public Ray ray;

		// Token: 0x040024B1 RID: 9393
		public Vector3 rayStartPos;

		// Token: 0x040024B2 RID: 9394
		public bool useExistingRay;

		// Token: 0x040024B3 RID: 9395
		public bool IsHarvesting;

		// Token: 0x040024B4 RID: 9396
		public List<int> alreadyHitEnts;

		// Token: 0x040024B5 RID: 9397
		public List<Vector3i> alreadyHitBlocks;

		// Token: 0x040024B6 RID: 9398
		public List<PropRef> alreadyHitProps;

		// Token: 0x040024B7 RID: 9399
		public Vector3 lastWeaponHeadPosition = Vector3.zero;

		// Token: 0x040024B8 RID: 9400
		public Vector3 lastWeaponHeadPositionDebug = Vector3.zero;

		// Token: 0x040024B9 RID: 9401
		public float lastClipPercentage = -1f;

		// Token: 0x040024BA RID: 9402
		public float attackTime;

		// Token: 0x040024BB RID: 9403
		public CollisionParticleController waterCollisionParticles = new CollisionParticleController();
	}
}

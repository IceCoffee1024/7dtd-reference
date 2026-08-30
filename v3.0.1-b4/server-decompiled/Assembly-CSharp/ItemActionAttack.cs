using System;
using System.Collections.Generic;
using System.Globalization;
using GUI_2;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200058C RID: 1420
public abstract class ItemActionAttack : ItemAction
{
	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x0012728A File Offset: 0x0012548A
	public int Hitmask
	{
		get
		{
			return this.hitmaskOverride;
		}
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x00127292 File Offset: 0x00125492
	public ItemActionAttack()
	{
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x001272B8 File Offset: 0x001254B8
	public float GetDamageEntity(ItemValue _itemValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
	{
		this.tmpTag = ((actionIndex == 0) ? ItemActionAttack.PrimaryTag : ItemActionAttack.SecondaryTag);
		this.tmpTag |= ((_itemValue.ItemClass == null) ? ItemActionAttack.MeleeTag : _itemValue.ItemClass.ItemTags);
		if (_holdingEntity != null)
		{
			this.tmpTag |= _holdingEntity.CurrentStanceTag | _holdingEntity.CurrentMovementTag;
		}
		return EffectManager.GetValue(PassiveEffects.EntityDamage, _itemValue, this.damageEntity, _holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x00127350 File Offset: 0x00125550
	public float GetDamageBlock(ItemValue _itemValue, BlockValue _blockValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
	{
		this.tmpTag = ((actionIndex == 0) ? ItemActionAttack.PrimaryTag : ItemActionAttack.SecondaryTag);
		this.tmpTag |= ((_itemValue.ItemClass == null) ? ItemActionAttack.MeleeTag : _itemValue.ItemClass.ItemTags);
		if (_holdingEntity != null)
		{
			this.tmpTag |= _holdingEntity.CurrentStanceTag | _holdingEntity.CurrentMovementTag;
		}
		this.tmpTag |= _blockValue.Block.Tags;
		float value = EffectManager.GetValue(PassiveEffects.BlockDamage, _itemValue, this.damageBlock, _holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
		return Utils.FastMin((float)_blockValue.Block.blockMaterial.MaxIncomingDamage, value);
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x0012741E File Offset: 0x0012561E
	public DamageMultiplier GetDamageMultiplier()
	{
		return this.damageMultiplier;
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x0004E076 File Offset: 0x0004C276
	public Vector3 GetKickbackForce(Vector3 _shotDirection)
	{
		return Vector3.zero;
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReloadGun(ItemActionData _actionData)
	{
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanReload(ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void showGunFire()
	{
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canShowOverlay(ItemActionData actionData)
	{
		return true;
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x00127428 File Offset: 0x00125628
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isShowOverlay(ItemActionData actionData)
	{
		if (actionData.invData.holdingEntity as EntityPlayerLocal == null)
		{
			return false;
		}
		WorldRayHitInfo executeActionTarget = this.GetExecuteActionTarget(actionData);
		if (executeActionTarget == null || !executeActionTarget.bHitValid || actionData.attackDetails.itemsToDrop == null || Time.time - actionData.lastUseTime > 1.5f)
		{
			return false;
		}
		if (actionData.attackDetails.bBlockHit)
		{
			return actionData.attackDetails.raycastHitPosition == executeActionTarget.hit.blockPos;
		}
		if (!actionData.attackDetails.bBlockHit)
		{
			Entity hitRootEntity = GameUtils.GetHitRootEntity(executeActionTarget.tag, executeActionTarget.transform);
			if (hitRootEntity)
			{
				return hitRootEntity == actionData.attackDetails.entityHit;
			}
		}
		return false;
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x001274F0 File Offset: 0x001256F0
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
		_perc = (num2 - num) / num2;
		_text = string.Format("{0}/{1}", Utils.FastMax(0f, num2 - num).ToCultureInvariantString("0"), num2.ToCultureInvariantString());
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x00127588 File Offset: 0x00125788
	public override void OnHUD(ItemActionData _actionData, int _x, int _y)
	{
		ItemActionAttackData itemActionAttackData = _actionData as ItemActionAttackData;
		if (itemActionAttackData == null)
		{
			return;
		}
		if (!this.canShowOverlay(itemActionAttackData))
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = itemActionAttackData.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		if (!this.isShowOverlay(itemActionAttackData))
		{
			if (itemActionAttackData.uiOpenedByMe && XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
			{
				XUiC_FocusedBlockHealth.SetData(uiforPlayer, null, 0f);
				itemActionAttackData.uiOpenedByMe = false;
				return;
			}
		}
		else
		{
			if (!XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
			{
				itemActionAttackData.uiOpenedByMe = true;
			}
			float num;
			string text;
			this.getOverlayData(itemActionAttackData, out num, out text);
			XUiC_FocusedBlockHealth.SetData(uiforPlayer, text, num);
		}
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x0012761C File Offset: 0x0012581C
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_data.invData.holdingEntity as EntityPlayerLocal);
		if (uiforPlayer != null && _data.invData.holdingEntity is EntityPlayerLocal && _data is ItemActionAttackData && XUiC_FocusedBlockHealth.IsWindowOpen(uiforPlayer))
		{
			XUiC_FocusedBlockHealth.SetData(uiforPlayer, null, 0f);
			((ItemActionAttackData)_data).uiOpenedByMe = false;
		}
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x00010E62 File Offset: 0x0000F062
	public override RenderCubeType GetFocusType(ItemActionData _actionData)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x0012768C File Offset: 0x0012588C
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
		this.damageEntity = 0f;
		_props.ParseFloat("DamageEntity", ref this.damageEntity);
		this.damageBlock = 0f;
		_props.ParseFloat("DamageBlock", ref this.damageBlock);
		this.Range = 0f;
		_props.ParseFloat("Range", ref this.Range);
		this.BlockRange = this.Range;
		_props.ParseFloat("Block_range", ref this.BlockRange);
		this.SphereRadius = 0f;
		_props.ParseFloat("Sphere", ref this.SphereRadius);
		_props.ParseInt("Magazine_size", ref this.BulletsPerMagazine);
		if (_props.Values.ContainsKey("Magazine_items"))
		{
			string[] array = _props.Values["Magazine_items"].Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Trim();
			}
			this.UsesMagazines = true;
			this.MagazineItemNames = array;
		}
		else
		{
			this.UsesMagazines = false;
		}
		if (_props.Values.ContainsKey("Magazine_item_ray_counts"))
		{
			this.MagazineItemRayCount = new int[this.MagazineItemNames.Length];
			string[] array2 = _props.Values["Magazine_item_ray_counts"].Split(',', StringSplitOptions.None);
			int num = 0;
			while (num < array2.Length && num < this.MagazineItemRayCount.Length)
			{
				if (num < this.MagazineItemRayCount.Length)
				{
					this.MagazineItemRayCount[num] = int.Parse(array2[num]);
				}
				num++;
			}
		}
		if (_props.Values.ContainsKey("Magazine_item_ray_spreads"))
		{
			this.MagazineItemSpread = new float[this.MagazineItemNames.Length];
			string[] array3 = _props.Values["Magazine_item_ray_spreads"].Split(',', StringSplitOptions.None);
			int num2 = 0;
			while (num2 < array3.Length && num2 < this.MagazineItemSpread.Length)
			{
				this.MagazineItemSpread[num2] = StringParsers.ParseFloat(array3[num2], 0, -1, NumberStyles.Any);
				num2++;
			}
		}
		if (_props.Values.ContainsKey("Single_magazine_usage"))
		{
			this.AmmoIsPerMagazine = StringParsers.ParseBool(_props.Values["Single_magazine_usage"], 0, -1, true);
		}
		if (_props.Values.ContainsKey("Bullet_use_per_shot"))
		{
			this.BulletUsePerShot = StringParsers.ParseFloat(_props.Values["Bullet_use_per_shot"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.BulletUsePerShot = 1f;
		}
		if (_props.Values.ContainsKey("Rays_per_shot"))
		{
			this.RaysPerShot = int.Parse(_props.Values["Rays_per_shot"]);
		}
		if (_props.Values.ContainsKey("Rays_spread"))
		{
			this.RaysSpread = StringParsers.ParseFloat(_props.Values["Rays_spread"], 0, -1, NumberStyles.Any);
		}
		if (_props.Values.ContainsKey("Reload_time"))
		{
			this.reloadingTime = StringParsers.ParseFloat(_props.Values["Reload_time"], 0, -1, NumberStyles.Any);
		}
		if (_props.Values.ContainsKey("Sound_repeat"))
		{
			this.soundRepeat = _props.Values["Sound_repeat"];
		}
		else
		{
			this.soundRepeat = "";
		}
		if (_props.Values.ContainsKey("Sound_end"))
		{
			this.soundEnd = _props.Values["Sound_end"];
		}
		else
		{
			this.soundEnd = "";
		}
		if (_props.Values.ContainsKey("Sound_empty"))
		{
			this.soundEmpty = _props.Values["Sound_empty"];
		}
		else
		{
			this.soundEmpty = "";
		}
		if (_props.Values.ContainsKey("Sound_reload"))
		{
			this.soundReload = _props.Values["Sound_reload"];
		}
		else
		{
			this.soundReload = "";
		}
		_props.ParseStringFloatDictWithSubStringKey("Sound_impact_volume_scale", '|', _props.Values, ref this.soundImpactVolumeScale);
		_props.ParseString("Particles_muzzle_fire", ref this.particlesMuzzleFire);
		_props.ParseString("Particles_muzzle_fire_fpv", ref this.particlesMuzzleFireFpv);
		if (_props.Values.ContainsKey("Particles_muzzle_smoke"))
		{
			this.particlesMuzzleSmoke = _props.Values["Particles_muzzle_smoke"];
		}
		if (_props.Values.ContainsKey("Particles_muzzle_smoke_fpv"))
		{
			this.particlesMuzzleSmokeFpv = _props.Values["Particles_muzzle_smoke_fpv"];
		}
		if (_props.Values.ContainsKey("Infinite_ammo"))
		{
			this.InfiniteAmmo = StringParsers.ParseBool(_props.Values["Infinite_ammo"], 0, -1, true);
		}
		if (_props.Values.ContainsKey("Show_ammo_force"))
		{
			this.ForceShowAmmo = StringParsers.ParseBool(_props.Values["Show_ammo_force"], 0, -1, true);
		}
		if (_props.Values.ContainsKey("Damage_type"))
		{
			this.DamageType = EnumUtils.Parse<EnumDamageTypes>(_props.Values["Damage_type"], true);
		}
		else
		{
			this.DamageType = EnumDamageTypes.None;
		}
		this.damageMultiplier = new DamageMultiplier(this.Properties);
		this.hitmaskOverride = Voxel.ToHitMask(_props.GetString("Hitmask_override"));
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x00127C80 File Offset: 0x00125E80
	public virtual float GetIdealAIRange(ItemActionData _actionData)
	{
		return this.Range;
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x00127C88 File Offset: 0x00125E88
	public static Entity GetEntityFromHit(WorldRayHitInfo hitInfo)
	{
		return GameUtils.GetHitRootEntity(hitInfo.tag, hitInfo.transform);
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x00127C9C File Offset: 0x00125E9C
	public static void Hit(WorldRayHitInfo hitInfo, int _attackerEntityId, EnumDamageTypes _damageType, float _blockDamage, float _entityDamage, float _staminaDamageMultiplier, float _weaponCondition, float _criticalHitChanceOLD, float _dismemberChance, string _attackingDeviceMadeOf, DamageMultiplier _damageMultiplier, List<string> _buffActions, ItemActionAttack.AttackHitInfo _attackDetails, int _flags = 1, int _actionExp = 0, float _actionExpBonus = 0f, ItemActionAttack rangeCheckedAction = null, Dictionary<string, ItemActionAttack.Bonuses> _toolBonuses = null, ItemActionAttack.EnumAttackMode _attackMode = ItemActionAttack.EnumAttackMode.RealNoHarvesting, Dictionary<string, string> _hitSoundOverrides = null, int ownedEntityId = -1, ItemValue damagingItemValue = null, bool _isGrazingHit = false, bool _forceNoDamage = false, bool _canHitSpecialBodyParts = true, Dictionary<string, float> _soundImpactVolumeScale = null)
	{
		if (hitInfo == null || hitInfo.tag == null)
		{
			return;
		}
		World world = GameManager.Instance.World;
		bool flag = true;
		if (_attackMode == ItemActionAttack.EnumAttackMode.RealNoHarvestingOrEffects)
		{
			flag = false;
			_attackMode = ItemActionAttack.EnumAttackMode.RealNoHarvesting;
		}
		if (_attackDetails != null)
		{
			_attackDetails.itemsToDrop = null;
			_attackDetails.bBlockHit = false;
			_attackDetails.entityHit = null;
		}
		string text = null;
		string text2 = null;
		string text3 = null;
		float num = 1f;
		Color color = Color.white;
		bool flag2 = false;
		EntityAlive entityAlive = world.GetEntity(_attackerEntityId) as EntityAlive;
		bool flag3 = hitInfo.tag.StartsWith("E_");
		bool flag4 = false;
		if (entityAlive != null)
		{
			if (damagingItemValue == null)
			{
				damagingItemValue = entityAlive.inventory.holdingItemItemValue;
			}
			flag4 = damagingItemValue.Equals(entityAlive.inventory.holdingItemItemValue);
		}
		bool flag5 = true;
		if (GameUtils.IsBlockOrTerrain(hitInfo.tag))
		{
			if (ItemAction.ShowDebugDisplayHit)
			{
				Vector3 position = Camera.main.transform.position;
				DebugLines.Create(null, entityAlive.RootTransform, position + Origin.position, hitInfo.hit.pos, new Color(1f, 0.5f, 1f), new Color(1f, 0f, 1f), ItemAction.DebugDisplayHitSize * 2f, ItemAction.DebugDisplayHitSize, ItemAction.DebugDisplayHitTime);
			}
			ChunkCluster chunkCache = world.ChunkCache;
			if (chunkCache == null)
			{
				return;
			}
			BlockValueRef blockValueRef = BlockValueRef.Create(hitInfo);
			BlockValue blockValue = chunkCache.GetBlock(blockValueRef);
			if (blockValue.isair && hitInfo.hit.blockValue.Block.IsDistantDecoration && hitInfo.hit.blockValue.damage >= hitInfo.hit.blockValue.Block.MaxDamage - 1)
			{
				blockValue = hitInfo.hit.blockValue;
				world.SetBlockRPC(blockValueRef, blockValue);
			}
			Block block = blockValue.Block;
			if (block == null)
			{
				return;
			}
			if (blockValueRef.Type == BlockValueRefType.Block && blockValue.ischild)
			{
				blockValueRef = new BlockValueRef(block.multiBlockPos.GetParentPos(blockValueRef.BlockPosition, blockValue));
				blockValue = chunkCache.GetBlock(blockValueRef.BlockPosition);
				block = blockValue.Block;
				if (block == null)
				{
					return;
				}
			}
			if (blockValue.isair)
			{
				return;
			}
			float num2 = world.GetLandProtectionHardnessModifier(hitInfo.hit.blockPos, entityAlive, world.GetGameManager().GetPersistentLocalPlayer());
			if (World.SandboxUseTraderArea == TraderAreaStates.Default && world.IsWithinTraderArea(hitInfo.hit.blockPos))
			{
				num2 = 0f;
			}
			if (world.InBoundsForPlayersPercent(hitInfo.hit.blockPos.ToVector3CenterXZ()) < 0.5f)
			{
				num2 = 0f;
			}
			if (!block.blockMaterial.CanDestroy)
			{
				num2 = 0f;
			}
			if (num2 != 1f)
			{
				if (entityAlive && _attackMode != ItemActionAttack.EnumAttackMode.Simulate && entityAlive is EntityPlayer && !damagingItemValue.ItemClass.ignoreKeystoneSound && !damagingItemValue.ToBlockValue(false).Block.IgnoreKeystoneOverlay)
				{
					entityAlive.PlayOneShot("keystone_impact_overlay", false, false, false, null, 1f);
				}
				if (num2 < 1f)
				{
					flag2 = true;
				}
			}
			bool flag6 = (blockValue.isTerrain ? (ItemActionAttack.TerrainDamagePercent == 0f) : (ItemActionAttack.BlockDamagePercent == 0f));
			if (blockValueRef != _attackDetails.hitRef || num2 != _attackDetails.hardnessScale || blockValue.type != _attackDetails.blockBeingDamaged.type || (flag4 && damagingItemValue.SelectedAmmoTypeIndex != _attackDetails.ammoIndex))
			{
				float num3 = Mathf.Max(block.GetHardness(), 0.1f) * num2;
				float num4 = _blockDamage * ((_damageMultiplier != null) ? _damageMultiplier.Get(block.blockMaterial.DamageCategory) : 1f);
				if (entityAlive)
				{
					num4 *= entityAlive.GetBlockDamageScale(blockValue.isTerrain);
				}
				if (_toolBonuses != null && _toolBonuses.Count > 0)
				{
					num4 *= ItemActionAttack.calculateHarvestToolDamageBonus(_toolBonuses, block.itemsToDrop);
					_attackDetails.bHarvestTool = true;
				}
				_attackDetails.damagePerHit = ((!flag2 && !flag6) ? (num4 / num3) : 0f);
				_attackDetails.damage = 0f;
				_attackDetails.hardnessScale = num2;
				_attackDetails.hitRef = blockValueRef;
				_attackDetails.blockBeingDamaged = blockValue;
				if (flag4)
				{
					_attackDetails.ammoIndex = damagingItemValue.SelectedAmmoTypeIndex;
				}
			}
			_attackDetails.raycastHitPosition = hitInfo.hit.blockPos;
			Block block2 = hitInfo.fmcHit.blockValue.Block;
			num = world.GetLightBrightness(hitInfo.fmcHit.blockPos);
			color = block2.GetColorForSide(hitInfo.fmcHit.blockValue, hitInfo.fmcHit.blockFace);
			text = block2.GetParticleForSide(hitInfo.fmcHit.blockValue, hitInfo.fmcHit.blockFace);
			MaterialBlock materialForSide = block2.GetMaterialForSide(hitInfo.fmcHit.blockValue, hitInfo.fmcHit.blockFace);
			text2 = materialForSide.SurfaceCategory;
			float num5 = _attackDetails.damagePerHit * _staminaDamageMultiplier;
			if (entityAlive)
			{
				string text4 = materialForSide.DamageCategory ?? string.Empty;
				num5 = (float)((int)EffectManager.GetValue(PassiveEffects.DamageModifier, damagingItemValue, num5, entityAlive, null, FastTags<TagGroup.Global>.Parse(text4) | _attackDetails.WeaponTypeTag | hitInfo.fmcHit.blockValue.Block.Tags, true, true, true, true, true, 1, true, false));
			}
			num5 = ItemActionAttack.DegradationModifier(num5, _weaponCondition);
			num5 = ((!flag2 && !flag6 && !materialForSide.CheckDamageIgnore(damagingItemValue.ItemClass.ItemTags, entityAlive)) ? Utils.FastMax(1f, num5) : 0f);
			_attackDetails.damage += num5;
			_attackDetails.bKilled = false;
			_attackDetails.damageTotalOfTarget = (float)blockValue.damage + _attackDetails.damage;
			if (_attackDetails.damage > 0f)
			{
				int num6;
				if (_attackDetails.hitRef.Type == BlockValueRefType.Block)
				{
					Vector3i blockPosition = _attackDetails.hitRef.BlockPosition;
					Vector3 vector;
					Vector3 vector2;
					BlockFace blockFaceFromHitInfo = GameUtils.GetBlockFaceFromHitInfo(blockPosition, blockValue, hitInfo.hitCollider, hitInfo.hitTriangleIdx, out vector, out vector2);
					num6 = chunkCache.GetBlockFaceTexture(blockPosition, blockFaceFromHitInfo, 0);
				}
				else
				{
					num6 = 0;
				}
				int damage = blockValue.damage;
				bool flag7 = damage >= block.MaxDamage;
				int num7 = ((ownedEntityId != -1 && ownedEntityId != -2) ? ownedEntityId : _attackerEntityId);
				int num8 = ((_attackMode != ItemActionAttack.EnumAttackMode.Simulate) ? block.DamageBlock(world, _attackDetails.hitRef, blockValue, (int)_attackDetails.damage, num7, _attackDetails, _attackDetails.bHarvestTool, false) : 0);
				if (num8 == 0)
				{
					_attackDetails.damage = 0f;
				}
				else
				{
					_attackDetails.damage -= (float)(num8 - damage);
				}
				if (_attackMode != ItemActionAttack.EnumAttackMode.Simulate && flag && entityAlive is EntityPlayerLocal && num6 > 0 && block.MeshIndex == 0 && (float)num8 >= (float)block.MaxDamage * 1f)
				{
					ParticleEffect particleEffect = new ParticleEffect("paint_block", hitInfo.fmcHit.pos - Origin.position, Utils.BlockFaceToRotation(hitInfo.fmcHit.blockFace), num, color, null, null, 1f, "");
					particleEffect.opqueTextureId = (int)BlockTextureData.list[num6].TextureID;
					GameManager.Instance.SpawnParticleEffectClient(particleEffect, _attackerEntityId, false, false);
				}
				_attackDetails.damageGiven = ((!flag7) ? (num8 - damage) : 0);
				_attackDetails.damageMax = block.MaxDamage;
				_attackDetails.bKilled = !flag7 && num8 >= block.MaxDamage;
				_attackDetails.itemsToDrop = block.itemsToDrop;
				_attackDetails.bBlockHit = true;
				_attackDetails.materialCategory = block.blockMaterial.SurfaceCategory;
				if (entityAlive != null && _attackMode != ItemActionAttack.EnumAttackMode.Simulate)
				{
					entityAlive.MinEventContext.ItemValue = damagingItemValue;
					entityAlive.MinEventContext.BlockValue = blockValue;
					entityAlive.MinEventContext.Tags = block.Tags;
					if (_attackDetails.bKilled)
					{
						entityAlive.FireEvent(MinEventTypes.onSelfDestroyedBlock, flag4);
						entityAlive.NotifyDestroyedBlock(_attackDetails);
					}
					else
					{
						entityAlive.FireEvent(MinEventTypes.onSelfDamagedBlock, flag4);
					}
				}
			}
		}
		else if (flag3)
		{
			bool flag8 = ownedEntityId == -2;
			string text5;
			Entity entity = ItemActionAttack.FindHitEntityNoTagCheck(hitInfo, out text5);
			if (entity == null)
			{
				return;
			}
			if (!flag8 && entity.entityId == _attackerEntityId)
			{
				return;
			}
			if (!entity.CanDamageEntity(_attackerEntityId))
			{
				return;
			}
			EntityAlive entityAlive2 = entity as EntityAlive;
			Vector2 vector3 = Vector2.zero;
			MeshCollider meshCollider = Voxel.phyxRaycastHit.collider as MeshCollider;
			if (meshCollider != null && meshCollider.sharedMesh != null && meshCollider.sharedMesh.HasVertexAttribute(VertexAttribute.TexCoord0))
			{
				vector3 = Voxel.phyxRaycastHit.textureCoord;
			}
			DamageSourceEntity damageSourceEntity = new DamageSourceEntity(EnumDamageSource.External, _damageType, _attackerEntityId, hitInfo.ray.direction, hitInfo.transform.name, hitInfo.hit.pos, vector3);
			damageSourceEntity.AttackingItem = damagingItemValue;
			damageSourceEntity.DismemberChance = _dismemberChance;
			damageSourceEntity.CreatorEntityId = ownedEntityId;
			damageSourceEntity.canHitSpecialBodyParts = _canHitSpecialBodyParts;
			bool isCriticalHit = _attackDetails.isCriticalHit;
			int num9 = (int)_entityDamage;
			if (entityAlive != null && entityAlive2 != null)
			{
				FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.none;
				if (entityAlive2.Health > 0)
				{
					fastTags = FastTags<TagGroup.Global>.Parse(damageSourceEntity.GetEntityDamageEquipmentSlotGroup(entityAlive2).ToStringCached<EquipmentSlotGroups>());
				}
				num9 = (int)EffectManager.GetValue(PassiveEffects.DamageModifier, damagingItemValue, (float)num9, entityAlive, null, fastTags | _attackDetails.WeaponTypeTag | entityAlive2.EntityClass.Tags, true, true, true, true, true, 1, true, false);
				if (ItemActionAttack.HeadshotMultiplier > 0f && fastTags.Test_AnySet(ItemAction.headTag))
				{
					num9 = (int)EffectManager.GetValue(PassiveEffects.HeadshotDamageModifier, damagingItemValue, (float)num9, entityAlive, null, fastTags | _attackDetails.WeaponTypeTag | entityAlive2.EntityClass.Tags, true, true, true, true, true, 1, true, false);
					num9 = (int)(ItemActionAttack.HeadshotMultiplier * (float)num9);
				}
				num9 = (int)EffectManager.GetValue(PassiveEffects.InternalDamageModifier, damagingItemValue, (float)num9, entityAlive2, null, fastTags | damagingItemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false);
			}
			if (!entityAlive2 || entityAlive2.Health > 0)
			{
				num9 = Utils.FastMax(_forceNoDamage ? 0 : 1, ItemActionAttack.difficultyModifier(num9, world.GetEntity(_attackerEntityId), entity));
			}
			else if (_toolBonuses != null)
			{
				num9 = (int)((float)num9 * ItemActionAttack.calculateHarvestToolDamageBonus(_toolBonuses, EntityClass.list[entity.entityClass].itemsToDrop));
			}
			bool flag9 = entity.IsDead();
			int num10 = ((entityAlive2 != null) ? entityAlive2.DeathHealth : 0);
			if (_attackMode != ItemActionAttack.EnumAttackMode.Simulate)
			{
				if (entityAlive != null)
				{
					MinEventParams minEventContext = entityAlive.MinEventContext;
					minEventContext.Other = entityAlive2;
					minEventContext.ItemValue = damagingItemValue;
					minEventContext.StartPosition = hitInfo.ray.origin;
				}
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && (entityAlive as EntityPlayer == null || !entityAlive.isEntityRemote) && entity.isEntityRemote && rangeCheckedAction != null)
				{
					EntityPlayer entityPlayer = entity as EntityPlayer;
					if (entityPlayer != null)
					{
						flag5 = false;
						Ray lookRay = entityAlive.GetLookRay();
						lookRay.origin -= lookRay.direction * 0.15f;
						float num11 = Utils.FastMax(rangeCheckedAction.Range, rangeCheckedAction.BlockRange) * ItemActionAttack.attackRangeMultiplier;
						string text6 = null;
						List<string> list = _buffActions;
						if (list != null)
						{
							if (entityAlive2)
							{
								text6 = ((text5 != null) ? GameUtils.GetChildTransformPath(entity.transform, hitInfo.transform) : null);
							}
							else
							{
								list = null;
							}
						}
						if (entityAlive != null)
						{
							entityAlive.FireEvent(MinEventTypes.onSelfAttackedOther, flag4);
							if (entityAlive2 != null && entityAlive2.RecordedDamage.Strength > 0)
							{
								entityAlive.FireEvent(MinEventTypes.onSelfDamagedOther, flag4);
							}
						}
						if (!flag9 && entity.IsDead() && entityAlive != null)
						{
							entityAlive.FireEvent(MinEventTypes.onSelfKilledOther, flag4);
						}
						if (entityAlive2)
						{
							EquipmentSlots entityDamageEquipmentSlot = damageSourceEntity.GetEntityDamageEquipmentSlot(entityAlive2);
							text3 = entityAlive2.GetArmorImpactSound(entityDamageEquipmentSlot, _isGrazingHit, true);
							if (entityAlive2.RecordedDamage.ArmorDamage > entityAlive2.RecordedDamage.Strength)
							{
								text2 = entityAlive2.GetArmorMaterial(entityDamageEquipmentSlot, true);
							}
						}
						if (string.IsNullOrEmpty(text2))
						{
							text2 = EntityClass.list[entity.entityClass].Properties.GetValue("SurfaceCategory");
						}
						text = text2;
						num = entity.GetLightBrightness();
						string text7 = string.Format("impact_{0}_on_{1}", _attackingDeviceMadeOf, text);
						string text8 = ((text2 != null) ? string.Format("{0}hit{1}", _attackingDeviceMadeOf, text2) : null);
						if (_hitSoundOverrides != null && _hitSoundOverrides.ContainsKey(text2))
						{
							text8 = _hitSoundOverrides[text2];
						}
						ParticleEffect particleEffect2 = new ParticleEffect(text7, hitInfo.fmcHit.pos, Utils.BlockFaceToRotation(hitInfo.fmcHit.blockFace), num, color, text8, null, 1f, "");
						entityPlayer.ServerNetSendRangeCheckedDamage(lookRay.origin, num11, damageSourceEntity, num9, isCriticalHit, list, text6, particleEffect2);
					}
				}
				if (flag5)
				{
					int num12 = entity.DamageEntity(damageSourceEntity, num9, isCriticalHit, 1f);
					if (num12 != -1 && entityAlive)
					{
						MinEventParams minEventContext2 = entityAlive.MinEventContext;
						minEventContext2.Other = entityAlive2;
						minEventContext2.ItemValue = damagingItemValue;
						minEventContext2.StartPosition = hitInfo.ray.origin;
						if (ownedEntityId != -1)
						{
							damagingItemValue.FireEvent(MinEventTypes.onSelfAttackedOther, entityAlive.MinEventContext);
						}
						entityAlive.FireEvent(MinEventTypes.onSelfAttackedOther, flag4);
						if (entityAlive2 && entityAlive2.RecordedDamage.Strength > 0)
						{
							entityAlive.FireEvent(MinEventTypes.onSelfDamagedOther, flag4);
						}
					}
					if (!flag9 && entity.IsDead() && entityAlive)
					{
						entityAlive.FireEvent(MinEventTypes.onSelfKilledOther, flag4);
					}
					if (num12 != -1 && entityAlive2 && _buffActions != null && _buffActions.Count > 0)
					{
						for (int i = 0; i < _buffActions.Count; i++)
						{
							BuffClass buff = BuffManager.GetBuff(_buffActions[i]);
							if (buff != null)
							{
								float num13 = 1f;
								num13 = EffectManager.GetValue(PassiveEffects.BuffProcChance, null, num13, entityAlive, null, FastTags<TagGroup.Global>.Parse(buff.Name), true, true, true, true, true, 1, true, false);
								if (entityAlive2.rand.RandomFloat <= num13)
								{
									entityAlive2.Buffs.AddBuff(_buffActions[i], entityAlive.entityId, true, false, -1f);
								}
							}
						}
					}
				}
			}
			if (entityAlive2)
			{
				EquipmentSlots entityDamageEquipmentSlot2 = damageSourceEntity.GetEntityDamageEquipmentSlot(entityAlive2);
				text3 = entityAlive2.GetArmorImpactSound(entityDamageEquipmentSlot2, _isGrazingHit, true);
				if (entityAlive2.RecordedDamage.ArmorDamage > entityAlive2.RecordedDamage.Strength)
				{
					text2 = entityAlive2.GetArmorMaterial(entityDamageEquipmentSlot2, true);
				}
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = EntityClass.list[entity.entityClass].Properties.GetValue("SurfaceCategory");
			}
			text = text2;
			num = entity.GetLightBrightness();
			EntityPlayer entityPlayer2 = entityAlive as EntityPlayer;
			if (entityPlayer2)
			{
				if (flag9 && entity.IsDead() && entityAlive2 && entityAlive2.DeathHealth + num9 > -1 * EntityClass.list[entity.entityClass].DeadBodyHitPoints)
				{
					_attackDetails.damageTotalOfTarget = (float)(-1 * entityAlive2.DeathHealth);
					_attackDetails.damageGiven = num10 + Mathf.Min(EntityClass.list[entity.entityClass].DeadBodyHitPoints, Mathf.Abs(entityAlive2.DeathHealth));
					_attackDetails.damageMax = EntityClass.list[entity.entityClass].DeadBodyHitPoints;
					_attackDetails.bKilled = -1 * entityAlive2.DeathHealth >= EntityClass.list[entity.entityClass].DeadBodyHitPoints;
					_attackDetails.itemsToDrop = EntityClass.list[entity.entityClass].itemsToDrop;
					_attackDetails.entityHit = entity;
					_attackDetails.materialCategory = text2;
				}
				if (!flag9 && (entityAlive2.IsDead() || entityAlive2.Health <= 0) && EntityClass.list.ContainsKey(entity.entityClass))
				{
					if ((_flags & 2) > 0)
					{
						float value = EffectManager.GetValue(PassiveEffects.ElectricalTrapXP, entityPlayer2.inventory.holdingItemItemValue, 0f, entityPlayer2, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
						if (value > 0f)
						{
							entityPlayer2.AddKillXP(entityAlive2, damageSourceEntity.AttackingItem, value);
						}
					}
					else
					{
						entityPlayer2.AddKillXP(entityAlive2, damageSourceEntity.AttackingItem, 1f);
					}
				}
			}
			if (entity is EntityDrone)
			{
				_attackDetails.entityHit = entity;
			}
		}
		if ((_flags & 8) > 0)
		{
			flag = false;
		}
		if (flag5 && _attackMode != ItemActionAttack.EnumAttackMode.Simulate && flag && text != null && ((_attackDetails.bBlockHit && !_attackDetails.bKilled) || !_attackDetails.bBlockHit))
		{
			string text9 = string.Format("impact_{0}_on_{1}", _attackingDeviceMadeOf, text);
			if (_attackMode == ItemActionAttack.EnumAttackMode.RealAndHarvesting && (_flags & 4) > 0 && ParticleEffect.IsAvailable(text9 + "_harvest"))
			{
				text9 += "_harvest";
			}
			string text10 = text3;
			float num14 = 1f;
			if (string.IsNullOrEmpty(text10))
			{
				text10 = ((text2 != null) ? string.Format("{0}hit{1}", _attackingDeviceMadeOf, text2) : null);
				if (_hitSoundOverrides != null && _hitSoundOverrides.ContainsKey(text2))
				{
					text10 = _hitSoundOverrides[text2];
				}
			}
			float num15;
			if (_soundImpactVolumeScale != null && _soundImpactVolumeScale.TryGetValue(text2, out num15))
			{
				num14 = num15;
			}
			Block block3 = _attackDetails.blockBeingDamaged.Block;
			string text11 = ((block3 != null && !flag3) ? block3.SoundHitAdditional : "");
			world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(text9, hitInfo.fmcHit.pos, Utils.BlockFaceToRotation(hitInfo.fmcHit.blockFace), num, color, text10, null, num14, text11), _attackerEntityId, false, true);
		}
		if ((_flags & 1) > 0 && entityAlive != null && entityAlive.inventory != null)
		{
			entityAlive.inventory.CallOnToolbeltChangedInternal();
		}
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x00128E9C File Offset: 0x0012709C
	public static BlockValue GetBlockHit(World _world, WorldRayHitInfo hitInfo)
	{
		if (!GameUtils.IsBlockOrTerrain(hitInfo.tag))
		{
			return BlockValue.Air;
		}
		BlockValue blockValue = BlockValue.Air;
		Vector3i vector3i = hitInfo.hit.blockPos;
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return BlockValue.Air;
		}
		blockValue = chunkCache.GetBlock(vector3i);
		if (blockValue.isair && hitInfo.hit.blockValue.Block.IsDistantDecoration && hitInfo.hit.blockValue.damage >= hitInfo.hit.blockValue.Block.MaxDamage - 1)
		{
			blockValue = hitInfo.hit.blockValue;
		}
		if (blockValue.Block == null)
		{
			return BlockValue.Air;
		}
		if (blockValue.ischild)
		{
			vector3i = blockValue.Block.multiBlockPos.GetParentPos(vector3i, blockValue);
			blockValue = chunkCache.GetBlock(vector3i);
			if (blockValue.Block == null)
			{
				return BlockValue.Air;
			}
		}
		if (blockValue.Equals(BlockValue.Air))
		{
			return BlockValue.Air;
		}
		return blockValue;
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x00128F9C File Offset: 0x0012719C
	public static Entity FindHitEntity(WorldRayHitInfo hitInfo)
	{
		if (!hitInfo.tag.StartsWith("E_"))
		{
			return null;
		}
		string text;
		return ItemActionAttack.FindHitEntityNoTagCheck(hitInfo, out text);
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x00128FC8 File Offset: 0x001271C8
	public static Entity FindHitEntityNoTagCheck(WorldRayHitInfo hitInfo, out string bodyPartName)
	{
		Transform transform = hitInfo.transform;
		bodyPartName = null;
		if (hitInfo.tag.StartsWith("E_BP_"))
		{
			bodyPartName = hitInfo.tag.Substring("E_BP_".Length).ToLower();
			transform = RootTransformRefEntity.FindEntityUpwards(hitInfo.transform);
		}
		if (transform == null)
		{
			return null;
		}
		Entity entity = transform.GetComponent<Entity>();
		if (entity == null && hitInfo.tag.StartsWith("E_Vehicle"))
		{
			entity = GameUtils.GetHitRootEntity(hitInfo.tag, hitInfo.transform);
		}
		return entity;
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x0012905C File Offset: 0x0012725C
	[PublicizedFrom(EAccessModifier.Private)]
	public static float calculateHarvestToolDamageBonus(Dictionary<string, ItemActionAttack.Bonuses> _toolBonuses, Dictionary<EnumDropEvent, List<Block.SItemDropProb>> _harvestItems)
	{
		if (!_harvestItems.ContainsKey(EnumDropEvent.Harvest))
		{
			return 1f;
		}
		List<Block.SItemDropProb> list = _harvestItems[EnumDropEvent.Harvest];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].toolCategory != null && _toolBonuses.ContainsKey(list[i].toolCategory))
			{
				return _toolBonuses[list[i].toolCategory].Damage;
			}
		}
		return 1f;
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x001290D0 File Offset: 0x001272D0
	public static float DegradationModifier(float _strength, float _condition)
	{
		return Mathf.Lerp(_strength * 0.5f, _strength, (_condition < 0.5f) ? (_condition + 0.5f) : 1f);
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x001290F5 File Offset: 0x001272F5
	public static float StaminaModifier(float _stamina)
	{
		return _stamina;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x001290F8 File Offset: 0x001272F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static int difficultyModifier(int _strength, Entity _attacker, Entity _target)
	{
		if (_attacker == null || _target == null)
		{
			return _strength;
		}
		if (_attacker.IsClientControlled() && _target.IsClientControlled())
		{
			return _strength;
		}
		if (!_attacker.IsClientControlled() && !_target.IsClientControlled())
		{
			return _strength;
		}
		if (!_attacker.IsClientControlled())
		{
			_strength = Mathf.RoundToInt((float)_strength * ItemActionAttack.IncomingDamageModifier);
		}
		else
		{
			_strength = Mathf.RoundToInt((float)_strength * ItemActionAttack.EntityIncomingDamageModifier);
		}
		return _strength;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x00129166 File Offset: 0x00127366
	public virtual bool ShowAmmoInUI()
	{
		return this.BulletsPerMagazine > 0;
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x00129174 File Offset: 0x00127374
	public override void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
		float num;
		if (_itemValue.ItemClass.Actions[0] is ItemActionRanged)
		{
			num = (_itemValue.ItemClass.ItemTags.Test_AnySet(ItemActionAttack.MeleeTag) ? ItemActionAttack.MeleeDamagePercent : ItemActionAttack.RangedDamagePercent);
		}
		else
		{
			num = ItemActionAttack.MeleeDamagePercent;
		}
		float num2 = this.GetDamageEntity(_itemValue, _xui.playerUI.entityPlayer, _actionIndex) * num;
		float num3 = this.GetDamageBlock(_itemValue, BlockValue.Air, _xui.playerUI.entityPlayer, _actionIndex) * ItemActionAttack.BlockDamagePercent;
		if (num2 > 0f)
		{
			_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblEntDmg", false, null), num2.ToCultureInvariantString("0")));
		}
		if (num3 > 0f)
		{
			_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblBlkDmg", false, null), num3.ToCultureInvariantString("0")));
		}
		ItemAction.BuffActionStrings(this, _infoList);
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x0012925D File Offset: 0x0012745D
	public override bool HasRadial()
	{
		return this.MagazineItemNames != null && this.MagazineItemNames.Length != 0;
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x00129274 File Offset: 0x00127474
	public override void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
		_xuiRadialWindow.ResetRadialEntries();
		string[] magazineItemNames = _epl.inventory.GetHoldingGun().MagazineItemNames;
		int num = -1;
		for (int i = 0; i < magazineItemNames.Length; i++)
		{
			ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[i], false);
			if (itemClass != null && (!_epl.isHeadUnderwater || itemClass.UsableUnderwater))
			{
				int itemCount = _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
				bool flag = (int)_epl.inventory.holdingItemItemValue.SelectedAmmoTypeIndex == i;
				_xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(), (itemCount > 0) ? "ItemIconAtlas" : "ItemIconAtlasGreyscale", itemCount.ToString(), itemClass.GetLocalizedItemName(), flag);
				if (flag)
				{
					num = i;
				}
			}
		}
		_xuiRadialWindow.SetCommonData(UIUtils.GetButtonIconForAction(_epl.playerInput.Reload), new XUiC_Radial.CommandHandlerDelegate(this.handleRadialCommand), new ItemActionAttack.RadialContextItem((ItemActionRanged)_epl.inventory.GetHoldingGun()), num, false, new XUiC_Radial.RadialStillValidDelegate(this.radialValidTest));
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x00129370 File Offset: 0x00127570
	[PublicizedFrom(EAccessModifier.Private)]
	public bool radialValidTest(XUiC_Radial _sender, XUiC_Radial.RadialContextAbs _context)
	{
		ItemActionAttack.RadialContextItem radialContextItem = _context as ItemActionAttack.RadialContextItem;
		if (radialContextItem == null)
		{
			return false;
		}
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		ItemActionData itemActionData = entityPlayer.inventory.holdingItemData.actionData[0];
		return radialContextItem.RangedItemAction == entityPlayer.inventory.GetHoldingGun() && !radialContextItem.RangedItemAction.IsActionRunning(itemActionData);
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x001293D8 File Offset: 0x001275D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleRadialCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		ItemActionAttack.RadialContextItem radialContextItem = _context as ItemActionAttack.RadialContextItem;
		if (radialContextItem == null)
		{
			return;
		}
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		ItemActionData itemActionData = entityPlayer.inventory.holdingItemData.actionData[0];
		if (radialContextItem.RangedItemAction == entityPlayer.inventory.GetHoldingGun() && !radialContextItem.RangedItemAction.IsActionRunning(itemActionData))
		{
			radialContextItem.RangedItemAction.SwapSelectedAmmo(entityPlayer, _commandIndex);
		}
	}

	// Token: 0x040023F8 RID: 9208
	public const string PropSurfaceCategory = "SurfaceCategory";

	// Token: 0x040023F9 RID: 9209
	public static float attackRangeMultiplier = 1f;

	// Token: 0x040023FA RID: 9210
	public static float IncomingDamageModifier = 1f;

	// Token: 0x040023FB RID: 9211
	public static float EntityIncomingDamageModifier = 1f;

	// Token: 0x040023FC RID: 9212
	public static float RangedDamagePercent = 1f;

	// Token: 0x040023FD RID: 9213
	public static float MeleeDamagePercent = 1f;

	// Token: 0x040023FE RID: 9214
	public static float BlockDamagePercent = 1f;

	// Token: 0x040023FF RID: 9215
	public static float TerrainDamagePercent = 1f;

	// Token: 0x04002400 RID: 9216
	public static float HeadshotMultiplier = 2f;

	// Token: 0x04002401 RID: 9217
	public static float StaminaUsageMultiplier = 1f;

	// Token: 0x04002402 RID: 9218
	public static float EntityPlayerDamagePercent = 1f;

	// Token: 0x04002403 RID: 9219
	public static float EntityBlockDamagePercent = 1f;

	// Token: 0x04002404 RID: 9220
	public static float BMBlockDamagePercent = 1f;

	// Token: 0x04002405 RID: 9221
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundRepeat;

	// Token: 0x04002406 RID: 9222
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundEnd;

	// Token: 0x04002407 RID: 9223
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundEmpty;

	// Token: 0x04002408 RID: 9224
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundReload;

	// Token: 0x04002409 RID: 9225
	public Dictionary<string, float> soundImpactVolumeScale = new Dictionary<string, float>();

	// Token: 0x0400240A RID: 9226
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particlesMuzzleFire;

	// Token: 0x0400240B RID: 9227
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particlesMuzzleSmoke;

	// Token: 0x0400240C RID: 9228
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particlesMuzzleFireFpv;

	// Token: 0x0400240D RID: 9229
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particlesMuzzleSmokeFpv;

	// Token: 0x0400240E RID: 9230
	public bool UsesMagazines;

	// Token: 0x0400240F RID: 9231
	public string[] MagazineItemNames;

	// Token: 0x04002410 RID: 9232
	public int[] MagazineItemRayCount;

	// Token: 0x04002411 RID: 9233
	public float[] MagazineItemSpread;

	// Token: 0x04002412 RID: 9234
	public int BulletsPerMagazine;

	// Token: 0x04002413 RID: 9235
	public bool AmmoIsPerMagazine;

	// Token: 0x04002414 RID: 9236
	[PublicizedFrom(EAccessModifier.Protected)]
	public float BulletUsePerShot;

	// Token: 0x04002415 RID: 9237
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumDamageTypes DamageType;

	// Token: 0x04002416 RID: 9238
	[PublicizedFrom(EAccessModifier.Protected)]
	public float reloadingTime;

	// Token: 0x04002417 RID: 9239
	[PublicizedFrom(EAccessModifier.Protected)]
	public float damageEntity;

	// Token: 0x04002418 RID: 9240
	[PublicizedFrom(EAccessModifier.Protected)]
	public float damageBlock;

	// Token: 0x04002419 RID: 9241
	public new float BlockRange;

	// Token: 0x0400241A RID: 9242
	public new int RaysPerShot = 1;

	// Token: 0x0400241B RID: 9243
	public new float RaysSpread;

	// Token: 0x0400241C RID: 9244
	public ItemActionAttack.EnumAttackType Type;

	// Token: 0x0400241D RID: 9245
	public new bool InfiniteAmmo;

	// Token: 0x0400241E RID: 9246
	public bool ForceShowAmmo;

	// Token: 0x0400241F RID: 9247
	[PublicizedFrom(EAccessModifier.Protected)]
	public DamageMultiplier damageMultiplier;

	// Token: 0x04002420 RID: 9248
	[PublicizedFrom(EAccessModifier.Protected)]
	public int hitmaskOverride;

	// Token: 0x04002421 RID: 9249
	[PublicizedFrom(EAccessModifier.Protected)]
	public Dictionary<string, ItemActionAttack.Bonuses> ToolBonuses = new Dictionary<string, ItemActionAttack.Bonuses>();

	// Token: 0x04002422 RID: 9250
	public static FastTags<TagGroup.Global> ThrownTag = FastTags<TagGroup.Global>.Parse("thrown");

	// Token: 0x04002423 RID: 9251
	public static FastTags<TagGroup.Global> RangedTag = FastTags<TagGroup.Global>.Parse("ranged");

	// Token: 0x04002424 RID: 9252
	public static FastTags<TagGroup.Global> MeleeTag = FastTags<TagGroup.Global>.Parse("melee");

	// Token: 0x04002425 RID: 9253
	public static FastTags<TagGroup.Global> PrimaryTag = FastTags<TagGroup.Global>.Parse("primary");

	// Token: 0x04002426 RID: 9254
	public static FastTags<TagGroup.Global> SecondaryTag = FastTags<TagGroup.Global>.Parse("secondary");

	// Token: 0x04002427 RID: 9255
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tmpTag;

	// Token: 0x04002428 RID: 9256
	public const int cHitDefault = 1;

	// Token: 0x04002429 RID: 9257
	public const int cHitToolBeltNotify = 1;

	// Token: 0x0400242A RID: 9258
	public const int cHitElectricTrap = 2;

	// Token: 0x0400242B RID: 9259
	public const int cHitHarvestParticles = 4;

	// Token: 0x0400242C RID: 9260
	public const int cHitEffectOff = 8;

	// Token: 0x0200058D RID: 1421
	public enum EnumAttackType
	{
		// Token: 0x0400242E RID: 9262
		Melee,
		// Token: 0x0400242F RID: 9263
		Projectile,
		// Token: 0x04002430 RID: 9264
		Missile
	}

	// Token: 0x0200058E RID: 1422
	public struct Bonuses
	{
		// Token: 0x06002ED9 RID: 11993 RVA: 0x00129518 File Offset: 0x00127718
		public Bonuses(float _tool, float _damage)
		{
			this.Tool = _tool;
			this.Damage = _damage;
		}

		// Token: 0x04002431 RID: 9265
		public float Tool;

		// Token: 0x04002432 RID: 9266
		public float Damage;
	}

	// Token: 0x0200058F RID: 1423
	public class AttackHitInfo
	{
		// Token: 0x04002433 RID: 9267
		public Vector3i raycastHitPosition;

		// Token: 0x04002434 RID: 9268
		public BlockValueRef hitRef;

		// Token: 0x04002435 RID: 9269
		public BlockValue blockBeingDamaged;

		// Token: 0x04002436 RID: 9270
		public float damagePerHit;

		// Token: 0x04002437 RID: 9271
		public float damage;

		// Token: 0x04002438 RID: 9272
		public float hardnessScale;

		// Token: 0x04002439 RID: 9273
		public float damageTotalOfTarget;

		// Token: 0x0400243A RID: 9274
		public int damageGiven;

		// Token: 0x0400243B RID: 9275
		public int damageMax;

		// Token: 0x0400243C RID: 9276
		public Dictionary<EnumDropEvent, List<Block.SItemDropProb>> itemsToDrop;

		// Token: 0x0400243D RID: 9277
		public bool bKilled;

		// Token: 0x0400243E RID: 9278
		public bool bBlockHit;

		// Token: 0x0400243F RID: 9279
		public bool bHarvestTool;

		// Token: 0x04002440 RID: 9280
		public Entity entityHit;

		// Token: 0x04002441 RID: 9281
		public string materialCategory;

		// Token: 0x04002442 RID: 9282
		public bool isCriticalHit;

		// Token: 0x04002443 RID: 9283
		public FastTags<TagGroup.Global> WeaponTypeTag;

		// Token: 0x04002444 RID: 9284
		public byte ammoIndex;
	}

	// Token: 0x02000590 RID: 1424
	public enum EnumAttackMode
	{
		// Token: 0x04002446 RID: 9286
		RealNoHarvesting,
		// Token: 0x04002447 RID: 9287
		RealAndHarvesting,
		// Token: 0x04002448 RID: 9288
		RealNoHarvestingOrEffects,
		// Token: 0x04002449 RID: 9289
		Simulate
	}

	// Token: 0x02000591 RID: 1425
	[PublicizedFrom(EAccessModifier.Private)]
	public class RadialContextItem : XUiC_Radial.RadialContextAbs
	{
		// Token: 0x06002EDB RID: 11995 RVA: 0x00129528 File Offset: 0x00127728
		public RadialContextItem(ItemActionRanged _rangedItemAction)
		{
			this.RangedItemAction = _rangedItemAction;
		}

		// Token: 0x0400244A RID: 9290
		public readonly ItemActionRanged RangedItemAction;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public abstract class LegacyAvatarController : AvatarController
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0002A268 File Offset: 0x00028468
	public Transform HeldItemTransform
	{
		get
		{
			return this.rightHandItemTransform;
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002A270 File Offset: 0x00028470
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.entity is EntityPlayerLocal)
		{
			this.modelTransform = base.transform.Find("Camera");
		}
		else
		{
			this.modelTransform = EModelBase.FindModel(base.transform);
		}
		this.assignStates();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002A2BF File Offset: 0x000284BF
	public override Transform GetActiveModelRoot()
	{
		return this.modelTransform;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0002A2C8 File Offset: 0x000284C8
	public override void SetInRightHand(Transform _transform)
	{
		this.idleTime = 0f;
		if (_transform != null)
		{
			_transform.SetParent(this.rightHand, false);
		}
		this.rightHandItemTransform = _transform;
		this.rightHandAnimator = ((_transform != null) ? _transform.GetComponent<Animator>() : null);
		if (this.rightHandAnimator != null)
		{
			this.rightHandAnimator.logWarnings = false;
			this.rightHandAnimator.runtimeAnimatorController = GameManager.Instance.ThirdPersonWeaponAnimatorController;
		}
		if (this.rightHandItemTransform != null)
		{
			Utils.SetLayerRecursively(this.rightHandItemTransform.gameObject, 0);
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002A268 File Offset: 0x00028468
	public override Transform GetRightHandTransform()
	{
		return this.rightHandItemTransform;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002A363 File Offset: 0x00028563
	public override bool IsAnimationAttackPlaying()
	{
		return this.timeAttackAnimationPlaying > 0f;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002A374 File Offset: 0x00028574
	public override void StartAnimationAttack()
	{
		this.idleTime = 0f;
		this.isAttackImpact = false;
		this.timeAttackAnimationPlaying = 0.3f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		this._setTrigger(AvatarController.weaponFireHash, true);
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002A3CB File Offset: 0x000285CB
	public override void SetAttackImpact()
	{
		if (!this.isAttackImpact)
		{
			this.isAttackImpact = true;
			this.timeAttackAnimationPlaying = 0.1f;
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0002A3E7 File Offset: 0x000285E7
	public override bool IsAttackImpact()
	{
		return this.isAttackImpact;
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0002A3EF File Offset: 0x000285EF
	public override bool IsAnimationUsePlaying()
	{
		return this.timeUseAnimationPlaying > 0f;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002A400 File Offset: 0x00028600
	public override void StartAnimationUse()
	{
		this.idleTime = 0f;
		this.itemUseTicks = 3;
		this.timeUseAnimationPlaying = 0.3f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		this._setBool(AvatarController.itemUseHash, true, true);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002A458 File Offset: 0x00028658
	public override bool IsAnimationSpecialAttackPlaying()
	{
		return this.bSpecialAttackPlaying;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0002A460 File Offset: 0x00028660
	public override void StartAnimationSpecialAttack(bool _b, int _animType)
	{
		this.idleTime = 0f;
		this.bSpecialAttackPlaying = _b;
		if (_b)
		{
			this._resetTrigger(AvatarController.weaponFireHash, true);
			this._resetTrigger(AvatarController.weaponPreFireCancelHash, true);
			this._setTrigger(AvatarController.weaponPreFireHash, true);
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0002A49B File Offset: 0x0002869B
	public override bool IsAnimationSpecialAttack2Playing()
	{
		return this.timeSpecialAttack2Playing > 0.3f;
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0002A4AA File Offset: 0x000286AA
	public override void StartAnimationSpecialAttack2()
	{
		this.idleTime = 0f;
		this.timeSpecialAttack2Playing = 0.3f;
		this._resetTrigger(AvatarController.weaponFireHash, true);
		this._resetTrigger(AvatarController.weaponPreFireHash, true);
		this._setTrigger(AvatarController.weaponPreFireCancelHash, true);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002A4E6 File Offset: 0x000286E6
	public override bool IsAnimationHarvestingPlaying()
	{
		return this.timeHarestingAnimationPlaying > 0f;
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002A4F5 File Offset: 0x000286F5
	public override void StartAnimationHarvesting(float _length, bool _weaponFireTrigger)
	{
		this.timeHarestingAnimationPlaying = _length;
		this._setBool(AvatarController.harvestingHash, true, true);
		if (_weaponFireTrigger)
		{
			this._setTrigger(AvatarController.weaponFireHash, true);
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002A51C File Offset: 0x0002871C
	public override void StartAnimationFiring()
	{
		this.idleTime = 0f;
		this.timeAttackAnimationPlaying = 0.3f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		this._setTrigger(AvatarController.weaponFireHash, true);
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0002A56C File Offset: 0x0002876C
	public override void StartAnimationReloading()
	{
		this.idleTime = 0f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		float value = EffectManager.GetValue(PassiveEffects.ReloadSpeedMultiplier, this.entity.inventory.holdingItemItemValue, 1f, this.entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this._setBool(AvatarController.reloadHash, true, true);
		this._setFloat(AvatarController.reloadSpeedHash, value, true);
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0002A5F4 File Offset: 0x000287F4
	public override void StartAnimationHit(EnumBodyPartHit _bodyPart, int _dir, int _hitDamage, bool _criticalHit, int _movementState, float random, float _duration)
	{
		this.InternalStartAnimationHit(_bodyPart, _dir, _hitDamage, _criticalHit, _movementState, random, _duration);
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0002A608 File Offset: 0x00028808
	[PublicizedFrom(EAccessModifier.Protected)]
	public void InternalStartAnimationHit(EnumBodyPartHit _bodyPart, int _dir, int _hitDamage, bool _criticalHit, int _movementState, float random, float _duration)
	{
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!base.CheckHit(_duration))
		{
			this.SetDataFloat(AvatarController.DataTypes.HitDuration, _duration, true);
			return;
		}
		this.idleTime = 0f;
		if (this.anim != null)
		{
			this.movementStateOverride = _movementState;
			this._setInt(AvatarController.movementStateHash, _movementState, true);
			this._setBool(AvatarController.isCriticalHash, _criticalHit, true);
			this._setInt(AvatarController.hitDirectionHash, _dir, true);
			this._setInt(AvatarController.hitDamageHash, _hitDamage, true);
			this._setFloat(AvatarController.hitRandomValueHash, random, true);
			this._setInt(AvatarController.hitBodyPartHash, (int)_bodyPart.ToPrimary().LowerToUpperLimb(), true);
			this._setTrigger(AvatarController.hitTriggerHash, true);
			this.SetDataFloat(AvatarController.DataTypes.HitDuration, _duration, true);
		}
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0002A6E0 File Offset: 0x000288E0
	public override void SetVisible(bool _b)
	{
		this.m_bVisible = _b;
		Transform meshTransform = this.GetMeshTransform();
		if (meshTransform != null && meshTransform.gameObject.activeSelf != _b)
		{
			meshTransform.gameObject.SetActive(_b);
			if (_b)
			{
				this.SwitchModelAndView(this.modelName, this.bFPV, this.bMale);
			}
		}
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002A739 File Offset: 0x00028939
	public override void SetVehicleAnimation(int _animHash, int _pose)
	{
		if (this.anim)
		{
			this._setInt(_animHash, _pose, true);
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0002A754 File Offset: 0x00028954
	public override void SetAiming(bool _bEnable)
	{
		this.idleTime = 0f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.anim != null)
		{
			this._setBool(AvatarController.isAimingHash, _bEnable, true);
		}
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0002A7A8 File Offset: 0x000289A8
	public override void SetCrouching(bool _bEnable)
	{
		this.idleTime = 0f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.anim != null)
		{
			this._setBool(AvatarController.isCrouchingHash, _bEnable, true);
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0002A7FC File Offset: 0x000289FC
	public override bool IsAnimationDigRunning()
	{
		return AvatarController.digHash == this.baseStateInfo.tagHash;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0002A810 File Offset: 0x00028A10
	public override void StartAnimationJumping()
	{
		this.idleTime = 0f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.anim != null)
		{
			this._setBool(AvatarController.jumpHash, true, true);
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0002A864 File Offset: 0x00028A64
	public override void StartAnimationJump(AnimJumpMode jumpMode)
	{
		this.idleTime = 0f;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		this.isJumpStarted = true;
		if (this.anim != null)
		{
			if (jumpMode == AnimJumpMode.Start)
			{
				this._setTrigger(AvatarController.jumpStartHash, true);
				this._setBool(AvatarController.inAirHash, true, true);
				return;
			}
			this._setTrigger(AvatarController.jumpLandHash, true);
			this._setInt(AvatarController.jumpLandResponseHash, 0, true);
			this._setBool(AvatarController.inAirHash, false, true);
		}
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0002A8F5 File Offset: 0x00028AF5
	public override bool IsAnimationJumpRunning()
	{
		return this.isJumpStarted || AvatarController.jumpHash == this.baseStateInfo.tagHash;
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0002A918 File Offset: 0x00028B18
	public override bool IsAnimationWithMotionRunning()
	{
		int tagHash = this.baseStateInfo.tagHash;
		return tagHash == AvatarController.jumpHash || tagHash == AvatarController.moveHash;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0002A944 File Offset: 0x00028B44
	public override void SetSwim(bool _enable)
	{
		int num = -1;
		if (!_enable)
		{
			num = this.entity.GetWalkType();
		}
		else
		{
			this._setFloat(AvatarController.swimSelectHash, this.entity.rand.RandomFloat, true);
		}
		this.SetWalkType(num, true);
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0002A988 File Offset: 0x00028B88
	public override void StartDeathAnimation(EnumBodyPartHit _bodyPart, int _movementState, float random)
	{
		this.idleTime = 0f;
		this.isInDeathAnim = true;
		this.didDeathTransition = false;
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.anim != null)
		{
			this.movementStateOverride = _movementState;
			this._setInt(AvatarController.movementStateHash, _movementState, true);
			this._setBool(AvatarController.isAliveHash, false, true);
			this._setInt(AvatarController.hitBodyPartHash, (int)_bodyPart.ToPrimary().LowerToUpperLimb(), true);
			this._setFloat(AvatarController.hitRandomValueHash, random, true);
			this.SetFallAndGround(false, this.entity.onGround);
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002AA34 File Offset: 0x00028C34
	public override void SetRagdollEnabled(bool _b)
	{
		this.bIsRagdoll = _b;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002AA40 File Offset: 0x00028C40
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateLayerStateInfo()
	{
		if (this.anim != null)
		{
			this.baseStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
			if (this.anim.layerCount > 1)
			{
				this.currentWeaponHoldLayer = this.anim.GetCurrentAnimatorStateInfo(1);
			}
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0002AA90 File Offset: 0x00028C90
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateSpineRotation()
	{
		if (this.modelTransform.parent != null && !this.bIsRagdoll && !this.entity.IsDead())
		{
			if (this.bFPV)
			{
				this.spine3.transform.localEulerAngles = new Vector3(this.spine1.transform.localEulerAngles.x, this.spine1.transform.localEulerAngles.y, this.spine1.transform.localEulerAngles.z + 1f * this.entity.rotation.x);
				return;
			}
			float num = 1f * this.entity.rotation.x / 3f;
			if (Time.timeScale > 0.001f)
			{
				this.spine1.transform.localEulerAngles = new Vector3(this.spine1.transform.localEulerAngles.x, this.spine1.transform.localEulerAngles.y, this.spine1.transform.localEulerAngles.z + num);
				this.spine2.transform.localEulerAngles = new Vector3(this.spine2.transform.localEulerAngles.x, this.spine2.transform.localEulerAngles.y, this.spine2.transform.localEulerAngles.z + num);
				this.spine3.transform.localEulerAngles = new Vector3(this.spine3.transform.localEulerAngles.x, this.spine3.transform.localEulerAngles.y, this.spine3.transform.localEulerAngles.z + num);
				return;
			}
			this.spine1.transform.localEulerAngles = new Vector3(this.spine1.transform.localEulerAngles.x, this.spine1.transform.localEulerAngles.y, num);
			this.spine2.transform.localEulerAngles = new Vector3(this.spine2.transform.localEulerAngles.x, this.spine2.transform.localEulerAngles.y, num);
			this.spine3.transform.localEulerAngles = new Vector3(this.spine3.transform.localEulerAngles.x, this.spine3.transform.localEulerAngles.y, num);
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0002AD34 File Offset: 0x00028F34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (this.entity == null || this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.anim == null || !this.anim.enabled)
		{
			return;
		}
		this.updateLayerStateInfo();
		this.updateSpineRotation();
		if (this.entity.inventory.holdingItem.Actions[0] != null)
		{
			this.entity.inventory.holdingItem.Actions[0].UpdateNozzleParticlesPosAndRot(this.entity.inventory.holdingItemData.actionData[0]);
		}
		if (this.entity.inventory.holdingItem.Actions[1] != null)
		{
			this.entity.inventory.holdingItem.Actions[1].UpdateNozzleParticlesPosAndRot(this.entity.inventory.holdingItemData.actionData[1]);
		}
		int fullPathHash = this.baseStateInfo.fullPathHash;
		bool flag = this.anim.IsInTransition(0);
		if (!flag)
		{
			this.isJumpStarted = false;
			if (fullPathHash == this.jumpState || fullPathHash == this.fpvJumpState)
			{
				this._setBool(AvatarController.jumpHash, false, true);
			}
			if (this.anim.GetBool(AvatarController.reloadHash) && this.reloadStates.Contains(this.currentWeaponHoldLayer.fullPathHash))
			{
				this._setBool(AvatarController.reloadHash, false, true);
			}
		}
		if (this.anim.GetBool(AvatarController.itemUseHash))
		{
			int num = this.itemUseTicks - 1;
			this.itemUseTicks = num;
			if (num <= 0)
			{
				this._setBool(AvatarController.itemUseHash, false, true);
			}
		}
		if (this.isInDeathAnim)
		{
			if ((this.baseStateInfo.tagHash == AvatarController.deathHash || this.deathStates.Contains(fullPathHash)) && this.baseStateInfo.normalizedTime >= 1f && !flag)
			{
				this.isInDeathAnim = false;
				if (this.entity.HasDeathAnim)
				{
					EModelBase emodel = this.entity.emodel;
					DamageResponse damageResponse = DamageResponse.New(true);
					emodel.DoRagdoll(in damageResponse, EModelBase.RagdollMode.Default, 999999f);
				}
			}
			if (this.entity.HasDeathAnim && this.entity.RootMotion && this.entity.isCollidedHorizontally)
			{
				this.isInDeathAnim = false;
				EModelBase emodel2 = this.entity.emodel;
				DamageResponse damageResponse = DamageResponse.New(true);
				emodel2.DoRagdoll(in damageResponse, EModelBase.RagdollMode.Default, 999999f);
			}
		}
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002AFA4 File Offset: 0x000291A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setLayerWeights()
	{
		if (this.anim == null || this.anim.layerCount <= 1)
		{
			return;
		}
		if (this.entity.IsDead())
		{
			this.anim.SetLayerWeight(1, 1f);
			if (!this.bFPV)
			{
				this.anim.SetLayerWeight(2, 1f);
				return;
			}
		}
		else
		{
			if (this.entity.inventory.holdingItem.HoldType == 0)
			{
				this.anim.SetLayerWeight(1, 0f);
			}
			else
			{
				this.anim.SetLayerWeight(1, 1f);
			}
			if (!this.bFPV)
			{
				this.anim.SetLayerWeight(2, 1f);
			}
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0002B060 File Offset: 0x00029260
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (this.timeAttackAnimationPlaying > 0f)
		{
			this.timeAttackAnimationPlaying -= Time.deltaTime;
			if (this.timeAttackAnimationPlaying <= 0f)
			{
				this.isAttackImpact = true;
			}
		}
		if (this.timeUseAnimationPlaying > 0f)
		{
			this.timeUseAnimationPlaying -= Time.deltaTime;
		}
		if (this.timeHarestingAnimationPlaying > 0f)
		{
			this.timeHarestingAnimationPlaying -= Time.deltaTime;
			if (this.timeHarestingAnimationPlaying <= 0f && this.anim != null)
			{
				this._setBool(AvatarController.harvestingHash, false, true);
			}
		}
		if (this.timeSpecialAttack2Playing > 0f)
		{
			this.timeSpecialAttack2Playing -= Time.deltaTime;
		}
		if (!this.m_bVisible && (!this.entity || !this.entity.RootMotion || this.entity.isEntityRemote))
		{
			return;
		}
		if (this.bipedTransform == null || !this.bipedTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.anim == null || !this.anim.avatar.isValid || !this.anim.enabled)
		{
			return;
		}
		this.updateLayerStateInfo();
		this.setLayerWeights();
		int value = this.entity.inventory.holdingItem.HoldType.Value;
		this._setInt(AvatarController.weaponHoldTypeHash, value, true);
		float speedForward = this.entity.speedForward;
		float speedStrafe = this.entity.speedStrafe;
		float num = speedStrafe;
		if (num >= 1234f)
		{
			num = 0f;
		}
		this._setFloat(AvatarController.forwardHash, speedForward, false);
		this._setFloat(AvatarController.strafeHash, num, false);
		if (!this.entity.IsDead())
		{
			if (this.movementStateOverride != -1)
			{
				this._setInt(AvatarController.movementStateHash, this.movementStateOverride, true);
				this.movementStateOverride = -1;
			}
			else if (speedStrafe >= 1234f)
			{
				this._setInt(AvatarController.movementStateHash, 4, true);
			}
			else
			{
				float num2 = speedForward * speedForward + num * num;
				this._setInt(AvatarController.movementStateHash, (num2 > this.entity.moveSpeedAggro * this.entity.moveSpeedAggro) ? 3 : ((num2 > this.entity.moveSpeed * this.entity.moveSpeed) ? 2 : ((num2 > 0.001f) ? 1 : 0)), true);
			}
		}
		if (base.IsMoving(speedForward, speedStrafe))
		{
			this.idleTime = 0f;
			this._setBool(AvatarController.isMovingHash, true, true);
		}
		else
		{
			this._setBool(AvatarController.isMovingHash, false, true);
		}
		if (this.useIdle)
		{
			float num3 = this.idleTime - this.idleTimeSent;
			if (num3 * num3 > 0.25f)
			{
				this.idleTimeSent = this.idleTime;
				this._setFloat(AvatarController.idleTimeHash, this.idleTime, false);
			}
			this.idleTime += Time.deltaTime;
		}
		float num4;
		this.TryGetFloat(AvatarController.rotationPitchHash, out num4);
		float num5 = Mathf.Lerp(num4, this.entity.rotation.x, Time.deltaTime * 12f);
		this._setFloat(AvatarController.rotationPitchHash, num5, false);
		float num6;
		this.TryGetFloat(AvatarController.yLookHash, out num6);
		base.UpdateFloat(AvatarController.yLookHash, Mathf.Lerp(num6, -base.Entity.rotation.x / 90f, Time.deltaTime * 12f), false);
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0002B3CB File Offset: 0x000295CB
	public virtual Transform GetMeshTransform()
	{
		return this.bipedTransform;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002B3D3 File Offset: 0x000295D3
	public override void SetArchetypeStance(NPCInfo.StanceTypes stance)
	{
		if (this.anim != null && this.anim.avatar.isValid && this.anim.enabled)
		{
			this._setInt(AvatarController.archetypeStanceHash, (int)stance, true);
		}
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0002B40F File Offset: 0x0002960F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setTrigger(int _propertyHash, bool _netsync = true)
	{
		base._setTrigger(_propertyHash, _netsync);
		if (this.rightHandAnimator != null && this.rightHandAnimator.runtimeAnimatorController != null)
		{
			this.rightHandAnimator.SetTrigger(_propertyHash);
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002B448 File Offset: 0x00029648
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _resetTrigger(int _propertyHash, bool _netsync = true)
	{
		base._resetTrigger(_propertyHash, _netsync);
		if (this.rightHandAnimator != null && this.rightHandAnimator.runtimeAnimatorController != null && this.rightHandAnimator.GetBool(_propertyHash))
		{
			this.rightHandAnimator.ResetTrigger(_propertyHash);
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002B498 File Offset: 0x00029698
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setFloat(int _propertyHash, float _value, bool _netsync = true)
	{
		base._setFloat(_propertyHash, _value, _netsync);
		if (this.rightHandAnimator != null && this.rightHandAnimator.runtimeAnimatorController != null)
		{
			this.rightHandAnimator.SetFloat(_propertyHash, _value);
		}
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002B4D1 File Offset: 0x000296D1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setBool(int _propertyHash, bool _value, bool _netsync = true)
	{
		base._setBool(_propertyHash, _value, _netsync);
		if (this.rightHandAnimator != null && this.rightHandAnimator.runtimeAnimatorController != null)
		{
			this.rightHandAnimator.SetBool(_propertyHash, _value);
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002B50A File Offset: 0x0002970A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setInt(int _propertyHash, int _value, bool _netsync = true)
	{
		base._setInt(_propertyHash, _value, _netsync);
		if (this.rightHandAnimator != null && this.rightHandAnimator.runtimeAnimatorController != null)
		{
			this.rightHandAnimator.SetInteger(_propertyHash, _value);
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002B543 File Offset: 0x00029743
	[PublicizedFrom(EAccessModifier.Protected)]
	public LegacyAvatarController()
	{
	}

	// Token: 0x04000690 RID: 1680
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPitchUpdateSpeed = 12f;

	// Token: 0x04000691 RID: 1681
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int jumpState;

	// Token: 0x04000692 RID: 1682
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int fpvJumpState;

	// Token: 0x04000693 RID: 1683
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isJumpStarted;

	// Token: 0x04000694 RID: 1684
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public HashSet<int> reloadStates = new HashSet<int>();

	// Token: 0x04000695 RID: 1685
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public HashSet<int> deathStates = new HashSet<int>();

	// Token: 0x04000696 RID: 1686
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_bVisible;

	// Token: 0x04000697 RID: 1687
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bFPV;

	// Token: 0x04000698 RID: 1688
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bMale;

	// Token: 0x04000699 RID: 1689
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimatorStateInfo baseStateInfo;

	// Token: 0x0400069A RID: 1690
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimatorStateInfo currentWeaponHoldLayer;

	// Token: 0x0400069B RID: 1691
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimatorStateInfo currentUpperBodyState;

	// Token: 0x0400069C RID: 1692
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float lastAbsMotionX;

	// Token: 0x0400069D RID: 1693
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float lastAbsMotionZ;

	// Token: 0x0400069E RID: 1694
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float lastAbsMotion;

	// Token: 0x0400069F RID: 1695
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform bipedTransform;

	// Token: 0x040006A0 RID: 1696
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform modelTransform;

	// Token: 0x040006A1 RID: 1697
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform rightHand;

	// Token: 0x040006A2 RID: 1698
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform pelvis;

	// Token: 0x040006A3 RID: 1699
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform spine;

	// Token: 0x040006A4 RID: 1700
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform spine1;

	// Token: 0x040006A5 RID: 1701
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform spine2;

	// Token: 0x040006A6 RID: 1702
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform spine3;

	// Token: 0x040006A7 RID: 1703
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform head;

	// Token: 0x040006A8 RID: 1704
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform cameraNode;

	// Token: 0x040006A9 RID: 1705
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float timeAttackAnimationPlaying;

	// Token: 0x040006AA RID: 1706
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool isAttackImpact;

	// Token: 0x040006AB RID: 1707
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float timeUseAnimationPlaying;

	// Token: 0x040006AC RID: 1708
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float timeHarestingAnimationPlaying;

	// Token: 0x040006AD RID: 1709
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform rightHandItemTransform;

	// Token: 0x040006AE RID: 1710
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator rightHandAnimator;

	// Token: 0x040006AF RID: 1711
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bIsRagdoll;

	// Token: 0x040006B0 RID: 1712
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool useIdle = true;

	// Token: 0x040006B1 RID: 1713
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float idleTime;

	// Token: 0x040006B2 RID: 1714
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float idleTimeSent;

	// Token: 0x040006B3 RID: 1715
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int itemUseTicks;

	// Token: 0x040006B4 RID: 1716
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int movementStateOverride = -1;

	// Token: 0x040006B5 RID: 1717
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string modelName;

	// Token: 0x040006B6 RID: 1718
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool isInDeathAnim;

	// Token: 0x040006B7 RID: 1719
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool didDeathTransition;

	// Token: 0x040006B8 RID: 1720
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bSpecialAttackPlaying;

	// Token: 0x040006B9 RID: 1721
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float timeSpecialAttack2Playing;
}

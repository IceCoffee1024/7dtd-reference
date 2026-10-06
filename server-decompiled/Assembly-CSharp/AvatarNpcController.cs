using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020000D2 RID: 210
[Preserve]
public class AvatarNpcController : LegacyAvatarController
{
	// Token: 0x06000519 RID: 1305 RVA: 0x00022B8C File Offset: 0x00020D8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void assignStates()
	{
		this.jumpState = Animator.StringToHash("Base Layer.Jump");
		this.fpvJumpState = Animator.StringToHash("Base Layer.FPVFemaleJump");
		this.deathStates = new HashSet<int>();
		AvatarCharacterController.GetThirdPersonDeathStates(this.deathStates);
		this.reloadStates = new HashSet<int>();
		AvatarCharacterController.GetThirdPersonReloadStates(this.reloadStates);
		this.hitStates = new HashSet<int>();
		AvatarCharacterController.GetThirdPersonHitStates(this.hitStates);
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00022BFC File Offset: 0x00020DFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void assignParts(bool _bFPV)
	{
		this.pelvis = this.bipedTransform.FindInChilds("Hips", false);
		this.spine = this.pelvis.Find("LowerBack");
		this.spine1 = this.spine.Find("Spine");
		this.spine2 = this.spine1.Find("Spine1");
		this.spine3 = this.spine2.Find("Neck");
		this.head = this.spine3.Find("Head");
		this.cameraNode = this.head.Find("CameraNode");
		this.cameraRootTransform = this.spine3;
		this.rightHand = this.bipedTransform.FindInChilds(this.entity.GetRightHandTransformName(), false);
		this.meshTransform = this.bipedTransform.FindInChilds("body", false);
		if (this.meshTransform == null)
		{
			this.meshTransform = this.bipedTransform.FindInChilds("TraderBob", false);
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00022D09 File Offset: 0x00020F09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		if (this.anim == null && this.m_bVisible)
		{
			base.SetAnimator(base.GetComponentInChildren<Animator>());
		}
		base.Update();
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00022D34 File Offset: 0x00020F34
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setLayerWeights()
	{
		if (this.anim != null)
		{
			if (this.entity.IsDead())
			{
				this.anim.SetLayerWeight(1, 0f);
				this.anim.SetLayerWeight(2, 0f);
				this.anim.SetLayerWeight(3, 0f);
				return;
			}
			this.anim.SetLayerWeight(3, 1f);
			if (this.anim.GetInteger(AvatarController.vehiclePoseHash) >= 0)
			{
				this.anim.SetLayerWeight(1, 0f);
				this.anim.SetLayerWeight(2, 0f);
				return;
			}
			this.anim.SetLayerWeight(1, (float)((this.entity.inventory.holdingItem.HoldType == 0 || AnimationDelayData.AnimationDelay[this.entity.inventory.holdingItem.HoldType.Value].TwoHanded) ? 0 : 1));
			this.anim.SetLayerWeight(2, (float)((this.entity.inventory.holdingItem.HoldType == 0 || !AnimationDelayData.AnimationDelay[this.entity.inventory.holdingItem.HoldType.Value].TwoHanded) ? 0 : 1));
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00022E8C File Offset: 0x0002108C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateLayerStateInfo()
	{
		if (this.anim != null)
		{
			this.baseStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
			this.currentWeaponHoldLayer = this.anim.GetCurrentAnimatorStateInfo((this.entity.inventory.holdingItem.HoldType != 0 && AnimationDelayData.AnimationDelay[this.entity.inventory.holdingItem.HoldType.Value].TwoHanded) ? 2 : 1);
			this.painLayer = this.anim.GetCurrentAnimatorStateInfo(4);
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00022F2C File Offset: 0x0002112C
	public override void SwitchModelAndView(string _modelName, bool _bFPV, bool _bMale)
	{
		Transform transform = this.modelTransform.Find(_modelName);
		if (transform == null && _bFPV)
		{
			transform = this.modelTransform.Find(_modelName);
		}
		if (this.bipedTransform != null && this.bipedTransform != transform)
		{
			this.bipedTransform.gameObject.SetActive(false);
		}
		this.bipedTransform = transform;
		this.bipedTransform.gameObject.SetActive(true);
		this.modelName = _modelName;
		this.bMale = _bMale;
		this.bFPV = _bFPV;
		this.assignParts(this.bFPV);
		base.SetAnimator(base.GetComponentInChildren<Animator>());
		if (this.anim == null)
		{
			return;
		}
		base._setBool("IsMale", _bMale, true);
		if (this.rightHandItemTransform != null)
		{
			this.rightHandItemTransform.parent = this.rightHand;
			Vector3 position = AnimationGunjointOffsetData.AnimationGunjointOffset[this.entity.inventory.holdingItem.HoldType.Value].position;
			Vector3 rotation = AnimationGunjointOffsetData.AnimationGunjointOffset[this.entity.inventory.holdingItem.HoldType.Value].rotation;
			this.rightHandItemTransform.localPosition = position;
			this.rightHandItemTransform.localEulerAngles = rotation;
		}
		this.SetWalkType(this.entity.GetWalkType(), false);
		this._setBool(AvatarController.isDeadHash, this.entity.IsDead(), true);
		this._setBool(AvatarController.isFPVHash, this.bFPV, true);
		this._setBool(AvatarController.isAliveHash, this.entity.IsAlive(), true);
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateSpineRotation()
	{
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000211C2 File Offset: 0x0001F3C2
	public override Transform GetRightHandTransform()
	{
		return this.rightHand;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000230CB File Offset: 0x000212CB
	public override Transform GetMeshTransform()
	{
		if (!(this.meshTransform != null))
		{
			return this.bipedTransform;
		}
		return this.meshTransform;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000230E8 File Offset: 0x000212E8
	public override bool IsAnimationHitRunning()
	{
		return base.IsAnimationHitRunning() || this.hitStates.Contains(this.painLayer.fullPathHash);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0002310C File Offset: 0x0002130C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void LateUpdate()
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
		if ((this.baseStateInfo.fullPathHash == this.jumpState || this.baseStateInfo.fullPathHash == this.fpvJumpState) && !this.anim.IsInTransition(0))
		{
			base._setBool("Jump", false, true);
		}
		if (this.deathStates.Contains(this.baseStateInfo.fullPathHash) && !this.anim.IsInTransition(0))
		{
			base._setBool("IsDead", false, true);
		}
		bool flag = this.anim.IsInTransition(4);
		int integer = this.anim.GetInteger(AvatarController.hitBodyPartHash);
		bool flag2 = this.IsAnimationHitRunning();
		if (!flag && integer != 0 && flag2)
		{
			this._setInt(AvatarController.hitBodyPartHash, 0, true);
			base._setBool("isCritical", false, true);
		}
		if (this.anim.GetBool(AvatarController.itemUseHash))
		{
			int num = this.itemUseTicks - 1;
			this.itemUseTicks = num;
			if (num <= 0)
			{
				this._setBool(AvatarController.itemUseHash, false, true);
				if (this.rightHandAnimator != null)
				{
					this.rightHandAnimator.SetBool(AvatarController.itemUseHash, false);
				}
			}
		}
		if (this.isInDeathAnim && this.deathStates.Contains(this.baseStateInfo.fullPathHash) && !this.anim.IsInTransition(0))
		{
			this.didDeathTransition = true;
		}
		if (this.isInDeathAnim && this.didDeathTransition && (this.baseStateInfo.normalizedTime >= 1f || this.anim.IsInTransition(0)))
		{
			this.isInDeathAnim = false;
			if (this.entity.HasDeathAnim)
			{
				EModelBase emodel = this.entity.emodel;
				DamageResponse damageResponse = DamageResponse.New(true);
				emodel.DoRagdoll(in damageResponse, EModelBase.RagdollMode.Default, 999999f);
			}
		}
		if (this.isInDeathAnim && this.entity.HasDeathAnim && this.entity.RootMotion && this.entity.isCollidedHorizontally)
		{
			this.isInDeathAnim = false;
			EModelBase emodel2 = this.entity.emodel;
			DamageResponse damageResponse = DamageResponse.New(true);
			emodel2.DoRagdoll(in damageResponse, EModelBase.RagdollMode.Default, 999999f);
		}
	}

	// Token: 0x0400057F RID: 1407
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform meshTransform;

	// Token: 0x04000580 RID: 1408
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform cameraRootTransform;

	// Token: 0x04000581 RID: 1409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public HashSet<int> hitStates;

	// Token: 0x04000582 RID: 1410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AnimatorStateInfo painLayer;
}

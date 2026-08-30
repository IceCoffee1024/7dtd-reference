using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class UMACharacterBodyAnimator : BodyAnimator
{
	// Token: 0x0600058B RID: 1419 RVA: 0x000276EC File Offset: 0x000258EC
	public UMACharacterBodyAnimator(EntityAlive _entity, AvatarCharacterController.AnimationStates _animStates, Transform _bodyTransform, BodyAnimator.EnumState _defaultState)
	{
		Transform transform = _bodyTransform.FindInChilds((_entity.emodel is EModelSDCS) ? "RightWeapon" : "Gunjoint", false);
		BodyAnimator.BodyParts bodyParts = new BodyAnimator.BodyParts(_bodyTransform, transform);
		base.initBodyAnimator(_entity, bodyParts, _defaultState);
		this.deathStates = _animStates.DeathStates;
		this.hitStates = _animStates.HitStates;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0002774C File Offset: 0x0002594C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void assignLayerWeights()
	{
		base.assignLayerWeights();
		Animator animator = base.Animator;
		if (animator)
		{
			if (this.Entity.IsDead())
			{
				animator.SetLayerWeight(1, 0f);
				animator.SetLayerWeight(2, 0f);
				animator.SetLayerWeight(3, 0f);
				return;
			}
			animator.SetLayerWeight(3, 1f);
			if (animator.GetInteger(AvatarController.vehiclePoseHash) >= 0)
			{
				animator.SetLayerWeight(1, 0f);
				animator.SetLayerWeight(2, 0f);
				return;
			}
			if (!animator.IsInTransition(1) && AnimationDelayData.AnimationDelay[this.Entity.inventory.holdingItem.HoldType.Value].TwoHanded)
			{
				animator.SetLayerWeight(1, 0f);
				animator.SetLayerWeight(2, 1f);
				return;
			}
			if (!animator.IsInTransition(2))
			{
				animator.SetLayerWeight(1, 1f);
				animator.SetLayerWeight(2, 0f);
			}
		}
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00027844 File Offset: 0x00025A44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void cacheLayerStateInfo()
	{
		base.cacheLayerStateInfo();
		Animator animator = base.Animator;
		if (animator)
		{
			this.currentOverrideLayer = animator.GetCurrentAnimatorStateInfo(1);
			this.twoHandedLayer = animator.GetCurrentAnimatorStateInfo(2);
			this.painLayer = animator.GetCurrentAnimatorStateInfo(4);
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002788D File Offset: 0x00025A8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override AnimatorStateInfo getCachedLayerStateInfo(int _layer)
	{
		if (_layer == 1)
		{
			return this.currentOverrideLayer;
		}
		if (_layer == 2)
		{
			return this.twoHandedLayer;
		}
		return base.getCachedLayerStateInfo(_layer);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x000278AC File Offset: 0x00025AAC
	public override void SetDrunk(float _numBeers)
	{
		if (base.Animator && this.Entity.AttachedToEntity == null && this.avatarController != null)
		{
			this.avatarController.UpdateFloat("drunk", _numBeers, true);
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x000278FC File Offset: 0x00025AFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateSpineRotation()
	{
		if (!base.RagdollActive && !this.Entity.IsDead() && base.Animator && this.Entity.AttachedToEntity == null && this.avatarController != null)
		{
			float num;
			this.avatarController.TryGetFloat(AvatarController.yLookHash, out num);
			EntityPlayerLocal entityPlayerLocal = this.Entity as EntityPlayerLocal;
			if (entityPlayerLocal != null && !entityPlayerLocal.vp_FPCamera.Locked3rdPerson && entityPlayerLocal.IsCameraFacingCharacter())
			{
				this.avatarController.UpdateFloat(AvatarController.yLookHash, Mathf.Lerp(num, 0f, Time.deltaTime * 12f), false);
				return;
			}
			this.avatarController.UpdateFloat(AvatarController.yLookHash, Mathf.Lerp(num, -this.Entity.rotation.x / 90f, Time.deltaTime * 12f), false);
		}
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x000279F3 File Offset: 0x00025BF3
	public override void StartDeathAnimation(EnumBodyPartHit _bodyPart, int _movementState, float random)
	{
		base.StartDeathAnimation(_bodyPart, _movementState, random);
		this.isInDeathAnim = true;
		this.didDeathTransition = false;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00027A0C File Offset: 0x00025C0C
	public override void Update()
	{
		base.Update();
		this.updateSpineRotation();
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00027A1C File Offset: 0x00025C1C
	public override void LateUpdate()
	{
		base.LateUpdate();
		Animator animator = base.Animator;
		if (animator)
		{
			AnimatorStateInfo cachedLayerStateInfo = this.getCachedLayerStateInfo(0);
			if (this.isInDeathAnim)
			{
				if (this.deathStates.Contains(cachedLayerStateInfo.fullPathHash) && !animator.IsInTransition(0))
				{
					this.didDeathTransition = true;
				}
				if (this.didDeathTransition && (cachedLayerStateInfo.normalizedTime >= 1f || animator.IsInTransition(0)))
				{
					this.isInDeathAnim = false;
					if (this.Entity.HasDeathAnim)
					{
						EModelBase emodel = this.Entity.emodel;
						DamageResponse damageResponse = DamageResponse.New(true);
						emodel.DoRagdoll(in damageResponse, EModelBase.RagdollMode.Default, 999999f);
					}
				}
			}
			if (animator.GetInteger(AvatarController.hitBodyPartHash) != 0 && this.avatarController != null && !animator.IsInTransition(4) && this.hitStates.Contains(this.painLayer.fullPathHash))
			{
				this.avatarController.UpdateInt(AvatarController.hitBodyPartHash, 0, true);
			}
		}
	}

	// Token: 0x040005C1 RID: 1473
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimatorStateInfo currentOverrideLayer;

	// Token: 0x040005C2 RID: 1474
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimatorStateInfo twoHandedLayer;

	// Token: 0x040005C3 RID: 1475
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimatorStateInfo painLayer;

	// Token: 0x040005C4 RID: 1476
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> deathStates;

	// Token: 0x040005C5 RID: 1477
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> hitStates;

	// Token: 0x040005C6 RID: 1478
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInDeathAnim;

	// Token: 0x040005C7 RID: 1479
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didDeathTransition;

	// Token: 0x040005C8 RID: 1480
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cYLookUpdateSpeed = 12f;
}

using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class BodyAnimator
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600057F RID: 1407 RVA: 0x0002752C File Offset: 0x0002572C
	// (set) Token: 0x06000580 RID: 1408 RVA: 0x00027548 File Offset: 0x00025748
	public Animator Animator
	{
		get
		{
			if (!this.bodyParts.BodyObj.activeInHierarchy)
			{
				return null;
			}
			return this.animator;
		}
		set
		{
			if (this.bodyParts.BodyObj.activeInHierarchy)
			{
				this.animator = value;
			}
		}
	}

	// Token: 0x17000058 RID: 88
	// (set) Token: 0x06000581 RID: 1409 RVA: 0x00027564 File Offset: 0x00025764
	public BodyAnimator.EnumState State
	{
		set
		{
			if (this.state != value)
			{
				this.state = value;
				this.bodyParts.BodyObj.SetActive(this.state != BodyAnimator.EnumState.Disabled);
				this.updateVisibility();
				if (this.state != BodyAnimator.EnumState.Disabled)
				{
					this.animator = this.bodyParts.BodyObj.GetComponentInChildren<Animator>();
				}
			}
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x000275C2 File Offset: 0x000257C2
	public BodyAnimator.BodyParts Parts
	{
		get
		{
			return this.bodyParts;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000583 RID: 1411 RVA: 0x000275CA File Offset: 0x000257CA
	// (set) Token: 0x06000584 RID: 1412 RVA: 0x000275D4 File Offset: 0x000257D4
	public bool RagdollActive
	{
		get
		{
			return this.isRagdoll;
		}
		set
		{
			if (this.isRagdoll != value)
			{
				this.isRagdoll = value;
				if (this.animator)
				{
					this.animator.cullingMode = (this.isRagdoll ? AnimatorCullingMode.AlwaysAnimate : this.defaultCullingMode);
					this.animator.enabled = !this.isRagdoll;
				}
			}
		}
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00027630 File Offset: 0x00025830
	public virtual void StartDeathAnimation(EnumBodyPartHit _bodyPart, int _movementState, float random)
	{
		if (this.avatarController != null)
		{
			this.avatarController.UpdateInt(AvatarController.movementStateHash, _movementState, true);
			this.avatarController.UpdateBool(AvatarController.isAliveHash, false, true);
			this.avatarController.UpdateBool(AvatarController.isDeadHash, true, true);
			this.avatarController.UpdateInt(AvatarController.hitBodyPartHash, (int)_bodyPart.ToPrimary().LowerToUpperLimb(), true);
			this.avatarController.UpdateFloat("HitRandomValue", random, true);
			this.avatarController.TriggerEvent("DeathTrigger");
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x000276C0 File Offset: 0x000258C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void initBodyAnimator(EntityAlive _entity, BodyAnimator.BodyParts _bodyParts, BodyAnimator.EnumState _defaultState)
	{
		this.Entity = _entity;
		this.bodyParts = _bodyParts;
		this.state = _defaultState;
		this.animator = this.bodyParts.BodyObj.GetComponentInChildren<Animator>();
		this.defaultCullingMode = AnimatorCullingMode.AlwaysAnimate;
		this.meshes = this.bodyParts.BodyObj.GetComponentsInChildren<MeshRenderer>();
		this.skinnedMeshes = this.bodyParts.BodyObj.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (this.Entity.emodel != null)
		{
			this.avatarController = this.Entity.emodel.avatarController;
		}
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00027754 File Offset: 0x00025954
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void cacheLayerStateInfo()
	{
		if (this.animator && this.animator.gameObject.activeInHierarchy)
		{
			this.currentBaseState = this.animator.GetCurrentAnimatorStateInfo(0);
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00027787 File Offset: 0x00025987
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual AnimatorStateInfo getCachedLayerStateInfo(int _layer)
	{
		return this.currentBaseState;
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00027790 File Offset: 0x00025990
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateVisibility()
	{
		if (this.state != BodyAnimator.EnumState.Disabled)
		{
			bool flag = this.state == BodyAnimator.EnumState.Visible;
			if (this.meshes != null)
			{
				MeshRenderer[] array = this.meshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = flag;
				}
			}
			if (this.skinnedMeshes != null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.skinnedMeshes)
				{
					if (skinnedMeshRenderer)
					{
						skinnedMeshRenderer.enabled = flag;
					}
				}
			}
		}
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void assignLayerWeights()
	{
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetDrunk(float _numBeers)
	{
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00027809 File Offset: 0x00025A09
	public virtual void Update()
	{
		this.assignLayerWeights();
		this.updateVisibility();
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00027817 File Offset: 0x00025A17
	public virtual void LateUpdate()
	{
		this.cacheLayerStateInfo();
	}

	// Token: 0x040005B4 RID: 1460
	public EntityAlive Entity;

	// Token: 0x040005B5 RID: 1461
	[PublicizedFrom(EAccessModifier.Private)]
	public Animator animator;

	// Token: 0x040005B6 RID: 1462
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimatorStateInfo currentBaseState;

	// Token: 0x040005B7 RID: 1463
	[PublicizedFrom(EAccessModifier.Protected)]
	public AvatarController avatarController;

	// Token: 0x040005B8 RID: 1464
	[PublicizedFrom(EAccessModifier.Private)]
	public BodyAnimator.BodyParts bodyParts;

	// Token: 0x040005B9 RID: 1465
	[PublicizedFrom(EAccessModifier.Private)]
	public BodyAnimator.EnumState state;

	// Token: 0x040005BA RID: 1466
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimatorCullingMode defaultCullingMode;

	// Token: 0x040005BB RID: 1467
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRagdoll;

	// Token: 0x040005BC RID: 1468
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer[] skinnedMeshes;

	// Token: 0x040005BD RID: 1469
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshRenderer[] meshes;

	// Token: 0x020000DA RID: 218
	public enum EnumState
	{
		// Token: 0x040005BF RID: 1471
		Visible,
		// Token: 0x040005C0 RID: 1472
		OnlyColliders,
		// Token: 0x040005C1 RID: 1473
		Disabled
	}

	// Token: 0x020000DB RID: 219
	public class BodyParts
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0002781F File Offset: 0x00025A1F
		public BodyParts(Transform _bodyTransform, Transform _rightHand)
		{
			this.BodyObj = _bodyTransform.gameObject;
			this.RightHandT = _rightHand;
		}

		// Token: 0x040005C2 RID: 1474
		public GameObject BodyObj;

		// Token: 0x040005C3 RID: 1475
		public Transform RightHandT;
	}
}

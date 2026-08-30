using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Assets.DuckType.Jiggle;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

// Token: 0x0200050A RID: 1290
[Preserve]
public abstract class EModelBase : MonoBehaviour
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002A51 RID: 10833 RVA: 0x0010AC8A File Offset: 0x00108E8A
	// (set) Token: 0x06002A52 RID: 10834 RVA: 0x0010AC92 File Offset: 0x00108E92
	public bool IsFPV { get; set; }

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002A53 RID: 10835 RVA: 0x0010AC9B File Offset: 0x00108E9B
	public virtual Transform NeckTransform
	{
		get
		{
			return this.neckTransform;
		}
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x0010ACA4 File Offset: 0x00108EA4
	public virtual void Init(World _world, Entity _entity, EModelInstanceAssets _eModelAssets)
	{
		EntityClass entityClass = EntityClass.list[_entity.entityClass];
		this.assets = _eModelAssets;
		this.visible = false;
		this.entity = _entity;
		this.ragdollChance = entityClass.RagdollOnDeathChance;
		this.bHasRagdoll = entityClass.HasRagdoll;
		this.modelTransformParent = EModelBase.FindModel(base.transform);
		this.createModel(_world, entityClass);
		bool flag = this.entity.RootMotion || this.bHasRagdoll;
		if (GameManager.IsDedicatedServer && !flag)
		{
			if (entityClass.Properties.GetString(EntityClass.PropAvatarController).Length > 0)
			{
				this.avatarController = base.gameObject.AddComponent<AvatarControllerDummy>();
				Animator[] componentsInChildren = base.transform.GetComponentsInChildren<Animator>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
			}
		}
		else
		{
			this.createAvatarController(entityClass);
			if (GameManager.IsDedicatedServer && this.avatarController && flag)
			{
				this.avatarController.SetVisible(true);
			}
		}
		this.gazeController = base.transform.GetComponentInChildren<CharacterGazeController>();
		this.InitCommon();
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x0010ADBC File Offset: 0x00108FBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCommon()
	{
		this.LookAtInit();
		if (this.modelTransformParent)
		{
			this.SwitchModelAndView(false, EntityClass.list[this.entity.entityClass].bIsMale);
		}
		else
		{
			this.modelTransformParent = base.transform;
			this.headTransform = base.transform;
		}
		this.InitRigidBodies();
		this.JiggleInit();
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0010AE24 File Offset: 0x00109024
	public void InitRigidBodies()
	{
		if (!this.bipedRootTransform)
		{
			return;
		}
		List<Rigidbody> list = new List<Rigidbody>();
		this.bipedRootTransform.GetComponentsInChildren<Rigidbody>(list);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i].gameObject.CompareTag("AudioRigidBody"))
			{
				list.RemoveAt(i);
			}
		}
		float num = EntityClass.list[this.entity.entityClass].MassKg / (float)list.Count;
		if (list.Count == 11)
		{
			num *= 1.1224489f;
			int j = 0;
			while (j < list.Count)
			{
				Rigidbody rigidbody = list[j];
				float num2 = 1f;
				bool flag = false;
				string name = rigidbody.name;
				uint num3 = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num3 <= 2248339218U)
				{
					if (num3 <= 977255260U)
					{
						if (num3 != 819656463U)
						{
							if (num3 != 928972447U)
							{
								if (num3 != 977255260U)
								{
									goto IL_02C9;
								}
								if (!(name == "RightUpLeg"))
								{
									goto IL_02C9;
								}
							}
							else
							{
								if (!(name == "RightForeArm"))
								{
									goto IL_02C9;
								}
								goto IL_0295;
							}
						}
						else if (!(name == "LeftUpLeg"))
						{
							goto IL_02C9;
						}
						num2 = 1f;
					}
					else
					{
						if (num3 != 1413449684U)
						{
							if (num3 != 2231561599U)
							{
								if (num3 != 2248339218U)
								{
									goto IL_02C9;
								}
								if (!(name == "Spine2"))
								{
									goto IL_02C9;
								}
							}
							else if (!(name == "Spine1"))
							{
								goto IL_02C9;
							}
						}
						else if (!(name == "Spine"))
						{
							goto IL_02C9;
						}
						num2 = 2f;
						flag = true;
					}
				}
				else
				{
					if (num3 <= 2996251363U)
					{
						if (num3 != 2449536012U)
						{
							if (num3 != 2925602325U)
							{
								if (num3 != 2996251363U)
								{
									goto IL_02C9;
								}
								if (!(name == "Head"))
								{
									goto IL_02C9;
								}
								num2 = 0.8f;
								goto IL_02C9;
							}
							else if (!(name == "RightArm"))
							{
								goto IL_02C9;
							}
						}
						else
						{
							if (!(name == "LeftLeg"))
							{
								goto IL_02C9;
							}
							goto IL_02BF;
						}
					}
					else if (num3 <= 3119934960U)
					{
						if (num3 != 3001187991U)
						{
							if (num3 != 3119934960U)
							{
								goto IL_02C9;
							}
							if (!(name == "LeftForeArm"))
							{
								goto IL_02C9;
							}
							goto IL_0295;
						}
						else
						{
							if (!(name == "RightLeg"))
							{
								goto IL_02C9;
							}
							goto IL_02BF;
						}
					}
					else if (num3 != 3537553655U)
					{
						if (num3 != 4018002826U)
						{
							goto IL_02C9;
						}
						if (!(name == "LeftArm"))
						{
							goto IL_02C9;
						}
					}
					else
					{
						if (!(name == "Hips"))
						{
							goto IL_02C9;
						}
						num2 = 2f;
						goto IL_02C9;
					}
					num2 = 0.5f;
					goto IL_02C9;
					IL_02BF:
					num2 = 0.5f;
					flag = true;
				}
				IL_02C9:
				rigidbody.mass = num * num2;
				if (flag && !this.entity.isEntityRemote)
				{
					rigidbody.gameObject.GetOrAddComponent<CollisionCallForward>().Entity = this.entity;
				}
				if (rigidbody.drag <= 0f)
				{
					rigidbody.drag = 0.25f;
				}
				j++;
				continue;
				IL_0295:
				num2 = 0.5f;
				flag = true;
				goto IL_02C9;
			}
			return;
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].mass = num;
		}
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PostInit()
	{
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x0010B17C File Offset: 0x0010937C
	public static Transform FindModel(Transform _t)
	{
		Transform transform = _t.Find("Graphics/Model");
		if (transform)
		{
			return transform;
		}
		return _t;
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnload()
	{
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x0010B1A0 File Offset: 0x001093A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.assets.Release();
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x0010B1AD File Offset: 0x001093AD
	public void OriginChanged(Vector3 _deltaPos)
	{
		this.ragdollPosePelvisPos += _deltaPos;
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x0010B1C4 File Offset: 0x001093C4
	public virtual Vector3 GetHeadPosition()
	{
		if (this.headTransform == null)
		{
			return this.entity.position + Vector3.up * this.entity.GetEyeHeight();
		}
		return this.headTransform.position + Origin.position;
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0010B21A File Offset: 0x0010941A
	public virtual Vector3 GetNavObjectPosition()
	{
		if (this.NavObjectTransform == null)
		{
			return this.GetHeadPosition();
		}
		return this.NavObjectTransform.position + Origin.position;
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x0010B248 File Offset: 0x00109448
	public virtual Vector3 GetHipPosition()
	{
		if (this.bipedPelvisTransform == null)
		{
			return this.entity.position + Vector3.up * (this.entity.height * 0.5f);
		}
		return this.bipedPelvisTransform.position + Origin.position;
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x0010B2A4 File Offset: 0x001094A4
	public virtual Quaternion GetFlatHipRotation()
	{
		Vector3 vector;
		if (this.bipedPelvisTransform == null)
		{
			vector = this.entity.rotation;
		}
		else
		{
			vector = this.bipedPelvisTransform.eulerAngles;
		}
		vector = new Vector3(0f, vector.y, 0f);
		return Quaternion.Euler(vector);
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x0010B2F8 File Offset: 0x001094F8
	public virtual Vector3 GetChestPosition()
	{
		if (this.bipedPelvisTransform == null || this.headTransform == null)
		{
			return Vector3.Lerp(this.GetHipPosition(), this.GetHeadPosition(), 0.4f);
		}
		return Vector3.Lerp(this.bipedPelvisTransform.position, this.headTransform.position, 0.6f) + Origin.position;
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0010B364 File Offset: 0x00109564
	public virtual Vector3 GetBellyPosition()
	{
		if (this.bipedPelvisTransform == null || this.headTransform == null)
		{
			return Vector3.Lerp(this.GetHipPosition(), this.GetHeadPosition(), 0.2f);
		}
		return Vector3.Lerp(this.bipedPelvisTransform.position, this.headTransform.position, 0.2f) + Origin.position;
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x0010B3D0 File Offset: 0x001095D0
	public IKController AddIKController()
	{
		IKController ikcontroller = null;
		Transform transform = this.GetModelTransform();
		if (transform)
		{
			Animator componentInChildren = transform.GetComponentInChildren<Animator>();
			if (componentInChildren)
			{
				ikcontroller = componentInChildren.GetComponent<IKController>();
				if (!ikcontroller)
				{
					ikcontroller = componentInChildren.gameObject.AddComponent<IKController>();
				}
			}
		}
		return ikcontroller;
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x0010B41C File Offset: 0x0010961C
	public void RemoveIKController()
	{
		Transform transform = this.GetModelTransform();
		if (transform)
		{
			IKController componentInChildren = transform.GetComponentInChildren<IKController>();
			if (componentInChildren)
			{
				componentInChildren.Cleanup();
				global::UnityEngine.Object.Destroy(componentInChildren);
			}
		}
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x0010B454 File Offset: 0x00109654
	[PublicizedFrom(EAccessModifier.Private)]
	public void CrouchUpdate(EntityAlive _ea)
	{
		Transform transform = this.neckParentTransform;
		Transform parent = transform.parent;
		float crouchBendPer = _ea.crouchBendPer;
		Quaternion quaternion = Quaternion.Euler(28f * crouchBendPer, 0f, 0f);
		transform.localRotation *= quaternion;
		parent.localRotation *= quaternion;
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x0010B4B0 File Offset: 0x001096B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void LookAtInit()
	{
		EntityClass entityClass = EntityClass.list[this.entity.entityClass];
		this.lookAtMaxAngle = entityClass.LookAtAngle;
		this.lookAtEnabled = this.lookAtMaxAngle > 0f;
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x0010B4F2 File Offset: 0x001096F2
	public void ClearLookAt()
	{
		this.lookAtBlendPerTarget = 0f;
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0010B4FF File Offset: 0x001096FF
	public void SetLookAt(Vector3 _pos)
	{
		this.lookAtPos = _pos;
		this.lookAtBlendPerTarget = this.lookAtFullBlendPer;
		this.lookAtIsPos = true;
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x0010B51B File Offset: 0x0010971B
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetLookAt()
	{
		this.lookAtBlendPer = 0f;
		this.lookAtBlendPerTarget = 0f;
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x0010B534 File Offset: 0x00109734
	[PublicizedFrom(EAccessModifier.Private)]
	public void LookAtUpdate(EntityAlive e)
	{
		if (this.gazeController != null)
		{
			return;
		}
		EnumEntityStunType currentStun = e.bodyDamage.CurrentStun;
		float deltaTime = Time.deltaTime;
		if (e.IsDead() || (currentStun != EnumEntityStunType.None && currentStun != EnumEntityStunType.Getup))
		{
			this.lookAtBlendPerTarget = 0f;
		}
		else if (!this.lookAtIsPos)
		{
			this.lookAtBlendPerTarget -= deltaTime;
			Vector3 vector;
			if (e.GetHeadLookTarget(out vector))
			{
				this.lookAtPos = vector;
				this.lookAtBlendPerTarget = this.lookAtFullBlendPer;
			}
		}
		if (this.lookAtBlendPer <= 0f && this.lookAtBlendPerTarget <= 0f)
		{
			return;
		}
		this.lookAtFullChangeTime -= deltaTime;
		if (this.lookAtFullChangeTime <= 0f)
		{
			this.lookAtFullChangeTime = 1.3f + 2.7f * e.rand.RandomFloat;
			this.lookAtFullBlendPer = 0.2f + 1.5f * e.rand.RandomFloat;
			if (this.lookAtFullBlendPer > 1f)
			{
				this.lookAtFullBlendPer = 1f;
			}
		}
		this.lookAtBlendPer = Mathf.MoveTowards(this.lookAtBlendPer, this.lookAtBlendPerTarget, deltaTime * 1.5f);
		Quaternion rotation = this.neckParentTransform.rotation;
		Transform transform = this.headTransform;
		Vector3 vector2 = rotation * Vector3.up;
		Quaternion quaternion;
		if (this.entity is EntityNPC && !(this.avatarController is AvatarSDCSController))
		{
			quaternion = Quaternion.LookRotation(this.lookAtPos - Origin.position - transform.position);
			quaternion *= Quaternion.AngleAxis(-90f, Vector3.forward);
		}
		else
		{
			quaternion = Quaternion.LookRotation(this.lookAtPos - Origin.position - transform.position, vector2);
			quaternion *= Quaternion.Slerp(Quaternion.identity, transform.localRotation, 0.5f);
		}
		Quaternion quaternion2 = Quaternion.RotateTowards(rotation, quaternion, this.lookAtMaxAngle);
		this.lookAtRot = Quaternion.Slerp(this.lookAtRot, quaternion2, 0.16f);
		float num = this.lookAtBlendPer;
		this.neckTransform.rotation = Quaternion.Slerp(this.neckTransform.rotation, this.lookAtRot, num * 0.4f);
		Quaternion rotation2 = transform.rotation;
		transform.rotation = Quaternion.Slerp(rotation2, this.lookAtRot, num);
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x0010B782 File Offset: 0x00109982
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.FrameUpdateRagdoll();
		if (this.modelTransformParent != this.headTransform)
		{
			this.UpdateHeadState();
		}
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x0010B7A4 File Offset: 0x001099A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (this.ragdollIsBlending)
		{
			this.BlendRagdoll();
		}
		EntityAlive entityAlive = this.entity as EntityAlive;
		if (entityAlive != null && !this.IsRagdollActive)
		{
			if (entityAlive.crouchType > 0)
			{
				this.CrouchUpdate(entityAlive);
			}
			if (this.lookAtEnabled)
			{
				this.LookAtUpdate(entityAlive);
			}
		}
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x0010B7F5 File Offset: 0x001099F5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Transform GetModelTransform()
	{
		if (!this.modelTransform)
		{
			return this.modelTransformParent;
		}
		return this.modelTransform;
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x0010B811 File Offset: 0x00109A11
	public Transform GetModelTransformParent()
	{
		return this.modelTransformParent;
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x0010B81C File Offset: 0x00109A1C
	public virtual Transform GetHitTransform(DamageSource _damageSource)
	{
		string hitTransformName = _damageSource.getHitTransformName();
		if (hitTransformName != null && this.bipedRootTransform)
		{
			return this.bipedRootTransform.FindInChilds(hitTransformName, false);
		}
		return null;
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x0010B850 File Offset: 0x00109A50
	public virtual Transform GetHitTransform(BodyPrimaryHit _primary)
	{
		if (this.physicsBody != null)
		{
			string text;
			switch (_primary)
			{
			case BodyPrimaryHit.Torso:
				text = "E_BP_Body";
				break;
			case BodyPrimaryHit.Head:
				text = "E_BP_Head";
				break;
			case BodyPrimaryHit.LeftUpperArm:
				text = "E_BP_LArm";
				break;
			case BodyPrimaryHit.RightUpperArm:
				text = "E_BP_RArm";
				break;
			case BodyPrimaryHit.LeftUpperLeg:
				text = "E_BP_LLeg";
				break;
			case BodyPrimaryHit.RightUpperLeg:
				text = "E_BP_RLeg";
				break;
			case BodyPrimaryHit.LeftLowerArm:
				text = "E_BP_LLowerArm";
				break;
			case BodyPrimaryHit.RightLowerArm:
				text = "E_BP_RLowerArm";
				break;
			case BodyPrimaryHit.LeftLowerLeg:
				text = "E_BP_LLowerLeg";
				break;
			case BodyPrimaryHit.RightLowerLeg:
				text = "E_BP_RLowerLeg";
				break;
			default:
				return null;
			}
			return this.physicsBody.GetTransformForColliderTag(text);
		}
		return null;
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x0010B8FA File Offset: 0x00109AFA
	public virtual Transform GetHeadTransform()
	{
		if (!this.headTransform)
		{
			return base.transform;
		}
		return this.headTransform;
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x0010B916 File Offset: 0x00109B16
	public virtual Transform GetPelvisTransform()
	{
		return this.bipedPelvisTransform;
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x0010B91E File Offset: 0x00109B1E
	public virtual Transform GetThirdPersonCameraTransform()
	{
		return base.transform;
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x0010B928 File Offset: 0x00109B28
	public virtual void OnDeath(DamageResponse _dmResponse, ChunkCluster _cc)
	{
		EntityAlive entityAlive = this.entity as EntityAlive;
		bool flag = entityAlive && entityAlive.bodyDamage.CurrentStun > EnumEntityStunType.None;
		bool flag2 = true;
		if (this.HasRagdoll() && (flag2 || !this.entity.HasDeathAnim || entityAlive.IsSleeper || entityAlive.GetWalkType() == 21 || flag || _dmResponse.Random < this.ragdollChance))
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.DoRagdoll(in _dmResponse, EModelBase.RagdollMode.Default, 999999f);
				return;
			}
			if (!entityAlive.IsSpawned())
			{
				this.SpawnWithRagdoll();
				return;
			}
		}
		else if (this.avatarController != null)
		{
			this.avatarController.StartDeathAnimation(_dmResponse.HitBodyPart, _dmResponse.MovementState, _dmResponse.Random);
			if (this.entity is EntityPlayer && this.bipedRootTransform)
			{
				Transform transform = this.bipedRootTransform.Find("Spine1");
				if (transform)
				{
					RagdollWhenHit ragdollWhenHit;
					if (transform.TryGetComponent<RagdollWhenHit>(out ragdollWhenHit))
					{
						ragdollWhenHit.enabled = true;
						return;
					}
					transform.gameObject.AddComponent<RagdollWhenHit>();
				}
			}
		}
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void restoreTPose(PhysicsBodyInstance physicsBody)
	{
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x0010BA46 File Offset: 0x00109C46
	public virtual bool HasRagdoll()
	{
		return this.bHasRagdoll && this.physicsBody != null;
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002A77 RID: 10871 RVA: 0x0010BA5B File Offset: 0x00109C5B
	public bool IsRagdollActive
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.ragdollState > EModelBase.ERagdollState.Off;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002A78 RID: 10872 RVA: 0x0010BA66 File Offset: 0x00109C66
	public bool IsRagdollMovement
	{
		get
		{
			return this.ragdollState != EModelBase.ERagdollState.Off && this.ragdollState != EModelBase.ERagdollState.StandCollide;
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002A79 RID: 10873 RVA: 0x0010BA7E File Offset: 0x00109C7E
	public bool IsRagdollOn
	{
		get
		{
			return this.ragdollState == EModelBase.ERagdollState.On || this.ragdollState == EModelBase.ERagdollState.Dead;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002A7A RID: 10874 RVA: 0x0010BA94 File Offset: 0x00109C94
	public bool IsRagdollDead
	{
		get
		{
			return this.ragdollState == EModelBase.ERagdollState.Dead;
		}
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x0010BAA0 File Offset: 0x00109CA0
	public void DoRagdoll(EModelBase.RagdollMode _mode, float stunTime, EnumBodyPartHit bodyPart, Vector3 forceVec, Vector3 forceWorldPos, bool isRemote)
	{
		if (this.entity.IsFlyMode.Value || this.entity.AttachedToEntity)
		{
			return;
		}
		if (!isRemote && !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.entity.isEntityRemote)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEntityRagdoll>().Setup(this.entity, (byte)_mode, stunTime, bodyPart, forceVec, forceWorldPos), false);
			return;
		}
		bool flag = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.entity.isEntityRemote;
		if (stunTime == 0f)
		{
			if (!flag)
			{
				this.entity.PhysicsPush(forceVec, forceWorldPos, false);
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.entity.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entity.entityId, -1, NetPackageManager.GetPackage<NetPackageEntityRagdoll>().Setup(this.entity, (byte)_mode, 0f, EnumBodyPartHit.Torso, forceVec, forceWorldPos), false);
			}
			return;
		}
		if (!this.StartRagdoll(stunTime))
		{
			return;
		}
		if (forceVec.sqrMagnitude > 0f && !this.entity.isEntityRemote)
		{
			Vector3 vector = forceVec;
			if (bodyPart == EnumBodyPartHit.None)
			{
				float num = -10f;
				if (vector.y < num)
				{
					vector.y = num;
				}
				this.SetRagdollVelocity(vector);
			}
			else
			{
				BodyPrimaryHit bodyPrimaryHit = bodyPart.ToPrimary();
				Transform hitTransform = this.GetHitTransform(bodyPrimaryHit);
				if (hitTransform)
				{
					Rigidbody component = hitTransform.GetComponent<Rigidbody>();
					if (component)
					{
						if (_mode != EModelBase.RagdollMode.FullForce)
						{
							this.ClampForceForMass(ref vector);
						}
						if (forceWorldPos.sqrMagnitude > 0f)
						{
							component.AddForceAtPosition(vector, forceWorldPos - Origin.position, ForceMode.Impulse);
						}
						else
						{
							component.AddForce(vector, ForceMode.Impulse);
						}
					}
				}
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.entity.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entity.entityId, -1, NetPackageManager.GetPackage<NetPackageEntityRagdoll>().Setup(this.entity, (byte)_mode, stunTime, bodyPart, forceVec, forceWorldPos), false);
		}
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x0010BC90 File Offset: 0x00109E90
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClampForceForMass(ref Vector3 _force)
	{
		float num = EntityClass.list[this.entity.entityClass].MassKg * 8f;
		float num2 = _force.sqrMagnitude + 0.001f;
		if (num2 > num * num)
		{
			_force *= num / Mathf.Sqrt(num2);
		}
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x0010BCEC File Offset: 0x00109EEC
	public void DoRagdoll(in DamageResponse dr, EModelBase.RagdollMode _mode = EModelBase.RagdollMode.Default, float stunTime = 999999f)
	{
		EntityAlive entityAlive = this.entity as EntityAlive;
		if (entityAlive && entityAlive.isDisintegrated)
		{
			return;
		}
		float num = (float)dr.Strength;
		DamageSource source = dr.Source;
		if (num > 0f && source != null)
		{
			Vector3 vector = source.getDirection();
			EnumDamageTypes damageType = source.GetDamageType();
			if (damageType != EnumDamageTypes.Falling && damageType != EnumDamageTypes.Crushing)
			{
				float num2;
				if (dr.HitBodyPart == EnumBodyPartHit.None)
				{
					num2 = this.entity.rand.RandomRange(5f, 25f);
				}
				else
				{
					float num3 = -10f;
					if (stunTime == 0f)
					{
						num3 = 5f;
					}
					num2 = this.entity.rand.RandomRange(num3, 40f);
					num *= 0.5f;
					if (source.damageType == EnumDamageTypes.Bashing)
					{
						num *= 2.5f;
					}
					if (dr.Critical)
					{
						num2 += 25f;
						num *= 2f;
					}
					if ((dr.HitBodyPart & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
					{
						num *= 0.45f;
					}
					num = Utils.FastMin(20f + num, 500f);
					vector *= num;
				}
				Vector3 vector2 = Vector3.Cross(vector.normalized, Vector3.up);
				vector = Quaternion.AngleAxis(num2, vector2) * vector;
			}
			this.DoRagdoll(_mode, stunTime, dr.HitBodyPart, vector, source.getHitTransformPosition(), false);
			return;
		}
		this.DoRagdoll(_mode, stunTime, dr.HitBodyPart, Vector3.zero, Vector3.zero, false);
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x0010BE61 File Offset: 0x0010A061
	public void SetRagdollState(int newState)
	{
		if (this.ragdollState == EModelBase.ERagdollState.On && newState == 2)
		{
			this.ragdollTime = 9999f;
		}
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x0010BE7B File Offset: 0x0010A07B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnWithRagdoll()
	{
		if (this.ragdollState == EModelBase.ERagdollState.Off)
		{
			this.ragdollState = EModelBase.ERagdollState.SpawnWait;
		}
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x0010BE8C File Offset: 0x0010A08C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool StartRagdoll(float stunTime)
	{
		if (!this.HasRagdoll())
		{
			return false;
		}
		if (this.ragdollState == EModelBase.ERagdollState.Dead)
		{
			return true;
		}
		bool flag = this.entity.IsDead();
		if (!flag && this.ragdollState == EModelBase.ERagdollState.On)
		{
			this.ragdollDuration = Utils.FastMax(this.ragdollDuration, stunTime);
			return true;
		}
		if (this.entity.IsMarkedForUnload())
		{
			return false;
		}
		this.ragdollAnimator = this.avatarController.GetAnimator();
		if (!this.ragdollAnimator)
		{
			return false;
		}
		bool flag2 = this.ragdollState > EModelBase.ERagdollState.Off;
		this.ragdollState = EModelBase.ERagdollState.On;
		this.ragdollTime = 0f;
		this.entity.OnRagdoll(true);
		this.ragdollIsPlayer = this.entity is EntityPlayer;
		this.ragdollAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		this.ragdollAnimator.keepAnimatorStateOnDisable = true;
		this.ragdollAnimator.enabled = false;
		Animation component = this.GetModelTransform().GetComponent<Animation>();
		if (component)
		{
			component.cullingType = AnimationCullingType.AlwaysAnimate;
			component.enabled = false;
		}
		this.CaptureRagdollBones();
		this.CaptureRagdollZeroBones();
		if (flag)
		{
			stunTime = 0.3f;
			this.SetRagdollDead();
		}
		if (this.physicsBody != null)
		{
			this.physicsBody.SetColliderMode(EnumColliderType.All, EnumColliderMode.Ragdoll);
		}
		this.entity.PhysicsPause();
		EntityAlive entityAlive = this.entity as EntityAlive;
		entityAlive.SetStun(EnumEntityStunType.Prone);
		entityAlive.bodyDamage.StunDuration = 1f;
		entityAlive.SetCVar("ragdoll", 1f);
		if (!this.ragdollIsPlayer)
		{
			this.entity.PhysicsTransform.gameObject.SetActive(false);
		}
		this.ragdollTime = 0f;
		this.ragdollDuration = stunTime;
		this.ragdollRotY = this.entity.rotation.y;
		if (this.ragdollIsPlayer)
		{
			this.ragdollDuration *= 0.5f;
		}
		if (flag2)
		{
			this.ragdollIsBlending = false;
			this.ragdollAdjustPosDelay = 0f;
		}
		this.ResetLookAt();
		return true;
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x0010C074 File Offset: 0x0010A274
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRagdollDead()
	{
		this.ragdollState = EModelBase.ERagdollState.Dead;
		for (int i = 0; i < this.ragdollPoses.Count; i++)
		{
			Rigidbody rb = this.ragdollPoses[i].rb;
			if (rb)
			{
				rb.maxDepenetrationVelocity = 2f;
				rb.maxAngularVelocity = 1f;
			}
		}
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x0010C0D0 File Offset: 0x0010A2D0
	public void DisableRagdoll(bool isSetAlive)
	{
		if (this.ragdollState == EModelBase.ERagdollState.Off)
		{
			return;
		}
		if (this.physicsBody != null)
		{
			this.physicsBody.SetColliderMode(EnumColliderType.All, EnumColliderMode.Collision);
		}
		if (this.bipedRootTransform)
		{
			RagdollWhenHit[] componentsInChildren = this.bipedRootTransform.GetComponentsInChildren<RagdollWhenHit>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
			}
		}
		EntityAlive entityAlive = this.entity as EntityAlive;
		if (!isSetAlive && !this.entity.IsDead())
		{
			Vector3 vector = this.headTransform.position - this.bipedPelvisTransform.position;
			float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			this.ragdollIsFacingUp = false;
			this.ragdollIsAnimal = EntityClass.list[entityAlive.entityClass].bIsAnimalEntity;
			string text;
			if (this.ragdollIsAnimal)
			{
				text = "Knockdown";
			}
			else
			{
				text = "Knockdown - Chest";
				if (this.bipedPelvisTransform.forward.y > 0f)
				{
					this.ragdollIsFacingUp = true;
					text = "Knockdown - Back";
					num += 180f;
				}
			}
			this.ragdollRotY = num;
			this.CopyRagdollRot();
			Animation component = this.GetModelTransform().GetComponent<Animation>();
			if (component != null)
			{
				component.cullingType = AnimationCullingType.AlwaysAnimate;
				component.enabled = true;
			}
			this.avatarController.ResetAnimations();
			this.ragdollAnimator.keepAnimatorStateOnDisable = true;
			int num2 = (this.ragdollIsPlayer ? 5 : 0);
			this.ragdollAnimator.CrossFadeInFixedTime(text, 0.25f, num2, 2f, 0f);
			this.ragdollAdjustPosDelay = 0.05f;
			this.ragdollState = EModelBase.ERagdollState.BlendOutGround;
			this.ragdollTime = 0f;
			this.ragdollIsBlending = true;
			entityAlive.SetStun(EnumEntityStunType.Getup);
			return;
		}
		this.bipedPelvisTransform.localPosition = this.ragdollPosePelvisLocalPos;
		this.RestoreRagdollStartRot();
		this.SetRagdollOff();
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x0010C2AC File Offset: 0x0010A4AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CaptureRagdollBones()
	{
		if (this.ragdollPoses.Count > 0)
		{
			return;
		}
		Animator animator = this.avatarController.GetAnimator();
		if (!animator)
		{
			return;
		}
		this.ragdollPosePelvisLocalPos = this.bipedPelvisTransform.localPosition;
		animator.GetComponentsInChildren<Rigidbody>(EModelBase.ragdollTempRBs);
		for (int i = 0; i < EModelBase.ragdollTempRBs.Count; i++)
		{
			Rigidbody rigidbody = EModelBase.ragdollTempRBs[i];
			GameObject gameObject = rigidbody.gameObject;
			if (!gameObject.CompareTag("Item") && !gameObject.CompareTag("AudioRigidBody"))
			{
				Transform transform = rigidbody.transform;
				EModelBase.RagdollPose ragdollPose;
				ragdollPose.t = transform;
				ragdollPose.rb = rigidbody;
				ragdollPose.rot = Quaternion.identity;
				ragdollPose.startRot = transform.localRotation;
				this.ragdollPoses.Add(ragdollPose);
			}
		}
		EModelBase.ragdollTempRBs.Clear();
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x0010C388 File Offset: 0x0010A588
	public void CaptureRagdollPositions(List<Vector3> positionList)
	{
		this.CaptureRagdollBones();
		positionList.Clear();
		for (int i = 0; i < this.ragdollPoses.Count; i++)
		{
			positionList.Add(this.ragdollPoses[i].t.position);
		}
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x0010C3D4 File Offset: 0x0010A5D4
	public void ApplyRagdollVelocities(List<Vector3> velocities)
	{
		if (velocities.Count == this.ragdollPoses.Count)
		{
			for (int i = 0; i < this.ragdollPoses.Count; i++)
			{
				Rigidbody rb = this.ragdollPoses[i].rb;
				if (rb)
				{
					rb.velocity = velocities[i];
				}
			}
		}
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x0010C434 File Offset: 0x0010A634
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRagdollVelocity(Vector3 _vel)
	{
		for (int i = 0; i < this.ragdollPoses.Count; i++)
		{
			this.ragdollPoses[i].rb.velocity = _vel;
		}
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x0010C470 File Offset: 0x0010A670
	[PublicizedFrom(EAccessModifier.Private)]
	public void CopyRagdollRot()
	{
		this.ragdollPosePelvisPos = this.bipedPelvisTransform.position;
		for (int i = 0; i < this.ragdollPoses.Count; i++)
		{
			EModelBase.RagdollPose ragdollPose = this.ragdollPoses[i];
			if (ragdollPose.t == this.bipedPelvisTransform)
			{
				ragdollPose.rot = ragdollPose.t.rotation;
			}
			else
			{
				ragdollPose.rot = ragdollPose.t.localRotation;
			}
			this.ragdollPoses[i] = ragdollPose;
		}
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x0010C4F8 File Offset: 0x0010A6F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RestoreRagdollStartRot()
	{
		for (int i = 0; i < this.ragdollPoses.Count; i++)
		{
			this.ragdollPoses[i].t.localRotation = this.ragdollPoses[i].startRot;
		}
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x0010C544 File Offset: 0x0010A744
	[PublicizedFrom(EAccessModifier.Private)]
	public void CaptureRagdollZeroBones()
	{
		if (this.ragdollZeroBones != null)
		{
			return;
		}
		Transform parent = this.headTransform;
		if (!parent)
		{
			return;
		}
		this.ragdollZeroTime = 0.33f;
		this.ragdollZeroBones = new List<Transform>();
		while ((parent = parent.parent) && !(parent == this.bipedPelvisTransform))
		{
			if (!parent.GetComponent<Rigidbody>())
			{
				GameObject gameObject = parent.gameObject;
				if (!gameObject.CompareTag("Item") && !gameObject.CompareTag("AudioRigidBody"))
				{
					this.ragdollZeroBones.Add(parent);
				}
			}
		}
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x0010C5DC File Offset: 0x0010A7DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BlendRagdollZeroBones()
	{
		if (this.ragdollZeroTime > 0f)
		{
			float deltaTime = Time.deltaTime;
			this.ragdollZeroTime -= deltaTime;
			for (int i = 0; i < this.ragdollZeroBones.Count; i++)
			{
				Transform transform = this.ragdollZeroBones[i];
				transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, 120f * deltaTime);
			}
		}
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x0010C648 File Offset: 0x0010A848
	[PublicizedFrom(EAccessModifier.Private)]
	public void BlendRagdoll()
	{
		if (this.ragdollAdjustPosDelay > 0f)
		{
			this.ragdollAdjustPosDelay -= Time.deltaTime;
			if (this.ragdollAdjustPosDelay <= 0f)
			{
				Vector3 vector = this.bipedPelvisTransform.position - this.modelTransform.position;
				Vector3 vector2 = this.ragdollPosePelvisPos;
				vector2.x -= vector.x;
				vector2.z -= vector.z;
				if (this.entity.isEntityRemote)
				{
					vector2 = Vector3.Lerp(vector2, this.entity.targetPos - Origin.position, Time.fixedDeltaTime * 10f);
				}
				int num = 0;
				RaycastHit raycastHit;
				while (num < 5 && Physics.Raycast(vector2, Vector3.down, out raycastHit, 3f, -538750989))
				{
					RootTransformRefEntity component = raycastHit.transform.GetComponent<RootTransformRefEntity>();
					if (!component || component.RootTransform != this.entity.transform)
					{
						vector2.y = raycastHit.point.y + 0.02f;
						break;
					}
					vector2.y = raycastHit.point.y - 0.01f;
					num++;
				}
				this.entity.PhysicsResume(vector2 + Origin.position, this.ragdollRotY);
			}
		}
		float num2 = this.ragdollTime / 0.7f;
		float num3 = num2;
		if (!this.ragdollIsAnimal)
		{
			num3 = (num2 - 0.2f) / 0.8f;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
		}
		this.bipedPelvisTransform.position = Vector3.Lerp(this.ragdollPosePelvisPos, this.bipedPelvisTransform.position, num3);
		for (int i = 0; i < this.ragdollPoses.Count; i++)
		{
			Transform t = this.ragdollPoses[i].t;
			if (t == this.bipedPelvisTransform)
			{
				t.rotation = Quaternion.Slerp(this.ragdollPoses[i].rot, t.rotation, num3);
			}
			else
			{
				t.localRotation = Quaternion.Slerp(this.ragdollPoses[i].rot, t.localRotation, num2);
			}
		}
	}

	// Token: 0x06002A8C RID: 10892 RVA: 0x0010C894 File Offset: 0x0010AA94
	[PublicizedFrom(EAccessModifier.Private)]
	public void FrameUpdateRagdoll()
	{
		if (this.ragdollState == EModelBase.ERagdollState.Off)
		{
			return;
		}
		if (this.ragdollState == EModelBase.ERagdollState.SpawnWait)
		{
			Chunk chunk = (Chunk)this.entity.world.GetChunkFromWorldPos(this.entity.GetBlockPosition());
			if (chunk != null && chunk.IsCollisionMeshGenerated && chunk.IsDisplayed)
			{
				this.ragdollState = EModelBase.ERagdollState.Off;
				this.StartRagdoll(float.MaxValue);
			}
			return;
		}
		bool flag = this.entity.IsDead();
		if (this.pelvisRB && this.IsRagdollMovement)
		{
			if (!flag && this.entity.isEntityRemote)
			{
				Vector3 position = this.pelvisRB.position;
				Vector3 vector = this.entity.targetPos - Origin.position;
				this.pelvisRB.AddForce(-EModelBase.serverPosSpringForce * (position - vector) - EModelBase.serverPosSpringDamping * this.pelvisRB.velocity, ForceMode.Acceleration);
			}
			if (!(this.entity is EntityPlayer))
			{
				this.entity.SetPosition(this.pelvisRB.position + Origin.position, false);
			}
			else if (!this.entity.isEntityRemote)
			{
				this.entity.SetPosition(this.pelvisRB.position + Origin.position, false);
			}
		}
		this.entity.SetRotationAndStopTurning(new Vector3(0f, this.ragdollRotY, 0f));
		this.ragdollTime += Time.deltaTime;
		switch (this.ragdollState)
		{
		case EModelBase.ERagdollState.On:
			this.BlendRagdollZeroBones();
			if (this.ragdollTime >= this.ragdollDuration && (this.pelvisRB.velocity.sqrMagnitude <= 0.25f || this.ragdollTime > 10f))
			{
				this.DisableRagdoll(false);
				if (!this.entity.isEntityRemote)
				{
					NetPackageEntityRagdoll netPackageEntityRagdoll = NetPackageManager.GetPackage<NetPackageEntityRagdoll>().Setup(this.entity, (byte)this.ragdollState);
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						this.entity.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entity.entityId, -1, netPackageEntityRagdoll, false);
						return;
					}
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageEntityRagdoll, false);
					return;
				}
			}
			break;
		case EModelBase.ERagdollState.BlendOutGround:
			this.RestoreRagdollStartRot();
			if (this.ragdollTime >= 0.25f)
			{
				string text = "GetUpChest";
				if (this.ragdollIsFacingUp)
				{
					text = "GetUpBack";
				}
				if ((this.entity as EntityAlive).IsWalkTypeACrawl())
				{
					text = "CrawlerGetUpChest";
				}
				int num = (this.ragdollIsPlayer ? 5 : 0);
				this.ragdollAnimator.CrossFade(text, 0.3f, num);
				this.ragdollState = EModelBase.ERagdollState.BlendOutStand;
				return;
			}
			break;
		case EModelBase.ERagdollState.BlendOutStand:
			this.RestoreRagdollStartRot();
			if (this.ragdollTime >= 0.7f)
			{
				this.ragdollIsBlending = false;
				this.ragdollState = EModelBase.ERagdollState.Stand;
				this.ragdollTime = 0f;
				return;
			}
			break;
		case EModelBase.ERagdollState.Stand:
			if (this.ragdollTime >= 0.8f)
			{
				if (!this.ragdollIsPlayer)
				{
					this.entity.PhysicsTransform.gameObject.SetActive(true);
				}
				this.ragdollState = EModelBase.ERagdollState.StandCollide;
				return;
			}
			break;
		case EModelBase.ERagdollState.StandCollide:
			if (this.ragdollTime >= 1.7f)
			{
				this.SetRagdollOff();
				this.entity.OnRagdoll(false);
				return;
			}
			break;
		case EModelBase.ERagdollState.SpawnWait:
			break;
		case EModelBase.ERagdollState.Dead:
			this.BlendRagdollZeroBones();
			if (this.ragdollTime >= this.ragdollDuration)
			{
				this.ragdollDuration = float.MaxValue;
				if (this.physicsBody != null)
				{
					this.physicsBody.SetColliderMode(EnumColliderType.All, EnumColliderMode.RagdollDead);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x0010CC28 File Offset: 0x0010AE28
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRagdollOff()
	{
		this.ragdollState = EModelBase.ERagdollState.Off;
		EntityAlive entityAlive = this.entity as EntityAlive;
		entityAlive.SetCVar("ragdoll", 0f);
		entityAlive.ClearStun();
		if (!this.ragdollIsPlayer)
		{
			this.entity.PhysicsTransform.gameObject.SetActive(true);
		}
		this.ragdollPoses.Clear();
		this.ragdollZeroBones = null;
		this.CheckAnimFreeze();
	}

	// Token: 0x06002A8E RID: 10894 RVA: 0x0010CC92 File Offset: 0x0010AE92
	public string GetRagdollDebugInfo()
	{
		return string.Format("{0:0.#}/{1:0.#} {2}", this.ragdollTime.ToCultureInvariantString(), this.ragdollDuration.ToCultureInvariantString(), this.ragdollState.ToStringCached<EModelBase.ERagdollState>());
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x0010CCC0 File Offset: 0x0010AEC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ClothSimInit()
	{
		this.clothSim = base.GetComponentsInChildren<Cloth>();
		if (this.clothSim != null)
		{
			for (int i = this.clothSim.Length - 1; i >= 0; i--)
			{
				this.clothSim[i].gameObject.SetActive(false);
			}
			if (GameManager.IsDedicatedServer)
			{
				this.clothSim = null;
			}
			this.isClothSimOn = false;
		}
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x0010CD20 File Offset: 0x0010AF20
	public void ClothSimOn(bool _on, bool _force = false)
	{
		if (this.clothSim == null || (this.isClothSimOn == _on && !_force))
		{
			return;
		}
		bool flag = this.entity is EntityPlayerLocal;
		this.isClothSimOn = _on;
		for (int i = this.clothSim.Length - 1; i >= 0; i--)
		{
			Cloth cloth = this.clothSim[i];
			cloth.gameObject.SetActive(_on);
			if (_on && flag)
			{
				cloth.worldAccelerationScale = 0.3f;
			}
		}
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x0010CD94 File Offset: 0x0010AF94
	public void ClearClothMotion()
	{
		for (int i = this.clothSim.Length - 1; i >= 0; i--)
		{
			this.clothSim[i].ClearTransformMotion();
		}
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x0010CDC4 File Offset: 0x0010AFC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void JiggleInit()
	{
		this.jiggles = base.GetComponentsInChildren<Jiggle>();
		if (GameManager.IsDedicatedServer && this.jiggles != null)
		{
			for (int i = this.jiggles.Length - 1; i >= 0; i--)
			{
				this.jiggles[i].gameObject.SetActive(false);
			}
			this.jiggles = null;
		}
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x0010CE1C File Offset: 0x0010B01C
	public void JiggleOn(bool _on)
	{
		if (this.jiggles == null || this.isJiggleOn == _on)
		{
			return;
		}
		this.isJiggleOn = _on;
		for (int i = this.jiggles.Length - 1; i >= 0; i--)
		{
			this.jiggles[i].gameObject.SetActive(_on);
		}
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x0010CE6C File Offset: 0x0010B06C
	public void LogJiggles()
	{
		if (this.jiggles == null || this.jiggles.Length == 0)
		{
			return;
		}
		Log.Out(string.Format("{0}_{1} jiggle count: {2}", this.entity.GetDebugName(), this.entity.entityId, this.jiggles.Length));
		for (int i = 0; i < this.jiggles.Length; i++)
		{
			Jiggle jiggle = this.jiggles[i];
			Log.Out(string.Format("\t{0} {1}", i, jiggle.DebugString()));
		}
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x0010CEFC File Offset: 0x0010B0FC
	public virtual void SwitchModelAndView(bool _bFPV, bool _bMale)
	{
		if (this.GetModelTransform() != null)
		{
			Animator component = this.GetModelTransform().GetComponent<Animator>();
			if (component != null)
			{
				component.enabled = !_bFPV;
			}
		}
		this.IsFPV = _bFPV;
		if (this.modelName != null && this.modelTransformParent != null)
		{
			this.modelTransform = this.modelTransformParent.Find(this.modelName);
			this.meshTransform = GameUtils.FindTagInDirectChilds(this.modelTransform, "E_Mesh");
			if (!this.meshTransform)
			{
				this.meshTransform = this.modelTransform.Find("LOD0");
				if (!this.meshTransform)
				{
					Renderer componentInChildren = this.modelTransform.GetComponentInChildren<Renderer>();
					if (componentInChildren)
					{
						this.meshTransform = componentInChildren.transform;
					}
				}
			}
		}
		Transform transform = this.GetModelTransform();
		if (this.avatarController)
		{
			this.avatarController.SwitchModelAndView(this.modelName, _bFPV, _bMale);
		}
		else if (transform)
		{
			transform.gameObject.SetActive(true);
		}
		this.headTransform = GameUtils.FindTagInChilds(transform, "E_BP_Head");
		if (this.headTransform)
		{
			this.neckTransform = this.headTransform.parent;
			this.neckParentTransform = this.neckTransform.parent;
		}
		this.bipedRootTransform = GameUtils.FindTagInChilds(transform, "E_BP_BipedRoot");
		if (this.bipedRootTransform == null)
		{
			foreach (string text in EModelBase.commonBips)
			{
				if ((this.bipedRootTransform = transform.Find(text)) != null || (this.bipedRootTransform = transform.FindInChilds(text, false)) != null)
				{
					break;
				}
			}
		}
		if (this.bipedRootTransform != null)
		{
			if (this.bipedRootTransform.name != "pelvis" && this.bipedRootTransform.name != "Hips")
			{
				this.bipedPelvisTransform = GameUtils.FindChildWithPartialName(this.bipedRootTransform, new string[] { "pelvis" });
				if (this.bipedPelvisTransform == null)
				{
					this.bipedPelvisTransform = GameUtils.FindChildWithPartialName(this.bipedRootTransform, new string[] { "hips" });
					if (this.bipedPelvisTransform == null)
					{
						this.bipedPelvisTransform = GameUtils.FindChildWithPartialName(this.bipedRootTransform, new string[] { "hip" });
					}
				}
			}
			else
			{
				this.bipedPelvisTransform = this.bipedRootTransform;
			}
		}
		if (this.bipedPelvisTransform)
		{
			this.pelvisRB = this.bipedPelvisTransform.GetComponent<Rigidbody>();
			if (!(this.entity is EntityPlayer))
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.modelTransformParent.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (skinnedMeshRenderer.quality == SkinQuality.Auto)
					{
						skinnedMeshRenderer.quality = SkinQuality.Bone2;
					}
					Bounds localBounds = skinnedMeshRenderer.localBounds;
					if (skinnedMeshRenderer.CompareTag("E_BP_Eye"))
					{
						if (this.headTransform)
						{
							skinnedMeshRenderer.rootBone = this.headTransform;
							localBounds.center = Vector3.zero;
							localBounds.extents = new Vector3(0.25f, 0.25f, 0.25f);
						}
						skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
					}
					else if (skinnedMeshRenderer.rootBone != this.bipedPelvisTransform)
					{
						skinnedMeshRenderer.rootBone = this.bipedPelvisTransform;
						localBounds.center += -this.bipedPelvisTransform.localPosition;
					}
					skinnedMeshRenderer.localBounds = localBounds;
				}
			}
		}
		EntityAlive entityAlive = this.entity as EntityAlive;
		if (entityAlive && entityAlive.inventory != null)
		{
			if (this.entity.isEntityRemote)
			{
				entityAlive.inventory.ForceHoldingItemUpdate();
			}
			else if (this.entity is EntityPlayerLocal && (this.entity as EntityPlayerLocal).PlayerUI != null && !(this.entity as EntityPlayerLocal).PlayerUI.windowManager.IsWindowOpen("character"))
			{
				entityAlive.inventory.ForceHoldingItemUpdate();
			}
		}
		if (this.GetRightHandTransform() != null)
		{
			SkinnedMeshRenderer[] componentsInChildren2 = this.GetRightHandTransform().GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				componentsInChildren2[k].updateWhenOffscreen = true;
			}
		}
		this.NavObjectTransform = base.transform.FindInChilds("IconTag", false);
		PhysicsBodyLayout physicsBodyLayout = EntityClass.list[this.entity.entityClass].PhysicsBody;
		if (physicsBodyLayout != null && this.bipedRootTransform != null)
		{
			if (this.physicsBody != null)
			{
				this.physicsBody.SetColliderMode(EnumColliderType.All, EnumColliderMode.Disabled);
			}
			this.physicsBody = new PhysicsBodyInstance(this.bipedRootTransform, physicsBodyLayout, EnumColliderMode.Collision);
			this.physicsBody.SetColliderMode(EnumColliderType.All, EnumColliderMode.Collision);
			RagdollWhenHit[] componentsInChildren3 = this.bipedRootTransform.GetComponentsInChildren<RagdollWhenHit>();
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
		}
		else
		{
			this.physicsBody = null;
		}
		if (transform != null)
		{
			Animator component2 = transform.GetComponent<Animator>();
			if (component2 != null)
			{
				component2.enabled = !this.IsFPV;
			}
			this.SetColliderLayers(transform, 0);
		}
		this.CheckAnimFreeze();
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x0010D45C File Offset: 0x0010B65C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetColliderLayers(Transform modelT, int layer)
	{
		CapsuleCollider[] componentsInChildren = modelT.GetComponentsInChildren<CapsuleCollider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			GameObject gameObject = componentsInChildren[i].gameObject;
			if (!gameObject.CompareTag("LargeEntityBlocker") && !gameObject.CompareTag("Physics"))
			{
				gameObject.layer = layer;
			}
		}
		BoxCollider[] componentsInChildren2 = modelT.GetComponentsInChildren<BoxCollider>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].gameObject.layer = layer;
		}
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x0010D4D0 File Offset: 0x0010B6D0
	public virtual void SetInRightHand(Transform _transform)
	{
		if (this.avatarController != null)
		{
			this.avatarController.SetInRightHand(_transform);
		}
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x0010D4EC File Offset: 0x0010B6EC
	public virtual void SetAlive()
	{
		this.DisableRagdoll(true);
		Transform transform = this.GetModelTransform();
		if (transform)
		{
			Utils.MoveTaggedToLayer(transform.gameObject, "LargeEntityBlocker", 19);
			Animator component = transform.GetComponent<Animator>();
			if (component != null)
			{
				component.enabled = !this.IsFPV;
			}
		}
		if (this.avatarController != null)
		{
			this.avatarController.SetAlive();
		}
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x0010D55C File Offset: 0x0010B75C
	public virtual void SetDead()
	{
		Transform transform = this.GetModelTransform();
		if (transform)
		{
			Utils.MoveTaggedToLayer(transform.gameObject, "LargeEntityBlocker", 17);
		}
		if (this.ragdollState == EModelBase.ERagdollState.On)
		{
			this.SetRagdollDead();
			if (this.physicsBody != null && this.physicsBody.Mode != EnumColliderMode.RagdollDead)
			{
				this.physicsBody.SetColliderMode(EnumColliderType.All, EnumColliderMode.RagdollDead);
			}
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06002A9A RID: 10906 RVA: 0x0010D5BC File Offset: 0x0010B7BC
	// (set) Token: 0x06002A9B RID: 10907 RVA: 0x0010D5C4 File Offset: 0x0010B7C4
	public bool visible
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x0010D5D0 File Offset: 0x0010B7D0
	public virtual void SetVisible(bool _bVisible, bool _isKeepColliders = false)
	{
		this.visible = _bVisible;
		if (_isKeepColliders)
		{
			this.modelTransformParent.GetComponentsInChildren<Renderer>(EModelBase.rendererList);
			for (int i = 0; i < EModelBase.rendererList.Count; i++)
			{
				EModelBase.rendererList[i].enabled = _bVisible;
			}
			EModelBase.rendererList.Clear();
			if (!_bVisible)
			{
				return;
			}
		}
		if (this.avatarController != null)
		{
			this.avatarController.SetVisible(_bVisible);
		}
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x0010D648 File Offset: 0x0010B848
	public void SetFade(float _fade)
	{
		if (this.matPropBlock == null)
		{
			this.matPropBlock = new MaterialPropertyBlock();
		}
		this.modelTransformParent.GetComponentsInChildren<Renderer>(EModelBase.rendererList);
		for (int i = 0; i < EModelBase.rendererList.Count; i++)
		{
			Renderer renderer = EModelBase.rendererList[i];
			bool flag = false;
			Material material = renderer.material;
			string name = material.shader.name;
			if (material.HasProperty("_Fade") && name.Contains("Game/Character"))
			{
				flag = true;
			}
			if (renderer.gameObject.CompareTag("LOD") || renderer.gameObject.CompareTag("E_Mesh") || flag)
			{
				this.matPropBlock.SetFloat(EModelBase.fadeId, _fade);
				renderer.SetPropertyBlock(this.matPropBlock);
			}
		}
		EModelBase.rendererList.Clear();
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x0010D71E File Offset: 0x0010B91E
	public virtual Transform GetRightHandTransform()
	{
		if (this.avatarController != null)
		{
			return this.avatarController.GetRightHandTransform();
		}
		return null;
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetSkinTexture(string _textureName)
	{
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x0010D73C File Offset: 0x0010B93C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createModel(World _world, EntityClass _ec)
	{
		if (this.modelTransformParent == null)
		{
			return;
		}
		Transform transform = null;
		if (_ec.IsPrefabCombined && this.modelTransformParent.childCount > 0)
		{
			transform = this.modelTransformParent.GetChild(0);
		}
		if (this.assets.Mesh)
		{
			transform = global::UnityEngine.Object.Instantiate<Transform>(this.assets.Mesh, this.modelTransformParent, false);
			transform.name = this.assets.Mesh.name;
			Vector3 localPosition = transform.localPosition;
			if ((double)localPosition.z < -0.5 || (double)localPosition.z > 0.5)
			{
				Log.Warning("createModel mesh moved {0} {1} {2}", new object[]
				{
					transform.name,
					localPosition.ToString("f3"),
					transform.localRotation
				});
			}
			localPosition.x = 0f;
			localPosition.y = 0f;
			transform.localPosition = localPosition;
		}
		if (transform)
		{
			transform.gameObject.SetActive(true);
			this.modelName = transform.name;
			if (_ec.particleOnSpawn.fileName != null)
			{
				ParticleSystem particleSystem = DataLoader.LoadAsset<ParticleSystem>(_ec.particleOnSpawn.fileName, false);
				if (particleSystem != null)
				{
					ParticleSystem particleSystem2 = global::UnityEngine.Object.Instantiate<ParticleSystem>(particleSystem);
					particleSystem2.transform.parent = this.modelTransformParent;
					if (_ec.particleOnSpawn.shapeMesh != null && _ec.particleOnSpawn.shapeMesh.Length > 0)
					{
						SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>();
						ParticleSystem.ShapeModule shapeModule = particleSystem2.shape;
						shapeModule.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
						string text = _ec.particleOnSpawn.shapeMesh.ToLower();
						if (text.Contains("setshapetomesh"))
						{
							text = text.Replace("setshapetomesh", "");
							int num = int.Parse(text);
							if (num >= 0 && num < componentsInChildren.Length)
							{
								shapeModule.skinnedMeshRenderer = componentsInChildren[num];
								ParticleSystem[] componentsInChildren2 = particleSystem2.transform.GetComponentsInChildren<ParticleSystem>();
								if (componentsInChildren2 != null)
								{
									for (int i = 0; i < componentsInChildren2.Length; i++)
									{
										shapeModule = componentsInChildren2[i].shape;
										shapeModule.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
										shapeModule.skinnedMeshRenderer = componentsInChildren[num];
									}
								}
							}
						}
					}
				}
			}
			bool flag = false;
			this.AltMaterial = this.assets.AltMaterial;
			if (this.AltMaterial != null)
			{
				flag = true;
			}
			Color color = Color.black;
			string @string = _ec.Properties.GetString(EntityClass.PropMatColor);
			if (@string.Length > 0)
			{
				color = EntityClass.sColors[@string];
				flag = true;
			}
			if (flag)
			{
				base.GetComponentsInChildren<SkinnedMeshRenderer>(true, EModelBase.skinnedRendererList);
				Material material = this.AltMaterial;
				if (@string.Length > 0)
				{
					if (!material)
					{
						for (int j = 0; j < EModelBase.skinnedRendererList.Count; j++)
						{
							SkinnedMeshRenderer skinnedMeshRenderer = EModelBase.skinnedRendererList[j];
							if (skinnedMeshRenderer.CompareTag("LOD"))
							{
								material = skinnedMeshRenderer.sharedMaterials[0];
								break;
							}
						}
					}
					if (@string.Length > 0)
					{
						material = global::UnityEngine.Object.Instantiate<Material>(material);
						material.SetColor("_EmissiveColor", color);
					}
					this.AltMaterial = material;
				}
				for (int k = 0; k < EModelBase.skinnedRendererList.Count; k++)
				{
					SkinnedMeshRenderer skinnedMeshRenderer2 = EModelBase.skinnedRendererList[k];
					if (skinnedMeshRenderer2.CompareTag("LOD"))
					{
						Material[] sharedMaterials = skinnedMeshRenderer2.sharedMaterials;
						sharedMaterials[0] = material;
						skinnedMeshRenderer2.materials = sharedMaterials;
					}
				}
				EModelBase.skinnedRendererList.Clear();
			}
			if (_ec.MatSwap != null)
			{
				foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
				{
					if (renderer.CompareTag("LOD"))
					{
						Material[] sharedMaterials2 = renderer.sharedMaterials;
						int num2 = Utils.FastMin(sharedMaterials2.Length, _ec.MatSwap.Length);
						for (int m = 0; m < num2; m++)
						{
							string text2 = _ec.MatSwap[m];
							if (text2 != null && text2.Length > 0)
							{
								Material material2 = DataLoader.LoadAsset<Material>(text2, false);
								if (material2)
								{
									sharedMaterials2[m] = material2;
								}
							}
						}
						renderer.materials = sharedMaterials2;
					}
				}
			}
		}
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x0010DB78 File Offset: 0x0010BD78
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createAvatarController(EntityClass _ec)
	{
		string text = ((this.entity is EntityPlayerLocal) ? EntityClass.PropLocalAvatarController : EntityClass.PropAvatarController);
		text = _ec.Properties.GetString(text);
		if (text.Length > 0)
		{
			Type type = Type.GetType(text);
			this.avatarController = base.gameObject.GetComponent(type) as AvatarController;
			if (!this.avatarController)
			{
				this.avatarController = base.gameObject.AddComponent(type) as AvatarController;
			}
		}
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x0010DBF8 File Offset: 0x0010BDF8
	public virtual void Detach()
	{
		for (int i = 0; i < this.modelTransformParent.childCount; i++)
		{
			global::UnityEngine.Object.Destroy(this.modelTransformParent.GetChild(i).gameObject);
		}
		global::UnityEngine.Object.Destroy(this.avatarController);
		global::UnityEngine.Object.Destroy(this);
		this.avatarController = null;
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x0010DC4C File Offset: 0x0010BE4C
	[Conditional("DEBUG_RAGDOLL")]
	public void LogRagdoll(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Ragdoll {1}, id{2}, {3}, {4}", new object[]
		{
			GameManager.frameCount,
			this.entity.GetDebugName(),
			this.entity.entityId,
			this.ragdollState,
			_format
		});
		Log.Warning(_format, _args);
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x0010DCB4 File Offset: 0x0010BEB4
	[Conditional("DEBUG_RAGDOLLDO")]
	public void LogRagdollDo(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Ragdoll Do {1}, id{2}, {3}, time {4}, {5}", new object[]
		{
			GameManager.frameCount,
			this.entity.GetDebugName(),
			this.entity.entityId,
			this.ragdollState,
			this.ragdollTime,
			_format
		});
		Log.Warning(_format, _args);
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x0010DD28 File Offset: 0x0010BF28
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckAnimFreeze()
	{
		if (EAIManager.isAnimFreeze)
		{
			EntityAlive entityAlive = this.entity as EntityAlive;
			if (entityAlive && entityAlive.aiManager != null && this.avatarController)
			{
				Animator animator = this.avatarController.GetAnimator();
				if (animator)
				{
					animator.enabled = false;
				}
			}
		}
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x0010DD80 File Offset: 0x0010BF80
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateHeadState()
	{
		if (this.headTransform == null)
		{
			return;
		}
		switch (this.HeadState)
		{
		case EModelBase.HeadStates.Standard:
			if (this.headTransform.localScale.x != this.HeadStandardSize)
			{
				this.headTransform.localScale = new Vector3(this.HeadStandardSize, this.HeadStandardSize, this.HeadStandardSize);
				return;
			}
			break;
		case EModelBase.HeadStates.Growing:
		{
			float num = this.headTransform.localScale.y + this.headScaleSpeed * Time.deltaTime;
			if (num >= this.HeadBigSize)
			{
				num = this.HeadBigSize;
				this.HeadState = EModelBase.HeadStates.BigHead;
				EntityAlive entityAlive = this.entity as EntityAlive;
				if (entityAlive != null)
				{
					entityAlive.CurrentHeadState = this.HeadState;
				}
			}
			this.headTransform.localScale = new Vector3(num, num, num);
			return;
		}
		case EModelBase.HeadStates.BigHead:
			if (this.headTransform.localScale.x != this.HeadBigSize)
			{
				this.headTransform.localScale = new Vector3(this.HeadBigSize, this.HeadBigSize, this.HeadBigSize);
			}
			break;
		case EModelBase.HeadStates.Shrinking:
		{
			float num2 = this.headTransform.localScale.y - this.headScaleSpeed * Time.deltaTime;
			if (num2 <= this.HeadStandardSize)
			{
				num2 = this.HeadStandardSize;
				this.HeadState = EModelBase.HeadStates.Standard;
				EntityAlive entityAlive2 = this.entity as EntityAlive;
				if (entityAlive2 != null)
				{
					entityAlive2.CurrentHeadState = this.HeadState;
				}
			}
			this.headTransform.localScale = new Vector3(num2, num2, num2);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x0010DF08 File Offset: 0x0010C108
	public void ForceHeadState(EModelBase.HeadStates headState)
	{
		if (this.headTransform == null)
		{
			return;
		}
		this.HeadState = headState;
		if (headState != EModelBase.HeadStates.Standard)
		{
			if (headState == EModelBase.HeadStates.BigHead)
			{
				float headBigSize = this.HeadBigSize;
				this.headTransform.localScale = new Vector3(headBigSize, headBigSize, headBigSize);
				return;
			}
		}
		else
		{
			float headStandardSize = this.HeadStandardSize;
			this.headTransform.localScale = new Vector3(headStandardSize, headStandardSize, headStandardSize);
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x0010DF67 File Offset: 0x0010C167
	public void SetHeadScale(float standard)
	{
		this.HeadStandardSize = standard;
		this.HeadBigSize = Mathf.Min(4.5f, standard * 3f);
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x0010DF88 File Offset: 0x0010C188
	[PublicizedFrom(EAccessModifier.Protected)]
	public EModelBase()
	{
	}

	// Token: 0x04002035 RID: 8245
	public const string ExtFirstPerson = "_FP";

	// Token: 0x04002036 RID: 8246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cEmissiveColor = "_EmissiveColor";

	// Token: 0x04002037 RID: 8247
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EModelInstanceAssets assets;

	// Token: 0x04002038 RID: 8248
	public AvatarController avatarController;

	// Token: 0x04002039 RID: 8249
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform modelTransformParent;

	// Token: 0x0400203A RID: 8250
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform modelTransform;

	// Token: 0x0400203B RID: 8251
	public Transform meshTransform;

	// Token: 0x0400203C RID: 8252
	public Transform bipedRootTransform;

	// Token: 0x0400203D RID: 8253
	public Transform bipedPelvisTransform;

	// Token: 0x0400203E RID: 8254
	public Rigidbody pelvisRB;

	// Token: 0x0400203F RID: 8255
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform headTransform;

	// Token: 0x04002040 RID: 8256
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform neckTransform;

	// Token: 0x04002041 RID: 8257
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform neckParentTransform;

	// Token: 0x04002042 RID: 8258
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CharacterGazeController gazeController;

	// Token: 0x04002043 RID: 8259
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PhysicsBodyInstance physicsBody;

	// Token: 0x04002044 RID: 8260
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string modelName;

	// Token: 0x04002045 RID: 8261
	public Material AltMaterial;

	// Token: 0x04002046 RID: 8262
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float ragdollChance;

	// Token: 0x04002047 RID: 8263
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bHasRagdoll;

	// Token: 0x04002048 RID: 8264
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Cloth[] clothSim;

	// Token: 0x04002049 RID: 8265
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isClothSimOn = true;

	// Token: 0x0400204A RID: 8266
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Jiggle[] jiggles;

	// Token: 0x0400204B RID: 8267
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isJiggleOn = true;

	// Token: 0x0400204D RID: 8269
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity entity;

	// Token: 0x0400204E RID: 8270
	public EModelBase.HeadStates HeadState;

	// Token: 0x0400204F RID: 8271
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float headScaleSpeed = 2f;

	// Token: 0x04002050 RID: 8272
	public float HeadStandardSize = 1f;

	// Token: 0x04002051 RID: 8273
	public float HeadBigSize = 3f;

	// Token: 0x04002052 RID: 8274
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform NavObjectTransform;

	// Token: 0x04002053 RID: 8275
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float ragdollAlignmentForce = 25f;

	// Token: 0x04002054 RID: 8276
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float ragdollAlingmentDistance = 0.2f;

	// Token: 0x04002055 RID: 8277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const float ragdollBlendPositionSpeed = 10f;

	// Token: 0x04002056 RID: 8278
	public static float serverPosSpringForce = 50f;

	// Token: 0x04002057 RID: 8279
	public static float serverPosSpringDamping = 0.1f;

	// Token: 0x04002058 RID: 8280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<Renderer> rendererList = new List<Renderer>();

	// Token: 0x04002059 RID: 8281
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<SkinnedMeshRenderer> skinnedRendererList = new List<SkinnedMeshRenderer>();

	// Token: 0x0400205A RID: 8282
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLookAtSlerpPer = 0.16f;

	// Token: 0x0400205B RID: 8283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLookAtAnimBlend = 0.5f;

	// Token: 0x0400205C RID: 8284
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool lookAtEnabled;

	// Token: 0x0400205D RID: 8285
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lookAtMaxAngle;

	// Token: 0x0400205E RID: 8286
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lookAtIsPos;

	// Token: 0x0400205F RID: 8287
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 lookAtPos;

	// Token: 0x04002060 RID: 8288
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion lookAtRot;

	// Token: 0x04002061 RID: 8289
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lookAtBlendPer;

	// Token: 0x04002062 RID: 8290
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lookAtBlendPerTarget;

	// Token: 0x04002063 RID: 8291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lookAtFullChangeTime;

	// Token: 0x04002064 RID: 8292
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lookAtFullBlendPer = 1f;

	// Token: 0x04002065 RID: 8293
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cRadgollBlendOutGroundTime = 0.25f;

	// Token: 0x04002066 RID: 8294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cRadgollBlendOutTime = 0.7f;

	// Token: 0x04002067 RID: 8295
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cRagdollMinDisableVel = 0.5f;

	// Token: 0x04002068 RID: 8296
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cRagdollDeadMaxDepentrationVel = 2f;

	// Token: 0x04002069 RID: 8297
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cRagdollDeadMaxAngularVel = 1f;

	// Token: 0x0400206A RID: 8298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cRadgollPlayerAnimLayer = 5;

	// Token: 0x0400206B RID: 8299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EModelBase.ERagdollState ragdollState;

	// Token: 0x0400206C RID: 8300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float ragdollTime;

	// Token: 0x0400206D RID: 8301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float ragdollDuration;

	// Token: 0x0400206E RID: 8302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator ragdollAnimator;

	// Token: 0x0400206F RID: 8303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float ragdollRotY;

	// Token: 0x04002070 RID: 8304
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool ragdollIsBlending;

	// Token: 0x04002071 RID: 8305
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float ragdollAdjustPosDelay;

	// Token: 0x04002072 RID: 8306
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool ragdollIsPlayer;

	// Token: 0x04002073 RID: 8307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool ragdollIsAnimal;

	// Token: 0x04002074 RID: 8308
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool ragdollIsFacingUp;

	// Token: 0x04002075 RID: 8309
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<EModelBase.RagdollPose> ragdollPoses = new List<EModelBase.RagdollPose>();

	// Token: 0x04002076 RID: 8310
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 ragdollPosePelvisPos;

	// Token: 0x04002077 RID: 8311
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 ragdollPosePelvisLocalPos;

	// Token: 0x04002078 RID: 8312
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Rigidbody> ragdollTempRBs = new List<Rigidbody>();

	// Token: 0x04002079 RID: 8313
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float ragdollZeroTime;

	// Token: 0x0400207A RID: 8314
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Transform> ragdollZeroBones;

	// Token: 0x0400207B RID: 8315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string[] commonBips = new string[] { "Bip001", "Bip01" };

	// Token: 0x0400207D RID: 8317
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MaterialPropertyBlock matPropBlock;

	// Token: 0x0400207E RID: 8318
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int fadeId = Shader.PropertyToID("_Fade");

	// Token: 0x0200050B RID: 1291
	public enum HeadStates
	{
		// Token: 0x04002080 RID: 8320
		Standard,
		// Token: 0x04002081 RID: 8321
		Growing,
		// Token: 0x04002082 RID: 8322
		BigHead,
		// Token: 0x04002083 RID: 8323
		Shrinking
	}

	// Token: 0x0200050C RID: 1292
	public enum RagdollMode
	{
		// Token: 0x04002085 RID: 8325
		Default,
		// Token: 0x04002086 RID: 8326
		FullForce
	}

	// Token: 0x0200050D RID: 1293
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ERagdollState
	{
		// Token: 0x04002088 RID: 8328
		Off,
		// Token: 0x04002089 RID: 8329
		On,
		// Token: 0x0400208A RID: 8330
		BlendOutGround,
		// Token: 0x0400208B RID: 8331
		BlendOutStand,
		// Token: 0x0400208C RID: 8332
		Stand,
		// Token: 0x0400208D RID: 8333
		StandCollide,
		// Token: 0x0400208E RID: 8334
		SpawnWait,
		// Token: 0x0400208F RID: 8335
		Dead
	}

	// Token: 0x0200050E RID: 1294
	public struct RagdollPose
	{
		// Token: 0x04002090 RID: 8336
		public Transform t;

		// Token: 0x04002091 RID: 8337
		public Rigidbody rb;

		// Token: 0x04002092 RID: 8338
		public Quaternion startRot;

		// Token: 0x04002093 RID: 8339
		public Quaternion rot;
	}
}

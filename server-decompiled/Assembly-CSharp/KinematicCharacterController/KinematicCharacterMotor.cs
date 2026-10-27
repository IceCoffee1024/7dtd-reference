using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DBB RID: 7611
	[RequireComponent(typeof(CapsuleCollider))]
	public class KinematicCharacterMotor : MonoBehaviour
	{
		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x0600E05F RID: 57439 RVA: 0x004FE425 File Offset: 0x004FC625
		public Transform Transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x0600E060 RID: 57440 RVA: 0x004FE42D File Offset: 0x004FC62D
		public Vector3 TransientPosition
		{
			get
			{
				return this._transientPosition;
			}
		}

		// Token: 0x17001C97 RID: 7319
		// (get) Token: 0x0600E061 RID: 57441 RVA: 0x004FE435 File Offset: 0x004FC635
		public Vector3 CharacterUp
		{
			get
			{
				return this._characterUp;
			}
		}

		// Token: 0x17001C98 RID: 7320
		// (get) Token: 0x0600E062 RID: 57442 RVA: 0x004FE43D File Offset: 0x004FC63D
		public Vector3 CharacterForward
		{
			get
			{
				return this._characterForward;
			}
		}

		// Token: 0x17001C99 RID: 7321
		// (get) Token: 0x0600E063 RID: 57443 RVA: 0x004FE445 File Offset: 0x004FC645
		public Vector3 CharacterRight
		{
			get
			{
				return this._characterRight;
			}
		}

		// Token: 0x17001C9A RID: 7322
		// (get) Token: 0x0600E064 RID: 57444 RVA: 0x004FE44D File Offset: 0x004FC64D
		public Vector3 InitialSimulationPosition
		{
			get
			{
				return this._initialSimulationPosition;
			}
		}

		// Token: 0x17001C9B RID: 7323
		// (get) Token: 0x0600E065 RID: 57445 RVA: 0x004FE455 File Offset: 0x004FC655
		public Quaternion InitialSimulationRotation
		{
			get
			{
				return this._initialSimulationRotation;
			}
		}

		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x0600E066 RID: 57446 RVA: 0x004FE45D File Offset: 0x004FC65D
		public Rigidbody AttachedRigidbody
		{
			get
			{
				return this._attachedRigidbody;
			}
		}

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x0600E067 RID: 57447 RVA: 0x004FE465 File Offset: 0x004FC665
		public Vector3 CharacterTransformToCapsuleCenter
		{
			get
			{
				return this._characterTransformToCapsuleCenter;
			}
		}

		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x0600E068 RID: 57448 RVA: 0x004FE46D File Offset: 0x004FC66D
		public Vector3 CharacterTransformToCapsuleBottom
		{
			get
			{
				return this._characterTransformToCapsuleBottom;
			}
		}

		// Token: 0x17001C9F RID: 7327
		// (get) Token: 0x0600E069 RID: 57449 RVA: 0x004FE475 File Offset: 0x004FC675
		public Vector3 CharacterTransformToCapsuleTop
		{
			get
			{
				return this._characterTransformToCapsuleTop;
			}
		}

		// Token: 0x17001CA0 RID: 7328
		// (get) Token: 0x0600E06A RID: 57450 RVA: 0x004FE47D File Offset: 0x004FC67D
		public Vector3 CharacterTransformToCapsuleBottomHemi
		{
			get
			{
				return this._characterTransformToCapsuleBottomHemi;
			}
		}

		// Token: 0x17001CA1 RID: 7329
		// (get) Token: 0x0600E06B RID: 57451 RVA: 0x004FE485 File Offset: 0x004FC685
		public Vector3 CharacterTransformToCapsuleTopHemi
		{
			get
			{
				return this._characterTransformToCapsuleTopHemi;
			}
		}

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x0600E06C RID: 57452 RVA: 0x004FE48D File Offset: 0x004FC68D
		public Vector3 AttachedRigidbodyVelocity
		{
			get
			{
				return this._attachedRigidbodyVelocity;
			}
		}

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x0600E06D RID: 57453 RVA: 0x004FE495 File Offset: 0x004FC695
		public int OverlapsCount
		{
			get
			{
				return this._overlapsCount;
			}
		}

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x0600E06E RID: 57454 RVA: 0x004FE49D File Offset: 0x004FC69D
		public OverlapResult[] Overlaps
		{
			get
			{
				return this._overlaps;
			}
		}

		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x0600E06F RID: 57455 RVA: 0x004FE4A5 File Offset: 0x004FC6A5
		// (set) Token: 0x0600E070 RID: 57456 RVA: 0x004FE4B0 File Offset: 0x004FC6B0
		public Quaternion TransientRotation
		{
			get
			{
				return this._transientRotation;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this._transientRotation = value;
				this._characterUp = this._transientRotation * this._cachedWorldUp;
				this._characterForward = this._transientRotation * this._cachedWorldForward;
				this._characterRight = this._transientRotation * this._cachedWorldRight;
			}
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x0600E071 RID: 57457 RVA: 0x004FE509 File Offset: 0x004FC709
		public Vector3 Velocity
		{
			get
			{
				return this.BaseVelocity + this._attachedRigidbodyVelocity;
			}
		}

		// Token: 0x0600E072 RID: 57458 RVA: 0x004FE51C File Offset: 0x004FC71C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			KinematicCharacterSystem.EnsureCreation();
			KinematicCharacterSystem.RegisterCharacterMotor(this);
		}

		// Token: 0x0600E073 RID: 57459 RVA: 0x004FE529 File Offset: 0x004FC729
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			KinematicCharacterSystem.UnregisterCharacterMotor(this);
		}

		// Token: 0x0600E074 RID: 57460 RVA: 0x004FE531 File Offset: 0x004FC731
		[PublicizedFrom(EAccessModifier.Private)]
		public void Reset()
		{
			this.ValidateData();
		}

		// Token: 0x0600E075 RID: 57461 RVA: 0x004FE531 File Offset: 0x004FC731
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnValidate()
		{
			this.ValidateData();
		}

		// Token: 0x0600E076 RID: 57462 RVA: 0x004FE539 File Offset: 0x004FC739
		[ContextMenu("Remove Component")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleRemoveComponent()
		{
			global::UnityEngine.Object component = base.gameObject.GetComponent<CapsuleCollider>();
			global::UnityEngine.Object.DestroyImmediate(this);
			global::UnityEngine.Object.DestroyImmediate(component);
		}

		// Token: 0x0600E077 RID: 57463 RVA: 0x004FE554 File Offset: 0x004FC754
		public void ValidateData()
		{
			if (base.GetComponent<Rigidbody>())
			{
				base.GetComponent<Rigidbody>().hideFlags = HideFlags.None;
			}
			this.Capsule = base.GetComponent<CapsuleCollider>();
			this.CapsuleRadius = Mathf.Clamp(this.CapsuleRadius, 0f, this.CapsuleHeight * 0.5f);
			this.Capsule.isTrigger = false;
			this.Capsule.direction = 1;
			this.Capsule.sharedMaterial = this.CapsulePhysicsMaterial;
			this.SetCapsuleDimensions(this.CapsuleRadius, this.CapsuleHeight, this.CapsuleYOffset);
			this.MaxStepHeight = Mathf.Clamp(this.MaxStepHeight, 0f, float.PositiveInfinity);
			this.MinRequiredStepDepth = Mathf.Clamp(this.MinRequiredStepDepth, 0f, this.CapsuleRadius);
			this.MaxStableDistanceFromLedge = Mathf.Clamp(this.MaxStableDistanceFromLedge, 0f, this.CapsuleRadius);
			base.transform.localScale = Vector3.one;
		}

		// Token: 0x0600E078 RID: 57464 RVA: 0x004FE64C File Offset: 0x004FC84C
		public void SetCapsuleCollisionsActivation(bool collisionsActive)
		{
			this.Capsule.isTrigger = !collisionsActive;
		}

		// Token: 0x0600E079 RID: 57465 RVA: 0x004FE65D File Offset: 0x004FC85D
		public void SetMovementCollisionsSolvingActivation(bool movementCollisionsSolvingActive)
		{
			this._solveMovementCollisions = movementCollisionsSolvingActive;
		}

		// Token: 0x0600E07A RID: 57466 RVA: 0x004FE666 File Offset: 0x004FC866
		public void SetGroundSolvingActivation(bool stabilitySolvingActive)
		{
			this._solveGrounding = stabilitySolvingActive;
		}

		// Token: 0x0600E07B RID: 57467 RVA: 0x004FE66F File Offset: 0x004FC86F
		public void SetPosition(Vector3 position, bool bypassInterpolation = true)
		{
			this._transform.position = position;
			this._initialSimulationPosition = position;
			this._transientPosition = position;
			if (bypassInterpolation)
			{
				this.InitialTickPosition = position;
			}
		}

		// Token: 0x0600E07C RID: 57468 RVA: 0x004FE695 File Offset: 0x004FC895
		public void SetRotation(Quaternion rotation, bool bypassInterpolation = true)
		{
			this._transform.rotation = rotation;
			this._initialSimulationRotation = rotation;
			this.TransientRotation = rotation;
			if (bypassInterpolation)
			{
				this.InitialTickRotation = rotation;
			}
		}

		// Token: 0x0600E07D RID: 57469 RVA: 0x004FE6BB File Offset: 0x004FC8BB
		public void SetPositionAndRotation(Vector3 position, Quaternion rotation, bool bypassInterpolation = true)
		{
			this._transform.SetPositionAndRotation(position, rotation);
			this._initialSimulationPosition = position;
			this._initialSimulationRotation = rotation;
			this._transientPosition = position;
			this.TransientRotation = rotation;
			if (bypassInterpolation)
			{
				this.InitialTickPosition = position;
				this.InitialTickRotation = rotation;
			}
		}

		// Token: 0x0600E07E RID: 57470 RVA: 0x004FE6F7 File Offset: 0x004FC8F7
		public void MoveCharacter(Vector3 toPosition)
		{
			this._movePositionDirty = true;
			this._movePositionTarget = toPosition;
		}

		// Token: 0x0600E07F RID: 57471 RVA: 0x004FE707 File Offset: 0x004FC907
		public void RotateCharacter(Quaternion toRotation)
		{
			this._moveRotationDirty = true;
			this._moveRotationTarget = toRotation;
		}

		// Token: 0x0600E080 RID: 57472 RVA: 0x004FE718 File Offset: 0x004FC918
		public KinematicCharacterMotorState GetState()
		{
			KinematicCharacterMotorState kinematicCharacterMotorState = default(KinematicCharacterMotorState);
			kinematicCharacterMotorState.Position = this._transientPosition;
			kinematicCharacterMotorState.Rotation = this._transientRotation;
			kinematicCharacterMotorState.BaseVelocity = this.BaseVelocity;
			kinematicCharacterMotorState.AttachedRigidbodyVelocity = this._attachedRigidbodyVelocity;
			kinematicCharacterMotorState.MustUnground = this._mustUnground;
			kinematicCharacterMotorState.MustUngroundTime = this._mustUngroundTimeCounter;
			kinematicCharacterMotorState.LastMovementIterationFoundAnyGround = this.LastMovementIterationFoundAnyGround;
			kinematicCharacterMotorState.GroundingStatus.CopyFrom(this.GroundingStatus);
			kinematicCharacterMotorState.AttachedRigidbody = this._attachedRigidbody;
			return kinematicCharacterMotorState;
		}

		// Token: 0x0600E081 RID: 57473 RVA: 0x004FE7A8 File Offset: 0x004FC9A8
		public void ApplyState(KinematicCharacterMotorState state, bool bypassInterpolation = true)
		{
			this.SetPositionAndRotation(state.Position, state.Rotation, bypassInterpolation);
			this.BaseVelocity = state.BaseVelocity;
			this._attachedRigidbodyVelocity = state.AttachedRigidbodyVelocity;
			this._mustUnground = state.MustUnground;
			this._mustUngroundTimeCounter = state.MustUngroundTime;
			this.LastMovementIterationFoundAnyGround = state.LastMovementIterationFoundAnyGround;
			this.GroundingStatus.CopyFrom(state.GroundingStatus);
			this._attachedRigidbody = state.AttachedRigidbody;
		}

		// Token: 0x0600E082 RID: 57474 RVA: 0x004FE824 File Offset: 0x004FCA24
		public void SetCapsuleDimensions(float radius, float height, float yOffset)
		{
			this.CapsuleRadius = radius;
			this.CapsuleHeight = height;
			this.CapsuleYOffset = yOffset;
			this.Capsule.radius = this.CapsuleRadius;
			this.Capsule.height = Mathf.Clamp(this.CapsuleHeight, this.CapsuleRadius * 2f, this.CapsuleHeight);
			this.Capsule.center = new Vector3(0f, this.CapsuleYOffset, 0f);
			this._characterTransformToCapsuleCenter = this.Capsule.center;
			this._characterTransformToCapsuleBottom = this.Capsule.center + -this._cachedWorldUp * (this.Capsule.height * 0.5f);
			this._characterTransformToCapsuleTop = this.Capsule.center + this._cachedWorldUp * (this.Capsule.height * 0.5f);
			this._characterTransformToCapsuleBottomHemi = this.Capsule.center + -this._cachedWorldUp * (this.Capsule.height * 0.5f) + this._cachedWorldUp * this.Capsule.radius;
			this._characterTransformToCapsuleTopHemi = this.Capsule.center + this._cachedWorldUp * (this.Capsule.height * 0.5f) + -this._cachedWorldUp * this.Capsule.radius;
		}

		// Token: 0x0600E083 RID: 57475 RVA: 0x004FE9C0 File Offset: 0x004FCBC0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this._transform = base.transform;
			this.ValidateData();
			this._transientPosition = this._transform.position;
			this.TransientRotation = this._transform.rotation;
			this.CollidableLayers = 0;
			for (int i = 0; i < 32; i++)
			{
				if (!Physics.GetIgnoreLayerCollision(base.gameObject.layer, i))
				{
					this.CollidableLayers |= 1 << i;
				}
			}
			this.SetCapsuleDimensions(this.CapsuleRadius, this.CapsuleHeight, this.CapsuleYOffset);
		}

		// Token: 0x0600E084 RID: 57476 RVA: 0x004FEA64 File Offset: 0x004FCC64
		public void UpdatePhase1(float deltaTime)
		{
			if (float.IsNaN(this.BaseVelocity.x) || float.IsNaN(this.BaseVelocity.y) || float.IsNaN(this.BaseVelocity.z))
			{
				this.BaseVelocity = Vector3.zero;
			}
			if (float.IsNaN(this._attachedRigidbodyVelocity.x) || float.IsNaN(this._attachedRigidbodyVelocity.y) || float.IsNaN(this._attachedRigidbodyVelocity.z))
			{
				this._attachedRigidbodyVelocity = Vector3.zero;
			}
			this.CharacterController.BeforeCharacterUpdate(deltaTime);
			this._transientPosition = this._transform.position;
			this.TransientRotation = this._transform.rotation;
			this._initialSimulationPosition = this._transientPosition;
			this._initialSimulationRotation = this._transientRotation;
			this._rigidbodyProjectionHitCount = 0;
			this._overlapsCount = 0;
			this._lastSolvedOverlapNormalDirty = false;
			if (this._movePositionDirty)
			{
				if (this._solveMovementCollisions)
				{
					if (this.InternalCharacterMove(this._movePositionTarget - this._transientPosition, deltaTime, out this._internalResultingMovementMagnitude, out this._internalResultingMovementDirection) && this.InteractiveRigidbodyHandling)
					{
						Vector3 zero = Vector3.zero;
						this.ProcessVelocityForRigidbodyHits(ref zero, deltaTime);
					}
				}
				else
				{
					this._transientPosition = this._movePositionTarget;
				}
				this._movePositionDirty = false;
			}
			this.LastGroundingStatus.CopyFrom(this.GroundingStatus);
			this.GroundingStatus = default(CharacterGroundingReport);
			this.GroundingStatus.GroundNormal = this._characterUp;
			if (this._solveMovementCollisions)
			{
				Vector3 vector = this._cachedWorldUp;
				float num = 0f;
				int num2 = 0;
				bool flag = false;
				while (num2 < 3 && !flag)
				{
					int num3 = this.CharacterCollisionsOverlap(this._transientPosition, this._transientRotation, this._internalProbedColliders, 0f, false);
					if (num3 > 0)
					{
						if (!this.CharacterController.OnCollisionOverlap(num3, this._internalProbedColliders))
						{
							break;
						}
						int i = 0;
						while (i < num3)
						{
							Transform component = this._internalProbedColliders[i].GetComponent<Transform>();
							if (Physics.ComputePenetration(this.Capsule, this._transientPosition, this._transientRotation, this._internalProbedColliders[i], component.position, component.rotation, out vector, out num))
							{
								HitStabilityReport hitStabilityReport = new HitStabilityReport
								{
									IsStable = this.IsStableOnNormal(vector)
								};
								vector = this.GetObstructionNormal(vector, hitStabilityReport);
								num *= this.CharacterController.GetCollisionOverlapScale(component);
								Vector3 vector2 = vector * (num + 0.001f);
								this._transientPosition += vector2;
								if (this._overlapsCount < this._overlaps.Length)
								{
									this._overlaps[this._overlapsCount] = new OverlapResult(vector, this._internalProbedColliders[i]);
									this._overlapsCount++;
									break;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
					else
					{
						flag = true;
					}
					num2++;
				}
			}
			if (this._solveGrounding)
			{
				if (this.MustUnground())
				{
					this._transientPosition += this._characterUp * 0.0075f;
				}
				else
				{
					float num4 = 0.005f;
					if (!this.LastGroundingStatus.SnappingPrevented && (this.LastGroundingStatus.IsStableOnGround || this.LastMovementIterationFoundAnyGround))
					{
						if (this.StepHandling != StepHandlingMethod.None)
						{
							num4 = Mathf.Max(this.CapsuleRadius, this.MaxStepHeight);
						}
						else
						{
							num4 = this.CapsuleRadius;
						}
						num4 += this.GroundDetectionExtraDistance;
					}
					this.ProbeGround(ref this._transientPosition, this._transientRotation, num4, ref this.GroundingStatus);
				}
			}
			this.LastMovementIterationFoundAnyGround = false;
			if (this._mustUngroundTimeCounter > 0f)
			{
				this._mustUngroundTimeCounter -= deltaTime;
			}
			this._mustUnground = false;
			if (this._solveGrounding)
			{
				this.CharacterController.PostGroundingUpdate(deltaTime);
			}
			if (this.InteractiveRigidbodyHandling)
			{
				this._lastAttachedRigidbody = this._attachedRigidbody;
				if (this.AttachedRigidbodyOverride)
				{
					this._attachedRigidbody = this.AttachedRigidbodyOverride;
				}
				else if (this.GroundingStatus.IsStableOnGround && this.GroundingStatus.GroundCollider.attachedRigidbody)
				{
					Rigidbody interactiveRigidbody = this.GetInteractiveRigidbody(this.GroundingStatus.GroundCollider);
					if (interactiveRigidbody)
					{
						this._attachedRigidbody = interactiveRigidbody;
					}
				}
				else
				{
					this._attachedRigidbody = null;
				}
				Vector3 vector3 = Vector3.zero;
				if (this._attachedRigidbody)
				{
					vector3 = this.GetVelocityFromRigidbodyMovement(this._attachedRigidbody, this._transientPosition, deltaTime);
				}
				if (this.PreserveAttachedRigidbodyMomentum && this._lastAttachedRigidbody != null && this._attachedRigidbody != this._lastAttachedRigidbody)
				{
					this.BaseVelocity += this._attachedRigidbodyVelocity;
					this.BaseVelocity -= vector3;
				}
				this._attachedRigidbodyVelocity = this._cachedZeroVector;
				if (this._attachedRigidbody)
				{
					this._attachedRigidbodyVelocity = vector3;
					Vector3 normalized = Vector3.ProjectOnPlane(Quaternion.Euler(57.29578f * this._attachedRigidbody.angularVelocity * deltaTime) * this._characterForward, this._characterUp).normalized;
					this.TransientRotation = Quaternion.LookRotation(normalized, this._characterUp);
				}
				if (this.GroundingStatus.GroundCollider && this.GroundingStatus.GroundCollider.attachedRigidbody && this.GroundingStatus.GroundCollider.attachedRigidbody == this._attachedRigidbody && this._attachedRigidbody != null && this._lastAttachedRigidbody == null)
				{
					this.BaseVelocity -= Vector3.ProjectOnPlane(this._attachedRigidbodyVelocity, this._characterUp);
				}
				if (this._attachedRigidbodyVelocity.sqrMagnitude > 0f)
				{
					this._isMovingFromAttachedRigidbody = true;
					if (this._solveMovementCollisions)
					{
						if (this.InternalCharacterMove(this._attachedRigidbodyVelocity * deltaTime, deltaTime, out this._internalResultingMovementMagnitude, out this._internalResultingMovementDirection))
						{
							this._attachedRigidbodyVelocity = this._internalResultingMovementDirection * this._internalResultingMovementMagnitude / deltaTime;
						}
						else
						{
							this._attachedRigidbodyVelocity = Vector3.zero;
						}
					}
					else
					{
						this._transientPosition += this._attachedRigidbodyVelocity * deltaTime;
					}
					this._isMovingFromAttachedRigidbody = false;
				}
			}
		}

		// Token: 0x0600E085 RID: 57477 RVA: 0x004FF0B8 File Offset: 0x004FD2B8
		public void UpdatePhase2(float deltaTime)
		{
			this.CharacterController.UpdateRotation(ref this._transientRotation, deltaTime);
			this.TransientRotation = this._transientRotation;
			if (this._moveRotationDirty)
			{
				this.TransientRotation = this._moveRotationTarget;
				this._moveRotationDirty = false;
			}
			if (this._solveMovementCollisions && this.InteractiveRigidbodyHandling)
			{
				if (this.InteractiveRigidbodyHandling && this._attachedRigidbody)
				{
					float radius = this.Capsule.radius;
					RaycastHit raycastHit;
					if (this.CharacterGroundSweep(this._transientPosition + this._characterUp * radius, this._transientRotation, -this._characterUp, radius, out raycastHit) && raycastHit.collider.attachedRigidbody == this._attachedRigidbody && this.IsStableOnNormal(raycastHit.normal))
					{
						float num = radius - raycastHit.distance;
						this._transientPosition = this._transientPosition + this._characterUp * num + this._characterUp * 0.001f;
					}
				}
				if (this.InteractiveRigidbodyHandling)
				{
					Vector3 vector = this._cachedWorldUp;
					float num2 = 0f;
					int num3 = 0;
					bool flag = false;
					while (num3 < 3 && !flag)
					{
						int num4 = this.CharacterCollisionsOverlap(this._transientPosition, this._transientRotation, this._internalProbedColliders, 0f, false);
						if (num4 > 0)
						{
							int i = 0;
							while (i < num4)
							{
								Transform component = this._internalProbedColliders[i].GetComponent<Transform>();
								if (Physics.ComputePenetration(this.Capsule, this._transientPosition, this._transientRotation, this._internalProbedColliders[i], component.position, component.rotation, out vector, out num2))
								{
									HitStabilityReport hitStabilityReport = new HitStabilityReport
									{
										IsStable = this.IsStableOnNormal(vector)
									};
									vector = this.GetObstructionNormal(vector, hitStabilityReport);
									Vector3 vector2 = vector * (num2 + 0.001f);
									this._transientPosition += vector2;
									if (this.InteractiveRigidbodyHandling)
									{
										Rigidbody interactiveRigidbody = this.GetInteractiveRigidbody(this._internalProbedColliders[i]);
										if (interactiveRigidbody != null)
										{
											HitStabilityReport hitStabilityReport2 = new HitStabilityReport
											{
												IsStable = this.IsStableOnNormal(vector)
											};
											if (hitStabilityReport2.IsStable)
											{
												this.LastMovementIterationFoundAnyGround = hitStabilityReport2.IsStable;
											}
											if (interactiveRigidbody != this._attachedRigidbody)
											{
												Vector3 vector3 = this._transientPosition + this._transientRotation * this._characterTransformToCapsuleCenter;
												Vector3 transientPosition = this._transientPosition;
												MeshCollider meshCollider = this._internalProbedColliders[i] as MeshCollider;
												if (!meshCollider || meshCollider.convex)
												{
													Physics.ClosestPoint(vector3, this._internalProbedColliders[i], component.position, component.rotation);
												}
												this.StoreRigidbodyHit(interactiveRigidbody, this.Velocity, transientPosition, vector, hitStabilityReport2);
											}
										}
									}
									if (this._overlapsCount < this._overlaps.Length)
									{
										this._overlaps[this._overlapsCount] = new OverlapResult(vector, this._internalProbedColliders[i]);
										this._overlapsCount++;
										break;
									}
									break;
								}
								else
								{
									i++;
								}
							}
						}
						else
						{
							flag = true;
						}
						num3++;
					}
				}
			}
			this.CharacterController.UpdateVelocity(ref this.BaseVelocity, deltaTime);
			if (this.BaseVelocity.magnitude < 0.01f)
			{
				this.BaseVelocity = Vector3.zero;
			}
			if (this.BaseVelocity.sqrMagnitude > 0f)
			{
				if (this._solveMovementCollisions)
				{
					if (this.InternalCharacterMove(this.BaseVelocity * deltaTime, deltaTime, out this._internalResultingMovementMagnitude, out this._internalResultingMovementDirection))
					{
						this.BaseVelocity = this._internalResultingMovementDirection * this._internalResultingMovementMagnitude / deltaTime;
					}
					else
					{
						this.BaseVelocity = Vector3.zero;
					}
				}
				else
				{
					this._transientPosition += this.BaseVelocity * deltaTime;
				}
			}
			if (this.InteractiveRigidbodyHandling)
			{
				this.ProcessVelocityForRigidbodyHits(ref this.BaseVelocity, deltaTime);
			}
			if (this.HasPlanarConstraint)
			{
				this._transientPosition = this._initialSimulationPosition + Vector3.ProjectOnPlane(this._transientPosition - this._initialSimulationPosition, this.PlanarConstraintAxis.normalized);
			}
			if (this.DiscreteCollisionEvents)
			{
				int num5 = this.CharacterCollisionsOverlap(this._transientPosition, this._transientRotation, this._internalProbedColliders, 0.002f, false);
				for (int j = 0; j < num5; j++)
				{
					this.CharacterController.OnDiscreteCollisionDetected(this._internalProbedColliders[j]);
				}
			}
			this.CharacterController.AfterCharacterUpdate(deltaTime);
		}

		// Token: 0x0600E086 RID: 57478 RVA: 0x004FF54F File Offset: 0x004FD74F
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsStableOnNormal(Vector3 normal)
		{
			return Vector3.Angle(this._characterUp, normal) <= this.MaxStableSlopeAngle;
		}

		// Token: 0x0600E087 RID: 57479 RVA: 0x004FF568 File Offset: 0x004FD768
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsStableWithSpecialCases(ref HitStabilityReport stabilityReport, Vector3 velocity)
		{
			if (this.LedgeAndDenivelationHandling)
			{
				if (stabilityReport.LedgeDetected && stabilityReport.IsMovingTowardsEmptySideOfLedge)
				{
					if (velocity.magnitude >= this.MaxVelocityForLedgeSnap)
					{
						return false;
					}
					if (stabilityReport.IsOnEmptySideOfLedge && stabilityReport.DistanceFromLedge > this.MaxStableDistanceFromLedge)
					{
						return false;
					}
				}
				if (this.LastGroundingStatus.FoundAnyGround && stabilityReport.InnerNormal.sqrMagnitude != 0f && stabilityReport.OuterNormal.sqrMagnitude != 0f)
				{
					if (Vector3.Angle(stabilityReport.InnerNormal, stabilityReport.OuterNormal) > this.MaxStableDenivelationAngle)
					{
						return false;
					}
					if (Vector3.Angle(this.LastGroundingStatus.InnerGroundNormal, stabilityReport.OuterNormal) > this.MaxStableDenivelationAngle)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600E088 RID: 57480 RVA: 0x004FF628 File Offset: 0x004FD828
		public void ProbeGround(ref Vector3 probingPosition, Quaternion atRotation, float probingDistance, ref CharacterGroundingReport groundingReport)
		{
			if (probingDistance < 0.005f)
			{
				probingDistance = 0.005f;
			}
			int num = 0;
			RaycastHit raycastHit = default(RaycastHit);
			bool flag = false;
			Vector3 vector = probingPosition;
			Vector3 vector2 = atRotation * -this._cachedWorldUp;
			float num2 = probingDistance;
			while (num2 > 0f && num <= 2 && !flag)
			{
				if (this.CharacterGroundSweep(vector, atRotation, vector2, num2, out raycastHit))
				{
					Vector3 vector3 = vector + vector2 * raycastHit.distance;
					HitStabilityReport hitStabilityReport = default(HitStabilityReport);
					this.EvaluateHitStability(raycastHit.collider, raycastHit.normal, raycastHit.point, vector3, this._transientRotation, this.BaseVelocity, ref hitStabilityReport);
					groundingReport.FoundAnyGround = true;
					groundingReport.GroundNormal = raycastHit.normal;
					groundingReport.InnerGroundNormal = hitStabilityReport.InnerNormal;
					groundingReport.OuterGroundNormal = hitStabilityReport.OuterNormal;
					groundingReport.GroundCollider = raycastHit.collider;
					groundingReport.GroundPoint = raycastHit.point;
					groundingReport.SnappingPrevented = false;
					if (hitStabilityReport.IsStable)
					{
						groundingReport.SnappingPrevented = !this.IsStableWithSpecialCases(ref hitStabilityReport, this.BaseVelocity);
						groundingReport.IsStableOnGround = true;
						if (!groundingReport.SnappingPrevented)
						{
							vector3 += -vector2 * 0.001f;
							probingPosition = vector3;
						}
						this.CharacterController.OnGroundHit(raycastHit.collider, raycastHit.normal, raycastHit.point, ref hitStabilityReport);
						flag = true;
					}
					else
					{
						Vector3 vector4 = vector2 * raycastHit.distance + atRotation * this._cachedWorldUp * Mathf.Max(0.001f, raycastHit.distance);
						vector += vector4;
						num2 = Mathf.Min(0.02f, Mathf.Max(num2 - vector4.magnitude, 0f));
						vector2 = Vector3.ProjectOnPlane(vector2, raycastHit.normal).normalized;
					}
				}
				else
				{
					flag = true;
				}
				num++;
			}
		}

		// Token: 0x0600E089 RID: 57481 RVA: 0x004FF832 File Offset: 0x004FDA32
		public void ForceUnground(float time = 0.1f)
		{
			this._mustUnground = true;
			this._mustUngroundTimeCounter = time;
		}

		// Token: 0x0600E08A RID: 57482 RVA: 0x004FF842 File Offset: 0x004FDA42
		public bool MustUnground()
		{
			return this._mustUnground || this._mustUngroundTimeCounter > 0f;
		}

		// Token: 0x0600E08B RID: 57483 RVA: 0x004FF85C File Offset: 0x004FDA5C
		public Vector3 GetDirectionTangentToSurface(Vector3 direction, Vector3 surfaceNormal)
		{
			Vector3 vector = Vector3.Cross(direction, this._characterUp);
			return Vector3.Cross(surfaceNormal, vector).normalized;
		}

		// Token: 0x0600E08C RID: 57484 RVA: 0x004FF888 File Offset: 0x004FDA88
		[PublicizedFrom(EAccessModifier.Private)]
		public bool InternalCharacterMove(Vector3 movement, float deltaTime, out float resultingMovementMagnitude, out Vector3 resultingMovementDirection)
		{
			this._rigidbodiesPushedCount = 0;
			bool flag = true;
			float num = movement.magnitude;
			Vector3 vector = movement.normalized;
			resultingMovementDirection = vector;
			resultingMovementMagnitude = num;
			int num2 = 0;
			bool flag2 = true;
			Vector3 vector2 = this._transientPosition;
			Vector3 vector3 = vector;
			Vector3 cachedZeroVector = this._cachedZeroVector;
			MovementSweepState movementSweepState = MovementSweepState.Initial;
			for (int i = 0; i < this._overlapsCount; i++)
			{
				Vector3 normal = this._overlaps[i].Normal;
				if (Vector3.Dot(vector, normal) < 0f)
				{
					this.InternalHandleMovementProjection(this.IsStableOnNormal(normal) && !this.MustUnground(), normal, normal, vector3, ref movementSweepState, ref cachedZeroVector, ref resultingMovementMagnitude, ref vector, ref num);
				}
			}
			while (num > 0f && num2 <= 6 && flag2)
			{
				RaycastHit raycastHit;
				if (this.CharacterCollisionsSweep(vector2, this._transientRotation, vector, num + 0.001f, out raycastHit, this._internalCharacterHits, 0f, false) > 0)
				{
					Vector3 vector4 = vector2 + vector * raycastHit.distance + raycastHit.normal * 0.001f;
					Vector3 vector5 = vector4 - vector2;
					float magnitude = vector5.magnitude;
					Vector3 vector6 = Vector3.zero;
					if (deltaTime > 0f)
					{
						vector6 = vector5 / deltaTime;
					}
					HitStabilityReport hitStabilityReport = default(HitStabilityReport);
					this.EvaluateHitStability(raycastHit.collider, raycastHit.normal, raycastHit.point, vector4, this._transientRotation, vector6, ref hitStabilityReport);
					bool flag3 = false;
					if (this._solveGrounding && this.StepHandling != StepHandlingMethod.None && hitStabilityReport.ValidStepDetected && Mathf.Abs(Vector3.Dot(raycastHit.normal, this._characterUp)) <= 0.01f)
					{
						Vector3 normalized = Vector3.ProjectOnPlane(-raycastHit.normal, this._characterUp).normalized;
						Vector3 vector7 = vector4 + normalized * 0.03f + this._characterUp * this.MaxStepHeight;
						RaycastHit raycastHit2;
						int num3 = this.CharacterCollisionsSweep(vector7, this._transientRotation, -this._characterUp, this.MaxStepHeight, out raycastHit2, this._internalCharacterHits, 0f, true);
						for (int j = 0; j < num3; j++)
						{
							if (this._internalCharacterHits[j].collider == hitStabilityReport.SteppedCollider)
							{
								vector2 = vector7 + -this._characterUp * (this._internalCharacterHits[j].distance - 0.001f);
								flag3 = true;
								num = Mathf.Max(num - magnitude, 0f);
								Vector3 vector8 = num * vector;
								vector8 = Vector3.ProjectOnPlane(vector8, this.CharacterUp);
								num = vector8.magnitude;
								vector = vector8.normalized;
								resultingMovementMagnitude = num;
								break;
							}
						}
					}
					if (!flag3)
					{
						Collider collider = raycastHit.collider;
						Vector3 point = raycastHit.point;
						Vector3 normal2 = raycastHit.normal;
						vector2 = vector4;
						num = Mathf.Max(num - magnitude, 0f);
						this.CharacterController.OnMovementHit(collider, normal2, point, ref hitStabilityReport);
						Vector3 obstructionNormal = this.GetObstructionNormal(normal2, hitStabilityReport);
						if (this.InteractiveRigidbodyHandling && collider.attachedRigidbody)
						{
							this.StoreRigidbodyHit(collider.attachedRigidbody, vector * resultingMovementMagnitude / deltaTime, point, obstructionNormal, hitStabilityReport);
						}
						this.InternalHandleMovementProjection(hitStabilityReport.IsStable && !this.MustUnground(), normal2, obstructionNormal, vector3, ref movementSweepState, ref cachedZeroVector, ref resultingMovementMagnitude, ref vector, ref num);
					}
				}
				else
				{
					flag2 = false;
				}
				num2++;
				if (num2 > 6)
				{
					num = 0f;
					flag = false;
				}
			}
			this._transientPosition = vector2 + vector * num;
			resultingMovementDirection = vector;
			return flag;
		}

		// Token: 0x0600E08D RID: 57485 RVA: 0x004FFC58 File Offset: 0x004FDE58
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 GetObstructionNormal(Vector3 hitNormal, HitStabilityReport hitStabilityReport)
		{
			Vector3 vector = hitNormal;
			if (this.GroundingStatus.IsStableOnGround && !this.MustUnground() && !hitStabilityReport.IsStable)
			{
				vector = Vector3.Cross(Vector3.Cross(this.GroundingStatus.GroundNormal, vector).normalized, this._characterUp).normalized;
			}
			if (vector.sqrMagnitude == 0f)
			{
				vector = hitNormal;
			}
			return vector;
		}

		// Token: 0x0600E08E RID: 57486 RVA: 0x004FFCC4 File Offset: 0x004FDEC4
		[PublicizedFrom(EAccessModifier.Private)]
		public void StoreRigidbodyHit(Rigidbody hitRigidbody, Vector3 hitVelocity, Vector3 hitPoint, Vector3 obstructionNormal, HitStabilityReport hitStabilityReport)
		{
			if (this._rigidbodyProjectionHitCount < this._internalRigidbodyProjectionHits.Length && !hitRigidbody.GetComponent<KinematicCharacterMotor>())
			{
				RigidbodyProjectionHit rigidbodyProjectionHit = default(RigidbodyProjectionHit);
				rigidbodyProjectionHit.Rigidbody = hitRigidbody;
				rigidbodyProjectionHit.HitPoint = hitPoint;
				rigidbodyProjectionHit.EffectiveHitNormal = obstructionNormal;
				rigidbodyProjectionHit.HitVelocity = hitVelocity;
				rigidbodyProjectionHit.StableOnHit = hitStabilityReport.IsStable;
				this._internalRigidbodyProjectionHits[this._rigidbodyProjectionHitCount] = rigidbodyProjectionHit;
				this._rigidbodyProjectionHitCount++;
			}
		}

		// Token: 0x0600E08F RID: 57487 RVA: 0x004FFD48 File Offset: 0x004FDF48
		[PublicizedFrom(EAccessModifier.Private)]
		public void InternalHandleMovementProjection(bool stableOnHit, Vector3 hitNormal, Vector3 obstructionNormal, Vector3 originalMoveDirection, ref MovementSweepState sweepState, ref Vector3 previousObstructionNormal, ref float resultingMovementMagnitude, ref Vector3 remainingMovementDirection, ref float remainingMovementMagnitude)
		{
			if (remainingMovementMagnitude <= 0f)
			{
				return;
			}
			Vector3 vector = originalMoveDirection * remainingMovementMagnitude;
			float num = remainingMovementMagnitude;
			if (stableOnHit)
			{
				this.LastMovementIterationFoundAnyGround = true;
			}
			if (sweepState == MovementSweepState.FoundBlockingCrease)
			{
				remainingMovementMagnitude = 0f;
				resultingMovementMagnitude = 0f;
				sweepState = MovementSweepState.FoundBlockingCorner;
			}
			else
			{
				this.HandleMovementProjection(ref vector, obstructionNormal, stableOnHit);
				remainingMovementMagnitude = vector.magnitude;
				remainingMovementDirection = vector.normalized;
				resultingMovementMagnitude = remainingMovementMagnitude / num * resultingMovementMagnitude;
				if (sweepState == MovementSweepState.Initial)
				{
					sweepState = MovementSweepState.AfterFirstHit;
				}
				else if (sweepState == MovementSweepState.AfterFirstHit && Vector3.Dot(previousObstructionNormal, remainingMovementDirection) < 0f)
				{
					Vector3 normalized = Vector3.Cross(previousObstructionNormal, obstructionNormal).normalized;
					vector = Vector3.Project(vector, normalized);
					remainingMovementMagnitude = vector.magnitude;
					remainingMovementDirection = vector.normalized;
					resultingMovementMagnitude = remainingMovementMagnitude / num * resultingMovementMagnitude;
					sweepState = MovementSweepState.FoundBlockingCrease;
				}
			}
			previousObstructionNormal = obstructionNormal;
		}

		// Token: 0x0600E090 RID: 57488 RVA: 0x004FFE40 File Offset: 0x004FE040
		public virtual void HandleMovementProjection(ref Vector3 movement, Vector3 obstructionNormal, bool stableOnHit)
		{
			if (this.GroundingStatus.IsStableOnGround && !this.MustUnground())
			{
				if (stableOnHit)
				{
					movement = this.GetDirectionTangentToSurface(movement, obstructionNormal) * movement.magnitude;
					return;
				}
				Vector3 normalized = Vector3.Cross(Vector3.Cross(obstructionNormal, this.GroundingStatus.GroundNormal).normalized, obstructionNormal).normalized;
				movement = this.GetDirectionTangentToSurface(movement, normalized) * movement.magnitude;
				movement = Vector3.ProjectOnPlane(movement, obstructionNormal);
				return;
			}
			else
			{
				if (stableOnHit)
				{
					movement = Vector3.ProjectOnPlane(movement, this.CharacterUp);
					movement = this.GetDirectionTangentToSurface(movement, obstructionNormal) * movement.magnitude;
					return;
				}
				movement = Vector3.ProjectOnPlane(movement, obstructionNormal);
				return;
			}
		}

		// Token: 0x0600E091 RID: 57489 RVA: 0x004FFF2C File Offset: 0x004FE12C
		public virtual void HandleSimulatedRigidbodyInteraction(ref Vector3 processedVelocity, RigidbodyProjectionHit hit, float deltaTime)
		{
			float num = 0.2f;
			if (num > 0f && !hit.StableOnHit && !hit.Rigidbody.isKinematic)
			{
				float num2 = num / hit.Rigidbody.mass;
				Vector3 velocityFromRigidbodyMovement = this.GetVelocityFromRigidbodyMovement(hit.Rigidbody, hit.HitPoint, deltaTime);
				Vector3 vector = Vector3.Project(hit.HitVelocity, hit.EffectiveHitNormal) - velocityFromRigidbodyMovement;
				hit.Rigidbody.AddForceAtPosition(num2 * vector, hit.HitPoint, ForceMode.VelocityChange);
			}
			if (!hit.StableOnHit)
			{
				Vector3 vector2 = Vector3.Project(this.GetVelocityFromRigidbodyMovement(hit.Rigidbody, hit.HitPoint, deltaTime), hit.EffectiveHitNormal);
				Vector3 vector3 = Vector3.Project(processedVelocity, hit.EffectiveHitNormal);
				processedVelocity += vector2 - vector3;
			}
		}

		// Token: 0x0600E092 RID: 57490 RVA: 0x00500004 File Offset: 0x004FE204
		[PublicizedFrom(EAccessModifier.Private)]
		public void ProcessVelocityForRigidbodyHits(ref Vector3 processedVelocity, float deltaTime)
		{
			for (int i = 0; i < this._rigidbodyProjectionHitCount; i++)
			{
				if (this._internalRigidbodyProjectionHits[i].Rigidbody)
				{
					bool flag = false;
					for (int j = 0; j < this._rigidbodiesPushedCount; j++)
					{
						if (this._rigidbodiesPushedThisMove[j] == this._internalRigidbodyProjectionHits[j].Rigidbody)
						{
							flag = true;
							break;
						}
					}
					if (!flag && this._internalRigidbodyProjectionHits[i].Rigidbody != this._attachedRigidbody && this._rigidbodiesPushedCount < this._rigidbodiesPushedThisMove.Length)
					{
						this._rigidbodiesPushedThisMove[this._rigidbodiesPushedCount] = this._internalRigidbodyProjectionHits[i].Rigidbody;
						this._rigidbodiesPushedCount++;
						if (this.RigidbodyInteractionType == RigidbodyInteractionType.SimulatedDynamic)
						{
							this.HandleSimulatedRigidbodyInteraction(ref processedVelocity, this._internalRigidbodyProjectionHits[i], deltaTime);
						}
					}
				}
			}
		}

		// Token: 0x0600E093 RID: 57491 RVA: 0x005000F4 File Offset: 0x004FE2F4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckIfColliderValidForCollisions(Collider coll)
		{
			return !(coll == this.Capsule) && this.InternalIsColliderValidForCollisions(coll);
		}

		// Token: 0x0600E094 RID: 57492 RVA: 0x00500114 File Offset: 0x004FE314
		[PublicizedFrom(EAccessModifier.Private)]
		public bool InternalIsColliderValidForCollisions(Collider coll)
		{
			Rigidbody attachedRigidbody = coll.attachedRigidbody;
			if (attachedRigidbody)
			{
				bool isKinematic = attachedRigidbody.isKinematic;
				if (this._isMovingFromAttachedRigidbody && (!isKinematic || attachedRigidbody == this._attachedRigidbody))
				{
					return false;
				}
				if (this.RigidbodyInteractionType == RigidbodyInteractionType.Kinematic && !isKinematic)
				{
					return false;
				}
			}
			return this.CharacterController.IsColliderValidForCollisions(coll);
		}

		// Token: 0x0600E095 RID: 57493 RVA: 0x00500174 File Offset: 0x004FE374
		public void EvaluateHitStability(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, Vector3 withCharacterVelocity, ref HitStabilityReport stabilityReport)
		{
			if (!this._solveGrounding)
			{
				stabilityReport.IsStable = false;
				return;
			}
			Vector3 vector = atCharacterRotation * this._cachedWorldUp;
			Vector3 normalized = Vector3.ProjectOnPlane(hitNormal, vector).normalized;
			stabilityReport.IsStable = this.IsStableOnNormal(hitNormal);
			stabilityReport.InnerNormal = hitNormal;
			stabilityReport.OuterNormal = hitNormal;
			if (this.LedgeAndDenivelationHandling)
			{
				float num = 0.05f;
				if (this.StepHandling != StepHandlingMethod.None)
				{
					num = this.MaxStepHeight;
				}
				bool flag = false;
				bool flag2 = false;
				RaycastHit raycastHit;
				if (this.CharacterCollisionsRaycast(hitPoint + vector * 0.02f + normalized * 0.001f, -vector, num + 0.02f, out raycastHit, this._internalCharacterHits, false) > 0)
				{
					Vector3 normal = raycastHit.normal;
					stabilityReport.InnerNormal = normal;
					flag = this.IsStableOnNormal(normal);
				}
				RaycastHit raycastHit2;
				if (this.CharacterCollisionsRaycast(hitPoint + vector * 0.02f + -normalized * 0.001f, -vector, num + 0.02f, out raycastHit2, this._internalCharacterHits, false) > 0)
				{
					Vector3 normal2 = raycastHit2.normal;
					stabilityReport.OuterNormal = normal2;
					flag2 = this.IsStableOnNormal(normal2);
				}
				stabilityReport.LedgeDetected = flag != flag2;
				if (stabilityReport.LedgeDetected)
				{
					stabilityReport.IsOnEmptySideOfLedge = flag2 && !flag;
					stabilityReport.LedgeGroundNormal = (flag2 ? stabilityReport.OuterNormal : stabilityReport.InnerNormal);
					stabilityReport.LedgeRightDirection = Vector3.Cross(hitNormal, stabilityReport.OuterNormal).normalized;
					stabilityReport.LedgeFacingDirection = Vector3.Cross(stabilityReport.LedgeGroundNormal, stabilityReport.LedgeRightDirection).normalized;
					stabilityReport.DistanceFromLedge = Vector3.ProjectOnPlane(hitPoint - (atCharacterPosition + atCharacterRotation * this._characterTransformToCapsuleBottom), vector).magnitude;
					stabilityReport.IsMovingTowardsEmptySideOfLedge = Vector3.Dot(withCharacterVelocity, Vector3.ProjectOnPlane(stabilityReport.LedgeFacingDirection, this.CharacterUp)) > 0f;
				}
				if (stabilityReport.IsStable)
				{
					stabilityReport.IsStable = this.IsStableWithSpecialCases(ref stabilityReport, withCharacterVelocity);
				}
			}
			if (this.StepHandling != StepHandlingMethod.None && !stabilityReport.IsStable)
			{
				Rigidbody attachedRigidbody = hitCollider.attachedRigidbody;
				if (!attachedRigidbody || attachedRigidbody.isKinematic)
				{
					this.DetectSteps(atCharacterPosition, atCharacterRotation, hitPoint, normalized, ref stabilityReport);
					if (stabilityReport.ValidStepDetected)
					{
						stabilityReport.IsStable = true;
					}
				}
			}
			this.CharacterController.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition, atCharacterRotation, ref stabilityReport);
		}

		// Token: 0x0600E096 RID: 57494 RVA: 0x00500408 File Offset: 0x004FE608
		[PublicizedFrom(EAccessModifier.Private)]
		public void DetectSteps(Vector3 characterPosition, Quaternion characterRotation, Vector3 hitPoint, Vector3 innerHitDirection, ref HitStabilityReport stabilityReport)
		{
			Vector3 vector = characterRotation * this._cachedWorldUp;
			Vector3 vector2 = Vector3.Project(hitPoint - characterPosition, vector);
			Vector3 vector3 = hitPoint - vector2 + vector * this.MaxStepHeight;
			RaycastHit raycastHit;
			int num = this.CharacterCollisionsSweep(vector3, characterRotation, -vector, this.MaxStepHeight + 0.001f, out raycastHit, this._internalCharacterHits, 0f, true);
			Collider collider;
			if (this.CheckStepValidity(num, characterPosition, characterRotation, innerHitDirection, vector3, out collider))
			{
				stabilityReport.ValidStepDetected = true;
				stabilityReport.SteppedCollider = collider;
			}
			if (this.StepHandling == StepHandlingMethod.Extra && !stabilityReport.ValidStepDetected)
			{
				vector3 = characterPosition + vector * this.MaxStepHeight + -innerHitDirection * this.MinRequiredStepDepth;
				num = this.CharacterCollisionsSweep(vector3, characterRotation, -vector, this.MaxStepHeight - 0.001f, out raycastHit, this._internalCharacterHits, 0f, true);
				if (this.CheckStepValidity(num, characterPosition, characterRotation, innerHitDirection, vector3, out collider))
				{
					stabilityReport.ValidStepDetected = true;
					stabilityReport.SteppedCollider = collider;
				}
			}
		}

		// Token: 0x0600E097 RID: 57495 RVA: 0x00500524 File Offset: 0x004FE724
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckStepValidity(int nbStepHits, Vector3 characterPosition, Quaternion characterRotation, Vector3 innerHitDirection, Vector3 stepCheckStartPos, out Collider hitCollider)
		{
			hitCollider = null;
			Vector3 vector = characterRotation * Vector3.up;
			bool flag = false;
			while (nbStepHits > 0 && !flag)
			{
				RaycastHit raycastHit = default(RaycastHit);
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < nbStepHits; i++)
				{
					float distance = this._internalCharacterHits[i].distance;
					if (distance > num)
					{
						num = distance;
						raycastHit = this._internalCharacterHits[i];
						num2 = i;
					}
				}
				Vector3 vector2 = characterPosition + characterRotation * this._characterTransformToCapsuleBottom;
				float sqrMagnitude = Vector3.Project(raycastHit.point - vector2, vector).sqrMagnitude;
				Vector3 vector3 = stepCheckStartPos + -vector * (raycastHit.distance - 0.001f);
				RaycastHit raycastHit2;
				if (this.CharacterCollisionsOverlap(vector3, characterRotation, this._internalProbedColliders, 0f, false) <= 0 && this.CharacterCollisionsRaycast(raycastHit.point + vector * 0.02f + -innerHitDirection * 0.001f, -vector, this.MaxStepHeight + 0.02f, out raycastHit2, this._internalCharacterHits, true) > 0 && this.IsStableOnNormal(raycastHit2.normal) && this.CharacterCollisionsSweep(characterPosition, characterRotation, vector, this.MaxStepHeight - raycastHit.distance, out raycastHit2, this._internalCharacterHits, 0f, false) <= 0)
				{
					bool flag2 = false;
					RaycastHit raycastHit3;
					if (this.AllowSteppingWithoutStableGrounding)
					{
						flag2 = true;
					}
					else if (this.CharacterCollisionsRaycast(characterPosition + Vector3.Project(vector3 - characterPosition, vector), -vector, this.MaxStepHeight, out raycastHit3, this._internalCharacterHits, true) > 0 && this.IsStableOnNormal(raycastHit3.normal))
					{
						flag2 = true;
					}
					if (!flag2 && this.CharacterCollisionsRaycast(raycastHit.point + innerHitDirection * 0.001f, -vector, this.MaxStepHeight, out raycastHit3, this._internalCharacterHits, true) > 0 && this.IsStableOnNormal(raycastHit3.normal))
					{
						flag2 = true;
					}
					if (flag2)
					{
						hitCollider = raycastHit.collider;
						return true;
					}
				}
				if (!flag)
				{
					nbStepHits--;
					if (num2 < nbStepHits)
					{
						this._internalCharacterHits[num2] = this._internalCharacterHits[nbStepHits];
					}
				}
			}
			return false;
		}

		// Token: 0x0600E098 RID: 57496 RVA: 0x0050077C File Offset: 0x004FE97C
		public Vector3 GetVelocityFromRigidbodyMovement(Rigidbody interactiveRigidbody, Vector3 atPoint, float deltaTime)
		{
			if (deltaTime > 0f)
			{
				Vector3 vector = interactiveRigidbody.velocity;
				if (interactiveRigidbody.angularVelocity != Vector3.zero)
				{
					Vector3 vector2 = interactiveRigidbody.position + interactiveRigidbody.centerOfMass;
					Vector3 vector3 = atPoint - vector2;
					Quaternion quaternion = Quaternion.Euler(57.29578f * interactiveRigidbody.angularVelocity * deltaTime);
					Vector3 vector4 = vector2 + quaternion * vector3;
					vector += (vector4 - atPoint) / deltaTime;
				}
				return vector;
			}
			return Vector3.zero;
		}

		// Token: 0x0600E099 RID: 57497 RVA: 0x0050080C File Offset: 0x004FEA0C
		[PublicizedFrom(EAccessModifier.Private)]
		public Rigidbody GetInteractiveRigidbody(Collider onCollider)
		{
			Rigidbody attachedRigidbody = onCollider.attachedRigidbody;
			if (attachedRigidbody)
			{
				if (attachedRigidbody.gameObject.GetComponent<PhysicsMover>())
				{
					return attachedRigidbody;
				}
				if (!attachedRigidbody.isKinematic)
				{
					return attachedRigidbody;
				}
			}
			return null;
		}

		// Token: 0x0600E09A RID: 57498 RVA: 0x00500847 File Offset: 0x004FEA47
		public Vector3 GetVelocityForMovePosition(Vector3 fromPosition, Vector3 toPosition, float deltaTime)
		{
			if (deltaTime > 0f)
			{
				return (toPosition - fromPosition) / deltaTime;
			}
			return Vector3.zero;
		}

		// Token: 0x0600E09B RID: 57499 RVA: 0x00500864 File Offset: 0x004FEA64
		[PublicizedFrom(EAccessModifier.Private)]
		public void RestrictVectorToPlane(ref Vector3 vector, Vector3 toPlane)
		{
			if (vector.x > 0f != toPlane.x > 0f)
			{
				vector.x = 0f;
			}
			if (vector.y > 0f != toPlane.y > 0f)
			{
				vector.y = 0f;
			}
			if (vector.z > 0f != toPlane.z > 0f)
			{
				vector.z = 0f;
			}
		}

		// Token: 0x0600E09C RID: 57500 RVA: 0x005008E8 File Offset: 0x004FEAE8
		public int CharacterCollisionsOverlap(Vector3 position, Quaternion rotation, Collider[] overlappedColliders, float inflate = 0f, bool acceptOnlyStableGroundLayer = false)
		{
			int num = this.CollidableLayers;
			if (acceptOnlyStableGroundLayer)
			{
				num = this.CollidableLayers & this.StableGroundLayers;
			}
			Vector3 vector = position + rotation * this._characterTransformToCapsuleBottomHemi;
			Vector3 vector2 = position + rotation * this._characterTransformToCapsuleTopHemi;
			if (inflate != 0f)
			{
				vector += rotation * Vector3.down * inflate;
				vector2 += rotation * Vector3.up * inflate;
			}
			int num2;
			for (int i = (num2 = Physics.OverlapCapsuleNonAlloc(vector, vector2, this.Capsule.radius + inflate, overlappedColliders, num, QueryTriggerInteraction.Ignore)) - 1; i >= 0; i--)
			{
				if (!this.CheckIfColliderValidForCollisions(overlappedColliders[i]))
				{
					num2--;
					if (i < num2)
					{
						overlappedColliders[i] = overlappedColliders[num2];
					}
				}
			}
			return num2;
		}

		// Token: 0x0600E09D RID: 57501 RVA: 0x005009C8 File Offset: 0x004FEBC8
		public int CharacterOverlap(Vector3 position, Quaternion rotation, Collider[] overlappedColliders, LayerMask layers, QueryTriggerInteraction triggerInteraction, float inflate = 0f)
		{
			Vector3 vector = position + rotation * this._characterTransformToCapsuleBottomHemi;
			Vector3 vector2 = position + rotation * this._characterTransformToCapsuleTopHemi;
			if (inflate != 0f)
			{
				vector += rotation * Vector3.down * inflate;
				vector2 += rotation * Vector3.up * inflate;
			}
			int num;
			for (int i = (num = Physics.OverlapCapsuleNonAlloc(vector, vector2, this.Capsule.radius + inflate, overlappedColliders, layers, triggerInteraction)) - 1; i >= 0; i--)
			{
				if (overlappedColliders[i] == this.Capsule)
				{
					num--;
					if (i < num)
					{
						overlappedColliders[i] = overlappedColliders[num];
					}
				}
			}
			return num;
		}

		// Token: 0x0600E09E RID: 57502 RVA: 0x00500A88 File Offset: 0x004FEC88
		public int CharacterCollisionsSweep(Vector3 position, Quaternion rotation, Vector3 direction, float distance, out RaycastHit closestHit, RaycastHit[] hits, float inflate = 0f, bool acceptOnlyStableGroundLayer = false)
		{
			int num = this.CollidableLayers;
			if (acceptOnlyStableGroundLayer)
			{
				num = this.CollidableLayers & this.StableGroundLayers;
			}
			Vector3 vector = position + rotation * this._characterTransformToCapsuleBottomHemi - direction * 0.002f;
			Vector3 vector2 = position + rotation * this._characterTransformToCapsuleTopHemi - direction * 0.002f;
			if (inflate != 0f)
			{
				vector += rotation * Vector3.down * inflate;
				vector2 += rotation * Vector3.up * inflate;
			}
			int num2 = Physics.CapsuleCastNonAlloc(vector, vector2, this.Capsule.radius + inflate, direction, hits, distance + 0.002f, num, QueryTriggerInteraction.Ignore);
			closestHit = default(RaycastHit);
			float num3 = float.PositiveInfinity;
			int num4 = num2;
			for (int i = num2 - 1; i >= 0; i--)
			{
				int num5 = i;
				hits[num5].distance = hits[num5].distance - 0.002f;
				RaycastHit raycastHit = hits[i];
				float distance2 = raycastHit.distance;
				if (distance2 <= 0f || !this.CheckIfColliderValidForCollisions(raycastHit.collider))
				{
					num4--;
					if (i < num4)
					{
						hits[i] = hits[num4];
					}
				}
				else if (distance2 < num3)
				{
					closestHit = raycastHit;
					num3 = distance2;
				}
			}
			return num4;
		}

		// Token: 0x0600E09F RID: 57503 RVA: 0x00500BFC File Offset: 0x004FEDFC
		public int CharacterSweep(Vector3 position, Quaternion rotation, Vector3 direction, float distance, out RaycastHit closestHit, RaycastHit[] hits, LayerMask layers, QueryTriggerInteraction triggerInteraction, float inflate = 0f)
		{
			closestHit = default(RaycastHit);
			Vector3 vector = position + rotation * this._characterTransformToCapsuleBottomHemi;
			Vector3 vector2 = position + rotation * this._characterTransformToCapsuleTopHemi;
			if (inflate != 0f)
			{
				vector += rotation * Vector3.down * inflate;
				vector2 += rotation * Vector3.up * inflate;
			}
			int num = Physics.CapsuleCastNonAlloc(vector, vector2, this.Capsule.radius + inflate, direction, hits, distance, layers, triggerInteraction);
			float num2 = float.PositiveInfinity;
			int num3 = num;
			for (int i = num - 1; i >= 0; i--)
			{
				RaycastHit raycastHit = hits[i];
				if (raycastHit.distance <= 0f || raycastHit.collider == this.Capsule)
				{
					num3--;
					if (i < num3)
					{
						hits[i] = hits[num3];
					}
				}
				else
				{
					float distance2 = raycastHit.distance;
					if (distance2 < num2)
					{
						closestHit = raycastHit;
						num2 = distance2;
					}
				}
			}
			return num3;
		}

		// Token: 0x0600E0A0 RID: 57504 RVA: 0x00500D14 File Offset: 0x004FEF14
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CharacterGroundSweep(Vector3 position, Quaternion rotation, Vector3 direction, float distance, out RaycastHit closestHit)
		{
			closestHit = default(RaycastHit);
			int num = Physics.CapsuleCastNonAlloc(position + rotation * this._characterTransformToCapsuleBottomHemi - direction * 0.1f, position + rotation * this._characterTransformToCapsuleTopHemi - direction * 0.1f, this.Capsule.radius, direction, this._internalCharacterHits, distance + 0.1f, this.CollidableLayers & this.StableGroundLayers, QueryTriggerInteraction.Ignore);
			bool flag = false;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this._internalCharacterHits[i];
				float distance2 = raycastHit.distance;
				if (distance2 > 0f && this.CheckIfColliderValidForCollisions(raycastHit.collider) && distance2 < num2)
				{
					closestHit = raycastHit;
					closestHit.distance -= 0.1f;
					num2 = distance2;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600E0A1 RID: 57505 RVA: 0x00500E10 File Offset: 0x004FF010
		public int CharacterCollisionsRaycast(Vector3 position, Vector3 direction, float distance, out RaycastHit closestHit, RaycastHit[] hits, bool acceptOnlyStableGroundLayer = false)
		{
			int num = this.CollidableLayers;
			if (acceptOnlyStableGroundLayer)
			{
				num = this.CollidableLayers & this.StableGroundLayers;
			}
			int num2 = Physics.RaycastNonAlloc(position, direction, hits, distance, num, QueryTriggerInteraction.Ignore);
			closestHit = default(RaycastHit);
			float num3 = float.PositiveInfinity;
			int num4 = num2;
			for (int i = num2 - 1; i >= 0; i--)
			{
				RaycastHit raycastHit = hits[i];
				float distance2 = raycastHit.distance;
				if (distance2 <= 0f || !this.CheckIfColliderValidForCollisions(raycastHit.collider))
				{
					num4--;
					if (i < num4)
					{
						hits[i] = hits[num4];
					}
				}
				else if (distance2 < num3)
				{
					closestHit = raycastHit;
					num3 = distance2;
				}
			}
			return num4;
		}

		// Token: 0x0400A9E2 RID: 43490
		[Header("Components")]
		[ReadOnly]
		public CapsuleCollider Capsule;

		// Token: 0x0400A9E3 RID: 43491
		[Header("Capsule Settings")]
		[SerializeField]
		[Tooltip("Radius of the Character Capsule")]
		[PublicizedFrom(EAccessModifier.Private)]
		public float CapsuleRadius = 0.5f;

		// Token: 0x0400A9E4 RID: 43492
		[SerializeField]
		[Tooltip("Height of the Character Capsule")]
		[PublicizedFrom(EAccessModifier.Private)]
		public float CapsuleHeight = 2f;

		// Token: 0x0400A9E5 RID: 43493
		[SerializeField]
		[Tooltip("Height of the Character Capsule")]
		[PublicizedFrom(EAccessModifier.Private)]
		public float CapsuleYOffset = 1f;

		// Token: 0x0400A9E6 RID: 43494
		[SerializeField]
		[Tooltip("Physics material of the Character Capsule (Does not affect character movement. Only affects things colliding with it)")]
		[PublicizedFrom(EAccessModifier.Private)]
		public PhysicMaterial CapsulePhysicsMaterial;

		// Token: 0x0400A9E7 RID: 43495
		[Header("Misc settings")]
		[Tooltip("Increases the range of ground detection, to allow snapping to ground at very high speeds")]
		public float GroundDetectionExtraDistance;

		// Token: 0x0400A9E8 RID: 43496
		[Range(0f, 89f)]
		[Tooltip("Maximum slope angle on which the character can be stable")]
		public float MaxStableSlopeAngle = 60f;

		// Token: 0x0400A9E9 RID: 43497
		[Tooltip("Which layers can the character be considered stable on")]
		public LayerMask StableGroundLayers = -1;

		// Token: 0x0400A9EA RID: 43498
		[Tooltip("Notifies the Character Controller when discrete collisions are detected")]
		public bool DiscreteCollisionEvents;

		// Token: 0x0400A9EB RID: 43499
		[Header("Step settings")]
		[Tooltip("Handles properly detecting grounding status on steps, but has a performance cost.")]
		public StepHandlingMethod StepHandling = StepHandlingMethod.Standard;

		// Token: 0x0400A9EC RID: 43500
		[Tooltip("Maximum height of a step which the character can climb")]
		public float MaxStepHeight = 0.5f;

		// Token: 0x0400A9ED RID: 43501
		[Tooltip("Can the character step up obstacles even if it is not currently stable?")]
		public bool AllowSteppingWithoutStableGrounding;

		// Token: 0x0400A9EE RID: 43502
		[Tooltip("Minimum length of a step that the character can step on (used in Extra stepping method). Use this to let the character step on steps that are smaller that its radius")]
		public float MinRequiredStepDepth = 0.1f;

		// Token: 0x0400A9EF RID: 43503
		[Header("Ledge settings")]
		[Tooltip("Handles properly detecting ledge information and grounding status, but has a performance cost.")]
		public bool LedgeAndDenivelationHandling = true;

		// Token: 0x0400A9F0 RID: 43504
		[Tooltip("The distance from the capsule central axis at which the character can stand on a ledge and still be stable")]
		public float MaxStableDistanceFromLedge = 0.5f;

		// Token: 0x0400A9F1 RID: 43505
		[Tooltip("Prevents snapping to ground on ledges beyond a certain velocity")]
		public float MaxVelocityForLedgeSnap;

		// Token: 0x0400A9F2 RID: 43506
		[Tooltip("The maximun downward slope angle change that the character can be subjected to and still be snapping to the ground")]
		[Range(1f, 180f)]
		public float MaxStableDenivelationAngle = 180f;

		// Token: 0x0400A9F3 RID: 43507
		[Header("Rigidbody interaction settings")]
		[Tooltip("Handles properly being pushed by and standing on PhysicsMovers or dynamic rigidbodies. Also handles pushing dynamic rigidbodies")]
		public bool InteractiveRigidbodyHandling = true;

		// Token: 0x0400A9F4 RID: 43508
		[Tooltip("How the character interacts with non-kinematic rigidbodies. \"Kinematic\" mode means the character pushes the rigidbodies with infinite force (as a kinematic body would). \"SimulatedDynamic\" pushes the rigidbodies with a simulated mass value.")]
		public RigidbodyInteractionType RigidbodyInteractionType;

		// Token: 0x0400A9F5 RID: 43509
		[Tooltip("Determines if the character preserves moving platform velocities when de-grounding from them")]
		public bool PreserveAttachedRigidbodyMomentum = true;

		// Token: 0x0400A9F6 RID: 43510
		[Header("Constraints settings")]
		[Tooltip("Determines if the character's movement uses the planar constraint")]
		public bool HasPlanarConstraint;

		// Token: 0x0400A9F7 RID: 43511
		[Tooltip("Defines the plane that the character's movement is constrained on, if HasMovementConstraintPlane is active")]
		public Vector3 PlanarConstraintAxis = Vector3.forward;

		// Token: 0x0400A9F8 RID: 43512
		[NonSerialized]
		public CharacterGroundingReport GroundingStatus;

		// Token: 0x0400A9F9 RID: 43513
		[NonSerialized]
		public CharacterTransientGroundingReport LastGroundingStatus;

		// Token: 0x0400A9FA RID: 43514
		[NonSerialized]
		public LayerMask CollidableLayers = -1;

		// Token: 0x0400A9FB RID: 43515
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Transform _transform;

		// Token: 0x0400A9FC RID: 43516
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _transientPosition;

		// Token: 0x0400A9FD RID: 43517
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterUp;

		// Token: 0x0400A9FE RID: 43518
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterForward;

		// Token: 0x0400A9FF RID: 43519
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterRight;

		// Token: 0x0400AA00 RID: 43520
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _initialSimulationPosition;

		// Token: 0x0400AA01 RID: 43521
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _initialSimulationRotation;

		// Token: 0x0400AA02 RID: 43522
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Rigidbody _attachedRigidbody;

		// Token: 0x0400AA03 RID: 43523
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleCenter;

		// Token: 0x0400AA04 RID: 43524
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleBottom;

		// Token: 0x0400AA05 RID: 43525
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleTop;

		// Token: 0x0400AA06 RID: 43526
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleBottomHemi;

		// Token: 0x0400AA07 RID: 43527
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleTopHemi;

		// Token: 0x0400AA08 RID: 43528
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _attachedRigidbodyVelocity;

		// Token: 0x0400AA09 RID: 43529
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _overlapsCount;

		// Token: 0x0400AA0A RID: 43530
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public OverlapResult[] _overlaps = new OverlapResult[16];

		// Token: 0x0400AA0B RID: 43531
		[NonSerialized]
		public ICharacterController CharacterController;

		// Token: 0x0400AA0C RID: 43532
		[NonSerialized]
		public bool LastMovementIterationFoundAnyGround;

		// Token: 0x0400AA0D RID: 43533
		[NonSerialized]
		public int IndexInCharacterSystem;

		// Token: 0x0400AA0E RID: 43534
		[NonSerialized]
		public Vector3 InitialTickPosition;

		// Token: 0x0400AA0F RID: 43535
		[NonSerialized]
		public Quaternion InitialTickRotation;

		// Token: 0x0400AA10 RID: 43536
		[NonSerialized]
		public Rigidbody AttachedRigidbodyOverride;

		// Token: 0x0400AA11 RID: 43537
		[NonSerialized]
		public Vector3 BaseVelocity;

		// Token: 0x0400AA12 RID: 43538
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RaycastHit[] _internalCharacterHits = new RaycastHit[16];

		// Token: 0x0400AA13 RID: 43539
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Collider[] _internalProbedColliders = new Collider[16];

		// Token: 0x0400AA14 RID: 43540
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Rigidbody[] _rigidbodiesPushedThisMove = new Rigidbody[16];

		// Token: 0x0400AA15 RID: 43541
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RigidbodyProjectionHit[] _internalRigidbodyProjectionHits = new RigidbodyProjectionHit[6];

		// Token: 0x0400AA16 RID: 43542
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Rigidbody _lastAttachedRigidbody;

		// Token: 0x0400AA17 RID: 43543
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _solveMovementCollisions = true;

		// Token: 0x0400AA18 RID: 43544
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _solveGrounding = true;

		// Token: 0x0400AA19 RID: 43545
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _movePositionDirty;

		// Token: 0x0400AA1A RID: 43546
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _movePositionTarget = Vector3.zero;

		// Token: 0x0400AA1B RID: 43547
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _moveRotationDirty;

		// Token: 0x0400AA1C RID: 43548
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _moveRotationTarget = Quaternion.identity;

		// Token: 0x0400AA1D RID: 43549
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _lastSolvedOverlapNormalDirty;

		// Token: 0x0400AA1E RID: 43550
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _lastSolvedOverlapNormal = Vector3.forward;

		// Token: 0x0400AA1F RID: 43551
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _rigidbodiesPushedCount;

		// Token: 0x0400AA20 RID: 43552
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _rigidbodyProjectionHitCount;

		// Token: 0x0400AA21 RID: 43553
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float _internalResultingMovementMagnitude;

		// Token: 0x0400AA22 RID: 43554
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _internalResultingMovementDirection = Vector3.zero;

		// Token: 0x0400AA23 RID: 43555
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _isMovingFromAttachedRigidbody;

		// Token: 0x0400AA24 RID: 43556
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _mustUnground;

		// Token: 0x0400AA25 RID: 43557
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float _mustUngroundTimeCounter;

		// Token: 0x0400AA26 RID: 43558
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedWorldUp = Vector3.up;

		// Token: 0x0400AA27 RID: 43559
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedWorldForward = Vector3.forward;

		// Token: 0x0400AA28 RID: 43560
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedWorldRight = Vector3.right;

		// Token: 0x0400AA29 RID: 43561
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedZeroVector = Vector3.zero;

		// Token: 0x0400AA2A RID: 43562
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _transientRotation;

		// Token: 0x0400AA2B RID: 43563
		public const int MaxHitsBudget = 16;

		// Token: 0x0400AA2C RID: 43564
		public const int MaxCollisionBudget = 16;

		// Token: 0x0400AA2D RID: 43565
		public const int MaxGroundingSweepIterations = 2;

		// Token: 0x0400AA2E RID: 43566
		public const int MaxMovementSweepIterations = 6;

		// Token: 0x0400AA2F RID: 43567
		public const int MaxSteppingSweepIterations = 3;

		// Token: 0x0400AA30 RID: 43568
		public const int MaxRigidbodyOverlapsCount = 16;

		// Token: 0x0400AA31 RID: 43569
		public const int MaxDiscreteCollisionIterations = 3;

		// Token: 0x0400AA32 RID: 43570
		public const float CollisionOffset = 0.001f;

		// Token: 0x0400AA33 RID: 43571
		public const float GroundProbeReboundDistance = 0.02f;

		// Token: 0x0400AA34 RID: 43572
		public const float MinimumGroundProbingDistance = 0.005f;

		// Token: 0x0400AA35 RID: 43573
		public const float GroundProbingBackstepDistance = 0.1f;

		// Token: 0x0400AA36 RID: 43574
		public const float SweepProbingBackstepDistance = 0.002f;

		// Token: 0x0400AA37 RID: 43575
		public const float SecondaryProbesVertical = 0.02f;

		// Token: 0x0400AA38 RID: 43576
		public const float SecondaryProbesHorizontal = 0.001f;

		// Token: 0x0400AA39 RID: 43577
		public const float MinVelocityMagnitude = 0.01f;

		// Token: 0x0400AA3A RID: 43578
		public const float SteppingForwardDistance = 0.03f;

		// Token: 0x0400AA3B RID: 43579
		public const float MinDistanceForLedge = 0.05f;

		// Token: 0x0400AA3C RID: 43580
		public const float CorrelationForVerticalObstruction = 0.01f;

		// Token: 0x0400AA3D RID: 43581
		public const float ExtraSteppingForwardDistance = 0.01f;

		// Token: 0x0400AA3E RID: 43582
		public const float ExtraStepHeightPadding = 0.01f;
	}
}

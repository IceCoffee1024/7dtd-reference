using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D8A RID: 7562
	[RequireComponent(typeof(CapsuleCollider))]
	public class KinematicCharacterMotor : MonoBehaviour
	{
		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x0600DF4B RID: 57163 RVA: 0x004FA885 File Offset: 0x004F8A85
		public Transform Transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x17001C4E RID: 7246
		// (get) Token: 0x0600DF4C RID: 57164 RVA: 0x004FA88D File Offset: 0x004F8A8D
		public Vector3 TransientPosition
		{
			get
			{
				return this._transientPosition;
			}
		}

		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x0600DF4D RID: 57165 RVA: 0x004FA895 File Offset: 0x004F8A95
		public Vector3 CharacterUp
		{
			get
			{
				return this._characterUp;
			}
		}

		// Token: 0x17001C50 RID: 7248
		// (get) Token: 0x0600DF4E RID: 57166 RVA: 0x004FA89D File Offset: 0x004F8A9D
		public Vector3 CharacterForward
		{
			get
			{
				return this._characterForward;
			}
		}

		// Token: 0x17001C51 RID: 7249
		// (get) Token: 0x0600DF4F RID: 57167 RVA: 0x004FA8A5 File Offset: 0x004F8AA5
		public Vector3 CharacterRight
		{
			get
			{
				return this._characterRight;
			}
		}

		// Token: 0x17001C52 RID: 7250
		// (get) Token: 0x0600DF50 RID: 57168 RVA: 0x004FA8AD File Offset: 0x004F8AAD
		public Vector3 InitialSimulationPosition
		{
			get
			{
				return this._initialSimulationPosition;
			}
		}

		// Token: 0x17001C53 RID: 7251
		// (get) Token: 0x0600DF51 RID: 57169 RVA: 0x004FA8B5 File Offset: 0x004F8AB5
		public Quaternion InitialSimulationRotation
		{
			get
			{
				return this._initialSimulationRotation;
			}
		}

		// Token: 0x17001C54 RID: 7252
		// (get) Token: 0x0600DF52 RID: 57170 RVA: 0x004FA8BD File Offset: 0x004F8ABD
		public Rigidbody AttachedRigidbody
		{
			get
			{
				return this._attachedRigidbody;
			}
		}

		// Token: 0x17001C55 RID: 7253
		// (get) Token: 0x0600DF53 RID: 57171 RVA: 0x004FA8C5 File Offset: 0x004F8AC5
		public Vector3 CharacterTransformToCapsuleCenter
		{
			get
			{
				return this._characterTransformToCapsuleCenter;
			}
		}

		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x0600DF54 RID: 57172 RVA: 0x004FA8CD File Offset: 0x004F8ACD
		public Vector3 CharacterTransformToCapsuleBottom
		{
			get
			{
				return this._characterTransformToCapsuleBottom;
			}
		}

		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x0600DF55 RID: 57173 RVA: 0x004FA8D5 File Offset: 0x004F8AD5
		public Vector3 CharacterTransformToCapsuleTop
		{
			get
			{
				return this._characterTransformToCapsuleTop;
			}
		}

		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x0600DF56 RID: 57174 RVA: 0x004FA8DD File Offset: 0x004F8ADD
		public Vector3 CharacterTransformToCapsuleBottomHemi
		{
			get
			{
				return this._characterTransformToCapsuleBottomHemi;
			}
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x0600DF57 RID: 57175 RVA: 0x004FA8E5 File Offset: 0x004F8AE5
		public Vector3 CharacterTransformToCapsuleTopHemi
		{
			get
			{
				return this._characterTransformToCapsuleTopHemi;
			}
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x0600DF58 RID: 57176 RVA: 0x004FA8ED File Offset: 0x004F8AED
		public Vector3 AttachedRigidbodyVelocity
		{
			get
			{
				return this._attachedRigidbodyVelocity;
			}
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x0600DF59 RID: 57177 RVA: 0x004FA8F5 File Offset: 0x004F8AF5
		public int OverlapsCount
		{
			get
			{
				return this._overlapsCount;
			}
		}

		// Token: 0x17001C5C RID: 7260
		// (get) Token: 0x0600DF5A RID: 57178 RVA: 0x004FA8FD File Offset: 0x004F8AFD
		public OverlapResult[] Overlaps
		{
			get
			{
				return this._overlaps;
			}
		}

		// Token: 0x17001C5D RID: 7261
		// (get) Token: 0x0600DF5B RID: 57179 RVA: 0x004FA905 File Offset: 0x004F8B05
		// (set) Token: 0x0600DF5C RID: 57180 RVA: 0x004FA910 File Offset: 0x004F8B10
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

		// Token: 0x17001C5E RID: 7262
		// (get) Token: 0x0600DF5D RID: 57181 RVA: 0x004FA969 File Offset: 0x004F8B69
		public Vector3 Velocity
		{
			get
			{
				return this.BaseVelocity + this._attachedRigidbodyVelocity;
			}
		}

		// Token: 0x0600DF5E RID: 57182 RVA: 0x004FA97C File Offset: 0x004F8B7C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			KinematicCharacterSystem.EnsureCreation();
			KinematicCharacterSystem.RegisterCharacterMotor(this);
		}

		// Token: 0x0600DF5F RID: 57183 RVA: 0x004FA989 File Offset: 0x004F8B89
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			KinematicCharacterSystem.UnregisterCharacterMotor(this);
		}

		// Token: 0x0600DF60 RID: 57184 RVA: 0x004FA991 File Offset: 0x004F8B91
		[PublicizedFrom(EAccessModifier.Private)]
		public void Reset()
		{
			this.ValidateData();
		}

		// Token: 0x0600DF61 RID: 57185 RVA: 0x004FA991 File Offset: 0x004F8B91
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnValidate()
		{
			this.ValidateData();
		}

		// Token: 0x0600DF62 RID: 57186 RVA: 0x004FA999 File Offset: 0x004F8B99
		[ContextMenu("Remove Component")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleRemoveComponent()
		{
			global::UnityEngine.Object component = base.gameObject.GetComponent<CapsuleCollider>();
			global::UnityEngine.Object.DestroyImmediate(this);
			global::UnityEngine.Object.DestroyImmediate(component);
		}

		// Token: 0x0600DF63 RID: 57187 RVA: 0x004FA9B4 File Offset: 0x004F8BB4
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

		// Token: 0x0600DF64 RID: 57188 RVA: 0x004FAAAC File Offset: 0x004F8CAC
		public void SetCapsuleCollisionsActivation(bool collisionsActive)
		{
			this.Capsule.isTrigger = !collisionsActive;
		}

		// Token: 0x0600DF65 RID: 57189 RVA: 0x004FAABD File Offset: 0x004F8CBD
		public void SetMovementCollisionsSolvingActivation(bool movementCollisionsSolvingActive)
		{
			this._solveMovementCollisions = movementCollisionsSolvingActive;
		}

		// Token: 0x0600DF66 RID: 57190 RVA: 0x004FAAC6 File Offset: 0x004F8CC6
		public void SetGroundSolvingActivation(bool stabilitySolvingActive)
		{
			this._solveGrounding = stabilitySolvingActive;
		}

		// Token: 0x0600DF67 RID: 57191 RVA: 0x004FAACF File Offset: 0x004F8CCF
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

		// Token: 0x0600DF68 RID: 57192 RVA: 0x004FAAF5 File Offset: 0x004F8CF5
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

		// Token: 0x0600DF69 RID: 57193 RVA: 0x004FAB1B File Offset: 0x004F8D1B
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

		// Token: 0x0600DF6A RID: 57194 RVA: 0x004FAB57 File Offset: 0x004F8D57
		public void MoveCharacter(Vector3 toPosition)
		{
			this._movePositionDirty = true;
			this._movePositionTarget = toPosition;
		}

		// Token: 0x0600DF6B RID: 57195 RVA: 0x004FAB67 File Offset: 0x004F8D67
		public void RotateCharacter(Quaternion toRotation)
		{
			this._moveRotationDirty = true;
			this._moveRotationTarget = toRotation;
		}

		// Token: 0x0600DF6C RID: 57196 RVA: 0x004FAB78 File Offset: 0x004F8D78
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

		// Token: 0x0600DF6D RID: 57197 RVA: 0x004FAC08 File Offset: 0x004F8E08
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

		// Token: 0x0600DF6E RID: 57198 RVA: 0x004FAC84 File Offset: 0x004F8E84
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

		// Token: 0x0600DF6F RID: 57199 RVA: 0x004FAE20 File Offset: 0x004F9020
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

		// Token: 0x0600DF70 RID: 57200 RVA: 0x004FAEC4 File Offset: 0x004F90C4
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

		// Token: 0x0600DF71 RID: 57201 RVA: 0x004FB518 File Offset: 0x004F9718
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

		// Token: 0x0600DF72 RID: 57202 RVA: 0x004FB9AF File Offset: 0x004F9BAF
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsStableOnNormal(Vector3 normal)
		{
			return Vector3.Angle(this._characterUp, normal) <= this.MaxStableSlopeAngle;
		}

		// Token: 0x0600DF73 RID: 57203 RVA: 0x004FB9C8 File Offset: 0x004F9BC8
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

		// Token: 0x0600DF74 RID: 57204 RVA: 0x004FBA88 File Offset: 0x004F9C88
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

		// Token: 0x0600DF75 RID: 57205 RVA: 0x004FBC92 File Offset: 0x004F9E92
		public void ForceUnground(float time = 0.1f)
		{
			this._mustUnground = true;
			this._mustUngroundTimeCounter = time;
		}

		// Token: 0x0600DF76 RID: 57206 RVA: 0x004FBCA2 File Offset: 0x004F9EA2
		public bool MustUnground()
		{
			return this._mustUnground || this._mustUngroundTimeCounter > 0f;
		}

		// Token: 0x0600DF77 RID: 57207 RVA: 0x004FBCBC File Offset: 0x004F9EBC
		public Vector3 GetDirectionTangentToSurface(Vector3 direction, Vector3 surfaceNormal)
		{
			Vector3 vector = Vector3.Cross(direction, this._characterUp);
			return Vector3.Cross(surfaceNormal, vector).normalized;
		}

		// Token: 0x0600DF78 RID: 57208 RVA: 0x004FBCE8 File Offset: 0x004F9EE8
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

		// Token: 0x0600DF79 RID: 57209 RVA: 0x004FC0B8 File Offset: 0x004FA2B8
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

		// Token: 0x0600DF7A RID: 57210 RVA: 0x004FC124 File Offset: 0x004FA324
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

		// Token: 0x0600DF7B RID: 57211 RVA: 0x004FC1A8 File Offset: 0x004FA3A8
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

		// Token: 0x0600DF7C RID: 57212 RVA: 0x004FC2A0 File Offset: 0x004FA4A0
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

		// Token: 0x0600DF7D RID: 57213 RVA: 0x004FC38C File Offset: 0x004FA58C
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

		// Token: 0x0600DF7E RID: 57214 RVA: 0x004FC464 File Offset: 0x004FA664
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

		// Token: 0x0600DF7F RID: 57215 RVA: 0x004FC554 File Offset: 0x004FA754
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckIfColliderValidForCollisions(Collider coll)
		{
			return !(coll == this.Capsule) && this.InternalIsColliderValidForCollisions(coll);
		}

		// Token: 0x0600DF80 RID: 57216 RVA: 0x004FC574 File Offset: 0x004FA774
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

		// Token: 0x0600DF81 RID: 57217 RVA: 0x004FC5D4 File Offset: 0x004FA7D4
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

		// Token: 0x0600DF82 RID: 57218 RVA: 0x004FC868 File Offset: 0x004FAA68
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

		// Token: 0x0600DF83 RID: 57219 RVA: 0x004FC984 File Offset: 0x004FAB84
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

		// Token: 0x0600DF84 RID: 57220 RVA: 0x004FCBDC File Offset: 0x004FADDC
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

		// Token: 0x0600DF85 RID: 57221 RVA: 0x004FCC6C File Offset: 0x004FAE6C
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

		// Token: 0x0600DF86 RID: 57222 RVA: 0x004FCCA7 File Offset: 0x004FAEA7
		public Vector3 GetVelocityForMovePosition(Vector3 fromPosition, Vector3 toPosition, float deltaTime)
		{
			if (deltaTime > 0f)
			{
				return (toPosition - fromPosition) / deltaTime;
			}
			return Vector3.zero;
		}

		// Token: 0x0600DF87 RID: 57223 RVA: 0x004FCCC4 File Offset: 0x004FAEC4
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

		// Token: 0x0600DF88 RID: 57224 RVA: 0x004FCD48 File Offset: 0x004FAF48
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

		// Token: 0x0600DF89 RID: 57225 RVA: 0x004FCE28 File Offset: 0x004FB028
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

		// Token: 0x0600DF8A RID: 57226 RVA: 0x004FCEE8 File Offset: 0x004FB0E8
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

		// Token: 0x0600DF8B RID: 57227 RVA: 0x004FD05C File Offset: 0x004FB25C
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

		// Token: 0x0600DF8C RID: 57228 RVA: 0x004FD174 File Offset: 0x004FB374
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

		// Token: 0x0600DF8D RID: 57229 RVA: 0x004FD270 File Offset: 0x004FB470
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

		// Token: 0x0400A8F9 RID: 43257
		[Header("Components")]
		[ReadOnly]
		public CapsuleCollider Capsule;

		// Token: 0x0400A8FA RID: 43258
		[Header("Capsule Settings")]
		[SerializeField]
		[Tooltip("Radius of the Character Capsule")]
		[PublicizedFrom(EAccessModifier.Private)]
		public float CapsuleRadius = 0.5f;

		// Token: 0x0400A8FB RID: 43259
		[SerializeField]
		[Tooltip("Height of the Character Capsule")]
		[PublicizedFrom(EAccessModifier.Private)]
		public float CapsuleHeight = 2f;

		// Token: 0x0400A8FC RID: 43260
		[SerializeField]
		[Tooltip("Height of the Character Capsule")]
		[PublicizedFrom(EAccessModifier.Private)]
		public float CapsuleYOffset = 1f;

		// Token: 0x0400A8FD RID: 43261
		[SerializeField]
		[Tooltip("Physics material of the Character Capsule (Does not affect character movement. Only affects things colliding with it)")]
		[PublicizedFrom(EAccessModifier.Private)]
		public PhysicMaterial CapsulePhysicsMaterial;

		// Token: 0x0400A8FE RID: 43262
		[Header("Misc settings")]
		[Tooltip("Increases the range of ground detection, to allow snapping to ground at very high speeds")]
		public float GroundDetectionExtraDistance;

		// Token: 0x0400A8FF RID: 43263
		[Range(0f, 89f)]
		[Tooltip("Maximum slope angle on which the character can be stable")]
		public float MaxStableSlopeAngle = 60f;

		// Token: 0x0400A900 RID: 43264
		[Tooltip("Which layers can the character be considered stable on")]
		public LayerMask StableGroundLayers = -1;

		// Token: 0x0400A901 RID: 43265
		[Tooltip("Notifies the Character Controller when discrete collisions are detected")]
		public bool DiscreteCollisionEvents;

		// Token: 0x0400A902 RID: 43266
		[Header("Step settings")]
		[Tooltip("Handles properly detecting grounding status on steps, but has a performance cost.")]
		public StepHandlingMethod StepHandling = StepHandlingMethod.Standard;

		// Token: 0x0400A903 RID: 43267
		[Tooltip("Maximum height of a step which the character can climb")]
		public float MaxStepHeight = 0.5f;

		// Token: 0x0400A904 RID: 43268
		[Tooltip("Can the character step up obstacles even if it is not currently stable?")]
		public bool AllowSteppingWithoutStableGrounding;

		// Token: 0x0400A905 RID: 43269
		[Tooltip("Minimum length of a step that the character can step on (used in Extra stepping method). Use this to let the character step on steps that are smaller that its radius")]
		public float MinRequiredStepDepth = 0.1f;

		// Token: 0x0400A906 RID: 43270
		[Header("Ledge settings")]
		[Tooltip("Handles properly detecting ledge information and grounding status, but has a performance cost.")]
		public bool LedgeAndDenivelationHandling = true;

		// Token: 0x0400A907 RID: 43271
		[Tooltip("The distance from the capsule central axis at which the character can stand on a ledge and still be stable")]
		public float MaxStableDistanceFromLedge = 0.5f;

		// Token: 0x0400A908 RID: 43272
		[Tooltip("Prevents snapping to ground on ledges beyond a certain velocity")]
		public float MaxVelocityForLedgeSnap;

		// Token: 0x0400A909 RID: 43273
		[Tooltip("The maximun downward slope angle change that the character can be subjected to and still be snapping to the ground")]
		[Range(1f, 180f)]
		public float MaxStableDenivelationAngle = 180f;

		// Token: 0x0400A90A RID: 43274
		[Header("Rigidbody interaction settings")]
		[Tooltip("Handles properly being pushed by and standing on PhysicsMovers or dynamic rigidbodies. Also handles pushing dynamic rigidbodies")]
		public bool InteractiveRigidbodyHandling = true;

		// Token: 0x0400A90B RID: 43275
		[Tooltip("How the character interacts with non-kinematic rigidbodies. \"Kinematic\" mode means the character pushes the rigidbodies with infinite force (as a kinematic body would). \"SimulatedDynamic\" pushes the rigidbodies with a simulated mass value.")]
		public RigidbodyInteractionType RigidbodyInteractionType;

		// Token: 0x0400A90C RID: 43276
		[Tooltip("Determines if the character preserves moving platform velocities when de-grounding from them")]
		public bool PreserveAttachedRigidbodyMomentum = true;

		// Token: 0x0400A90D RID: 43277
		[Header("Constraints settings")]
		[Tooltip("Determines if the character's movement uses the planar constraint")]
		public bool HasPlanarConstraint;

		// Token: 0x0400A90E RID: 43278
		[Tooltip("Defines the plane that the character's movement is constrained on, if HasMovementConstraintPlane is active")]
		public Vector3 PlanarConstraintAxis = Vector3.forward;

		// Token: 0x0400A90F RID: 43279
		[NonSerialized]
		public CharacterGroundingReport GroundingStatus;

		// Token: 0x0400A910 RID: 43280
		[NonSerialized]
		public CharacterTransientGroundingReport LastGroundingStatus;

		// Token: 0x0400A911 RID: 43281
		[NonSerialized]
		public LayerMask CollidableLayers = -1;

		// Token: 0x0400A912 RID: 43282
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Transform _transform;

		// Token: 0x0400A913 RID: 43283
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _transientPosition;

		// Token: 0x0400A914 RID: 43284
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterUp;

		// Token: 0x0400A915 RID: 43285
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterForward;

		// Token: 0x0400A916 RID: 43286
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterRight;

		// Token: 0x0400A917 RID: 43287
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _initialSimulationPosition;

		// Token: 0x0400A918 RID: 43288
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _initialSimulationRotation;

		// Token: 0x0400A919 RID: 43289
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Rigidbody _attachedRigidbody;

		// Token: 0x0400A91A RID: 43290
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleCenter;

		// Token: 0x0400A91B RID: 43291
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleBottom;

		// Token: 0x0400A91C RID: 43292
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleTop;

		// Token: 0x0400A91D RID: 43293
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleBottomHemi;

		// Token: 0x0400A91E RID: 43294
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _characterTransformToCapsuleTopHemi;

		// Token: 0x0400A91F RID: 43295
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _attachedRigidbodyVelocity;

		// Token: 0x0400A920 RID: 43296
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _overlapsCount;

		// Token: 0x0400A921 RID: 43297
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public OverlapResult[] _overlaps = new OverlapResult[16];

		// Token: 0x0400A922 RID: 43298
		[NonSerialized]
		public ICharacterController CharacterController;

		// Token: 0x0400A923 RID: 43299
		[NonSerialized]
		public bool LastMovementIterationFoundAnyGround;

		// Token: 0x0400A924 RID: 43300
		[NonSerialized]
		public int IndexInCharacterSystem;

		// Token: 0x0400A925 RID: 43301
		[NonSerialized]
		public Vector3 InitialTickPosition;

		// Token: 0x0400A926 RID: 43302
		[NonSerialized]
		public Quaternion InitialTickRotation;

		// Token: 0x0400A927 RID: 43303
		[NonSerialized]
		public Rigidbody AttachedRigidbodyOverride;

		// Token: 0x0400A928 RID: 43304
		[NonSerialized]
		public Vector3 BaseVelocity;

		// Token: 0x0400A929 RID: 43305
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RaycastHit[] _internalCharacterHits = new RaycastHit[16];

		// Token: 0x0400A92A RID: 43306
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Collider[] _internalProbedColliders = new Collider[16];

		// Token: 0x0400A92B RID: 43307
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Rigidbody[] _rigidbodiesPushedThisMove = new Rigidbody[16];

		// Token: 0x0400A92C RID: 43308
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RigidbodyProjectionHit[] _internalRigidbodyProjectionHits = new RigidbodyProjectionHit[6];

		// Token: 0x0400A92D RID: 43309
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Rigidbody _lastAttachedRigidbody;

		// Token: 0x0400A92E RID: 43310
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _solveMovementCollisions = true;

		// Token: 0x0400A92F RID: 43311
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _solveGrounding = true;

		// Token: 0x0400A930 RID: 43312
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _movePositionDirty;

		// Token: 0x0400A931 RID: 43313
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _movePositionTarget = Vector3.zero;

		// Token: 0x0400A932 RID: 43314
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _moveRotationDirty;

		// Token: 0x0400A933 RID: 43315
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _moveRotationTarget = Quaternion.identity;

		// Token: 0x0400A934 RID: 43316
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _lastSolvedOverlapNormalDirty;

		// Token: 0x0400A935 RID: 43317
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _lastSolvedOverlapNormal = Vector3.forward;

		// Token: 0x0400A936 RID: 43318
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _rigidbodiesPushedCount;

		// Token: 0x0400A937 RID: 43319
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _rigidbodyProjectionHitCount;

		// Token: 0x0400A938 RID: 43320
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float _internalResultingMovementMagnitude;

		// Token: 0x0400A939 RID: 43321
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _internalResultingMovementDirection = Vector3.zero;

		// Token: 0x0400A93A RID: 43322
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _isMovingFromAttachedRigidbody;

		// Token: 0x0400A93B RID: 43323
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool _mustUnground;

		// Token: 0x0400A93C RID: 43324
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float _mustUngroundTimeCounter;

		// Token: 0x0400A93D RID: 43325
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedWorldUp = Vector3.up;

		// Token: 0x0400A93E RID: 43326
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedWorldForward = Vector3.forward;

		// Token: 0x0400A93F RID: 43327
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedWorldRight = Vector3.right;

		// Token: 0x0400A940 RID: 43328
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _cachedZeroVector = Vector3.zero;

		// Token: 0x0400A941 RID: 43329
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _transientRotation;

		// Token: 0x0400A942 RID: 43330
		public const int MaxHitsBudget = 16;

		// Token: 0x0400A943 RID: 43331
		public const int MaxCollisionBudget = 16;

		// Token: 0x0400A944 RID: 43332
		public const int MaxGroundingSweepIterations = 2;

		// Token: 0x0400A945 RID: 43333
		public const int MaxMovementSweepIterations = 6;

		// Token: 0x0400A946 RID: 43334
		public const int MaxSteppingSweepIterations = 3;

		// Token: 0x0400A947 RID: 43335
		public const int MaxRigidbodyOverlapsCount = 16;

		// Token: 0x0400A948 RID: 43336
		public const int MaxDiscreteCollisionIterations = 3;

		// Token: 0x0400A949 RID: 43337
		public const float CollisionOffset = 0.001f;

		// Token: 0x0400A94A RID: 43338
		public const float GroundProbeReboundDistance = 0.02f;

		// Token: 0x0400A94B RID: 43339
		public const float MinimumGroundProbingDistance = 0.005f;

		// Token: 0x0400A94C RID: 43340
		public const float GroundProbingBackstepDistance = 0.1f;

		// Token: 0x0400A94D RID: 43341
		public const float SweepProbingBackstepDistance = 0.002f;

		// Token: 0x0400A94E RID: 43342
		public const float SecondaryProbesVertical = 0.02f;

		// Token: 0x0400A94F RID: 43343
		public const float SecondaryProbesHorizontal = 0.001f;

		// Token: 0x0400A950 RID: 43344
		public const float MinVelocityMagnitude = 0.01f;

		// Token: 0x0400A951 RID: 43345
		public const float SteppingForwardDistance = 0.03f;

		// Token: 0x0400A952 RID: 43346
		public const float MinDistanceForLedge = 0.05f;

		// Token: 0x0400A953 RID: 43347
		public const float CorrelationForVerticalObstruction = 0.01f;

		// Token: 0x0400A954 RID: 43348
		public const float ExtraSteppingForwardDistance = 0.01f;

		// Token: 0x0400A955 RID: 43349
		public const float ExtraStepHeightPadding = 0.01f;
	}
}

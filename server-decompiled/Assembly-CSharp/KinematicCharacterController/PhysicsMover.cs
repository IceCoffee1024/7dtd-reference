using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D33 RID: 7475
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsMover : MonoBehaviour
	{
		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x0600DC85 RID: 56453 RVA: 0x004F2258 File Offset: 0x004F0458
		// (set) Token: 0x0600DC86 RID: 56454 RVA: 0x004F2260 File Offset: 0x004F0460
		public int IndexInCharacterSystem { get; set; }

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x0600DC87 RID: 56455 RVA: 0x004F2269 File Offset: 0x004F0469
		// (set) Token: 0x0600DC88 RID: 56456 RVA: 0x004F2271 File Offset: 0x004F0471
		public Vector3 InitialTickPosition { get; set; }

		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x0600DC89 RID: 56457 RVA: 0x004F227A File Offset: 0x004F047A
		// (set) Token: 0x0600DC8A RID: 56458 RVA: 0x004F2282 File Offset: 0x004F0482
		public Quaternion InitialTickRotation { get; set; }

		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x0600DC8B RID: 56459 RVA: 0x004F228B File Offset: 0x004F048B
		// (set) Token: 0x0600DC8C RID: 56460 RVA: 0x004F2293 File Offset: 0x004F0493
		public Transform Transform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x0600DC8D RID: 56461 RVA: 0x004F229C File Offset: 0x004F049C
		// (set) Token: 0x0600DC8E RID: 56462 RVA: 0x004F22A4 File Offset: 0x004F04A4
		public Vector3 InitialSimulationPosition
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x0600DC8F RID: 56463 RVA: 0x004F22AD File Offset: 0x004F04AD
		// (set) Token: 0x0600DC90 RID: 56464 RVA: 0x004F22B5 File Offset: 0x004F04B5
		public Quaternion InitialSimulationRotation
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600DC91 RID: 56465 RVA: 0x004F22BE File Offset: 0x004F04BE
		// (set) Token: 0x0600DC92 RID: 56466 RVA: 0x004F22C6 File Offset: 0x004F04C6
		public Vector3 TransientPosition
		{
			get
			{
				return this._internalTransientPosition;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this._internalTransientPosition = value;
			}
		}

		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x0600DC93 RID: 56467 RVA: 0x004F22CF File Offset: 0x004F04CF
		// (set) Token: 0x0600DC94 RID: 56468 RVA: 0x004F22D7 File Offset: 0x004F04D7
		public Quaternion TransientRotation
		{
			get
			{
				return this._internalTransientRotation;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this._internalTransientRotation = value;
			}
		}

		// Token: 0x0600DC95 RID: 56469 RVA: 0x004F22E0 File Offset: 0x004F04E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Reset()
		{
			this.ValidateData();
		}

		// Token: 0x0600DC96 RID: 56470 RVA: 0x004F22E0 File Offset: 0x004F04E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnValidate()
		{
			this.ValidateData();
		}

		// Token: 0x0600DC97 RID: 56471 RVA: 0x004F22E8 File Offset: 0x004F04E8
		public void ValidateData()
		{
			this.Rigidbody = base.gameObject.GetComponent<Rigidbody>();
			this.Rigidbody.centerOfMass = Vector3.zero;
			this.Rigidbody.useGravity = false;
			this.Rigidbody.drag = 0f;
			this.Rigidbody.angularDrag = 0f;
			this.Rigidbody.maxAngularVelocity = float.PositiveInfinity;
			this.Rigidbody.maxDepenetrationVelocity = float.PositiveInfinity;
			this.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
			this.Rigidbody.isKinematic = true;
			this.Rigidbody.constraints = RigidbodyConstraints.None;
			this.Rigidbody.interpolation = RigidbodyInterpolation.None;
		}

		// Token: 0x0600DC98 RID: 56472 RVA: 0x004F2392 File Offset: 0x004F0592
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			KinematicCharacterSystem.EnsureCreation();
			KinematicCharacterSystem.RegisterPhysicsMover(this);
		}

		// Token: 0x0600DC99 RID: 56473 RVA: 0x004F239F File Offset: 0x004F059F
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			KinematicCharacterSystem.UnregisterPhysicsMover(this);
		}

		// Token: 0x0600DC9A RID: 56474 RVA: 0x004F23A8 File Offset: 0x004F05A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.Transform = base.transform;
			this.ValidateData();
			this.TransientPosition = this.Rigidbody.position;
			this.TransientRotation = this.Rigidbody.rotation;
			this.InitialSimulationPosition = this.Rigidbody.position;
			this.InitialSimulationRotation = this.Rigidbody.rotation;
		}

		// Token: 0x0600DC9B RID: 56475 RVA: 0x004F240B File Offset: 0x004F060B
		public void SetPosition(Vector3 position)
		{
			this.Transform.position = position;
			this.Rigidbody.position = position;
			this.InitialSimulationPosition = position;
			this.TransientPosition = position;
		}

		// Token: 0x0600DC9C RID: 56476 RVA: 0x004F2433 File Offset: 0x004F0633
		public void SetRotation(Quaternion rotation)
		{
			this.Transform.rotation = rotation;
			this.Rigidbody.rotation = rotation;
			this.InitialSimulationRotation = rotation;
			this.TransientRotation = rotation;
		}

		// Token: 0x0600DC9D RID: 56477 RVA: 0x004F245C File Offset: 0x004F065C
		public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			this.Transform.SetPositionAndRotation(position, rotation);
			this.Rigidbody.position = position;
			this.Rigidbody.rotation = rotation;
			this.InitialSimulationPosition = position;
			this.InitialSimulationRotation = rotation;
			this.TransientPosition = position;
			this.TransientRotation = rotation;
		}

		// Token: 0x0600DC9E RID: 56478 RVA: 0x004F24AC File Offset: 0x004F06AC
		public PhysicsMoverState GetState()
		{
			return new PhysicsMoverState
			{
				Position = this.TransientPosition,
				Rotation = this.TransientRotation,
				Velocity = this.Rigidbody.velocity,
				AngularVelocity = this.Rigidbody.velocity
			};
		}

		// Token: 0x0600DC9F RID: 56479 RVA: 0x004F2500 File Offset: 0x004F0700
		public void ApplyState(PhysicsMoverState state)
		{
			this.SetPositionAndRotation(state.Position, state.Rotation);
			this.Rigidbody.velocity = state.Velocity;
			this.Rigidbody.angularVelocity = state.AngularVelocity;
		}

		// Token: 0x0600DCA0 RID: 56480 RVA: 0x004F2538 File Offset: 0x004F0738
		public void VelocityUpdate(float deltaTime)
		{
			this.InitialSimulationPosition = this.TransientPosition;
			this.InitialSimulationRotation = this.TransientRotation;
			this.MoverController.UpdateMovement(out this._internalTransientPosition, out this._internalTransientRotation, deltaTime);
			if (deltaTime > 0f)
			{
				this.Rigidbody.velocity = (this.TransientPosition - this.InitialSimulationPosition) / deltaTime;
				Quaternion quaternion = this.TransientRotation * Quaternion.Inverse(this.InitialSimulationRotation);
				this.Rigidbody.angularVelocity = 0.017453292f * quaternion.eulerAngles / deltaTime;
			}
		}

		// Token: 0x0400A757 RID: 42839
		[ReadOnly]
		public Rigidbody Rigidbody;

		// Token: 0x0400A758 RID: 42840
		[NonSerialized]
		public IMoverController MoverController;

		// Token: 0x0400A75F RID: 42847
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _internalTransientPosition;

		// Token: 0x0400A760 RID: 42848
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _internalTransientRotation;
	}
}

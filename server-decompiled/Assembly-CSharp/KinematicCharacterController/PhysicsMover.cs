using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DBE RID: 7614
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsMover : MonoBehaviour
	{
		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x0600E0B5 RID: 57525 RVA: 0x005014B4 File Offset: 0x004FF6B4
		// (set) Token: 0x0600E0B6 RID: 57526 RVA: 0x005014BC File Offset: 0x004FF6BC
		public int IndexInCharacterSystem { get; set; }

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x0600E0B7 RID: 57527 RVA: 0x005014C5 File Offset: 0x004FF6C5
		// (set) Token: 0x0600E0B8 RID: 57528 RVA: 0x005014CD File Offset: 0x004FF6CD
		public Vector3 InitialTickPosition { get; set; }

		// Token: 0x17001CA9 RID: 7337
		// (get) Token: 0x0600E0B9 RID: 57529 RVA: 0x005014D6 File Offset: 0x004FF6D6
		// (set) Token: 0x0600E0BA RID: 57530 RVA: 0x005014DE File Offset: 0x004FF6DE
		public Quaternion InitialTickRotation { get; set; }

		// Token: 0x17001CAA RID: 7338
		// (get) Token: 0x0600E0BB RID: 57531 RVA: 0x005014E7 File Offset: 0x004FF6E7
		// (set) Token: 0x0600E0BC RID: 57532 RVA: 0x005014EF File Offset: 0x004FF6EF
		public Transform Transform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001CAB RID: 7339
		// (get) Token: 0x0600E0BD RID: 57533 RVA: 0x005014F8 File Offset: 0x004FF6F8
		// (set) Token: 0x0600E0BE RID: 57534 RVA: 0x00501500 File Offset: 0x004FF700
		public Vector3 InitialSimulationPosition
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001CAC RID: 7340
		// (get) Token: 0x0600E0BF RID: 57535 RVA: 0x00501509 File Offset: 0x004FF709
		// (set) Token: 0x0600E0C0 RID: 57536 RVA: 0x00501511 File Offset: 0x004FF711
		public Quaternion InitialSimulationRotation
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x0600E0C1 RID: 57537 RVA: 0x0050151A File Offset: 0x004FF71A
		// (set) Token: 0x0600E0C2 RID: 57538 RVA: 0x00501522 File Offset: 0x004FF722
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

		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x0600E0C3 RID: 57539 RVA: 0x0050152B File Offset: 0x004FF72B
		// (set) Token: 0x0600E0C4 RID: 57540 RVA: 0x00501533 File Offset: 0x004FF733
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

		// Token: 0x0600E0C5 RID: 57541 RVA: 0x0050153C File Offset: 0x004FF73C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Reset()
		{
			this.ValidateData();
		}

		// Token: 0x0600E0C6 RID: 57542 RVA: 0x0050153C File Offset: 0x004FF73C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnValidate()
		{
			this.ValidateData();
		}

		// Token: 0x0600E0C7 RID: 57543 RVA: 0x00501544 File Offset: 0x004FF744
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

		// Token: 0x0600E0C8 RID: 57544 RVA: 0x005015EE File Offset: 0x004FF7EE
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			KinematicCharacterSystem.EnsureCreation();
			KinematicCharacterSystem.RegisterPhysicsMover(this);
		}

		// Token: 0x0600E0C9 RID: 57545 RVA: 0x005015FB File Offset: 0x004FF7FB
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			KinematicCharacterSystem.UnregisterPhysicsMover(this);
		}

		// Token: 0x0600E0CA RID: 57546 RVA: 0x00501604 File Offset: 0x004FF804
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

		// Token: 0x0600E0CB RID: 57547 RVA: 0x00501667 File Offset: 0x004FF867
		public void SetPosition(Vector3 position)
		{
			this.Transform.position = position;
			this.Rigidbody.position = position;
			this.InitialSimulationPosition = position;
			this.TransientPosition = position;
		}

		// Token: 0x0600E0CC RID: 57548 RVA: 0x0050168F File Offset: 0x004FF88F
		public void SetRotation(Quaternion rotation)
		{
			this.Transform.rotation = rotation;
			this.Rigidbody.rotation = rotation;
			this.InitialSimulationRotation = rotation;
			this.TransientRotation = rotation;
		}

		// Token: 0x0600E0CD RID: 57549 RVA: 0x005016B8 File Offset: 0x004FF8B8
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

		// Token: 0x0600E0CE RID: 57550 RVA: 0x00501708 File Offset: 0x004FF908
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

		// Token: 0x0400AA4C RID: 43596
		[ReadOnly]
		public Rigidbody Rigidbody;

		// Token: 0x0400AA4D RID: 43597
		[NonSerialized]
		public IMoverController MoverController;

		// Token: 0x0400AA54 RID: 43604
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _internalTransientPosition;

		// Token: 0x0400AA55 RID: 43605
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _internalTransientRotation;
	}
}

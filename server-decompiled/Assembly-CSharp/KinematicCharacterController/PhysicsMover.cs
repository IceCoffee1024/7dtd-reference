using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D5B RID: 7515
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsMover : MonoBehaviour
	{
		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x0600DE26 RID: 56870 RVA: 0x004F8CCC File Offset: 0x004F6ECC
		// (set) Token: 0x0600DE27 RID: 56871 RVA: 0x004F8CD4 File Offset: 0x004F6ED4
		public int IndexInCharacterSystem { get; set; }

		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x0600DE28 RID: 56872 RVA: 0x004F8CDD File Offset: 0x004F6EDD
		// (set) Token: 0x0600DE29 RID: 56873 RVA: 0x004F8CE5 File Offset: 0x004F6EE5
		public Vector3 InitialTickPosition { get; set; }

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x0600DE2A RID: 56874 RVA: 0x004F8CEE File Offset: 0x004F6EEE
		// (set) Token: 0x0600DE2B RID: 56875 RVA: 0x004F8CF6 File Offset: 0x004F6EF6
		public Quaternion InitialTickRotation { get; set; }

		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x0600DE2C RID: 56876 RVA: 0x004F8CFF File Offset: 0x004F6EFF
		// (set) Token: 0x0600DE2D RID: 56877 RVA: 0x004F8D07 File Offset: 0x004F6F07
		public Transform Transform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x0600DE2E RID: 56878 RVA: 0x004F8D10 File Offset: 0x004F6F10
		// (set) Token: 0x0600DE2F RID: 56879 RVA: 0x004F8D18 File Offset: 0x004F6F18
		public Vector3 InitialSimulationPosition
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x0600DE30 RID: 56880 RVA: 0x004F8D21 File Offset: 0x004F6F21
		// (set) Token: 0x0600DE31 RID: 56881 RVA: 0x004F8D29 File Offset: 0x004F6F29
		public Quaternion InitialSimulationRotation
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x0600DE32 RID: 56882 RVA: 0x004F8D32 File Offset: 0x004F6F32
		// (set) Token: 0x0600DE33 RID: 56883 RVA: 0x004F8D3A File Offset: 0x004F6F3A
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

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x0600DE34 RID: 56884 RVA: 0x004F8D43 File Offset: 0x004F6F43
		// (set) Token: 0x0600DE35 RID: 56885 RVA: 0x004F8D4B File Offset: 0x004F6F4B
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

		// Token: 0x0600DE36 RID: 56886 RVA: 0x004F8D54 File Offset: 0x004F6F54
		[PublicizedFrom(EAccessModifier.Private)]
		public void Reset()
		{
			this.ValidateData();
		}

		// Token: 0x0600DE37 RID: 56887 RVA: 0x004F8D54 File Offset: 0x004F6F54
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnValidate()
		{
			this.ValidateData();
		}

		// Token: 0x0600DE38 RID: 56888 RVA: 0x004F8D5C File Offset: 0x004F6F5C
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

		// Token: 0x0600DE39 RID: 56889 RVA: 0x004F8E06 File Offset: 0x004F7006
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			KinematicCharacterSystem.EnsureCreation();
			KinematicCharacterSystem.RegisterPhysicsMover(this);
		}

		// Token: 0x0600DE3A RID: 56890 RVA: 0x004F8E13 File Offset: 0x004F7013
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			KinematicCharacterSystem.UnregisterPhysicsMover(this);
		}

		// Token: 0x0600DE3B RID: 56891 RVA: 0x004F8E1C File Offset: 0x004F701C
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

		// Token: 0x0600DE3C RID: 56892 RVA: 0x004F8E7F File Offset: 0x004F707F
		public void SetPosition(Vector3 position)
		{
			this.Transform.position = position;
			this.Rigidbody.position = position;
			this.InitialSimulationPosition = position;
			this.TransientPosition = position;
		}

		// Token: 0x0600DE3D RID: 56893 RVA: 0x004F8EA7 File Offset: 0x004F70A7
		public void SetRotation(Quaternion rotation)
		{
			this.Transform.rotation = rotation;
			this.Rigidbody.rotation = rotation;
			this.InitialSimulationRotation = rotation;
			this.TransientRotation = rotation;
		}

		// Token: 0x0600DE3E RID: 56894 RVA: 0x004F8ED0 File Offset: 0x004F70D0
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

		// Token: 0x0600DE3F RID: 56895 RVA: 0x004F8F20 File Offset: 0x004F7120
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

		// Token: 0x0600DE40 RID: 56896 RVA: 0x004F8F74 File Offset: 0x004F7174
		public void ApplyState(PhysicsMoverState state)
		{
			this.SetPositionAndRotation(state.Position, state.Rotation);
			this.Rigidbody.velocity = state.Velocity;
			this.Rigidbody.angularVelocity = state.AngularVelocity;
		}

		// Token: 0x0600DE41 RID: 56897 RVA: 0x004F8FAC File Offset: 0x004F71AC
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

		// Token: 0x0400A88C RID: 43148
		[ReadOnly]
		public Rigidbody Rigidbody;

		// Token: 0x0400A88D RID: 43149
		[NonSerialized]
		public IMoverController MoverController;

		// Token: 0x0400A894 RID: 43156
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _internalTransientPosition;

		// Token: 0x0400A895 RID: 43157
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _internalTransientRotation;
	}
}

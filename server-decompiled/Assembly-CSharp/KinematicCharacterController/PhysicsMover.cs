using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D8D RID: 7565
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsMover : MonoBehaviour
	{
		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x0600DFA1 RID: 57249 RVA: 0x004FD914 File Offset: 0x004FBB14
		// (set) Token: 0x0600DFA2 RID: 57250 RVA: 0x004FD91C File Offset: 0x004FBB1C
		public int IndexInCharacterSystem { get; set; }

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x0600DFA3 RID: 57251 RVA: 0x004FD925 File Offset: 0x004FBB25
		// (set) Token: 0x0600DFA4 RID: 57252 RVA: 0x004FD92D File Offset: 0x004FBB2D
		public Vector3 InitialTickPosition { get; set; }

		// Token: 0x17001C61 RID: 7265
		// (get) Token: 0x0600DFA5 RID: 57253 RVA: 0x004FD936 File Offset: 0x004FBB36
		// (set) Token: 0x0600DFA6 RID: 57254 RVA: 0x004FD93E File Offset: 0x004FBB3E
		public Quaternion InitialTickRotation { get; set; }

		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x0600DFA7 RID: 57255 RVA: 0x004FD947 File Offset: 0x004FBB47
		// (set) Token: 0x0600DFA8 RID: 57256 RVA: 0x004FD94F File Offset: 0x004FBB4F
		public Transform Transform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x0600DFA9 RID: 57257 RVA: 0x004FD958 File Offset: 0x004FBB58
		// (set) Token: 0x0600DFAA RID: 57258 RVA: 0x004FD960 File Offset: 0x004FBB60
		public Vector3 InitialSimulationPosition
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x0600DFAB RID: 57259 RVA: 0x004FD969 File Offset: 0x004FBB69
		// (set) Token: 0x0600DFAC RID: 57260 RVA: 0x004FD971 File Offset: 0x004FBB71
		public Quaternion InitialSimulationRotation
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x0600DFAD RID: 57261 RVA: 0x004FD97A File Offset: 0x004FBB7A
		// (set) Token: 0x0600DFAE RID: 57262 RVA: 0x004FD982 File Offset: 0x004FBB82
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

		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x0600DFAF RID: 57263 RVA: 0x004FD98B File Offset: 0x004FBB8B
		// (set) Token: 0x0600DFB0 RID: 57264 RVA: 0x004FD993 File Offset: 0x004FBB93
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

		// Token: 0x0600DFB1 RID: 57265 RVA: 0x004FD99C File Offset: 0x004FBB9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Reset()
		{
			this.ValidateData();
		}

		// Token: 0x0600DFB2 RID: 57266 RVA: 0x004FD99C File Offset: 0x004FBB9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnValidate()
		{
			this.ValidateData();
		}

		// Token: 0x0600DFB3 RID: 57267 RVA: 0x004FD9A4 File Offset: 0x004FBBA4
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

		// Token: 0x0600DFB4 RID: 57268 RVA: 0x004FDA4E File Offset: 0x004FBC4E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			KinematicCharacterSystem.EnsureCreation();
			KinematicCharacterSystem.RegisterPhysicsMover(this);
		}

		// Token: 0x0600DFB5 RID: 57269 RVA: 0x004FDA5B File Offset: 0x004FBC5B
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			KinematicCharacterSystem.UnregisterPhysicsMover(this);
		}

		// Token: 0x0600DFB6 RID: 57270 RVA: 0x004FDA64 File Offset: 0x004FBC64
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

		// Token: 0x0600DFB7 RID: 57271 RVA: 0x004FDAC7 File Offset: 0x004FBCC7
		public void SetPosition(Vector3 position)
		{
			this.Transform.position = position;
			this.Rigidbody.position = position;
			this.InitialSimulationPosition = position;
			this.TransientPosition = position;
		}

		// Token: 0x0600DFB8 RID: 57272 RVA: 0x004FDAEF File Offset: 0x004FBCEF
		public void SetRotation(Quaternion rotation)
		{
			this.Transform.rotation = rotation;
			this.Rigidbody.rotation = rotation;
			this.InitialSimulationRotation = rotation;
			this.TransientRotation = rotation;
		}

		// Token: 0x0600DFB9 RID: 57273 RVA: 0x004FDB18 File Offset: 0x004FBD18
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

		// Token: 0x0600DFBA RID: 57274 RVA: 0x004FDB68 File Offset: 0x004FBD68
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

		// Token: 0x0400A963 RID: 43363
		[ReadOnly]
		public Rigidbody Rigidbody;

		// Token: 0x0400A964 RID: 43364
		[NonSerialized]
		public IMoverController MoverController;

		// Token: 0x0400A96B RID: 43371
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 _internalTransientPosition;

		// Token: 0x0400A96C RID: 43372
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion _internalTransientRotation;
	}
}

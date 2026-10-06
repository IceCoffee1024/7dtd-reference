using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200162D RID: 5677
public class vp_RagdollHandler : MonoBehaviour
{
	// Token: 0x170014FB RID: 5371
	// (get) Token: 0x0600B1F0 RID: 45552 RVA: 0x00427567 File Offset: 0x00425767
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null && !this.m_TriedToFetchPlayer)
			{
				this.m_Player = base.transform.root.GetComponentInChildren<vp_PlayerEventHandler>();
				this.m_TriedToFetchPlayer = true;
			}
			return this.m_Player;
		}
	}

	// Token: 0x170014FC RID: 5372
	// (get) Token: 0x0600B1F1 RID: 45553 RVA: 0x004275A2 File Offset: 0x004257A2
	public vp_FPCamera FPCamera
	{
		get
		{
			if (this.m_FPCamera == null)
			{
				this.m_FPCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
			}
			return this.m_FPCamera;
		}
	}

	// Token: 0x170014FD RID: 5373
	// (get) Token: 0x0600B1F2 RID: 45554 RVA: 0x004275CE File Offset: 0x004257CE
	public CharacterController CharacterController
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CharacterController == null)
			{
				this.m_CharacterController = base.transform.root.GetComponentInChildren<CharacterController>();
			}
			return this.m_CharacterController;
		}
	}

	// Token: 0x170014FE RID: 5374
	// (get) Token: 0x0600B1F3 RID: 45555 RVA: 0x004275FC File Offset: 0x004257FC
	public List<Collider> Colliders
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Colliders == null)
			{
				this.m_Colliders = new List<Collider>();
				foreach (Collider collider in base.GetComponentsInChildren<Collider>())
				{
					if (collider.gameObject.layer != 23)
					{
						this.m_Colliders.Add(collider);
					}
				}
			}
			return this.m_Colliders;
		}
	}

	// Token: 0x170014FF RID: 5375
	// (get) Token: 0x0600B1F4 RID: 45556 RVA: 0x00427656 File Offset: 0x00425856
	public List<Rigidbody> Rigidbodies
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Rigidbodies == null)
			{
				this.m_Rigidbodies = new List<Rigidbody>(base.GetComponentsInChildren<Rigidbody>());
			}
			return this.m_Rigidbodies;
		}
	}

	// Token: 0x17001500 RID: 5376
	// (get) Token: 0x0600B1F5 RID: 45557 RVA: 0x00427678 File Offset: 0x00425878
	public List<Transform> Transforms
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Transforms == null)
			{
				this.m_Transforms = new List<Transform>();
				foreach (Rigidbody rigidbody in this.Rigidbodies)
				{
					this.m_Transforms.Add(rigidbody.transform);
				}
			}
			return this.m_Transforms;
		}
	}

	// Token: 0x17001501 RID: 5377
	// (get) Token: 0x0600B1F6 RID: 45558 RVA: 0x004276F0 File Offset: 0x004258F0
	public Animator Animator
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Animator == null)
			{
				this.m_Animator = base.GetComponent<Animator>();
			}
			return this.m_Animator;
		}
	}

	// Token: 0x17001502 RID: 5378
	// (get) Token: 0x0600B1F7 RID: 45559 RVA: 0x00427712 File Offset: 0x00425912
	public vp_BodyAnimator BodyAnimator
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_BodyAnimator == null)
			{
				this.m_BodyAnimator = base.GetComponent<vp_BodyAnimator>();
			}
			return this.m_BodyAnimator;
		}
	}

	// Token: 0x0600B1F8 RID: 45560 RVA: 0x00427734 File Offset: 0x00425934
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (this.Colliders == null || this.Colliders.Count == 0 || this.Rigidbodies == null || this.Rigidbodies.Count == 0 || this.Transforms == null || this.Transforms.Count == 0 || this.Animator == null || this.BodyAnimator == null)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") Could not be initialized. Please make sure hierarchy has ragdoll colliders, Animator and vp_BodyAnimator.");
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600B1F9 RID: 45561 RVA: 0x004277C5 File Offset: 0x004259C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.SetRagdoll(false);
	}

	// Token: 0x0600B1FA RID: 45562 RVA: 0x004277D0 File Offset: 0x004259D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SaveStartPose()
	{
		foreach (Transform transform in this.Transforms)
		{
			if (!this.TransformRotations.ContainsKey(transform))
			{
				this.TransformRotations.Add(transform.transform, transform.localRotation);
			}
			if (!this.TransformPositions.ContainsKey(transform))
			{
				this.TransformPositions.Add(transform.transform, transform.localPosition);
			}
		}
	}

	// Token: 0x0600B1FB RID: 45563 RVA: 0x00427868 File Offset: 0x00425A68
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RestoreStartPose()
	{
		foreach (Transform transform in this.Transforms)
		{
			if (this.TransformRotations.TryGetValue(transform, out this.m_Rot))
			{
				transform.localRotation = this.m_Rot;
			}
			if (this.TransformPositions.TryGetValue(transform, out this.m_Pos))
			{
				transform.localPosition = this.m_Pos;
			}
		}
	}

	// Token: 0x0600B1FC RID: 45564 RVA: 0x004278F4 File Offset: 0x00425AF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B1FD RID: 45565 RVA: 0x00427910 File Offset: 0x00425B10
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1FE RID: 45566 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x0600B1FF RID: 45567 RVA: 0x0042792C File Offset: 0x00425B2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.UpdateDeathCamera();
	}

	// Token: 0x0600B200 RID: 45568 RVA: 0x00427934 File Offset: 0x00425B34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateDeathCamera()
	{
		if (this.Player == null)
		{
			return;
		}
		if (!this.Player.Dead.Active)
		{
			return;
		}
		if (this.HeadBone == null)
		{
			return;
		}
		if (!this.Player.IsFirstPerson.Get())
		{
			return;
		}
		this.FPCamera.Transform.position = this.HeadBone.transform.position;
		this.m_HeadRotationCorrection = this.HeadBone.transform.localEulerAngles;
		if (Time.time - this.m_TimeOfDeath < this.CameraFreezeDelay)
		{
			this.FPCamera.Transform.localEulerAngles = (this.m_CameraFreezeAngle = new Vector3(-this.m_HeadRotationCorrection.z, -this.m_HeadRotationCorrection.x, this.m_HeadRotationCorrection.y));
			return;
		}
		this.FPCamera.Transform.localEulerAngles = this.m_CameraFreezeAngle;
	}

	// Token: 0x0600B201 RID: 45569 RVA: 0x00427A2C File Offset: 0x00425C2C
	public virtual void SetRagdoll(bool enabled = true)
	{
		if (this.Animator != null)
		{
			this.Animator.enabled = !enabled;
		}
		if (this.BodyAnimator != null)
		{
			this.BodyAnimator.enabled = !enabled;
		}
		if (this.CharacterController != null)
		{
			this.CharacterController.enabled = !enabled;
		}
		foreach (Rigidbody rigidbody in this.Rigidbodies)
		{
			rigidbody.isKinematic = !enabled;
		}
		foreach (Collider collider in this.Colliders)
		{
			collider.enabled = enabled;
		}
		if (!enabled)
		{
			this.RestoreStartPose();
		}
	}

	// Token: 0x0600B202 RID: 45570 RVA: 0x00427B20 File Offset: 0x00425D20
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		this.SetRagdoll(true);
		foreach (Rigidbody rigidbody in this.Rigidbodies)
		{
			rigidbody.AddForce(this.Player.Velocity.Get() * this.VelocityMultiplier);
		}
		this.m_TimeOfDeath = Time.time;
	}

	// Token: 0x0600B203 RID: 45571 RVA: 0x00427BA4 File Offset: 0x00425DA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.SetRagdoll(false);
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x040085A0 RID: 34208
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;

	// Token: 0x040085A1 RID: 34209
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Rigidbody> m_Rigidbodies;

	// Token: 0x040085A2 RID: 34210
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Transforms;

	// Token: 0x040085A3 RID: 34211
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x040085A4 RID: 34212
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_BodyAnimator m_BodyAnimator;

	// Token: 0x040085A5 RID: 34213
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040085A6 RID: 34214
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x040085A7 RID: 34215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x040085A8 RID: 34216
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadRotationCorrection = Vector3.zero;

	// Token: 0x040085A9 RID: 34217
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TimeOfDeath;

	// Token: 0x040085AA RID: 34218
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraFreezeAngle = Vector3.zero;

	// Token: 0x040085AB RID: 34219
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Quaternion> TransformRotations = new Dictionary<Transform, Quaternion>();

	// Token: 0x040085AC RID: 34220
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Vector3> TransformPositions = new Dictionary<Transform, Vector3>();

	// Token: 0x040085AD RID: 34221
	public float VelocityMultiplier = 30f;

	// Token: 0x040085AE RID: 34222
	public float CameraFreezeDelay = 2.5f;

	// Token: 0x040085AF RID: 34223
	public GameObject HeadBone;

	// Token: 0x040085B0 RID: 34224
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_Rot;

	// Token: 0x040085B1 RID: 34225
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Pos;

	// Token: 0x040085B2 RID: 34226
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_TriedToFetchPlayer;
}

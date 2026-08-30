using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200160E RID: 5646
public class vp_RagdollHandler : MonoBehaviour
{
	// Token: 0x170014B8 RID: 5304
	// (get) Token: 0x0600B15E RID: 45406 RVA: 0x0042388B File Offset: 0x00421A8B
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

	// Token: 0x170014B9 RID: 5305
	// (get) Token: 0x0600B15F RID: 45407 RVA: 0x004238C6 File Offset: 0x00421AC6
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

	// Token: 0x170014BA RID: 5306
	// (get) Token: 0x0600B160 RID: 45408 RVA: 0x004238F2 File Offset: 0x00421AF2
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

	// Token: 0x170014BB RID: 5307
	// (get) Token: 0x0600B161 RID: 45409 RVA: 0x00423920 File Offset: 0x00421B20
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

	// Token: 0x170014BC RID: 5308
	// (get) Token: 0x0600B162 RID: 45410 RVA: 0x0042397A File Offset: 0x00421B7A
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

	// Token: 0x170014BD RID: 5309
	// (get) Token: 0x0600B163 RID: 45411 RVA: 0x0042399C File Offset: 0x00421B9C
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

	// Token: 0x170014BE RID: 5310
	// (get) Token: 0x0600B164 RID: 45412 RVA: 0x00423A14 File Offset: 0x00421C14
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

	// Token: 0x170014BF RID: 5311
	// (get) Token: 0x0600B165 RID: 45413 RVA: 0x00423A36 File Offset: 0x00421C36
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

	// Token: 0x0600B166 RID: 45414 RVA: 0x00423A58 File Offset: 0x00421C58
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

	// Token: 0x0600B167 RID: 45415 RVA: 0x00423AE9 File Offset: 0x00421CE9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.SetRagdoll(false);
	}

	// Token: 0x0600B168 RID: 45416 RVA: 0x00423AF4 File Offset: 0x00421CF4
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

	// Token: 0x0600B169 RID: 45417 RVA: 0x00423B8C File Offset: 0x00421D8C
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

	// Token: 0x0600B16A RID: 45418 RVA: 0x00423C18 File Offset: 0x00421E18
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B16B RID: 45419 RVA: 0x00423C34 File Offset: 0x00421E34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B16C RID: 45420 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x0600B16D RID: 45421 RVA: 0x00423C50 File Offset: 0x00421E50
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.UpdateDeathCamera();
	}

	// Token: 0x0600B16E RID: 45422 RVA: 0x00423C58 File Offset: 0x00421E58
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

	// Token: 0x0600B16F RID: 45423 RVA: 0x00423D50 File Offset: 0x00421F50
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

	// Token: 0x0600B170 RID: 45424 RVA: 0x00423E44 File Offset: 0x00422044
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

	// Token: 0x0600B171 RID: 45425 RVA: 0x00423EC8 File Offset: 0x004220C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.SetRagdoll(false);
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x0400854A RID: 34122
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;

	// Token: 0x0400854B RID: 34123
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Rigidbody> m_Rigidbodies;

	// Token: 0x0400854C RID: 34124
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Transforms;

	// Token: 0x0400854D RID: 34125
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x0400854E RID: 34126
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_BodyAnimator m_BodyAnimator;

	// Token: 0x0400854F RID: 34127
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008550 RID: 34128
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x04008551 RID: 34129
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x04008552 RID: 34130
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadRotationCorrection = Vector3.zero;

	// Token: 0x04008553 RID: 34131
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TimeOfDeath;

	// Token: 0x04008554 RID: 34132
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraFreezeAngle = Vector3.zero;

	// Token: 0x04008555 RID: 34133
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Quaternion> TransformRotations = new Dictionary<Transform, Quaternion>();

	// Token: 0x04008556 RID: 34134
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Vector3> TransformPositions = new Dictionary<Transform, Vector3>();

	// Token: 0x04008557 RID: 34135
	public float VelocityMultiplier = 30f;

	// Token: 0x04008558 RID: 34136
	public float CameraFreezeDelay = 2.5f;

	// Token: 0x04008559 RID: 34137
	public GameObject HeadBone;

	// Token: 0x0400855A RID: 34138
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_Rot;

	// Token: 0x0400855B RID: 34139
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Pos;

	// Token: 0x0400855C RID: 34140
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_TriedToFetchPlayer;
}

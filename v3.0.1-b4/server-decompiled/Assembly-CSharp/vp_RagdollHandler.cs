using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015F3 RID: 5619
public class vp_RagdollHandler : MonoBehaviour
{
	// Token: 0x1700147F RID: 5247
	// (get) Token: 0x0600B061 RID: 45153 RVA: 0x0041E54B File Offset: 0x0041C74B
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

	// Token: 0x17001480 RID: 5248
	// (get) Token: 0x0600B062 RID: 45154 RVA: 0x0041E586 File Offset: 0x0041C786
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

	// Token: 0x17001481 RID: 5249
	// (get) Token: 0x0600B063 RID: 45155 RVA: 0x0041E5B2 File Offset: 0x0041C7B2
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

	// Token: 0x17001482 RID: 5250
	// (get) Token: 0x0600B064 RID: 45156 RVA: 0x0041E5E0 File Offset: 0x0041C7E0
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

	// Token: 0x17001483 RID: 5251
	// (get) Token: 0x0600B065 RID: 45157 RVA: 0x0041E63A File Offset: 0x0041C83A
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

	// Token: 0x17001484 RID: 5252
	// (get) Token: 0x0600B066 RID: 45158 RVA: 0x0041E65C File Offset: 0x0041C85C
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

	// Token: 0x17001485 RID: 5253
	// (get) Token: 0x0600B067 RID: 45159 RVA: 0x0041E6D4 File Offset: 0x0041C8D4
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

	// Token: 0x17001486 RID: 5254
	// (get) Token: 0x0600B068 RID: 45160 RVA: 0x0041E6F6 File Offset: 0x0041C8F6
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

	// Token: 0x0600B069 RID: 45161 RVA: 0x0041E718 File Offset: 0x0041C918
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

	// Token: 0x0600B06A RID: 45162 RVA: 0x0041E7A9 File Offset: 0x0041C9A9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.SetRagdoll(false);
	}

	// Token: 0x0600B06B RID: 45163 RVA: 0x0041E7B4 File Offset: 0x0041C9B4
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

	// Token: 0x0600B06C RID: 45164 RVA: 0x0041E84C File Offset: 0x0041CA4C
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

	// Token: 0x0600B06D RID: 45165 RVA: 0x0041E8D8 File Offset: 0x0041CAD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B06E RID: 45166 RVA: 0x0041E8F4 File Offset: 0x0041CAF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B06F RID: 45167 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x0600B070 RID: 45168 RVA: 0x0041E910 File Offset: 0x0041CB10
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.UpdateDeathCamera();
	}

	// Token: 0x0600B071 RID: 45169 RVA: 0x0041E918 File Offset: 0x0041CB18
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

	// Token: 0x0600B072 RID: 45170 RVA: 0x0041EA10 File Offset: 0x0041CC10
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

	// Token: 0x0600B073 RID: 45171 RVA: 0x0041EB04 File Offset: 0x0041CD04
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

	// Token: 0x0600B074 RID: 45172 RVA: 0x0041EB88 File Offset: 0x0041CD88
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.SetRagdoll(false);
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x04008471 RID: 33905
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;

	// Token: 0x04008472 RID: 33906
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Rigidbody> m_Rigidbodies;

	// Token: 0x04008473 RID: 33907
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Transforms;

	// Token: 0x04008474 RID: 33908
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x04008475 RID: 33909
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_BodyAnimator m_BodyAnimator;

	// Token: 0x04008476 RID: 33910
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008477 RID: 33911
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x04008478 RID: 33912
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x04008479 RID: 33913
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadRotationCorrection = Vector3.zero;

	// Token: 0x0400847A RID: 33914
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TimeOfDeath;

	// Token: 0x0400847B RID: 33915
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraFreezeAngle = Vector3.zero;

	// Token: 0x0400847C RID: 33916
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Quaternion> TransformRotations = new Dictionary<Transform, Quaternion>();

	// Token: 0x0400847D RID: 33917
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Vector3> TransformPositions = new Dictionary<Transform, Vector3>();

	// Token: 0x0400847E RID: 33918
	public float VelocityMultiplier = 30f;

	// Token: 0x0400847F RID: 33919
	public float CameraFreezeDelay = 2.5f;

	// Token: 0x04008480 RID: 33920
	public GameObject HeadBone;

	// Token: 0x04008481 RID: 33921
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_Rot;

	// Token: 0x04008482 RID: 33922
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Pos;

	// Token: 0x04008483 RID: 33923
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_TriedToFetchPlayer;
}

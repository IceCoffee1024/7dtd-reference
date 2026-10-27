using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200163F RID: 5695
public class vp_RagdollHandler : MonoBehaviour
{
	// Token: 0x17001507 RID: 5383
	// (get) Token: 0x0600B248 RID: 45640 RVA: 0x0042996B File Offset: 0x00427B6B
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

	// Token: 0x17001508 RID: 5384
	// (get) Token: 0x0600B249 RID: 45641 RVA: 0x004299A6 File Offset: 0x00427BA6
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

	// Token: 0x17001509 RID: 5385
	// (get) Token: 0x0600B24A RID: 45642 RVA: 0x004299D2 File Offset: 0x00427BD2
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

	// Token: 0x1700150A RID: 5386
	// (get) Token: 0x0600B24B RID: 45643 RVA: 0x00429A00 File Offset: 0x00427C00
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

	// Token: 0x1700150B RID: 5387
	// (get) Token: 0x0600B24C RID: 45644 RVA: 0x00429A5A File Offset: 0x00427C5A
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

	// Token: 0x1700150C RID: 5388
	// (get) Token: 0x0600B24D RID: 45645 RVA: 0x00429A7C File Offset: 0x00427C7C
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

	// Token: 0x1700150D RID: 5389
	// (get) Token: 0x0600B24E RID: 45646 RVA: 0x00429AF4 File Offset: 0x00427CF4
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

	// Token: 0x1700150E RID: 5390
	// (get) Token: 0x0600B24F RID: 45647 RVA: 0x00429B16 File Offset: 0x00427D16
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

	// Token: 0x0600B250 RID: 45648 RVA: 0x00429B38 File Offset: 0x00427D38
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

	// Token: 0x0600B251 RID: 45649 RVA: 0x00429BC9 File Offset: 0x00427DC9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.SetRagdoll(false);
	}

	// Token: 0x0600B252 RID: 45650 RVA: 0x00429BD4 File Offset: 0x00427DD4
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

	// Token: 0x0600B253 RID: 45651 RVA: 0x00429C6C File Offset: 0x00427E6C
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

	// Token: 0x0600B254 RID: 45652 RVA: 0x00429CF8 File Offset: 0x00427EF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B255 RID: 45653 RVA: 0x00429D14 File Offset: 0x00427F14
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B256 RID: 45654 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x0600B257 RID: 45655 RVA: 0x00429D30 File Offset: 0x00427F30
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.UpdateDeathCamera();
	}

	// Token: 0x0600B258 RID: 45656 RVA: 0x00429D38 File Offset: 0x00427F38
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

	// Token: 0x0600B259 RID: 45657 RVA: 0x00429E30 File Offset: 0x00428030
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

	// Token: 0x0600B25A RID: 45658 RVA: 0x00429F24 File Offset: 0x00428124
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

	// Token: 0x0600B25B RID: 45659 RVA: 0x00429FA8 File Offset: 0x004281A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		this.SetRagdoll(false);
		this.Player.OutOfControl.Stop(0f);
	}

	// Token: 0x040085E1 RID: 34273
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;

	// Token: 0x040085E2 RID: 34274
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Rigidbody> m_Rigidbodies;

	// Token: 0x040085E3 RID: 34275
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Transforms;

	// Token: 0x040085E4 RID: 34276
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x040085E5 RID: 34277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_BodyAnimator m_BodyAnimator;

	// Token: 0x040085E6 RID: 34278
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040085E7 RID: 34279
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x040085E8 RID: 34280
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;

	// Token: 0x040085E9 RID: 34281
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_HeadRotationCorrection = Vector3.zero;

	// Token: 0x040085EA RID: 34282
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TimeOfDeath;

	// Token: 0x040085EB RID: 34283
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraFreezeAngle = Vector3.zero;

	// Token: 0x040085EC RID: 34284
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Quaternion> TransformRotations = new Dictionary<Transform, Quaternion>();

	// Token: 0x040085ED RID: 34285
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Transform, Vector3> TransformPositions = new Dictionary<Transform, Vector3>();

	// Token: 0x040085EE RID: 34286
	public float VelocityMultiplier = 30f;

	// Token: 0x040085EF RID: 34287
	public float CameraFreezeDelay = 2.5f;

	// Token: 0x040085F0 RID: 34288
	public GameObject HeadBone;

	// Token: 0x040085F1 RID: 34289
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_Rot;

	// Token: 0x040085F2 RID: 34290
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Pos;

	// Token: 0x040085F3 RID: 34291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_TriedToFetchPlayer;
}

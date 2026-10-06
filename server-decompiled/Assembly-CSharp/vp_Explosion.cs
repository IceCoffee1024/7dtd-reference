using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D0 RID: 5584
[RequireComponent(typeof(AudioSource))]
public class vp_Explosion : MonoBehaviour
{
	// Token: 0x17001459 RID: 5209
	// (get) Token: 0x0600AE62 RID: 44642 RVA: 0x004108E4 File Offset: 0x0040EAE4
	public float DistanceModifier
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_DistanceModifier == 0f)
			{
				this.m_DistanceModifier = 1f - Vector3.Distance(this.Transform.position, this.m_TargetTransform.position) / this.Radius;
			}
			return this.m_DistanceModifier;
		}
	}

	// Token: 0x1700145A RID: 5210
	// (get) Token: 0x0600AE63 RID: 44643 RVA: 0x00410932 File Offset: 0x0040EB32
	public Transform Transform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x1700145B RID: 5211
	// (get) Token: 0x0600AE64 RID: 44644 RVA: 0x00410954 File Offset: 0x0040EB54
	// (set) Token: 0x0600AE65 RID: 44645 RVA: 0x00410976 File Offset: 0x0040EB76
	public Transform Source
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Source == null)
			{
				this.m_Source = base.transform;
			}
			return this.m_Source;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_Source = value;
		}
	}

	// Token: 0x1700145C RID: 5212
	// (get) Token: 0x0600AE66 RID: 44646 RVA: 0x0041097F File Offset: 0x0040EB7F
	// (set) Token: 0x0600AE67 RID: 44647 RVA: 0x004109A1 File Offset: 0x0040EBA1
	public Transform OriginalSource
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_OriginalSource == null)
			{
				this.m_OriginalSource = base.transform;
			}
			return this.m_OriginalSource;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_OriginalSource = value;
		}
	}

	// Token: 0x1700145D RID: 5213
	// (get) Token: 0x0600AE68 RID: 44648 RVA: 0x004109AA File Offset: 0x0040EBAA
	public AudioSource Audio
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Audio == null)
			{
				this.m_Audio = base.GetComponent<AudioSource>();
			}
			return this.m_Audio;
		}
	}

	// Token: 0x0600AE69 RID: 44649 RVA: 0x004109CC File Offset: 0x0040EBCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.Source = base.transform;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Register(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600AE6A RID: 44650 RVA: 0x004109FD File Offset: 0x0040EBFD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.Source = null;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Unregister(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600AE6B RID: 44651 RVA: 0x00410A29 File Offset: 0x0040EC29
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_HaveExploded)
		{
			if (!this.Audio.isPlaying)
			{
				vp_Utility.Destroy(base.gameObject);
			}
			return;
		}
		this.DoExplode();
	}

	// Token: 0x0600AE6C RID: 44652 RVA: 0x00410A54 File Offset: 0x0040EC54
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoExplode()
	{
		this.m_HaveExploded = true;
		foreach (GameObject gameObject in this.FXPrefabs)
		{
			if (gameObject != null)
			{
				vp_Utility.Instantiate(gameObject, this.Transform.position, this.Transform.rotation);
			}
		}
		this.m_DHandlersHitByThisExplosion.Clear();
		foreach (Collider collider in Physics.OverlapSphere(this.Transform.position, this.Radius, -738197525))
		{
			if (!collider.gameObject.isStatic)
			{
				this.m_DistanceModifier = 0f;
				if (collider != null && collider != base.GetComponent<Collider>())
				{
					this.m_TargetCollider = collider;
					this.m_TargetTransform = collider.transform;
					this.AddUFPSCameraShake();
					if (!this.TargetInCover())
					{
						this.m_TargetRigidbody = collider.GetComponent<Rigidbody>();
						if (this.m_TargetRigidbody != null)
						{
							this.AddRigidbodyForce();
						}
						else
						{
							this.AddUFPSForce();
						}
						vp_Explosion.m_TargetDHandler = vp_DamageHandler.GetDamageHandlerOfCollider(this.m_TargetCollider);
						if (vp_Explosion.m_TargetDHandler != null)
						{
							this.DoUFPSDamage(this.DistanceModifier * this.Damage);
						}
						else if (!this.RequireDamageHandler)
						{
							this.DoUnityDamage(this.DistanceModifier * this.Damage);
						}
					}
				}
			}
		}
		this.Audio.clip = this.Sound;
		this.Audio.pitch = global::UnityEngine.Random.Range(this.SoundMinPitch, this.SoundMaxPitch) * Time.timeScale;
		if (!this.Audio.playOnAwake)
		{
			this.Audio.Play();
		}
	}

	// Token: 0x0600AE6D RID: 44653 RVA: 0x00410C2C File Offset: 0x0040EE2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TargetInCover()
	{
		if (!this.AllowCover)
		{
			return false;
		}
		this.m_Ray.origin = this.Transform.position;
		this.m_Ray.direction = (this.m_TargetCollider.bounds.center - this.Transform.position).normalized;
		if (Physics.Raycast(this.m_Ray, out this.m_RaycastHit, this.Radius + 1f) && this.m_RaycastHit.collider == this.m_TargetCollider)
		{
			return false;
		}
		this.m_Ray.direction = (vp_3DUtility.HorizontalVector(this.m_TargetCollider.bounds.center) + Vector3.up * this.m_TargetCollider.bounds.max.y - this.Transform.position).normalized;
		return !Physics.Raycast(this.m_Ray, out this.m_RaycastHit, this.Radius + 1f) || !(this.m_RaycastHit.collider == this.m_TargetCollider);
	}

	// Token: 0x0600AE6E RID: 44654 RVA: 0x00410D64 File Offset: 0x0040EF64
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddRigidbodyForce()
	{
		if (this.m_TargetRigidbody.isKinematic)
		{
			return;
		}
		this.m_Ray.origin = this.m_TargetTransform.position;
		this.m_Ray.direction = -Vector3.up;
		if (!Physics.Raycast(this.m_Ray, out this.m_RaycastHit, 1f))
		{
			this.UpForce = 0f;
		}
		this.m_TargetRigidbody.AddExplosionForce(this.Force / Time.timeScale / vp_TimeUtility.AdjustedTimeScale, this.Transform.position, this.Radius, this.UpForce);
	}

	// Token: 0x0600AE6F RID: 44655 RVA: 0x00410E04 File Offset: 0x0040F004
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSForce()
	{
		vp_TargetEvent<Vector3>.Send(this.m_TargetTransform.root, "ForceImpact", (this.m_TargetTransform.position - this.Transform.position).normalized * this.Force * 0.001f * this.DistanceModifier, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600AE70 RID: 44656 RVA: 0x00410E6A File Offset: 0x0040F06A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSCameraShake()
	{
		vp_TargetEvent<float>.Send(this.m_TargetTransform.root, "CameraBombShake", this.DistanceModifier * this.CameraShake, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600AE71 RID: 44657 RVA: 0x00410E90 File Offset: 0x0040F090
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DoUFPSDamage(float damage)
	{
		if (this.m_DHandlersHitByThisExplosion.ContainsKey(vp_Explosion.m_TargetDHandler))
		{
			return;
		}
		this.m_DHandlersHitByThisExplosion.Add(vp_Explosion.m_TargetDHandler, null);
		vp_Explosion.m_TargetDHandler.Damage(new vp_DamageInfo(damage, this.Source, this.OriginalSource));
	}

	// Token: 0x0600AE72 RID: 44658 RVA: 0x00410EDD File Offset: 0x0040F0DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoUnityDamage(float damage)
	{
		this.m_TargetCollider.gameObject.BroadcastMessage(this.DamageMessageName, damage, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600AE73 RID: 44659 RVA: 0x004109A1 File Offset: 0x0040EBA1
	public void SetSource(Transform source)
	{
		this.m_OriginalSource = source;
	}

	// Token: 0x040081AE RID: 33198
	public float Radius = 15f;

	// Token: 0x040081AF RID: 33199
	public float Force = 1000f;

	// Token: 0x040081B0 RID: 33200
	public float UpForce = 10f;

	// Token: 0x040081B1 RID: 33201
	public float Damage = 10f;

	// Token: 0x040081B2 RID: 33202
	public bool AllowCover;

	// Token: 0x040081B3 RID: 33203
	public float CameraShake = 1f;

	// Token: 0x040081B4 RID: 33204
	public string DamageMessageName = "Damage";

	// Token: 0x040081B5 RID: 33205
	public bool RequireDamageHandler = true;

	// Token: 0x040081B6 RID: 33206
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HaveExploded;

	// Token: 0x040081B7 RID: 33207
	public AudioClip Sound;

	// Token: 0x040081B8 RID: 33208
	public float SoundMinPitch = 0.8f;

	// Token: 0x040081B9 RID: 33209
	public float SoundMaxPitch = 1.2f;

	// Token: 0x040081BA RID: 33210
	public List<GameObject> FXPrefabs = new List<GameObject>();

	// Token: 0x040081BB RID: 33211
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Ray m_Ray;

	// Token: 0x040081BC RID: 33212
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_RaycastHit;

	// Token: 0x040081BD RID: 33213
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_TargetCollider;

	// Token: 0x040081BE RID: 33214
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_TargetTransform;

	// Token: 0x040081BF RID: 33215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_TargetRigidbody;

	// Token: 0x040081C0 RID: 33216
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DistanceModifier;

	// Token: 0x040081C1 RID: 33217
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_DamageHandler, object> m_DHandlersHitByThisExplosion = new Dictionary<vp_DamageHandler, object>(50);

	// Token: 0x040081C2 RID: 33218
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_TargetDHandler;

	// Token: 0x040081C3 RID: 33219
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081C4 RID: 33220
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x040081C5 RID: 33221
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;

	// Token: 0x040081C6 RID: 33222
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015AE RID: 5550
[RequireComponent(typeof(AudioSource))]
public class vp_Explosion : MonoBehaviour
{
	// Token: 0x17001414 RID: 5140
	// (get) Token: 0x0600ADAF RID: 44463 RVA: 0x0040BA90 File Offset: 0x00409C90
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

	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x0600ADB0 RID: 44464 RVA: 0x0040BADE File Offset: 0x00409CDE
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

	// Token: 0x17001416 RID: 5142
	// (get) Token: 0x0600ADB1 RID: 44465 RVA: 0x0040BB00 File Offset: 0x00409D00
	// (set) Token: 0x0600ADB2 RID: 44466 RVA: 0x0040BB22 File Offset: 0x00409D22
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

	// Token: 0x17001417 RID: 5143
	// (get) Token: 0x0600ADB3 RID: 44467 RVA: 0x0040BB2B File Offset: 0x00409D2B
	// (set) Token: 0x0600ADB4 RID: 44468 RVA: 0x0040BB4D File Offset: 0x00409D4D
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

	// Token: 0x17001418 RID: 5144
	// (get) Token: 0x0600ADB5 RID: 44469 RVA: 0x0040BB56 File Offset: 0x00409D56
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

	// Token: 0x0600ADB6 RID: 44470 RVA: 0x0040BB78 File Offset: 0x00409D78
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.Source = base.transform;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Register(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600ADB7 RID: 44471 RVA: 0x0040BBA9 File Offset: 0x00409DA9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.Source = null;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Unregister(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600ADB8 RID: 44472 RVA: 0x0040BBD5 File Offset: 0x00409DD5
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

	// Token: 0x0600ADB9 RID: 44473 RVA: 0x0040BC00 File Offset: 0x00409E00
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

	// Token: 0x0600ADBA RID: 44474 RVA: 0x0040BDD8 File Offset: 0x00409FD8
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

	// Token: 0x0600ADBB RID: 44475 RVA: 0x0040BF10 File Offset: 0x0040A110
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

	// Token: 0x0600ADBC RID: 44476 RVA: 0x0040BFB0 File Offset: 0x0040A1B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSForce()
	{
		vp_TargetEvent<Vector3>.Send(this.m_TargetTransform.root, "ForceImpact", (this.m_TargetTransform.position - this.Transform.position).normalized * this.Force * 0.001f * this.DistanceModifier, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600ADBD RID: 44477 RVA: 0x0040C016 File Offset: 0x0040A216
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSCameraShake()
	{
		vp_TargetEvent<float>.Send(this.m_TargetTransform.root, "CameraBombShake", this.DistanceModifier * this.CameraShake, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600ADBE RID: 44478 RVA: 0x0040C03C File Offset: 0x0040A23C
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

	// Token: 0x0600ADBF RID: 44479 RVA: 0x0040C089 File Offset: 0x0040A289
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoUnityDamage(float damage)
	{
		this.m_TargetCollider.gameObject.BroadcastMessage(this.DamageMessageName, damage, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600ADC0 RID: 44480 RVA: 0x0040BB4D File Offset: 0x00409D4D
	public void SetSource(Transform source)
	{
		this.m_OriginalSource = source;
	}

	// Token: 0x0400812B RID: 33067
	public float Radius = 15f;

	// Token: 0x0400812C RID: 33068
	public float Force = 1000f;

	// Token: 0x0400812D RID: 33069
	public float UpForce = 10f;

	// Token: 0x0400812E RID: 33070
	public float Damage = 10f;

	// Token: 0x0400812F RID: 33071
	public bool AllowCover;

	// Token: 0x04008130 RID: 33072
	public float CameraShake = 1f;

	// Token: 0x04008131 RID: 33073
	public string DamageMessageName = "Damage";

	// Token: 0x04008132 RID: 33074
	public bool RequireDamageHandler = true;

	// Token: 0x04008133 RID: 33075
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HaveExploded;

	// Token: 0x04008134 RID: 33076
	public AudioClip Sound;

	// Token: 0x04008135 RID: 33077
	public float SoundMinPitch = 0.8f;

	// Token: 0x04008136 RID: 33078
	public float SoundMaxPitch = 1.2f;

	// Token: 0x04008137 RID: 33079
	public List<GameObject> FXPrefabs = new List<GameObject>();

	// Token: 0x04008138 RID: 33080
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Ray m_Ray;

	// Token: 0x04008139 RID: 33081
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_RaycastHit;

	// Token: 0x0400813A RID: 33082
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_TargetCollider;

	// Token: 0x0400813B RID: 33083
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_TargetTransform;

	// Token: 0x0400813C RID: 33084
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_TargetRigidbody;

	// Token: 0x0400813D RID: 33085
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DistanceModifier;

	// Token: 0x0400813E RID: 33086
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_DamageHandler, object> m_DHandlersHitByThisExplosion = new Dictionary<vp_DamageHandler, object>(50);

	// Token: 0x0400813F RID: 33087
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_TargetDHandler;

	// Token: 0x04008140 RID: 33088
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008141 RID: 33089
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008142 RID: 33090
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;

	// Token: 0x04008143 RID: 33091
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;
}

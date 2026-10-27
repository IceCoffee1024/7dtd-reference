using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E2 RID: 5602
[RequireComponent(typeof(AudioSource))]
public class vp_Explosion : MonoBehaviour
{
	// Token: 0x17001465 RID: 5221
	// (get) Token: 0x0600AEBA RID: 44730 RVA: 0x00412CE8 File Offset: 0x00410EE8
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

	// Token: 0x17001466 RID: 5222
	// (get) Token: 0x0600AEBB RID: 44731 RVA: 0x00412D36 File Offset: 0x00410F36
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

	// Token: 0x17001467 RID: 5223
	// (get) Token: 0x0600AEBC RID: 44732 RVA: 0x00412D58 File Offset: 0x00410F58
	// (set) Token: 0x0600AEBD RID: 44733 RVA: 0x00412D7A File Offset: 0x00410F7A
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

	// Token: 0x17001468 RID: 5224
	// (get) Token: 0x0600AEBE RID: 44734 RVA: 0x00412D83 File Offset: 0x00410F83
	// (set) Token: 0x0600AEBF RID: 44735 RVA: 0x00412DA5 File Offset: 0x00410FA5
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

	// Token: 0x17001469 RID: 5225
	// (get) Token: 0x0600AEC0 RID: 44736 RVA: 0x00412DAE File Offset: 0x00410FAE
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

	// Token: 0x0600AEC1 RID: 44737 RVA: 0x00412DD0 File Offset: 0x00410FD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.Source = base.transform;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Register(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600AEC2 RID: 44738 RVA: 0x00412E01 File Offset: 0x00411001
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.Source = null;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Unregister(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600AEC3 RID: 44739 RVA: 0x00412E2D File Offset: 0x0041102D
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

	// Token: 0x0600AEC4 RID: 44740 RVA: 0x00412E58 File Offset: 0x00411058
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

	// Token: 0x0600AEC5 RID: 44741 RVA: 0x00413030 File Offset: 0x00411230
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

	// Token: 0x0600AEC6 RID: 44742 RVA: 0x00413168 File Offset: 0x00411368
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

	// Token: 0x0600AEC7 RID: 44743 RVA: 0x00413208 File Offset: 0x00411408
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSForce()
	{
		vp_TargetEvent<Vector3>.Send(this.m_TargetTransform.root, "ForceImpact", (this.m_TargetTransform.position - this.Transform.position).normalized * this.Force * 0.001f * this.DistanceModifier, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600AEC8 RID: 44744 RVA: 0x0041326E File Offset: 0x0041146E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSCameraShake()
	{
		vp_TargetEvent<float>.Send(this.m_TargetTransform.root, "CameraBombShake", this.DistanceModifier * this.CameraShake, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600AEC9 RID: 44745 RVA: 0x00413294 File Offset: 0x00411494
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

	// Token: 0x0600AECA RID: 44746 RVA: 0x004132E1 File Offset: 0x004114E1
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoUnityDamage(float damage)
	{
		this.m_TargetCollider.gameObject.BroadcastMessage(this.DamageMessageName, damage, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600AECB RID: 44747 RVA: 0x00412DA5 File Offset: 0x00410FA5
	public void SetSource(Transform source)
	{
		this.m_OriginalSource = source;
	}

	// Token: 0x040081EF RID: 33263
	public float Radius = 15f;

	// Token: 0x040081F0 RID: 33264
	public float Force = 1000f;

	// Token: 0x040081F1 RID: 33265
	public float UpForce = 10f;

	// Token: 0x040081F2 RID: 33266
	public float Damage = 10f;

	// Token: 0x040081F3 RID: 33267
	public bool AllowCover;

	// Token: 0x040081F4 RID: 33268
	public float CameraShake = 1f;

	// Token: 0x040081F5 RID: 33269
	public string DamageMessageName = "Damage";

	// Token: 0x040081F6 RID: 33270
	public bool RequireDamageHandler = true;

	// Token: 0x040081F7 RID: 33271
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HaveExploded;

	// Token: 0x040081F8 RID: 33272
	public AudioClip Sound;

	// Token: 0x040081F9 RID: 33273
	public float SoundMinPitch = 0.8f;

	// Token: 0x040081FA RID: 33274
	public float SoundMaxPitch = 1.2f;

	// Token: 0x040081FB RID: 33275
	public List<GameObject> FXPrefabs = new List<GameObject>();

	// Token: 0x040081FC RID: 33276
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Ray m_Ray;

	// Token: 0x040081FD RID: 33277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_RaycastHit;

	// Token: 0x040081FE RID: 33278
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_TargetCollider;

	// Token: 0x040081FF RID: 33279
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_TargetTransform;

	// Token: 0x04008200 RID: 33280
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_TargetRigidbody;

	// Token: 0x04008201 RID: 33281
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DistanceModifier;

	// Token: 0x04008202 RID: 33282
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_DamageHandler, object> m_DHandlersHitByThisExplosion = new Dictionary<vp_DamageHandler, object>(50);

	// Token: 0x04008203 RID: 33283
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_TargetDHandler;

	// Token: 0x04008204 RID: 33284
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008205 RID: 33285
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008206 RID: 33286
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;

	// Token: 0x04008207 RID: 33287
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;
}

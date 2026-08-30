using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001593 RID: 5523
[RequireComponent(typeof(AudioSource))]
public class vp_Explosion : MonoBehaviour
{
	// Token: 0x170013DB RID: 5083
	// (get) Token: 0x0600ACB2 RID: 44210 RVA: 0x00406750 File Offset: 0x00404950
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

	// Token: 0x170013DC RID: 5084
	// (get) Token: 0x0600ACB3 RID: 44211 RVA: 0x0040679E File Offset: 0x0040499E
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

	// Token: 0x170013DD RID: 5085
	// (get) Token: 0x0600ACB4 RID: 44212 RVA: 0x004067C0 File Offset: 0x004049C0
	// (set) Token: 0x0600ACB5 RID: 44213 RVA: 0x004067E2 File Offset: 0x004049E2
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

	// Token: 0x170013DE RID: 5086
	// (get) Token: 0x0600ACB6 RID: 44214 RVA: 0x004067EB File Offset: 0x004049EB
	// (set) Token: 0x0600ACB7 RID: 44215 RVA: 0x0040680D File Offset: 0x00404A0D
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

	// Token: 0x170013DF RID: 5087
	// (get) Token: 0x0600ACB8 RID: 44216 RVA: 0x00406816 File Offset: 0x00404A16
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

	// Token: 0x0600ACB9 RID: 44217 RVA: 0x00406838 File Offset: 0x00404A38
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.Source = base.transform;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Register(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600ACBA RID: 44218 RVA: 0x00406869 File Offset: 0x00404A69
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.Source = null;
		this.OriginalSource = null;
		vp_TargetEvent<Transform>.Unregister(base.transform, "SetSource", new Action<Transform>(this.SetSource));
	}

	// Token: 0x0600ACBB RID: 44219 RVA: 0x00406895 File Offset: 0x00404A95
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

	// Token: 0x0600ACBC RID: 44220 RVA: 0x004068C0 File Offset: 0x00404AC0
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

	// Token: 0x0600ACBD RID: 44221 RVA: 0x00406A98 File Offset: 0x00404C98
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

	// Token: 0x0600ACBE RID: 44222 RVA: 0x00406BD0 File Offset: 0x00404DD0
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

	// Token: 0x0600ACBF RID: 44223 RVA: 0x00406C70 File Offset: 0x00404E70
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSForce()
	{
		vp_TargetEvent<Vector3>.Send(this.m_TargetTransform.root, "ForceImpact", (this.m_TargetTransform.position - this.Transform.position).normalized * this.Force * 0.001f * this.DistanceModifier, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600ACC0 RID: 44224 RVA: 0x00406CD6 File Offset: 0x00404ED6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AddUFPSCameraShake()
	{
		vp_TargetEvent<float>.Send(this.m_TargetTransform.root, "CameraBombShake", this.DistanceModifier * this.CameraShake, vp_TargetEventOptions.DontRequireReceiver);
	}

	// Token: 0x0600ACC1 RID: 44225 RVA: 0x00406CFC File Offset: 0x00404EFC
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

	// Token: 0x0600ACC2 RID: 44226 RVA: 0x00406D49 File Offset: 0x00404F49
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoUnityDamage(float damage)
	{
		this.m_TargetCollider.gameObject.BroadcastMessage(this.DamageMessageName, damage, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600ACC3 RID: 44227 RVA: 0x0040680D File Offset: 0x00404A0D
	public void SetSource(Transform source)
	{
		this.m_OriginalSource = source;
	}

	// Token: 0x04008052 RID: 32850
	public float Radius = 15f;

	// Token: 0x04008053 RID: 32851
	public float Force = 1000f;

	// Token: 0x04008054 RID: 32852
	public float UpForce = 10f;

	// Token: 0x04008055 RID: 32853
	public float Damage = 10f;

	// Token: 0x04008056 RID: 32854
	public bool AllowCover;

	// Token: 0x04008057 RID: 32855
	public float CameraShake = 1f;

	// Token: 0x04008058 RID: 32856
	public string DamageMessageName = "Damage";

	// Token: 0x04008059 RID: 32857
	public bool RequireDamageHandler = true;

	// Token: 0x0400805A RID: 32858
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_HaveExploded;

	// Token: 0x0400805B RID: 32859
	public AudioClip Sound;

	// Token: 0x0400805C RID: 32860
	public float SoundMinPitch = 0.8f;

	// Token: 0x0400805D RID: 32861
	public float SoundMaxPitch = 1.2f;

	// Token: 0x0400805E RID: 32862
	public List<GameObject> FXPrefabs = new List<GameObject>();

	// Token: 0x0400805F RID: 32863
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Ray m_Ray;

	// Token: 0x04008060 RID: 32864
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public RaycastHit m_RaycastHit;

	// Token: 0x04008061 RID: 32865
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_TargetCollider;

	// Token: 0x04008062 RID: 32866
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_TargetTransform;

	// Token: 0x04008063 RID: 32867
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_TargetRigidbody;

	// Token: 0x04008064 RID: 32868
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DistanceModifier;

	// Token: 0x04008065 RID: 32869
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_DamageHandler, object> m_DHandlersHitByThisExplosion = new Dictionary<vp_DamageHandler, object>(50);

	// Token: 0x04008066 RID: 32870
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_DamageHandler m_TargetDHandler;

	// Token: 0x04008067 RID: 32871
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008068 RID: 32872
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008069 RID: 32873
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;

	// Token: 0x0400806A RID: 32874
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;
}

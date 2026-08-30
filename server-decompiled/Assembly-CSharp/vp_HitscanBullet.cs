using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001595 RID: 5525
[RequireComponent(typeof(AudioSource))]
public class vp_HitscanBullet : MonoBehaviour
{
	// Token: 0x0600ACCB RID: 44235 RVA: 0x00406EF5 File Offset: 0x004050F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Renderer = base.GetComponent<Renderer>();
		this.m_Audio = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600ACCC RID: 44236 RVA: 0x00406F1B File Offset: 0x0040511B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Initialized = true;
		this.DoHit();
	}

	// Token: 0x0600ACCD RID: 44237 RVA: 0x00406F2A File Offset: 0x0040512A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.m_Initialized)
		{
			return;
		}
		this.DoHit();
	}

	// Token: 0x0600ACCE RID: 44238 RVA: 0x00406F3C File Offset: 0x0040513C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoHit()
	{
		Ray ray = new Ray(this.m_Transform.position, this.m_Transform.forward);
		RaycastHit raycastHit;
		if (!Physics.Raycast(ray, out raycastHit, this.Range, this.IgnoreLocalPlayer ? (-538750989) : (-738197525)))
		{
			vp_Utility.Destroy(base.gameObject);
			return;
		}
		Vector3 localScale = this.m_Transform.localScale;
		this.m_Transform.parent = raycastHit.transform;
		this.m_Transform.localPosition = raycastHit.transform.InverseTransformPoint(raycastHit.point);
		this.m_Transform.rotation = Quaternion.LookRotation(raycastHit.normal);
		if (raycastHit.transform.lossyScale == Vector3.one)
		{
			this.m_Transform.Rotate(Vector3.forward, (float)global::UnityEngine.Random.Range(0, 360), Space.Self);
		}
		else
		{
			this.m_Transform.parent = null;
			this.m_Transform.localScale = localScale;
			this.m_Transform.parent = raycastHit.transform;
		}
		Rigidbody attachedRigidbody = raycastHit.collider.attachedRigidbody;
		if (attachedRigidbody != null && !attachedRigidbody.isKinematic)
		{
			attachedRigidbody.AddForceAtPosition(ray.direction * this.Force / Time.timeScale / vp_TimeUtility.AdjustedTimeScale, raycastHit.point);
		}
		if (this.m_ImpactPrefab != null)
		{
			vp_Utility.Instantiate(this.m_ImpactPrefab, this.m_Transform.position, this.m_Transform.rotation);
		}
		if (this.m_DustPrefab != null)
		{
			vp_Utility.Instantiate(this.m_DustPrefab, this.m_Transform.position, this.m_Transform.rotation);
		}
		if (this.m_SparkPrefab != null && global::UnityEngine.Random.value < this.m_SparkFactor)
		{
			vp_Utility.Instantiate(this.m_SparkPrefab, this.m_Transform.position, this.m_Transform.rotation);
		}
		if (this.m_DebrisPrefab != null)
		{
			vp_Utility.Instantiate(this.m_DebrisPrefab, this.m_Transform.position, this.m_Transform.rotation);
		}
		if (this.m_ImpactSounds.Count > 0)
		{
			this.m_Audio.pitch = global::UnityEngine.Random.Range(this.SoundImpactPitch.x, this.SoundImpactPitch.y) * Time.timeScale;
			this.m_Audio.clip = this.m_ImpactSounds[global::UnityEngine.Random.Range(0, this.m_ImpactSounds.Count)];
			this.m_Audio.Stop();
			this.m_Audio.Play();
		}
		if (this.m_Source != null)
		{
			raycastHit.collider.SendMessageUpwards(this.DamageMethodName, new vp_DamageInfo(this.Damage, this.m_Source), SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			raycastHit.collider.SendMessageUpwards(this.DamageMethodName, this.Damage, SendMessageOptions.DontRequireReceiver);
		}
		if (this.NoDecalOnTheseLayers.Length != 0)
		{
			foreach (int num in this.NoDecalOnTheseLayers)
			{
				if (raycastHit.transform.gameObject.layer == num)
				{
					this.m_Renderer.enabled = false;
					this.TryDestroy();
					return;
				}
			}
		}
		if (this.m_Renderer != null)
		{
			vp_DecalManager.Add(base.gameObject);
			return;
		}
		vp_Timer.In(1f, new vp_Timer.Callback(this.TryDestroy), null);
	}

	// Token: 0x0600ACCF RID: 44239 RVA: 0x004072B8 File Offset: 0x004054B8
	public void SetSource(Transform source)
	{
		this.m_Source = source;
		if (source.transform.root == Camera.main.transform.root)
		{
			this.IgnoreLocalPlayer = true;
		}
	}

	// Token: 0x0600ACD0 RID: 44240 RVA: 0x004072E9 File Offset: 0x004054E9
	[Obsolete("Please use 'SetSource' instead.")]
	public void SetSender(Transform sender)
	{
		this.SetSource(sender);
	}

	// Token: 0x0600ACD1 RID: 44241 RVA: 0x004072F4 File Offset: 0x004054F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void TryDestroy()
	{
		if (this == null)
		{
			return;
		}
		if (!this.m_Audio.isPlaying)
		{
			this.m_Renderer.enabled = true;
			vp_Utility.Destroy(base.gameObject);
			return;
		}
		vp_Timer.In(1f, new vp_Timer.Callback(this.TryDestroy), null);
	}

	// Token: 0x04008071 RID: 32881
	public bool IgnoreLocalPlayer = true;

	// Token: 0x04008072 RID: 32882
	public float Range = 100f;

	// Token: 0x04008073 RID: 32883
	public float Force = 100f;

	// Token: 0x04008074 RID: 32884
	public float Damage = 1f;

	// Token: 0x04008075 RID: 32885
	public string DamageMethodName = "Damage";

	// Token: 0x04008076 RID: 32886
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008077 RID: 32887
	public float m_SparkFactor = 0.5f;

	// Token: 0x04008078 RID: 32888
	public GameObject m_ImpactPrefab;

	// Token: 0x04008079 RID: 32889
	public GameObject m_DustPrefab;

	// Token: 0x0400807A RID: 32890
	public GameObject m_SparkPrefab;

	// Token: 0x0400807B RID: 32891
	public GameObject m_DebrisPrefab;

	// Token: 0x0400807C RID: 32892
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400807D RID: 32893
	public List<AudioClip> m_ImpactSounds = new List<AudioClip>();

	// Token: 0x0400807E RID: 32894
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x0400807F RID: 32895
	public int[] NoDecalOnTheseLayers;

	// Token: 0x04008080 RID: 32896
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008081 RID: 32897
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x04008082 RID: 32898
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D2 RID: 5586
[RequireComponent(typeof(AudioSource))]
public class vp_HitscanBullet : MonoBehaviour
{
	// Token: 0x0600AE7B RID: 44667 RVA: 0x00411089 File Offset: 0x0040F289
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Renderer = base.GetComponent<Renderer>();
		this.m_Audio = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600AE7C RID: 44668 RVA: 0x004110AF File Offset: 0x0040F2AF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Initialized = true;
		this.DoHit();
	}

	// Token: 0x0600AE7D RID: 44669 RVA: 0x004110BE File Offset: 0x0040F2BE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.m_Initialized)
		{
			return;
		}
		this.DoHit();
	}

	// Token: 0x0600AE7E RID: 44670 RVA: 0x004110D0 File Offset: 0x0040F2D0
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

	// Token: 0x0600AE7F RID: 44671 RVA: 0x0041144C File Offset: 0x0040F64C
	public void SetSource(Transform source)
	{
		this.m_Source = source;
		if (source.transform.root == Camera.main.transform.root)
		{
			this.IgnoreLocalPlayer = true;
		}
	}

	// Token: 0x0600AE80 RID: 44672 RVA: 0x0041147D File Offset: 0x0040F67D
	[Obsolete("Please use 'SetSource' instead.")]
	public void SetSender(Transform sender)
	{
		this.SetSource(sender);
	}

	// Token: 0x0600AE81 RID: 44673 RVA: 0x00411488 File Offset: 0x0040F688
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

	// Token: 0x040081CD RID: 33229
	public bool IgnoreLocalPlayer = true;

	// Token: 0x040081CE RID: 33230
	public float Range = 100f;

	// Token: 0x040081CF RID: 33231
	public float Force = 100f;

	// Token: 0x040081D0 RID: 33232
	public float Damage = 1f;

	// Token: 0x040081D1 RID: 33233
	public string DamageMethodName = "Damage";

	// Token: 0x040081D2 RID: 33234
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x040081D3 RID: 33235
	public float m_SparkFactor = 0.5f;

	// Token: 0x040081D4 RID: 33236
	public GameObject m_ImpactPrefab;

	// Token: 0x040081D5 RID: 33237
	public GameObject m_DustPrefab;

	// Token: 0x040081D6 RID: 33238
	public GameObject m_SparkPrefab;

	// Token: 0x040081D7 RID: 33239
	public GameObject m_DebrisPrefab;

	// Token: 0x040081D8 RID: 33240
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081D9 RID: 33241
	public List<AudioClip> m_ImpactSounds = new List<AudioClip>();

	// Token: 0x040081DA RID: 33242
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x040081DB RID: 33243
	public int[] NoDecalOnTheseLayers;

	// Token: 0x040081DC RID: 33244
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081DD RID: 33245
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x040081DE RID: 33246
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;
}

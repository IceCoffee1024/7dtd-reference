using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E4 RID: 5604
[RequireComponent(typeof(AudioSource))]
public class vp_HitscanBullet : MonoBehaviour
{
	// Token: 0x0600AED3 RID: 44755 RVA: 0x0041348D File Offset: 0x0041168D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Renderer = base.GetComponent<Renderer>();
		this.m_Audio = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600AED4 RID: 44756 RVA: 0x004134B3 File Offset: 0x004116B3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Initialized = true;
		this.DoHit();
	}

	// Token: 0x0600AED5 RID: 44757 RVA: 0x004134C2 File Offset: 0x004116C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.m_Initialized)
		{
			return;
		}
		this.DoHit();
	}

	// Token: 0x0600AED6 RID: 44758 RVA: 0x004134D4 File Offset: 0x004116D4
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

	// Token: 0x0600AED7 RID: 44759 RVA: 0x00413850 File Offset: 0x00411A50
	public void SetSource(Transform source)
	{
		this.m_Source = source;
		if (source.transform.root == Camera.main.transform.root)
		{
			this.IgnoreLocalPlayer = true;
		}
	}

	// Token: 0x0600AED8 RID: 44760 RVA: 0x00413881 File Offset: 0x00411A81
	[Obsolete("Please use 'SetSource' instead.")]
	public void SetSender(Transform sender)
	{
		this.SetSource(sender);
	}

	// Token: 0x0600AED9 RID: 44761 RVA: 0x0041388C File Offset: 0x00411A8C
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

	// Token: 0x0400820E RID: 33294
	public bool IgnoreLocalPlayer = true;

	// Token: 0x0400820F RID: 33295
	public float Range = 100f;

	// Token: 0x04008210 RID: 33296
	public float Force = 100f;

	// Token: 0x04008211 RID: 33297
	public float Damage = 1f;

	// Token: 0x04008212 RID: 33298
	public string DamageMethodName = "Damage";

	// Token: 0x04008213 RID: 33299
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008214 RID: 33300
	public float m_SparkFactor = 0.5f;

	// Token: 0x04008215 RID: 33301
	public GameObject m_ImpactPrefab;

	// Token: 0x04008216 RID: 33302
	public GameObject m_DustPrefab;

	// Token: 0x04008217 RID: 33303
	public GameObject m_SparkPrefab;

	// Token: 0x04008218 RID: 33304
	public GameObject m_DebrisPrefab;

	// Token: 0x04008219 RID: 33305
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400821A RID: 33306
	public List<AudioClip> m_ImpactSounds = new List<AudioClip>();

	// Token: 0x0400821B RID: 33307
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x0400821C RID: 33308
	public int[] NoDecalOnTheseLayers;

	// Token: 0x0400821D RID: 33309
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400821E RID: 33310
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x0400821F RID: 33311
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;
}

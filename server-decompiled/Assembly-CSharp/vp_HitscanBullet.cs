using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015B0 RID: 5552
[RequireComponent(typeof(AudioSource))]
public class vp_HitscanBullet : MonoBehaviour
{
	// Token: 0x0600ADC8 RID: 44488 RVA: 0x0040C235 File Offset: 0x0040A435
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Renderer = base.GetComponent<Renderer>();
		this.m_Audio = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600ADC9 RID: 44489 RVA: 0x0040C25B File Offset: 0x0040A45B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Initialized = true;
		this.DoHit();
	}

	// Token: 0x0600ADCA RID: 44490 RVA: 0x0040C26A File Offset: 0x0040A46A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.m_Initialized)
		{
			return;
		}
		this.DoHit();
	}

	// Token: 0x0600ADCB RID: 44491 RVA: 0x0040C27C File Offset: 0x0040A47C
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

	// Token: 0x0600ADCC RID: 44492 RVA: 0x0040C5F8 File Offset: 0x0040A7F8
	public void SetSource(Transform source)
	{
		this.m_Source = source;
		if (source.transform.root == Camera.main.transform.root)
		{
			this.IgnoreLocalPlayer = true;
		}
	}

	// Token: 0x0600ADCD RID: 44493 RVA: 0x0040C629 File Offset: 0x0040A829
	[Obsolete("Please use 'SetSource' instead.")]
	public void SetSender(Transform sender)
	{
		this.SetSource(sender);
	}

	// Token: 0x0600ADCE RID: 44494 RVA: 0x0040C634 File Offset: 0x0040A834
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

	// Token: 0x0400814A RID: 33098
	public bool IgnoreLocalPlayer = true;

	// Token: 0x0400814B RID: 33099
	public float Range = 100f;

	// Token: 0x0400814C RID: 33100
	public float Force = 100f;

	// Token: 0x0400814D RID: 33101
	public float Damage = 1f;

	// Token: 0x0400814E RID: 33102
	public string DamageMethodName = "Damage";

	// Token: 0x0400814F RID: 33103
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008150 RID: 33104
	public float m_SparkFactor = 0.5f;

	// Token: 0x04008151 RID: 33105
	public GameObject m_ImpactPrefab;

	// Token: 0x04008152 RID: 33106
	public GameObject m_DustPrefab;

	// Token: 0x04008153 RID: 33107
	public GameObject m_SparkPrefab;

	// Token: 0x04008154 RID: 33108
	public GameObject m_DebrisPrefab;

	// Token: 0x04008155 RID: 33109
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008156 RID: 33110
	public List<AudioClip> m_ImpactSounds = new List<AudioClip>();

	// Token: 0x04008157 RID: 33111
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008158 RID: 33112
	public int[] NoDecalOnTheseLayers;

	// Token: 0x04008159 RID: 33113
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400815A RID: 33114
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x0400815B RID: 33115
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;
}

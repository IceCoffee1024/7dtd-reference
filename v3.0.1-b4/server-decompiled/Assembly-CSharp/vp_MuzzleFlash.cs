using System;
using UnityEngine;

// Token: 0x0200158B RID: 5515
public class vp_MuzzleFlash : MonoBehaviour
{
	// Token: 0x170013D3 RID: 5075
	// (get) Token: 0x0600AC6B RID: 44139 RVA: 0x00405310 File Offset: 0x00403510
	// (set) Token: 0x0600AC6C RID: 44140 RVA: 0x00405318 File Offset: 0x00403518
	public float FadeSpeed
	{
		get
		{
			return this.m_FadeSpeed;
		}
		set
		{
			this.m_FadeSpeed = value;
		}
	}

	// Token: 0x170013D4 RID: 5076
	// (get) Token: 0x0600AC6D RID: 44141 RVA: 0x00405321 File Offset: 0x00403521
	// (set) Token: 0x0600AC6E RID: 44142 RVA: 0x00405329 File Offset: 0x00403529
	public bool ForceShow
	{
		get
		{
			return this.m_ForceShow;
		}
		set
		{
			this.m_ForceShow = value;
		}
	}

	// Token: 0x0600AC6F RID: 44143 RVA: 0x00405334 File Offset: 0x00403534
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_ForceShow = false;
		this.m_Light = base.GetComponent<Light>();
		if (this.m_Light != null)
		{
			this.m_LightIntensity = this.m_Light.intensity;
			this.m_Light.intensity = 0f;
		}
		this.m_Renderer = base.GetComponent<Renderer>();
		if (this.m_Renderer != null)
		{
			this.m_Material = base.GetComponent<Renderer>().material;
			if (this.m_Material != null)
			{
				this.m_Color = this.m_Material.GetColor("_TintColor");
				this.m_Color.a = 0f;
			}
		}
	}

	// Token: 0x0600AC70 RID: 44144 RVA: 0x004053F0 File Offset: 0x004035F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		GameObject gameObject = GameObject.Find("WeaponCamera");
		if (gameObject != null && gameObject.transform.parent == this.m_Transform.parent)
		{
			base.gameObject.layer = 10;
		}
	}

	// Token: 0x0600AC71 RID: 44145 RVA: 0x0040543C File Offset: 0x0040363C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_ForceShow)
		{
			this.Show();
		}
		else if (this.m_Color.a > 0f)
		{
			this.m_Color.a = this.m_Color.a - this.m_FadeSpeed * (Time.deltaTime * 60f);
			if (this.m_Light != null)
			{
				this.m_Light.intensity = this.m_LightIntensity * (this.m_Color.a * 2f);
			}
		}
		if (this.m_Material != null)
		{
			this.m_Material.SetColor("_TintColor", this.m_Color);
		}
		if (this.m_Color.a < 0.01f)
		{
			this.m_Renderer.enabled = false;
			if (this.m_Light != null)
			{
				this.m_Light.enabled = false;
			}
		}
	}

	// Token: 0x0600AC72 RID: 44146 RVA: 0x0040551C File Offset: 0x0040371C
	public void Show()
	{
		this.m_Renderer.enabled = true;
		if (this.m_Light != null)
		{
			this.m_Light.enabled = true;
			this.m_Light.intensity = this.m_LightIntensity;
		}
		this.m_Color.a = 0.5f;
	}

	// Token: 0x0600AC73 RID: 44147 RVA: 0x00405570 File Offset: 0x00403770
	public void Shoot()
	{
		this.ShootInternal(true);
	}

	// Token: 0x0600AC74 RID: 44148 RVA: 0x00405579 File Offset: 0x00403779
	public void ShootLightOnly()
	{
		this.ShootInternal(false);
	}

	// Token: 0x0600AC75 RID: 44149 RVA: 0x00405584 File Offset: 0x00403784
	public void ShootInternal(bool showMesh)
	{
		this.m_Color.a = 0.5f;
		if (showMesh)
		{
			this.m_Transform.Rotate(0f, 0f, (float)global::UnityEngine.Random.Range(0, 360));
			this.m_Renderer.enabled = true;
		}
		if (this.m_Light != null)
		{
			this.m_Light.enabled = true;
			this.m_Light.intensity = this.m_LightIntensity;
		}
	}

	// Token: 0x0600AC76 RID: 44150 RVA: 0x004055FC File Offset: 0x004037FC
	public void SetFadeSpeed(float fadeSpeed)
	{
		this.FadeSpeed = fadeSpeed;
	}

	// Token: 0x04008013 RID: 32787
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FadeSpeed = 0.075f;

	// Token: 0x04008014 RID: 32788
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ForceShow;

	// Token: 0x04008015 RID: 32789
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_Color = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04008016 RID: 32790
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008017 RID: 32791
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x04008018 RID: 32792
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LightIntensity;

	// Token: 0x04008019 RID: 32793
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x0400801A RID: 32794
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material m_Material;
}

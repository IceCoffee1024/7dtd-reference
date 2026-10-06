using System;
using UnityEngine;

// Token: 0x020015C8 RID: 5576
public class vp_MuzzleFlash : MonoBehaviour
{
	// Token: 0x17001451 RID: 5201
	// (get) Token: 0x0600AE1B RID: 44571 RVA: 0x0040F4A4 File Offset: 0x0040D6A4
	// (set) Token: 0x0600AE1C RID: 44572 RVA: 0x0040F4AC File Offset: 0x0040D6AC
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

	// Token: 0x17001452 RID: 5202
	// (get) Token: 0x0600AE1D RID: 44573 RVA: 0x0040F4B5 File Offset: 0x0040D6B5
	// (set) Token: 0x0600AE1E RID: 44574 RVA: 0x0040F4BD File Offset: 0x0040D6BD
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

	// Token: 0x0600AE1F RID: 44575 RVA: 0x0040F4C8 File Offset: 0x0040D6C8
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

	// Token: 0x0600AE20 RID: 44576 RVA: 0x0040F584 File Offset: 0x0040D784
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		GameObject gameObject = GameObject.Find("WeaponCamera");
		if (gameObject != null && gameObject.transform.parent == this.m_Transform.parent)
		{
			base.gameObject.layer = 10;
		}
	}

	// Token: 0x0600AE21 RID: 44577 RVA: 0x0040F5D0 File Offset: 0x0040D7D0
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

	// Token: 0x0600AE22 RID: 44578 RVA: 0x0040F6B0 File Offset: 0x0040D8B0
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

	// Token: 0x0600AE23 RID: 44579 RVA: 0x0040F704 File Offset: 0x0040D904
	public void Shoot()
	{
		this.ShootInternal(true);
	}

	// Token: 0x0600AE24 RID: 44580 RVA: 0x0040F70D File Offset: 0x0040D90D
	public void ShootLightOnly()
	{
		this.ShootInternal(false);
	}

	// Token: 0x0600AE25 RID: 44581 RVA: 0x0040F718 File Offset: 0x0040D918
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

	// Token: 0x0600AE26 RID: 44582 RVA: 0x0040F790 File Offset: 0x0040D990
	public void SetFadeSpeed(float fadeSpeed)
	{
		this.FadeSpeed = fadeSpeed;
	}

	// Token: 0x0400816F RID: 33135
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FadeSpeed = 0.075f;

	// Token: 0x04008170 RID: 33136
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ForceShow;

	// Token: 0x04008171 RID: 33137
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_Color = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04008172 RID: 33138
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008173 RID: 33139
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x04008174 RID: 33140
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LightIntensity;

	// Token: 0x04008175 RID: 33141
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x04008176 RID: 33142
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material m_Material;
}

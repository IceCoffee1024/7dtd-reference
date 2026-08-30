using System;
using UnityEngine;

// Token: 0x020015A6 RID: 5542
public class vp_MuzzleFlash : MonoBehaviour
{
	// Token: 0x1700140C RID: 5132
	// (get) Token: 0x0600AD68 RID: 44392 RVA: 0x0040A650 File Offset: 0x00408850
	// (set) Token: 0x0600AD69 RID: 44393 RVA: 0x0040A658 File Offset: 0x00408858
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

	// Token: 0x1700140D RID: 5133
	// (get) Token: 0x0600AD6A RID: 44394 RVA: 0x0040A661 File Offset: 0x00408861
	// (set) Token: 0x0600AD6B RID: 44395 RVA: 0x0040A669 File Offset: 0x00408869
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

	// Token: 0x0600AD6C RID: 44396 RVA: 0x0040A674 File Offset: 0x00408874
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

	// Token: 0x0600AD6D RID: 44397 RVA: 0x0040A730 File Offset: 0x00408930
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		GameObject gameObject = GameObject.Find("WeaponCamera");
		if (gameObject != null && gameObject.transform.parent == this.m_Transform.parent)
		{
			base.gameObject.layer = 10;
		}
	}

	// Token: 0x0600AD6E RID: 44398 RVA: 0x0040A77C File Offset: 0x0040897C
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

	// Token: 0x0600AD6F RID: 44399 RVA: 0x0040A85C File Offset: 0x00408A5C
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

	// Token: 0x0600AD70 RID: 44400 RVA: 0x0040A8B0 File Offset: 0x00408AB0
	public void Shoot()
	{
		this.ShootInternal(true);
	}

	// Token: 0x0600AD71 RID: 44401 RVA: 0x0040A8B9 File Offset: 0x00408AB9
	public void ShootLightOnly()
	{
		this.ShootInternal(false);
	}

	// Token: 0x0600AD72 RID: 44402 RVA: 0x0040A8C4 File Offset: 0x00408AC4
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

	// Token: 0x0600AD73 RID: 44403 RVA: 0x0040A93C File Offset: 0x00408B3C
	public void SetFadeSpeed(float fadeSpeed)
	{
		this.FadeSpeed = fadeSpeed;
	}

	// Token: 0x040080EC RID: 33004
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FadeSpeed = 0.075f;

	// Token: 0x040080ED RID: 33005
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ForceShow;

	// Token: 0x040080EE RID: 33006
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_Color = new Color(1f, 1f, 1f, 0f);

	// Token: 0x040080EF RID: 33007
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040080F0 RID: 33008
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x040080F1 RID: 33009
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LightIntensity;

	// Token: 0x040080F2 RID: 33010
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x040080F3 RID: 33011
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material m_Material;
}

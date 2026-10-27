using System;
using UnityEngine;

// Token: 0x020015DA RID: 5594
public class vp_MuzzleFlash : MonoBehaviour
{
	// Token: 0x1700145D RID: 5213
	// (get) Token: 0x0600AE73 RID: 44659 RVA: 0x004118A8 File Offset: 0x0040FAA8
	// (set) Token: 0x0600AE74 RID: 44660 RVA: 0x004118B0 File Offset: 0x0040FAB0
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

	// Token: 0x1700145E RID: 5214
	// (get) Token: 0x0600AE75 RID: 44661 RVA: 0x004118B9 File Offset: 0x0040FAB9
	// (set) Token: 0x0600AE76 RID: 44662 RVA: 0x004118C1 File Offset: 0x0040FAC1
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

	// Token: 0x0600AE77 RID: 44663 RVA: 0x004118CC File Offset: 0x0040FACC
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

	// Token: 0x0600AE78 RID: 44664 RVA: 0x00411988 File Offset: 0x0040FB88
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		GameObject gameObject = GameObject.Find("WeaponCamera");
		if (gameObject != null && gameObject.transform.parent == this.m_Transform.parent)
		{
			base.gameObject.layer = 10;
		}
	}

	// Token: 0x0600AE79 RID: 44665 RVA: 0x004119D4 File Offset: 0x0040FBD4
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

	// Token: 0x0600AE7A RID: 44666 RVA: 0x00411AB4 File Offset: 0x0040FCB4
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

	// Token: 0x0600AE7B RID: 44667 RVA: 0x00411B08 File Offset: 0x0040FD08
	public void Shoot()
	{
		this.ShootInternal(true);
	}

	// Token: 0x0600AE7C RID: 44668 RVA: 0x00411B11 File Offset: 0x0040FD11
	public void ShootLightOnly()
	{
		this.ShootInternal(false);
	}

	// Token: 0x0600AE7D RID: 44669 RVA: 0x00411B1C File Offset: 0x0040FD1C
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

	// Token: 0x0600AE7E RID: 44670 RVA: 0x00411B94 File Offset: 0x0040FD94
	public void SetFadeSpeed(float fadeSpeed)
	{
		this.FadeSpeed = fadeSpeed;
	}

	// Token: 0x040081B0 RID: 33200
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FadeSpeed = 0.075f;

	// Token: 0x040081B1 RID: 33201
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ForceShow;

	// Token: 0x040081B2 RID: 33202
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_Color = new Color(1f, 1f, 1f, 0f);

	// Token: 0x040081B3 RID: 33203
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081B4 RID: 33204
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Light m_Light;

	// Token: 0x040081B5 RID: 33205
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LightIntensity;

	// Token: 0x040081B6 RID: 33206
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Renderer m_Renderer;

	// Token: 0x040081B7 RID: 33207
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Material m_Material;
}

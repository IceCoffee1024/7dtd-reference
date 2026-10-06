using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class DroneRunningLight : MonoBehaviour
{
	// Token: 0x06001E2C RID: 7724 RVA: 0x000B7BA9 File Offset: 0x000B5DA9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.runningLight = base.GetComponent<Light>();
		this.particles = base.transform.GetComponentInChildren<ParticleSystem>();
		this._initLights();
		this.setLightsActive(true);
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000B7BD5 File Offset: 0x000B5DD5
	[PublicizedFrom(EAccessModifier.Private)]
	public void _initLights()
	{
		this.lightBlinkTimer = this.LightBlinkInterval;
		this.startIntensity = this.MinLightIntensity;
		this.runningLight.intensity = this.startIntensity;
		this.setLightColor(this.LightColor);
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000B7C0C File Offset: 0x000B5E0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLightsActive(bool value)
	{
		this.runningLight.enabled = value;
		if (this.connectedLights != null)
		{
			for (int i = 0; i < this.connectedLights.Count; i++)
			{
				this.connectedLights[i].enabled = value;
			}
		}
		if (!this.dayTimeVisibility)
		{
			this.particles.gameObject.SetActive(value);
		}
		this.lightsActive = value;
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x000B7C78 File Offset: 0x000B5E78
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLightColor(Color color)
	{
		this.runningLight.color = color;
		this.particles.main.startColor = color;
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000B7CAC File Offset: 0x000B5EAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		float num = (float)GameUtils.WorldTimeToHours(world.worldTime);
		if (num > 4f && num < 22f)
		{
			if (this.runningLight.intensity > this.startIntensity)
			{
				this._initLights();
			}
			if (this.lightsActive)
			{
				this.setLightsActive(!this.lightsActive);
			}
			return;
		}
		if (!this.lightsActive)
		{
			this.setLightsActive(!this.lightsActive);
		}
		if (this.runningLight.color != this.LightColor || this.particles.main.startColor.color != this.LightColor)
		{
			this.setLightColor(this.LightColor);
		}
		if (this.startIntensity != this.MinLightIntensity)
		{
			this.startIntensity = this.MinLightIntensity;
		}
		if (this.lightBlinkTimer > 0f)
		{
			this.lightBlinkTimer -= Time.deltaTime;
			if (this.lightBlinkTimer < 0.2f && this.lightBlinkTimer > 0.15f && this.particles.gameObject.activeSelf)
			{
				this.particles.gameObject.SetActive(false);
			}
			if (this.lightBlinkTimer < 0.15f && !this.particles.gameObject.activeSelf)
			{
				this.particles.gameObject.SetActive(true);
			}
			if (this.lightBlinkTimer <= 0f)
			{
				base.StartCoroutine(this.blink());
			}
		}
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x000B7E3E File Offset: 0x000B603E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator blink()
	{
		this.transitionTimer = this.transitionTime;
		while (this.runningLight.intensity < this.MaxLightIntensity)
		{
			this.transitionTimer += Time.deltaTime;
			this.runningLight.intensity = Mathf.Lerp(this.startIntensity, this.MaxLightIntensity, this.transitionTimer / this.transitionTime);
			yield return null;
		}
		this.transitionTimer = this.transitionTime;
		while (this.runningLight.intensity > this.startIntensity)
		{
			this.transitionTimer += Time.deltaTime;
			this.runningLight.intensity = Mathf.Lerp(this.MaxLightIntensity, this.startIntensity, this.transitionTimer / this.transitionTime);
			yield return null;
		}
		this.runningLight.intensity = this.startIntensity;
		this.lightBlinkTimer = this.LightBlinkInterval;
		yield return null;
		yield break;
	}

	// Token: 0x0400141C RID: 5148
	public float MinLightIntensity;

	// Token: 0x0400141D RID: 5149
	public float MaxLightIntensity;

	// Token: 0x0400141E RID: 5150
	public float LightBlinkInterval;

	// Token: 0x0400141F RID: 5151
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light runningLight;

	// Token: 0x04001420 RID: 5152
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startIntensity;

	// Token: 0x04001421 RID: 5153
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ParticleSystem particles;

	// Token: 0x04001422 RID: 5154
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightBlinkTimer;

	// Token: 0x04001423 RID: 5155
	public Color LightColor;

	// Token: 0x04001424 RID: 5156
	public List<Light> connectedLights;

	// Token: 0x04001425 RID: 5157
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lightsActive;

	// Token: 0x04001426 RID: 5158
	public bool dayTimeVisibility;

	// Token: 0x04001427 RID: 5159
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float transitionTime = 0.2f;

	// Token: 0x04001428 RID: 5160
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float transitionTimer;
}

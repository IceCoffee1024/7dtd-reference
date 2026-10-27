using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class DroneRunningLight : MonoBehaviour
{
	// Token: 0x06001E31 RID: 7729 RVA: 0x000B7F55 File Offset: 0x000B6155
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.runningLight = base.GetComponent<Light>();
		this.particles = base.transform.GetComponentInChildren<ParticleSystem>();
		this._initLights();
		this.setLightsActive(true);
	}

	// Token: 0x06001E32 RID: 7730 RVA: 0x000B7F81 File Offset: 0x000B6181
	[PublicizedFrom(EAccessModifier.Private)]
	public void _initLights()
	{
		this.lightBlinkTimer = this.LightBlinkInterval;
		this.startIntensity = this.MinLightIntensity;
		this.runningLight.intensity = this.startIntensity;
		this.setLightColor(this.LightColor);
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x000B7FB8 File Offset: 0x000B61B8
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

	// Token: 0x06001E34 RID: 7732 RVA: 0x000B8024 File Offset: 0x000B6224
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLightColor(Color color)
	{
		this.runningLight.color = color;
		this.particles.main.startColor = color;
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x000B8058 File Offset: 0x000B6258
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

	// Token: 0x06001E36 RID: 7734 RVA: 0x000B81EA File Offset: 0x000B63EA
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

	// Token: 0x0400142C RID: 5164
	public float MinLightIntensity;

	// Token: 0x0400142D RID: 5165
	public float MaxLightIntensity;

	// Token: 0x0400142E RID: 5166
	public float LightBlinkInterval;

	// Token: 0x0400142F RID: 5167
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light runningLight;

	// Token: 0x04001430 RID: 5168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startIntensity;

	// Token: 0x04001431 RID: 5169
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ParticleSystem particles;

	// Token: 0x04001432 RID: 5170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightBlinkTimer;

	// Token: 0x04001433 RID: 5171
	public Color LightColor;

	// Token: 0x04001434 RID: 5172
	public List<Light> connectedLights;

	// Token: 0x04001435 RID: 5173
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lightsActive;

	// Token: 0x04001436 RID: 5174
	public bool dayTimeVisibility;

	// Token: 0x04001437 RID: 5175
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float transitionTime = 0.2f;

	// Token: 0x04001438 RID: 5176
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float transitionTimer;
}

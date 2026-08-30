using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C90 RID: 3216
[Preserve]
public class VPHeadlight : VehiclePart
{
	// Token: 0x060062A5 RID: 25253 RVA: 0x0026C5DC File Offset: 0x0026A7DC
	public override void InitPrefabConnections()
	{
		this.headlightT = base.GetTransform();
		if (this.headlightT)
		{
			GameObject gameObject = this.headlightT.gameObject;
			this.lights = new List<Light>();
			gameObject.GetComponentsInChildren<Light>(true, this.lights);
			for (int i = this.lights.Count - 1; i >= 0; i--)
			{
				if (this.lights[i].type != LightType.Spot)
				{
					this.lights.RemoveAt(i);
				}
			}
		}
		Transform transform = base.GetTransform("matT");
		if (transform)
		{
			MeshRenderer componentInChildren = transform.GetComponentInChildren<MeshRenderer>();
			if (componentInChildren)
			{
				this.headlightMat = componentInChildren.material;
			}
		}
		this.modT = base.GetTransform("modT");
		if (this.modT)
		{
			List<MeshRenderer> list = new List<MeshRenderer>();
			this.modT.GetComponentsInChildren<MeshRenderer>(list);
			for (int j = 0; j < list.Count; j++)
			{
				MeshRenderer meshRenderer = list[j];
				if (meshRenderer.gameObject.CompareTag("LOD"))
				{
					if (!this.modMat)
					{
						this.modMat = meshRenderer.material;
					}
					else
					{
						meshRenderer.material = this.modMat;
					}
				}
			}
		}
		this.modOnT = base.GetTransform("modOnT");
		if (this.modOnT)
		{
			GameObject gameObject2 = this.modOnT.gameObject;
			this.modLights = new List<Light>();
			gameObject2.GetComponentsInChildren<Light>(true, this.modLights);
			for (int k = this.modLights.Count - 1; k >= 0; k--)
			{
				if (this.modLights[k].type != LightType.Spot)
				{
					this.modLights.RemoveAt(k);
				}
			}
		}
		this.curIntensity = -1f;
	}

	// Token: 0x060062A6 RID: 25254 RVA: 0x0026C7A0 File Offset: 0x0026A9A0
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		StringParsers.TryParseFloat(base.GetProperty("bright"), out this.bright);
		this.properties.ParseColorHex("matEmissive", ref this.headLightEmissive);
		this.properties.ParseColorHex("modMatEmissive", ref this.modMatEmissive);
		this.properties.ParseColorHex("tailEmissive", ref this.tailLightEmissive);
	}

	// Token: 0x060062A7 RID: 25255 RVA: 0x0026C80D File Offset: 0x0026AA0D
	public override void SetMods()
	{
		base.SetMods();
		this.UpdateOn();
	}

	// Token: 0x060062A8 RID: 25256 RVA: 0x0026C81B File Offset: 0x0026AA1B
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _arg)
	{
		if (_event == VehiclePart.Event.LightsOn)
		{
			this.SetOn(_arg != 0f);
			this.PlaySound();
		}
	}

	// Token: 0x060062A9 RID: 25257 RVA: 0x0026C838 File Offset: 0x0026AA38
	public override void Update(float _dt)
	{
		if (this.IsBroken())
		{
			this.SetOn(false);
			return;
		}
		if (this.lights != null)
		{
			float num = (this.vehicle.EffectLightIntensity - 1f) * 0.5f + 1f;
			num = Utils.FastClamp(num, 0f, 10f);
			if (num != this.curIntensity)
			{
				this.curIntensity = num;
				float num2 = this.bright * num;
				float num3 = 50f * num;
				if (this.modInstalled && this.modLights != null)
				{
					num2 *= 0.58f;
					for (int i = this.modLights.Count - 1; i >= 0; i--)
					{
						Light light = this.modLights[i];
						light.intensity = num2;
						light.range = num3;
					}
				}
				for (int j = this.lights.Count - 1; j >= 0; j--)
				{
					Light light2 = this.lights[j];
					light2.intensity = num2;
					light2.range = num3;
				}
			}
		}
		this.SetTailLights();
	}

	// Token: 0x060062AA RID: 25258 RVA: 0x0026C938 File Offset: 0x0026AB38
	public void SetTailLights()
	{
		float num = 0f;
		if (this.isOn)
		{
			num = 0.5f;
		}
		if (this.vehicle.CurrentIsBreak)
		{
			num = 1f;
		}
		if (num == this.tailLightIntensity)
		{
			return;
		}
		this.tailLightIntensity = num;
		if (this.vehicle.mainEmissiveMat && this.tailLightEmissive.a > 0f)
		{
			Color color = this.tailLightEmissive;
			color.r *= num;
			color.g *= num;
			color.b *= num;
			this.vehicle.mainEmissiveMat.SetColor("_EmissionColor", color);
		}
	}

	// Token: 0x060062AB RID: 25259 RVA: 0x0026C9E2 File Offset: 0x0026ABE2
	public bool IsOn()
	{
		return this.isOn;
	}

	// Token: 0x060062AC RID: 25260 RVA: 0x0026C9EA File Offset: 0x0026ABEA
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetOn(bool _isOn)
	{
		if (_isOn == this.isOn)
		{
			return;
		}
		this.isOn = _isOn;
		this.UpdateOn();
		this.SetTailLights();
	}

	// Token: 0x060062AD RID: 25261 RVA: 0x0026CA0C File Offset: 0x0026AC0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateOn()
	{
		this.curIntensity = -1f;
		if (this.headlightT)
		{
			this.headlightT.gameObject.SetActive(this.isOn);
		}
		if (this.headlightMat)
		{
			Color color = (this.isOn ? this.headLightEmissive : Color.black);
			this.headlightMat.SetColor("_EmissionColor", color);
		}
		if (this.modInstalled)
		{
			if (this.modOnT)
			{
				this.modOnT.gameObject.SetActive(this.isOn);
			}
			if (this.modMat)
			{
				Color color2 = (this.isOn ? this.modMatEmissive : Color.black);
				this.modMat.SetColor("_EmissionColor", color2);
			}
		}
	}

	// Token: 0x060062AE RID: 25262 RVA: 0x0026CADA File Offset: 0x0026ACDA
	public float GetLightLevel()
	{
		if (!this.isOn)
		{
			return 0f;
		}
		return this.bright * 3f;
	}

	// Token: 0x060062AF RID: 25263 RVA: 0x0026CAF8 File Offset: 0x0026ACF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlaySound()
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (!primaryPlayer || !primaryPlayer.IsSpawned())
		{
			return;
		}
		if (this.vehicle.entity != null && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot("UseActions/flashlight_toggle", false, false, false, null, 1f);
		}
	}

	// Token: 0x04004C7C RID: 19580
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRange = 50f;

	// Token: 0x04004C7D RID: 19581
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cModBrightPer = 0.58f;

	// Token: 0x04004C7E RID: 19582
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform headlightT;

	// Token: 0x04004C7F RID: 19583
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> lights;

	// Token: 0x04004C80 RID: 19584
	[PublicizedFrom(EAccessModifier.Private)]
	public Material headlightMat;

	// Token: 0x04004C81 RID: 19585
	[PublicizedFrom(EAccessModifier.Private)]
	public Color headLightEmissive;

	// Token: 0x04004C82 RID: 19586
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modT;

	// Token: 0x04004C83 RID: 19587
	[PublicizedFrom(EAccessModifier.Private)]
	public Material modMat;

	// Token: 0x04004C84 RID: 19588
	[PublicizedFrom(EAccessModifier.Private)]
	public Color modMatEmissive;

	// Token: 0x04004C85 RID: 19589
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modOnT;

	// Token: 0x04004C86 RID: 19590
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> modLights;

	// Token: 0x04004C87 RID: 19591
	[PublicizedFrom(EAccessModifier.Private)]
	public float bright;

	// Token: 0x04004C88 RID: 19592
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tailLightEmissive;

	// Token: 0x04004C89 RID: 19593
	[PublicizedFrom(EAccessModifier.Private)]
	public float tailLightIntensity = -1f;

	// Token: 0x04004C8A RID: 19594
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x04004C8B RID: 19595
	[PublicizedFrom(EAccessModifier.Private)]
	public float curIntensity;
}

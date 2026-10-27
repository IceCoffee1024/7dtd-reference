using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CBB RID: 3259
[Preserve]
public class VPHeadlight : VehiclePart
{
	// Token: 0x06006348 RID: 25416 RVA: 0x00273E20 File Offset: 0x00272020
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

	// Token: 0x06006349 RID: 25417 RVA: 0x00273FE4 File Offset: 0x002721E4
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		StringParsers.TryParseFloat(base.GetProperty("bright"), out this.bright);
		this.properties.ParseColorHex("matEmissive", ref this.headLightEmissive);
		this.properties.ParseColorHex("modMatEmissive", ref this.modMatEmissive);
		this.properties.ParseColorHex("tailEmissive", ref this.tailLightEmissive);
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x00274051 File Offset: 0x00272251
	public override void SetMods()
	{
		base.SetMods();
		this.UpdateOn();
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x0027405F File Offset: 0x0027225F
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _arg)
	{
		if (_event == VehiclePart.Event.LightsOn)
		{
			this.SetOn(_arg != 0f);
			this.PlaySound();
		}
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x0027407C File Offset: 0x0027227C
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

	// Token: 0x0600634D RID: 25421 RVA: 0x0027417C File Offset: 0x0027237C
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

	// Token: 0x0600634E RID: 25422 RVA: 0x00274226 File Offset: 0x00272426
	public bool IsOn()
	{
		return this.isOn;
	}

	// Token: 0x0600634F RID: 25423 RVA: 0x0027422E File Offset: 0x0027242E
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

	// Token: 0x06006350 RID: 25424 RVA: 0x00274250 File Offset: 0x00272450
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

	// Token: 0x06006351 RID: 25425 RVA: 0x0027431E File Offset: 0x0027251E
	public float GetLightLevel()
	{
		if (!this.isOn)
		{
			return 0f;
		}
		return this.bright * 3f;
	}

	// Token: 0x06006352 RID: 25426 RVA: 0x0027433C File Offset: 0x0027253C
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

	// Token: 0x04004D74 RID: 19828
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRange = 50f;

	// Token: 0x04004D75 RID: 19829
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cModBrightPer = 0.58f;

	// Token: 0x04004D76 RID: 19830
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform headlightT;

	// Token: 0x04004D77 RID: 19831
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> lights;

	// Token: 0x04004D78 RID: 19832
	[PublicizedFrom(EAccessModifier.Private)]
	public Material headlightMat;

	// Token: 0x04004D79 RID: 19833
	[PublicizedFrom(EAccessModifier.Private)]
	public Color headLightEmissive;

	// Token: 0x04004D7A RID: 19834
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modT;

	// Token: 0x04004D7B RID: 19835
	[PublicizedFrom(EAccessModifier.Private)]
	public Material modMat;

	// Token: 0x04004D7C RID: 19836
	[PublicizedFrom(EAccessModifier.Private)]
	public Color modMatEmissive;

	// Token: 0x04004D7D RID: 19837
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modOnT;

	// Token: 0x04004D7E RID: 19838
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> modLights;

	// Token: 0x04004D7F RID: 19839
	[PublicizedFrom(EAccessModifier.Private)]
	public float bright;

	// Token: 0x04004D80 RID: 19840
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tailLightEmissive;

	// Token: 0x04004D81 RID: 19841
	[PublicizedFrom(EAccessModifier.Private)]
	public float tailLightIntensity = -1f;

	// Token: 0x04004D82 RID: 19842
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x04004D83 RID: 19843
	[PublicizedFrom(EAccessModifier.Private)]
	public float curIntensity;
}

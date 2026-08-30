using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CA2 RID: 3234
[Preserve]
public class VPHeadlight : VehiclePart
{
	// Token: 0x06006330 RID: 25392 RVA: 0x0026F4B4 File Offset: 0x0026D6B4
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

	// Token: 0x06006331 RID: 25393 RVA: 0x0026F678 File Offset: 0x0026D878
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		StringParsers.TryParseFloat(base.GetProperty("bright"), out this.bright);
		this.properties.ParseColorHex("matEmissive", ref this.headLightEmissive);
		this.properties.ParseColorHex("modMatEmissive", ref this.modMatEmissive);
		this.properties.ParseColorHex("tailEmissive", ref this.tailLightEmissive);
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x0026F6E5 File Offset: 0x0026D8E5
	public override void SetMods()
	{
		base.SetMods();
		this.UpdateOn();
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x0026F6F3 File Offset: 0x0026D8F3
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _arg)
	{
		if (_event == VehiclePart.Event.LightsOn)
		{
			this.SetOn(_arg != 0f);
			this.PlaySound();
		}
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x0026F710 File Offset: 0x0026D910
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

	// Token: 0x06006335 RID: 25397 RVA: 0x0026F810 File Offset: 0x0026DA10
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

	// Token: 0x06006336 RID: 25398 RVA: 0x0026F8BA File Offset: 0x0026DABA
	public bool IsOn()
	{
		return this.isOn;
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x0026F8C2 File Offset: 0x0026DAC2
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

	// Token: 0x06006338 RID: 25400 RVA: 0x0026F8E4 File Offset: 0x0026DAE4
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

	// Token: 0x06006339 RID: 25401 RVA: 0x0026F9B2 File Offset: 0x0026DBB2
	public float GetLightLevel()
	{
		if (!this.isOn)
		{
			return 0f;
		}
		return this.bright * 3f;
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x0026F9D0 File Offset: 0x0026DBD0
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

	// Token: 0x04004D19 RID: 19737
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRange = 50f;

	// Token: 0x04004D1A RID: 19738
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cModBrightPer = 0.58f;

	// Token: 0x04004D1B RID: 19739
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform headlightT;

	// Token: 0x04004D1C RID: 19740
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> lights;

	// Token: 0x04004D1D RID: 19741
	[PublicizedFrom(EAccessModifier.Private)]
	public Material headlightMat;

	// Token: 0x04004D1E RID: 19742
	[PublicizedFrom(EAccessModifier.Private)]
	public Color headLightEmissive;

	// Token: 0x04004D1F RID: 19743
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modT;

	// Token: 0x04004D20 RID: 19744
	[PublicizedFrom(EAccessModifier.Private)]
	public Material modMat;

	// Token: 0x04004D21 RID: 19745
	[PublicizedFrom(EAccessModifier.Private)]
	public Color modMatEmissive;

	// Token: 0x04004D22 RID: 19746
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modOnT;

	// Token: 0x04004D23 RID: 19747
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Light> modLights;

	// Token: 0x04004D24 RID: 19748
	[PublicizedFrom(EAccessModifier.Private)]
	public float bright;

	// Token: 0x04004D25 RID: 19749
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tailLightEmissive;

	// Token: 0x04004D26 RID: 19750
	[PublicizedFrom(EAccessModifier.Private)]
	public float tailLightIntensity = -1f;

	// Token: 0x04004D27 RID: 19751
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOn;

	// Token: 0x04004D28 RID: 19752
	[PublicizedFrom(EAccessModifier.Private)]
	public float curIntensity;
}

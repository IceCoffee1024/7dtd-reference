using System;
using Audio;
using UnityEngine;

// Token: 0x02001233 RID: 4659
public class LightLOD : MonoBehaviour
{
	// Token: 0x06009419 RID: 37913 RVA: 0x0037EC98 File Offset: 0x0037CE98
	public Light GetLight()
	{
		if (!this.myLight)
		{
			if (!this.otherLight)
			{
				this.myLight = base.GetComponent<Light>();
			}
			else
			{
				this.myLight = this.otherLight;
			}
			if (this.myLight)
			{
				this.shadowStateMaster = this.myLight.shadows;
				this.shadowStrengthMaster = this.myLight.shadowStrength;
			}
		}
		return this.myLight;
	}

	// Token: 0x0600941A RID: 37914 RVA: 0x0037ED0E File Offset: 0x0037CF0E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.Init();
	}

	// Token: 0x0600941B RID: 37915 RVA: 0x0037ED18 File Offset: 0x0037CF18
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		if (this.hasInitialized)
		{
			return;
		}
		this.hasInitialized = true;
		this.selfT = base.transform;
		Light light = this.GetLight();
		if (light)
		{
			if (GameManager.IsDedicatedServer && this.registeredRange == 0f)
			{
				this.SetRange(light.range);
			}
			this.lightIntensityMaster = light.intensity;
			this.lightRangeMaster = light.range;
			this.CalcViewDistance();
			if (this.lightStateType != LightStateType.Static)
			{
				Type type = Type.GetType(this.lightStateType.ToStringCached<LightStateType>());
				this.lightState = base.gameObject.GetComponent(type) as LightState;
				if (!this.lightState)
				{
					this.lightState = (LightState)base.gameObject.AddComponent(type);
				}
			}
		}
		if (this.RefFlare)
		{
			this.lensFlare = this.RefFlare.GetComponent<LensFlare>();
		}
		else
		{
			this.lensFlare = base.GetComponent<LensFlare>();
		}
		if (this.RefIlluminatedMaterials != null)
		{
			this.maxLightPowerValues = new float[this.RefIlluminatedMaterials.Length];
			this.cachedMaterials = new Material[this.RefIlluminatedMaterials.Length][];
			for (int i = 0; i < this.RefIlluminatedMaterials.Length; i++)
			{
				Transform transform = this.RefIlluminatedMaterials[i];
				if (transform)
				{
					Renderer component = transform.GetComponent<Renderer>();
					if (component != null)
					{
						Material material = component.material;
						if (material != null)
						{
							if (material.HasProperty("_LightPower"))
							{
								this.maxLightPowerValues[i] = Utils.FastMax(material.GetFloat("_LightPower"), 1f);
							}
							else
							{
								this.maxLightPowerValues[i] = 1f;
							}
						}
						this.cachedMaterials[i] = component.materials;
					}
				}
			}
		}
		if (this.lightStateStart != LightStateType.Static)
		{
			this.LightStateType = this.lightStateStart;
		}
		if (!GameManager.IsDedicatedServer)
		{
			this.audioPlayer = base.GetComponent<AudioPlayer>();
		}
	}

	// Token: 0x0600941C RID: 37916 RVA: 0x0037EF04 File Offset: 0x0037D104
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.Init();
		if (GameManager.Instance.World == null)
		{
			return;
		}
		this.lightRange = this.lightRangeMaster;
		this.lightIntensity = this.lightIntensityMaster;
		if (this.myLight)
		{
			this.myLight.enabled = false;
		}
		this.isInitialBlockDone = false;
		this.parentT = this.selfT.parent;
		LightManager.LightChanged(this.selfT.position + Origin.position);
		if (GameLightManager.Instance != null)
		{
			GameLightManager.Instance.AddLight(this);
		}
	}

	// Token: 0x0600941D RID: 37917 RVA: 0x0037EF9C File Offset: 0x0037D19C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckInitialBlock()
	{
		if (this.isInitialBlockDone || !this.parentT || !this.selfT)
		{
			return;
		}
		if (this.bed == null)
		{
			this.isInitialBlockDone = true;
			if (!this.isHeld && this.registeredRange == 0f && this.myLight)
			{
				this.SetRange(this.lightRange);
			}
			return;
		}
		World world = GameManager.Instance.World;
		BlockValue blockValue = world.GetBlock(this.bed.pos);
		Block block = blockValue.Block;
		if (block.isMultiBlock && blockValue.ischild)
		{
			Vector3i parentPos = block.multiBlockPos.GetParentPos(this.bed.pos, blockValue);
			blockValue = world.GetBlock(parentPos);
		}
		if (this.bToggleable)
		{
			this.SwitchOnOff((blockValue.meta & 2) > 0, false);
		}
		if (this.registeredRange == 0f && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.myLight)
		{
			this.SetRange(this.lightRange);
		}
		this.isInitialBlockDone = true;
	}

	// Token: 0x1700117E RID: 4478
	// (get) Token: 0x0600941E RID: 37918 RVA: 0x0037F0B4 File Offset: 0x0037D2B4
	// (set) Token: 0x0600941F RID: 37919 RVA: 0x0037F0BC File Offset: 0x0037D2BC
	public LightStateType LightStateType
	{
		get
		{
			return this.lightStateType;
		}
		set
		{
			if (this.lightStateType == value)
			{
				return;
			}
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			if (this.lightState)
			{
				global::UnityEngine.Object.Destroy(this.lightState);
			}
			this.lightStateType = value;
			if (this.lightStateType != LightStateType.Static)
			{
				Type type = Type.GetType(this.lightStateType.ToStringCached<LightStateType>());
				this.lightState = (LightState)base.gameObject.AddComponent(type);
			}
		}
	}

	// Token: 0x1700117F RID: 4479
	// (get) Token: 0x06009421 RID: 37921 RVA: 0x0037F14A File Offset: 0x0037D34A
	// (set) Token: 0x06009420 RID: 37920 RVA: 0x0037F12A File Offset: 0x0037D32A
	public float MaxIntensity
	{
		get
		{
			return this.lightIntensity;
		}
		set
		{
			this.lightIntensity = value;
			this.lightIntensityMaster = value;
			LightLOD.MaxIntensityEvent maxIntensityChanged = this.MaxIntensityChanged;
			if (maxIntensityChanged == null)
			{
				return;
			}
			maxIntensityChanged();
		}
	}

	// Token: 0x17001180 RID: 4480
	// (set) Token: 0x06009422 RID: 37922 RVA: 0x0037F152 File Offset: 0x0037D352
	public float LightAngle
	{
		set
		{
			if (this.myLight.type == LightType.Spot)
			{
				if (value > 160f)
				{
					value = 160f;
				}
				this.myLight.spotAngle = value;
			}
		}
	}

	// Token: 0x06009423 RID: 37923 RVA: 0x0037F17C File Offset: 0x0037D37C
	public void SetEmissiveColor(bool _on)
	{
		this.SetEmissiveColor(_on ? 1f : 0f);
	}

	// Token: 0x06009424 RID: 37924 RVA: 0x0037F194 File Offset: 0x0037D394
	public void SetEmissiveColor(float _newV)
	{
		if (_newV <= 0f)
		{
			this.SetEmissiveColor(LightLOD.EmissiveColorOff);
			return;
		}
		Color color;
		if (!this.EmissiveFromLightColorOn)
		{
			color = this.EmissiveColor;
		}
		else
		{
			Light light = this.GetLight();
			if (!light)
			{
				return;
			}
			color = light.color;
		}
		if (_newV >= 1f)
		{
			this.SetEmissiveColor(color);
		}
		float num;
		float num2;
		float num3;
		Color.RGBToHSV(color, out num, out num2, out num3);
		Color color2 = Color.HSVToRGB(num, num2, _newV);
		this.SetEmissiveColor(color2);
	}

	// Token: 0x06009425 RID: 37925 RVA: 0x0037F20C File Offset: 0x0037D40C
	public void SetEmissiveColor(Color _color)
	{
		if (this.RefIlluminatedMaterials == null)
		{
			return;
		}
		if (this.lastEmissiveColor != null && _color == this.lastEmissiveColor.Value)
		{
			return;
		}
		this.lastEmissiveColor = new Color?(_color);
		bool flag = _color != LightLOD.EmissiveColorOff;
		for (int i = 0; i < this.cachedMaterials.Length; i++)
		{
			Material[] array = this.cachedMaterials[i];
			if (array != null)
			{
				foreach (Material material in array)
				{
					if (material)
					{
						material.SetColor(LightLOD.EmissionColorId, _color);
						if (flag)
						{
							material.EnableKeyword("_EMISSION");
						}
						else
						{
							material.DisableKeyword("_EMISSION");
						}
					}
				}
			}
		}
	}

	// Token: 0x06009426 RID: 37926 RVA: 0x0037F2C0 File Offset: 0x0037D4C0
	public void SetRange(float _range)
	{
		Light light = this.GetLight();
		this.lightRange = _range;
		this.lightRangeMaster = _range;
		light.range = _range;
		this.CalcViewDistance();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && base.enabled && light.enabled)
		{
			this.UnregisterFromLightManager();
			this.registeredPos = light.transform.position + Origin.position;
			this.registeredRange = LightManager.RegisterLight(light);
		}
	}

	// Token: 0x06009427 RID: 37927 RVA: 0x0037F338 File Offset: 0x0037D538
	public void TestRegistration()
	{
		if (this.myLight)
		{
			this.SetRange(this.lightRange);
		}
	}

	// Token: 0x06009428 RID: 37928 RVA: 0x0037F353 File Offset: 0x0037D553
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcViewDistance()
	{
		this.lightViewDistance = Utils.FastMax(this.MaxDistance, this.lightRangeMaster * 1.5f);
	}

	// Token: 0x06009429 RID: 37929 RVA: 0x0037F372 File Offset: 0x0037D572
	public void SetBlockEntityData(BlockEntityData _bed)
	{
		this.bed = _bed;
	}

	// Token: 0x0600942A RID: 37930 RVA: 0x0037F37B File Offset: 0x0037D57B
	public void SwitchOnOff(bool _isOn, bool _ignoreToggle = false)
	{
		if ((_ignoreToggle || this.bToggleable) && _isOn != this.bSwitchedOn)
		{
			this.bSwitchedOn = _isOn;
		}
		if (this.bSwitchedOn)
		{
			base.enabled = true;
		}
	}

	// Token: 0x0600942B RID: 37931 RVA: 0x0037F3A7 File Offset: 0x0037D5A7
	public void SetCulled(bool _culled)
	{
		this.isCulled = _culled;
		if (this.myLight)
		{
			this.myLight.enabled = !_culled;
		}
	}

	// Token: 0x0600942C RID: 37932 RVA: 0x0037F3CC File Offset: 0x0037D5CC
	public void OnDisable()
	{
		if (!string.IsNullOrEmpty(this.cyclingAudioClipPath) && this.cyclingAudioHandle != null)
		{
			this.cyclingAudioHandle.SetVolume(0f);
		}
		if (GameManager.Instance.World != null)
		{
			LightManager.LightChanged(this.selfT.position + Origin.position);
			if (GameLightManager.Instance != null)
			{
				GameLightManager.Instance.RemoveLight(this);
			}
		}
		this.UnregisterFromLightManager();
		this.parentT = null;
	}

	// Token: 0x0600942D RID: 37933 RVA: 0x0037F443 File Offset: 0x0037D643
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnregisterFromLightManager()
	{
		if (this.registeredRange > 0f)
		{
			LightManager.UnRegisterLight(this.registeredPos, this.registeredRange);
			this.registeredRange = 0f;
		}
	}

	// Token: 0x0600942E RID: 37934 RVA: 0x0037F470 File Offset: 0x0037D670
	public void FrameUpdate(Vector3 _cameraPos)
	{
		bool flag = this.lightStateType > LightStateType.Static;
		this.priority = (float)(flag ? 1 : 0);
		if (this.bRenderingOff || !this.myLight)
		{
			return;
		}
		this.CheckInitialBlock();
		bool flag2 = this.bSwitchedOn && (!flag || this.lightState.CanBeOn);
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		if (!this.isCulled)
		{
			num = (this.selfT.position - _cameraPos).sqrMagnitude * this.DistanceScale;
			num2 = Mathf.Sqrt(num) - this.lightRange;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
		}
		float num3 = this.lightViewDistance;
		if (this.bPlayerPlacedLight)
		{
			num3 *= 1.2f;
		}
		if (LightLOD.DebugViewDistance > 0f)
		{
			num3 = Utils.FastMax(LightLOD.DebugViewDistance, this.lightRange + 0.01f);
		}
		float num4 = num3 * num3;
		float num5 = num / num4;
		float num6 = num3 - this.lightRange;
		if (flag2)
		{
			flag2 &= num2 < num6;
			if (this.lightStateType != LightStateType.Blinking)
			{
				flag2 &= num5 < (flag ? this.lightState.LODThreshold : 1f);
			}
		}
		if (flag2 && !this.bWorksUnderwater)
		{
			bool flag3 = false;
			if (this.waterLevelRecheckTime != 3.4028235E+38f)
			{
				if (Time.time >= this.waterLevelRecheckTime)
				{
					this.waterLevelRecheckTime = float.MaxValue;
					if (this.WaterLevelDirty)
					{
						this.waterLevelRecheckTime = Time.time + 0.5f;
					}
					this.WaterLevelDirty = false;
					flag3 = this.bed != null;
				}
			}
			else if (this.WaterLevelDirty)
			{
				this.WaterLevelDirty = false;
				flag3 = this.bed != null;
				if (flag3)
				{
					this.waterLevelRecheckTime = Time.time + 0.5f;
				}
			}
			if (flag3)
			{
				World world = GameManager.Instance.World;
				bool flag4 = world.IsWater(this.bed.pos.x, this.bed.pos.y + 1, this.bed.pos.z);
				if (!flag4 && world.IsWater(this.bed.pos))
				{
					flag4 = 0.6f + (float)this.bed.pos.y > base.transform.position.y;
				}
				if (this.isUnderwater != flag4)
				{
					this.isUnderwater = flag4;
				}
			}
			flag2 &= !this.isUnderwater;
		}
		if (flag2)
		{
			if (this.bPlayerPlacedLight)
			{
				if (num5 >= 0.64000005f)
				{
					this.myLight.shadows = LightShadows.None;
				}
				else if (num5 >= 0.0625f)
				{
					if (this.shadowStateMaster == LightShadows.Soft)
					{
						this.myLight.shadows = LightShadows.Hard;
					}
					this.myLight.shadowStrength = (1f - Utils.FastClamp01((num5 - 0.36f) / 0.28000003f)) * this.shadowStrengthMaster;
				}
				else
				{
					this.myLight.shadows = this.shadowStateMaster;
					this.myLight.shadowStrength = this.shadowStrengthMaster;
				}
			}
			float num7 = num2 / num6;
			float num8 = 1f - num7 * num7;
			this.myLight.intensity = this.lightIntensity * num8 * (flag ? this.lightState.Intensity : 1f);
			this.myLight.range = this.lightRange * 0.5f + this.lightRange * 0.5f * num8;
			this.SetEmissiveColor(flag ? this.lightState.Emissive : 1f);
			if (num8 < 0.9f && this.myLight.enabled)
			{
				this.priority = 1f;
			}
			if (flag && this.lightState.AudioFrequency > 0f && !string.IsNullOrEmpty(this.cyclingAudioClipPath))
			{
				float num9 = this.cyclingAudioPlayEveryNCycles * (1f / this.lightState.AudioFrequency);
				float num10 = (Time.time + this.cyclingAudioOffset) % num9;
				if (num10 < this.previousCycleTime)
				{
					this.cyclingAudioHandle = Manager.Play(this.selfT.position + Origin.position, this.cyclingAudioClipPath, -1, true, 1f);
				}
				this.previousCycleTime = num10;
			}
			if (this.bed != null)
			{
				Block block = this.bed.blockValue.Block;
				if (block.RadiusEffects != null)
				{
					GameManager.Instance.World.GetPrimaryPlayer().BlockRadiusEffectsApply(block, this.selfT.position + Origin.position);
				}
			}
		}
		else
		{
			this.SetEmissiveColor(false);
		}
		this.myLight.enabled = flag2;
		if (this.LitRootObject)
		{
			this.LitRootObject.SetActive(flag2);
		}
		if (this.audioPlayer)
		{
			this.audioPlayer.enabled = flag2;
		}
		if (this.lensFlare)
		{
			if (flag2 && num < 10f * num4)
			{
				float num11 = (1f - num / (num4 * 10f)) * this.lightIntensity * 0.33f * this.FlareBrightnessFactor;
				if (num11 > 1f)
				{
					num11 = 1f;
				}
				if (this.lightRange < 4f)
				{
					num11 *= this.lightRange * 0.25f;
				}
				this.lensFlare.brightness = num11;
				this.lensFlare.color = this.myLight.color;
				this.lensFlare.enabled = true;
			}
			else
			{
				this.lensFlare.enabled = false;
			}
		}
		base.enabled = this.bSwitchedOn;
	}

	// Token: 0x0600942F RID: 37935 RVA: 0x0037FA04 File Offset: 0x0037DC04
	public void SetRenderingOn()
	{
		this.bRenderingOff = false;
	}

	// Token: 0x06009430 RID: 37936 RVA: 0x0037FA0D File Offset: 0x0037DC0D
	public void SetRenderingOff()
	{
		this.bRenderingOff = true;
	}

	// Token: 0x04006EEB RID: 28395
	public GameObject LitRootObject;

	// Token: 0x04006EEC RID: 28396
	public Transform[] RefIlluminatedMaterials;

	// Token: 0x04006EED RID: 28397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material[][] cachedMaterials;

	// Token: 0x04006EEE RID: 28398
	public Transform RefFlare;

	// Token: 0x04006EEF RID: 28399
	public float MaxDistance = 30f;

	// Token: 0x04006EF0 RID: 28400
	public float DistanceScale = 1f;

	// Token: 0x04006EF1 RID: 28401
	public float FlareBrightnessFactor = 1f;

	// Token: 0x04006EF2 RID: 28402
	public bool bPlayerPlacedLight;

	// Token: 0x04006EF3 RID: 28403
	public bool bSwitchedOn;

	// Token: 0x04006EF4 RID: 28404
	public bool bToggleable = true;

	// Token: 0x04006EF5 RID: 28405
	public bool bWorksUnderwater = true;

	// Token: 0x04006EF6 RID: 28406
	public bool isHeld;

	// Token: 0x04006EF7 RID: 28407
	public Light otherLight;

	// Token: 0x04006EF8 RID: 28408
	public LightStateType lightStateStart;

	// Token: 0x04006EF9 RID: 28409
	public string cyclingAudioClipPath;

	// Token: 0x04006EFA RID: 28410
	public float cyclingAudioOffset;

	// Token: 0x04006EFB RID: 28411
	public float cyclingAudioPlayEveryNCycles = 1f;

	// Token: 0x04006EFC RID: 28412
	public bool EmissiveFromLightColorOn;

	// Token: 0x04006EFD RID: 28413
	public Color EmissiveColor = Color.white;

	// Token: 0x04006EFE RID: 28414
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color EmissiveColorOff = Color.black;

	// Token: 0x04006EFF RID: 28415
	public float StateRate = 1f;

	// Token: 0x04006F00 RID: 28416
	public float FluxDelay = 1f;

	// Token: 0x04006F01 RID: 28417
	public static float DebugViewDistance;

	// Token: 0x04006F02 RID: 28418
	public LightLOD.MaxIntensityEvent MaxIntensityChanged;

	// Token: 0x04006F03 RID: 28419
	public bool WaterLevelDirty;

	// Token: 0x04006F04 RID: 28420
	public float priority;

	// Token: 0x04006F05 RID: 28421
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCulled;

	// Token: 0x04006F06 RID: 28422
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform selfT;

	// Token: 0x04006F07 RID: 28423
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform parentT;

	// Token: 0x04006F08 RID: 28424
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

	// Token: 0x04006F09 RID: 28425
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasInitialized;

	// Token: 0x04006F0A RID: 28426
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockEntityData bed;

	// Token: 0x04006F0B RID: 28427
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isInitialBlockDone;

	// Token: 0x04006F0C RID: 28428
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light myLight;

	// Token: 0x04006F0D RID: 28429
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightIntensityMaster;

	// Token: 0x04006F0E RID: 28430
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightIntensity;

	// Token: 0x04006F0F RID: 28431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightRangeMaster;

	// Token: 0x04006F10 RID: 28432
	[NonSerialized]
	public float lightRange;

	// Token: 0x04006F11 RID: 28433
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightViewDistance;

	// Token: 0x04006F12 RID: 28434
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bRenderingOff;

	// Token: 0x04006F13 RID: 28435
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LensFlare lensFlare;

	// Token: 0x04006F14 RID: 28436
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] maxLightPowerValues;

	// Token: 0x04006F15 RID: 28437
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 registeredPos;

	// Token: 0x04006F16 RID: 28438
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float registeredRange;

	// Token: 0x04006F17 RID: 28439
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightStateType lightStateType;

	// Token: 0x04006F18 RID: 28440
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightState lightState;

	// Token: 0x04006F19 RID: 28441
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightShadows shadowStateMaster;

	// Token: 0x04006F1A RID: 28442
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float shadowStrengthMaster;

	// Token: 0x04006F1B RID: 28443
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioPlayer audioPlayer;

	// Token: 0x04006F1C RID: 28444
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Handle cyclingAudioHandle;

	// Token: 0x04006F1D RID: 28445
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isUnderwater;

	// Token: 0x04006F1E RID: 28446
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float waterLevelRecheckTime;

	// Token: 0x04006F1F RID: 28447
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color? lastEmissiveColor;

	// Token: 0x04006F20 RID: 28448
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float previousCycleTime;

	// Token: 0x02001234 RID: 4660
	// (Invoke) Token: 0x06009434 RID: 37940
	public delegate void MaxIntensityEvent();
}

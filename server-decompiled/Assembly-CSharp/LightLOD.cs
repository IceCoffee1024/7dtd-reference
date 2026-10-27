using System;
using Audio;
using UnityEngine;

// Token: 0x02001279 RID: 4729
public class LightLOD : MonoBehaviour
{
	// Token: 0x06009602 RID: 38402 RVA: 0x0038ABEC File Offset: 0x00388DEC
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

	// Token: 0x06009603 RID: 38403 RVA: 0x0038AC62 File Offset: 0x00388E62
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.Init();
	}

	// Token: 0x06009604 RID: 38404 RVA: 0x0038AC6C File Offset: 0x00388E6C
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

	// Token: 0x06009605 RID: 38405 RVA: 0x0038AE58 File Offset: 0x00389058
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

	// Token: 0x06009606 RID: 38406 RVA: 0x0038AEF0 File Offset: 0x003890F0
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

	// Token: 0x17001208 RID: 4616
	// (get) Token: 0x06009607 RID: 38407 RVA: 0x0038B008 File Offset: 0x00389208
	// (set) Token: 0x06009608 RID: 38408 RVA: 0x0038B010 File Offset: 0x00389210
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

	// Token: 0x17001209 RID: 4617
	// (get) Token: 0x0600960A RID: 38410 RVA: 0x0038B09E File Offset: 0x0038929E
	// (set) Token: 0x06009609 RID: 38409 RVA: 0x0038B07E File Offset: 0x0038927E
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

	// Token: 0x1700120A RID: 4618
	// (set) Token: 0x0600960B RID: 38411 RVA: 0x0038B0A6 File Offset: 0x003892A6
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

	// Token: 0x0600960C RID: 38412 RVA: 0x0038B0D0 File Offset: 0x003892D0
	public void SetEmissiveColor(bool _on)
	{
		this.SetEmissiveColor(_on ? 1f : 0f);
	}

	// Token: 0x0600960D RID: 38413 RVA: 0x0038B0E8 File Offset: 0x003892E8
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

	// Token: 0x0600960E RID: 38414 RVA: 0x0038B160 File Offset: 0x00389360
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

	// Token: 0x0600960F RID: 38415 RVA: 0x0038B214 File Offset: 0x00389414
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

	// Token: 0x06009610 RID: 38416 RVA: 0x0038B28C File Offset: 0x0038948C
	public void TestRegistration()
	{
		if (this.myLight)
		{
			this.SetRange(this.lightRange);
		}
	}

	// Token: 0x06009611 RID: 38417 RVA: 0x0038B2A7 File Offset: 0x003894A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcViewDistance()
	{
		this.lightViewDistance = Utils.FastMax(this.MaxDistance, this.lightRangeMaster * 1.5f);
	}

	// Token: 0x06009612 RID: 38418 RVA: 0x0038B2C6 File Offset: 0x003894C6
	public void SetBlockEntityData(BlockEntityData _bed)
	{
		this.bed = _bed;
	}

	// Token: 0x06009613 RID: 38419 RVA: 0x0038B2CF File Offset: 0x003894CF
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

	// Token: 0x06009614 RID: 38420 RVA: 0x0038B2FB File Offset: 0x003894FB
	public void SetCulled(bool _culled)
	{
		this.isCulled = _culled;
		if (this.myLight)
		{
			this.myLight.enabled = !_culled;
		}
	}

	// Token: 0x06009615 RID: 38421 RVA: 0x0038B320 File Offset: 0x00389520
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

	// Token: 0x06009616 RID: 38422 RVA: 0x0038B397 File Offset: 0x00389597
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnregisterFromLightManager()
	{
		if (this.registeredRange > 0f)
		{
			LightManager.UnRegisterLight(this.registeredPos, this.registeredRange);
			this.registeredRange = 0f;
		}
	}

	// Token: 0x06009617 RID: 38423 RVA: 0x0038B3C4 File Offset: 0x003895C4
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

	// Token: 0x06009618 RID: 38424 RVA: 0x0038B958 File Offset: 0x00389B58
	public void SetRenderingOn()
	{
		this.bRenderingOff = false;
	}

	// Token: 0x06009619 RID: 38425 RVA: 0x0038B961 File Offset: 0x00389B61
	public void SetRenderingOff()
	{
		this.bRenderingOff = true;
	}

	// Token: 0x0400706F RID: 28783
	public GameObject LitRootObject;

	// Token: 0x04007070 RID: 28784
	public Transform[] RefIlluminatedMaterials;

	// Token: 0x04007071 RID: 28785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material[][] cachedMaterials;

	// Token: 0x04007072 RID: 28786
	public Transform RefFlare;

	// Token: 0x04007073 RID: 28787
	public float MaxDistance = 30f;

	// Token: 0x04007074 RID: 28788
	public float DistanceScale = 1f;

	// Token: 0x04007075 RID: 28789
	public float FlareBrightnessFactor = 1f;

	// Token: 0x04007076 RID: 28790
	public bool bPlayerPlacedLight;

	// Token: 0x04007077 RID: 28791
	public bool bSwitchedOn;

	// Token: 0x04007078 RID: 28792
	public bool bToggleable = true;

	// Token: 0x04007079 RID: 28793
	public bool bWorksUnderwater = true;

	// Token: 0x0400707A RID: 28794
	public bool isHeld;

	// Token: 0x0400707B RID: 28795
	public Light otherLight;

	// Token: 0x0400707C RID: 28796
	public LightStateType lightStateStart;

	// Token: 0x0400707D RID: 28797
	public string cyclingAudioClipPath;

	// Token: 0x0400707E RID: 28798
	public float cyclingAudioOffset;

	// Token: 0x0400707F RID: 28799
	public float cyclingAudioPlayEveryNCycles = 1f;

	// Token: 0x04007080 RID: 28800
	public bool EmissiveFromLightColorOn;

	// Token: 0x04007081 RID: 28801
	public Color EmissiveColor = Color.white;

	// Token: 0x04007082 RID: 28802
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color EmissiveColorOff = Color.black;

	// Token: 0x04007083 RID: 28803
	public float StateRate = 1f;

	// Token: 0x04007084 RID: 28804
	public float FluxDelay = 1f;

	// Token: 0x04007085 RID: 28805
	public static float DebugViewDistance;

	// Token: 0x04007086 RID: 28806
	public LightLOD.MaxIntensityEvent MaxIntensityChanged;

	// Token: 0x04007087 RID: 28807
	public bool WaterLevelDirty;

	// Token: 0x04007088 RID: 28808
	public float priority;

	// Token: 0x04007089 RID: 28809
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCulled;

	// Token: 0x0400708A RID: 28810
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform selfT;

	// Token: 0x0400708B RID: 28811
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform parentT;

	// Token: 0x0400708C RID: 28812
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

	// Token: 0x0400708D RID: 28813
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasInitialized;

	// Token: 0x0400708E RID: 28814
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockEntityData bed;

	// Token: 0x0400708F RID: 28815
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isInitialBlockDone;

	// Token: 0x04007090 RID: 28816
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light myLight;

	// Token: 0x04007091 RID: 28817
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightIntensityMaster;

	// Token: 0x04007092 RID: 28818
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightIntensity;

	// Token: 0x04007093 RID: 28819
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightRangeMaster;

	// Token: 0x04007094 RID: 28820
	[NonSerialized]
	public float lightRange;

	// Token: 0x04007095 RID: 28821
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightViewDistance;

	// Token: 0x04007096 RID: 28822
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bRenderingOff;

	// Token: 0x04007097 RID: 28823
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LensFlare lensFlare;

	// Token: 0x04007098 RID: 28824
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] maxLightPowerValues;

	// Token: 0x04007099 RID: 28825
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 registeredPos;

	// Token: 0x0400709A RID: 28826
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float registeredRange;

	// Token: 0x0400709B RID: 28827
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightStateType lightStateType;

	// Token: 0x0400709C RID: 28828
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightState lightState;

	// Token: 0x0400709D RID: 28829
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightShadows shadowStateMaster;

	// Token: 0x0400709E RID: 28830
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float shadowStrengthMaster;

	// Token: 0x0400709F RID: 28831
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioPlayer audioPlayer;

	// Token: 0x040070A0 RID: 28832
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Handle cyclingAudioHandle;

	// Token: 0x040070A1 RID: 28833
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isUnderwater;

	// Token: 0x040070A2 RID: 28834
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float waterLevelRecheckTime;

	// Token: 0x040070A3 RID: 28835
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color? lastEmissiveColor;

	// Token: 0x040070A4 RID: 28836
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float previousCycleTime;

	// Token: 0x0200127A RID: 4730
	// (Invoke) Token: 0x0600961D RID: 38429
	public delegate void MaxIntensityEvent();
}

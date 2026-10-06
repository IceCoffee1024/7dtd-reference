using System;
using Audio;
using UnityEngine;

// Token: 0x02001267 RID: 4711
public class LightLOD : MonoBehaviour
{
	// Token: 0x060095AB RID: 38315 RVA: 0x003887F0 File Offset: 0x003869F0
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

	// Token: 0x060095AC RID: 38316 RVA: 0x00388866 File Offset: 0x00386A66
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.Init();
	}

	// Token: 0x060095AD RID: 38317 RVA: 0x00388870 File Offset: 0x00386A70
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

	// Token: 0x060095AE RID: 38318 RVA: 0x00388A5C File Offset: 0x00386C5C
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

	// Token: 0x060095AF RID: 38319 RVA: 0x00388AF4 File Offset: 0x00386CF4
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

	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x060095B0 RID: 38320 RVA: 0x00388C0C File Offset: 0x00386E0C
	// (set) Token: 0x060095B1 RID: 38321 RVA: 0x00388C14 File Offset: 0x00386E14
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

	// Token: 0x170011FE RID: 4606
	// (get) Token: 0x060095B3 RID: 38323 RVA: 0x00388CA2 File Offset: 0x00386EA2
	// (set) Token: 0x060095B2 RID: 38322 RVA: 0x00388C82 File Offset: 0x00386E82
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

	// Token: 0x170011FF RID: 4607
	// (set) Token: 0x060095B4 RID: 38324 RVA: 0x00388CAA File Offset: 0x00386EAA
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

	// Token: 0x060095B5 RID: 38325 RVA: 0x00388CD4 File Offset: 0x00386ED4
	public void SetEmissiveColor(bool _on)
	{
		this.SetEmissiveColor(_on ? 1f : 0f);
	}

	// Token: 0x060095B6 RID: 38326 RVA: 0x00388CEC File Offset: 0x00386EEC
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

	// Token: 0x060095B7 RID: 38327 RVA: 0x00388D64 File Offset: 0x00386F64
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

	// Token: 0x060095B8 RID: 38328 RVA: 0x00388E18 File Offset: 0x00387018
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

	// Token: 0x060095B9 RID: 38329 RVA: 0x00388E90 File Offset: 0x00387090
	public void TestRegistration()
	{
		if (this.myLight)
		{
			this.SetRange(this.lightRange);
		}
	}

	// Token: 0x060095BA RID: 38330 RVA: 0x00388EAB File Offset: 0x003870AB
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcViewDistance()
	{
		this.lightViewDistance = Utils.FastMax(this.MaxDistance, this.lightRangeMaster * 1.5f);
	}

	// Token: 0x060095BB RID: 38331 RVA: 0x00388ECA File Offset: 0x003870CA
	public void SetBlockEntityData(BlockEntityData _bed)
	{
		this.bed = _bed;
	}

	// Token: 0x060095BC RID: 38332 RVA: 0x00388ED3 File Offset: 0x003870D3
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

	// Token: 0x060095BD RID: 38333 RVA: 0x00388EFF File Offset: 0x003870FF
	public void SetCulled(bool _culled)
	{
		this.isCulled = _culled;
		if (this.myLight)
		{
			this.myLight.enabled = !_culled;
		}
	}

	// Token: 0x060095BE RID: 38334 RVA: 0x00388F24 File Offset: 0x00387124
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

	// Token: 0x060095BF RID: 38335 RVA: 0x00388F9B File Offset: 0x0038719B
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnregisterFromLightManager()
	{
		if (this.registeredRange > 0f)
		{
			LightManager.UnRegisterLight(this.registeredPos, this.registeredRange);
			this.registeredRange = 0f;
		}
	}

	// Token: 0x060095C0 RID: 38336 RVA: 0x00388FC8 File Offset: 0x003871C8
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

	// Token: 0x060095C1 RID: 38337 RVA: 0x0038955C File Offset: 0x0038775C
	public void SetRenderingOn()
	{
		this.bRenderingOff = false;
	}

	// Token: 0x060095C2 RID: 38338 RVA: 0x00389565 File Offset: 0x00387765
	public void SetRenderingOff()
	{
		this.bRenderingOff = true;
	}

	// Token: 0x0400702E RID: 28718
	public GameObject LitRootObject;

	// Token: 0x0400702F RID: 28719
	public Transform[] RefIlluminatedMaterials;

	// Token: 0x04007030 RID: 28720
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material[][] cachedMaterials;

	// Token: 0x04007031 RID: 28721
	public Transform RefFlare;

	// Token: 0x04007032 RID: 28722
	public float MaxDistance = 30f;

	// Token: 0x04007033 RID: 28723
	public float DistanceScale = 1f;

	// Token: 0x04007034 RID: 28724
	public float FlareBrightnessFactor = 1f;

	// Token: 0x04007035 RID: 28725
	public bool bPlayerPlacedLight;

	// Token: 0x04007036 RID: 28726
	public bool bSwitchedOn;

	// Token: 0x04007037 RID: 28727
	public bool bToggleable = true;

	// Token: 0x04007038 RID: 28728
	public bool bWorksUnderwater = true;

	// Token: 0x04007039 RID: 28729
	public bool isHeld;

	// Token: 0x0400703A RID: 28730
	public Light otherLight;

	// Token: 0x0400703B RID: 28731
	public LightStateType lightStateStart;

	// Token: 0x0400703C RID: 28732
	public string cyclingAudioClipPath;

	// Token: 0x0400703D RID: 28733
	public float cyclingAudioOffset;

	// Token: 0x0400703E RID: 28734
	public float cyclingAudioPlayEveryNCycles = 1f;

	// Token: 0x0400703F RID: 28735
	public bool EmissiveFromLightColorOn;

	// Token: 0x04007040 RID: 28736
	public Color EmissiveColor = Color.white;

	// Token: 0x04007041 RID: 28737
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Color EmissiveColorOff = Color.black;

	// Token: 0x04007042 RID: 28738
	public float StateRate = 1f;

	// Token: 0x04007043 RID: 28739
	public float FluxDelay = 1f;

	// Token: 0x04007044 RID: 28740
	public static float DebugViewDistance;

	// Token: 0x04007045 RID: 28741
	public LightLOD.MaxIntensityEvent MaxIntensityChanged;

	// Token: 0x04007046 RID: 28742
	public bool WaterLevelDirty;

	// Token: 0x04007047 RID: 28743
	public float priority;

	// Token: 0x04007048 RID: 28744
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCulled;

	// Token: 0x04007049 RID: 28745
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform selfT;

	// Token: 0x0400704A RID: 28746
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform parentT;

	// Token: 0x0400704B RID: 28747
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

	// Token: 0x0400704C RID: 28748
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasInitialized;

	// Token: 0x0400704D RID: 28749
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockEntityData bed;

	// Token: 0x0400704E RID: 28750
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isInitialBlockDone;

	// Token: 0x0400704F RID: 28751
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light myLight;

	// Token: 0x04007050 RID: 28752
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightIntensityMaster;

	// Token: 0x04007051 RID: 28753
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightIntensity;

	// Token: 0x04007052 RID: 28754
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightRangeMaster;

	// Token: 0x04007053 RID: 28755
	[NonSerialized]
	public float lightRange;

	// Token: 0x04007054 RID: 28756
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lightViewDistance;

	// Token: 0x04007055 RID: 28757
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bRenderingOff;

	// Token: 0x04007056 RID: 28758
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LensFlare lensFlare;

	// Token: 0x04007057 RID: 28759
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] maxLightPowerValues;

	// Token: 0x04007058 RID: 28760
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 registeredPos;

	// Token: 0x04007059 RID: 28761
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float registeredRange;

	// Token: 0x0400705A RID: 28762
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightStateType lightStateType;

	// Token: 0x0400705B RID: 28763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightState lightState;

	// Token: 0x0400705C RID: 28764
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LightShadows shadowStateMaster;

	// Token: 0x0400705D RID: 28765
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float shadowStrengthMaster;

	// Token: 0x0400705E RID: 28766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioPlayer audioPlayer;

	// Token: 0x0400705F RID: 28767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Handle cyclingAudioHandle;

	// Token: 0x04007060 RID: 28768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isUnderwater;

	// Token: 0x04007061 RID: 28769
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float waterLevelRecheckTime;

	// Token: 0x04007062 RID: 28770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color? lastEmissiveColor;

	// Token: 0x04007063 RID: 28771
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float previousCycleTime;

	// Token: 0x02001268 RID: 4712
	// (Invoke) Token: 0x060095C6 RID: 38342
	public delegate void MaxIntensityEvent();
}

using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000C96 RID: 3222
public class Vehicle
{
	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x060062C1 RID: 25281 RVA: 0x0026D10C File Offset: 0x0026B30C
	public Vector2 CameraDistance
	{
		get
		{
			return this.cameraDistance;
		}
	}

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x060062C2 RID: 25282 RVA: 0x0026D114 File Offset: 0x0026B314
	public Vector2 CameraTurnRate
	{
		get
		{
			return this.cameraTurnRate;
		}
	}

	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x060062C3 RID: 25283 RVA: 0x0026D11C File Offset: 0x0026B31C
	public float BrakeTorque
	{
		get
		{
			return this.brakeTorque;
		}
	}

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x060062C4 RID: 25284 RVA: 0x0026D124 File Offset: 0x0026B324
	public float SteerAngleMax
	{
		get
		{
			return this.steerAngleMax;
		}
	}

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x060062C5 RID: 25285 RVA: 0x0026D12C File Offset: 0x0026B32C
	public float SteerRate
	{
		get
		{
			return this.steerRate;
		}
	}

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x060062C6 RID: 25286 RVA: 0x0026D134 File Offset: 0x0026B334
	public float SteerCenteringRate
	{
		get
		{
			return this.steerCenteringRate;
		}
	}

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x060062C7 RID: 25287 RVA: 0x0026D13C File Offset: 0x0026B33C
	public float TiltAngleMax
	{
		get
		{
			return this.tiltAngleMax;
		}
	}

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x060062C8 RID: 25288 RVA: 0x0026D144 File Offset: 0x0026B344
	public float TiltThreshold
	{
		get
		{
			return this.tiltThreshold;
		}
	}

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x060062C9 RID: 25289 RVA: 0x0026D14C File Offset: 0x0026B34C
	public float TiltDampening
	{
		get
		{
			return this.tiltDampening;
		}
	}

	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x060062CA RID: 25290 RVA: 0x0026D154 File Offset: 0x0026B354
	public float TiltDampenThreshold
	{
		get
		{
			return this.tiltDampenThreshold;
		}
	}

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x060062CB RID: 25291 RVA: 0x0026D15C File Offset: 0x0026B35C
	public float TiltUpForce
	{
		get
		{
			return this.tiltUpForce;
		}
	}

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x060062CC RID: 25292 RVA: 0x0026D164 File Offset: 0x0026B364
	public Vector2 HopForce
	{
		get
		{
			return this.hopForce;
		}
	}

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x060062CD RID: 25293 RVA: 0x0026D16C File Offset: 0x0026B36C
	public float UpAngleMax
	{
		get
		{
			return this.upAngleMax;
		}
	}

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x060062CE RID: 25294 RVA: 0x0026D174 File Offset: 0x0026B374
	public float UpForce
	{
		get
		{
			return this.upForce;
		}
	}

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x060062CF RID: 25295 RVA: 0x0026D17C File Offset: 0x0026B37C
	public float UnstickForce
	{
		get
		{
			return this.unstickForce;
		}
	}

	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x060062D0 RID: 25296 RVA: 0x0026D184 File Offset: 0x0026B384
	public float MaxPossibleSpeed
	{
		get
		{
			return this.VelocityMaxTurboForward;
		}
	}

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x060062D1 RID: 25297 RVA: 0x0026D18C File Offset: 0x0026B38C
	// (set) Token: 0x060062D2 RID: 25298 RVA: 0x0026D194 File Offset: 0x0026B394
	public PlatformUserIdentifierAbs OwnerId
	{
		get
		{
			return this.m_ownerId;
		}
		set
		{
			this.m_ownerId = value;
			int num = -1;
			PersistentPlayerData playerData = GameManager.Instance.persistentPlayers.GetPlayerData(this.m_ownerId);
			if (playerData != null)
			{
				num = playerData.EntityId;
			}
			this.entity.belongsPlayerId = num;
		}
	}

	// Token: 0x060062D3 RID: 25299 RVA: 0x0026D1D8 File Offset: 0x0026B3D8
	public Vehicle(string _vehicleName, EntityVehicle _entity)
	{
		this.vehicleName = _vehicleName.ToLower();
		this.entity = _entity;
		this.SetupProperties();
		this.meshT = this.entity.ModelTransform.Find("Mesh");
		if (!this.meshT)
		{
			this.meshT = this.entity.ModelTransform;
		}
		this.vehicleParts = new List<VehiclePart>();
		this.OwnerId = null;
		this.AllowedUsers = new List<PlatformUserIdentifierAbs>();
		this.PasswordHash = string.Empty;
		this.MakeItemValue();
		this.CreateParts();
	}

	// Token: 0x060062D4 RID: 25300 RVA: 0x0026D3AC File Offset: 0x0026B5AC
	public void MakeItemValue()
	{
		string name = this.GetName();
		int num = 0;
		ItemClass itemClass = ItemClass.GetItemClass(name + "Placeable", true);
		if (itemClass != null)
		{
			num = itemClass.Id;
		}
		this.itemValue = new ItemValue(num, 1, 6, false, null, 1f);
		this.SetItemValue(this.itemValue);
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x0026D400 File Offset: 0x0026B600
	public void SetItemValue(ItemValue _itemValue)
	{
		this.itemValue = _itemValue;
		if (this.itemValue.CosmeticMods.Length == 0)
		{
			this.itemValue.CosmeticMods = new ItemValue[1];
		}
		int num = this.itemValue.MaxUseTimes;
		if (this.itemValue.type == 0)
		{
			num = 5555;
		}
		int num2 = num - (int)this.itemValue.UseTimes;
		this.entity.Stats.Health.BaseMax = (float)num;
		this.entity.Stats.Health.OriginalMax = (float)num;
		this.entity.Health = num2;
		this.CalcEffects();
		this.SetFuelLevel((float)this.itemValue.Meta / 50f);
		this.CalcMods();
		this.SetColors();
		this.SetSeats();
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x0026D4CC File Offset: 0x0026B6CC
	public void SetItemValueMods(ItemValue _itemValue)
	{
		ItemValue itemValue = _itemValue.Clone();
		this.itemValue.Modifications = itemValue.Modifications;
		this.itemValue.CosmeticMods = itemValue.CosmeticMods;
		this.CalcEffects();
		this.CalcMods();
		this.SetColors();
		this.SetSeats();
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x0026D51C File Offset: 0x0026B71C
	public void SetColors()
	{
		Color white = Color.white;
		Vector3 vector = Block.StringToVector3(this.itemValue.GetPropertyOverride(Block.PropTintColor, "255,255,255"));
		white.r = vector.x;
		white.g = vector.y;
		white.b = vector.z;
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].SetColors(white);
		}
	}

	// Token: 0x060062D8 RID: 25304 RVA: 0x0026D59C File Offset: 0x0026B79C
	public void SetSeats()
	{
		int num = (int)EffectManager.GetValue(PassiveEffects.VehicleSeats, this.itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		int num2 = 0;
		if (this.Properties != null)
		{
			int num3 = 0;
			int num4 = 0;
			DynamicProperties dynamicProperties;
			while (num4 < 99 && this.Properties.Classes.TryGetValue("seat" + num4.ToString(), out dynamicProperties))
			{
				if (dynamicProperties.GetString("mod").Length > 0)
				{
					if (num3 >= num)
					{
						break;
					}
					num3++;
				}
				num2++;
				num4++;
			}
		}
		this.entity.SetAttachMaxCount(num2);
	}

	// Token: 0x060062D9 RID: 25305 RVA: 0x0026D640 File Offset: 0x0026B840
	public int GetSeatPose(int _seatIndex)
	{
		DynamicProperties dynamicProperties;
		if (this.Properties != null && this.Properties.Classes.TryGetValue("seat" + _seatIndex.ToString(), out dynamicProperties))
		{
			string @string = dynamicProperties.GetString("pose");
			if (@string.Length > 0)
			{
				return int.Parse(@string);
			}
		}
		return 0;
	}

	// Token: 0x060062DA RID: 25306 RVA: 0x0026D698 File Offset: 0x0026B898
	public ItemValue GetUpdatedItemValue()
	{
		this.itemValue.UseTimes = (float)((int)this.entity.Stats.Health.BaseMax - this.entity.Health);
		this.itemValue.Meta = (int)(this.GetFuelLevel() * 50f);
		return this.itemValue;
	}

	// Token: 0x060062DB RID: 25307 RVA: 0x0026D6F1 File Offset: 0x0026B8F1
	public void LoadItems(ItemStack[] _items)
	{
		this.SetItemValue(_items[0].itemValue);
	}

	// Token: 0x060062DC RID: 25308 RVA: 0x0026D701 File Offset: 0x0026B901
	public ItemStack[] GetItems()
	{
		return new ItemStack[]
		{
			new ItemStack(this.GetUpdatedItemValue(), 1)
		};
	}

	// Token: 0x060062DD RID: 25309 RVA: 0x0026D718 File Offset: 0x0026B918
	public void Update(float _deltaTime)
	{
		this.UpdateEffects(_deltaTime);
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].Update(_deltaTime);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !this.HasStorage())
		{
			this.entity.DropBagServer();
		}
	}

	// Token: 0x060062DE RID: 25310 RVA: 0x0026D773 File Offset: 0x0026B973
	public void UpdateSimulation()
	{
		this.FireEvent(Vehicle.Event.SimulationUpdate);
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x0026D77C File Offset: 0x0026B97C
	public void FireEvent(Vehicle.Event _event)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, 0f);
		}
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x0026D7B8 File Offset: 0x0026B9B8
	public void FireEvent(VehiclePart.Event _event, VehiclePart _fromPart, float _arg)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, _fromPart, _arg);
		}
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x0026D7EF File Offset: 0x0026B9EF
	public void SetupProperties()
	{
		if (!Vehicle.PropertyMap.TryGetValue(this.vehicleName, out this.Properties))
		{
			Log.Error("Vehicle properties for '{0}' not found!", new object[] { this.vehicleName });
		}
	}

	// Token: 0x060062E2 RID: 25314 RVA: 0x0026D824 File Offset: 0x0026BA24
	public DynamicProperties GetPropertiesForClass(string className)
	{
		if (this.Properties == null)
		{
			return null;
		}
		DynamicProperties dynamicProperties;
		this.Properties.Classes.TryGetValue(className, out dynamicProperties);
		return dynamicProperties;
	}

	// Token: 0x060062E3 RID: 25315 RVA: 0x0026D850 File Offset: 0x0026BA50
	[PublicizedFrom(EAccessModifier.Private)]
	public void ParseGeneralProperties(DynamicProperties properties)
	{
		properties.ParseVec("cameraDistance", ref this.cameraDistance);
		properties.ParseVec("cameraTurnRate", ref this.cameraTurnRate);
		properties.ParseFloat("steerAngleMax", ref this.steerAngleMax);
		properties.ParseFloat("steerRate", ref this.steerRate);
		properties.ParseFloat("steerCenteringRate", ref this.steerCenteringRate);
		properties.ParseFloat("tiltAngleMax", ref this.tiltAngleMax);
		properties.ParseFloat("tiltThreshold", ref this.tiltThreshold);
		properties.ParseFloat("tiltDampening", ref this.tiltDampening);
		properties.ParseFloat("tiltDampenThreshold", ref this.tiltDampenThreshold);
		properties.ParseFloat("tiltUpForce", ref this.tiltUpForce);
		properties.ParseFloat("upAngleMax", ref this.upAngleMax);
		properties.ParseFloat("upForce", ref this.upForce);
		properties.ParseVec("motorTorque_turbo", ref this.MotorTorqueForward, ref this.MotorTorqueBackward, ref this.MotorTorqueTurboForward, ref this.MotorTorqueTurboBackward);
		properties.ParseVec("velocityMax_turbo", ref this.VelocityMaxForward, ref this.VelocityMaxBackward, ref this.VelocityMaxTurboForward, ref this.VelocityMaxTurboBackward);
		properties.ParseFloat("brakeTorque", ref this.brakeTorque);
		properties.ParseVec("hopForce", ref this.hopForce);
		properties.ParseFloat("unstickForce", ref this.unstickForce);
		properties.ParseVec("airDrag_velScale_angVelScale", ref this.AirDragVelScale, ref this.AirDragAngVelScale);
		properties.ParseVec("waterDrag_y_velScale_velMaxScale", ref this.WaterDragY, ref this.WaterDragVelScale, ref this.WaterDragVelMaxScale);
		properties.ParseVec("waterLift_y_depth_force", ref this.WaterLiftY, ref this.WaterLiftDepth, ref this.WaterLiftForce);
		properties.ParseFloat("wheelPtlScale", ref this.WheelPtlScale);
		properties.ParseString("recipeName", ref this.RecipeName);
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x0026DA18 File Offset: 0x0026BC18
	public void OnXMLChanged()
	{
		this.SetupProperties();
		DynamicProperties properties = this.Properties;
		if (properties == null)
		{
			return;
		}
		this.ParseGeneralProperties(properties);
		foreach (KeyValuePair<string, DynamicProperties> keyValuePair in properties.Classes)
		{
			VehiclePart vehiclePart = this.FindPart(keyValuePair.Key);
			if (vehiclePart != null)
			{
				DynamicProperties value = keyValuePair.Value;
				vehiclePart.SetProperties(value);
			}
		}
	}

	// Token: 0x060062E5 RID: 25317 RVA: 0x0026DAA0 File Offset: 0x0026BCA0
	public void TriggerUpdateEffects()
	{
		this.effectUpdateDelay = 0f;
	}

	// Token: 0x060062E6 RID: 25318 RVA: 0x0026DAAD File Offset: 0x0026BCAD
	public void UpdateEffects(float _deltaTime)
	{
		this.effectUpdateDelay -= _deltaTime;
		if (this.effectUpdateDelay > 0f)
		{
			return;
		}
		this.effectUpdateDelay = 2f;
		this.GetUpdatedItemValue();
		this.CalcEffects();
	}

	// Token: 0x060062E7 RID: 25319 RVA: 0x0026DAE4 File Offset: 0x0026BCE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcEffects()
	{
		EntityAlive entityAlive = this.entity.AttachedMainEntity as EntityAlive;
		FastTags<TagGroup.Global> entityTags = this.entity.EntityTags;
		this.EffectEntityDamagePer = EffectManager.GetValue(PassiveEffects.VehicleEntityDamage, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false) * EntityVehicle.VehicleEntityDamageModifier;
		this.EffectBlockDamagePer = EffectManager.GetValue(PassiveEffects.VehicleBlockDamage, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false) * EntityVehicle.VehicleBlockDamageModifier;
		this.EffectSelfDamagePer = EffectManager.GetValue(PassiveEffects.VehicleSelfDamage, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false) * EntityVehicle.VehicleSelfDamageModifier;
		this.EffectStrongSelfDamagePer = EffectManager.GetValue(PassiveEffects.VehicleStrongSelfDamage, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false) * EntityVehicle.VehicleSelfDamageModifier;
		this.EffectLightIntensity = EffectManager.GetValue(PassiveEffects.LightIntensity, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false);
		this.EffectFuelMaxPer = EffectManager.GetValue(PassiveEffects.VehicleFuelMaxPer, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false);
		this.EffectFuelUsePer = EffectManager.GetValue(PassiveEffects.VehicleFuelUsePer, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false) * EntityVehicle.VehicleFuelUsageModifier;
		this.EffectMotorTorquePer = EffectManager.GetValue(PassiveEffects.VehicleMotorTorquePer, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false);
		this.EffectVelocityMaxPer = EffectManager.GetValue(PassiveEffects.VehicleVelocityMaxPer, this.itemValue, 1f, entityAlive, null, entityTags, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x060062E8 RID: 25320 RVA: 0x0026DC68 File Offset: 0x0026BE68
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcMods()
	{
		int num = 0;
		this.ModTags = FastTags<TagGroup.Global>.none;
		ItemValue[] modifications = this.itemValue.Modifications;
		if (modifications != null)
		{
			foreach (ItemValue itemValue in modifications)
			{
				if (itemValue != null)
				{
					ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
					if (itemClassModifier != null)
					{
						this.ModTags |= itemClassModifier.ModifierTags;
						if (itemClassModifier.ModifierTags.Test_AnySet(EntityVehicle.StorageModifierTags))
						{
							num++;
						}
					}
				}
			}
		}
		for (int j = 0; j < this.vehicleParts.Count; j++)
		{
			this.vehicleParts[j].SetMods();
		}
		this.entity.UpdateStorageModCount(num);
		this.entity.UpdateContainerSize(false);
	}

	// Token: 0x060062E9 RID: 25321 RVA: 0x0026DD2C File Offset: 0x0026BF2C
	public void CreateParts()
	{
		DynamicProperties properties = this.Properties;
		if (properties == null)
		{
			return;
		}
		this.ParseGeneralProperties(properties);
		foreach (KeyValuePair<string, DynamicProperties> keyValuePair in properties.Classes)
		{
			DynamicProperties value = keyValuePair.Value;
			string @string = value.GetString("class");
			if (@string.Length > 0)
			{
				try
				{
					VehiclePart vehiclePart = (VehiclePart)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("VP", @string));
					vehiclePart.SetVehicle(this);
					vehiclePart.SetTag(keyValuePair.Key);
					vehiclePart.SetProperties(value);
					this.vehicleParts.Add(vehiclePart);
				}
				catch (Exception ex)
				{
					Log.Out(ex.Message);
					Log.Out(ex.StackTrace);
					throw new Exception("No vehicle part class 'VP" + @string + "' found!");
				}
			}
		}
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].InitPrefabConnections();
		}
	}

	// Token: 0x060062EA RID: 25322 RVA: 0x0026DE58 File Offset: 0x0026C058
	public VehiclePart FindPart(string _tag)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			if (this.vehicleParts[i].tag == _tag)
			{
				return this.vehicleParts[i];
			}
		}
		return null;
	}

	// Token: 0x060062EB RID: 25323 RVA: 0x0026DEA4 File Offset: 0x0026C0A4
	public string GetPartProperty(string _tag, string _propertyName)
	{
		VehiclePart vehiclePart = this.FindPart(_tag);
		if (vehiclePart == null)
		{
			return string.Empty;
		}
		return vehiclePart.GetProperty(_propertyName);
	}

	// Token: 0x060062EC RID: 25324 RVA: 0x0026DEC9 File Offset: 0x0026C0C9
	public List<VehiclePart> GetParts()
	{
		return this.vehicleParts;
	}

	// Token: 0x060062ED RID: 25325 RVA: 0x0026DED4 File Offset: 0x0026C0D4
	public static void SetupPreview(Transform rootT)
	{
		Transform transform = rootT.Find("Physics");
		if (transform)
		{
			global::UnityEngine.Object.Destroy(transform.gameObject);
		}
		ParticleSystem[] componentsInChildren = rootT.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x060062EE RID: 25326 RVA: 0x0026DF23 File Offset: 0x0026C123
	public Transform GetMeshTransform()
	{
		return this.meshT;
	}

	// Token: 0x060062EF RID: 25327 RVA: 0x0026DF2B File Offset: 0x0026C12B
	public string GetName()
	{
		return this.vehicleName;
	}

	// Token: 0x060062F0 RID: 25328 RVA: 0x0026DF33 File Offset: 0x0026C133
	public static void Cleanup()
	{
		Vehicle.PropertyMap = new Dictionary<string, DynamicProperties>();
	}

	// Token: 0x060062F1 RID: 25329 RVA: 0x0026DF3F File Offset: 0x0026C13F
	public string GetFuelItem()
	{
		if (this.HasEnginePart())
		{
			return "ammoGasCan";
		}
		return "";
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x0026DF54 File Offset: 0x0026C154
	public float GetFuelPercent()
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			if (this.vehicleParts[i].GetType() == typeof(VPFuelTank))
			{
				float num = ((VPFuelTank)this.vehicleParts[i]).GetFuelLevelPercent();
				if (num > 0.993f)
				{
					num = 1f;
				}
				return num;
			}
		}
		return 0f;
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x0026DFC8 File Offset: 0x0026C1C8
	public float GetFuelLevel()
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			if (this.vehicleParts[i].GetType() == typeof(VPFuelTank))
			{
				return ((VPFuelTank)this.vehicleParts[i]).GetFuelLevel();
			}
		}
		return 0f;
	}

	// Token: 0x060062F4 RID: 25332 RVA: 0x0026E02C File Offset: 0x0026C22C
	public float GetMaxFuelLevel()
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			if (this.vehicleParts[i].GetType() == typeof(VPFuelTank))
			{
				return ((VPFuelTank)this.vehicleParts[i]).GetMaxFuelLevel();
			}
		}
		return 0f;
	}

	// Token: 0x060062F5 RID: 25333 RVA: 0x0026E090 File Offset: 0x0026C290
	public void SetFuelLevel(float _fuelLevel)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			if (this.vehicleParts[i].GetType() == typeof(VPFuelTank))
			{
				((VPFuelTank)this.vehicleParts[i]).SetFuelLevel(_fuelLevel);
				return;
			}
		}
	}

	// Token: 0x060062F6 RID: 25334 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetBatteryLevel()
	{
		return 0f;
	}

	// Token: 0x060062F7 RID: 25335 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBatteryLevel(float _amount)
	{
	}

	// Token: 0x060062F8 RID: 25336 RVA: 0x0026E0F0 File Offset: 0x0026C2F0
	public void AddFuel(float _fuelLevel)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			if (this.vehicleParts[i].GetType() == typeof(VPFuelTank))
			{
				((VPFuelTank)this.vehicleParts[i]).AddFuel(_fuelLevel);
				return;
			}
		}
	}

	// Token: 0x060062F9 RID: 25337 RVA: 0x0026E14D File Offset: 0x0026C34D
	public int GetRepairAmountNeeded()
	{
		return this.GetMaxHealth() - this.entity.Health;
	}

	// Token: 0x060062FA RID: 25338 RVA: 0x0026E164 File Offset: 0x0026C364
	public void RepairParts(int _add, float _percent)
	{
		int num = _add + (int)((float)this.GetMaxHealth() * _percent);
		num = Utils.FastMin(num, this.GetRepairAmountNeeded());
		this.entity.Health += num;
	}

	// Token: 0x060062FB RID: 25339 RVA: 0x0026E19E File Offset: 0x0026C39E
	public bool IsDriveable()
	{
		return this.HasSteering();
	}

	// Token: 0x060062FC RID: 25340 RVA: 0x0026E1A6 File Offset: 0x0026C3A6
	public bool HasEnginePart()
	{
		return this.FindPart("engine") != null;
	}

	// Token: 0x060062FD RID: 25341 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetEngineQualityPercent()
	{
		return 0f;
	}

	// Token: 0x060062FE RID: 25342 RVA: 0x0026E1B6 File Offset: 0x0026C3B6
	public bool HasStorage()
	{
		return this.FindPart("storage") != null;
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool HasSteering()
	{
		return true;
	}

	// Token: 0x06006300 RID: 25344 RVA: 0x0026E1C6 File Offset: 0x0026C3C6
	public bool IsSteeringBroken()
	{
		return !this.HasSteering() || this.FindPart("handlebars").IsBroken();
	}

	// Token: 0x06006301 RID: 25345 RVA: 0x0026E1E2 File Offset: 0x0026C3E2
	public bool HasLock()
	{
		return this.FindPart("lock") != null;
	}

	// Token: 0x06006302 RID: 25346 RVA: 0x0026E1F2 File Offset: 0x0026C3F2
	public bool IsLockBroken()
	{
		return this.FindPart("lock").GetHealthPercentage() == 0f;
	}

	// Token: 0x06006303 RID: 25347 RVA: 0x0026E20B File Offset: 0x0026C40B
	public string GetHornSoundName()
	{
		return this.Properties.GetString("hornSound");
	}

	// Token: 0x06006304 RID: 25348 RVA: 0x0026E21D File Offset: 0x0026C41D
	public bool HasHorn()
	{
		return this.GetHornSoundName().Length > 0;
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x0026E230 File Offset: 0x0026C430
	public List<IKController.Target> GetIKTargets(int slot)
	{
		List<IKController.Target> list = new List<IKController.Target>();
		if (slot == 0)
		{
			VehiclePart vehiclePart = this.FindPart("handlebars");
			if (vehiclePart != null)
			{
				list.AddRange(vehiclePart.ikTargets);
			}
			VehiclePart vehiclePart2 = this.FindPart("pedals");
			if (vehiclePart2 != null)
			{
				list.AddRange(vehiclePart2.ikTargets);
			}
		}
		VehiclePart vehiclePart3 = this.FindPart("seat" + slot.ToString());
		if (vehiclePart3 != null && vehiclePart3.ikTargets != null)
		{
			list.AddRange(vehiclePart3.ikTargets);
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list;
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x0026E2B8 File Offset: 0x0026C4B8
	public List<string> GetParticleTransformPaths()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			string property = this.vehicleParts[i].GetProperty("particle_transform");
			if (property != string.Empty)
			{
				list.Add(property);
			}
		}
		return list;
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x0026E30D File Offset: 0x0026C50D
	public int GetVehicleQuality()
	{
		return (int)this.itemValue.Quality;
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x0026E31C File Offset: 0x0026C51C
	public int GetHealth()
	{
		int health = this.entity.Health;
		if (health <= 1)
		{
			return 0;
		}
		return health;
	}

	// Token: 0x06006309 RID: 25353 RVA: 0x0026E33C File Offset: 0x0026C53C
	public int GetMaxHealth()
	{
		return this.entity.GetMaxHealth();
	}

	// Token: 0x0600630A RID: 25354 RVA: 0x0026E349 File Offset: 0x0026C549
	public float GetHealthPercent()
	{
		return (float)this.GetHealth() / (float)this.entity.GetMaxHealth();
	}

	// Token: 0x0600630B RID: 25355 RVA: 0x0026E35F File Offset: 0x0026C55F
	public float GetPlayerDamagePercent()
	{
		return 0.1f;
	}

	// Token: 0x0600630C RID: 25356 RVA: 0x0014A743 File Offset: 0x00148943
	public float GetNoise()
	{
		return 0.5f;
	}

	// Token: 0x0600630D RID: 25357 RVA: 0x0026E368 File Offset: 0x0026C568
	public void SetLocked(bool isLocked, EntityPlayerLocal player)
	{
		if (player == null)
		{
			return;
		}
		if (isLocked)
		{
			this.entity.SetOwner(PlatformManager.InternalLocalUserIdentifier);
			this.entity.isLocked = true;
			this.PasswordHash = string.Empty;
			return;
		}
		this.entity.isLocked = false;
		this.PasswordHash = string.Empty;
	}

	// Token: 0x04004C9D RID: 19613
	public static Dictionary<string, DynamicProperties> PropertyMap;

	// Token: 0x04004C9E RID: 19614
	public DynamicProperties Properties;

	// Token: 0x04004C9F RID: 19615
	public ItemValue itemValue;

	// Token: 0x04004CA0 RID: 19616
	public List<PlatformUserIdentifierAbs> AllowedUsers;

	// Token: 0x04004CA1 RID: 19617
	public string PasswordHash = string.Empty;

	// Token: 0x04004CA2 RID: 19618
	public EntityVehicle entity;

	// Token: 0x04004CA3 RID: 19619
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_ownerId;

	// Token: 0x04004CA4 RID: 19620
	[PublicizedFrom(EAccessModifier.Private)]
	public string vehicleName;

	// Token: 0x04004CA5 RID: 19621
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VehiclePart> vehicleParts;

	// Token: 0x04004CA6 RID: 19622
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform meshT;

	// Token: 0x04004CA7 RID: 19623
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraDistance = new Vector2(1f, 1f);

	// Token: 0x04004CA8 RID: 19624
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraTurnRate = new Vector2(1f, 1f);

	// Token: 0x04004CA9 RID: 19625
	[PublicizedFrom(EAccessModifier.Private)]
	public float upAngleMax = 70f;

	// Token: 0x04004CAA RID: 19626
	[PublicizedFrom(EAccessModifier.Private)]
	public float upForce = 5f;

	// Token: 0x04004CAB RID: 19627
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerAngleMax = 25f;

	// Token: 0x04004CAC RID: 19628
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerRate = 130f;

	// Token: 0x04004CAD RID: 19629
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerCenteringRate = 90f;

	// Token: 0x04004CAE RID: 19630
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltAngleMax = 20f;

	// Token: 0x04004CAF RID: 19631
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltThreshold = 3f;

	// Token: 0x04004CB0 RID: 19632
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampening = 0.22f;

	// Token: 0x04004CB1 RID: 19633
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampenThreshold = 8f;

	// Token: 0x04004CB2 RID: 19634
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltUpForce = 5f;

	// Token: 0x04004CB3 RID: 19635
	public float MotorTorqueForward = 300f;

	// Token: 0x04004CB4 RID: 19636
	public float MotorTorqueBackward = 300f;

	// Token: 0x04004CB5 RID: 19637
	public float MotorTorqueTurboForward;

	// Token: 0x04004CB6 RID: 19638
	public float MotorTorqueTurboBackward;

	// Token: 0x04004CB7 RID: 19639
	public float VelocityMaxForward = 10f;

	// Token: 0x04004CB8 RID: 19640
	public float VelocityMaxBackward = 10f;

	// Token: 0x04004CB9 RID: 19641
	public float VelocityMaxTurboForward = 10f;

	// Token: 0x04004CBA RID: 19642
	public float VelocityMaxTurboBackward = 10f;

	// Token: 0x04004CBB RID: 19643
	[PublicizedFrom(EAccessModifier.Private)]
	public float brakeTorque = 4000f;

	// Token: 0x04004CBC RID: 19644
	public bool CanTurbo = true;

	// Token: 0x04004CBD RID: 19645
	public bool IsTurbo;

	// Token: 0x04004CBE RID: 19646
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 hopForce;

	// Token: 0x04004CBF RID: 19647
	[PublicizedFrom(EAccessModifier.Private)]
	public float unstickForce = 1f;

	// Token: 0x04004CC0 RID: 19648
	public float AirDragVelScale = 0.997f;

	// Token: 0x04004CC1 RID: 19649
	public float AirDragAngVelScale = 1f;

	// Token: 0x04004CC2 RID: 19650
	public float WaterDragY;

	// Token: 0x04004CC3 RID: 19651
	public float WaterDragVelScale = 1f;

	// Token: 0x04004CC4 RID: 19652
	public float WaterDragVelMaxScale = 1f;

	// Token: 0x04004CC5 RID: 19653
	public float WaterLiftY;

	// Token: 0x04004CC6 RID: 19654
	public float WaterLiftDepth;

	// Token: 0x04004CC7 RID: 19655
	public float WaterLiftForce;

	// Token: 0x04004CC8 RID: 19656
	public float WheelPtlScale;

	// Token: 0x04004CC9 RID: 19657
	public float CurrentForwardVelocity;

	// Token: 0x04004CCA RID: 19658
	public bool CurrentIsAccel;

	// Token: 0x04004CCB RID: 19659
	public bool CurrentIsBreak;

	// Token: 0x04004CCC RID: 19660
	public float CurrentMotorTorquePercent;

	// Token: 0x04004CCD RID: 19661
	public float CurrentSteeringPercent;

	// Token: 0x04004CCE RID: 19662
	public Vector3 CurrentVelocity;

	// Token: 0x04004CCF RID: 19663
	public string RecipeName;

	// Token: 0x04004CD0 RID: 19664
	public Material mainEmissiveMat;

	// Token: 0x04004CD1 RID: 19665
	public float EffectEntityDamagePer;

	// Token: 0x04004CD2 RID: 19666
	public float EffectBlockDamagePer;

	// Token: 0x04004CD3 RID: 19667
	public float EffectSelfDamagePer;

	// Token: 0x04004CD4 RID: 19668
	public float EffectStrongSelfDamagePer;

	// Token: 0x04004CD5 RID: 19669
	public float EffectLightIntensity;

	// Token: 0x04004CD6 RID: 19670
	public float EffectFuelMaxPer = 1f;

	// Token: 0x04004CD7 RID: 19671
	public float EffectFuelUsePer;

	// Token: 0x04004CD8 RID: 19672
	public float EffectMotorTorquePer;

	// Token: 0x04004CD9 RID: 19673
	public float EffectVelocityMaxPer;

	// Token: 0x04004CDA RID: 19674
	[PublicizedFrom(EAccessModifier.Private)]
	public float effectUpdateDelay;

	// Token: 0x04004CDB RID: 19675
	public FastTags<TagGroup.Global> ModTags;

	// Token: 0x02000C97 RID: 3223
	public enum Event
	{
		// Token: 0x04004CDD RID: 19677
		Start,
		// Token: 0x04004CDE RID: 19678
		Started,
		// Token: 0x04004CDF RID: 19679
		Stop,
		// Token: 0x04004CE0 RID: 19680
		Stopped,
		// Token: 0x04004CE1 RID: 19681
		SimulationUpdate,
		// Token: 0x04004CE2 RID: 19682
		HealthChanged
	}
}

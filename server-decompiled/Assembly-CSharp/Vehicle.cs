using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000CA8 RID: 3240
public class Vehicle
{
	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x0600634C RID: 25420 RVA: 0x0026FFE4 File Offset: 0x0026E1E4
	public Vector2 CameraDistance
	{
		get
		{
			return this.cameraDistance;
		}
	}

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x0600634D RID: 25421 RVA: 0x0026FFEC File Offset: 0x0026E1EC
	public Vector2 CameraTurnRate
	{
		get
		{
			return this.cameraTurnRate;
		}
	}

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x0600634E RID: 25422 RVA: 0x0026FFF4 File Offset: 0x0026E1F4
	public float BrakeTorque
	{
		get
		{
			return this.brakeTorque;
		}
	}

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x0600634F RID: 25423 RVA: 0x0026FFFC File Offset: 0x0026E1FC
	public float SteerAngleMax
	{
		get
		{
			return this.steerAngleMax;
		}
	}

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x06006350 RID: 25424 RVA: 0x00270004 File Offset: 0x0026E204
	public float SteerRate
	{
		get
		{
			return this.steerRate;
		}
	}

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x06006351 RID: 25425 RVA: 0x0027000C File Offset: 0x0026E20C
	public float SteerCenteringRate
	{
		get
		{
			return this.steerCenteringRate;
		}
	}

	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x06006352 RID: 25426 RVA: 0x00270014 File Offset: 0x0026E214
	public float TiltAngleMax
	{
		get
		{
			return this.tiltAngleMax;
		}
	}

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x06006353 RID: 25427 RVA: 0x0027001C File Offset: 0x0026E21C
	public float TiltThreshold
	{
		get
		{
			return this.tiltThreshold;
		}
	}

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x06006354 RID: 25428 RVA: 0x00270024 File Offset: 0x0026E224
	public float TiltDampening
	{
		get
		{
			return this.tiltDampening;
		}
	}

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x06006355 RID: 25429 RVA: 0x0027002C File Offset: 0x0026E22C
	public float TiltDampenThreshold
	{
		get
		{
			return this.tiltDampenThreshold;
		}
	}

	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x06006356 RID: 25430 RVA: 0x00270034 File Offset: 0x0026E234
	public float TiltUpForce
	{
		get
		{
			return this.tiltUpForce;
		}
	}

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x06006357 RID: 25431 RVA: 0x0027003C File Offset: 0x0026E23C
	public Vector2 HopForce
	{
		get
		{
			return this.hopForce;
		}
	}

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x06006358 RID: 25432 RVA: 0x00270044 File Offset: 0x0026E244
	public float UpAngleMax
	{
		get
		{
			return this.upAngleMax;
		}
	}

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x06006359 RID: 25433 RVA: 0x0027004C File Offset: 0x0026E24C
	public float UpForce
	{
		get
		{
			return this.upForce;
		}
	}

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x0600635A RID: 25434 RVA: 0x00270054 File Offset: 0x0026E254
	public float UnstickForce
	{
		get
		{
			return this.unstickForce;
		}
	}

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x0600635B RID: 25435 RVA: 0x0027005C File Offset: 0x0026E25C
	public float MaxPossibleSpeed
	{
		get
		{
			return this.VelocityMaxTurboForward;
		}
	}

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x0600635C RID: 25436 RVA: 0x00270064 File Offset: 0x0026E264
	// (set) Token: 0x0600635D RID: 25437 RVA: 0x0027006C File Offset: 0x0026E26C
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

	// Token: 0x0600635E RID: 25438 RVA: 0x002700B0 File Offset: 0x0026E2B0
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

	// Token: 0x0600635F RID: 25439 RVA: 0x00270290 File Offset: 0x0026E490
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

	// Token: 0x06006360 RID: 25440 RVA: 0x002702E4 File Offset: 0x0026E4E4
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

	// Token: 0x06006361 RID: 25441 RVA: 0x002703B0 File Offset: 0x0026E5B0
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

	// Token: 0x06006362 RID: 25442 RVA: 0x00270400 File Offset: 0x0026E600
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

	// Token: 0x06006363 RID: 25443 RVA: 0x00270480 File Offset: 0x0026E680
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

	// Token: 0x06006364 RID: 25444 RVA: 0x00270524 File Offset: 0x0026E724
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

	// Token: 0x06006365 RID: 25445 RVA: 0x0027057C File Offset: 0x0026E77C
	public ItemValue GetUpdatedItemValue()
	{
		this.itemValue.UseTimes = (float)((int)this.entity.Stats.Health.BaseMax - this.entity.Health);
		this.itemValue.Meta = (int)(this.GetFuelLevel() * 50f);
		return this.itemValue;
	}

	// Token: 0x06006366 RID: 25446 RVA: 0x002705D5 File Offset: 0x0026E7D5
	public void LoadItems(ItemStack[] _items)
	{
		this.SetItemValue(_items[0].itemValue);
	}

	// Token: 0x06006367 RID: 25447 RVA: 0x002705E5 File Offset: 0x0026E7E5
	public ItemStack[] GetItems()
	{
		return new ItemStack[]
		{
			new ItemStack(this.GetUpdatedItemValue(), 1)
		};
	}

	// Token: 0x06006368 RID: 25448 RVA: 0x002705FC File Offset: 0x0026E7FC
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

	// Token: 0x06006369 RID: 25449 RVA: 0x00270657 File Offset: 0x0026E857
	public void UpdateSimulation()
	{
		this.FireEvent(Vehicle.Event.SimulationUpdate);
	}

	// Token: 0x0600636A RID: 25450 RVA: 0x00270660 File Offset: 0x0026E860
	public void FireEvent(Vehicle.Event _event)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, 0f);
		}
	}

	// Token: 0x0600636B RID: 25451 RVA: 0x0027069C File Offset: 0x0026E89C
	public void FireEvent(VehiclePart.Event _event, VehiclePart _fromPart, float _arg)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, _fromPart, _arg);
		}
	}

	// Token: 0x0600636C RID: 25452 RVA: 0x002706D3 File Offset: 0x0026E8D3
	public void SetupProperties()
	{
		if (!Vehicle.PropertyMap.TryGetValue(this.vehicleName, out this.Properties))
		{
			Log.Error("Vehicle properties for '{0}' not found!", new object[] { this.vehicleName });
		}
	}

	// Token: 0x0600636D RID: 25453 RVA: 0x00270708 File Offset: 0x0026E908
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

	// Token: 0x0600636E RID: 25454 RVA: 0x00270734 File Offset: 0x0026E934
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
		properties.ParseString("honkEvent", ref this.onHonkEvent);
	}

	// Token: 0x0600636F RID: 25455 RVA: 0x0027090C File Offset: 0x0026EB0C
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

	// Token: 0x06006370 RID: 25456 RVA: 0x00270994 File Offset: 0x0026EB94
	public void TriggerUpdateEffects()
	{
		this.effectUpdateDelay = 0f;
	}

	// Token: 0x06006371 RID: 25457 RVA: 0x002709A1 File Offset: 0x0026EBA1
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

	// Token: 0x06006372 RID: 25458 RVA: 0x002709D8 File Offset: 0x0026EBD8
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

	// Token: 0x06006373 RID: 25459 RVA: 0x00270B5C File Offset: 0x0026ED5C
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

	// Token: 0x06006374 RID: 25460 RVA: 0x00270C20 File Offset: 0x0026EE20
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

	// Token: 0x06006375 RID: 25461 RVA: 0x00270D4C File Offset: 0x0026EF4C
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

	// Token: 0x06006376 RID: 25462 RVA: 0x00270D98 File Offset: 0x0026EF98
	public string GetPartProperty(string _tag, string _propertyName)
	{
		VehiclePart vehiclePart = this.FindPart(_tag);
		if (vehiclePart == null)
		{
			return string.Empty;
		}
		return vehiclePart.GetProperty(_propertyName);
	}

	// Token: 0x06006377 RID: 25463 RVA: 0x00270DBD File Offset: 0x0026EFBD
	public List<VehiclePart> GetParts()
	{
		return this.vehicleParts;
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x00270DC8 File Offset: 0x0026EFC8
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

	// Token: 0x06006379 RID: 25465 RVA: 0x00270E17 File Offset: 0x0026F017
	public Transform GetMeshTransform()
	{
		return this.meshT;
	}

	// Token: 0x0600637A RID: 25466 RVA: 0x00270E1F File Offset: 0x0026F01F
	public string GetName()
	{
		return this.vehicleName;
	}

	// Token: 0x0600637B RID: 25467 RVA: 0x00270E27 File Offset: 0x0026F027
	public static void Cleanup()
	{
		Vehicle.PropertyMap = new Dictionary<string, DynamicProperties>();
	}

	// Token: 0x0600637C RID: 25468 RVA: 0x00270E33 File Offset: 0x0026F033
	public string GetFuelItem()
	{
		if (this.HasEnginePart())
		{
			return "ammoGasCan";
		}
		return "";
	}

	// Token: 0x0600637D RID: 25469 RVA: 0x00270E48 File Offset: 0x0026F048
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

	// Token: 0x0600637E RID: 25470 RVA: 0x00270EBC File Offset: 0x0026F0BC
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

	// Token: 0x0600637F RID: 25471 RVA: 0x00270F20 File Offset: 0x0026F120
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

	// Token: 0x06006380 RID: 25472 RVA: 0x00270F84 File Offset: 0x0026F184
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

	// Token: 0x06006381 RID: 25473 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetBatteryLevel()
	{
		return 0f;
	}

	// Token: 0x06006382 RID: 25474 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBatteryLevel(float _amount)
	{
	}

	// Token: 0x06006383 RID: 25475 RVA: 0x00270FE4 File Offset: 0x0026F1E4
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

	// Token: 0x06006384 RID: 25476 RVA: 0x00271041 File Offset: 0x0026F241
	public int GetRepairAmountNeeded()
	{
		return this.GetMaxHealth() - this.entity.Health;
	}

	// Token: 0x06006385 RID: 25477 RVA: 0x00271058 File Offset: 0x0026F258
	public void RepairParts(int _add, float _percent)
	{
		int num = _add + (int)((float)this.GetMaxHealth() * _percent);
		num = Utils.FastMin(num, this.GetRepairAmountNeeded());
		this.entity.Health += num;
	}

	// Token: 0x06006386 RID: 25478 RVA: 0x00271092 File Offset: 0x0026F292
	public bool IsDriveable()
	{
		return this.HasSteering();
	}

	// Token: 0x06006387 RID: 25479 RVA: 0x0027109A File Offset: 0x0026F29A
	public bool HasEnginePart()
	{
		return this.FindPart("engine") != null;
	}

	// Token: 0x06006388 RID: 25480 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetEngineQualityPercent()
	{
		return 0f;
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x002710AA File Offset: 0x0026F2AA
	public bool HasStorage()
	{
		return this.FindPart("storage") != null;
	}

	// Token: 0x0600638A RID: 25482 RVA: 0x00020021 File Offset: 0x0001E221
	public bool HasSteering()
	{
		return true;
	}

	// Token: 0x0600638B RID: 25483 RVA: 0x002710BA File Offset: 0x0026F2BA
	public bool IsSteeringBroken()
	{
		return !this.HasSteering() || this.FindPart("handlebars").IsBroken();
	}

	// Token: 0x0600638C RID: 25484 RVA: 0x002710D6 File Offset: 0x0026F2D6
	public bool HasLock()
	{
		return this.FindPart("lock") != null;
	}

	// Token: 0x0600638D RID: 25485 RVA: 0x002710E6 File Offset: 0x0026F2E6
	public bool IsLockBroken()
	{
		return this.FindPart("lock").GetHealthPercentage() == 0f;
	}

	// Token: 0x0600638E RID: 25486 RVA: 0x002710FF File Offset: 0x0026F2FF
	public string GetHornSoundName()
	{
		return this.Properties.GetString("hornSound");
	}

	// Token: 0x0600638F RID: 25487 RVA: 0x00271111 File Offset: 0x0026F311
	public string GetHornEventName()
	{
		return this.onHonkEvent;
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x00271119 File Offset: 0x0026F319
	public bool HasHorn()
	{
		return this.GetHornSoundName().Length > 0;
	}

	// Token: 0x06006391 RID: 25489 RVA: 0x0027112C File Offset: 0x0026F32C
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

	// Token: 0x06006392 RID: 25490 RVA: 0x002711B4 File Offset: 0x0026F3B4
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

	// Token: 0x06006393 RID: 25491 RVA: 0x00271209 File Offset: 0x0026F409
	public int GetVehicleQuality()
	{
		return (int)this.itemValue.Quality;
	}

	// Token: 0x06006394 RID: 25492 RVA: 0x00271218 File Offset: 0x0026F418
	public int GetHealth()
	{
		int health = this.entity.Health;
		if (health <= 1)
		{
			return 0;
		}
		return health;
	}

	// Token: 0x06006395 RID: 25493 RVA: 0x00271238 File Offset: 0x0026F438
	public int GetMaxHealth()
	{
		return this.entity.GetMaxHealth();
	}

	// Token: 0x06006396 RID: 25494 RVA: 0x00271245 File Offset: 0x0026F445
	public float GetHealthPercent()
	{
		return (float)this.GetHealth() / (float)this.entity.GetMaxHealth();
	}

	// Token: 0x06006397 RID: 25495 RVA: 0x0027125B File Offset: 0x0026F45B
	public float GetPlayerDamagePercent()
	{
		return 0.1f;
	}

	// Token: 0x06006398 RID: 25496 RVA: 0x0014C85F File Offset: 0x0014AA5F
	public float GetNoise()
	{
		return 0.5f;
	}

	// Token: 0x06006399 RID: 25497 RVA: 0x00271264 File Offset: 0x0026F464
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

	// Token: 0x04004D3A RID: 19770
	public static Dictionary<string, DynamicProperties> PropertyMap;

	// Token: 0x04004D3B RID: 19771
	public DynamicProperties Properties;

	// Token: 0x04004D3C RID: 19772
	public ItemValue itemValue;

	// Token: 0x04004D3D RID: 19773
	public List<PlatformUserIdentifierAbs> AllowedUsers;

	// Token: 0x04004D3E RID: 19774
	public string PasswordHash = string.Empty;

	// Token: 0x04004D3F RID: 19775
	public EntityVehicle entity;

	// Token: 0x04004D40 RID: 19776
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_ownerId;

	// Token: 0x04004D41 RID: 19777
	[PublicizedFrom(EAccessModifier.Private)]
	public string vehicleName;

	// Token: 0x04004D42 RID: 19778
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VehiclePart> vehicleParts;

	// Token: 0x04004D43 RID: 19779
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform meshT;

	// Token: 0x04004D44 RID: 19780
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraDistance = new Vector2(1f, 1f);

	// Token: 0x04004D45 RID: 19781
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraTurnRate = new Vector2(1f, 1f);

	// Token: 0x04004D46 RID: 19782
	[PublicizedFrom(EAccessModifier.Private)]
	public float upAngleMax = 70f;

	// Token: 0x04004D47 RID: 19783
	[PublicizedFrom(EAccessModifier.Private)]
	public float upForce = 5f;

	// Token: 0x04004D48 RID: 19784
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerAngleMax = 25f;

	// Token: 0x04004D49 RID: 19785
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerRate = 130f;

	// Token: 0x04004D4A RID: 19786
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerCenteringRate = 90f;

	// Token: 0x04004D4B RID: 19787
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltAngleMax = 20f;

	// Token: 0x04004D4C RID: 19788
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltThreshold = 3f;

	// Token: 0x04004D4D RID: 19789
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampening = 0.22f;

	// Token: 0x04004D4E RID: 19790
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampenThreshold = 8f;

	// Token: 0x04004D4F RID: 19791
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltUpForce = 5f;

	// Token: 0x04004D50 RID: 19792
	public float MotorTorqueForward = 300f;

	// Token: 0x04004D51 RID: 19793
	public float MotorTorqueBackward = 300f;

	// Token: 0x04004D52 RID: 19794
	public float MotorTorqueTurboForward;

	// Token: 0x04004D53 RID: 19795
	public float MotorTorqueTurboBackward;

	// Token: 0x04004D54 RID: 19796
	public float VelocityMaxForward = 10f;

	// Token: 0x04004D55 RID: 19797
	public float VelocityMaxBackward = 10f;

	// Token: 0x04004D56 RID: 19798
	public float VelocityMaxTurboForward = 10f;

	// Token: 0x04004D57 RID: 19799
	public float VelocityMaxTurboBackward = 10f;

	// Token: 0x04004D58 RID: 19800
	[PublicizedFrom(EAccessModifier.Private)]
	public float brakeTorque = 4000f;

	// Token: 0x04004D59 RID: 19801
	public bool CanTurbo = true;

	// Token: 0x04004D5A RID: 19802
	public bool IsTurbo;

	// Token: 0x04004D5B RID: 19803
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 hopForce;

	// Token: 0x04004D5C RID: 19804
	[PublicizedFrom(EAccessModifier.Private)]
	public float unstickForce = 1f;

	// Token: 0x04004D5D RID: 19805
	public string onHonkEvent = "";

	// Token: 0x04004D5E RID: 19806
	public float AirDragVelScale = 0.997f;

	// Token: 0x04004D5F RID: 19807
	public float AirDragAngVelScale = 1f;

	// Token: 0x04004D60 RID: 19808
	public float WaterDragY;

	// Token: 0x04004D61 RID: 19809
	public float WaterDragVelScale = 1f;

	// Token: 0x04004D62 RID: 19810
	public float WaterDragVelMaxScale = 1f;

	// Token: 0x04004D63 RID: 19811
	public float WaterLiftY;

	// Token: 0x04004D64 RID: 19812
	public float WaterLiftDepth;

	// Token: 0x04004D65 RID: 19813
	public float WaterLiftForce;

	// Token: 0x04004D66 RID: 19814
	public float WheelPtlScale;

	// Token: 0x04004D67 RID: 19815
	public float CurrentForwardVelocity;

	// Token: 0x04004D68 RID: 19816
	public bool CurrentIsAccel;

	// Token: 0x04004D69 RID: 19817
	public bool CurrentIsBreak;

	// Token: 0x04004D6A RID: 19818
	public float CurrentMotorTorquePercent;

	// Token: 0x04004D6B RID: 19819
	public float CurrentSteeringPercent;

	// Token: 0x04004D6C RID: 19820
	public Vector3 CurrentVelocity;

	// Token: 0x04004D6D RID: 19821
	public string RecipeName;

	// Token: 0x04004D6E RID: 19822
	public Material mainEmissiveMat;

	// Token: 0x04004D6F RID: 19823
	public float EffectEntityDamagePer;

	// Token: 0x04004D70 RID: 19824
	public float EffectBlockDamagePer;

	// Token: 0x04004D71 RID: 19825
	public float EffectSelfDamagePer;

	// Token: 0x04004D72 RID: 19826
	public float EffectStrongSelfDamagePer;

	// Token: 0x04004D73 RID: 19827
	public float EffectLightIntensity;

	// Token: 0x04004D74 RID: 19828
	public float EffectFuelMaxPer = 1f;

	// Token: 0x04004D75 RID: 19829
	public float EffectFuelUsePer;

	// Token: 0x04004D76 RID: 19830
	public float EffectMotorTorquePer;

	// Token: 0x04004D77 RID: 19831
	public float EffectVelocityMaxPer;

	// Token: 0x04004D78 RID: 19832
	[PublicizedFrom(EAccessModifier.Private)]
	public float effectUpdateDelay;

	// Token: 0x04004D79 RID: 19833
	public FastTags<TagGroup.Global> ModTags;

	// Token: 0x02000CA9 RID: 3241
	public enum Event
	{
		// Token: 0x04004D7B RID: 19835
		Start,
		// Token: 0x04004D7C RID: 19836
		Started,
		// Token: 0x04004D7D RID: 19837
		Stop,
		// Token: 0x04004D7E RID: 19838
		Stopped,
		// Token: 0x04004D7F RID: 19839
		SimulationUpdate,
		// Token: 0x04004D80 RID: 19840
		HealthChanged
	}
}

using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000CB6 RID: 3254
public class Vehicle
{
	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x0600633F RID: 25407 RVA: 0x00273428 File Offset: 0x00271628
	public Vector2 CameraDistance
	{
		get
		{
			return this.cameraDistance;
		}
	}

	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x06006340 RID: 25408 RVA: 0x00273430 File Offset: 0x00271630
	public Vector2 CameraTurnRate
	{
		get
		{
			return this.cameraTurnRate;
		}
	}

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x06006341 RID: 25409 RVA: 0x00273438 File Offset: 0x00271638
	public float BrakeTorque
	{
		get
		{
			return this.brakeTorque;
		}
	}

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x06006342 RID: 25410 RVA: 0x00273440 File Offset: 0x00271640
	public float SteerAngleMax
	{
		get
		{
			return this.steerAngleMax;
		}
	}

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x06006343 RID: 25411 RVA: 0x00273448 File Offset: 0x00271648
	public float SteerRate
	{
		get
		{
			return this.steerRate;
		}
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06006344 RID: 25412 RVA: 0x00273450 File Offset: 0x00271650
	public float SteerCenteringRate
	{
		get
		{
			return this.steerCenteringRate;
		}
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06006345 RID: 25413 RVA: 0x00273458 File Offset: 0x00271658
	public float TiltAngleMax
	{
		get
		{
			return this.tiltAngleMax;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06006346 RID: 25414 RVA: 0x00273460 File Offset: 0x00271660
	public float TiltThreshold
	{
		get
		{
			return this.tiltThreshold;
		}
	}

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06006347 RID: 25415 RVA: 0x00273468 File Offset: 0x00271668
	public float TiltDampening
	{
		get
		{
			return this.tiltDampening;
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x06006348 RID: 25416 RVA: 0x00273470 File Offset: 0x00271670
	public float TiltDampenThreshold
	{
		get
		{
			return this.tiltDampenThreshold;
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x06006349 RID: 25417 RVA: 0x00273478 File Offset: 0x00271678
	public float TiltUpForce
	{
		get
		{
			return this.tiltUpForce;
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x0600634A RID: 25418 RVA: 0x00273480 File Offset: 0x00271680
	public Vector2 HopForce
	{
		get
		{
			return this.hopForce;
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x0600634B RID: 25419 RVA: 0x00273488 File Offset: 0x00271688
	public float UpAngleMax
	{
		get
		{
			return this.upAngleMax;
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x0600634C RID: 25420 RVA: 0x00273490 File Offset: 0x00271690
	public float UpForce
	{
		get
		{
			return this.upForce;
		}
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x0600634D RID: 25421 RVA: 0x00273498 File Offset: 0x00271698
	public float UnstickForce
	{
		get
		{
			return this.unstickForce;
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x0600634E RID: 25422 RVA: 0x002734A0 File Offset: 0x002716A0
	public float MaxPossibleSpeed
	{
		get
		{
			return this.VelocityMaxTurboForward;
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x0600634F RID: 25423 RVA: 0x002734A8 File Offset: 0x002716A8
	// (set) Token: 0x06006350 RID: 25424 RVA: 0x002734B0 File Offset: 0x002716B0
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

	// Token: 0x06006351 RID: 25425 RVA: 0x002734F4 File Offset: 0x002716F4
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

	// Token: 0x06006352 RID: 25426 RVA: 0x002736D4 File Offset: 0x002718D4
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

	// Token: 0x06006353 RID: 25427 RVA: 0x00273728 File Offset: 0x00271928
	public void SetItemValue(ItemValue _itemValue)
	{
		this.itemValue = _itemValue;
		if (this.itemValue.CosmeticModCount == 0)
		{
			this.itemValue.ResizeCosmeticMods(1);
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

	// Token: 0x06006354 RID: 25428 RVA: 0x002737ED File Offset: 0x002719ED
	public void OnModsChanged()
	{
		this.CalcEffects();
		this.CalcMods();
		this.SetColors();
		this.SetSeats();
	}

	// Token: 0x06006355 RID: 25429 RVA: 0x00273808 File Offset: 0x00271A08
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

	// Token: 0x06006356 RID: 25430 RVA: 0x00273888 File Offset: 0x00271A88
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

	// Token: 0x06006357 RID: 25431 RVA: 0x0027392C File Offset: 0x00271B2C
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

	// Token: 0x06006358 RID: 25432 RVA: 0x00273984 File Offset: 0x00271B84
	public ItemValue GetUpdatedItemValue()
	{
		this.itemValue.UseTimes = (float)((int)this.entity.Stats.Health.BaseMax - this.entity.Health);
		this.itemValue.Meta = (int)(this.GetFuelLevel() * 50f);
		return this.itemValue;
	}

	// Token: 0x06006359 RID: 25433 RVA: 0x002739DD File Offset: 0x00271BDD
	public void LoadItems(ItemStack[] _items)
	{
		this.SetItemValue(_items[0].itemValue);
	}

	// Token: 0x0600635A RID: 25434 RVA: 0x002739ED File Offset: 0x00271BED
	public ItemStack[] GetItems()
	{
		return new ItemStack[]
		{
			new ItemStack(this.GetUpdatedItemValue(), 1)
		};
	}

	// Token: 0x0600635B RID: 25435 RVA: 0x00273A04 File Offset: 0x00271C04
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

	// Token: 0x0600635C RID: 25436 RVA: 0x00273A5F File Offset: 0x00271C5F
	public void UpdateSimulation()
	{
		this.FireEvent(Vehicle.Event.SimulationUpdate);
	}

	// Token: 0x0600635D RID: 25437 RVA: 0x00273A68 File Offset: 0x00271C68
	public void FireEvent(Vehicle.Event _event)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, 0f);
		}
	}

	// Token: 0x0600635E RID: 25438 RVA: 0x00273AA4 File Offset: 0x00271CA4
	public void FireEvent(VehiclePart.Event _event, VehiclePart _fromPart, float _arg)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, _fromPart, _arg);
		}
	}

	// Token: 0x0600635F RID: 25439 RVA: 0x00273ADB File Offset: 0x00271CDB
	public void SetupProperties()
	{
		if (!Vehicle.PropertyMap.TryGetValue(this.vehicleName, out this.Properties))
		{
			Log.Error("Vehicle properties for '{0}' not found!", new object[] { this.vehicleName });
		}
	}

	// Token: 0x06006360 RID: 25440 RVA: 0x00273B10 File Offset: 0x00271D10
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

	// Token: 0x06006361 RID: 25441 RVA: 0x00273B3C File Offset: 0x00271D3C
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

	// Token: 0x06006362 RID: 25442 RVA: 0x00273D14 File Offset: 0x00271F14
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

	// Token: 0x06006363 RID: 25443 RVA: 0x00273D9C File Offset: 0x00271F9C
	public void TriggerUpdateEffects()
	{
		this.effectUpdateDelay = 0f;
	}

	// Token: 0x06006364 RID: 25444 RVA: 0x00273DA9 File Offset: 0x00271FA9
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

	// Token: 0x06006365 RID: 25445 RVA: 0x00273DE0 File Offset: 0x00271FE0
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

	// Token: 0x06006366 RID: 25446 RVA: 0x00273F64 File Offset: 0x00272164
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcMods()
	{
		int num = 0;
		this.ModTags = FastTags<TagGroup.Global>.none;
		for (int i = 0; i < this.itemValue.ModificationCount; i++)
		{
			ItemValue modification = this.itemValue.GetModification(i);
			if (modification != null)
			{
				ItemClassModifier itemClassModifier = modification.ItemClass as ItemClassModifier;
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
		for (int j = 0; j < this.vehicleParts.Count; j++)
		{
			this.vehicleParts[j].SetMods();
		}
		this.entity.UpdateStorageModCount(num);
		this.entity.UpdateContainerSize(false);
	}

	// Token: 0x06006367 RID: 25447 RVA: 0x00274024 File Offset: 0x00272224
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

	// Token: 0x06006368 RID: 25448 RVA: 0x00274150 File Offset: 0x00272350
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

	// Token: 0x06006369 RID: 25449 RVA: 0x0027419C File Offset: 0x0027239C
	public string GetPartProperty(string _tag, string _propertyName)
	{
		VehiclePart vehiclePart = this.FindPart(_tag);
		if (vehiclePart == null)
		{
			return string.Empty;
		}
		return vehiclePart.GetProperty(_propertyName);
	}

	// Token: 0x0600636A RID: 25450 RVA: 0x002741C1 File Offset: 0x002723C1
	public List<VehiclePart> GetParts()
	{
		return this.vehicleParts;
	}

	// Token: 0x0600636B RID: 25451 RVA: 0x002741CC File Offset: 0x002723CC
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

	// Token: 0x0600636C RID: 25452 RVA: 0x0027421B File Offset: 0x0027241B
	public Transform GetMeshTransform()
	{
		return this.meshT;
	}

	// Token: 0x0600636D RID: 25453 RVA: 0x00274223 File Offset: 0x00272423
	public string GetName()
	{
		return this.vehicleName;
	}

	// Token: 0x0600636E RID: 25454 RVA: 0x0027422B File Offset: 0x0027242B
	public static void Cleanup()
	{
		Vehicle.PropertyMap = new Dictionary<string, DynamicProperties>();
	}

	// Token: 0x0600636F RID: 25455 RVA: 0x00274237 File Offset: 0x00272437
	public string GetFuelItem()
	{
		if (this.HasEnginePart())
		{
			return "ammoGasCan";
		}
		return "";
	}

	// Token: 0x06006370 RID: 25456 RVA: 0x0027424C File Offset: 0x0027244C
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

	// Token: 0x06006371 RID: 25457 RVA: 0x002742C0 File Offset: 0x002724C0
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

	// Token: 0x06006372 RID: 25458 RVA: 0x00274324 File Offset: 0x00272524
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

	// Token: 0x06006373 RID: 25459 RVA: 0x00274388 File Offset: 0x00272588
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

	// Token: 0x06006374 RID: 25460 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetBatteryLevel()
	{
		return 0f;
	}

	// Token: 0x06006375 RID: 25461 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBatteryLevel(float _amount)
	{
	}

	// Token: 0x06006376 RID: 25462 RVA: 0x002743E8 File Offset: 0x002725E8
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

	// Token: 0x06006377 RID: 25463 RVA: 0x00274445 File Offset: 0x00272645
	public int GetRepairAmountNeeded()
	{
		return this.GetMaxHealth() - this.entity.Health;
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x0027445C File Offset: 0x0027265C
	public void RepairParts(int _add, float _percent)
	{
		int num = _add + (int)((float)this.GetMaxHealth() * _percent);
		num = Utils.FastMin(num, this.GetRepairAmountNeeded());
		this.entity.Health += num;
	}

	// Token: 0x06006379 RID: 25465 RVA: 0x00274496 File Offset: 0x00272696
	public bool IsDriveable()
	{
		return this.HasSteering();
	}

	// Token: 0x0600637A RID: 25466 RVA: 0x0027449E File Offset: 0x0027269E
	public bool HasEnginePart()
	{
		return this.FindPart("engine") != null;
	}

	// Token: 0x0600637B RID: 25467 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetEngineQualityPercent()
	{
		return 0f;
	}

	// Token: 0x0600637C RID: 25468 RVA: 0x002744AE File Offset: 0x002726AE
	public bool HasStorage()
	{
		return this.FindPart("storage") != null;
	}

	// Token: 0x0600637D RID: 25469 RVA: 0x00020119 File Offset: 0x0001E319
	public bool HasSteering()
	{
		return true;
	}

	// Token: 0x0600637E RID: 25470 RVA: 0x002744BE File Offset: 0x002726BE
	public bool IsSteeringBroken()
	{
		return !this.HasSteering() || this.FindPart("handlebars").IsBroken();
	}

	// Token: 0x0600637F RID: 25471 RVA: 0x002744DA File Offset: 0x002726DA
	public bool HasLock()
	{
		return this.FindPart("lock") != null;
	}

	// Token: 0x06006380 RID: 25472 RVA: 0x002744EA File Offset: 0x002726EA
	public bool IsLockBroken()
	{
		return this.FindPart("lock").GetHealthPercentage() == 0f;
	}

	// Token: 0x06006381 RID: 25473 RVA: 0x00274503 File Offset: 0x00272703
	public string GetHornSoundName()
	{
		return this.Properties.GetString("hornSound");
	}

	// Token: 0x06006382 RID: 25474 RVA: 0x00274515 File Offset: 0x00272715
	public string GetHornEventName()
	{
		return this.onHonkEvent;
	}

	// Token: 0x06006383 RID: 25475 RVA: 0x0027451D File Offset: 0x0027271D
	public bool HasHorn()
	{
		return this.GetHornSoundName().Length > 0;
	}

	// Token: 0x06006384 RID: 25476 RVA: 0x00274530 File Offset: 0x00272730
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

	// Token: 0x06006385 RID: 25477 RVA: 0x002745B8 File Offset: 0x002727B8
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

	// Token: 0x06006386 RID: 25478 RVA: 0x0027460D File Offset: 0x0027280D
	public int GetVehicleQuality()
	{
		return (int)this.itemValue.Quality;
	}

	// Token: 0x06006387 RID: 25479 RVA: 0x0027461C File Offset: 0x0027281C
	public int GetHealth()
	{
		int health = this.entity.Health;
		if (health <= 1)
		{
			return 0;
		}
		return health;
	}

	// Token: 0x06006388 RID: 25480 RVA: 0x0027463C File Offset: 0x0027283C
	public int GetMaxHealth()
	{
		return this.entity.GetMaxHealth();
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x00274649 File Offset: 0x00272849
	public float GetHealthPercent()
	{
		return (float)this.GetHealth() / (float)this.entity.GetMaxHealth();
	}

	// Token: 0x0600638A RID: 25482 RVA: 0x0027465F File Offset: 0x0027285F
	public float GetPlayerDamagePercent()
	{
		return 0.1f;
	}

	// Token: 0x0600638B RID: 25483 RVA: 0x0014D1CF File Offset: 0x0014B3CF
	public float GetNoise()
	{
		return 0.5f;
	}

	// Token: 0x0600638C RID: 25484 RVA: 0x00274668 File Offset: 0x00272868
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

	// Token: 0x04004D70 RID: 19824
	public static Dictionary<string, DynamicProperties> PropertyMap;

	// Token: 0x04004D71 RID: 19825
	public DynamicProperties Properties;

	// Token: 0x04004D72 RID: 19826
	public ItemValue itemValue;

	// Token: 0x04004D73 RID: 19827
	public List<PlatformUserIdentifierAbs> AllowedUsers;

	// Token: 0x04004D74 RID: 19828
	public string PasswordHash = string.Empty;

	// Token: 0x04004D75 RID: 19829
	public EntityVehicle entity;

	// Token: 0x04004D76 RID: 19830
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_ownerId;

	// Token: 0x04004D77 RID: 19831
	[PublicizedFrom(EAccessModifier.Private)]
	public string vehicleName;

	// Token: 0x04004D78 RID: 19832
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VehiclePart> vehicleParts;

	// Token: 0x04004D79 RID: 19833
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform meshT;

	// Token: 0x04004D7A RID: 19834
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraDistance = new Vector2(1f, 1f);

	// Token: 0x04004D7B RID: 19835
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraTurnRate = new Vector2(1f, 1f);

	// Token: 0x04004D7C RID: 19836
	[PublicizedFrom(EAccessModifier.Private)]
	public float upAngleMax = 70f;

	// Token: 0x04004D7D RID: 19837
	[PublicizedFrom(EAccessModifier.Private)]
	public float upForce = 5f;

	// Token: 0x04004D7E RID: 19838
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerAngleMax = 25f;

	// Token: 0x04004D7F RID: 19839
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerRate = 130f;

	// Token: 0x04004D80 RID: 19840
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerCenteringRate = 90f;

	// Token: 0x04004D81 RID: 19841
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltAngleMax = 20f;

	// Token: 0x04004D82 RID: 19842
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltThreshold = 3f;

	// Token: 0x04004D83 RID: 19843
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampening = 0.22f;

	// Token: 0x04004D84 RID: 19844
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampenThreshold = 8f;

	// Token: 0x04004D85 RID: 19845
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltUpForce = 5f;

	// Token: 0x04004D86 RID: 19846
	public float MotorTorqueForward = 300f;

	// Token: 0x04004D87 RID: 19847
	public float MotorTorqueBackward = 300f;

	// Token: 0x04004D88 RID: 19848
	public float MotorTorqueTurboForward;

	// Token: 0x04004D89 RID: 19849
	public float MotorTorqueTurboBackward;

	// Token: 0x04004D8A RID: 19850
	public float VelocityMaxForward = 10f;

	// Token: 0x04004D8B RID: 19851
	public float VelocityMaxBackward = 10f;

	// Token: 0x04004D8C RID: 19852
	public float VelocityMaxTurboForward = 10f;

	// Token: 0x04004D8D RID: 19853
	public float VelocityMaxTurboBackward = 10f;

	// Token: 0x04004D8E RID: 19854
	[PublicizedFrom(EAccessModifier.Private)]
	public float brakeTorque = 4000f;

	// Token: 0x04004D8F RID: 19855
	public bool CanTurbo = true;

	// Token: 0x04004D90 RID: 19856
	public bool IsTurbo;

	// Token: 0x04004D91 RID: 19857
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 hopForce;

	// Token: 0x04004D92 RID: 19858
	[PublicizedFrom(EAccessModifier.Private)]
	public float unstickForce = 1f;

	// Token: 0x04004D93 RID: 19859
	public string onHonkEvent = "";

	// Token: 0x04004D94 RID: 19860
	public float AirDragVelScale = 0.997f;

	// Token: 0x04004D95 RID: 19861
	public float AirDragAngVelScale = 1f;

	// Token: 0x04004D96 RID: 19862
	public float WaterDragY;

	// Token: 0x04004D97 RID: 19863
	public float WaterDragVelScale = 1f;

	// Token: 0x04004D98 RID: 19864
	public float WaterDragVelMaxScale = 1f;

	// Token: 0x04004D99 RID: 19865
	public float WaterLiftY;

	// Token: 0x04004D9A RID: 19866
	public float WaterLiftDepth;

	// Token: 0x04004D9B RID: 19867
	public float WaterLiftForce;

	// Token: 0x04004D9C RID: 19868
	public float WheelPtlScale;

	// Token: 0x04004D9D RID: 19869
	public float CurrentForwardVelocity;

	// Token: 0x04004D9E RID: 19870
	public bool CurrentIsAccel;

	// Token: 0x04004D9F RID: 19871
	public bool CurrentIsBreak;

	// Token: 0x04004DA0 RID: 19872
	public float CurrentMotorTorquePercent;

	// Token: 0x04004DA1 RID: 19873
	public float CurrentSteeringPercent;

	// Token: 0x04004DA2 RID: 19874
	public Vector3 CurrentVelocity;

	// Token: 0x04004DA3 RID: 19875
	public string RecipeName;

	// Token: 0x04004DA4 RID: 19876
	public Material mainEmissiveMat;

	// Token: 0x04004DA5 RID: 19877
	public float EffectEntityDamagePer;

	// Token: 0x04004DA6 RID: 19878
	public float EffectBlockDamagePer;

	// Token: 0x04004DA7 RID: 19879
	public float EffectSelfDamagePer;

	// Token: 0x04004DA8 RID: 19880
	public float EffectStrongSelfDamagePer;

	// Token: 0x04004DA9 RID: 19881
	public float EffectLightIntensity;

	// Token: 0x04004DAA RID: 19882
	public float EffectFuelMaxPer = 1f;

	// Token: 0x04004DAB RID: 19883
	public float EffectFuelUsePer;

	// Token: 0x04004DAC RID: 19884
	public float EffectMotorTorquePer;

	// Token: 0x04004DAD RID: 19885
	public float EffectVelocityMaxPer;

	// Token: 0x04004DAE RID: 19886
	[PublicizedFrom(EAccessModifier.Private)]
	public float effectUpdateDelay;

	// Token: 0x04004DAF RID: 19887
	public FastTags<TagGroup.Global> ModTags;

	// Token: 0x02000CB7 RID: 3255
	public enum Event
	{
		// Token: 0x04004DB1 RID: 19889
		Start,
		// Token: 0x04004DB2 RID: 19890
		Started,
		// Token: 0x04004DB3 RID: 19891
		Stop,
		// Token: 0x04004DB4 RID: 19892
		Stopped,
		// Token: 0x04004DB5 RID: 19893
		SimulationUpdate,
		// Token: 0x04004DB6 RID: 19894
		HealthChanged
	}
}

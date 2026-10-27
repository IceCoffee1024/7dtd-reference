using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000CC1 RID: 3265
public class Vehicle
{
	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06006364 RID: 25444 RVA: 0x00274950 File Offset: 0x00272B50
	public Vector2 CameraDistance
	{
		get
		{
			return this.cameraDistance;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06006365 RID: 25445 RVA: 0x00274958 File Offset: 0x00272B58
	public Vector2 CameraTurnRate
	{
		get
		{
			return this.cameraTurnRate;
		}
	}

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06006366 RID: 25446 RVA: 0x00274960 File Offset: 0x00272B60
	public float BrakeTorque
	{
		get
		{
			return this.brakeTorque;
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x06006367 RID: 25447 RVA: 0x00274968 File Offset: 0x00272B68
	public float SteerAngleMax
	{
		get
		{
			return this.steerAngleMax;
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x06006368 RID: 25448 RVA: 0x00274970 File Offset: 0x00272B70
	public float SteerRate
	{
		get
		{
			return this.steerRate;
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06006369 RID: 25449 RVA: 0x00274978 File Offset: 0x00272B78
	public float SteerCenteringRate
	{
		get
		{
			return this.steerCenteringRate;
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x0600636A RID: 25450 RVA: 0x00274980 File Offset: 0x00272B80
	public float TiltAngleMax
	{
		get
		{
			return this.tiltAngleMax;
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x0600636B RID: 25451 RVA: 0x00274988 File Offset: 0x00272B88
	public float TiltThreshold
	{
		get
		{
			return this.tiltThreshold;
		}
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x0600636C RID: 25452 RVA: 0x00274990 File Offset: 0x00272B90
	public float TiltDampening
	{
		get
		{
			return this.tiltDampening;
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x0600636D RID: 25453 RVA: 0x00274998 File Offset: 0x00272B98
	public float TiltDampenThreshold
	{
		get
		{
			return this.tiltDampenThreshold;
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x0600636E RID: 25454 RVA: 0x002749A0 File Offset: 0x00272BA0
	public float TiltUpForce
	{
		get
		{
			return this.tiltUpForce;
		}
	}

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x0600636F RID: 25455 RVA: 0x002749A8 File Offset: 0x00272BA8
	public Vector2 HopForce
	{
		get
		{
			return this.hopForce;
		}
	}

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06006370 RID: 25456 RVA: 0x002749B0 File Offset: 0x00272BB0
	public float UpAngleMax
	{
		get
		{
			return this.upAngleMax;
		}
	}

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06006371 RID: 25457 RVA: 0x002749B8 File Offset: 0x00272BB8
	public float UpForce
	{
		get
		{
			return this.upForce;
		}
	}

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06006372 RID: 25458 RVA: 0x002749C0 File Offset: 0x00272BC0
	public float UnstickForce
	{
		get
		{
			return this.unstickForce;
		}
	}

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x06006373 RID: 25459 RVA: 0x002749C8 File Offset: 0x00272BC8
	public float MaxPossibleSpeed
	{
		get
		{
			return this.VelocityMaxTurboForward;
		}
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x06006374 RID: 25460 RVA: 0x002749D0 File Offset: 0x00272BD0
	// (set) Token: 0x06006375 RID: 25461 RVA: 0x002749D8 File Offset: 0x00272BD8
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

	// Token: 0x06006376 RID: 25462 RVA: 0x00274A1C File Offset: 0x00272C1C
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

	// Token: 0x06006377 RID: 25463 RVA: 0x00274BFC File Offset: 0x00272DFC
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

	// Token: 0x06006378 RID: 25464 RVA: 0x00274C50 File Offset: 0x00272E50
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

	// Token: 0x06006379 RID: 25465 RVA: 0x00274D15 File Offset: 0x00272F15
	public void OnModsChanged()
	{
		this.CalcEffects();
		this.CalcMods();
		this.SetColors();
		this.SetSeats();
	}

	// Token: 0x0600637A RID: 25466 RVA: 0x00274D30 File Offset: 0x00272F30
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

	// Token: 0x0600637B RID: 25467 RVA: 0x00274DB0 File Offset: 0x00272FB0
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

	// Token: 0x0600637C RID: 25468 RVA: 0x00274E54 File Offset: 0x00273054
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

	// Token: 0x0600637D RID: 25469 RVA: 0x00274EAC File Offset: 0x002730AC
	public ItemValue GetUpdatedItemValue()
	{
		this.itemValue.UseTimes = (float)((int)this.entity.Stats.Health.BaseMax - this.entity.Health);
		this.itemValue.Meta = (int)(this.GetFuelLevel() * 50f);
		return this.itemValue;
	}

	// Token: 0x0600637E RID: 25470 RVA: 0x00274F05 File Offset: 0x00273105
	public void LoadItems(ItemStack[] _items)
	{
		this.SetItemValue(_items[0].itemValue);
	}

	// Token: 0x0600637F RID: 25471 RVA: 0x00274F15 File Offset: 0x00273115
	public ItemStack[] GetItems()
	{
		return new ItemStack[]
		{
			new ItemStack(this.GetUpdatedItemValue(), 1)
		};
	}

	// Token: 0x06006380 RID: 25472 RVA: 0x00274F2C File Offset: 0x0027312C
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

	// Token: 0x06006381 RID: 25473 RVA: 0x00274F87 File Offset: 0x00273187
	public void UpdateSimulation()
	{
		this.FireEvent(Vehicle.Event.SimulationUpdate);
	}

	// Token: 0x06006382 RID: 25474 RVA: 0x00274F90 File Offset: 0x00273190
	public void FireEvent(Vehicle.Event _event)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, 0f);
		}
	}

	// Token: 0x06006383 RID: 25475 RVA: 0x00274FCC File Offset: 0x002731CC
	public void FireEvent(VehiclePart.Event _event, VehiclePart _fromPart, float _arg)
	{
		for (int i = 0; i < this.vehicleParts.Count; i++)
		{
			this.vehicleParts[i].HandleEvent(_event, _fromPart, _arg);
		}
	}

	// Token: 0x06006384 RID: 25476 RVA: 0x00275003 File Offset: 0x00273203
	public void SetupProperties()
	{
		if (!Vehicle.PropertyMap.TryGetValue(this.vehicleName, out this.Properties))
		{
			Log.Error("Vehicle properties for '{0}' not found!", new object[] { this.vehicleName });
		}
	}

	// Token: 0x06006385 RID: 25477 RVA: 0x00275038 File Offset: 0x00273238
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

	// Token: 0x06006386 RID: 25478 RVA: 0x00275064 File Offset: 0x00273264
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

	// Token: 0x06006387 RID: 25479 RVA: 0x0027523C File Offset: 0x0027343C
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

	// Token: 0x06006388 RID: 25480 RVA: 0x002752C4 File Offset: 0x002734C4
	public void TriggerUpdateEffects()
	{
		this.effectUpdateDelay = 0f;
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x002752D1 File Offset: 0x002734D1
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

	// Token: 0x0600638A RID: 25482 RVA: 0x00275308 File Offset: 0x00273508
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

	// Token: 0x0600638B RID: 25483 RVA: 0x0027548C File Offset: 0x0027368C
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

	// Token: 0x0600638C RID: 25484 RVA: 0x0027554C File Offset: 0x0027374C
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

	// Token: 0x0600638D RID: 25485 RVA: 0x00275678 File Offset: 0x00273878
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

	// Token: 0x0600638E RID: 25486 RVA: 0x002756C4 File Offset: 0x002738C4
	public string GetPartProperty(string _tag, string _propertyName)
	{
		VehiclePart vehiclePart = this.FindPart(_tag);
		if (vehiclePart == null)
		{
			return string.Empty;
		}
		return vehiclePart.GetProperty(_propertyName);
	}

	// Token: 0x0600638F RID: 25487 RVA: 0x002756E9 File Offset: 0x002738E9
	public List<VehiclePart> GetParts()
	{
		return this.vehicleParts;
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x002756F4 File Offset: 0x002738F4
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

	// Token: 0x06006391 RID: 25489 RVA: 0x00275743 File Offset: 0x00273943
	public Transform GetMeshTransform()
	{
		return this.meshT;
	}

	// Token: 0x06006392 RID: 25490 RVA: 0x0027574B File Offset: 0x0027394B
	public string GetName()
	{
		return this.vehicleName;
	}

	// Token: 0x06006393 RID: 25491 RVA: 0x00275753 File Offset: 0x00273953
	public static void Cleanup()
	{
		Vehicle.PropertyMap = new Dictionary<string, DynamicProperties>();
	}

	// Token: 0x06006394 RID: 25492 RVA: 0x0027575F File Offset: 0x0027395F
	public string GetFuelItem()
	{
		if (this.HasEnginePart())
		{
			return "ammoGasCan";
		}
		return "";
	}

	// Token: 0x06006395 RID: 25493 RVA: 0x00275774 File Offset: 0x00273974
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

	// Token: 0x06006396 RID: 25494 RVA: 0x002757E8 File Offset: 0x002739E8
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

	// Token: 0x06006397 RID: 25495 RVA: 0x0027584C File Offset: 0x00273A4C
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

	// Token: 0x06006398 RID: 25496 RVA: 0x002758B0 File Offset: 0x00273AB0
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

	// Token: 0x06006399 RID: 25497 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetBatteryLevel()
	{
		return 0f;
	}

	// Token: 0x0600639A RID: 25498 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBatteryLevel(float _amount)
	{
	}

	// Token: 0x0600639B RID: 25499 RVA: 0x00275910 File Offset: 0x00273B10
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

	// Token: 0x0600639C RID: 25500 RVA: 0x0027596D File Offset: 0x00273B6D
	public int GetRepairAmountNeeded()
	{
		return this.GetMaxHealth() - this.entity.Health;
	}

	// Token: 0x0600639D RID: 25501 RVA: 0x00275984 File Offset: 0x00273B84
	public void RepairParts(int _add, float _percent)
	{
		int num = _add + (int)((float)this.GetMaxHealth() * _percent);
		num = Utils.FastMin(num, this.GetRepairAmountNeeded());
		this.entity.Health += num;
	}

	// Token: 0x0600639E RID: 25502 RVA: 0x002759BE File Offset: 0x00273BBE
	public bool IsDriveable()
	{
		return this.HasSteering();
	}

	// Token: 0x0600639F RID: 25503 RVA: 0x002759C6 File Offset: 0x00273BC6
	public bool HasEnginePart()
	{
		return this.FindPart("engine") != null;
	}

	// Token: 0x060063A0 RID: 25504 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetEngineQualityPercent()
	{
		return 0f;
	}

	// Token: 0x060063A1 RID: 25505 RVA: 0x002759D6 File Offset: 0x00273BD6
	public bool HasStorage()
	{
		return this.FindPart("storage") != null;
	}

	// Token: 0x060063A2 RID: 25506 RVA: 0x00020119 File Offset: 0x0001E319
	public bool HasSteering()
	{
		return true;
	}

	// Token: 0x060063A3 RID: 25507 RVA: 0x002759E6 File Offset: 0x00273BE6
	public bool IsSteeringBroken()
	{
		return !this.HasSteering() || this.FindPart("handlebars").IsBroken();
	}

	// Token: 0x060063A4 RID: 25508 RVA: 0x00275A02 File Offset: 0x00273C02
	public bool HasLock()
	{
		return this.FindPart("lock") != null;
	}

	// Token: 0x060063A5 RID: 25509 RVA: 0x00275A12 File Offset: 0x00273C12
	public bool IsLockBroken()
	{
		return this.FindPart("lock").GetHealthPercentage() == 0f;
	}

	// Token: 0x060063A6 RID: 25510 RVA: 0x00275A2B File Offset: 0x00273C2B
	public string GetHornSoundName()
	{
		return this.Properties.GetString("hornSound");
	}

	// Token: 0x060063A7 RID: 25511 RVA: 0x00275A3D File Offset: 0x00273C3D
	public string GetHornEventName()
	{
		return this.onHonkEvent;
	}

	// Token: 0x060063A8 RID: 25512 RVA: 0x00275A45 File Offset: 0x00273C45
	public bool HasHorn()
	{
		return this.GetHornSoundName().Length > 0;
	}

	// Token: 0x060063A9 RID: 25513 RVA: 0x00275A58 File Offset: 0x00273C58
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

	// Token: 0x060063AA RID: 25514 RVA: 0x00275AE0 File Offset: 0x00273CE0
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

	// Token: 0x060063AB RID: 25515 RVA: 0x00275B35 File Offset: 0x00273D35
	public int GetVehicleQuality()
	{
		return (int)this.itemValue.Quality;
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x00275B44 File Offset: 0x00273D44
	public int GetHealth()
	{
		int health = this.entity.Health;
		if (health <= 1)
		{
			return 0;
		}
		return health;
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x00275B64 File Offset: 0x00273D64
	public int GetMaxHealth()
	{
		return this.entity.GetMaxHealth();
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x00275B71 File Offset: 0x00273D71
	public float GetHealthPercent()
	{
		return (float)this.GetHealth() / (float)this.entity.GetMaxHealth();
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x00275B87 File Offset: 0x00273D87
	public float GetPlayerDamagePercent()
	{
		return 0.1f;
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x0014DDC3 File Offset: 0x0014BFC3
	public float GetNoise()
	{
		return 0.5f;
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x00275B90 File Offset: 0x00273D90
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

	// Token: 0x04004D95 RID: 19861
	public static Dictionary<string, DynamicProperties> PropertyMap;

	// Token: 0x04004D96 RID: 19862
	public DynamicProperties Properties;

	// Token: 0x04004D97 RID: 19863
	public ItemValue itemValue;

	// Token: 0x04004D98 RID: 19864
	public List<PlatformUserIdentifierAbs> AllowedUsers;

	// Token: 0x04004D99 RID: 19865
	public string PasswordHash = string.Empty;

	// Token: 0x04004D9A RID: 19866
	public EntityVehicle entity;

	// Token: 0x04004D9B RID: 19867
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_ownerId;

	// Token: 0x04004D9C RID: 19868
	[PublicizedFrom(EAccessModifier.Private)]
	public string vehicleName;

	// Token: 0x04004D9D RID: 19869
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VehiclePart> vehicleParts;

	// Token: 0x04004D9E RID: 19870
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform meshT;

	// Token: 0x04004D9F RID: 19871
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraDistance = new Vector2(1f, 1f);

	// Token: 0x04004DA0 RID: 19872
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 cameraTurnRate = new Vector2(1f, 1f);

	// Token: 0x04004DA1 RID: 19873
	[PublicizedFrom(EAccessModifier.Private)]
	public float upAngleMax = 70f;

	// Token: 0x04004DA2 RID: 19874
	[PublicizedFrom(EAccessModifier.Private)]
	public float upForce = 5f;

	// Token: 0x04004DA3 RID: 19875
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerAngleMax = 25f;

	// Token: 0x04004DA4 RID: 19876
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerRate = 130f;

	// Token: 0x04004DA5 RID: 19877
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerCenteringRate = 90f;

	// Token: 0x04004DA6 RID: 19878
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltAngleMax = 20f;

	// Token: 0x04004DA7 RID: 19879
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltThreshold = 3f;

	// Token: 0x04004DA8 RID: 19880
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampening = 0.22f;

	// Token: 0x04004DA9 RID: 19881
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltDampenThreshold = 8f;

	// Token: 0x04004DAA RID: 19882
	[PublicizedFrom(EAccessModifier.Private)]
	public float tiltUpForce = 5f;

	// Token: 0x04004DAB RID: 19883
	public float MotorTorqueForward = 300f;

	// Token: 0x04004DAC RID: 19884
	public float MotorTorqueBackward = 300f;

	// Token: 0x04004DAD RID: 19885
	public float MotorTorqueTurboForward;

	// Token: 0x04004DAE RID: 19886
	public float MotorTorqueTurboBackward;

	// Token: 0x04004DAF RID: 19887
	public float VelocityMaxForward = 10f;

	// Token: 0x04004DB0 RID: 19888
	public float VelocityMaxBackward = 10f;

	// Token: 0x04004DB1 RID: 19889
	public float VelocityMaxTurboForward = 10f;

	// Token: 0x04004DB2 RID: 19890
	public float VelocityMaxTurboBackward = 10f;

	// Token: 0x04004DB3 RID: 19891
	[PublicizedFrom(EAccessModifier.Private)]
	public float brakeTorque = 4000f;

	// Token: 0x04004DB4 RID: 19892
	public bool CanTurbo = true;

	// Token: 0x04004DB5 RID: 19893
	public bool IsTurbo;

	// Token: 0x04004DB6 RID: 19894
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 hopForce;

	// Token: 0x04004DB7 RID: 19895
	[PublicizedFrom(EAccessModifier.Private)]
	public float unstickForce = 1f;

	// Token: 0x04004DB8 RID: 19896
	public string onHonkEvent = "";

	// Token: 0x04004DB9 RID: 19897
	public float AirDragVelScale = 0.997f;

	// Token: 0x04004DBA RID: 19898
	public float AirDragAngVelScale = 1f;

	// Token: 0x04004DBB RID: 19899
	public float WaterDragY;

	// Token: 0x04004DBC RID: 19900
	public float WaterDragVelScale = 1f;

	// Token: 0x04004DBD RID: 19901
	public float WaterDragVelMaxScale = 1f;

	// Token: 0x04004DBE RID: 19902
	public float WaterLiftY;

	// Token: 0x04004DBF RID: 19903
	public float WaterLiftDepth;

	// Token: 0x04004DC0 RID: 19904
	public float WaterLiftForce;

	// Token: 0x04004DC1 RID: 19905
	public float WheelPtlScale;

	// Token: 0x04004DC2 RID: 19906
	public float CurrentForwardVelocity;

	// Token: 0x04004DC3 RID: 19907
	public bool CurrentIsAccel;

	// Token: 0x04004DC4 RID: 19908
	public bool CurrentIsBreak;

	// Token: 0x04004DC5 RID: 19909
	public float CurrentMotorTorquePercent;

	// Token: 0x04004DC6 RID: 19910
	public float CurrentSteeringPercent;

	// Token: 0x04004DC7 RID: 19911
	public Vector3 CurrentVelocity;

	// Token: 0x04004DC8 RID: 19912
	public string RecipeName;

	// Token: 0x04004DC9 RID: 19913
	public Material mainEmissiveMat;

	// Token: 0x04004DCA RID: 19914
	public float EffectEntityDamagePer;

	// Token: 0x04004DCB RID: 19915
	public float EffectBlockDamagePer;

	// Token: 0x04004DCC RID: 19916
	public float EffectSelfDamagePer;

	// Token: 0x04004DCD RID: 19917
	public float EffectStrongSelfDamagePer;

	// Token: 0x04004DCE RID: 19918
	public float EffectLightIntensity;

	// Token: 0x04004DCF RID: 19919
	public float EffectFuelMaxPer = 1f;

	// Token: 0x04004DD0 RID: 19920
	public float EffectFuelUsePer;

	// Token: 0x04004DD1 RID: 19921
	public float EffectMotorTorquePer;

	// Token: 0x04004DD2 RID: 19922
	public float EffectVelocityMaxPer;

	// Token: 0x04004DD3 RID: 19923
	[PublicizedFrom(EAccessModifier.Private)]
	public float effectUpdateDelay;

	// Token: 0x04004DD4 RID: 19924
	public FastTags<TagGroup.Global> ModTags;

	// Token: 0x02000CC2 RID: 3266
	public enum Event
	{
		// Token: 0x04004DD6 RID: 19926
		Start,
		// Token: 0x04004DD7 RID: 19927
		Started,
		// Token: 0x04004DD8 RID: 19928
		Stop,
		// Token: 0x04004DD9 RID: 19929
		Stopped,
		// Token: 0x04004DDA RID: 19930
		SimulationUpdate,
		// Token: 0x04004DDB RID: 19931
		HealthChanged
	}
}

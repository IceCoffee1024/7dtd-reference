using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CB7 RID: 3255
[Preserve]
public class VPEngine : VehiclePart
{
	// Token: 0x0600632B RID: 25387 RVA: 0x00273074 File Offset: 0x00271274
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		StringParsers.TryParseFloat(base.GetProperty("fuelKmPerL"), out this.fuelKmPerL);
		_properties.ParseVec("foodDrain", ref this.foodDrain, ref this.foodDrainTurbo);
		this.gears.Clear();
		for (int i = 1; i < 9; i++)
		{
			string property = base.GetProperty("gear" + i.ToString());
			if (property.Length == 0)
			{
				break;
			}
			string[] array = property.Split(',', StringSplitOptions.None);
			VPEngine.Gear gear = new VPEngine.Gear();
			this.gears.Add(gear);
			int num = 0;
			StringParsers.TryParseFloat(array[num++], out gear.rpmMin);
			StringParsers.TryParseFloat(array[num++], out gear.rpmMax);
			StringParsers.TryParseFloat(array[num++], out gear.rpmDecel);
			StringParsers.TryParseFloat(array[num++], out gear.rpmDownShiftPoint);
			StringParsers.TryParseFloat(array[num++], out gear.rpmDownShiftTo);
			StringParsers.TryParseFloat(array[num++], out gear.rpmAccel);
			StringParsers.TryParseFloat(array[num++], out gear.rpmUpShiftPoint);
			StringParsers.TryParseFloat(array[num++], out gear.rpmUpShiftTo);
			gear.accelSoundName = array[num++].Trim();
			gear.decelSoundName = array[num++].Trim();
			int num2 = (array.Length - num) / 8;
			if (num2 > 0)
			{
				gear.soundRanges = new VPEngine.SoundRange[num2];
				for (int j = 0; j < num2; j++)
				{
					VPEngine.SoundRange soundRange = new VPEngine.SoundRange();
					gear.soundRanges[j] = soundRange;
					int num3 = num + j * 8;
					StringParsers.TryParseFloat(array[num3], out soundRange.pitchMin);
					StringParsers.TryParseFloat(array[num3 + 1], out soundRange.pitchMax);
					StringParsers.TryParseFloat(array[num3 + 2], out soundRange.volumeMin);
					StringParsers.TryParseFloat(array[num3 + 3], out soundRange.volumeMax);
					StringParsers.TryParseFloat(array[num3 + 4], out soundRange.pitchFadeMin);
					StringParsers.TryParseFloat(array[num3 + 5], out soundRange.pitchFadeMax);
					StringParsers.TryParseFloat(array[num3 + 6], out soundRange.pitchFadeRange);
					soundRange.pitchFadeRange += 1E-05f;
					soundRange.name = array[num3 + 7].Trim();
				}
			}
		}
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x002732DF File Offset: 0x002714DF
	public override void InitPrefabConnections()
	{
		this.ParticleEffectUpdate();
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x002732E8 File Offset: 0x002714E8
	public override void Update(float _dt)
	{
		if (this.IsBroken())
		{
			this.stopEngine(false);
			return;
		}
		EntityAlive entityAlive = this.vehicle.entity.AttachedMainEntity as EntityAlive;
		if (entityAlive)
		{
			entityAlive.CurrentMovementTag = EntityAlive.MovementTagDriving;
			float num = 0f;
			if (this.vehicle.CurrentIsAccel)
			{
				num = this.foodDrain;
				if (this.vehicle.IsTurbo)
				{
					num = this.foodDrainTurbo;
				}
			}
			entityAlive.SetCVar("_vehicleFood", num);
		}
		if (!this.isRunning)
		{
			return;
		}
		float magnitude = this.vehicle.CurrentVelocity.magnitude;
		float num2 = _dt / (this.fuelKmPerL * 1000f);
		if (this.vehicle.IsTurbo)
		{
			num2 *= 2f;
		}
		if (this.vehicle.CurrentIsAccel)
		{
			num2 *= magnitude;
		}
		else
		{
			num2 *= this.vehicle.VelocityMaxForward * 0.1f;
		}
		num2 *= this.vehicle.EffectFuelUsePer;
		this.vehicle.FireEvent(VehiclePart.Event.FuelRemove, this, num2);
	}

	// Token: 0x0600632E RID: 25390 RVA: 0x002733EC File Offset: 0x002715EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ParticleEffectUpdate()
	{
		base.SetTransformActive("particleOn", this.isRunning);
		float healthPercentage = base.GetHealthPercentage();
		if (healthPercentage <= 0f)
		{
			base.SetTransformActive("particleDamaged", true);
			base.SetTransformActive("particleBroken", true);
			return;
		}
		base.SetTransformActive("particleBroken", false);
		if (healthPercentage <= 0.25f)
		{
			base.SetTransformActive("particleDamaged", this.isRunning);
			return;
		}
		base.SetTransformActive("particleDamaged", false);
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x00273464 File Offset: 0x00271664
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateEngineSimulation()
	{
		if (!this.isRunning)
		{
			return;
		}
		float num = 500f;
		float num2 = 5000f;
		float num3 = -2400f;
		float num4 = 2700f;
		float num5 = 2700f;
		float num6 = 5000f;
		float num7 = 1500f;
		float num8 = 2800f;
		if (this.gears.Count > 0)
		{
			VPEngine.Gear gear = this.gears[this.gearIndex];
			num = gear.rpmMin;
			num2 = gear.rpmMax;
			num3 = gear.rpmDecel;
			num4 = gear.rpmDownShiftPoint;
			num5 = gear.rpmDownShiftTo;
			num8 = gear.rpmAccel;
			num6 = gear.rpmUpShiftPoint;
			num7 = gear.rpmUpShiftTo;
		}
		if (this.vehicle.CurrentIsAccel)
		{
			this.rpm += num8 * Time.deltaTime;
			this.rpm = Mathf.Min(this.rpm, num2);
			if (this.rpm >= num6 && this.gearIndex < this.gears.Count - 1 && this.vehicle.CurrentForwardVelocity > 4f)
			{
				this.gearIndex++;
				this.rpm = num7;
				this.vehicle.entity.AddRelativeForce(new Vector3(0f, 0.2f, -2f), ForceMode.VelocityChange);
				VPEngine.Gear gear2 = this.gears[this.gearIndex];
				this.playAccelDecelSound(gear2.accelSoundName);
			}
			if (this.acceleratePhase <= 0)
			{
				if (this.gears.Count > 0)
				{
					VPEngine.Gear gear3 = this.gears[this.gearIndex];
					this.isDecelSoundPlayed = false;
					this.playAccelDecelSound(gear3.accelSoundName);
				}
				this.acceleratePhase = 1;
			}
			float num9 = (this.rpm - num) / (num2 - num);
			this.updateEngineSounds(num9);
			return;
		}
		if (this.acceleratePhase >= 0)
		{
			float num10 = num3;
			if (Mathf.Abs(this.vehicle.CurrentForwardVelocity) < 2f)
			{
				num10 *= 2f;
			}
			this.rpm += num10 * Time.deltaTime;
			if (this.rpm > num4)
			{
				float num11 = (this.rpm - num) / (num2 - num);
				this.updateEngineSounds(num11);
				return;
			}
			if (this.gears.Count > 0 && !this.isDecelSoundPlayed)
			{
				this.isDecelSoundPlayed = true;
				VPEngine.Gear gear4 = this.gears[this.gearIndex];
				this.playAccelDecelSound(gear4.decelSoundName);
			}
			if (this.gearIndex <= 0)
			{
				this.acceleratePhase = -1;
				this.updateEngineSounds(0f);
				return;
			}
			this.acceleratePhase = 0;
			this.gearIndex = 0;
			if (num5 > 0f)
			{
				this.rpm = num5;
				return;
			}
		}
		else
		{
			this.updateEngineSounds(0f);
		}
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x00273714 File Offset: 0x00271914
	public override void HandleEvent(Vehicle.Event _event, float _arg)
	{
		switch (_event)
		{
		case Vehicle.Event.Start:
			if (!this.IsBroken())
			{
				this.startEngine();
				return;
			}
			break;
		case Vehicle.Event.Started:
		case Vehicle.Event.Stopped:
			break;
		case Vehicle.Event.Stop:
		{
			EntityAlive entityAlive = this.vehicle.entity.AttachedMainEntity as EntityAlive;
			if (entityAlive)
			{
				entityAlive.SetCVar("_vehicleFood", 0f);
			}
			this.stopEngine(false);
			return;
		}
		case Vehicle.Event.SimulationUpdate:
			if (!this.IsBroken())
			{
				this.updateEngineSimulation();
				return;
			}
			break;
		case Vehicle.Event.HealthChanged:
			this.ParticleEffectUpdate();
			break;
		default:
			return;
		}
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x0027379A File Offset: 0x0027199A
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _arg)
	{
		if (_event == VehiclePart.Event.FuelEmpty)
		{
			this.stopEngine(true);
		}
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x002737A8 File Offset: 0x002719A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void startEngine()
	{
		if (this.isRunning)
		{
			return;
		}
		this.isRunning = true;
		if (this.vehicle.GetFuelLevel() > 0f || EntityVehicle.VehicleFuelUsageModifier == 0f)
		{
			this.playSound(this.properties.GetValue("sound_start"));
			this.gearIndex = 0;
			this.updateEngineSounds(0f);
		}
		this.vehicle.entity.IsEngineRunning = true;
		this.vehicle.FireEvent(Vehicle.Event.Started);
		this.ParticleEffectUpdate();
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x00273830 File Offset: 0x00271A30
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopEngine(bool _outOfFuel = false)
	{
		if (!this.isRunning)
		{
			return;
		}
		this.isRunning = false;
		this.stopEngineSounds();
		if (!_outOfFuel)
		{
			this.playSound(this.properties.GetValue("sound_shut_off"));
		}
		else
		{
			this.playSound(this.properties.GetValue("sound_no_fuel_shut_off"));
		}
		this.vehicle.entity.IsEngineRunning = false;
		this.vehicle.FireEvent(Vehicle.Event.Stopped);
		this.ParticleEffectUpdate();
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x002738A7 File Offset: 0x00271AA7
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x002738E7 File Offset: 0x00271AE7
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopSound(string _sound)
	{
		if (this.vehicle.entity && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.StopOneShot(_sound);
		}
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x0027391E File Offset: 0x00271B1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void changeSoundLoop(string soundName, ref Handle handle)
	{
		this.stopSoundLoop(ref handle);
		this.playSoundLoop(soundName, ref handle);
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x0027392F File Offset: 0x00271B2F
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSoundLoop(string soundName, ref Handle handle)
	{
		if (handle != null)
		{
			return;
		}
		if (this.vehicle.entity)
		{
			handle = Manager.Play(this.vehicle.entity, soundName, 1f, true);
		}
	}

	// Token: 0x06006338 RID: 25400 RVA: 0x00273961 File Offset: 0x00271B61
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopSoundLoop(ref Handle handle)
	{
		if (handle != null)
		{
			handle.Stop(this.vehicle.entity.entityId);
			handle = null;
		}
	}

	// Token: 0x06006339 RID: 25401 RVA: 0x00273981 File Offset: 0x00271B81
	[PublicizedFrom(EAccessModifier.Private)]
	public void playAccelDecelSound(string name)
	{
		if (this.accelDecelSoundName != null)
		{
			this.stopSound(this.accelDecelSoundName);
		}
		if (name != null && name.Length > 0)
		{
			this.playSound(name);
		}
		this.accelDecelSoundName = name;
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x002739B4 File Offset: 0x00271BB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateEngineSounds(float rpmPercent)
	{
		if (this.gears.Count > 0)
		{
			VPEngine.Gear gear;
			for (int i = 0; i < this.gears.Count; i++)
			{
				if (i != this.gearIndex)
				{
					gear = this.gears[i];
					for (int j = 0; j < gear.soundRanges.Length; j++)
					{
						VPEngine.SoundRange soundRange = gear.soundRanges[j];
						if (soundRange.soundHandle != null)
						{
							this.stopSoundLoop(ref soundRange.soundHandle);
						}
					}
				}
			}
			float deltaTime = Time.deltaTime;
			this.pitchRandTime -= deltaTime;
			if (this.pitchRandTime <= 0f)
			{
				this.pitchRandTime = 0.75f;
				this.pitchRand = this.vehicle.entity.rand.RandomRange(-1f, 1f) * 0.03f;
			}
			float num = this.pitchRand;
			if (rpmPercent > 0f && this.vehicle.IsTurbo)
			{
				num += 0.2f;
				if (!this.isTurbo)
				{
					this.playSound("vehicle_turbo");
				}
			}
			this.isTurbo = rpmPercent > 0f && this.vehicle.IsTurbo;
			this.pitchAdd = Mathf.MoveTowards(this.pitchAdd, num, deltaTime * 0.15f);
			gear = this.gears[this.gearIndex];
			for (int k = 0; k < gear.soundRanges.Length; k++)
			{
				VPEngine.SoundRange soundRange2 = gear.soundRanges[k];
				float num2 = Mathf.Lerp(soundRange2.pitchMin, soundRange2.pitchMax, rpmPercent);
				float num3 = Mathf.Lerp(soundRange2.volumeMin, soundRange2.volumeMax, rpmPercent);
				float num4 = 1f;
				float num5 = soundRange2.pitchFadeMin - num2;
				if (num5 > 0f)
				{
					num4 = Mathf.Lerp(1f, 0f, num5 / soundRange2.pitchFadeRange);
				}
				else
				{
					float num6 = num2 - soundRange2.pitchFadeMax;
					if (num6 > 0f)
					{
						num4 = Mathf.Lerp(1f, 0f, num6 / soundRange2.pitchFadeRange);
					}
				}
				float num7 = num3 * num4;
				if (num7 < 0.01f)
				{
					if (soundRange2.soundHandle != null)
					{
						this.stopSoundLoop(ref soundRange2.soundHandle);
					}
				}
				else
				{
					if (soundRange2.soundHandle == null)
					{
						this.playSoundLoop(soundRange2.name, ref soundRange2.soundHandle);
					}
					if (soundRange2.soundHandle != null)
					{
						soundRange2.soundHandle.SetPitch(num2 + this.pitchAdd);
						soundRange2.soundHandle.SetVolume(num7);
					}
				}
			}
		}
	}

	// Token: 0x0600633B RID: 25403 RVA: 0x00273C38 File Offset: 0x00271E38
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopEngineSounds()
	{
		if (this.gears.Count > 0)
		{
			for (int i = 0; i < this.gears.Count; i++)
			{
				VPEngine.Gear gear = this.gears[i];
				for (int j = 0; j < gear.soundRanges.Length; j++)
				{
					VPEngine.SoundRange soundRange = gear.soundRanges[j];
					if (soundRange.soundHandle != null)
					{
						this.stopSoundLoop(ref soundRange.soundHandle);
					}
				}
			}
		}
		this.playAccelDecelSound(null);
	}

	// Token: 0x04004D4E RID: 19790
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cIdleFuelPercent = 0.1f;

	// Token: 0x04004D4F RID: 19791
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTurboFuelPercent = 2f;

	// Token: 0x04004D50 RID: 19792
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelKmPerL;

	// Token: 0x04004D51 RID: 19793
	[PublicizedFrom(EAccessModifier.Private)]
	public float foodDrain;

	// Token: 0x04004D52 RID: 19794
	[PublicizedFrom(EAccessModifier.Private)]
	public float foodDrainTurbo;

	// Token: 0x04004D53 RID: 19795
	public bool isRunning;

	// Token: 0x04004D54 RID: 19796
	[PublicizedFrom(EAccessModifier.Private)]
	public int acceleratePhase;

	// Token: 0x04004D55 RID: 19797
	[PublicizedFrom(EAccessModifier.Private)]
	public float rpm;

	// Token: 0x04004D56 RID: 19798
	[PublicizedFrom(EAccessModifier.Private)]
	public int gearIndex;

	// Token: 0x04004D57 RID: 19799
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecelSoundPlayed;

	// Token: 0x04004D58 RID: 19800
	[PublicizedFrom(EAccessModifier.Private)]
	public string accelDecelSoundName;

	// Token: 0x04004D59 RID: 19801
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchRandTime;

	// Token: 0x04004D5A RID: 19802
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchRand;

	// Token: 0x04004D5B RID: 19803
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchAdd;

	// Token: 0x04004D5C RID: 19804
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTurbo;

	// Token: 0x04004D5D RID: 19805
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VPEngine.Gear> gears = new List<VPEngine.Gear>();

	// Token: 0x02000CB8 RID: 3256
	[PublicizedFrom(EAccessModifier.Private)]
	public class Gear
	{
		// Token: 0x04004D5E RID: 19806
		public float rpmMin;

		// Token: 0x04004D5F RID: 19807
		public float rpmMax;

		// Token: 0x04004D60 RID: 19808
		public float rpmDecel;

		// Token: 0x04004D61 RID: 19809
		public float rpmAccel;

		// Token: 0x04004D62 RID: 19810
		public float rpmDownShiftPoint;

		// Token: 0x04004D63 RID: 19811
		public float rpmUpShiftPoint;

		// Token: 0x04004D64 RID: 19812
		public float rpmDownShiftTo;

		// Token: 0x04004D65 RID: 19813
		public float rpmUpShiftTo;

		// Token: 0x04004D66 RID: 19814
		public string accelSoundName;

		// Token: 0x04004D67 RID: 19815
		public string decelSoundName;

		// Token: 0x04004D68 RID: 19816
		public VPEngine.SoundRange[] soundRanges;
	}

	// Token: 0x02000CB9 RID: 3257
	[PublicizedFrom(EAccessModifier.Private)]
	public class SoundRange
	{
		// Token: 0x04004D69 RID: 19817
		public float pitchMin;

		// Token: 0x04004D6A RID: 19818
		public float pitchMax;

		// Token: 0x04004D6B RID: 19819
		public float volumeMin;

		// Token: 0x04004D6C RID: 19820
		public float volumeMax;

		// Token: 0x04004D6D RID: 19821
		public float pitchFadeMin;

		// Token: 0x04004D6E RID: 19822
		public float pitchFadeMax;

		// Token: 0x04004D6F RID: 19823
		public float pitchFadeRange;

		// Token: 0x04004D70 RID: 19824
		public string name;

		// Token: 0x04004D71 RID: 19825
		public Handle soundHandle;
	}
}

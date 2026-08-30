using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C9E RID: 3230
[Preserve]
public class VPEngine : VehiclePart
{
	// Token: 0x06006313 RID: 25363 RVA: 0x0026E708 File Offset: 0x0026C908
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

	// Token: 0x06006314 RID: 25364 RVA: 0x0026E973 File Offset: 0x0026CB73
	public override void InitPrefabConnections()
	{
		this.ParticleEffectUpdate();
	}

	// Token: 0x06006315 RID: 25365 RVA: 0x0026E97C File Offset: 0x0026CB7C
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

	// Token: 0x06006316 RID: 25366 RVA: 0x0026EA80 File Offset: 0x0026CC80
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

	// Token: 0x06006317 RID: 25367 RVA: 0x0026EAF8 File Offset: 0x0026CCF8
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

	// Token: 0x06006318 RID: 25368 RVA: 0x0026EDA8 File Offset: 0x0026CFA8
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

	// Token: 0x06006319 RID: 25369 RVA: 0x0026EE2E File Offset: 0x0026D02E
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _arg)
	{
		if (_event == VehiclePart.Event.FuelEmpty)
		{
			this.stopEngine(true);
		}
	}

	// Token: 0x0600631A RID: 25370 RVA: 0x0026EE3C File Offset: 0x0026D03C
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

	// Token: 0x0600631B RID: 25371 RVA: 0x0026EEC4 File Offset: 0x0026D0C4
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

	// Token: 0x0600631C RID: 25372 RVA: 0x0026EF3B File Offset: 0x0026D13B
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x0600631D RID: 25373 RVA: 0x0026EF7B File Offset: 0x0026D17B
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopSound(string _sound)
	{
		if (this.vehicle.entity && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.StopOneShot(_sound);
		}
	}

	// Token: 0x0600631E RID: 25374 RVA: 0x0026EFB2 File Offset: 0x0026D1B2
	[PublicizedFrom(EAccessModifier.Private)]
	public void changeSoundLoop(string soundName, ref Handle handle)
	{
		this.stopSoundLoop(ref handle);
		this.playSoundLoop(soundName, ref handle);
	}

	// Token: 0x0600631F RID: 25375 RVA: 0x0026EFC3 File Offset: 0x0026D1C3
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

	// Token: 0x06006320 RID: 25376 RVA: 0x0026EFF5 File Offset: 0x0026D1F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopSoundLoop(ref Handle handle)
	{
		if (handle != null)
		{
			handle.Stop(this.vehicle.entity.entityId);
			handle = null;
		}
	}

	// Token: 0x06006321 RID: 25377 RVA: 0x0026F015 File Offset: 0x0026D215
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

	// Token: 0x06006322 RID: 25378 RVA: 0x0026F048 File Offset: 0x0026D248
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

	// Token: 0x06006323 RID: 25379 RVA: 0x0026F2CC File Offset: 0x0026D4CC
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

	// Token: 0x04004CF3 RID: 19699
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cIdleFuelPercent = 0.1f;

	// Token: 0x04004CF4 RID: 19700
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTurboFuelPercent = 2f;

	// Token: 0x04004CF5 RID: 19701
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelKmPerL;

	// Token: 0x04004CF6 RID: 19702
	[PublicizedFrom(EAccessModifier.Private)]
	public float foodDrain;

	// Token: 0x04004CF7 RID: 19703
	[PublicizedFrom(EAccessModifier.Private)]
	public float foodDrainTurbo;

	// Token: 0x04004CF8 RID: 19704
	public bool isRunning;

	// Token: 0x04004CF9 RID: 19705
	[PublicizedFrom(EAccessModifier.Private)]
	public int acceleratePhase;

	// Token: 0x04004CFA RID: 19706
	[PublicizedFrom(EAccessModifier.Private)]
	public float rpm;

	// Token: 0x04004CFB RID: 19707
	[PublicizedFrom(EAccessModifier.Private)]
	public int gearIndex;

	// Token: 0x04004CFC RID: 19708
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecelSoundPlayed;

	// Token: 0x04004CFD RID: 19709
	[PublicizedFrom(EAccessModifier.Private)]
	public string accelDecelSoundName;

	// Token: 0x04004CFE RID: 19710
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchRandTime;

	// Token: 0x04004CFF RID: 19711
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchRand;

	// Token: 0x04004D00 RID: 19712
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchAdd;

	// Token: 0x04004D01 RID: 19713
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTurbo;

	// Token: 0x04004D02 RID: 19714
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VPEngine.Gear> gears = new List<VPEngine.Gear>();

	// Token: 0x02000C9F RID: 3231
	[PublicizedFrom(EAccessModifier.Private)]
	public class Gear
	{
		// Token: 0x04004D03 RID: 19715
		public float rpmMin;

		// Token: 0x04004D04 RID: 19716
		public float rpmMax;

		// Token: 0x04004D05 RID: 19717
		public float rpmDecel;

		// Token: 0x04004D06 RID: 19718
		public float rpmAccel;

		// Token: 0x04004D07 RID: 19719
		public float rpmDownShiftPoint;

		// Token: 0x04004D08 RID: 19720
		public float rpmUpShiftPoint;

		// Token: 0x04004D09 RID: 19721
		public float rpmDownShiftTo;

		// Token: 0x04004D0A RID: 19722
		public float rpmUpShiftTo;

		// Token: 0x04004D0B RID: 19723
		public string accelSoundName;

		// Token: 0x04004D0C RID: 19724
		public string decelSoundName;

		// Token: 0x04004D0D RID: 19725
		public VPEngine.SoundRange[] soundRanges;
	}

	// Token: 0x02000CA0 RID: 3232
	[PublicizedFrom(EAccessModifier.Private)]
	public class SoundRange
	{
		// Token: 0x04004D0E RID: 19726
		public float pitchMin;

		// Token: 0x04004D0F RID: 19727
		public float pitchMax;

		// Token: 0x04004D10 RID: 19728
		public float volumeMin;

		// Token: 0x04004D11 RID: 19729
		public float volumeMax;

		// Token: 0x04004D12 RID: 19730
		public float pitchFadeMin;

		// Token: 0x04004D13 RID: 19731
		public float pitchFadeMax;

		// Token: 0x04004D14 RID: 19732
		public float pitchFadeRange;

		// Token: 0x04004D15 RID: 19733
		public string name;

		// Token: 0x04004D16 RID: 19734
		public Handle soundHandle;
	}
}

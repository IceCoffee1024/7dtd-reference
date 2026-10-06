using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CAC RID: 3244
[Preserve]
public class VPEngine : VehiclePart
{
	// Token: 0x06006306 RID: 25350 RVA: 0x00271B4C File Offset: 0x0026FD4C
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

	// Token: 0x06006307 RID: 25351 RVA: 0x00271DB7 File Offset: 0x0026FFB7
	public override void InitPrefabConnections()
	{
		this.ParticleEffectUpdate();
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x00271DC0 File Offset: 0x0026FFC0
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

	// Token: 0x06006309 RID: 25353 RVA: 0x00271EC4 File Offset: 0x002700C4
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

	// Token: 0x0600630A RID: 25354 RVA: 0x00271F3C File Offset: 0x0027013C
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

	// Token: 0x0600630B RID: 25355 RVA: 0x002721EC File Offset: 0x002703EC
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

	// Token: 0x0600630C RID: 25356 RVA: 0x00272272 File Offset: 0x00270472
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _arg)
	{
		if (_event == VehiclePart.Event.FuelEmpty)
		{
			this.stopEngine(true);
		}
	}

	// Token: 0x0600630D RID: 25357 RVA: 0x00272280 File Offset: 0x00270480
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

	// Token: 0x0600630E RID: 25358 RVA: 0x00272308 File Offset: 0x00270508
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

	// Token: 0x0600630F RID: 25359 RVA: 0x0027237F File Offset: 0x0027057F
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x06006310 RID: 25360 RVA: 0x002723BF File Offset: 0x002705BF
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopSound(string _sound)
	{
		if (this.vehicle.entity && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.StopOneShot(_sound);
		}
	}

	// Token: 0x06006311 RID: 25361 RVA: 0x002723F6 File Offset: 0x002705F6
	[PublicizedFrom(EAccessModifier.Private)]
	public void changeSoundLoop(string soundName, ref Handle handle)
	{
		this.stopSoundLoop(ref handle);
		this.playSoundLoop(soundName, ref handle);
	}

	// Token: 0x06006312 RID: 25362 RVA: 0x00272407 File Offset: 0x00270607
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

	// Token: 0x06006313 RID: 25363 RVA: 0x00272439 File Offset: 0x00270639
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopSoundLoop(ref Handle handle)
	{
		if (handle != null)
		{
			handle.Stop(this.vehicle.entity.entityId);
			handle = null;
		}
	}

	// Token: 0x06006314 RID: 25364 RVA: 0x00272459 File Offset: 0x00270659
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

	// Token: 0x06006315 RID: 25365 RVA: 0x0027248C File Offset: 0x0027068C
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

	// Token: 0x06006316 RID: 25366 RVA: 0x00272710 File Offset: 0x00270910
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

	// Token: 0x04004D29 RID: 19753
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cIdleFuelPercent = 0.1f;

	// Token: 0x04004D2A RID: 19754
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTurboFuelPercent = 2f;

	// Token: 0x04004D2B RID: 19755
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelKmPerL;

	// Token: 0x04004D2C RID: 19756
	[PublicizedFrom(EAccessModifier.Private)]
	public float foodDrain;

	// Token: 0x04004D2D RID: 19757
	[PublicizedFrom(EAccessModifier.Private)]
	public float foodDrainTurbo;

	// Token: 0x04004D2E RID: 19758
	public bool isRunning;

	// Token: 0x04004D2F RID: 19759
	[PublicizedFrom(EAccessModifier.Private)]
	public int acceleratePhase;

	// Token: 0x04004D30 RID: 19760
	[PublicizedFrom(EAccessModifier.Private)]
	public float rpm;

	// Token: 0x04004D31 RID: 19761
	[PublicizedFrom(EAccessModifier.Private)]
	public int gearIndex;

	// Token: 0x04004D32 RID: 19762
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecelSoundPlayed;

	// Token: 0x04004D33 RID: 19763
	[PublicizedFrom(EAccessModifier.Private)]
	public string accelDecelSoundName;

	// Token: 0x04004D34 RID: 19764
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchRandTime;

	// Token: 0x04004D35 RID: 19765
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchRand;

	// Token: 0x04004D36 RID: 19766
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitchAdd;

	// Token: 0x04004D37 RID: 19767
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTurbo;

	// Token: 0x04004D38 RID: 19768
	[PublicizedFrom(EAccessModifier.Private)]
	public List<VPEngine.Gear> gears = new List<VPEngine.Gear>();

	// Token: 0x02000CAD RID: 3245
	[PublicizedFrom(EAccessModifier.Private)]
	public class Gear
	{
		// Token: 0x04004D39 RID: 19769
		public float rpmMin;

		// Token: 0x04004D3A RID: 19770
		public float rpmMax;

		// Token: 0x04004D3B RID: 19771
		public float rpmDecel;

		// Token: 0x04004D3C RID: 19772
		public float rpmAccel;

		// Token: 0x04004D3D RID: 19773
		public float rpmDownShiftPoint;

		// Token: 0x04004D3E RID: 19774
		public float rpmUpShiftPoint;

		// Token: 0x04004D3F RID: 19775
		public float rpmDownShiftTo;

		// Token: 0x04004D40 RID: 19776
		public float rpmUpShiftTo;

		// Token: 0x04004D41 RID: 19777
		public string accelSoundName;

		// Token: 0x04004D42 RID: 19778
		public string decelSoundName;

		// Token: 0x04004D43 RID: 19779
		public VPEngine.SoundRange[] soundRanges;
	}

	// Token: 0x02000CAE RID: 3246
	[PublicizedFrom(EAccessModifier.Private)]
	public class SoundRange
	{
		// Token: 0x04004D44 RID: 19780
		public float pitchMin;

		// Token: 0x04004D45 RID: 19781
		public float pitchMax;

		// Token: 0x04004D46 RID: 19782
		public float volumeMin;

		// Token: 0x04004D47 RID: 19783
		public float volumeMax;

		// Token: 0x04004D48 RID: 19784
		public float pitchFadeMin;

		// Token: 0x04004D49 RID: 19785
		public float pitchFadeMax;

		// Token: 0x04004D4A RID: 19786
		public float pitchFadeRange;

		// Token: 0x04004D4B RID: 19787
		public string name;

		// Token: 0x04004D4C RID: 19788
		public Handle soundHandle;
	}
}

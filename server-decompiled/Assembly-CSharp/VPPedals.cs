using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CBC RID: 3260
[Preserve]
public class VPPedals : VehiclePart
{
	// Token: 0x06006354 RID: 25428 RVA: 0x002743C0 File Offset: 0x002725C0
	public override void InitPrefabConnections()
	{
		this.initPedal("L", 0);
		this.initPedal("R", 1);
		this.ParticleEffectUpdate();
	}

	// Token: 0x06006355 RID: 25429 RVA: 0x002743E0 File Offset: 0x002725E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void initPedal(string name, int index)
	{
		this.crankT = base.GetTransform();
		Transform transform = this.crankT.Find("Pedal" + name);
		this.pedalTs[index] = transform;
		base.InitIKTarget(AvatarIKGoal.LeftFoot + index, transform);
	}

	// Token: 0x06006356 RID: 25430 RVA: 0x00274423 File Offset: 0x00272623
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		_properties.ParseString("pedalSound", ref this.pedalSoundName);
		_properties.ParseVec("staminaDrain", ref this.staminaDrain, ref this.staminaDrainTurbo);
	}

	// Token: 0x06006357 RID: 25431 RVA: 0x00274454 File Offset: 0x00272654
	public override void Update(float deltaTime)
	{
		if (!this.vehicle.entity.HasDriver)
		{
			return;
		}
		EntityAlive entityAlive = this.vehicle.entity.AttachedMainEntity as EntityAlive;
		if (!entityAlive)
		{
			return;
		}
		float currentMotorTorquePercent = this.vehicle.CurrentMotorTorquePercent;
		float currentForwardVelocity = this.vehicle.CurrentForwardVelocity;
		if (currentMotorTorquePercent > 0f)
		{
			if (currentForwardVelocity > 0f)
			{
				this.rotSpeed += deltaTime * 10f * currentForwardVelocity;
				this.didPedal = true;
				this.didRun |= this.vehicle.IsTurbo;
			}
			this.backPedalTime = 0f;
		}
		else if (global::UnityEngine.Random.value < 0.3f * deltaTime)
		{
			this.backPedalTime = global::UnityEngine.Random.value * 1.2f;
		}
		entityAlive.CurrentMovementTag = (this.didRun ? EntityAlive.MovementTagRunning : EntityAlive.MovementTagIdle);
		this.staminaCheckTime += deltaTime;
		if (this.staminaCheckTime >= 0.2f)
		{
			this.staminaCheckTime = 0f;
			if (this.didPedal)
			{
				float num = (this.didRun ? this.staminaDrainTurbo : this.staminaDrain);
				entityAlive.AddStamina(-num * 0.2f);
				this.didPedal = false;
				this.didRun = false;
			}
		}
		if (currentForwardVelocity != 0f && this.backPedalTime > 0f)
		{
			this.backPedalTime -= deltaTime;
			this.rotSpeed += -15f * deltaTime;
		}
		this.rotSpeed *= 0.8f;
		if (Mathf.Abs(this.rotSpeed) > 0.1f)
		{
			this.rot += this.rotSpeed;
			this.crankT.localEulerAngles = new Vector3(this.rot, 0f, 0f);
			Quaternion quaternion = Quaternion.Inverse(this.crankT.localRotation);
			for (int i = 0; i < this.pedalTs.Length; i++)
			{
				this.pedalTs[i].localRotation = quaternion;
			}
			if (this.rotSpeed > 1f)
			{
				this.pedalSoundTime += deltaTime;
				float num2 = (this.vehicle.IsTurbo ? 0.55f : 0.75f);
				if (this.pedalSoundTime > num2)
				{
					this.playSound(this.pedalSoundName);
					this.pedalSoundTime = 0f;
				}
			}
		}
		if (entityAlive.Stamina < 1f)
		{
			this.staminaCooldownDelay = 2f;
		}
		if (this.staminaCooldownDelay > 0f)
		{
			this.staminaCooldownDelay -= deltaTime;
			this.vehicle.CanTurbo = false;
			return;
		}
		this.vehicle.CanTurbo = true;
	}

	// Token: 0x06006358 RID: 25432 RVA: 0x00274700 File Offset: 0x00272900
	public override void HandleEvent(Vehicle.Event _event, float _arg)
	{
		if (_event == Vehicle.Event.Start || _event == Vehicle.Event.Stop || _event == Vehicle.Event.HealthChanged)
		{
			this.ParticleEffectUpdate();
		}
	}

	// Token: 0x06006359 RID: 25433 RVA: 0x00274714 File Offset: 0x00272914
	[PublicizedFrom(EAccessModifier.Private)]
	public void ParticleEffectUpdate()
	{
		float healthPercentage = base.GetHealthPercentage();
		if (healthPercentage <= 0f)
		{
			base.SetTransformActive("chain", false);
			base.SetTransformActive("particleDamaged", true);
			base.SetTransformActive("particleBroken", true);
			return;
		}
		base.SetTransformActive("chain", true);
		base.SetTransformActive("particleBroken", false);
		if (healthPercentage <= 0.25f)
		{
			base.SetTransformActive("particleDamaged", this.vehicle.entity.HasDriver);
			return;
		}
		base.SetTransformActive("particleDamaged", false);
	}

	// Token: 0x0600635A RID: 25434 RVA: 0x002747A0 File Offset: 0x002729A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity != null && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x04004D84 RID: 19844
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform crankT;

	// Token: 0x04004D85 RID: 19845
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform[] pedalTs = new Transform[2];

	// Token: 0x04004D86 RID: 19846
	[PublicizedFrom(EAccessModifier.Private)]
	public float rot;

	// Token: 0x04004D87 RID: 19847
	[PublicizedFrom(EAccessModifier.Private)]
	public float rotSpeed;

	// Token: 0x04004D88 RID: 19848
	[PublicizedFrom(EAccessModifier.Private)]
	public float backPedalTime;

	// Token: 0x04004D89 RID: 19849
	[PublicizedFrom(EAccessModifier.Private)]
	public string pedalSoundName;

	// Token: 0x04004D8A RID: 19850
	[PublicizedFrom(EAccessModifier.Private)]
	public float pedalSoundTime;

	// Token: 0x04004D8B RID: 19851
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didPedal;

	// Token: 0x04004D8C RID: 19852
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didRun;

	// Token: 0x04004D8D RID: 19853
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCheckTime;

	// Token: 0x04004D8E RID: 19854
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrain;

	// Token: 0x04004D8F RID: 19855
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrainTurbo;

	// Token: 0x04004D90 RID: 19856
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCooldownDelay;
}

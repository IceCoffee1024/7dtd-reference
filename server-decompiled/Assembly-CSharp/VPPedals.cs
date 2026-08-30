using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CA3 RID: 3235
[Preserve]
public class VPPedals : VehiclePart
{
	// Token: 0x0600633C RID: 25404 RVA: 0x0026FA54 File Offset: 0x0026DC54
	public override void InitPrefabConnections()
	{
		this.initPedal("L", 0);
		this.initPedal("R", 1);
		this.ParticleEffectUpdate();
	}

	// Token: 0x0600633D RID: 25405 RVA: 0x0026FA74 File Offset: 0x0026DC74
	[PublicizedFrom(EAccessModifier.Private)]
	public void initPedal(string name, int index)
	{
		this.crankT = base.GetTransform();
		Transform transform = this.crankT.Find("Pedal" + name);
		this.pedalTs[index] = transform;
		base.InitIKTarget(AvatarIKGoal.LeftFoot + index, transform);
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x0026FAB7 File Offset: 0x0026DCB7
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		_properties.ParseString("pedalSound", ref this.pedalSoundName);
		_properties.ParseVec("staminaDrain", ref this.staminaDrain, ref this.staminaDrainTurbo);
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x0026FAE8 File Offset: 0x0026DCE8
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

	// Token: 0x06006340 RID: 25408 RVA: 0x0026FD94 File Offset: 0x0026DF94
	public override void HandleEvent(Vehicle.Event _event, float _arg)
	{
		if (_event == Vehicle.Event.Start || _event == Vehicle.Event.Stop || _event == Vehicle.Event.HealthChanged)
		{
			this.ParticleEffectUpdate();
		}
	}

	// Token: 0x06006341 RID: 25409 RVA: 0x0026FDA8 File Offset: 0x0026DFA8
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

	// Token: 0x06006342 RID: 25410 RVA: 0x0026FE34 File Offset: 0x0026E034
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity != null && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x04004D29 RID: 19753
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform crankT;

	// Token: 0x04004D2A RID: 19754
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform[] pedalTs = new Transform[2];

	// Token: 0x04004D2B RID: 19755
	[PublicizedFrom(EAccessModifier.Private)]
	public float rot;

	// Token: 0x04004D2C RID: 19756
	[PublicizedFrom(EAccessModifier.Private)]
	public float rotSpeed;

	// Token: 0x04004D2D RID: 19757
	[PublicizedFrom(EAccessModifier.Private)]
	public float backPedalTime;

	// Token: 0x04004D2E RID: 19758
	[PublicizedFrom(EAccessModifier.Private)]
	public string pedalSoundName;

	// Token: 0x04004D2F RID: 19759
	[PublicizedFrom(EAccessModifier.Private)]
	public float pedalSoundTime;

	// Token: 0x04004D30 RID: 19760
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didPedal;

	// Token: 0x04004D31 RID: 19761
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didRun;

	// Token: 0x04004D32 RID: 19762
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCheckTime;

	// Token: 0x04004D33 RID: 19763
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrain;

	// Token: 0x04004D34 RID: 19764
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrainTurbo;

	// Token: 0x04004D35 RID: 19765
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCooldownDelay;
}

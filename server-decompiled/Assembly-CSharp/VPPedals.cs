using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C91 RID: 3217
[Preserve]
public class VPPedals : VehiclePart
{
	// Token: 0x060062B1 RID: 25265 RVA: 0x0026CB7C File Offset: 0x0026AD7C
	public override void InitPrefabConnections()
	{
		this.initPedal("L", 0);
		this.initPedal("R", 1);
		this.ParticleEffectUpdate();
	}

	// Token: 0x060062B2 RID: 25266 RVA: 0x0026CB9C File Offset: 0x0026AD9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void initPedal(string name, int index)
	{
		this.crankT = base.GetTransform();
		Transform transform = this.crankT.Find("Pedal" + name);
		this.pedalTs[index] = transform;
		base.InitIKTarget(AvatarIKGoal.LeftFoot + index, transform);
	}

	// Token: 0x060062B3 RID: 25267 RVA: 0x0026CBDF File Offset: 0x0026ADDF
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		_properties.ParseString("pedalSound", ref this.pedalSoundName);
		_properties.ParseVec("staminaDrain", ref this.staminaDrain, ref this.staminaDrainTurbo);
	}

	// Token: 0x060062B4 RID: 25268 RVA: 0x0026CC10 File Offset: 0x0026AE10
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

	// Token: 0x060062B5 RID: 25269 RVA: 0x0026CEBC File Offset: 0x0026B0BC
	public override void HandleEvent(Vehicle.Event _event, float _arg)
	{
		if (_event == Vehicle.Event.Start || _event == Vehicle.Event.Stop || _event == Vehicle.Event.HealthChanged)
		{
			this.ParticleEffectUpdate();
		}
	}

	// Token: 0x060062B6 RID: 25270 RVA: 0x0026CED0 File Offset: 0x0026B0D0
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

	// Token: 0x060062B7 RID: 25271 RVA: 0x0026CF5C File Offset: 0x0026B15C
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity != null && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x04004C8C RID: 19596
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform crankT;

	// Token: 0x04004C8D RID: 19597
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform[] pedalTs = new Transform[2];

	// Token: 0x04004C8E RID: 19598
	[PublicizedFrom(EAccessModifier.Private)]
	public float rot;

	// Token: 0x04004C8F RID: 19599
	[PublicizedFrom(EAccessModifier.Private)]
	public float rotSpeed;

	// Token: 0x04004C90 RID: 19600
	[PublicizedFrom(EAccessModifier.Private)]
	public float backPedalTime;

	// Token: 0x04004C91 RID: 19601
	[PublicizedFrom(EAccessModifier.Private)]
	public string pedalSoundName;

	// Token: 0x04004C92 RID: 19602
	[PublicizedFrom(EAccessModifier.Private)]
	public float pedalSoundTime;

	// Token: 0x04004C93 RID: 19603
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didPedal;

	// Token: 0x04004C94 RID: 19604
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didRun;

	// Token: 0x04004C95 RID: 19605
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCheckTime;

	// Token: 0x04004C96 RID: 19606
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrain;

	// Token: 0x04004C97 RID: 19607
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrainTurbo;

	// Token: 0x04004C98 RID: 19608
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCooldownDelay;
}

using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CB1 RID: 3249
[Preserve]
public class VPPedals : VehiclePart
{
	// Token: 0x0600632F RID: 25391 RVA: 0x00272E98 File Offset: 0x00271098
	public override void InitPrefabConnections()
	{
		this.initPedal("L", 0);
		this.initPedal("R", 1);
		this.ParticleEffectUpdate();
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x00272EB8 File Offset: 0x002710B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void initPedal(string name, int index)
	{
		this.crankT = base.GetTransform();
		Transform transform = this.crankT.Find("Pedal" + name);
		this.pedalTs[index] = transform;
		base.InitIKTarget(AvatarIKGoal.LeftFoot + index, transform);
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x00272EFB File Offset: 0x002710FB
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		_properties.ParseString("pedalSound", ref this.pedalSoundName);
		_properties.ParseVec("staminaDrain", ref this.staminaDrain, ref this.staminaDrainTurbo);
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x00272F2C File Offset: 0x0027112C
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

	// Token: 0x06006333 RID: 25395 RVA: 0x002731D8 File Offset: 0x002713D8
	public override void HandleEvent(Vehicle.Event _event, float _arg)
	{
		if (_event == Vehicle.Event.Start || _event == Vehicle.Event.Stop || _event == Vehicle.Event.HealthChanged)
		{
			this.ParticleEffectUpdate();
		}
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x002731EC File Offset: 0x002713EC
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

	// Token: 0x06006335 RID: 25397 RVA: 0x00273278 File Offset: 0x00271478
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string _sound)
	{
		if (this.vehicle.entity != null && !this.vehicle.entity.isEntityRemote)
		{
			this.vehicle.entity.PlayOneShot(_sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x04004D5F RID: 19807
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform crankT;

	// Token: 0x04004D60 RID: 19808
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform[] pedalTs = new Transform[2];

	// Token: 0x04004D61 RID: 19809
	[PublicizedFrom(EAccessModifier.Private)]
	public float rot;

	// Token: 0x04004D62 RID: 19810
	[PublicizedFrom(EAccessModifier.Private)]
	public float rotSpeed;

	// Token: 0x04004D63 RID: 19811
	[PublicizedFrom(EAccessModifier.Private)]
	public float backPedalTime;

	// Token: 0x04004D64 RID: 19812
	[PublicizedFrom(EAccessModifier.Private)]
	public string pedalSoundName;

	// Token: 0x04004D65 RID: 19813
	[PublicizedFrom(EAccessModifier.Private)]
	public float pedalSoundTime;

	// Token: 0x04004D66 RID: 19814
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didPedal;

	// Token: 0x04004D67 RID: 19815
	[PublicizedFrom(EAccessModifier.Private)]
	public bool didRun;

	// Token: 0x04004D68 RID: 19816
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCheckTime;

	// Token: 0x04004D69 RID: 19817
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrain;

	// Token: 0x04004D6A RID: 19818
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaDrainTurbo;

	// Token: 0x04004D6B RID: 19819
	[PublicizedFrom(EAccessModifier.Private)]
	public float staminaCooldownDelay;
}

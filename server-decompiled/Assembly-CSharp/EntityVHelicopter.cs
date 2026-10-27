using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004F9 RID: 1273
[Preserve]
public class EntityVHelicopter : EntityDriveable
{
	// Token: 0x0600296C RID: 10604 RVA: 0x00104C2C File Offset: 0x00102E2C
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		Transform meshTransform = this.vehicle.GetMeshTransform();
		this.topPropT = meshTransform.Find("Origin/TopPropellerJoint");
		this.rearPropT = meshTransform.Find("Origin/BackPropellerJoint");
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x00104C70 File Offset: 0x00102E70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void PhysicsInputMove()
	{
		float deltaTime = Time.deltaTime;
		this.vehicleRB.velocity *= 0.995f;
		this.vehicleRB.velocity += new Vector3(0f, -0.002f, 0f);
		this.vehicleRB.angularVelocity *= 0.97f;
		if (this.movementInput != null)
		{
			this.vehicleRB.AddForce(0f, Mathf.Lerp(0.1f, 1.005f, this.topRPM / 3f) * -Physics.gravity.y * deltaTime, 0f, ForceMode.VelocityChange);
			float num = 1f;
			if (this.movementInput.running)
			{
				num *= 6f;
			}
			this.wheelMotor = this.movementInput.moveForward;
			this.vehicleRB.AddRelativeForce(0f, 0f, this.wheelMotor * num * 0.1f, ForceMode.VelocityChange);
			float num2;
			if (this.movementInput.lastInputController)
			{
				num2 = this.movementInput.moveStrafe * num;
			}
			else
			{
				num2 = this.movementInput.moveStrafe * num;
			}
			this.vehicleRB.AddRelativeTorque(0f, num2 * 0.03f, 0f, ForceMode.VelocityChange);
			if (this.movementInput.jump)
			{
				this.vehicleRB.AddRelativeForce(0f, 0.03f * num, 0f, ForceMode.VelocityChange);
				this.vehicleRB.AddRelativeTorque(-0.01f, 0f, 0f, ForceMode.VelocityChange);
			}
			if (this.movementInput.down)
			{
				this.vehicleRB.AddRelativeForce(0f, -0.03f * num, 0f, ForceMode.VelocityChange);
				this.vehicleRB.AddRelativeTorque(0.01f, 0f, 0f, ForceMode.VelocityChange);
			}
		}
		if (base.HasDriver)
		{
			this.topRPM += 0.6f * deltaTime;
			this.topRPM = Mathf.Min(this.topRPM, 3f);
			return;
		}
		this.topRPM *= 0.99f;
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x00104E94 File Offset: 0x00103094
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetWheelsForces(float motorTorque, float motorTorqueBase, float brakeTorque, float _friction)
	{
		for (int i = 0; i < this.wheels.Length; i++)
		{
			EntityVehicle.Wheel wheel = this.wheels[i];
			wheel.wheelC.motorTorque = motorTorque;
			wheel.wheelC.brakeTorque = 0f;
		}
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x00100070 File Offset: 0x000FE270
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateWheelsSteering()
	{
		this.wheels[0].wheelC.steerAngle = this.wheelDir;
	}

	// Token: 0x06002970 RID: 10608 RVA: 0x00104ED8 File Offset: 0x001030D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (base.HasDriver && this.rearPropT)
		{
			Vector3 localEulerAngles = this.rearPropT.localEulerAngles;
			localEulerAngles.z += 2880f * Time.deltaTime;
			this.rearPropT.localEulerAngles = localEulerAngles;
		}
		if (this.topRPM > 0.1f && this.topPropT)
		{
			Vector3 localEulerAngles2 = this.topPropT.localEulerAngles;
			localEulerAngles2.y += this.topRPM * 360f * Time.deltaTime;
			this.topPropT.localEulerAngles = localEulerAngles2;
		}
	}

	// Token: 0x04001F6E RID: 8046
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cTopRPMMax = 3f;

	// Token: 0x04001F6F RID: 8047
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform topPropT;

	// Token: 0x04001F70 RID: 8048
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float topRPM;

	// Token: 0x04001F71 RID: 8049
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform rearPropT;
}

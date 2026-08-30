using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C93 RID: 3219
[Preserve]
public class VPSteering : VehiclePart
{
	// Token: 0x060062BB RID: 25275 RVA: 0x0026CFE0 File Offset: 0x0026B1E0
	public override void InitPrefabConnections()
	{
		StringParsers.TryParseFloat(base.GetProperty("steerMaxAngle"), out this.steerMaxAngle);
		this.properties.ParseVec("steerAngle", ref this.steerAngles);
		this.steeringJoint = base.GetTransform();
		if (this.steeringJoint)
		{
			this.baseRotation = this.steeringJoint.localRotation;
		}
		base.InitIKTarget(AvatarIKGoal.LeftHand, this.steeringJoint);
		base.InitIKTarget(AvatarIKGoal.RightHand, this.steeringJoint);
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x0026D060 File Offset: 0x0026B260
	public override void Update(float _dt)
	{
		if (this.steerMaxAngle != 0f)
		{
			this.steeringJoint.localRotation = this.baseRotation * Quaternion.AngleAxis(this.vehicle.CurrentSteeringPercent * this.steerMaxAngle, Vector3.up);
		}
		if (this.steerAngles.sqrMagnitude != 0f)
		{
			float currentSteeringPercent = this.vehicle.CurrentSteeringPercent;
			this.steeringJoint.localRotation = this.baseRotation * Quaternion.Euler(currentSteeringPercent * this.steerAngles.x, currentSteeringPercent * this.steerAngles.y, currentSteeringPercent * this.steerAngles.z);
		}
	}

	// Token: 0x04004C99 RID: 19609
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform steeringJoint;

	// Token: 0x04004C9A RID: 19610
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion baseRotation;

	// Token: 0x04004C9B RID: 19611
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerMaxAngle;

	// Token: 0x04004C9C RID: 19612
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 steerAngles;
}

using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CB3 RID: 3251
[Preserve]
public class VPSteering : VehiclePart
{
	// Token: 0x06006339 RID: 25401 RVA: 0x002732FC File Offset: 0x002714FC
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

	// Token: 0x0600633A RID: 25402 RVA: 0x0027337C File Offset: 0x0027157C
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

	// Token: 0x04004D6C RID: 19820
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform steeringJoint;

	// Token: 0x04004D6D RID: 19821
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion baseRotation;

	// Token: 0x04004D6E RID: 19822
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerMaxAngle;

	// Token: 0x04004D6F RID: 19823
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 steerAngles;
}

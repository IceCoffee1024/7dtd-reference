using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CBE RID: 3262
[Preserve]
public class VPSteering : VehiclePart
{
	// Token: 0x0600635E RID: 25438 RVA: 0x00274824 File Offset: 0x00272A24
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

	// Token: 0x0600635F RID: 25439 RVA: 0x002748A4 File Offset: 0x00272AA4
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

	// Token: 0x04004D91 RID: 19857
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform steeringJoint;

	// Token: 0x04004D92 RID: 19858
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion baseRotation;

	// Token: 0x04004D93 RID: 19859
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerMaxAngle;

	// Token: 0x04004D94 RID: 19860
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 steerAngles;
}

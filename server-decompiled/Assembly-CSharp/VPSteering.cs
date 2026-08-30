using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000CA5 RID: 3237
[Preserve]
public class VPSteering : VehiclePart
{
	// Token: 0x06006346 RID: 25414 RVA: 0x0026FEB8 File Offset: 0x0026E0B8
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

	// Token: 0x06006347 RID: 25415 RVA: 0x0026FF38 File Offset: 0x0026E138
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

	// Token: 0x04004D36 RID: 19766
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform steeringJoint;

	// Token: 0x04004D37 RID: 19767
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion baseRotation;

	// Token: 0x04004D38 RID: 19768
	[PublicizedFrom(EAccessModifier.Private)]
	public float steerMaxAngle;

	// Token: 0x04004D39 RID: 19769
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 steerAngles;
}

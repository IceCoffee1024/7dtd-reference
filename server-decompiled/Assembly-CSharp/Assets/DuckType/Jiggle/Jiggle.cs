using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D90 RID: 7568
	public class Jiggle : MonoBehaviour
	{
		// Token: 0x0600DFC4 RID: 57284 RVA: 0x004FDD00 File Offset: 0x004FBF00
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDrawGizmos()
		{
			if (!this.ShowViewportGizmos)
			{
				return;
			}
			Vector3 vector = base.transform.localToWorldMatrix.MultiplyPoint3x4(this.CenterOfMass);
			Gizmos.color = Color.green;
			if (this.UseCenterOfMass)
			{
				Gizmos.DrawSphere(vector, this.CenterOfMassInertia * 5f * this.GizmoScale);
				Gizmos.DrawLine(base.transform.position, vector);
			}
			if (this.Hinge)
			{
				this.DrawGizmosArc(base.transform.position, base.transform.position + this.GetRestRotationWorld() * this.CenterOfMass * 11f * this.GizmoScale, this.GetHingeNormalWorld(), 360f);
			}
		}

		// Token: 0x0600DFC5 RID: 57285 RVA: 0x004FDDC8 File Offset: 0x004FBFC8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDrawGizmosSelected()
		{
			if (!this.ShowViewportGizmos)
			{
				return;
			}
			if (this.UseAngleLimit & (this.AngleLimit > 0f))
			{
				float num = 10f * this.GizmoScale;
				Vector3 vector = this.GetRestRotationWorld() * this.CenterOfMass;
				List<Vector3> list;
				if (this.Hinge)
				{
					Vector3 vector2 = Vector3.Cross(vector, this.GetHingeNormalWorld());
					list = new List<Vector3>
					{
						vector2,
						-vector2
					};
				}
				else
				{
					list = vector.GetOrthogonalVectors(12);
				}
				foreach (Vector3 vector3 in list)
				{
					Gizmos.color = Color.red;
					Vector3 vector4 = ((this.AngleLimit < 90f) ? Vector3.RotateTowards(vector3, base.transform.rotation * this.CenterOfMass, (90f - this.AngleLimit) * 0.017453292f, 1f) : Vector3.RotateTowards(vector3, base.transform.rotation * -this.CenterOfMass, (this.AngleLimit - 90f) * 0.017453292f, 1f));
					vector4 *= num;
					Gizmos.DrawRay(base.transform.position, vector4);
					if (this.UseSoftLimit)
					{
						Gizmos.color = Color.yellow;
						Vector3 vector5 = base.transform.position + vector4;
						this.DrawGizmosArc(base.transform.position, vector5, Vector3.Cross(vector4, vector), this.AngleLimit * this.SoftLimitInfluence);
					}
				}
			}
		}

		// Token: 0x0600DFC6 RID: 57286 RVA: 0x004FDF90 File Offset: 0x004FC190
		[PublicizedFrom(EAccessModifier.Private)]
		public void Start()
		{
			this.m_Initialised = true;
			this.m_RestLocalRotation = base.transform.localRotation;
			this.m_LastWorldRotation = base.transform.rotation;
			this.m_LastCenterOfMassWorld = base.transform.localToWorldMatrix.MultiplyPoint3x4(this.CenterOfMass);
		}

		// Token: 0x0600DFC7 RID: 57287 RVA: 0x004FDFE5 File Offset: 0x004FC1E5
		[PublicizedFrom(EAccessModifier.Private)]
		public void LateUpdate()
		{
			JiggleScheduler.Update(this);
		}

		// Token: 0x0600DFC8 RID: 57288 RVA: 0x004FDFED File Offset: 0x004FC1ED
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			JiggleScheduler.Register(this);
		}

		// Token: 0x0600DFC9 RID: 57289 RVA: 0x004FDFF5 File Offset: 0x004FC1F5
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			JiggleScheduler.Deregister(this);
		}

		// Token: 0x0600DFCA RID: 57290 RVA: 0x004FE000 File Offset: 0x004FC200
		public void ScheduledUpdate(float deltaTime)
		{
			Quaternion quaternion;
			if (this.RotationInertia > 0f)
			{
				quaternion = Quaternion.SlerpUnclamped(base.transform.rotation, this.m_LastWorldRotation, this.RotationInertia);
			}
			else
			{
				quaternion = base.transform.rotation;
			}
			if (this.UseCenterOfMass && this.CenterOfMassInertia > 0f)
			{
				Quaternion quaternion2 = Quaternion.FromToRotation(quaternion * this.CenterOfMass, this.m_LastCenterOfMassWorld - base.transform.position);
				Debug.DrawLine(this.m_LastCenterOfMassWorld, base.transform.position);
				quaternion = quaternion2.Scale(this.CenterOfMassInertia) * quaternion;
			}
			quaternion *= this.m_Torque.Scale(deltaTime / 0.001f);
			Quaternion quaternion3 = ((base.transform.parent != null) ? base.transform.parent.rotation : Quaternion.identity) * this.m_RestLocalRotation;
			if (this.BlendToOriginalRotation)
			{
				quaternion3 = base.transform.rotation;
			}
			float num = Quaternion.Angle(quaternion, quaternion3);
			if (this.UseAngleLimit && num > this.AngleLimit)
			{
				quaternion = Quaternion.Slerp(quaternion3, quaternion, this.AngleLimit / num);
			}
			if (this.Hinge)
			{
				Vector3 vector = quaternion * this.CenterOfMass;
				Vector3 hingeNormalWorld = this.GetHingeNormalWorld();
				Vector3 vector2 = Vector3.Cross(hingeNormalWorld, Vector3.Cross(vector, hingeNormalWorld));
				quaternion = Quaternion.FromToRotation(vector, vector2) * quaternion;
			}
			base.transform.rotation = quaternion;
			if (this.SpringStrength > 0f)
			{
				Quaternion quaternion4 = base.transform.rotation.FromToRotation(quaternion3);
				quaternion4 = quaternion4.Scale(0.001f * this.SpringStrength * 250f * deltaTime);
				this.m_Torque = this.m_Torque.Append(quaternion4);
			}
			if (this.UseCenterOfMass)
			{
				if (this.Gravity > 0f)
				{
					Quaternion closestRotationFromTo = this.GetClosestRotationFromTo(base.transform.rotation * this.CenterOfMass, Vector3.down);
					this.m_Torque = this.m_Torque.Append(closestRotationFromTo.Scale(0.001f * this.Gravity * 50f * deltaTime));
				}
				if (this.AddWind)
				{
					Quaternion closestRotationFromTo2 = this.GetClosestRotationFromTo(base.transform.rotation * this.CenterOfMass, this.WindDirection);
					this.m_Torque = this.m_Torque.Append(closestRotationFromTo2.Scale(0.001f * this.WindStrength * 50f * deltaTime));
				}
				if (this.AddNoise)
				{
					Vector3 noiseVector = this.GetNoiseVector(base.transform.localToWorldMatrix.MultiplyPoint3x4(this.CenterOfMass), this.NoiseScale * 10f, this.m_NoisePhase += deltaTime * this.NoiseSpeed);
					Quaternion closestRotationFromTo3 = this.GetClosestRotationFromTo(base.transform.rotation * this.CenterOfMass, noiseVector);
					this.m_Torque = this.m_Torque.Append(closestRotationFromTo3.Scale(0.001f * this.NoiseStrength * 50f * deltaTime));
				}
			}
			if (this.UseSoftLimit && this.UseAngleLimit && this.AngleLimit > 0f && this.SoftLimitStrength > 0f)
			{
				num = Quaternion.Angle(quaternion, quaternion3);
				float num2 = this.AngleLimit * (1f - this.SoftLimitInfluence);
				if (num > num2)
				{
					float num3 = Mathf.Min((num - num2) / (this.AngleLimit - num2), 1f);
					Quaternion quaternion5 = base.transform.rotation.FromToRotation(quaternion3);
					this.m_Torque = this.m_Torque.Append(quaternion5.Scale(0.001f * num3 * this.SoftLimitStrength * 250f * deltaTime));
				}
			}
			this.m_Torque = this.m_Torque.Scale((1f - this.Dampening * 10f * deltaTime).Clamp01());
			this.m_LastCenterOfMassWorld = base.transform.localToWorldMatrix.MultiplyPoint3x4(this.CenterOfMass);
			this.m_LastWorldRotation = base.transform.rotation;
		}

		// Token: 0x0600DFCB RID: 57291 RVA: 0x004FE438 File Offset: 0x004FC638
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 GetHingeNormalWorld()
		{
			Vector3 vector = ((Mathf.Abs(this.CenterOfMass.normalized.y) != 1f) ? (Quaternion.AngleAxis(this.HingeAngle, this.CenterOfMass) * Vector3.Cross(this.CenterOfMass, Vector3.up)) : (Quaternion.AngleAxis(this.HingeAngle, this.CenterOfMass) * Vector3.Cross(this.CenterOfMass, Vector3.right)));
			return this.GetRestRotationWorld() * vector;
		}

		// Token: 0x0600DFCC RID: 57292 RVA: 0x004FE4BC File Offset: 0x004FC6BC
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion GetRestRotationWorld()
		{
			if (!this.m_Initialised)
			{
				return base.transform.rotation;
			}
			if (!(base.transform.parent != null))
			{
				return this.m_RestLocalRotation;
			}
			return base.transform.parent.rotation * this.m_RestLocalRotation;
		}

		// Token: 0x0600DFCD RID: 57293 RVA: 0x004FE514 File Offset: 0x004FC714
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 GetNoiseVector(Vector3 pos, float scale, float phase)
		{
			pos /= scale;
			return new Vector3(Mathf.PerlinNoise(pos.x, pos.y + phase) - 0.5f, Mathf.PerlinNoise(pos.y, pos.z + phase) - 0.5f, Mathf.PerlinNoise(pos.z, pos.x + phase) - 0.5f);
		}

		// Token: 0x0600DFCE RID: 57294 RVA: 0x004FE57C File Offset: 0x004FC77C
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion GetClosestRotationFromTo(Vector3 from, Vector3 to)
		{
			Quaternion quaternion = Quaternion.FromToRotation(from, to) * base.transform.rotation;
			return base.transform.rotation.FromToRotation(quaternion);
		}

		// Token: 0x0600DFCF RID: 57295 RVA: 0x004FE5B4 File Offset: 0x004FC7B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void DrawGizmosArc(Vector3 center, Vector3 startPoint, Vector3 normal, float degrees)
		{
			int num = (int)Mathf.Ceil(degrees / 20f);
			Quaternion quaternion = Quaternion.AngleAxis(degrees / (float)num, normal);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = quaternion * (startPoint - center) + center;
				Gizmos.DrawRay(startPoint, vector - startPoint);
				startPoint = vector;
			}
		}

		// Token: 0x0600DFD0 RID: 57296 RVA: 0x004FE60C File Offset: 0x004FC80C
		public string DebugString()
		{
			return string.Format("{0} - enabled: {1}, m_Initialized: {2}, m_RestLocalRotation: {3}, UseCenterOfMass: {4}, CenterOfMass: {5}, Hinge: {6}, HingeAngle: {7}, UseAngleLimit: {8}, AngleLimit: {9}", new object[]
			{
				base.gameObject.name,
				base.isActiveAndEnabled,
				this.m_Initialised,
				this.m_RestLocalRotation,
				this.UseCenterOfMass,
				this.CenterOfMass,
				this.Hinge,
				this.HingeAngle,
				this.UseAngleLimit,
				this.AngleLimit
			});
		}

		// Token: 0x0400A96D RID: 43373
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float TORQUE_FACTOR = 0.001f;

		// Token: 0x0400A96E RID: 43374
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool m_Initialised;

		// Token: 0x0400A96F RID: 43375
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion m_RestLocalRotation;

		// Token: 0x0400A970 RID: 43376
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion m_LastWorldRotation;

		// Token: 0x0400A971 RID: 43377
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion m_Torque = Quaternion.identity;

		// Token: 0x0400A972 RID: 43378
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 m_LastCenterOfMassWorld;

		// Token: 0x0400A973 RID: 43379
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float m_NoisePhase;

		// Token: 0x0400A974 RID: 43380
		public bool UpdateWithPhysics;

		// Token: 0x0400A975 RID: 43381
		public bool UseCenterOfMass = true;

		// Token: 0x0400A976 RID: 43382
		public Vector3 CenterOfMass = new Vector3(1f, 0f, 0f);

		// Token: 0x0400A977 RID: 43383
		public float CenterOfMassInertia = 1f;

		// Token: 0x0400A978 RID: 43384
		public bool AddWind;

		// Token: 0x0400A979 RID: 43385
		public Vector3 WindDirection = new Vector3(1f, 0f, 0f);

		// Token: 0x0400A97A RID: 43386
		public float WindStrength = 1f;

		// Token: 0x0400A97B RID: 43387
		public bool AddNoise;

		// Token: 0x0400A97C RID: 43388
		public float NoiseStrength = 1f;

		// Token: 0x0400A97D RID: 43389
		public float NoiseScale = 1f;

		// Token: 0x0400A97E RID: 43390
		public float NoiseSpeed = 1f;

		// Token: 0x0400A97F RID: 43391
		public float RotationInertia = 1f;

		// Token: 0x0400A980 RID: 43392
		public float Gravity;

		// Token: 0x0400A981 RID: 43393
		public float SpringStrength = 0.4f;

		// Token: 0x0400A982 RID: 43394
		public float Dampening = 0.4f;

		// Token: 0x0400A983 RID: 43395
		public bool BlendToOriginalRotation;

		// Token: 0x0400A984 RID: 43396
		public bool Hinge;

		// Token: 0x0400A985 RID: 43397
		public float HingeAngle;

		// Token: 0x0400A986 RID: 43398
		public bool UseAngleLimit;

		// Token: 0x0400A987 RID: 43399
		public float AngleLimit = 180f;

		// Token: 0x0400A988 RID: 43400
		public bool UseSoftLimit;

		// Token: 0x0400A989 RID: 43401
		public float SoftLimitInfluence = 0.5f;

		// Token: 0x0400A98A RID: 43402
		public float SoftLimitStrength = 0.5f;

		// Token: 0x0400A98B RID: 43403
		public bool ShowViewportGizmos = true;

		// Token: 0x0400A98C RID: 43404
		public float GizmoScale = 0.1f;
	}
}

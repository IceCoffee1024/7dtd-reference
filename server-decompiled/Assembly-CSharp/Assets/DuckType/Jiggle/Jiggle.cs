using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D5E RID: 7518
	public class Jiggle : MonoBehaviour
	{
		// Token: 0x0600DE4B RID: 56907 RVA: 0x004F9144 File Offset: 0x004F7344
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

		// Token: 0x0600DE4C RID: 56908 RVA: 0x004F920C File Offset: 0x004F740C
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

		// Token: 0x0600DE4D RID: 56909 RVA: 0x004F93D4 File Offset: 0x004F75D4
		[PublicizedFrom(EAccessModifier.Private)]
		public void Start()
		{
			this.m_Initialised = true;
			this.m_RestLocalRotation = base.transform.localRotation;
			this.m_LastWorldRotation = base.transform.rotation;
			this.m_LastCenterOfMassWorld = base.transform.localToWorldMatrix.MultiplyPoint3x4(this.CenterOfMass);
		}

		// Token: 0x0600DE4E RID: 56910 RVA: 0x004F9429 File Offset: 0x004F7629
		[PublicizedFrom(EAccessModifier.Private)]
		public void LateUpdate()
		{
			JiggleScheduler.Update(this);
		}

		// Token: 0x0600DE4F RID: 56911 RVA: 0x004F9431 File Offset: 0x004F7631
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnEnable()
		{
			JiggleScheduler.Register(this);
		}

		// Token: 0x0600DE50 RID: 56912 RVA: 0x004F9439 File Offset: 0x004F7639
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			JiggleScheduler.Deregister(this);
		}

		// Token: 0x0600DE51 RID: 56913 RVA: 0x004F9444 File Offset: 0x004F7644
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

		// Token: 0x0600DE52 RID: 56914 RVA: 0x004F987C File Offset: 0x004F7A7C
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 GetHingeNormalWorld()
		{
			Vector3 vector = ((Mathf.Abs(this.CenterOfMass.normalized.y) != 1f) ? (Quaternion.AngleAxis(this.HingeAngle, this.CenterOfMass) * Vector3.Cross(this.CenterOfMass, Vector3.up)) : (Quaternion.AngleAxis(this.HingeAngle, this.CenterOfMass) * Vector3.Cross(this.CenterOfMass, Vector3.right)));
			return this.GetRestRotationWorld() * vector;
		}

		// Token: 0x0600DE53 RID: 56915 RVA: 0x004F9900 File Offset: 0x004F7B00
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

		// Token: 0x0600DE54 RID: 56916 RVA: 0x004F9958 File Offset: 0x004F7B58
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 GetNoiseVector(Vector3 pos, float scale, float phase)
		{
			pos /= scale;
			return new Vector3(Mathf.PerlinNoise(pos.x, pos.y + phase) - 0.5f, Mathf.PerlinNoise(pos.y, pos.z + phase) - 0.5f, Mathf.PerlinNoise(pos.z, pos.x + phase) - 0.5f);
		}

		// Token: 0x0600DE55 RID: 56917 RVA: 0x004F99C0 File Offset: 0x004F7BC0
		[PublicizedFrom(EAccessModifier.Private)]
		public Quaternion GetClosestRotationFromTo(Vector3 from, Vector3 to)
		{
			Quaternion quaternion = Quaternion.FromToRotation(from, to) * base.transform.rotation;
			return base.transform.rotation.FromToRotation(quaternion);
		}

		// Token: 0x0600DE56 RID: 56918 RVA: 0x004F99F8 File Offset: 0x004F7BF8
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

		// Token: 0x0600DE57 RID: 56919 RVA: 0x004F9A50 File Offset: 0x004F7C50
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

		// Token: 0x0400A896 RID: 43158
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float TORQUE_FACTOR = 0.001f;

		// Token: 0x0400A897 RID: 43159
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool m_Initialised;

		// Token: 0x0400A898 RID: 43160
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion m_RestLocalRotation;

		// Token: 0x0400A899 RID: 43161
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion m_LastWorldRotation;

		// Token: 0x0400A89A RID: 43162
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Quaternion m_Torque = Quaternion.identity;

		// Token: 0x0400A89B RID: 43163
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 m_LastCenterOfMassWorld;

		// Token: 0x0400A89C RID: 43164
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float m_NoisePhase;

		// Token: 0x0400A89D RID: 43165
		public bool UpdateWithPhysics;

		// Token: 0x0400A89E RID: 43166
		public bool UseCenterOfMass = true;

		// Token: 0x0400A89F RID: 43167
		public Vector3 CenterOfMass = new Vector3(1f, 0f, 0f);

		// Token: 0x0400A8A0 RID: 43168
		public float CenterOfMassInertia = 1f;

		// Token: 0x0400A8A1 RID: 43169
		public bool AddWind;

		// Token: 0x0400A8A2 RID: 43170
		public Vector3 WindDirection = new Vector3(1f, 0f, 0f);

		// Token: 0x0400A8A3 RID: 43171
		public float WindStrength = 1f;

		// Token: 0x0400A8A4 RID: 43172
		public bool AddNoise;

		// Token: 0x0400A8A5 RID: 43173
		public float NoiseStrength = 1f;

		// Token: 0x0400A8A6 RID: 43174
		public float NoiseScale = 1f;

		// Token: 0x0400A8A7 RID: 43175
		public float NoiseSpeed = 1f;

		// Token: 0x0400A8A8 RID: 43176
		public float RotationInertia = 1f;

		// Token: 0x0400A8A9 RID: 43177
		public float Gravity;

		// Token: 0x0400A8AA RID: 43178
		public float SpringStrength = 0.4f;

		// Token: 0x0400A8AB RID: 43179
		public float Dampening = 0.4f;

		// Token: 0x0400A8AC RID: 43180
		public bool BlendToOriginalRotation;

		// Token: 0x0400A8AD RID: 43181
		public bool Hinge;

		// Token: 0x0400A8AE RID: 43182
		public float HingeAngle;

		// Token: 0x0400A8AF RID: 43183
		public bool UseAngleLimit;

		// Token: 0x0400A8B0 RID: 43184
		public float AngleLimit = 180f;

		// Token: 0x0400A8B1 RID: 43185
		public bool UseSoftLimit;

		// Token: 0x0400A8B2 RID: 43186
		public float SoftLimitInfluence = 0.5f;

		// Token: 0x0400A8B3 RID: 43187
		public float SoftLimitStrength = 0.5f;

		// Token: 0x0400A8B4 RID: 43188
		public bool ShowViewportGizmos = true;

		// Token: 0x0400A8B5 RID: 43189
		public float GizmoScale = 0.1f;
	}
}

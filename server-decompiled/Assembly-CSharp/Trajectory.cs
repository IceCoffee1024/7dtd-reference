using System;
using UnityEngine;

// Token: 0x020014BB RID: 5307
public static class Trajectory
{
	// Token: 0x0600A6AB RID: 42667 RVA: 0x003E0154 File Offset: 0x003DE354
	public unsafe static int Calculate(Vector3 source, Vector3 target, float projectileSpeed, float gravity, Span<Trajectory.AimTrajectory> trajectories)
	{
		Span<float> span = new Span<float>(stackalloc byte[(UIntPtr)8], 2);
		float magnitude = (new Vector2(target.x, target.z) - new Vector2(source.x, source.z)).magnitude;
		float num = target.y - source.y;
		float num2 = 4f * num * num + 4f * magnitude * magnitude;
		float num3 = -4f * projectileSpeed * projectileSpeed - 4f * num * gravity;
		float num4 = gravity * gravity;
		float num5 = num3 * num3 - 4f * num2 * num4;
		if (num5 <= 0f || num2 == 0f)
		{
			return 0;
		}
		float num6 = Mathf.Sqrt(num5);
		float num7 = 0.5f / num2;
		*span[0] = (-num3 + num6) * num7;
		*span[1] = (-num3 - num6) * num7;
		int num8 = 0;
		for (int i = 0; i < 2; i++)
		{
			if (*span[i] > 0f)
			{
				num5 = Mathf.Sqrt(*span[i]);
				trajectories[num8].Angle = Mathf.Atan2(0.5f * (2f * num * *span[i] - gravity) / num5, num5 * magnitude);
				trajectories[num8].Time = magnitude / (Mathf.Cos(trajectories[num8].Angle) * projectileSpeed);
				Vector3 vector = target;
				Vector3 vector2 = vector - source;
				vector.y = source.y + Mathf.Tan(trajectories[num8].Angle) * new Vector2(vector2.x, vector2.z).magnitude;
				trajectories[num8].Direction = vector - source;
				trajectories[num8].Direction.Normalize();
				trajectories[num8].LaunchVelocity = trajectories[num8].Direction * projectileSpeed;
				num8++;
			}
		}
		return num8;
	}

	// Token: 0x0600A6AC RID: 42668 RVA: 0x003E036C File Offset: 0x003DE56C
	public static bool SuggestVelocity_CustomArc(out Vector3 launchVelocity, Vector3 source, Vector3 target, float projectileGravity, float arcParam = 0.5f)
	{
		Vector3 vector = target - source;
		float num;
		Vector3 vector2 = vector.NormalizeReturnMagnitude(out num);
		if (num > 1E-45f)
		{
			Vector3 normalized = Vector3.Lerp(Vector3.up, vector2, arcParam).normalized;
			float magnitude = new Vector2(vector.x, vector.z).magnitude;
			float y = vector.y;
			float num2 = Mathf.Atan2(vector.y, magnitude) * 57.29578f;
			float num3 = Mathf.Cos(num2);
			float num4 = projectileGravity * (magnitude * magnitude) * 0.5f;
			float num5 = (y - magnitude * Mathf.Tan(num2)) * num3;
			float num6 = num4 / num5;
			if (num6 >= 0f)
			{
				float num7 = Mathf.Sqrt(num6);
				launchVelocity = normalized * num7;
				return true;
			}
		}
		launchVelocity = Vector3.zero;
		return false;
	}

	// Token: 0x020014BC RID: 5308
	public struct AimTrajectory
	{
		// Token: 0x04007B64 RID: 31588
		public Vector3 Direction;

		// Token: 0x04007B65 RID: 31589
		public Vector3 LaunchVelocity;

		// Token: 0x04007B66 RID: 31590
		public float Angle;

		// Token: 0x04007B67 RID: 31591
		public float Time;
	}
}

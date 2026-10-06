using System;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D8B RID: 7563
	[DefaultExecutionOrder(-100)]
	public class KinematicCharacterSystem : MonoBehaviour
	{
		// Token: 0x0600DF8F RID: 57231 RVA: 0x004FD478 File Offset: 0x004FB678
		public static void EnsureCreation()
		{
			if (KinematicCharacterSystem._instance == null)
			{
				GameObject gameObject = new GameObject("KinematicCharacterSystem");
				KinematicCharacterSystem._instance = gameObject.AddComponent<KinematicCharacterSystem>();
				gameObject.hideFlags = HideFlags.NotEditable;
				KinematicCharacterSystem._instance.hideFlags = HideFlags.NotEditable;
			}
		}

		// Token: 0x0600DF90 RID: 57232 RVA: 0x004FD4AD File Offset: 0x004FB6AD
		public static KinematicCharacterSystem GetInstance()
		{
			return KinematicCharacterSystem._instance;
		}

		// Token: 0x0600DF91 RID: 57233 RVA: 0x004FD4B4 File Offset: 0x004FB6B4
		public static void SetCharacterMotorsCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.CharacterMotors.Count)
			{
				capacity = KinematicCharacterSystem.CharacterMotors.Count;
			}
			KinematicCharacterSystem.CharacterMotors.Capacity = capacity;
		}

		// Token: 0x0600DF92 RID: 57234 RVA: 0x004FD4DA File Offset: 0x004FB6DA
		public static void RegisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Add(motor);
		}

		// Token: 0x0600DF93 RID: 57235 RVA: 0x004FD4E7 File Offset: 0x004FB6E7
		public static void UnregisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Remove(motor);
		}

		// Token: 0x0600DF94 RID: 57236 RVA: 0x004FD4F5 File Offset: 0x004FB6F5
		public static void SetPhysicsMoversCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.PhysicsMovers.Count)
			{
				capacity = KinematicCharacterSystem.PhysicsMovers.Count;
			}
			KinematicCharacterSystem.PhysicsMovers.Capacity = capacity;
		}

		// Token: 0x0600DF95 RID: 57237 RVA: 0x004FD51B File Offset: 0x004FB71B
		public static void RegisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Add(mover);
			mover.Rigidbody.interpolation = RigidbodyInterpolation.None;
		}

		// Token: 0x0600DF96 RID: 57238 RVA: 0x004FD534 File Offset: 0x004FB734
		public static void UnregisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Remove(mover);
		}

		// Token: 0x0600DF97 RID: 57239 RVA: 0x001483DF File Offset: 0x001465DF
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600DF98 RID: 57240 RVA: 0x004FD542 File Offset: 0x004FB742
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			KinematicCharacterSystem._instance = this;
		}

		// Token: 0x0600DF99 RID: 57241 RVA: 0x004FD54C File Offset: 0x004FB74C
		[PublicizedFrom(EAccessModifier.Private)]
		public void FixedUpdate()
		{
			if (KinematicCharacterSystem.AutoSimulation)
			{
				float deltaTime = Time.deltaTime;
				if (KinematicCharacterSystem.Interpolate)
				{
					KinematicCharacterSystem.PreSimulationInterpolationUpdate(deltaTime);
				}
				KinematicCharacterSystem.Simulate(deltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.CharacterMotors.Count, KinematicCharacterSystem.PhysicsMovers, KinematicCharacterSystem.PhysicsMovers.Count);
				if (KinematicCharacterSystem.Interpolate)
				{
					KinematicCharacterSystem.PostSimulationInterpolationUpdate(deltaTime);
				}
			}
		}

		// Token: 0x0600DF9A RID: 57242 RVA: 0x004FD5A4 File Offset: 0x004FB7A4
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (KinematicCharacterSystem.Interpolate)
			{
				KinematicCharacterSystem.CustomInterpolationUpdate();
			}
		}

		// Token: 0x0600DF9B RID: 57243 RVA: 0x004FD5B4 File Offset: 0x004FB7B4
		public static void PreSimulationInterpolationUpdate(float deltaTime)
		{
			for (int i = 0; i < KinematicCharacterSystem.CharacterMotors.Count; i++)
			{
				KinematicCharacterMotor kinematicCharacterMotor = KinematicCharacterSystem.CharacterMotors[i];
				kinematicCharacterMotor.InitialTickPosition = kinematicCharacterMotor.TransientPosition;
				kinematicCharacterMotor.InitialTickRotation = kinematicCharacterMotor.TransientRotation;
				kinematicCharacterMotor.Transform.SetPositionAndRotation(kinematicCharacterMotor.TransientPosition, kinematicCharacterMotor.TransientRotation);
			}
			for (int j = 0; j < KinematicCharacterSystem.PhysicsMovers.Count; j++)
			{
				PhysicsMover physicsMover = KinematicCharacterSystem.PhysicsMovers[j];
				physicsMover.InitialTickPosition = physicsMover.TransientPosition;
				physicsMover.InitialTickRotation = physicsMover.TransientRotation;
				physicsMover.Transform.SetPositionAndRotation(physicsMover.TransientPosition, physicsMover.TransientRotation);
				physicsMover.Rigidbody.position = physicsMover.TransientPosition;
				physicsMover.Rigidbody.rotation = physicsMover.TransientRotation;
			}
		}

		// Token: 0x0600DF9C RID: 57244 RVA: 0x004FD684 File Offset: 0x004FB884
		public static void Simulate(float deltaTime, List<KinematicCharacterMotor> motors, int characterMotorsCount, List<PhysicsMover> movers, int physicsMoversCount)
		{
			for (int i = 0; i < physicsMoversCount; i++)
			{
				movers[i].VelocityUpdate(deltaTime);
			}
			for (int j = 0; j < characterMotorsCount; j++)
			{
				motors[j].UpdatePhase1(deltaTime);
			}
			for (int k = 0; k < physicsMoversCount; k++)
			{
				PhysicsMover physicsMover = movers[k];
				physicsMover.Transform.SetPositionAndRotation(physicsMover.TransientPosition, physicsMover.TransientRotation);
				physicsMover.Rigidbody.position = physicsMover.TransientPosition;
				physicsMover.Rigidbody.rotation = physicsMover.TransientRotation;
			}
			for (int l = 0; l < characterMotorsCount; l++)
			{
				KinematicCharacterMotor kinematicCharacterMotor = motors[l];
				kinematicCharacterMotor.UpdatePhase2(deltaTime);
				kinematicCharacterMotor.Transform.SetPositionAndRotation(kinematicCharacterMotor.TransientPosition, kinematicCharacterMotor.TransientRotation);
			}
			Physics.SyncTransforms();
		}

		// Token: 0x0600DF9D RID: 57245 RVA: 0x004FD754 File Offset: 0x004FB954
		public static void PostSimulationInterpolationUpdate(float deltaTime)
		{
			KinematicCharacterSystem._lastCustomInterpolationStartTime = Time.time;
			KinematicCharacterSystem._lastCustomInterpolationDeltaTime = deltaTime;
			for (int i = 0; i < KinematicCharacterSystem.CharacterMotors.Count; i++)
			{
				KinematicCharacterMotor kinematicCharacterMotor = KinematicCharacterSystem.CharacterMotors[i];
				kinematicCharacterMotor.Transform.SetPositionAndRotation(kinematicCharacterMotor.InitialTickPosition, kinematicCharacterMotor.InitialTickRotation);
			}
			for (int j = 0; j < KinematicCharacterSystem.PhysicsMovers.Count; j++)
			{
				PhysicsMover physicsMover = KinematicCharacterSystem.PhysicsMovers[j];
				physicsMover.Rigidbody.position = physicsMover.InitialTickPosition;
				physicsMover.Rigidbody.rotation = physicsMover.InitialTickRotation;
				physicsMover.Rigidbody.MovePosition(physicsMover.TransientPosition);
				physicsMover.Rigidbody.MoveRotation(physicsMover.TransientRotation);
			}
		}

		// Token: 0x0600DF9E RID: 57246 RVA: 0x004FD810 File Offset: 0x004FBA10
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CustomInterpolationUpdate()
		{
			float num = Mathf.Clamp01((Time.time - KinematicCharacterSystem._lastCustomInterpolationStartTime) / KinematicCharacterSystem._lastCustomInterpolationDeltaTime);
			for (int i = 0; i < KinematicCharacterSystem.CharacterMotors.Count; i++)
			{
				KinematicCharacterMotor kinematicCharacterMotor = KinematicCharacterSystem.CharacterMotors[i];
				kinematicCharacterMotor.Transform.SetPositionAndRotation(Vector3.Lerp(kinematicCharacterMotor.InitialTickPosition, kinematicCharacterMotor.TransientPosition, num), Quaternion.Slerp(kinematicCharacterMotor.InitialTickRotation, kinematicCharacterMotor.TransientRotation, num));
			}
			for (int j = 0; j < KinematicCharacterSystem.PhysicsMovers.Count; j++)
			{
				PhysicsMover physicsMover = KinematicCharacterSystem.PhysicsMovers[j];
				physicsMover.Transform.SetPositionAndRotation(Vector3.Lerp(physicsMover.InitialTickPosition, physicsMover.TransientPosition, num), Quaternion.Slerp(physicsMover.InitialTickRotation, physicsMover.TransientRotation, num));
			}
		}

		// Token: 0x0400A956 RID: 43350
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static KinematicCharacterSystem _instance;

		// Token: 0x0400A957 RID: 43351
		public static List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>(100);

		// Token: 0x0400A958 RID: 43352
		public static List<PhysicsMover> PhysicsMovers = new List<PhysicsMover>(100);

		// Token: 0x0400A959 RID: 43353
		public static bool AutoSimulation = true;

		// Token: 0x0400A95A RID: 43354
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationStartTime = -1f;

		// Token: 0x0400A95B RID: 43355
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationDeltaTime = -1f;

		// Token: 0x0400A95C RID: 43356
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int CharacterMotorsBaseCapacity = 100;

		// Token: 0x0400A95D RID: 43357
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int PhysicsMoversBaseCapacity = 100;

		// Token: 0x0400A95E RID: 43358
		public static bool Interpolate = true;
	}
}

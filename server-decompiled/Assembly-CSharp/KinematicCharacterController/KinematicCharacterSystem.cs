using System;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DBC RID: 7612
	[DefaultExecutionOrder(-100)]
	public class KinematicCharacterSystem : MonoBehaviour
	{
		// Token: 0x0600E0A3 RID: 57507 RVA: 0x00501018 File Offset: 0x004FF218
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

		// Token: 0x0600E0A4 RID: 57508 RVA: 0x0050104D File Offset: 0x004FF24D
		public static KinematicCharacterSystem GetInstance()
		{
			return KinematicCharacterSystem._instance;
		}

		// Token: 0x0600E0A5 RID: 57509 RVA: 0x00501054 File Offset: 0x004FF254
		public static void SetCharacterMotorsCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.CharacterMotors.Count)
			{
				capacity = KinematicCharacterSystem.CharacterMotors.Count;
			}
			KinematicCharacterSystem.CharacterMotors.Capacity = capacity;
		}

		// Token: 0x0600E0A6 RID: 57510 RVA: 0x0050107A File Offset: 0x004FF27A
		public static void RegisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Add(motor);
		}

		// Token: 0x0600E0A7 RID: 57511 RVA: 0x00501087 File Offset: 0x004FF287
		public static void UnregisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Remove(motor);
		}

		// Token: 0x0600E0A8 RID: 57512 RVA: 0x00501095 File Offset: 0x004FF295
		public static void SetPhysicsMoversCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.PhysicsMovers.Count)
			{
				capacity = KinematicCharacterSystem.PhysicsMovers.Count;
			}
			KinematicCharacterSystem.PhysicsMovers.Capacity = capacity;
		}

		// Token: 0x0600E0A9 RID: 57513 RVA: 0x005010BB File Offset: 0x004FF2BB
		public static void RegisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Add(mover);
			mover.Rigidbody.interpolation = RigidbodyInterpolation.None;
		}

		// Token: 0x0600E0AA RID: 57514 RVA: 0x005010D4 File Offset: 0x004FF2D4
		public static void UnregisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Remove(mover);
		}

		// Token: 0x0600E0AB RID: 57515 RVA: 0x00148FD3 File Offset: 0x001471D3
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600E0AC RID: 57516 RVA: 0x005010E2 File Offset: 0x004FF2E2
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			KinematicCharacterSystem._instance = this;
		}

		// Token: 0x0600E0AD RID: 57517 RVA: 0x005010EC File Offset: 0x004FF2EC
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

		// Token: 0x0600E0AE RID: 57518 RVA: 0x00501144 File Offset: 0x004FF344
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (KinematicCharacterSystem.Interpolate)
			{
				KinematicCharacterSystem.CustomInterpolationUpdate();
			}
		}

		// Token: 0x0600E0AF RID: 57519 RVA: 0x00501154 File Offset: 0x004FF354
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

		// Token: 0x0600E0B0 RID: 57520 RVA: 0x00501224 File Offset: 0x004FF424
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

		// Token: 0x0600E0B1 RID: 57521 RVA: 0x005012F4 File Offset: 0x004FF4F4
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

		// Token: 0x0600E0B2 RID: 57522 RVA: 0x005013B0 File Offset: 0x004FF5B0
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

		// Token: 0x0400AA3F RID: 43583
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static KinematicCharacterSystem _instance;

		// Token: 0x0400AA40 RID: 43584
		public static List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>(100);

		// Token: 0x0400AA41 RID: 43585
		public static List<PhysicsMover> PhysicsMovers = new List<PhysicsMover>(100);

		// Token: 0x0400AA42 RID: 43586
		public static bool AutoSimulation = true;

		// Token: 0x0400AA43 RID: 43587
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationStartTime = -1f;

		// Token: 0x0400AA44 RID: 43588
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationDeltaTime = -1f;

		// Token: 0x0400AA45 RID: 43589
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int CharacterMotorsBaseCapacity = 100;

		// Token: 0x0400AA46 RID: 43590
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int PhysicsMoversBaseCapacity = 100;

		// Token: 0x0400AA47 RID: 43591
		public static bool Interpolate = true;
	}
}

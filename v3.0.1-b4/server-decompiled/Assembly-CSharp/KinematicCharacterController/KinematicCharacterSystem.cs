using System;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D31 RID: 7473
	[DefaultExecutionOrder(-100)]
	public class KinematicCharacterSystem : MonoBehaviour
	{
		// Token: 0x0600DC73 RID: 56435 RVA: 0x004F1DBC File Offset: 0x004EFFBC
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

		// Token: 0x0600DC74 RID: 56436 RVA: 0x004F1DF1 File Offset: 0x004EFFF1
		public static KinematicCharacterSystem GetInstance()
		{
			return KinematicCharacterSystem._instance;
		}

		// Token: 0x0600DC75 RID: 56437 RVA: 0x004F1DF8 File Offset: 0x004EFFF8
		public static void SetCharacterMotorsCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.CharacterMotors.Count)
			{
				capacity = KinematicCharacterSystem.CharacterMotors.Count;
			}
			KinematicCharacterSystem.CharacterMotors.Capacity = capacity;
		}

		// Token: 0x0600DC76 RID: 56438 RVA: 0x004F1E1E File Offset: 0x004F001E
		public static void RegisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Add(motor);
		}

		// Token: 0x0600DC77 RID: 56439 RVA: 0x004F1E2B File Offset: 0x004F002B
		public static void UnregisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Remove(motor);
		}

		// Token: 0x0600DC78 RID: 56440 RVA: 0x004F1E39 File Offset: 0x004F0039
		public static void SetPhysicsMoversCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.PhysicsMovers.Count)
			{
				capacity = KinematicCharacterSystem.PhysicsMovers.Count;
			}
			KinematicCharacterSystem.PhysicsMovers.Capacity = capacity;
		}

		// Token: 0x0600DC79 RID: 56441 RVA: 0x004F1E5F File Offset: 0x004F005F
		public static void RegisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Add(mover);
			mover.Rigidbody.interpolation = RigidbodyInterpolation.None;
		}

		// Token: 0x0600DC7A RID: 56442 RVA: 0x004F1E78 File Offset: 0x004F0078
		public static void UnregisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Remove(mover);
		}

		// Token: 0x0600DC7B RID: 56443 RVA: 0x00145937 File Offset: 0x00143B37
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600DC7C RID: 56444 RVA: 0x004F1E86 File Offset: 0x004F0086
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			KinematicCharacterSystem._instance = this;
		}

		// Token: 0x0600DC7D RID: 56445 RVA: 0x004F1E90 File Offset: 0x004F0090
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

		// Token: 0x0600DC7E RID: 56446 RVA: 0x004F1EE8 File Offset: 0x004F00E8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (KinematicCharacterSystem.Interpolate)
			{
				KinematicCharacterSystem.CustomInterpolationUpdate();
			}
		}

		// Token: 0x0600DC7F RID: 56447 RVA: 0x004F1EF8 File Offset: 0x004F00F8
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

		// Token: 0x0600DC80 RID: 56448 RVA: 0x004F1FC8 File Offset: 0x004F01C8
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

		// Token: 0x0600DC81 RID: 56449 RVA: 0x004F2098 File Offset: 0x004F0298
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

		// Token: 0x0600DC82 RID: 56450 RVA: 0x004F2154 File Offset: 0x004F0354
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

		// Token: 0x0400A74A RID: 42826
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static KinematicCharacterSystem _instance;

		// Token: 0x0400A74B RID: 42827
		public static List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>(100);

		// Token: 0x0400A74C RID: 42828
		public static List<PhysicsMover> PhysicsMovers = new List<PhysicsMover>(100);

		// Token: 0x0400A74D RID: 42829
		public static bool AutoSimulation = true;

		// Token: 0x0400A74E RID: 42830
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationStartTime = -1f;

		// Token: 0x0400A74F RID: 42831
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationDeltaTime = -1f;

		// Token: 0x0400A750 RID: 42832
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int CharacterMotorsBaseCapacity = 100;

		// Token: 0x0400A751 RID: 42833
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int PhysicsMoversBaseCapacity = 100;

		// Token: 0x0400A752 RID: 42834
		public static bool Interpolate = true;
	}
}

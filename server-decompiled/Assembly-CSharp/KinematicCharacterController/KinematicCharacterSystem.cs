using System;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D59 RID: 7513
	[DefaultExecutionOrder(-100)]
	public class KinematicCharacterSystem : MonoBehaviour
	{
		// Token: 0x0600DE14 RID: 56852 RVA: 0x004F8830 File Offset: 0x004F6A30
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

		// Token: 0x0600DE15 RID: 56853 RVA: 0x004F8865 File Offset: 0x004F6A65
		public static KinematicCharacterSystem GetInstance()
		{
			return KinematicCharacterSystem._instance;
		}

		// Token: 0x0600DE16 RID: 56854 RVA: 0x004F886C File Offset: 0x004F6A6C
		public static void SetCharacterMotorsCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.CharacterMotors.Count)
			{
				capacity = KinematicCharacterSystem.CharacterMotors.Count;
			}
			KinematicCharacterSystem.CharacterMotors.Capacity = capacity;
		}

		// Token: 0x0600DE17 RID: 56855 RVA: 0x004F8892 File Offset: 0x004F6A92
		public static void RegisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Add(motor);
		}

		// Token: 0x0600DE18 RID: 56856 RVA: 0x004F889F File Offset: 0x004F6A9F
		public static void UnregisterCharacterMotor(KinematicCharacterMotor motor)
		{
			KinematicCharacterSystem.CharacterMotors.Remove(motor);
		}

		// Token: 0x0600DE19 RID: 56857 RVA: 0x004F88AD File Offset: 0x004F6AAD
		public static void SetPhysicsMoversCapacity(int capacity)
		{
			if (capacity < KinematicCharacterSystem.PhysicsMovers.Count)
			{
				capacity = KinematicCharacterSystem.PhysicsMovers.Count;
			}
			KinematicCharacterSystem.PhysicsMovers.Capacity = capacity;
		}

		// Token: 0x0600DE1A RID: 56858 RVA: 0x004F88D3 File Offset: 0x004F6AD3
		public static void RegisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Add(mover);
			mover.Rigidbody.interpolation = RigidbodyInterpolation.None;
		}

		// Token: 0x0600DE1B RID: 56859 RVA: 0x004F88EC File Offset: 0x004F6AEC
		public static void UnregisterPhysicsMover(PhysicsMover mover)
		{
			KinematicCharacterSystem.PhysicsMovers.Remove(mover);
		}

		// Token: 0x0600DE1C RID: 56860 RVA: 0x00147A47 File Offset: 0x00145C47
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisable()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600DE1D RID: 56861 RVA: 0x004F88FA File Offset: 0x004F6AFA
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			KinematicCharacterSystem._instance = this;
		}

		// Token: 0x0600DE1E RID: 56862 RVA: 0x004F8904 File Offset: 0x004F6B04
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

		// Token: 0x0600DE1F RID: 56863 RVA: 0x004F895C File Offset: 0x004F6B5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (KinematicCharacterSystem.Interpolate)
			{
				KinematicCharacterSystem.CustomInterpolationUpdate();
			}
		}

		// Token: 0x0600DE20 RID: 56864 RVA: 0x004F896C File Offset: 0x004F6B6C
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

		// Token: 0x0600DE21 RID: 56865 RVA: 0x004F8A3C File Offset: 0x004F6C3C
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

		// Token: 0x0600DE22 RID: 56866 RVA: 0x004F8B0C File Offset: 0x004F6D0C
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

		// Token: 0x0600DE23 RID: 56867 RVA: 0x004F8BC8 File Offset: 0x004F6DC8
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

		// Token: 0x0400A87F RID: 43135
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static KinematicCharacterSystem _instance;

		// Token: 0x0400A880 RID: 43136
		public static List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>(100);

		// Token: 0x0400A881 RID: 43137
		public static List<PhysicsMover> PhysicsMovers = new List<PhysicsMover>(100);

		// Token: 0x0400A882 RID: 43138
		public static bool AutoSimulation = true;

		// Token: 0x0400A883 RID: 43139
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationStartTime = -1f;

		// Token: 0x0400A884 RID: 43140
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static float _lastCustomInterpolationDeltaTime = -1f;

		// Token: 0x0400A885 RID: 43141
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int CharacterMotorsBaseCapacity = 100;

		// Token: 0x0400A886 RID: 43142
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int PhysicsMoversBaseCapacity = 100;

		// Token: 0x0400A887 RID: 43143
		public static bool Interpolate = true;
	}
}

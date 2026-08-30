using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.PlayerLoop
{
	// Token: 0x02000331 RID: 817
	[RequiredByNativeCode]
	[MovedFrom("UnityEngine.Experimental.PlayerLoop")]
	public struct Initialization
	{
		// Token: 0x02000332 RID: 818
		[RequiredByNativeCode]
		public struct ProfilerStartFrame
		{
		}

		// Token: 0x02000333 RID: 819
		[Obsolete("PlayerUpdateTime player loop component has been moved to its own category called TimeUpdate. (UnityUpgradable) -> UnityEngine.PlayerLoop.TimeUpdate/WaitForLastPresentationAndUpdateTime", true)]
		public struct PlayerUpdateTime
		{
		}

		// Token: 0x02000334 RID: 820
		[RequiredByNativeCode]
		public struct UpdateCameraMotionVectors
		{
		}

		// Token: 0x02000335 RID: 821
		[RequiredByNativeCode]
		public struct DirectorSampleTime
		{
		}

		// Token: 0x02000336 RID: 822
		[RequiredByNativeCode]
		public struct AsyncUploadTimeSlicedUpdate
		{
		}

		// Token: 0x02000337 RID: 823
		[RequiredByNativeCode]
		public struct SynchronizeState
		{
		}

		// Token: 0x02000338 RID: 824
		[RequiredByNativeCode]
		public struct SynchronizeInputs
		{
		}

		// Token: 0x02000339 RID: 825
		[RequiredByNativeCode]
		public struct XREarlyUpdate
		{
		}
	}
}

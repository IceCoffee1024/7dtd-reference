using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000406 RID: 1030
	[UsedByNativeCode]
	public enum GraphicsDeviceType
	{
		// Token: 0x04000C29 RID: 3113
		[Obsolete("OpenGL2 is no longer supported in Unity 5.5+")]
		OpenGL2,
		// Token: 0x04000C2A RID: 3114
		[Obsolete("Direct3D 9 is no longer supported in Unity 2017.2+")]
		Direct3D9,
		// Token: 0x04000C2B RID: 3115
		Direct3D11,
		// Token: 0x04000C2C RID: 3116
		[Obsolete("PS3 is no longer supported in Unity 5.5+")]
		PlayStation3,
		// Token: 0x04000C2D RID: 3117
		Null,
		// Token: 0x04000C2E RID: 3118
		[Obsolete("Xbox360 is no longer supported in Unity 5.5+")]
		Xbox360 = 6,
		// Token: 0x04000C2F RID: 3119
		OpenGLES2 = 8,
		// Token: 0x04000C30 RID: 3120
		OpenGLES3 = 11,
		// Token: 0x04000C31 RID: 3121
		[Obsolete("PVita is no longer supported as of Unity 2018")]
		PlayStationVita,
		// Token: 0x04000C32 RID: 3122
		PlayStation4,
		// Token: 0x04000C33 RID: 3123
		XboxOne,
		// Token: 0x04000C34 RID: 3124
		[Obsolete("PlayStationMobile is no longer supported in Unity 5.3+")]
		PlayStationMobile,
		// Token: 0x04000C35 RID: 3125
		Metal,
		// Token: 0x04000C36 RID: 3126
		OpenGLCore,
		// Token: 0x04000C37 RID: 3127
		Direct3D12,
		// Token: 0x04000C38 RID: 3128
		[Obsolete("Nintendo 3DS support is unavailable since 2018.1")]
		N3DS,
		// Token: 0x04000C39 RID: 3129
		Vulkan = 21,
		// Token: 0x04000C3A RID: 3130
		Switch,
		// Token: 0x04000C3B RID: 3131
		XboxOneD3D12,
		// Token: 0x04000C3C RID: 3132
		GameCoreXboxOne,
		// Token: 0x04000C3D RID: 3133
		[Obsolete("GameCoreScarlett is deprecated, please use GameCoreXboxSeries (UnityUpgradable) -> GameCoreXboxSeries", false)]
		GameCoreScarlett = -1,
		// Token: 0x04000C3E RID: 3134
		GameCoreXboxSeries = 25,
		// Token: 0x04000C3F RID: 3135
		PlayStation5,
		// Token: 0x04000C40 RID: 3136
		PlayStation5NGGC
	}
}

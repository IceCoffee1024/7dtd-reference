using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000290 RID: 656
	[StaticAccessor("GetTimeManager()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Input/TimeManager.h")]
	public class Time
	{
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001BBC RID: 7100
		[NativeProperty("CurTime")]
		public static extern float time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001BBD RID: 7101
		[NativeProperty("CurTime")]
		public static extern double timeAsDouble
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001BBE RID: 7102
		[NativeProperty("TimeSinceSceneLoad")]
		public static extern float timeSinceLevelLoad
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001BBF RID: 7103
		[NativeProperty("TimeSinceSceneLoad")]
		public static extern double timeSinceLevelLoadAsDouble
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001BC0 RID: 7104
		public static extern float deltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001BC1 RID: 7105
		public static extern float fixedTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001BC2 RID: 7106
		[NativeProperty("FixedTime")]
		public static extern double fixedTimeAsDouble
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001BC3 RID: 7107
		public static extern float unscaledTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001BC4 RID: 7108
		[NativeProperty("UnscaledTime")]
		public static extern double unscaledTimeAsDouble
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001BC5 RID: 7109
		public static extern float fixedUnscaledTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001BC6 RID: 7110
		[NativeProperty("FixedUnscaledTime")]
		public static extern double fixedUnscaledTimeAsDouble
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001BC7 RID: 7111
		public static extern float unscaledDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001BC8 RID: 7112
		public static extern float fixedUnscaledDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001BC9 RID: 7113
		// (set) Token: 0x06001BCA RID: 7114
		public static extern float fixedDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001BCB RID: 7115
		// (set) Token: 0x06001BCC RID: 7116
		public static extern float maximumDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001BCD RID: 7117
		public static extern float smoothDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001BCE RID: 7118
		// (set) Token: 0x06001BCF RID: 7119
		public static extern float maximumParticleDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001BD0 RID: 7120
		// (set) Token: 0x06001BD1 RID: 7121
		public static extern float timeScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001BD2 RID: 7122
		public static extern int frameCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001BD3 RID: 7123
		[NativeProperty("RenderFrameCount")]
		public static extern int renderedFrameCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001BD4 RID: 7124
		[NativeProperty("Realtime")]
		public static extern float realtimeSinceStartup
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001BD5 RID: 7125
		[NativeProperty("Realtime")]
		public static extern double realtimeSinceStartupAsDouble
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001BD6 RID: 7126
		// (set) Token: 0x06001BD7 RID: 7127
		public static extern float captureDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x0002E1F8 File Offset: 0x0002C3F8
		// (set) Token: 0x06001BD9 RID: 7129 RVA: 0x0002E22A File Offset: 0x0002C42A
		public static int captureFramerate
		{
			get
			{
				return (Time.captureDeltaTime == 0f) ? 0 : ((int)Mathf.Round(1f / Time.captureDeltaTime));
			}
			set
			{
				Time.captureDeltaTime = ((value == 0) ? 0f : (1f / (float)value));
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001BDA RID: 7130
		public static extern bool inFixedTimeStep
		{
			[NativeName("IsUsingFixedTimeStep")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}

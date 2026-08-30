using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x020019EA RID: 6634
	public static class SignalProcessing
	{
		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x0600C97E RID: 51582 RVA: 0x0049F40B File Offset: 0x0049D60B
		public static float SuspenseRange
		{
			get
			{
				return SignalProcessing.CombatReadyThreshold - SignalProcessing.SuspenseThreshold;
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x0600C97F RID: 51583 RVA: 0x0049F418 File Offset: 0x0049D618
		public static float SuspenseThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x0600C980 RID: 51584 RVA: 0x0014A743 File Offset: 0x00148943
		public static float CombatReadyThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x0400990A RID: 39178
		public const float cSilentVolume = -80f;

		// Token: 0x0400990B RID: 39179
		public const float cMaxLPFCutoff = 22000f;

		// Token: 0x0400990C RID: 39180
		public const float cPauseLowPassFilterCutoff = 500f;

		// Token: 0x0400990D RID: 39181
		public const double cdBFullScaleBase = 1.12246204831;

		// Token: 0x0400990E RID: 39182
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationLowPassCutoff = "ExpLPFCutOff";

		// Token: 0x0400990F RID: 39183
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbDryLevel = "ExpReverbDryLevel";

		// Token: 0x04009910 RID: 39184
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoom = "ExpReverbRoom";

		// Token: 0x04009911 RID: 39185
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomHF = "ExpReverbRoomHF";

		// Token: 0x04009912 RID: 39186
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationDecayHFRatio = "DecayHFRatio";

		// Token: 0x04009913 RID: 39187
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReflectDelay = "ExpReflectDelay";

		// Token: 0x04009914 RID: 39188
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationHFReference = "ExpHFReference";

		// Token: 0x04009915 RID: 39189
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomLF = "ExpReverbRoomLF";

		// Token: 0x04009916 RID: 39190
		public static readonly Dictionary<string, Curve> DspCurves = new Dictionary<string, Curve>
		{
			{
				"ExpLPFCutOff",
				new ExponentialCurve(2.0, 22000f, 750f, 0.25f, 0.5f)
			},
			{
				"ExpReverbDryLevel",
				new LogarithmicCurve(2.0, 600.0, 0f, -600f, 0.25f, 0.5f)
			},
			{
				"ExpReverbRoom",
				new LogarithmicCurve(2.0, 600.0, -10000f, -250f, 0.25f, 0.5f)
			},
			{
				"ExpReverbRoomHF",
				new LogarithmicCurve(2.0, 600.0, -600f, -250f, 0.25f, 0.5f)
			},
			{
				"DecayHFRatio",
				new LinearCurve(0.1f, 1f, 0.25f, 0.5f)
			},
			{
				"ExpReflectDelay",
				new LinearCurve(0.02f, 0.2f, 0.25f, 0.5f)
			},
			{
				"ExpHFReference",
				new ExponentialCurve(2.0, 1244.508f, 1174.659f, 0.25f, 0.5f)
			},
			{
				"ExpReverbRoomLF",
				new LogarithmicCurve(2.0, 600.0, -600f, 0f, 0.25f, 0.5f)
			}
		};
	}
}

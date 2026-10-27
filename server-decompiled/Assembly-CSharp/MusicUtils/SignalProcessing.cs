using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x02001A6E RID: 6766
	public static class SignalProcessing
	{
		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x0600CD73 RID: 52595 RVA: 0x004AD7C7 File Offset: 0x004AB9C7
		public static float SuspenseRange
		{
			get
			{
				return SignalProcessing.CombatReadyThreshold - SignalProcessing.SuspenseThreshold;
			}
		}

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x0600CD74 RID: 52596 RVA: 0x004AD7D4 File Offset: 0x004AB9D4
		public static float SuspenseThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x0600CD75 RID: 52597 RVA: 0x0014DDC3 File Offset: 0x0014BFC3
		public static float CombatReadyThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x04009BEA RID: 39914
		public const float cSilentVolume = -80f;

		// Token: 0x04009BEB RID: 39915
		public const float cMaxLPFCutoff = 22000f;

		// Token: 0x04009BEC RID: 39916
		public const float cPauseLowPassFilterCutoff = 500f;

		// Token: 0x04009BED RID: 39917
		public const double cdBFullScaleBase = 1.12246204831;

		// Token: 0x04009BEE RID: 39918
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationLowPassCutoff = "ExpLPFCutOff";

		// Token: 0x04009BEF RID: 39919
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbDryLevel = "ExpReverbDryLevel";

		// Token: 0x04009BF0 RID: 39920
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoom = "ExpReverbRoom";

		// Token: 0x04009BF1 RID: 39921
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomHF = "ExpReverbRoomHF";

		// Token: 0x04009BF2 RID: 39922
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationDecayHFRatio = "DecayHFRatio";

		// Token: 0x04009BF3 RID: 39923
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReflectDelay = "ExpReflectDelay";

		// Token: 0x04009BF4 RID: 39924
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationHFReference = "ExpHFReference";

		// Token: 0x04009BF5 RID: 39925
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomLF = "ExpReverbRoomLF";

		// Token: 0x04009BF6 RID: 39926
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

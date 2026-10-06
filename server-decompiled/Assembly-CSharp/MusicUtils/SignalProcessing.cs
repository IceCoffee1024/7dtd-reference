using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x02001A3D RID: 6717
	public static class SignalProcessing
	{
		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600CC5F RID: 52319 RVA: 0x004A9CCB File Offset: 0x004A7ECB
		public static float SuspenseRange
		{
			get
			{
				return SignalProcessing.CombatReadyThreshold - SignalProcessing.SuspenseThreshold;
			}
		}

		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600CC60 RID: 52320 RVA: 0x004A9CD8 File Offset: 0x004A7ED8
		public static float SuspenseThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600CC61 RID: 52321 RVA: 0x0014D1CF File Offset: 0x0014B3CF
		public static float CombatReadyThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x04009B01 RID: 39681
		public const float cSilentVolume = -80f;

		// Token: 0x04009B02 RID: 39682
		public const float cMaxLPFCutoff = 22000f;

		// Token: 0x04009B03 RID: 39683
		public const float cPauseLowPassFilterCutoff = 500f;

		// Token: 0x04009B04 RID: 39684
		public const double cdBFullScaleBase = 1.12246204831;

		// Token: 0x04009B05 RID: 39685
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationLowPassCutoff = "ExpLPFCutOff";

		// Token: 0x04009B06 RID: 39686
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbDryLevel = "ExpReverbDryLevel";

		// Token: 0x04009B07 RID: 39687
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoom = "ExpReverbRoom";

		// Token: 0x04009B08 RID: 39688
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomHF = "ExpReverbRoomHF";

		// Token: 0x04009B09 RID: 39689
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationDecayHFRatio = "DecayHFRatio";

		// Token: 0x04009B0A RID: 39690
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReflectDelay = "ExpReflectDelay";

		// Token: 0x04009B0B RID: 39691
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationHFReference = "ExpHFReference";

		// Token: 0x04009B0C RID: 39692
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomLF = "ExpReverbRoomLF";

		// Token: 0x04009B0D RID: 39693
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

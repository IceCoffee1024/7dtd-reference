using System;
using System.Collections.Generic;

namespace MusicUtils
{
	// Token: 0x02001A11 RID: 6673
	public static class SignalProcessing
	{
		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x0600CB07 RID: 51975 RVA: 0x004A57FF File Offset: 0x004A39FF
		public static float SuspenseRange
		{
			get
			{
				return SignalProcessing.CombatReadyThreshold - SignalProcessing.SuspenseThreshold;
			}
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x0600CB08 RID: 51976 RVA: 0x004A580C File Offset: 0x004A3A0C
		public static float SuspenseThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x0600CB09 RID: 51977 RVA: 0x0014C85F File Offset: 0x0014AA5F
		public static float CombatReadyThreshold
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x04009A3B RID: 39483
		public const float cSilentVolume = -80f;

		// Token: 0x04009A3C RID: 39484
		public const float cMaxLPFCutoff = 22000f;

		// Token: 0x04009A3D RID: 39485
		public const float cPauseLowPassFilterCutoff = 500f;

		// Token: 0x04009A3E RID: 39486
		public const double cdBFullScaleBase = 1.12246204831;

		// Token: 0x04009A3F RID: 39487
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationLowPassCutoff = "ExpLPFCutOff";

		// Token: 0x04009A40 RID: 39488
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbDryLevel = "ExpReverbDryLevel";

		// Token: 0x04009A41 RID: 39489
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoom = "ExpReverbRoom";

		// Token: 0x04009A42 RID: 39490
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomHF = "ExpReverbRoomHF";

		// Token: 0x04009A43 RID: 39491
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationDecayHFRatio = "DecayHFRatio";

		// Token: 0x04009A44 RID: 39492
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReflectDelay = "ExpReflectDelay";

		// Token: 0x04009A45 RID: 39493
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationHFReference = "ExpHFReference";

		// Token: 0x04009A46 RID: 39494
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cExplorationReverbRoomLF = "ExpReverbRoomLF";

		// Token: 0x04009A47 RID: 39495
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

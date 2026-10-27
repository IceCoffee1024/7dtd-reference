using System;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AE7 RID: 6887
	public struct ThreatLevel : IThreatLevel
	{
		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x0600CF7D RID: 53117 RVA: 0x004B2C67 File Offset: 0x004B0E67
		// (set) Token: 0x0600CF7E RID: 53118 RVA: 0x004B2C6F File Offset: 0x004B0E6F
		public ThreatLevelType Category
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x0600CF7F RID: 53119 RVA: 0x004B2C78 File Offset: 0x004B0E78
		// (set) Token: 0x0600CF80 RID: 53120 RVA: 0x004B2C80 File Offset: 0x004B0E80
		public float Numeric
		{
			get
			{
				return this.numeric;
			}
			set
			{
				if (value < 0.3f && Time.time - this.suspenseTimer <= 30f)
				{
					this.numeric = 0.3f;
					this.Category = ThreatLevelType.Spooked;
					return;
				}
				this.numeric = value;
				this.Category = ((value < 0.3f) ? ThreatLevelType.Safe : ((value < 0.7f) ? ThreatLevelType.Spooked : ThreatLevelType.Panicked));
				if (this.Category == ThreatLevelType.Spooked)
				{
					this.suspenseTimer = Time.time;
					return;
				}
				if (this.Category == ThreatLevelType.Panicked)
				{
					this.suspenseTimer = 0f;
				}
			}
		}

		// Token: 0x04009D26 RID: 40230
		public const float SPOOKED_THRESHOLD = 0.3f;

		// Token: 0x04009D27 RID: 40231
		public const float PANICKED_THRESHOLD = 0.7f;

		// Token: 0x04009D28 RID: 40232
		[PublicizedFrom(EAccessModifier.Private)]
		public const float SPOOKED_DECAY_TIME = 30f;

		// Token: 0x04009D29 RID: 40233
		[PublicizedFrom(EAccessModifier.Private)]
		public float suspenseTimer;

		// Token: 0x04009D2B RID: 40235
		[PublicizedFrom(EAccessModifier.Private)]
		public float numeric;
	}
}

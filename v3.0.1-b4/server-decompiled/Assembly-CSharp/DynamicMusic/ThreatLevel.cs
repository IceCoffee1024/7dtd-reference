using System;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A63 RID: 6755
	public struct ThreatLevel : IThreatLevel
	{
		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x0600CB88 RID: 52104 RVA: 0x004A48AB File Offset: 0x004A2AAB
		// (set) Token: 0x0600CB89 RID: 52105 RVA: 0x004A48B3 File Offset: 0x004A2AB3
		public ThreatLevelType Category
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x0600CB8A RID: 52106 RVA: 0x004A48BC File Offset: 0x004A2ABC
		// (set) Token: 0x0600CB8B RID: 52107 RVA: 0x004A48C4 File Offset: 0x004A2AC4
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

		// Token: 0x04009A46 RID: 39494
		public const float SPOOKED_THRESHOLD = 0.3f;

		// Token: 0x04009A47 RID: 39495
		public const float PANICKED_THRESHOLD = 0.7f;

		// Token: 0x04009A48 RID: 39496
		[PublicizedFrom(EAccessModifier.Private)]
		public const float SPOOKED_DECAY_TIME = 30f;

		// Token: 0x04009A49 RID: 39497
		[PublicizedFrom(EAccessModifier.Private)]
		public float suspenseTimer;

		// Token: 0x04009A4B RID: 39499
		[PublicizedFrom(EAccessModifier.Private)]
		public float numeric;
	}
}

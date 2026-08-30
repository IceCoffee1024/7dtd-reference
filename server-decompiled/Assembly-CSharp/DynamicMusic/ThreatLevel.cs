using System;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A8A RID: 6794
	public struct ThreatLevel : IThreatLevel
	{
		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x0600CD11 RID: 52497 RVA: 0x004AAC9F File Offset: 0x004A8E9F
		// (set) Token: 0x0600CD12 RID: 52498 RVA: 0x004AACA7 File Offset: 0x004A8EA7
		public ThreatLevelType Category
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x0600CD13 RID: 52499 RVA: 0x004AACB0 File Offset: 0x004A8EB0
		// (set) Token: 0x0600CD14 RID: 52500 RVA: 0x004AACB8 File Offset: 0x004A8EB8
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

		// Token: 0x04009B77 RID: 39799
		public const float SPOOKED_THRESHOLD = 0.3f;

		// Token: 0x04009B78 RID: 39800
		public const float PANICKED_THRESHOLD = 0.7f;

		// Token: 0x04009B79 RID: 39801
		[PublicizedFrom(EAccessModifier.Private)]
		public const float SPOOKED_DECAY_TIME = 30f;

		// Token: 0x04009B7A RID: 39802
		[PublicizedFrom(EAccessModifier.Private)]
		public float suspenseTimer;

		// Token: 0x04009B7C RID: 39804
		[PublicizedFrom(EAccessModifier.Private)]
		public float numeric;
	}
}

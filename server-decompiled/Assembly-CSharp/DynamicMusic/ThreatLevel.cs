using System;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AB6 RID: 6838
	public struct ThreatLevel : IThreatLevel
	{
		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x0600CE69 RID: 52841 RVA: 0x004AF16B File Offset: 0x004AD36B
		// (set) Token: 0x0600CE6A RID: 52842 RVA: 0x004AF173 File Offset: 0x004AD373
		public ThreatLevelType Category
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x0600CE6B RID: 52843 RVA: 0x004AF17C File Offset: 0x004AD37C
		// (set) Token: 0x0600CE6C RID: 52844 RVA: 0x004AF184 File Offset: 0x004AD384
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

		// Token: 0x04009C3D RID: 39997
		public const float SPOOKED_THRESHOLD = 0.3f;

		// Token: 0x04009C3E RID: 39998
		public const float PANICKED_THRESHOLD = 0.7f;

		// Token: 0x04009C3F RID: 39999
		[PublicizedFrom(EAccessModifier.Private)]
		public const float SPOOKED_DECAY_TIME = 30f;

		// Token: 0x04009C40 RID: 40000
		[PublicizedFrom(EAccessModifier.Private)]
		public float suspenseTimer;

		// Token: 0x04009C42 RID: 40002
		[PublicizedFrom(EAccessModifier.Private)]
		public float numeric;
	}
}

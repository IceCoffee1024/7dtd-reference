using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x0200185A RID: 6234
	public class PubSubGoalMessage : BasePubSubMessage
	{
		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x0600C000 RID: 49152 RVA: 0x0047361D File Offset: 0x0047181D
		// (set) Token: 0x0600C001 RID: 49153 RVA: 0x00473625 File Offset: 0x00471825
		public new string type { get; set; }

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x0600C002 RID: 49154 RVA: 0x0047362E File Offset: 0x0047182E
		public string TheType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x0600C003 RID: 49155 RVA: 0x00473636 File Offset: 0x00471836
		// (set) Token: 0x0600C004 RID: 49156 RVA: 0x0047363E File Offset: 0x0047183E
		public PubSubGoalMessage.GoalData data { get; set; }

		// Token: 0x0600C005 RID: 49157 RVA: 0x00473647 File Offset: 0x00471847
		public static PubSubGoalMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubGoalMessage>(message);
		}

		// Token: 0x0200185B RID: 6235
		public class GoalData
		{
			// Token: 0x1700174F RID: 5967
			// (get) Token: 0x0600C007 RID: 49159 RVA: 0x0047364F File Offset: 0x0047184F
			// (set) Token: 0x0600C008 RID: 49160 RVA: 0x00473657 File Offset: 0x00471857
			public PubSubGoalMessage.Goal goal { get; set; }
		}

		// Token: 0x0200185C RID: 6236
		public class Goal
		{
			// Token: 0x17001750 RID: 5968
			// (get) Token: 0x0600C00A RID: 49162 RVA: 0x00473660 File Offset: 0x00471860
			// (set) Token: 0x0600C00B RID: 49163 RVA: 0x00473668 File Offset: 0x00471868
			public string contributionType { get; set; }

			// Token: 0x17001751 RID: 5969
			// (get) Token: 0x0600C00C RID: 49164 RVA: 0x00473671 File Offset: 0x00471871
			// (set) Token: 0x0600C00D RID: 49165 RVA: 0x00473679 File Offset: 0x00471879
			public string state { get; set; }

			// Token: 0x17001752 RID: 5970
			// (get) Token: 0x0600C00E RID: 49166 RVA: 0x00473682 File Offset: 0x00471882
			// (set) Token: 0x0600C00F RID: 49167 RVA: 0x0047368A File Offset: 0x0047188A
			public int currentContributions { get; set; }

			// Token: 0x17001753 RID: 5971
			// (get) Token: 0x0600C010 RID: 49168 RVA: 0x00473693 File Offset: 0x00471893
			// (set) Token: 0x0600C011 RID: 49169 RVA: 0x0047369B File Offset: 0x0047189B
			public int targetContributions { get; set; }
		}
	}
}

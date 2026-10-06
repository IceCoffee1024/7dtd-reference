using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018AC RID: 6316
	public class PubSubGoalMessage : BasePubSubMessage
	{
		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x0600C2DD RID: 49885 RVA: 0x0047D8F5 File Offset: 0x0047BAF5
		// (set) Token: 0x0600C2DE RID: 49886 RVA: 0x0047D8FD File Offset: 0x0047BAFD
		public new string type { get; set; }

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x0600C2DF RID: 49887 RVA: 0x0047D906 File Offset: 0x0047BB06
		public string TheType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x0600C2E0 RID: 49888 RVA: 0x0047D90E File Offset: 0x0047BB0E
		// (set) Token: 0x0600C2E1 RID: 49889 RVA: 0x0047D916 File Offset: 0x0047BB16
		public PubSubGoalMessage.GoalData data { get; set; }

		// Token: 0x0600C2E2 RID: 49890 RVA: 0x0047D91F File Offset: 0x0047BB1F
		public static PubSubGoalMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubGoalMessage>(message);
		}

		// Token: 0x020018AD RID: 6317
		public class GoalData
		{
			// Token: 0x17001863 RID: 6243
			// (get) Token: 0x0600C2E4 RID: 49892 RVA: 0x0047D927 File Offset: 0x0047BB27
			// (set) Token: 0x0600C2E5 RID: 49893 RVA: 0x0047D92F File Offset: 0x0047BB2F
			public PubSubGoalMessage.Goal goal { get; set; }
		}

		// Token: 0x020018AE RID: 6318
		public class Goal
		{
			// Token: 0x17001864 RID: 6244
			// (get) Token: 0x0600C2E7 RID: 49895 RVA: 0x0047D938 File Offset: 0x0047BB38
			// (set) Token: 0x0600C2E8 RID: 49896 RVA: 0x0047D940 File Offset: 0x0047BB40
			public string contributionType { get; set; }

			// Token: 0x17001865 RID: 6245
			// (get) Token: 0x0600C2E9 RID: 49897 RVA: 0x0047D949 File Offset: 0x0047BB49
			// (set) Token: 0x0600C2EA RID: 49898 RVA: 0x0047D951 File Offset: 0x0047BB51
			public string state { get; set; }

			// Token: 0x17001866 RID: 6246
			// (get) Token: 0x0600C2EB RID: 49899 RVA: 0x0047D95A File Offset: 0x0047BB5A
			// (set) Token: 0x0600C2EC RID: 49900 RVA: 0x0047D962 File Offset: 0x0047BB62
			public int currentContributions { get; set; }

			// Token: 0x17001867 RID: 6247
			// (get) Token: 0x0600C2ED RID: 49901 RVA: 0x0047D96B File Offset: 0x0047BB6B
			// (set) Token: 0x0600C2EE RID: 49902 RVA: 0x0047D973 File Offset: 0x0047BB73
			public int targetContributions { get; set; }
		}
	}
}

using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x02001880 RID: 6272
	public class PubSubGoalMessage : BasePubSubMessage
	{
		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x0600C185 RID: 49541 RVA: 0x004793B9 File Offset: 0x004775B9
		// (set) Token: 0x0600C186 RID: 49542 RVA: 0x004793C1 File Offset: 0x004775C1
		public new string type { get; set; }

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x0600C187 RID: 49543 RVA: 0x004793CA File Offset: 0x004775CA
		public string TheType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x0600C188 RID: 49544 RVA: 0x004793D2 File Offset: 0x004775D2
		// (set) Token: 0x0600C189 RID: 49545 RVA: 0x004793DA File Offset: 0x004775DA
		public PubSubGoalMessage.GoalData data { get; set; }

		// Token: 0x0600C18A RID: 49546 RVA: 0x004793E3 File Offset: 0x004775E3
		public static PubSubGoalMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubGoalMessage>(message);
		}

		// Token: 0x02001881 RID: 6273
		public class GoalData
		{
			// Token: 0x170017C1 RID: 6081
			// (get) Token: 0x0600C18C RID: 49548 RVA: 0x004793EB File Offset: 0x004775EB
			// (set) Token: 0x0600C18D RID: 49549 RVA: 0x004793F3 File Offset: 0x004775F3
			public PubSubGoalMessage.Goal goal { get; set; }
		}

		// Token: 0x02001882 RID: 6274
		public class Goal
		{
			// Token: 0x170017C2 RID: 6082
			// (get) Token: 0x0600C18F RID: 49551 RVA: 0x004793FC File Offset: 0x004775FC
			// (set) Token: 0x0600C190 RID: 49552 RVA: 0x00479404 File Offset: 0x00477604
			public string contributionType { get; set; }

			// Token: 0x170017C3 RID: 6083
			// (get) Token: 0x0600C191 RID: 49553 RVA: 0x0047940D File Offset: 0x0047760D
			// (set) Token: 0x0600C192 RID: 49554 RVA: 0x00479415 File Offset: 0x00477615
			public string state { get; set; }

			// Token: 0x170017C4 RID: 6084
			// (get) Token: 0x0600C193 RID: 49555 RVA: 0x0047941E File Offset: 0x0047761E
			// (set) Token: 0x0600C194 RID: 49556 RVA: 0x00479426 File Offset: 0x00477626
			public int currentContributions { get; set; }

			// Token: 0x170017C5 RID: 6085
			// (get) Token: 0x0600C195 RID: 49557 RVA: 0x0047942F File Offset: 0x0047762F
			// (set) Token: 0x0600C196 RID: 49558 RVA: 0x00479437 File Offset: 0x00477637
			public int targetContributions { get; set; }
		}
	}
}

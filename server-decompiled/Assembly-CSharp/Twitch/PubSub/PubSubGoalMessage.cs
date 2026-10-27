using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018DD RID: 6365
	public class PubSubGoalMessage : BasePubSubMessage
	{
		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x0600C3F1 RID: 50161 RVA: 0x004812A1 File Offset: 0x0047F4A1
		// (set) Token: 0x0600C3F2 RID: 50162 RVA: 0x004812A9 File Offset: 0x0047F4A9
		public new string type { get; set; }

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x0600C3F3 RID: 50163 RVA: 0x004812B2 File Offset: 0x0047F4B2
		public string TheType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x0600C3F4 RID: 50164 RVA: 0x004812BA File Offset: 0x0047F4BA
		// (set) Token: 0x0600C3F5 RID: 50165 RVA: 0x004812C2 File Offset: 0x0047F4C2
		public PubSubGoalMessage.GoalData data { get; set; }

		// Token: 0x0600C3F6 RID: 50166 RVA: 0x004812CB File Offset: 0x0047F4CB
		public static PubSubGoalMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubGoalMessage>(message);
		}

		// Token: 0x020018DE RID: 6366
		public class GoalData
		{
			// Token: 0x170018AB RID: 6315
			// (get) Token: 0x0600C3F8 RID: 50168 RVA: 0x004812D3 File Offset: 0x0047F4D3
			// (set) Token: 0x0600C3F9 RID: 50169 RVA: 0x004812DB File Offset: 0x0047F4DB
			public PubSubGoalMessage.Goal goal { get; set; }
		}

		// Token: 0x020018DF RID: 6367
		public class Goal
		{
			// Token: 0x170018AC RID: 6316
			// (get) Token: 0x0600C3FB RID: 50171 RVA: 0x004812E4 File Offset: 0x0047F4E4
			// (set) Token: 0x0600C3FC RID: 50172 RVA: 0x004812EC File Offset: 0x0047F4EC
			public string contributionType { get; set; }

			// Token: 0x170018AD RID: 6317
			// (get) Token: 0x0600C3FD RID: 50173 RVA: 0x004812F5 File Offset: 0x0047F4F5
			// (set) Token: 0x0600C3FE RID: 50174 RVA: 0x004812FD File Offset: 0x0047F4FD
			public string state { get; set; }

			// Token: 0x170018AE RID: 6318
			// (get) Token: 0x0600C3FF RID: 50175 RVA: 0x00481306 File Offset: 0x0047F506
			// (set) Token: 0x0600C400 RID: 50176 RVA: 0x0048130E File Offset: 0x0047F50E
			public int currentContributions { get; set; }

			// Token: 0x170018AF RID: 6319
			// (get) Token: 0x0600C401 RID: 50177 RVA: 0x00481317 File Offset: 0x0047F517
			// (set) Token: 0x0600C402 RID: 50178 RVA: 0x0048131F File Offset: 0x0047F51F
			public int targetContributions { get; set; }
		}
	}
}

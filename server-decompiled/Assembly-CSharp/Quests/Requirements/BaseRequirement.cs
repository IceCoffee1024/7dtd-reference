using System;

namespace Quests.Requirements
{
	// Token: 0x020018CF RID: 6351
	public abstract class BaseRequirement
	{
		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x0600C40B RID: 50187 RVA: 0x00484D18 File Offset: 0x00482F18
		// (set) Token: 0x0600C40C RID: 50188 RVA: 0x00484D20 File Offset: 0x00482F20
		public string ID { get; set; }

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x0600C40D RID: 50189 RVA: 0x00484D29 File Offset: 0x00482F29
		// (set) Token: 0x0600C40E RID: 50190 RVA: 0x00484D31 File Offset: 0x00482F31
		public string Value { get; set; }

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x0600C40F RID: 50191 RVA: 0x00484D3A File Offset: 0x00482F3A
		// (set) Token: 0x0600C410 RID: 50192 RVA: 0x00484D42 File Offset: 0x00482F42
		public bool Complete
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x0600C411 RID: 50193 RVA: 0x00484D4B File Offset: 0x00482F4B
		// (set) Token: 0x0600C412 RID: 50194 RVA: 0x00484D53 File Offset: 0x00482F53
		public Quest OwnerQuest { get; set; }

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x0600C413 RID: 50195 RVA: 0x00484D5C File Offset: 0x00482F5C
		// (set) Token: 0x0600C414 RID: 50196 RVA: 0x00484D64 File Offset: 0x00482F64
		public QuestClass Owner { get; set; }

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x0600C415 RID: 50197 RVA: 0x00484D6D File Offset: 0x00482F6D
		// (set) Token: 0x0600C416 RID: 50198 RVA: 0x00484D75 File Offset: 0x00482F75
		public string Description
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x0600C417 RID: 50199 RVA: 0x00484D7E File Offset: 0x00482F7E
		// (set) Token: 0x0600C418 RID: 50200 RVA: 0x00484D86 File Offset: 0x00482F86
		public string StatusText
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x0600C419 RID: 50201 RVA: 0x00484D8F File Offset: 0x00482F8F
		// (set) Token: 0x0600C41A RID: 50202 RVA: 0x00484D97 File Offset: 0x00482F97
		public int Phase { get; set; }

		// Token: 0x0600C41B RID: 50203 RVA: 0x00484DA0 File Offset: 0x00482FA0
		public virtual void HandleVariables()
		{
			this.ID = this.OwnerQuest.ParseVariable(this.ID);
			this.Value = this.OwnerQuest.ParseVariable(this.Value);
		}

		// Token: 0x0600C41C RID: 50204 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetupRequirement()
		{
		}

		// Token: 0x0600C41D RID: 50205 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool CheckRequirement()
		{
			return false;
		}

		// Token: 0x0600C41E RID: 50206 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual BaseRequirement Clone()
		{
			return null;
		}

		// Token: 0x0600C41F RID: 50207 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseRequirement()
		{
		}

		// Token: 0x04009485 RID: 38021
		public DynamicProperties Properties;

		// Token: 0x020018D0 RID: 6352
		public enum RequirementTypes
		{
			// Token: 0x04009487 RID: 38023
			Buff,
			// Token: 0x04009488 RID: 38024
			Holding,
			// Token: 0x04009489 RID: 38025
			Level,
			// Token: 0x0400948A RID: 38026
			Wearing
		}
	}
}

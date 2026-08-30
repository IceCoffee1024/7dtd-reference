using System;

namespace Quests.Requirements
{
	// Token: 0x0200187D RID: 6269
	public abstract class BaseRequirement
	{
		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x0600C12C RID: 49452 RVA: 0x0047A6B0 File Offset: 0x004788B0
		// (set) Token: 0x0600C12D RID: 49453 RVA: 0x0047A6B8 File Offset: 0x004788B8
		public string ID { get; set; }

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x0600C12E RID: 49454 RVA: 0x0047A6C1 File Offset: 0x004788C1
		// (set) Token: 0x0600C12F RID: 49455 RVA: 0x0047A6C9 File Offset: 0x004788C9
		public string Value { get; set; }

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x0600C130 RID: 49456 RVA: 0x0047A6D2 File Offset: 0x004788D2
		// (set) Token: 0x0600C131 RID: 49457 RVA: 0x0047A6DA File Offset: 0x004788DA
		public bool Complete
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x0600C132 RID: 49458 RVA: 0x0047A6E3 File Offset: 0x004788E3
		// (set) Token: 0x0600C133 RID: 49459 RVA: 0x0047A6EB File Offset: 0x004788EB
		public Quest OwnerQuest { get; set; }

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x0600C134 RID: 49460 RVA: 0x0047A6F4 File Offset: 0x004788F4
		// (set) Token: 0x0600C135 RID: 49461 RVA: 0x0047A6FC File Offset: 0x004788FC
		public QuestClass Owner { get; set; }

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x0600C136 RID: 49462 RVA: 0x0047A705 File Offset: 0x00478905
		// (set) Token: 0x0600C137 RID: 49463 RVA: 0x0047A70D File Offset: 0x0047890D
		public string Description
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x0600C138 RID: 49464 RVA: 0x0047A716 File Offset: 0x00478916
		// (set) Token: 0x0600C139 RID: 49465 RVA: 0x0047A71E File Offset: 0x0047891E
		public string StatusText
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x0600C13A RID: 49466 RVA: 0x0047A727 File Offset: 0x00478927
		// (set) Token: 0x0600C13B RID: 49467 RVA: 0x0047A72F File Offset: 0x0047892F
		public int Phase { get; set; }

		// Token: 0x0600C13C RID: 49468 RVA: 0x0047A738 File Offset: 0x00478938
		public virtual void HandleVariables()
		{
			this.ID = this.OwnerQuest.ParseVariable(this.ID);
			this.Value = this.OwnerQuest.ParseVariable(this.Value);
		}

		// Token: 0x0600C13D RID: 49469 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetupRequirement()
		{
		}

		// Token: 0x0600C13E RID: 49470 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool CheckRequirement()
		{
			return false;
		}

		// Token: 0x0600C13F RID: 49471 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual BaseRequirement Clone()
		{
			return null;
		}

		// Token: 0x0600C140 RID: 49472 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseRequirement()
		{
		}

		// Token: 0x04009296 RID: 37526
		public DynamicProperties Properties;

		// Token: 0x0200187E RID: 6270
		public enum RequirementTypes
		{
			// Token: 0x04009298 RID: 37528
			Buff,
			// Token: 0x04009299 RID: 37529
			Holding,
			// Token: 0x0400929A RID: 37530
			Level,
			// Token: 0x0400929B RID: 37531
			Wearing
		}
	}
}

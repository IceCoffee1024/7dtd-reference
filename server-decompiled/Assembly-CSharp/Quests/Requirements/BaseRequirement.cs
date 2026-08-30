using System;

namespace Quests.Requirements
{
	// Token: 0x020018A3 RID: 6307
	public abstract class BaseRequirement
	{
		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x0600C2B3 RID: 49843 RVA: 0x004807DC File Offset: 0x0047E9DC
		// (set) Token: 0x0600C2B4 RID: 49844 RVA: 0x004807E4 File Offset: 0x0047E9E4
		public string ID { get; set; }

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x0600C2B5 RID: 49845 RVA: 0x004807ED File Offset: 0x0047E9ED
		// (set) Token: 0x0600C2B6 RID: 49846 RVA: 0x004807F5 File Offset: 0x0047E9F5
		public string Value { get; set; }

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x0600C2B7 RID: 49847 RVA: 0x004807FE File Offset: 0x0047E9FE
		// (set) Token: 0x0600C2B8 RID: 49848 RVA: 0x00480806 File Offset: 0x0047EA06
		public bool Complete
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x0600C2B9 RID: 49849 RVA: 0x0048080F File Offset: 0x0047EA0F
		// (set) Token: 0x0600C2BA RID: 49850 RVA: 0x00480817 File Offset: 0x0047EA17
		public Quest OwnerQuest { get; set; }

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x0600C2BB RID: 49851 RVA: 0x00480820 File Offset: 0x0047EA20
		// (set) Token: 0x0600C2BC RID: 49852 RVA: 0x00480828 File Offset: 0x0047EA28
		public QuestClass Owner { get; set; }

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x0600C2BD RID: 49853 RVA: 0x00480831 File Offset: 0x0047EA31
		// (set) Token: 0x0600C2BE RID: 49854 RVA: 0x00480839 File Offset: 0x0047EA39
		public string Description
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x0600C2BF RID: 49855 RVA: 0x00480842 File Offset: 0x0047EA42
		// (set) Token: 0x0600C2C0 RID: 49856 RVA: 0x0048084A File Offset: 0x0047EA4A
		public string StatusText
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x0600C2C1 RID: 49857 RVA: 0x00480853 File Offset: 0x0047EA53
		// (set) Token: 0x0600C2C2 RID: 49858 RVA: 0x0048085B File Offset: 0x0047EA5B
		public int Phase { get; set; }

		// Token: 0x0600C2C3 RID: 49859 RVA: 0x00480864 File Offset: 0x0047EA64
		public virtual void HandleVariables()
		{
			this.ID = this.OwnerQuest.ParseVariable(this.ID);
			this.Value = this.OwnerQuest.ParseVariable(this.Value);
		}

		// Token: 0x0600C2C4 RID: 49860 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetupRequirement()
		{
		}

		// Token: 0x0600C2C5 RID: 49861 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool CheckRequirement()
		{
			return false;
		}

		// Token: 0x0600C2C6 RID: 49862 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual BaseRequirement Clone()
		{
			return null;
		}

		// Token: 0x0600C2C7 RID: 49863 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseRequirement()
		{
		}

		// Token: 0x040093BF RID: 37823
		public DynamicProperties Properties;

		// Token: 0x020018A4 RID: 6308
		public enum RequirementTypes
		{
			// Token: 0x040093C1 RID: 37825
			Buff,
			// Token: 0x040093C2 RID: 37826
			Holding,
			// Token: 0x040093C3 RID: 37827
			Level,
			// Token: 0x040093C4 RID: 37828
			Wearing
		}
	}
}

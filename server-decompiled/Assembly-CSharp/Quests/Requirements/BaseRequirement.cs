using System;

namespace Quests.Requirements
{
	// Token: 0x02001900 RID: 6400
	public abstract class BaseRequirement
	{
		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x0600C51F RID: 50463 RVA: 0x004886C4 File Offset: 0x004868C4
		// (set) Token: 0x0600C520 RID: 50464 RVA: 0x004886CC File Offset: 0x004868CC
		public string ID { get; set; }

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x0600C521 RID: 50465 RVA: 0x004886D5 File Offset: 0x004868D5
		// (set) Token: 0x0600C522 RID: 50466 RVA: 0x004886DD File Offset: 0x004868DD
		public string Value { get; set; }

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x0600C523 RID: 50467 RVA: 0x004886E6 File Offset: 0x004868E6
		// (set) Token: 0x0600C524 RID: 50468 RVA: 0x004886EE File Offset: 0x004868EE
		public bool Complete
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x0600C525 RID: 50469 RVA: 0x004886F7 File Offset: 0x004868F7
		// (set) Token: 0x0600C526 RID: 50470 RVA: 0x004886FF File Offset: 0x004868FF
		public Quest OwnerQuest { get; set; }

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x0600C527 RID: 50471 RVA: 0x00488708 File Offset: 0x00486908
		// (set) Token: 0x0600C528 RID: 50472 RVA: 0x00488710 File Offset: 0x00486910
		public QuestClass Owner { get; set; }

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x0600C529 RID: 50473 RVA: 0x00488719 File Offset: 0x00486919
		// (set) Token: 0x0600C52A RID: 50474 RVA: 0x00488721 File Offset: 0x00486921
		public string Description
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x0600C52B RID: 50475 RVA: 0x0048872A File Offset: 0x0048692A
		// (set) Token: 0x0600C52C RID: 50476 RVA: 0x00488732 File Offset: 0x00486932
		public string StatusText
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x0600C52D RID: 50477 RVA: 0x0048873B File Offset: 0x0048693B
		// (set) Token: 0x0600C52E RID: 50478 RVA: 0x00488743 File Offset: 0x00486943
		public int Phase { get; set; }

		// Token: 0x0600C52F RID: 50479 RVA: 0x0048874C File Offset: 0x0048694C
		public virtual void HandleVariables()
		{
			this.ID = this.OwnerQuest.ParseVariable(this.ID);
			this.Value = this.OwnerQuest.ParseVariable(this.Value);
		}

		// Token: 0x0600C530 RID: 50480 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetupRequirement()
		{
		}

		// Token: 0x0600C531 RID: 50481 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool CheckRequirement()
		{
			return false;
		}

		// Token: 0x0600C532 RID: 50482 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual BaseRequirement Clone()
		{
			return null;
		}

		// Token: 0x0600C533 RID: 50483 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseRequirement()
		{
		}

		// Token: 0x0400956E RID: 38254
		public DynamicProperties Properties;

		// Token: 0x02001901 RID: 6401
		public enum RequirementTypes
		{
			// Token: 0x04009570 RID: 38256
			Buff,
			// Token: 0x04009571 RID: 38257
			Holding,
			// Token: 0x04009572 RID: 38258
			Level,
			// Token: 0x04009573 RID: 38259
			Wearing
		}
	}
}

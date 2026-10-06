using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200192B RID: 6443
	[Preserve]
	public class ChallengeObjectiveCureDebuff : BaseChallengeObjective
	{
		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600C6D2 RID: 50898 RVA: 0x000809CD File Offset: 0x0007EBCD
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.CureDebuff;
			}
		}

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x0600C6D3 RID: 50899 RVA: 0x0048F8C8 File Offset: 0x0048DAC8
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveCure", false, null) + " " + BuffManager.GetBuff(this.buffName).LocalizedName + ":";
			}
		}

		// Token: 0x0600C6D4 RID: 50900 RVA: 0x0048F8F8 File Offset: 0x0048DAF8
		public override void Init()
		{
			if (this.buffName != null)
			{
				this.buffNames = this.buffName.Split(',', StringSplitOptions.None);
				if (this.buffNames.Length > 1)
				{
					this.buffName = this.buffNames[0];
				}
			}
			if (this.itemName != null)
			{
				this.itemNames = this.itemName.Split(',', StringSplitOptions.None);
				if (this.itemNames.Length > 1)
				{
					this.itemName = this.itemNames[0];
				}
			}
		}

		// Token: 0x0600C6D5 RID: 50901 RVA: 0x0048F96F File Offset: 0x0048DB6F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C6D6 RID: 50902 RVA: 0x0048F987 File Offset: 0x0048DB87
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C6D7 RID: 50903 RVA: 0x0048F9A0 File Offset: 0x0048DBA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_UseItem(ItemValue itemValue)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.itemNames.ContainsCaseInsensitive(itemValue.ItemClass.Name) && this.PlayerHasBuff())
			{
				int num = base.Current;
				base.Current = num + 1;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C6D8 RID: 50904 RVA: 0x0048FA08 File Offset: 0x0048DC08
		[PublicizedFrom(EAccessModifier.Private)]
		public bool PlayerHasBuff()
		{
			EntityBuffs buffs = this.Owner.Owner.Player.Buffs;
			for (int i = 0; i < this.buffNames.Length; i++)
			{
				if (buffs.HasBuff(this.buffNames[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C6D9 RID: 50905 RVA: 0x0048FA54 File Offset: 0x0048DC54
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("debuff"))
			{
				this.buffName = e.GetAttribute("debuff");
			}
			if (e.HasAttribute("item"))
			{
				this.itemName = e.GetAttribute("item");
			}
		}

		// Token: 0x0600C6DA RID: 50906 RVA: 0x0048FAB8 File Offset: 0x0048DCB8
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveCureDebuff
			{
				buffName = this.buffName,
				buffNames = this.buffNames,
				itemName = this.itemName,
				itemNames = this.itemNames
			};
		}

		// Token: 0x040095FE RID: 38398
		[PublicizedFrom(EAccessModifier.Private)]
		public string buffName = "";

		// Token: 0x040095FF RID: 38399
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] buffNames;

		// Token: 0x04009600 RID: 38400
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x04009601 RID: 38401
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;
	}
}

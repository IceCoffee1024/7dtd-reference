using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018FF RID: 6399
	[Preserve]
	public class ChallengeObjectiveCureDebuff : BaseChallengeObjective
	{
		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x0600C57A RID: 50554 RVA: 0x00080719 File Offset: 0x0007E919
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.CureDebuff;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x0600C57B RID: 50555 RVA: 0x0048B3D8 File Offset: 0x004895D8
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveCure", false, null) + " " + BuffManager.GetBuff(this.buffName).LocalizedName + ":";
			}
		}

		// Token: 0x0600C57C RID: 50556 RVA: 0x0048B408 File Offset: 0x00489608
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

		// Token: 0x0600C57D RID: 50557 RVA: 0x0048B47F File Offset: 0x0048967F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C57E RID: 50558 RVA: 0x0048B497 File Offset: 0x00489697
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C57F RID: 50559 RVA: 0x0048B4B0 File Offset: 0x004896B0
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

		// Token: 0x0600C580 RID: 50560 RVA: 0x0048B518 File Offset: 0x00489718
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

		// Token: 0x0600C581 RID: 50561 RVA: 0x0048B564 File Offset: 0x00489764
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

		// Token: 0x0600C582 RID: 50562 RVA: 0x0048B5C8 File Offset: 0x004897C8
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

		// Token: 0x04009538 RID: 38200
		[PublicizedFrom(EAccessModifier.Private)]
		public string buffName = "";

		// Token: 0x04009539 RID: 38201
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] buffNames;

		// Token: 0x0400953A RID: 38202
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x0400953B RID: 38203
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;
	}
}

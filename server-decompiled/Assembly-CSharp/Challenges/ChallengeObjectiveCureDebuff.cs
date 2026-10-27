using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200195C RID: 6492
	[Preserve]
	public class ChallengeObjectiveCureDebuff : BaseChallengeObjective
	{
		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x0600C7E6 RID: 51174 RVA: 0x00080BF2 File Offset: 0x0007EDF2
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.CureDebuff;
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600C7E7 RID: 51175 RVA: 0x004932BC File Offset: 0x004914BC
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveCure", false, null) + " " + BuffManager.GetBuff(this.buffName).LocalizedName + ":";
			}
		}

		// Token: 0x0600C7E8 RID: 51176 RVA: 0x004932EC File Offset: 0x004914EC
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

		// Token: 0x0600C7E9 RID: 51177 RVA: 0x00493363 File Offset: 0x00491563
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C7EA RID: 51178 RVA: 0x0049337B File Offset: 0x0049157B
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C7EB RID: 51179 RVA: 0x00493394 File Offset: 0x00491594
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

		// Token: 0x0600C7EC RID: 51180 RVA: 0x004933FC File Offset: 0x004915FC
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

		// Token: 0x0600C7ED RID: 51181 RVA: 0x00493448 File Offset: 0x00491648
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

		// Token: 0x0600C7EE RID: 51182 RVA: 0x004934AC File Offset: 0x004916AC
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

		// Token: 0x040096E7 RID: 38631
		[PublicizedFrom(EAccessModifier.Private)]
		public string buffName = "";

		// Token: 0x040096E8 RID: 38632
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] buffNames;

		// Token: 0x040096E9 RID: 38633
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x040096EA RID: 38634
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;
	}
}

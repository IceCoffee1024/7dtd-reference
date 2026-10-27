using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001970 RID: 6512
	[Preserve]
	public class ChallengeObjectiveUseItem : BaseChallengeObjective
	{
		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x0600C8B3 RID: 51379 RVA: 0x0026CA45 File Offset: 0x0026AC45
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Use;
			}
		}

		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x0600C8B4 RID: 51380 RVA: 0x004964E4 File Offset: 0x004946E4
		public override string DescriptionText
		{
			get
			{
				string text = ((this.overrideText != "") ? this.overrideText : Localization.Get(this.itemName, false, null));
				return Localization.Get("challengeObjectiveUse", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C8B5 RID: 51381 RVA: 0x00496535 File Offset: 0x00494735
		public override void Init()
		{
			if (this.itemName != null)
			{
				this.itemNames = this.itemName.Split(',', StringSplitOptions.None);
				if (this.itemNames.Length > 1)
				{
					this.itemName = this.itemNames[0];
				}
			}
		}

		// Token: 0x0600C8B6 RID: 51382 RVA: 0x0049656C File Offset: 0x0049476C
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C8B7 RID: 51383 RVA: 0x00496584 File Offset: 0x00494784
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C8B8 RID: 51384 RVA: 0x0049659C File Offset: 0x0049479C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_UseItem(ItemValue itemValue)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.itemNames.ContainsCaseInsensitive(itemValue.ItemClass.Name) || (!this.itemTags.IsEmpty && itemValue.ItemClass.ItemTags.Test_AnySet(this.itemTags)))
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

		// Token: 0x0600C8B9 RID: 51385 RVA: 0x00496624 File Offset: 0x00494824
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.itemName = e.GetAttribute("item");
			}
			if (e.HasAttribute("item_tags"))
			{
				this.itemTags = FastTags<TagGroup.Global>.Parse(e.GetAttribute("item_tags"));
			}
			if (e.HasAttribute("override_text_key"))
			{
				this.overrideText = Localization.Get(e.GetAttribute("override_text_key"), false, null);
				return;
			}
			if (e.HasAttribute("override_text"))
			{
				this.overrideText = e.GetAttribute("override_text");
			}
		}

		// Token: 0x0600C8BA RID: 51386 RVA: 0x004966E5 File Offset: 0x004948E5
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveUseItem
			{
				itemName = this.itemName,
				itemNames = this.itemNames,
				itemTags = this.itemTags,
				overrideText = this.overrideText
			};
		}

		// Token: 0x04009731 RID: 38705
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x04009732 RID: 38706
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;

		// Token: 0x04009733 RID: 38707
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideText = "";

		// Token: 0x04009734 RID: 38708
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> itemTags = FastTags<TagGroup.Global>.none;
	}
}

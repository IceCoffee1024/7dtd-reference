using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200193F RID: 6463
	[Preserve]
	public class ChallengeObjectiveUseItem : BaseChallengeObjective
	{
		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x0600C79F RID: 51103 RVA: 0x0026B51D File Offset: 0x0026971D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Use;
			}
		}

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x0600C7A0 RID: 51104 RVA: 0x00492AF0 File Offset: 0x00490CF0
		public override string DescriptionText
		{
			get
			{
				string text = ((this.overrideText != "") ? this.overrideText : Localization.Get(this.itemName, false, null));
				return Localization.Get("challengeObjectiveUse", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C7A1 RID: 51105 RVA: 0x00492B41 File Offset: 0x00490D41
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

		// Token: 0x0600C7A2 RID: 51106 RVA: 0x00492B78 File Offset: 0x00490D78
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C7A3 RID: 51107 RVA: 0x00492B90 File Offset: 0x00490D90
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C7A4 RID: 51108 RVA: 0x00492BA8 File Offset: 0x00490DA8
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

		// Token: 0x0600C7A5 RID: 51109 RVA: 0x00492C30 File Offset: 0x00490E30
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

		// Token: 0x0600C7A6 RID: 51110 RVA: 0x00492CF1 File Offset: 0x00490EF1
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

		// Token: 0x04009648 RID: 38472
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x04009649 RID: 38473
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;

		// Token: 0x0400964A RID: 38474
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideText = "";

		// Token: 0x0400964B RID: 38475
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> itemTags = FastTags<TagGroup.Global>.none;
	}
}

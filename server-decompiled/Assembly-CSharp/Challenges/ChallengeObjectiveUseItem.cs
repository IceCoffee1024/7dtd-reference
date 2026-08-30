using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001913 RID: 6419
	[Preserve]
	public class ChallengeObjectiveUseItem : BaseChallengeObjective
	{
		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x0600C647 RID: 50759 RVA: 0x00268079 File Offset: 0x00266279
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Use;
			}
		}

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x0600C648 RID: 50760 RVA: 0x0048E604 File Offset: 0x0048C804
		public override string DescriptionText
		{
			get
			{
				string text = ((this.overrideText != "") ? this.overrideText : Localization.Get(this.itemName, false, null));
				return Localization.Get("challengeObjectiveUse", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C649 RID: 50761 RVA: 0x0048E655 File Offset: 0x0048C855
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

		// Token: 0x0600C64A RID: 50762 RVA: 0x0048E68C File Offset: 0x0048C88C
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C64B RID: 50763 RVA: 0x0048E6A4 File Offset: 0x0048C8A4
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C64C RID: 50764 RVA: 0x0048E6BC File Offset: 0x0048C8BC
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

		// Token: 0x0600C64D RID: 50765 RVA: 0x0048E744 File Offset: 0x0048C944
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

		// Token: 0x0600C64E RID: 50766 RVA: 0x0048E805 File Offset: 0x0048CA05
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

		// Token: 0x04009582 RID: 38274
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x04009583 RID: 38275
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;

		// Token: 0x04009584 RID: 38276
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideText = "";

		// Token: 0x04009585 RID: 38277
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> itemTags = FastTags<TagGroup.Global>.none;
	}
}

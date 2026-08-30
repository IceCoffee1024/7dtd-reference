using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018ED RID: 6381
	[Preserve]
	public class ChallengeObjectiveUseItem : BaseChallengeObjective
	{
		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x0600C4C0 RID: 50368 RVA: 0x00265191 File Offset: 0x00263391
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Use;
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x0600C4C1 RID: 50369 RVA: 0x004882B8 File Offset: 0x004864B8
		public override string DescriptionText
		{
			get
			{
				string text = ((this.overrideText != "") ? this.overrideText : Localization.Get(this.itemName, false, null));
				return Localization.Get("challengeObjectiveUse", false, null) + " " + text + ":";
			}
		}

		// Token: 0x0600C4C2 RID: 50370 RVA: 0x00488309 File Offset: 0x00486509
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

		// Token: 0x0600C4C3 RID: 50371 RVA: 0x00488340 File Offset: 0x00486540
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.UseItem += this.Current_UseItem;
		}

		// Token: 0x0600C4C4 RID: 50372 RVA: 0x00488358 File Offset: 0x00486558
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.UseItem -= this.Current_UseItem;
		}

		// Token: 0x0600C4C5 RID: 50373 RVA: 0x00488370 File Offset: 0x00486570
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

		// Token: 0x0600C4C6 RID: 50374 RVA: 0x004883F8 File Offset: 0x004865F8
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

		// Token: 0x0600C4C7 RID: 50375 RVA: 0x004884B9 File Offset: 0x004866B9
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

		// Token: 0x04009459 RID: 37977
		[PublicizedFrom(EAccessModifier.Private)]
		public string itemName = "";

		// Token: 0x0400945A RID: 37978
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] itemNames;

		// Token: 0x0400945B RID: 37979
		[PublicizedFrom(EAccessModifier.Private)]
		public string overrideText = "";

		// Token: 0x0400945C RID: 37980
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> itemTags = FastTags<TagGroup.Global>.none;
	}
}

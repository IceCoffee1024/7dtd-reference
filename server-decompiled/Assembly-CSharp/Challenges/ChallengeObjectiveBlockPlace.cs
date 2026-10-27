using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001956 RID: 6486
	[Preserve]
	public class ChallengeObjectiveBlockPlace : BaseChallengeObjective
	{
		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x0600C79E RID: 51102 RVA: 0x00020119 File Offset: 0x0001E319
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockPlace;
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600C79F RID: 51103 RVA: 0x004922B5 File Offset: 0x004904B5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("xuiWorldPrefabsPlace", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C7A0 RID: 51104 RVA: 0x004922DF File Offset: 0x004904DF
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C7A1 RID: 51105 RVA: 0x004922ED File Offset: 0x004904ED
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupPlace((this.alternateItem != "") ? this.alternateItem : this.expectedBlock));
		}

		// Token: 0x0600C7A2 RID: 51106 RVA: 0x00492328 File Offset: 0x00490528
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
		}

		// Token: 0x0600C7A3 RID: 51107 RVA: 0x00492340 File Offset: 0x00490540
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		}

		// Token: 0x0600C7A4 RID: 51108 RVA: 0x00492358 File Offset: 0x00490558
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_BlockPlace(string blockName, Vector3i blockPos)
		{
			bool flag = false;
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.expectedBlock == null || this.expectedBlock == "" || this.expectedBlock.EqualsCaseInsensitive(blockName))
			{
				flag = true;
			}
			if (!flag && this.expectedBlock != null && this.expectedBlock != "")
			{
				Block blockByName = Block.GetBlockByName(this.expectedBlock, true);
				if (blockByName != null && blockByName.SelectAlternates && blockByName.ContainsAlternateBlock(blockName))
				{
					flag = true;
				}
			}
			if (flag)
			{
				int num = base.Current;
				base.Current = num + 1;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C7A5 RID: 51109 RVA: 0x004923F8 File Offset: 0x004905F8
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("block"))
			{
				this.expectedBlock = e.GetAttribute("block");
			}
			if (e.HasAttribute("alternate_item"))
			{
				this.alternateItem = e.GetAttribute("alternate_item");
			}
		}

		// Token: 0x0600C7A6 RID: 51110 RVA: 0x0049245C File Offset: 0x0049065C
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBlockPlace
			{
				expectedBlock = this.expectedBlock,
				alternateItem = this.alternateItem
			};
		}

		// Token: 0x040096D3 RID: 38611
		public string expectedBlock = "";

		// Token: 0x040096D4 RID: 38612
		public string alternateItem = "";
	}
}

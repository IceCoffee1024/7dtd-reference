using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001926 RID: 6438
	[Preserve]
	public class ChallengeObjectiveBlockUpgrade : BaseChallengeObjective
	{
		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x0600C694 RID: 50836 RVA: 0x0002F260 File Offset: 0x0002D460
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockUpgrade;
			}
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x0600C695 RID: 50837 RVA: 0x0048EAA5 File Offset: 0x0048CCA5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveUpgrade", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C696 RID: 50838 RVA: 0x0048EACF File Offset: 0x0048CCCF
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C697 RID: 50839 RVA: 0x0048EADD File Offset: 0x0048CCDD
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupBlockUpgrade(this.heldItemID, this.neededResourceID, this.neededResourceCount));
		}

		// Token: 0x0600C698 RID: 50840 RVA: 0x0048EB0A File Offset: 0x0048CD0A
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
			QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
		}

		// Token: 0x0600C699 RID: 50841 RVA: 0x0048EB38 File Offset: 0x0048CD38
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		}

		// Token: 0x0600C69A RID: 50842 RVA: 0x0048EB50 File Offset: 0x0048CD50
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_BlockUpgrade(string blockName, Vector3i blockPos)
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
			if (!flag && blockName.Contains(":") && this.expectedBlock.EqualsCaseInsensitive(blockName.Substring(0, blockName.IndexOf(':'))))
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

		// Token: 0x0600C69B RID: 50843 RVA: 0x0048EC1C File Offset: 0x0048CE1C
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("block"))
			{
				this.expectedBlock = e.GetAttribute("block");
			}
			if (e.HasAttribute("held"))
			{
				this.heldItemID = e.GetAttribute("held");
			}
			if (e.HasAttribute("needed_resource"))
			{
				this.neededResourceID = e.GetAttribute("needed_resource");
			}
			if (e.HasAttribute("needed_resource_count"))
			{
				this.neededResourceCount = StringParsers.ParseSInt32(e.GetAttribute("needed_resource_count"), 0, -1, NumberStyles.Integer);
			}
		}

		// Token: 0x0600C69C RID: 50844 RVA: 0x0048ECD8 File Offset: 0x0048CED8
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBlockUpgrade
			{
				expectedBlock = this.expectedBlock,
				heldItemID = this.heldItemID,
				neededResourceID = this.neededResourceID,
				neededResourceCount = this.neededResourceCount
			};
		}

		// Token: 0x040095EC RID: 38380
		public string expectedBlock = "";

		// Token: 0x040095ED RID: 38381
		public string heldItemID = "";

		// Token: 0x040095EE RID: 38382
		public string neededResourceID = "";

		// Token: 0x040095EF RID: 38383
		public int neededResourceCount = 1;
	}
}

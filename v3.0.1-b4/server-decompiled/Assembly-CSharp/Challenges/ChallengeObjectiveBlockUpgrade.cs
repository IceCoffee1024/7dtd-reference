using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D4 RID: 6356
	[Preserve]
	public class ChallengeObjectiveBlockUpgrade : BaseChallengeObjective
	{
		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x0600C3B5 RID: 50101 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockUpgrade;
			}
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x0600C3B6 RID: 50102 RVA: 0x00484271 File Offset: 0x00482471
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveUpgrade", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C3B7 RID: 50103 RVA: 0x0048429B File Offset: 0x0048249B
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C3B8 RID: 50104 RVA: 0x004842A9 File Offset: 0x004824A9
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupBlockUpgrade(this.heldItemID, this.neededResourceID, this.neededResourceCount));
		}

		// Token: 0x0600C3B9 RID: 50105 RVA: 0x004842D6 File Offset: 0x004824D6
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
			QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
		}

		// Token: 0x0600C3BA RID: 50106 RVA: 0x00484304 File Offset: 0x00482504
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		}

		// Token: 0x0600C3BB RID: 50107 RVA: 0x0048431C File Offset: 0x0048251C
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

		// Token: 0x0600C3BC RID: 50108 RVA: 0x004843E8 File Offset: 0x004825E8
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

		// Token: 0x0600C3BD RID: 50109 RVA: 0x004844A4 File Offset: 0x004826A4
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

		// Token: 0x040093FD RID: 37885
		public string expectedBlock = "";

		// Token: 0x040093FE RID: 37886
		public string heldItemID = "";

		// Token: 0x040093FF RID: 37887
		public string neededResourceID = "";

		// Token: 0x04009400 RID: 37888
		public int neededResourceCount = 1;
	}
}

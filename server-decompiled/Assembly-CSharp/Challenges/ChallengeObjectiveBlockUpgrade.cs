using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018FA RID: 6394
	[Preserve]
	public class ChallengeObjectiveBlockUpgrade : BaseChallengeObjective
	{
		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x0600C53C RID: 50492 RVA: 0x0002F168 File Offset: 0x0002D368
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockUpgrade;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x0600C53D RID: 50493 RVA: 0x0048A5B5 File Offset: 0x004887B5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveUpgrade", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C53E RID: 50494 RVA: 0x0048A5DF File Offset: 0x004887DF
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C53F RID: 50495 RVA: 0x0048A5ED File Offset: 0x004887ED
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupBlockUpgrade(this.heldItemID, this.neededResourceID, this.neededResourceCount));
		}

		// Token: 0x0600C540 RID: 50496 RVA: 0x0048A61A File Offset: 0x0048881A
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
			QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
		}

		// Token: 0x0600C541 RID: 50497 RVA: 0x0048A648 File Offset: 0x00488848
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		}

		// Token: 0x0600C542 RID: 50498 RVA: 0x0048A660 File Offset: 0x00488860
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

		// Token: 0x0600C543 RID: 50499 RVA: 0x0048A72C File Offset: 0x0048892C
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

		// Token: 0x0600C544 RID: 50500 RVA: 0x0048A7E8 File Offset: 0x004889E8
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

		// Token: 0x04009526 RID: 38182
		public string expectedBlock = "";

		// Token: 0x04009527 RID: 38183
		public string heldItemID = "";

		// Token: 0x04009528 RID: 38184
		public string neededResourceID = "";

		// Token: 0x04009529 RID: 38185
		public int neededResourceCount = 1;
	}
}

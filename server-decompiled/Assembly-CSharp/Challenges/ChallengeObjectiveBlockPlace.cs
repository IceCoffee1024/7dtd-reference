using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D3 RID: 6355
	[Preserve]
	public class ChallengeObjectiveBlockPlace : BaseChallengeObjective
	{
		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x0600C3AB RID: 50091 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockPlace;
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x0600C3AC RID: 50092 RVA: 0x0048408C File Offset: 0x0048228C
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("xuiWorldPrefabsPlace", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C3AD RID: 50093 RVA: 0x004840B6 File Offset: 0x004822B6
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C3AE RID: 50094 RVA: 0x004840C4 File Offset: 0x004822C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupPlace((this.alternateItem != "") ? this.alternateItem : this.expectedBlock));
		}

		// Token: 0x0600C3AF RID: 50095 RVA: 0x004840FF File Offset: 0x004822FF
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
		}

		// Token: 0x0600C3B0 RID: 50096 RVA: 0x00484117 File Offset: 0x00482317
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		}

		// Token: 0x0600C3B1 RID: 50097 RVA: 0x00484130 File Offset: 0x00482330
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

		// Token: 0x0600C3B2 RID: 50098 RVA: 0x004841D0 File Offset: 0x004823D0
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

		// Token: 0x0600C3B3 RID: 50099 RVA: 0x00484234 File Offset: 0x00482434
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBlockPlace
			{
				expectedBlock = this.expectedBlock,
				alternateItem = this.alternateItem
			};
		}

		// Token: 0x040093FB RID: 37883
		public string expectedBlock = "";

		// Token: 0x040093FC RID: 37884
		public string alternateItem = "";
	}
}

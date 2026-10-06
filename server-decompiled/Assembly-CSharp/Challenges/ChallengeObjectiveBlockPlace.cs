using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001925 RID: 6437
	[Preserve]
	public class ChallengeObjectiveBlockPlace : BaseChallengeObjective
	{
		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x0600C68A RID: 50826 RVA: 0x00020119 File Offset: 0x0001E319
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockPlace;
			}
		}

		// Token: 0x170018FA RID: 6394
		// (get) Token: 0x0600C68B RID: 50827 RVA: 0x0048E8C1 File Offset: 0x0048CAC1
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("xuiWorldPrefabsPlace", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C68C RID: 50828 RVA: 0x0048E8EB File Offset: 0x0048CAEB
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C68D RID: 50829 RVA: 0x0048E8F9 File Offset: 0x0048CAF9
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupPlace((this.alternateItem != "") ? this.alternateItem : this.expectedBlock));
		}

		// Token: 0x0600C68E RID: 50830 RVA: 0x0048E934 File Offset: 0x0048CB34
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
		}

		// Token: 0x0600C68F RID: 50831 RVA: 0x0048E94C File Offset: 0x0048CB4C
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		}

		// Token: 0x0600C690 RID: 50832 RVA: 0x0048E964 File Offset: 0x0048CB64
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

		// Token: 0x0600C691 RID: 50833 RVA: 0x0048EA04 File Offset: 0x0048CC04
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

		// Token: 0x0600C692 RID: 50834 RVA: 0x0048EA68 File Offset: 0x0048CC68
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBlockPlace
			{
				expectedBlock = this.expectedBlock,
				alternateItem = this.alternateItem
			};
		}

		// Token: 0x040095EA RID: 38378
		public string expectedBlock = "";

		// Token: 0x040095EB RID: 38379
		public string alternateItem = "";
	}
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F9 RID: 6393
	[Preserve]
	public class ChallengeObjectiveBlockPlace : BaseChallengeObjective
	{
		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x0600C532 RID: 50482 RVA: 0x00020021 File Offset: 0x0001E221
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.BlockPlace;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x0600C533 RID: 50483 RVA: 0x0048A3D1 File Offset: 0x004885D1
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("xuiWorldPrefabsPlace", false, null) + " " + Localization.Get(this.expectedBlock, false, null) + ":";
			}
		}

		// Token: 0x0600C534 RID: 50484 RVA: 0x0048A3FB File Offset: 0x004885FB
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C535 RID: 50485 RVA: 0x0048A409 File Offset: 0x00488609
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupPlace((this.alternateItem != "") ? this.alternateItem : this.expectedBlock));
		}

		// Token: 0x0600C536 RID: 50486 RVA: 0x0048A444 File Offset: 0x00488644
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BlockPlace += this.Current_BlockPlace;
		}

		// Token: 0x0600C537 RID: 50487 RVA: 0x0048A45C File Offset: 0x0048865C
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BlockPlace -= this.Current_BlockPlace;
		}

		// Token: 0x0600C538 RID: 50488 RVA: 0x0048A474 File Offset: 0x00488674
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

		// Token: 0x0600C539 RID: 50489 RVA: 0x0048A514 File Offset: 0x00488714
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

		// Token: 0x0600C53A RID: 50490 RVA: 0x0048A578 File Offset: 0x00488778
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveBlockPlace
			{
				expectedBlock = this.expectedBlock,
				alternateItem = this.alternateItem
			};
		}

		// Token: 0x04009524 RID: 38180
		public string expectedBlock = "";

		// Token: 0x04009525 RID: 38181
		public string alternateItem = "";
	}
}

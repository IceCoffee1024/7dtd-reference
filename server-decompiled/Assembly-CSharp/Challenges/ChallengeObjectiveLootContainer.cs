using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001966 RID: 6502
	[Preserve]
	public class ChallengeObjectiveLootContainer : BaseChallengeObjective
	{
		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x0600C859 RID: 51289 RVA: 0x004955EE File Offset: 0x004937EE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.LootContainer;
			}
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600C85A RID: 51290 RVA: 0x004955F2 File Offset: 0x004937F2
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("ObjectiveLootContainer_keyword", false, null);
			}
		}

		// Token: 0x0600C85B RID: 51291 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C85C RID: 51292 RVA: 0x00495600 File Offset: 0x00493800
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
			QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		}

		// Token: 0x0600C85D RID: 51293 RVA: 0x00495630 File Offset: 0x00493830
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_ContainerOpened(Vector3i containerLocation, TEFeatureStorage tileEntity, bool firstTimeTouched)
		{
			if (containerLocation == this.lastLocation)
			{
				return;
			}
			this.lastLocation = containerLocation;
			if (!firstTimeTouched)
			{
				return;
			}
			if (this.CheckBaseRequirements())
			{
				return;
			}
			int num = base.Current;
			base.Current = num + 1;
			this.CheckObjectiveComplete(true);
		}

		// Token: 0x0600C85E RID: 51294 RVA: 0x00495678 File Offset: 0x00493878
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		}

		// Token: 0x0600C85F RID: 51295 RVA: 0x00495690 File Offset: 0x00493890
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveLootContainer();
		}

		// Token: 0x04009713 RID: 38675
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i lastLocation = Vector3i.zero;
	}
}

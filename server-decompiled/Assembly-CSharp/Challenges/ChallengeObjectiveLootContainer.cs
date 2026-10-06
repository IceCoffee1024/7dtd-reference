using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001935 RID: 6453
	[Preserve]
	public class ChallengeObjectiveLootContainer : BaseChallengeObjective
	{
		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x0600C745 RID: 51013 RVA: 0x00491BFA File Offset: 0x0048FDFA
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.LootContainer;
			}
		}

		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x0600C746 RID: 51014 RVA: 0x00491BFE File Offset: 0x0048FDFE
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("ObjectiveLootContainer_keyword", false, null);
			}
		}

		// Token: 0x0600C747 RID: 51015 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C748 RID: 51016 RVA: 0x00491C0C File Offset: 0x0048FE0C
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
			QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		}

		// Token: 0x0600C749 RID: 51017 RVA: 0x00491C3C File Offset: 0x0048FE3C
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

		// Token: 0x0600C74A RID: 51018 RVA: 0x00491C84 File Offset: 0x0048FE84
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		}

		// Token: 0x0600C74B RID: 51019 RVA: 0x00491C9C File Offset: 0x0048FE9C
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveLootContainer();
		}

		// Token: 0x0400962A RID: 38442
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i lastLocation = Vector3i.zero;
	}
}

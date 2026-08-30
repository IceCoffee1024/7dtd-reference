using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E3 RID: 6371
	[Preserve]
	public class ChallengeObjectiveLootContainer : BaseChallengeObjective
	{
		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x0600C466 RID: 50278 RVA: 0x004873BE File Offset: 0x004855BE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.LootContainer;
			}
		}

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x0600C467 RID: 50279 RVA: 0x004873C2 File Offset: 0x004855C2
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("ObjectiveLootContainer_keyword", false, null);
			}
		}

		// Token: 0x0600C468 RID: 50280 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C469 RID: 50281 RVA: 0x004873D0 File Offset: 0x004855D0
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
			QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		}

		// Token: 0x0600C46A RID: 50282 RVA: 0x00487400 File Offset: 0x00485600
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_ContainerOpened(Vector3i containerLocation, ITileEntityLootable tileEntity)
		{
			if (containerLocation == this.lastLocation)
			{
				return;
			}
			this.lastLocation = containerLocation;
			if (tileEntity.bWasTouched)
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

		// Token: 0x0600C46B RID: 50283 RVA: 0x0048744D File Offset: 0x0048564D
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		}

		// Token: 0x0600C46C RID: 50284 RVA: 0x00487465 File Offset: 0x00485665
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveLootContainer();
		}

		// Token: 0x0400943B RID: 37947
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i lastLocation = Vector3i.zero;
	}
}

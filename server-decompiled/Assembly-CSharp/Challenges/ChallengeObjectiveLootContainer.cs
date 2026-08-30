using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001909 RID: 6409
	[Preserve]
	public class ChallengeObjectiveLootContainer : BaseChallengeObjective
	{
		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x0600C5ED RID: 50669 RVA: 0x0048D70A File Offset: 0x0048B90A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.LootContainer;
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x0600C5EE RID: 50670 RVA: 0x0048D70E File Offset: 0x0048B90E
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("ObjectiveLootContainer_keyword", false, null);
			}
		}

		// Token: 0x0600C5EF RID: 50671 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C5F0 RID: 50672 RVA: 0x0048D71C File Offset: 0x0048B91C
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
			QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		}

		// Token: 0x0600C5F1 RID: 50673 RVA: 0x0048D74C File Offset: 0x0048B94C
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

		// Token: 0x0600C5F2 RID: 50674 RVA: 0x0048D799 File Offset: 0x0048B999
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		}

		// Token: 0x0600C5F3 RID: 50675 RVA: 0x0048D7B1 File Offset: 0x0048B9B1
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveLootContainer();
		}

		// Token: 0x04009564 RID: 38244
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i lastLocation = Vector3i.zero;
	}
}

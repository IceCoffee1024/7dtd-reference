using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C4 RID: 6596
	[Preserve]
	public class ActionRemoveSpawnedBlocks : BaseAction
	{
		// Token: 0x0600C8DF RID: 51423 RVA: 0x0049CA50 File Offset: 0x0049AC50
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			for (int i = 0; i < GameEventManager.Current.blockEntries.Count; i++)
			{
				GameEventManager.SpawnedBlocksEntry spawnedBlocksEntry = GameEventManager.Current.blockEntries[i];
				if (!this.targetOnly || spawnedBlocksEntry.Target == base.Owner.Target)
				{
					spawnedBlocksEntry.TimeAlive = 1f;
					spawnedBlocksEntry.IsDespawn = this.despawn;
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C8E0 RID: 51424 RVA: 0x0049CAC0 File Offset: 0x0049ACC0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveSpawnedBlocks.PropTargetOnly))
			{
				this.targetOnly = StringParsers.ParseBool(properties.Values[ActionRemoveSpawnedBlocks.PropTargetOnly], 0, -1, true);
			}
			properties.ParseBool(ActionRemoveSpawnedBlocks.PropDespawn, ref this.despawn);
		}

		// Token: 0x0600C8E1 RID: 51425 RVA: 0x0049CB15 File Offset: 0x0049AD15
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveSpawnedBlocks
			{
				targetOnly = this.targetOnly,
				despawn = this.despawn
			};
		}

		// Token: 0x04009878 RID: 39032
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x04009879 RID: 39033
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool despawn;

		// Token: 0x0400987A RID: 39034
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x0400987B RID: 39035
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDespawn = "despawn";
	}
}

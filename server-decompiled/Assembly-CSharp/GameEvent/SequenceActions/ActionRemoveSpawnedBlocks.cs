using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A48 RID: 6728
	[Preserve]
	public class ActionRemoveSpawnedBlocks : BaseAction
	{
		// Token: 0x0600CCD4 RID: 52436 RVA: 0x004AADE4 File Offset: 0x004A8FE4
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

		// Token: 0x0600CCD5 RID: 52437 RVA: 0x004AAE54 File Offset: 0x004A9054
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveSpawnedBlocks.PropTargetOnly))
			{
				this.targetOnly = StringParsers.ParseBool(properties.Values[ActionRemoveSpawnedBlocks.PropTargetOnly], 0, -1, true);
			}
			properties.ParseBool(ActionRemoveSpawnedBlocks.PropDespawn, ref this.despawn);
		}

		// Token: 0x0600CCD6 RID: 52438 RVA: 0x004AAEA9 File Offset: 0x004A90A9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveSpawnedBlocks
			{
				targetOnly = this.targetOnly,
				despawn = this.despawn
			};
		}

		// Token: 0x04009B58 RID: 39768
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x04009B59 RID: 39769
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool despawn;

		// Token: 0x04009B5A RID: 39770
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x04009B5B RID: 39771
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDespawn = "despawn";
	}
}

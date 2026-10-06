using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A17 RID: 6679
	[Preserve]
	public class ActionRemoveSpawnedBlocks : BaseAction
	{
		// Token: 0x0600CBC0 RID: 52160 RVA: 0x004A72E8 File Offset: 0x004A54E8
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

		// Token: 0x0600CBC1 RID: 52161 RVA: 0x004A7358 File Offset: 0x004A5558
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveSpawnedBlocks.PropTargetOnly))
			{
				this.targetOnly = StringParsers.ParseBool(properties.Values[ActionRemoveSpawnedBlocks.PropTargetOnly], 0, -1, true);
			}
			properties.ParseBool(ActionRemoveSpawnedBlocks.PropDespawn, ref this.despawn);
		}

		// Token: 0x0600CBC2 RID: 52162 RVA: 0x004A73AD File Offset: 0x004A55AD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveSpawnedBlocks
			{
				targetOnly = this.targetOnly,
				despawn = this.despawn
			};
		}

		// Token: 0x04009A6F RID: 39535
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x04009A70 RID: 39536
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool despawn;

		// Token: 0x04009A71 RID: 39537
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x04009A72 RID: 39538
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDespawn = "despawn";
	}
}

using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EB RID: 6635
	[Preserve]
	public class ActionRemoveSpawnedBlocks : BaseAction
	{
		// Token: 0x0600CA68 RID: 51816 RVA: 0x004A2E44 File Offset: 0x004A1044
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

		// Token: 0x0600CA69 RID: 51817 RVA: 0x004A2EB4 File Offset: 0x004A10B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveSpawnedBlocks.PropTargetOnly))
			{
				this.targetOnly = StringParsers.ParseBool(properties.Values[ActionRemoveSpawnedBlocks.PropTargetOnly], 0, -1, true);
			}
			properties.ParseBool(ActionRemoveSpawnedBlocks.PropDespawn, ref this.despawn);
		}

		// Token: 0x0600CA6A RID: 51818 RVA: 0x004A2F09 File Offset: 0x004A1109
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveSpawnedBlocks
			{
				targetOnly = this.targetOnly,
				despawn = this.despawn
			};
		}

		// Token: 0x040099A9 RID: 39337
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x040099AA RID: 39338
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool despawn;

		// Token: 0x040099AB RID: 39339
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x040099AC RID: 39340
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDespawn = "despawn";
	}
}

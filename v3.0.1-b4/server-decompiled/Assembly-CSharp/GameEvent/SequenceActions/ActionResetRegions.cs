using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001988 RID: 6536
	[Preserve]
	public class ActionResetRegions : BaseAction
	{
		// Token: 0x0600C7BD RID: 51133 RVA: 0x00497F49 File Offset: 0x00496149
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.StartCoroutine(this.HandleReset());
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7BE RID: 51134 RVA: 0x00497F5D File Offset: 0x0049615D
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator HandleReset()
		{
			yield return new WaitForSeconds(1f);
			World world = GameManager.Instance.World;
			ChunkCluster cc = world.ChunkCache;
			HashSetLong hashSetLong = new HashSetLong();
			HashSetLong regeneratedChunks = new HashSetLong();
			ChunkProviderGenerateWorld chunkProvider = world.ChunkCache.ChunkProvider as ChunkProviderGenerateWorld;
			if (this.ResetType == ActionResetRegions.ResetTypes.Full)
			{
				foreach (long num in chunkProvider.ResetAllChunks(ChunkProtectionLevel.None, EnumResetUnprotectedChunksGroupingMode.GroupedPOIs))
				{
					if (cc.ContainsChunkSync(num))
					{
						hashSetLong.Add(num);
					}
				}
				if (hashSetLong.Count > 0)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Regenerating {0} synced chunks.", hashSetLong.Count));
					foreach (long chunkKey in hashSetLong)
					{
						if (!chunkProvider.GenerateSingleChunk(cc, chunkKey, true))
						{
							yield return new WaitForEndOfFrame();
							SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Region reset failed regenerating chunk at world XZ position: {0}, {1}", WorldChunkCache.extractX(chunkKey) << 4, WorldChunkCache.extractZ(chunkKey) << 4));
						}
						else
						{
							regeneratedChunks.Add(chunkKey);
						}
					}
					HashSetLong.Enumerator enumerator2 = default(HashSetLong.Enumerator);
					world.m_ChunkManager.ResendChunksToClients(regeneratedChunks);
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Regeneration complete.");
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600C7BF RID: 51135 RVA: 0x00497F6C File Offset: 0x0049616C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionResetRegions.ResetTypes>(ActionResetRegions.PropResetType, ref this.ResetType);
		}

		// Token: 0x0600C7C0 RID: 51136 RVA: 0x00497F86 File Offset: 0x00496186
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetRegions
			{
				ResetType = this.ResetType
			};
		}

		// Token: 0x0400975E RID: 38750
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionResetRegions.ResetTypes ResetType;

		// Token: 0x0400975F RID: 38751
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetType = "reset_type";

		// Token: 0x04009760 RID: 38752
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isComplete;

		// Token: 0x02001989 RID: 6537
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ResetTypes
		{
			// Token: 0x04009762 RID: 38754
			None,
			// Token: 0x04009763 RID: 38755
			Full
		}
	}
}

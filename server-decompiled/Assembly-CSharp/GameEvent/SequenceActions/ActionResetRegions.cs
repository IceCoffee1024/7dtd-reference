using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0B RID: 6667
	[Preserve]
	public class ActionResetRegions : BaseAction
	{
		// Token: 0x0600CBB0 RID: 52144 RVA: 0x004A6291 File Offset: 0x004A4491
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.StartCoroutine(this.HandleReset());
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBB1 RID: 52145 RVA: 0x004A62A5 File Offset: 0x004A44A5
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

		// Token: 0x0600CBB2 RID: 52146 RVA: 0x004A62B4 File Offset: 0x004A44B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionResetRegions.ResetTypes>(ActionResetRegions.PropResetType, ref this.ResetType);
		}

		// Token: 0x0600CBB3 RID: 52147 RVA: 0x004A62CE File Offset: 0x004A44CE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetRegions
			{
				ResetType = this.ResetType
			};
		}

		// Token: 0x04009A36 RID: 39478
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionResetRegions.ResetTypes ResetType;

		// Token: 0x04009A37 RID: 39479
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetType = "reset_type";

		// Token: 0x04009A38 RID: 39480
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isComplete;

		// Token: 0x02001A0C RID: 6668
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ResetTypes
		{
			// Token: 0x04009A3A RID: 39482
			None,
			// Token: 0x04009A3B RID: 39483
			Full
		}
	}
}

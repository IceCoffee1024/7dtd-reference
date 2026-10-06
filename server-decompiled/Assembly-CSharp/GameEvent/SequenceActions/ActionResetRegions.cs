using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DA RID: 6618
	[Preserve]
	public class ActionResetRegions : BaseAction
	{
		// Token: 0x0600CA9C RID: 51868 RVA: 0x004A2791 File Offset: 0x004A0991
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.StartCoroutine(this.HandleReset());
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA9D RID: 51869 RVA: 0x004A27A5 File Offset: 0x004A09A5
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

		// Token: 0x0600CA9E RID: 51870 RVA: 0x004A27B4 File Offset: 0x004A09B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionResetRegions.ResetTypes>(ActionResetRegions.PropResetType, ref this.ResetType);
		}

		// Token: 0x0600CA9F RID: 51871 RVA: 0x004A27CE File Offset: 0x004A09CE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetRegions
			{
				ResetType = this.ResetType
			};
		}

		// Token: 0x0400994D RID: 39245
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionResetRegions.ResetTypes ResetType;

		// Token: 0x0400994E RID: 39246
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetType = "reset_type";

		// Token: 0x0400994F RID: 39247
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isComplete;

		// Token: 0x020019DB RID: 6619
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ResetTypes
		{
			// Token: 0x04009951 RID: 39249
			None,
			// Token: 0x04009952 RID: 39250
			Full
		}
	}
}

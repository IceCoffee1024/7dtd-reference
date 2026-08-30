using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AE RID: 6574
	[Preserve]
	public class ActionResetRegions : BaseAction
	{
		// Token: 0x0600C944 RID: 51524 RVA: 0x0049E2A1 File Offset: 0x0049C4A1
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.StartCoroutine(this.HandleReset());
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C945 RID: 51525 RVA: 0x0049E2B5 File Offset: 0x0049C4B5
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

		// Token: 0x0600C946 RID: 51526 RVA: 0x0049E2C4 File Offset: 0x0049C4C4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionResetRegions.ResetTypes>(ActionResetRegions.PropResetType, ref this.ResetType);
		}

		// Token: 0x0600C947 RID: 51527 RVA: 0x0049E2DE File Offset: 0x0049C4DE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetRegions
			{
				ResetType = this.ResetType
			};
		}

		// Token: 0x04009887 RID: 39047
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionResetRegions.ResetTypes ResetType;

		// Token: 0x04009888 RID: 39048
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetType = "reset_type";

		// Token: 0x04009889 RID: 39049
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isComplete;

		// Token: 0x020019AF RID: 6575
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ResetTypes
		{
			// Token: 0x0400988B RID: 39051
			None,
			// Token: 0x0400988C RID: 39052
			Full
		}
	}
}

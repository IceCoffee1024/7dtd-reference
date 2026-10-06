using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200188C RID: 6284
	public class TwitchRespawnEntry
	{
		// Token: 0x0600C20A RID: 49674 RVA: 0x00478FBE File Offset: 0x004771BE
		public TwitchRespawnEntry(string username, int respawnsLeft, EntityPlayer target, TwitchAction action)
		{
			this.UserName = username;
			this.Target = target;
			this.Action = action;
			this.RespawnsLeft = respawnsLeft;
		}

		// Token: 0x0600C20B RID: 49675 RVA: 0x00478FF9 File Offset: 0x004771F9
		public bool CheckRespawn(string username, EntityPlayer target, TwitchAction action)
		{
			return this.UserName == username && this.Target == target && this.Action == action;
		}

		// Token: 0x0600C20C RID: 49676 RVA: 0x00479024 File Offset: 0x00477224
		public bool RemoveSpawnedEntry(int entityID, bool checkForRemove)
		{
			bool flag = false;
			for (int i = this.SpawnedEntities.Count - 1; i >= 0; i--)
			{
				if (this.SpawnedEntities[i] == entityID)
				{
					flag = true;
					this.SpawnedEntities.RemoveAt(i);
				}
			}
			if (checkForRemove)
			{
				this.CheckReadyForRemove();
			}
			return flag;
		}

		// Token: 0x0600C20D RID: 49677 RVA: 0x00479074 File Offset: 0x00477274
		public bool RemoveSpawnedBlock(Vector3i pos, bool checkForRemove)
		{
			bool flag = false;
			for (int i = this.SpawnedBlocks.Count - 1; i >= 0; i--)
			{
				if (this.SpawnedBlocks[i] == pos)
				{
					flag = true;
					this.SpawnedBlocks.RemoveAt(i);
				}
			}
			if (checkForRemove)
			{
				this.CheckReadyForRemove();
			}
			return flag;
		}

		// Token: 0x0600C20E RID: 49678 RVA: 0x004790C8 File Offset: 0x004772C8
		public bool RemoveAllSpawnedBlock(bool checkForRemove)
		{
			bool flag = false;
			if (this.SpawnedBlocks.Count > 0)
			{
				flag = true;
				this.SpawnedBlocks.Clear();
			}
			if (checkForRemove)
			{
				this.CheckReadyForRemove();
			}
			return flag;
		}

		// Token: 0x0600C20F RID: 49679 RVA: 0x004790FC File Offset: 0x004772FC
		public void CheckReadyForRemove()
		{
			TwitchAction.RespawnCountTypes respawnCountType = this.Action.RespawnCountType;
			if (respawnCountType == TwitchAction.RespawnCountTypes.SpawnsOnly)
			{
				this.ReadyForRemove = this.SpawnedEntities.Count == 0;
				return;
			}
			if (respawnCountType == TwitchAction.RespawnCountTypes.BlocksOnly)
			{
				this.ReadyForRemove = this.SpawnedBlocks.Count == 0;
				return;
			}
			this.ReadyForRemove = this.SpawnedEntities.Count == 0 && this.SpawnedBlocks.Count == 0;
		}

		// Token: 0x0600C210 RID: 49680 RVA: 0x0047916C File Offset: 0x0047736C
		public TwitchActionEntry RespawnAction()
		{
			TwitchActionEntry twitchActionEntry = this.Action.SetupActionEntry();
			twitchActionEntry.UserName = this.UserName;
			twitchActionEntry.Target = this.Target;
			twitchActionEntry.Action = this.Action;
			twitchActionEntry.IsRespawn = true;
			twitchActionEntry.IsBitAction = this.Action.PointType == TwitchAction.PointTypes.Bits;
			this.RespawnsLeft--;
			this.NeedsRespawn = false;
			if (this.RespawnsLeft <= 0)
			{
				this.ReadyForRemove = true;
			}
			return twitchActionEntry;
		}

		// Token: 0x0600C211 RID: 49681 RVA: 0x004791E8 File Offset: 0x004773E8
		public bool CanRespawn(TwitchManager tm)
		{
			return this.NeedsRespawn && tm.CheckCanRespawnEvent(this.Target);
		}

		// Token: 0x04009214 RID: 37396
		public string UserName;

		// Token: 0x04009215 RID: 37397
		public EntityPlayer Target;

		// Token: 0x04009216 RID: 37398
		public TwitchAction Action;

		// Token: 0x04009217 RID: 37399
		public int RespawnsLeft;

		// Token: 0x04009218 RID: 37400
		public bool NeedsRespawn;

		// Token: 0x04009219 RID: 37401
		public bool ReadyForRemove;

		// Token: 0x0400921A RID: 37402
		public List<int> SpawnedEntities = new List<int>();

		// Token: 0x0400921B RID: 37403
		public List<Vector3i> SpawnedBlocks = new List<Vector3i>();
	}
}

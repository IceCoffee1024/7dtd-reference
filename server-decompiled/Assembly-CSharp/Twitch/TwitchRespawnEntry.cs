using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183A RID: 6202
	public class TwitchRespawnEntry
	{
		// Token: 0x0600BF2D RID: 48941 RVA: 0x0046ECE6 File Offset: 0x0046CEE6
		public TwitchRespawnEntry(string username, int respawnsLeft, EntityPlayer target, TwitchAction action)
		{
			this.UserName = username;
			this.Target = target;
			this.Action = action;
			this.RespawnsLeft = respawnsLeft;
		}

		// Token: 0x0600BF2E RID: 48942 RVA: 0x0046ED21 File Offset: 0x0046CF21
		public bool CheckRespawn(string username, EntityPlayer target, TwitchAction action)
		{
			return this.UserName == username && this.Target == target && this.Action == action;
		}

		// Token: 0x0600BF2F RID: 48943 RVA: 0x0046ED4C File Offset: 0x0046CF4C
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

		// Token: 0x0600BF30 RID: 48944 RVA: 0x0046ED9C File Offset: 0x0046CF9C
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

		// Token: 0x0600BF31 RID: 48945 RVA: 0x0046EDF0 File Offset: 0x0046CFF0
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

		// Token: 0x0600BF32 RID: 48946 RVA: 0x0046EE24 File Offset: 0x0046D024
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

		// Token: 0x0600BF33 RID: 48947 RVA: 0x0046EE94 File Offset: 0x0046D094
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

		// Token: 0x0600BF34 RID: 48948 RVA: 0x0046EF10 File Offset: 0x0046D110
		public bool CanRespawn(TwitchManager tm)
		{
			return this.NeedsRespawn && tm.CheckCanRespawnEvent(this.Target);
		}

		// Token: 0x04009032 RID: 36914
		public string UserName;

		// Token: 0x04009033 RID: 36915
		public EntityPlayer Target;

		// Token: 0x04009034 RID: 36916
		public TwitchAction Action;

		// Token: 0x04009035 RID: 36917
		public int RespawnsLeft;

		// Token: 0x04009036 RID: 36918
		public bool NeedsRespawn;

		// Token: 0x04009037 RID: 36919
		public bool ReadyForRemove;

		// Token: 0x04009038 RID: 36920
		public List<int> SpawnedEntities = new List<int>();

		// Token: 0x04009039 RID: 36921
		public List<Vector3i> SpawnedBlocks = new List<Vector3i>();
	}
}

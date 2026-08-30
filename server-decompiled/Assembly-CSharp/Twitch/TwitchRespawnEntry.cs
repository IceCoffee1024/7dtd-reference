using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001860 RID: 6240
	public class TwitchRespawnEntry
	{
		// Token: 0x0600C0B2 RID: 49330 RVA: 0x00474A82 File Offset: 0x00472C82
		public TwitchRespawnEntry(string username, int respawnsLeft, EntityPlayer target, TwitchAction action)
		{
			this.UserName = username;
			this.Target = target;
			this.Action = action;
			this.RespawnsLeft = respawnsLeft;
		}

		// Token: 0x0600C0B3 RID: 49331 RVA: 0x00474ABD File Offset: 0x00472CBD
		public bool CheckRespawn(string username, EntityPlayer target, TwitchAction action)
		{
			return this.UserName == username && this.Target == target && this.Action == action;
		}

		// Token: 0x0600C0B4 RID: 49332 RVA: 0x00474AE8 File Offset: 0x00472CE8
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

		// Token: 0x0600C0B5 RID: 49333 RVA: 0x00474B38 File Offset: 0x00472D38
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

		// Token: 0x0600C0B6 RID: 49334 RVA: 0x00474B8C File Offset: 0x00472D8C
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

		// Token: 0x0600C0B7 RID: 49335 RVA: 0x00474BC0 File Offset: 0x00472DC0
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

		// Token: 0x0600C0B8 RID: 49336 RVA: 0x00474C30 File Offset: 0x00472E30
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

		// Token: 0x0600C0B9 RID: 49337 RVA: 0x00474CAC File Offset: 0x00472EAC
		public bool CanRespawn(TwitchManager tm)
		{
			return this.NeedsRespawn && tm.CheckCanRespawnEvent(this.Target);
		}

		// Token: 0x0400914E RID: 37198
		public string UserName;

		// Token: 0x0400914F RID: 37199
		public EntityPlayer Target;

		// Token: 0x04009150 RID: 37200
		public TwitchAction Action;

		// Token: 0x04009151 RID: 37201
		public int RespawnsLeft;

		// Token: 0x04009152 RID: 37202
		public bool NeedsRespawn;

		// Token: 0x04009153 RID: 37203
		public bool ReadyForRemove;

		// Token: 0x04009154 RID: 37204
		public List<int> SpawnedEntities = new List<int>();

		// Token: 0x04009155 RID: 37205
		public List<Vector3i> SpawnedBlocks = new List<Vector3i>();
	}
}

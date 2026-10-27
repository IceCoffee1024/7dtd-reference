using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020018BD RID: 6333
	public class TwitchRespawnEntry
	{
		// Token: 0x0600C31E RID: 49950 RVA: 0x0047C96A File Offset: 0x0047AB6A
		public TwitchRespawnEntry(string username, int respawnsLeft, EntityPlayer target, TwitchAction action)
		{
			this.UserName = username;
			this.Target = target;
			this.Action = action;
			this.RespawnsLeft = respawnsLeft;
		}

		// Token: 0x0600C31F RID: 49951 RVA: 0x0047C9A5 File Offset: 0x0047ABA5
		public bool CheckRespawn(string username, EntityPlayer target, TwitchAction action)
		{
			return this.UserName == username && this.Target == target && this.Action == action;
		}

		// Token: 0x0600C320 RID: 49952 RVA: 0x0047C9D0 File Offset: 0x0047ABD0
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

		// Token: 0x0600C321 RID: 49953 RVA: 0x0047CA20 File Offset: 0x0047AC20
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

		// Token: 0x0600C322 RID: 49954 RVA: 0x0047CA74 File Offset: 0x0047AC74
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

		// Token: 0x0600C323 RID: 49955 RVA: 0x0047CAA8 File Offset: 0x0047ACA8
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

		// Token: 0x0600C324 RID: 49956 RVA: 0x0047CB18 File Offset: 0x0047AD18
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

		// Token: 0x0600C325 RID: 49957 RVA: 0x0047CB94 File Offset: 0x0047AD94
		public bool CanRespawn(TwitchManager tm)
		{
			return this.NeedsRespawn && tm.CheckCanRespawnEvent(this.Target);
		}

		// Token: 0x040092FD RID: 37629
		public string UserName;

		// Token: 0x040092FE RID: 37630
		public EntityPlayer Target;

		// Token: 0x040092FF RID: 37631
		public TwitchAction Action;

		// Token: 0x04009300 RID: 37632
		public int RespawnsLeft;

		// Token: 0x04009301 RID: 37633
		public bool NeedsRespawn;

		// Token: 0x04009302 RID: 37634
		public bool ReadyForRemove;

		// Token: 0x04009303 RID: 37635
		public List<int> SpawnedEntities = new List<int>();

		// Token: 0x04009304 RID: 37636
		public List<Vector3i> SpawnedBlocks = new List<Vector3i>();
	}
}

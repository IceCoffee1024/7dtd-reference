using System;
using UnityEngine.Scripting;

// Token: 0x0200041B RID: 1051
[Preserve]
public class AIDirectorPlayerManagementComponent : AIDirectorComponent
{
	// Token: 0x06002056 RID: 8278 RVA: 0x000C401C File Offset: 0x000C221C
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		this.TickPlayerStates(_dt);
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000C402C File Offset: 0x000C222C
	public void AddPlayer(EntityPlayer _player)
	{
		if (!this.trackedPlayers.dict.ContainsKey(_player.entityId))
		{
			AIDirectorPlayerState aidirectorPlayerState = this.playerPool.Alloc(false);
			if (aidirectorPlayerState != null)
			{
				this.trackedPlayers.Add(_player.entityId, aidirectorPlayerState.Construct(_player));
			}
		}
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000C407C File Offset: 0x000C227C
	public void RemovePlayer(EntityPlayer _player)
	{
		AIDirectorPlayerState aidirectorPlayerState;
		if (this.trackedPlayers.dict.TryGetValue(_player.entityId, out aidirectorPlayerState))
		{
			this.trackedPlayers.Remove(_player.entityId);
			aidirectorPlayerState.Reset();
			this.playerPool.Free(aidirectorPlayerState);
		}
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x000C40C8 File Offset: 0x000C22C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickPlayerStates(double _dt)
	{
		for (int i = 0; i < this.trackedPlayers.list.Count; i++)
		{
			AIDirectorPlayerState aidirectorPlayerState = this.trackedPlayers.list[i];
			this.TickPlayerState(aidirectorPlayerState, _dt);
		}
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000C410C File Offset: 0x000C230C
	public void UpdatePlayerInventory(int entityId, AIDirectorPlayerInventory inventory)
	{
		AIDirectorPlayerState aidirectorPlayerState;
		if (this.trackedPlayers.dict.TryGetValue(entityId, out aidirectorPlayerState))
		{
			aidirectorPlayerState.Inventory = inventory;
		}
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x000C4135 File Offset: 0x000C2335
	public void UpdatePlayerInventory(EntityPlayerLocal player)
	{
		this.UpdatePlayerInventory(player.entityId, AIDirectorPlayerInventory.FromEntity(player));
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000C4149 File Offset: 0x000C2349
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickPlayerState(AIDirectorPlayerState _ps, double _dt)
	{
		_ps.Dead = _ps.Player.IsDead();
	}

	// Token: 0x04001620 RID: 5664
	public DictionaryList<int, AIDirectorPlayerState> trackedPlayers = new DictionaryList<int, AIDirectorPlayerState>();

	// Token: 0x04001621 RID: 5665
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<AIDirectorPlayerState> playerPool = new MemoryPooledObject<AIDirectorPlayerState>(32);
}

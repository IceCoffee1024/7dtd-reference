using System;
using UnityEngine.Scripting;

// Token: 0x0200041D RID: 1053
[Preserve]
public class AIDirectorPlayerManagementComponent : AIDirectorComponent
{
	// Token: 0x0600205B RID: 8283 RVA: 0x000C43C8 File Offset: 0x000C25C8
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		this.TickPlayerStates(_dt);
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000C43D8 File Offset: 0x000C25D8
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

	// Token: 0x0600205D RID: 8285 RVA: 0x000C4428 File Offset: 0x000C2628
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

	// Token: 0x0600205E RID: 8286 RVA: 0x000C4474 File Offset: 0x000C2674
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickPlayerStates(double _dt)
	{
		for (int i = 0; i < this.trackedPlayers.list.Count; i++)
		{
			AIDirectorPlayerState aidirectorPlayerState = this.trackedPlayers.list[i];
			this.TickPlayerState(aidirectorPlayerState, _dt);
		}
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000C44B8 File Offset: 0x000C26B8
	public void UpdatePlayerInventory(int entityId, AIDirectorPlayerInventory inventory)
	{
		AIDirectorPlayerState aidirectorPlayerState;
		if (this.trackedPlayers.dict.TryGetValue(entityId, out aidirectorPlayerState))
		{
			aidirectorPlayerState.Inventory = inventory;
		}
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000C44E1 File Offset: 0x000C26E1
	public void UpdatePlayerInventory(EntityPlayerLocal player)
	{
		this.UpdatePlayerInventory(player.entityId, AIDirectorPlayerInventory.FromEntity(player));
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000C44F5 File Offset: 0x000C26F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickPlayerState(AIDirectorPlayerState _ps, double _dt)
	{
		_ps.Dead = _ps.Player.IsDead();
	}

	// Token: 0x04001630 RID: 5680
	public DictionaryList<int, AIDirectorPlayerState> trackedPlayers = new DictionaryList<int, AIDirectorPlayerState>();

	// Token: 0x04001631 RID: 5681
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<AIDirectorPlayerState> playerPool = new MemoryPooledObject<AIDirectorPlayerState>(32);
}

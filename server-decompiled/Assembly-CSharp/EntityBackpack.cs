using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200048D RID: 1165
[Preserve]
public class EntityBackpack : EntityContainerAbs
{
	// Token: 0x06002404 RID: 9220 RVA: 0x000DA35A File Offset: 0x000D855A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		this.pmEntityType = new ProfilerMarker("EntityBackpack");
		base.Awake();
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000DA372 File Offset: 0x000D8572
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		this.LogBackpack("Start", Array.Empty<object>());
		base.Start();
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x000DA38C File Offset: 0x000D858C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		Vector3 position = this.position;
		if (this.world.AdjustBoundsForPlayers(ref position, 0.06f))
		{
			this.itemRB.velocity *= 0.5f;
			position.y = this.itemRB.position.y + Origin.position.y;
			this.itemRB.position = position - Origin.position;
			this.SetPosition(position, true);
		}
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000DA418 File Offset: 0x000D8618
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updatePosition()
	{
		base.updatePosition();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !this.bRemoved && !this.isEntityRemote && base.transform.position.y + Origin.position.y < 1f)
		{
			Vector3 vector = new Vector3(this.position.x, (float)(this.world.GetHeight(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.z)) + 5) + this.rand.RandomFloat * 20f, this.position.z);
			Log.Warning("EntityBackpack below world {0}, moving to {1}", new object[]
			{
				this.position.ToCultureInvariantString(),
				vector.ToCultureInvariantString()
			});
			this.SetPosition(vector, true);
			base.transform.position = vector - Origin.position;
			int num = this.safetyCounter + 1;
			this.safetyCounter = num;
			if (num > 500)
			{
				base.removeBackpack("retries");
			}
		}
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000DA53C File Offset: 0x000D873C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateMarkers()
	{
		base.updateMarkers();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !this.bRemoved && !this.isEntityRemote && this.RefPlayerId != -1)
		{
			using (IEnumerator<PersistentPlayerData> enumerator = GameManager.Instance.persistentPlayers.Players.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryUpdateBackpackPosition(this.entityId, new Vector3i(this.position)))
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000DA5D4 File Offset: 0x000D87D4
	public override string GetActivationText()
	{
		if (this.bag == null)
		{
			return string.Empty;
		}
		GameManager instance = GameManager.Instance;
		EntityPlayerLocal entityPlayerLocal;
		if (instance == null)
		{
			entityPlayerLocal = null;
		}
		else
		{
			World world = instance.World;
			entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		}
		EntityPlayerLocal entityPlayerLocal2 = entityPlayerLocal;
		if (entityPlayerLocal2 == null)
		{
			return string.Empty;
		}
		PlayerActionsLocal playerInput = entityPlayerLocal2.playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		return string.Format(Localization.Get("lootTooltipTouched", false, null), text, this.LocalizedEntityName);
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000DA664 File Offset: 0x000D8864
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void onBackpackRemoved(string reason)
	{
		base.onBackpackRemoved(reason);
		this.LogBackpack("RemoveBackpack empty {0}, reason {1}", new object[]
		{
			this.bag == null || this.bag.IsEmpty(),
			reason
		});
		Vector3i zero = Vector3i.zero;
		if (!this.isEntityRemote && this.RefPlayerId != -1)
		{
			foreach (PersistentPlayerData persistentPlayerData in GameManager.Instance.persistentPlayers.Players.Values)
			{
				if (persistentPlayerData.TryRemoveDroppedBackpack(this.entityId))
				{
					Vector3i mostRecentBackpackPosition = persistentPlayerData.MostRecentBackpackPosition;
					break;
				}
			}
		}
		EntityPlayer entityPlayer = this.world.GetEntity(this.RefPlayerId) as EntityPlayer;
		if (entityPlayer != null)
		{
			if (!entityPlayer.isEntityRemote && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				entityPlayer.SetDroppedBackpackPositions(GameManager.Instance.persistentLocalPlayer.GetDroppedBackpackPositions());
				return;
			}
			if (!this.world.IsRemote())
			{
				PersistentPlayerData persistentPlayerData2 = GameManager.Instance.persistentPlayers.EntityToPlayerMap[entityPlayer.entityId];
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerSetBackpackPosition>().Setup(entityPlayer.entityId, persistentPlayerData2.GetDroppedBackpackPositions()), false, entityPlayer.entityId, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000DA7D0 File Offset: 0x000D89D0
	public override void OnEntityUnload()
	{
		this.LogBackpack("OnEntityUnload markedForUnload {0}, IsDead {1}, IsDespawned {2}", new object[]
		{
			this.markedForUnload,
			this.IsDead(),
			this.IsDespawned
		});
		base.OnEntityUnload();
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x000DA81E File Offset: 0x000D8A1E
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		this.LogBackpack("Write", Array.Empty<object>());
		base.Write(_bw, _eStreamMode);
		_bw.Write(this.RefPlayerId);
		this.bag.Write(_bw, _eStreamMode);
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x000DA851 File Offset: 0x000D8A51
	public override void Read(byte _version, PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_version, _br, _eStreamMode);
		this.RefPlayerId = _br.ReadInt32();
		if (_version >= 35)
		{
			this.bag.ReadInto(_br, _eStreamMode);
		}
		this.LogBackpack("Read", Array.Empty<object>());
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x000BDA8D File Offset: 0x000BBC8D
	public override string GetLootList()
	{
		return this.lootList;
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x000DA88C File Offset: 0x000D8A8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleNavObject()
	{
		if (this.RefPlayerId != -1)
		{
			EntityPlayerLocal entityPlayerLocal = this.world.GetEntity(this.RefPlayerId) as EntityPlayerLocal;
			if (entityPlayerLocal != null && this.RefPlayerId == entityPlayerLocal.entityId)
			{
				if (GamePrefs.GetInt(EnumGamePrefs.DeathPenalty) == 3 && entityPlayerLocal.GetDroppedBackpackPositions().Count == 0)
				{
					this.RefPlayerId = -1;
					return;
				}
				if (EntityClass.list[this.entityClass].NavObject != "")
				{
					this.NavObject = NavObjectManager.Instance.RegisterNavObject(EntityClass.list[this.entityClass].NavObject, base.transform, "", false);
				}
			}
		}
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000DA944 File Offset: 0x000D8B44
	[Conditional("DEBUG_ENTITYBACKPACK")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogBackpack(string format, params object[] args)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !this.world.IsLocalPlayer(this.RefPlayerId))
		{
			return;
		}
		string text = "?";
		if (this.bag != null)
		{
			text = string.Empty;
			int num = 0;
			for (int i = 0; i < this.bag.SlotCount; i++)
			{
				ItemStack itemStack = this.bag.ItemGrid[i];
				if (!itemStack.IsEmpty())
				{
					num++;
					if (num == 1)
					{
						text = itemStack.itemValue.ItemClass.Name;
					}
					if (num == 2)
					{
						text = text + ", " + itemStack.itemValue.ItemClass.Name;
					}
				}
			}
			text = string.Format("{0} {1} at ({2})", num, text, this.position);
		}
		int num2 = 0;
		if (ThreadManager.IsMainThread())
		{
			num2 = Time.frameCount;
		}
		format = string.Format("{0} EntityBackpack id {1}, plyrId {2}, {3} ({4}), chunk {5} ({6}), items {7} : {8}", new object[]
		{
			num2,
			this.entityId,
			this.RefPlayerId,
			this.position.ToCultureInvariantString(),
			World.toChunkXZ(this.position),
			this.addedToChunk,
			this.chunkPosAddedEntityTo,
			text,
			format
		});
		Log.Out(format, args);
	}

	// Token: 0x04001999 RID: 6553
	public int RefPlayerId = -1;

	// Token: 0x0400199A RID: 6554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int safetyCounter;
}

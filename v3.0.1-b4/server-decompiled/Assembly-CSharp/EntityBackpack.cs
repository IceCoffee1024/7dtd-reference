using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000485 RID: 1157
[Preserve]
public class EntityBackpack : EntityItem
{
	// Token: 0x060023EA RID: 9194 RVA: 0x000D93E4 File Offset: 0x000D75E4
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		float num = 5f;
		entityClass.Properties.ParseFloat(EntityClass.PropTimeStayAfterDeath, ref num);
		this.ticksStayAfterDeath = (int)(num * 20f);
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000D942C File Offset: 0x000D762C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.LogBackpack("Start", Array.Empty<object>());
		foreach (Collider collider in base.transform.GetComponentsInChildren<Collider>())
		{
			collider.gameObject.tag = "E_BP_Body";
			collider.gameObject.layer = 13;
			collider.enabled = true;
			collider.gameObject.AddMissingComponent<RootTransformRefEntity>().RootTransform = base.transform;
		}
		this.SetDead();
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000D94AC File Offset: 0x000D76AC
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

	// Token: 0x060023ED RID: 9197 RVA: 0x000D9538 File Offset: 0x000D7738
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		try
		{
			if (!(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? LockManager.Instance.IsLockedServer(this, 0) : LockManager.Instance.IsLockedByLocalPlayer(this, 0)))
			{
				if (this.bag.Touched && this.bag.IsEmpty())
				{
					this.RemoveBackpack("empty");
				}
				else if (this.deathUpdateTicks >= this.ticksStayAfterDeath - 1)
				{
					this.RemoveBackpack("old");
				}
				else
				{
					this.deathUpdateTicks++;
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
							this.RemoveBackpack("retries");
						}
					}
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
			}
		}
		finally
		{
		}
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000D978C File Offset: 0x000D798C
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null;
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000D97B0 File Offset: 0x000D79B0
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

	// Token: 0x060023F0 RID: 9200 RVA: 0x000D9840 File Offset: 0x000D7A40
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveBackpack(string reason)
	{
		this.LogBackpack("RemoveBackpack empty {0}, reason {1}", new object[]
		{
			this.bag == null || this.bag.IsEmpty(),
			reason
		});
		this.deathUpdateTicks = this.ticksStayAfterDeath;
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
			}
			else if (!this.world.IsRemote())
			{
				PersistentPlayerData persistentPlayerData2 = GameManager.Instance.persistentPlayers.EntityToPlayerMap[entityPlayer.entityId];
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerSetBackpackPosition>().Setup(entityPlayer.entityId, persistentPlayerData2.GetDroppedBackpackPositions()), false, entityPlayer.entityId, -1, -1, null, 192, false);
			}
		}
		this.bRemoved = true;
		this.MarkToUnload();
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000D99C0 File Offset: 0x000D7BC0
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

	// Token: 0x060023F2 RID: 9202 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createMesh()
	{
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000D9A0E File Offset: 0x000D7C0E
	public override void Write(BinaryWriter _bw, bool _bNetworkWrite)
	{
		this.LogBackpack("Write", Array.Empty<object>());
		base.Write(_bw, _bNetworkWrite);
		_bw.Write(this.RefPlayerId);
		this.bag.Write(_bw);
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000D9A40 File Offset: 0x000D7C40
	public override void Read(byte _version, BinaryReader _br)
	{
		base.Read(_version, _br);
		this.RefPlayerId = _br.ReadInt32();
		if (_version >= 35)
		{
			this.bag.ReadInto(_br);
		}
		this.LogBackpack("Read", Array.Empty<object>());
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000D9A78 File Offset: 0x000D7C78
	public override bool IsMarkedForUnload()
	{
		return base.IsMarkedForUnload() && this.bRemoved;
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000BCDC3 File Offset: 0x000BAFC3
	public override string GetLootList()
	{
		return this.lootList;
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000D9A8C File Offset: 0x000D7C8C
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

	// Token: 0x060023F8 RID: 9208 RVA: 0x000D9B44 File Offset: 0x000D7D44
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		if (this.bag.IsEmpty())
		{
			this.KillLootContainer();
		}
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000D9B5C File Offset: 0x000D7D5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogBackpack(string format, params object[] args)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || this.world.IsLocalPlayer(this.RefPlayerId))
		{
			string text = "?";
			if (this.bag != null)
			{
				text = string.Empty;
				int num = 0;
				foreach (ItemStack itemStack in this.bag.GetSlots())
				{
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
	}

	// Token: 0x0400197B RID: 6523
	public int RefPlayerId = -1;

	// Token: 0x0400197C RID: 6524
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int deathUpdateTicks;

	// Token: 0x0400197D RID: 6525
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksStayAfterDeath;

	// Token: 0x0400197E RID: 6526
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bRemoved;

	// Token: 0x0400197F RID: 6527
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int safetyCounter;
}

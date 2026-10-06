using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000151 RID: 337
[Preserve]
public class BlockSpawnEntity : Block
{
	// Token: 0x06000945 RID: 2373 RVA: 0x000407AC File Offset: 0x0003E9AC
	public override void Init()
	{
		base.Init();
		if (!base.Properties.Values.ContainsKey("SpawnClass"))
		{
			throw new Exception(string.Format("Need 'SpawnClass' in block {0}", base.GetBlockName()));
		}
		this.spawnClasses = base.Properties.Values["SpawnClass"].Split(',', StringSplitOptions.None);
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00040810 File Offset: 0x0003EA10
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (!GameManager.Instance.IsEditMode() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.spawnClasses.Length != 0)
		{
			EntityCreationData entityCreationData = new EntityCreationData();
			entityCreationData.id = -1;
			string text = this.spawnClasses[(int)_blockValue.meta % this.spawnClasses.Length];
			entityCreationData.entityClass = text.GetHashCode();
			entityCreationData.pos = _blockPos.ToVector3() + new Vector3(0.5f, 0.25f, 0.5f);
			entityCreationData.rot = new Vector3(0f, (float)(90 * (_blockValue.rotation & 3)), 0f);
			_chunk.AddEntityStub(entityCreationData);
			_world.GetWBT().AddScheduledBlockUpdate(_blockPos, this.blockID, 80UL);
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x000408EC File Offset: 0x0003EAEC
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		if (_blockValue.ischild)
		{
			return;
		}
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(_blockPos);
		if (chunk == null)
		{
			return;
		}
		chunk.AddEntityBlockStub(new BlockEntityData(_blockValue, _blockPos)
		{
			bNeedsTemperature = true
		});
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			_world.GetWBT().AddScheduledBlockUpdate(_blockPos, this.blockID, 300UL);
		}
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00040964 File Offset: 0x0003EB64
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		if (_blockValue.ischild)
		{
			return;
		}
		if (!_world.IsEditor())
		{
			_ebcd.transform.gameObject.SetActive(false);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			_world.GetWBT().AddScheduledBlockUpdate(_blockPos, this.blockID, 300UL);
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x000409C4 File Offset: 0x0003EBC4
	public override bool UpdateTick(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bRandomTick, ulong _ticksIfLoaded, GameRandom _rnd)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (GameManager.Instance.World.GetEntitiesInBounds(null, new Bounds(_blockPos.ToVector3(), Vector3.one * 2f)).Count == 0 && !GameManager.Instance.IsEditMode())
			{
				ChunkCluster chunkCache = _world.ChunkCache;
				if (chunkCache == null)
				{
					return false;
				}
				if ((Chunk)chunkCache.GetChunkFromWorldPos(_blockPos) == null)
				{
					return false;
				}
				if (this.spawnClasses.Length != 0)
				{
					string text = this.spawnClasses[(int)_blockValue.meta % this.spawnClasses.Length];
					int num = -1;
					foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
					{
						if (keyValuePair.Value.entityClassName == text)
						{
							num = keyValuePair.Key;
						}
					}
					Vector3 vector = _blockPos.ToVector3() + new Vector3(0.5f, 0.25f, 0.5f);
					Vector3 vector2 = new Vector3(0f, (float)(90 * (_blockValue.rotation & 3)), 0f);
					Entity entity = EntityFactory.CreateEntity(num, vector, vector2);
					entity.SetSpawnerSource(EnumSpawnerSource.StaticSpawner);
					GameManager.Instance.World.SpawnEntityInWorld(entity);
					Log.Out("BlockSpawnEntity:: Spawn New Trader.");
				}
			}
			_world.GetWBT().AddScheduledBlockUpdate(_blockPos, this.blockID, 320UL);
		}
		return base.UpdateTick(_world, _blockPos, _blockValue, _bRandomTick, _ticksIfLoaded, _rnd);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00040B5C File Offset: 0x0003ED5C
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor())
		{
			return null;
		}
		byte meta = _blockValue.meta;
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		return string.Format(Localization.Get("editBlockSpawnEntity", false, null), text, ((int)meta < this.spawnClasses.Length) ? this.spawnClasses[(int)_blockValue.meta] : "-");
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00040BE1 File Offset: 0x0003EDE1
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!(_commandName == "edit"))
		{
			return false;
		}
		if (!_world.IsEditor())
		{
			return false;
		}
		XUiC_SpawnBlockEditor.Open(_player.PlayerUI, _blockPos, _blockValue, this);
		return true;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00040C0D File Offset: 0x0003EE0D
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = _world.IsEditor();
		return this.cmds;
	}

	// Token: 0x040009B8 RID: 2488
	public string[] spawnClasses;

	// Token: 0x040009B9 RID: 2489
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("edit", "tool", false, false, null)
	};
}

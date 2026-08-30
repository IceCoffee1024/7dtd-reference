using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200014E RID: 334
[Preserve]
public class BlockSpawnEntity : Block
{
	// Token: 0x06000935 RID: 2357 RVA: 0x0004046C File Offset: 0x0003E66C
	public override void Init()
	{
		base.Init();
		if (!base.Properties.Values.ContainsKey("SpawnClass"))
		{
			throw new Exception(string.Format("Need 'SpawnClass' in block {0}", base.GetBlockName()));
		}
		this.spawnClasses = base.Properties.Values["SpawnClass"].Split(',', StringSplitOptions.None);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x000404D0 File Offset: 0x0003E6D0
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

	// Token: 0x06000937 RID: 2359 RVA: 0x000405AC File Offset: 0x0003E7AC
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

	// Token: 0x06000938 RID: 2360 RVA: 0x00040624 File Offset: 0x0003E824
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

	// Token: 0x06000939 RID: 2361 RVA: 0x00040684 File Offset: 0x0003E884
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

	// Token: 0x0600093A RID: 2362 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0004081C File Offset: 0x0003EA1C
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

	// Token: 0x0600093C RID: 2364 RVA: 0x000408A1 File Offset: 0x0003EAA1
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

	// Token: 0x0600093D RID: 2365 RVA: 0x000408CD File Offset: 0x0003EACD
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = _world.IsEditor();
		return this.cmds;
	}

	// Token: 0x040009B3 RID: 2483
	public string[] spawnClasses;

	// Token: 0x040009B4 RID: 2484
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("edit", "tool", false, false, null)
	};
}

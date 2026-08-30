using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000139 RID: 313
[Preserve]
public class BlockPowered : Block
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x0003C548 File Offset: 0x0003A748
	public int RequiredPower
	{
		get
		{
			return this.requiredPower;
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0003C550 File Offset: 0x0003A750
	public BlockPowered()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0003C5AC File Offset: 0x0003A7AC
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.requiredPower = int.Parse(base.Properties.Values["RequiredPower"]);
		}
		else
		{
			this.requiredPower = 5;
		}
		if (base.Properties.Values.ContainsKey("PoweredType"))
		{
			this.poweredType = base.Properties.Values["PoweredType"];
		}
		if (base.Properties.Values.ContainsKey("TakeDelay"))
		{
			this.TakeDelay = StringParsers.ParseFloat(base.Properties.Values["TakeDelay"], 0, -1, NumberStyles.Any);
			return;
		}
		this.TakeDelay = 2f;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0003C67C File Offset: 0x0003A87C
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
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0003C6D0 File Offset: 0x0003A8D0
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		if (_blockValue.ischild)
		{
			return;
		}
		TileEntityPowered tileEntityPowered = _world.GetTileEntity(_blockPos) as TileEntityPowered;
		if (tileEntityPowered != null)
		{
			tileEntityPowered.BlockTransform = _ebcd.transform;
			GameManager.Instance.StartCoroutine(this.drawWiresLater(tileEntityPowered));
			if (tileEntityPowered.GetParent().y != -9999)
			{
				IPowered powered = _world.GetTileEntity(tileEntityPowered.GetParent()) as IPowered;
				if (powered != null)
				{
					GameManager.Instance.StartCoroutine(this.drawWiresLater(powered));
				}
			}
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0003C75A File Offset: 0x0003A95A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator drawWiresLater(IPowered powered)
	{
		yield return new WaitForSeconds(0.5f);
		powered.DrawWires();
		yield break;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0003C76C File Offset: 0x0003A96C
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		if (_blockValue.ischild)
		{
			return;
		}
		TileEntityPowered tileEntityPowered = _chunk.GetTileEntity(World.toBlock(_blockPos)) as TileEntityPowered;
		if (tileEntityPowered != null)
		{
			if (!GameManager.IsDedicatedServer)
			{
				EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				if (primaryPlayer.inventory.holdingItem.Actions[1] is ItemActionConnectPower)
				{
					(primaryPlayer.inventory.holdingItem.Actions[1] as ItemActionConnectPower).CheckForWireRemoveNeeded(primaryPlayer, _blockPos);
				}
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				PowerManager.Instance.RemovePowerNode(tileEntityPowered.GetPowerItem());
			}
			if (tileEntityPowered.GetParent().y != -9999)
			{
				IPowered powered = world.GetTileEntity(tileEntityPowered.GetParent()) as IPowered;
				if (powered != null && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					powered.SendWireData();
				}
			}
			tileEntityPowered.RemoveWires();
		}
		_chunk.RemoveTileEntityAt<TileEntityPowered>((World)world, World.toBlock(_blockPos));
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0003C861 File Offset: 0x0003AA61
	public virtual TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredBlock(chunk);
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x0003C86C File Offset: 0x0003AA6C
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x0003C8B8 File Offset: 0x0003AAB8
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer()) || this.TakeDelay <= 0f)
		{
			return "";
		}
		Block block = _blockValue.Block;
		return string.Format(Localization.Get("pickupPrompt", false, null), block.GetLocalizedBlockName());
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x0003C90C File Offset: 0x0003AB0C
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_blockValue.ischild)
		{
			Vector3i parentPos = _blockValue.Block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.OnBlockActivated(_commandName, _world, parentPos, block, _player);
		}
		if (_commandName == "take")
		{
			base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
			return true;
		}
		return false;
	}

	// Token: 0x04000975 RID: 2421
	[PublicizedFrom(EAccessModifier.Protected)]
	public int requiredPower = -1;

	// Token: 0x04000976 RID: 2422
	[PublicizedFrom(EAccessModifier.Protected)]
	public string poweredType = "";

	// Token: 0x04000977 RID: 2423
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;

	// Token: 0x04000978 RID: 2424
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}

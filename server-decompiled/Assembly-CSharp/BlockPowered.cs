using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200013C RID: 316
[Preserve]
public class BlockPowered : Block
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x0003C894 File Offset: 0x0003AA94
	public int RequiredPower
	{
		get
		{
			return this.requiredPower;
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0003C89C File Offset: 0x0003AA9C
	public BlockPowered()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0003C8F8 File Offset: 0x0003AAF8
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

	// Token: 0x0600088D RID: 2189 RVA: 0x0003C9C8 File Offset: 0x0003ABC8
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

	// Token: 0x0600088E RID: 2190 RVA: 0x0003CA1C File Offset: 0x0003AC1C
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

	// Token: 0x0600088F RID: 2191 RVA: 0x0003CAA6 File Offset: 0x0003ACA6
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator drawWiresLater(IPowered powered)
	{
		yield return new WaitForSeconds(0.5f);
		powered.DrawWires();
		yield break;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0003CAB8 File Offset: 0x0003ACB8
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

	// Token: 0x06000891 RID: 2193 RVA: 0x0003CBAD File Offset: 0x0003ADAD
	public virtual TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredBlock(chunk);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0003CBB8 File Offset: 0x0003ADB8
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0003CC04 File Offset: 0x0003AE04
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer()) || this.TakeDelay <= 0f)
		{
			return "";
		}
		Block block = _blockValue.Block;
		return string.Format(Localization.Get("pickupPrompt", false, null), block.GetLocalizedBlockName());
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0003CC58 File Offset: 0x0003AE58
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

	// Token: 0x0400097A RID: 2426
	[PublicizedFrom(EAccessModifier.Protected)]
	public int requiredPower = -1;

	// Token: 0x0400097B RID: 2427
	[PublicizedFrom(EAccessModifier.Protected)]
	public string poweredType = "";

	// Token: 0x0400097C RID: 2428
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;

	// Token: 0x0400097D RID: 2429
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}

using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x0200014F RID: 335
[Preserve]
public class BlockSpeaker : BlockPowered
{
	// Token: 0x0600093E RID: 2366 RVA: 0x000408EC File Offset: 0x0003EAEC
	public BlockSpeaker()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00040906 File Offset: 0x0003EB06
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("PlaySound"))
		{
			this.playSound = base.Properties.Values["PlaySound"];
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00040940 File Offset: 0x0003EB40
	public override bool ActivateBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool isOn, bool isPowered)
	{
		byte b = (byte)(((int)_blockValue.meta & -3) | (isOn ? 2 : 0));
		if (_blockValue.meta != b)
		{
			_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (isOn ? 2 : 0));
			_world.SetBlockRPC(_blockPos, _blockValue);
			try
			{
				if (isOn)
				{
					Manager.BroadcastPlay(_blockPos.ToVector3(), this.playSound, 0f);
				}
				else
				{
					Manager.BroadcastStop(_blockPos.ToVector3(), this.playSound);
				}
			}
			catch (Exception)
			{
			}
		}
		return true;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x000409DC File Offset: 0x0003EBDC
	public override void OnBlockUnloaded(WorldBase world, Vector3i blockPos, BlockValue blockValue)
	{
		base.OnBlockUnloaded(world, blockPos, blockValue);
		Manager.Stop(blockPos.ToVector3(), this.playSound);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x000409F9 File Offset: 0x0003EBF9
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		Manager.Stop(_blockPos.ToVector3(), this.playSound);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00040A18 File Offset: 0x0003EC18
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		if ((TileEntityPoweredBlock)_world.GetTileEntity(_blockPos) != null)
		{
			if ((_blockValue.meta & 2) > 0)
			{
				Manager.Play(_blockPos.ToVector3(), this.playSound, -1, false, 1f);
				return;
			}
			Manager.Stop(_blockPos.ToVector3(), this.playSound);
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00040A77 File Offset: 0x0003EC77
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredBlock(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.Consumer
		};
	}

	// Token: 0x040009B5 RID: 2485
	[PublicizedFrom(EAccessModifier.Private)]
	public string playSound;

	// Token: 0x040009B6 RID: 2486
	[PublicizedFrom(EAccessModifier.Private)]
	public float soundDelay = 1f;
}

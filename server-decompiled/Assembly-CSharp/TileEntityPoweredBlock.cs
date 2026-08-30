using System;

// Token: 0x02000C61 RID: 3169
public class TileEntityPoweredBlock : TileEntityPowered
{
	// Token: 0x060060DD RID: 24797 RVA: 0x002647A8 File Offset: 0x002629A8
	public TileEntityPoweredBlock(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x060060DE RID: 24798 RVA: 0x002647B8 File Offset: 0x002629B8
	// (set) Token: 0x060060DF RID: 24799 RVA: 0x002647EC File Offset: 0x002629EC
	public bool IsToggled
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.PowerItem is PowerConsumerToggle)
			{
				return (this.PowerItem as PowerConsumerToggle).IsToggled;
			}
			return this.isToggled;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				if (this.PowerItem is PowerConsumerToggle)
				{
					(this.PowerItem as PowerConsumerToggle).IsToggled = value;
				}
				this.isToggled = value;
				base.SetModified();
				return;
			}
			this.isToggled = value;
			base.SetModified();
		}
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x060060E0 RID: 24800 RVA: 0x0026483E File Offset: 0x00262A3E
	public override int PowerUsed
	{
		get
		{
			if (this.IsToggled)
			{
				return base.PowerUsed;
			}
			return 0;
		}
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x00264850 File Offset: 0x00262A50
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x0026485C File Offset: 0x00262A5C
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x0026489C File Offset: 0x00262A9C
	public override bool ActivateOnce()
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlockOnce(world, base.ToWorldPos(), block);
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x002648DA File Offset: 0x00262ADA
	public override void OnRemove(World world)
	{
		base.OnRemove(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x002648EF File Offset: 0x00262AEF
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x060060E6 RID: 24806 RVA: 0x00264904 File Offset: 0x00262B04
	public override void OnSetLocalChunkPosition()
	{
		base.OnSetLocalChunkPosition();
		if (GameManager.Instance == null)
		{
			return;
		}
		World world = GameManager.Instance.World;
		if (this.chunk != null)
		{
			BlockValue block = this.chunk.GetBlock(base.localChunkPos);
			Block block2 = block.Block;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				block2.ActivateBlock(world, base.ToWorldPos(), block, base.IsPowered, base.IsPowered);
			}
		}
	}

	// Token: 0x060060E7 RID: 24807 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClientUpdate()
	{
	}

	// Token: 0x060060E8 RID: 24808 RVA: 0x0026497C File Offset: 0x00262B7C
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			if (base.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.GetLegacyForkVersion();
			}
		}
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeRead.FromClient)
			{
				this.isToggled = _br.ReadBoolean();
				if (this.PowerItem is PowerConsumerToggle)
				{
					(this.PowerItem as PowerConsumerToggle).IsToggled = this.isToggled;
					return;
				}
			}
			else
			{
				this.isToggled = _br.ReadBoolean();
			}
		}
	}

	// Token: 0x060060E9 RID: 24809 RVA: 0x002649EF File Offset: 0x00262BEF
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (_eStreamMode != TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(this.IsToggled);
		}
	}

	// Token: 0x04004B90 RID: 19344
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004B91 RID: 19345
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isToggled = true;

	// Token: 0x04004B92 RID: 19346
	public float DelayTime;

	// Token: 0x04004B93 RID: 19347
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;
}

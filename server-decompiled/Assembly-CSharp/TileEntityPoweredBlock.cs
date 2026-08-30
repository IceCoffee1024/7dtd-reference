using System;

// Token: 0x02000C73 RID: 3187
public class TileEntityPoweredBlock : TileEntityPowered
{
	// Token: 0x06006168 RID: 24936 RVA: 0x00267690 File Offset: 0x00265890
	public TileEntityPoweredBlock(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06006169 RID: 24937 RVA: 0x002676A0 File Offset: 0x002658A0
	// (set) Token: 0x0600616A RID: 24938 RVA: 0x002676D4 File Offset: 0x002658D4
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

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x0600616B RID: 24939 RVA: 0x00267726 File Offset: 0x00265926
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

	// Token: 0x0600616C RID: 24940 RVA: 0x00267738 File Offset: 0x00265938
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
	}

	// Token: 0x0600616D RID: 24941 RVA: 0x00267744 File Offset: 0x00265944
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x0600616E RID: 24942 RVA: 0x00267784 File Offset: 0x00265984
	public override bool ActivateOnce()
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlockOnce(world, base.ToWorldPos(), block);
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x002677C2 File Offset: 0x002659C2
	public override void OnRemove(World world)
	{
		base.OnRemove(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x06006170 RID: 24944 RVA: 0x002677D7 File Offset: 0x002659D7
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		PowerManager.Instance.SetTileEntityUpdate(this, false);
	}

	// Token: 0x06006171 RID: 24945 RVA: 0x002677EC File Offset: 0x002659EC
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

	// Token: 0x06006172 RID: 24946 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClientUpdate()
	{
	}

	// Token: 0x06006173 RID: 24947 RVA: 0x00267864 File Offset: 0x00265A64
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

	// Token: 0x06006174 RID: 24948 RVA: 0x002678D7 File Offset: 0x00265AD7
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

	// Token: 0x04004C2D RID: 19501
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C2E RID: 19502
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isToggled = true;

	// Token: 0x04004C2F RID: 19503
	public float DelayTime;

	// Token: 0x04004C30 RID: 19504
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;
}

using System;
using UnityEngine;

// Token: 0x02000C6C RID: 3180
public class TileEntityForge : TileEntity
{
	// Token: 0x06006102 RID: 24834 RVA: 0x00264AC8 File Offset: 0x00262CC8
	public TileEntityForge(Chunk _chunk)
		: base(_chunk)
	{
		this.fuel = ItemStack.CreateArray(3);
		this.input = ItemStack.CreateArray(1);
		this.mold = ItemStack.Empty;
		this.output = ItemStack.Empty;
		this.outputItem = ItemValue.None;
		this.burningItemValue = ItemValue.None;
		this.fuelInForgeInTicks = 0;
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x00264B3D File Offset: 0x00262D3D
	public bool CanOperate(ulong _worldTimeInTicks)
	{
		return this.GetFuelLeft(_worldTimeInTicks) + this.GetFuelInStorage() > 0 && this.outputWeight > 0 && this.metalInForge > 0;
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x00264B64 File Offset: 0x00262D64
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		this.recalcStats();
		int num = ((this.lastTickTime != 0UL) ? ((int)(GameTimer.Instance.ticks - this.lastTickTime)) : 0);
		this.lastTickTime = GameTimer.Instance.ticks;
		this.lastTickTimeDataCalculated = this.lastTickTime;
		this.updateLightState(world);
		if (this.fuelInStorageInTicks + this.fuelInForgeInTicks == 0)
		{
			return;
		}
		base.emitHeatMapEvent(world, EnumAIDirectorChunkEvent.Forge);
		bool flag = false;
		num = Utils.FastMin(num, (int)((float)(this.fuelInStorageInTicks + this.fuelInForgeInTicks) / 1f));
		if (this.fuelInStorageInTicks + this.fuelInForgeInTicks > 0)
		{
			int num2 = (int)((float)num * 1f);
			this.fuelInForgeInTicks -= num2;
			while (this.fuelInForgeInTicks < 0)
			{
				flag |= this.moveDown(this.fuel);
				if (!this.fuel[this.fuel.Length - 1].IsEmpty())
				{
					this.burningItemValue = this.fuel[this.fuel.Length - 1].itemValue;
					this.fuelInForgeInTicks += ItemClass.GetFuelValue(this.fuel[this.fuel.Length - 1].itemValue) * 20;
					this.fuel[this.fuel.Length - 1].count--;
					if (this.fuel[this.fuel.Length - 1].count == 0)
					{
						this.fuel[this.fuel.Length - 1].Clear();
					}
				}
			}
			this.updateLightState(world);
			flag = true;
		}
		flag |= this.moveDown(this.fuel);
		if (this.outputWeight > 0)
		{
			int num3 = (int)((float)num * 0.1f);
			while (this.metalInForge < num3 && this.inputMetal >= num3 - this.metalInForge)
			{
				flag |= this.moveDown(this.input);
				if (!this.input[this.input.Length - 1].IsEmpty())
				{
					this.metalInForge += ItemClass.GetForId(this.input[this.input.Length - 1].itemValue.type).GetWeight();
					this.input[this.input.Length - 1].count--;
					if (this.input[this.input.Length - 1].count == 0)
					{
						this.input[this.input.Length - 1].Clear();
					}
					flag = true;
				}
				this.recalcStats();
			}
			if (this.metalInForge > 0)
			{
				num3 = Utils.FastMin(this.metalInForge, num3);
				this.metalInForge -= num3;
				this.moldedMetalSoFar += num3;
				flag = true;
			}
			bool flag2 = false;
			while (this.moldedMetalSoFar >= this.outputWeight)
			{
				this.moldedMetalSoFar -= this.outputWeight;
				this.output = new ItemStack(this.outputItem, this.output.count + 1);
				flag2 = true;
				flag = true;
			}
			if (flag2)
			{
				world.GetGameManager().PlaySoundAtPositionServer(base.ToWorldPos().ToVector3(), "Forge/forge_item_complete", AudioRolloffMode.Logarithmic, 100, 1f);
			}
		}
		if (flag)
		{
			this.setModified();
		}
	}

	// Token: 0x06006105 RID: 24837 RVA: 0x00264EA0 File Offset: 0x002630A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateLightState(World world)
	{
		BlockValue block = world.GetBlock(base.ToWorldPos());
		if (this.fuelInStorageInTicks + this.fuelInForgeInTicks == 0 && block.meta != 0)
		{
			block.meta = 0;
			world.SetBlockRPC(base.ToWorldPos(), block);
			return;
		}
		if (this.fuelInStorageInTicks + this.fuelInForgeInTicks != 0 && block.meta == 0)
		{
			block.meta = 1;
			world.SetBlockRPC(base.ToWorldPos(), block);
		}
	}

	// Token: 0x06006106 RID: 24838 RVA: 0x00264F20 File Offset: 0x00263120
	[PublicizedFrom(EAccessModifier.Private)]
	public bool moveDown(ItemStack[] _items)
	{
		if (_items.Length < 2)
		{
			return false;
		}
		bool flag = false;
		for (int i = _items.Length - 1; i > 0; i--)
		{
			if (_items[i].IsEmpty() && !_items[i - 1].IsEmpty())
			{
				_items[i] = _items[i - 1].Clone();
				_items[i - 1].Clear();
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x06006107 RID: 24839 RVA: 0x00264F78 File Offset: 0x00263178
	public int GetFuelLeft(ulong _worldTimeInTicks)
	{
		if (_worldTimeInTicks == 0UL || this.lastTickTimeDataCalculated == 0UL)
		{
			return this.fuelInForgeInTicks / 20;
		}
		float num = (_worldTimeInTicks - this.lastTickTimeDataCalculated) * 1f;
		return (int)Math.Max((float)this.fuelInForgeInTicks - num, 0f) / 20;
	}

	// Token: 0x06006108 RID: 24840 RVA: 0x00264FC4 File Offset: 0x002631C4
	public override bool IsActive(World world)
	{
		return world.GetBlock(base.ToWorldPos()).meta > 0;
	}

	// Token: 0x06006109 RID: 24841 RVA: 0x00264FE8 File Offset: 0x002631E8
	public int GetInputWeight()
	{
		return this.inputMetal;
	}

	// Token: 0x0600610A RID: 24842 RVA: 0x00264FF0 File Offset: 0x002631F0
	public int GetFuelInStorage()
	{
		return this.fuelInStorageInTicks / 20;
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x00264FFC File Offset: 0x002631FC
	public int GetMetalForgedSoFar(ulong _currentTickTime)
	{
		if (_currentTickTime == 0UL || this.lastTickTimeDataCalculated == 0UL || !this.CanOperate(_currentTickTime))
		{
			return this.moldedMetalSoFar;
		}
		int num = (int)((_currentTickTime - this.lastTickTimeDataCalculated) * 0.1f);
		if (num < this.metalInForge)
		{
			return Math.Min(this.moldedMetalSoFar + num, this.moldedMetalSoFar + this.metalInForge);
		}
		return Math.Min(this.moldedMetalSoFar + num, this.outputWeight);
	}

	// Token: 0x0600610C RID: 24844 RVA: 0x0026506D File Offset: 0x0026326D
	public int GetOutputWeight()
	{
		return this.outputWeight;
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x00265078 File Offset: 0x00263278
	public int GetCurrentMetalInForge(ulong _currentTickTime)
	{
		if (_currentTickTime == 0UL || this.lastTickTimeDataCalculated == 0UL || !this.CanOperate(_currentTickTime))
		{
			return this.metalInForge;
		}
		int num = (int)((_currentTickTime - this.lastTickTimeDataCalculated) * 0.1f);
		return Math.Max(this.metalInForge - num, 0);
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x002650C0 File Offset: 0x002632C0
	public float GetMoldTimeNeeded(ulong _currentTickTime)
	{
		return (float)(this.GetInputWeight() + this.GetCurrentMetalInForge(_currentTickTime)) / 2f;
	}

	// Token: 0x0600610F RID: 24847 RVA: 0x002650D7 File Offset: 0x002632D7
	public ItemStack[] GetFuel()
	{
		return this.fuel;
	}

	// Token: 0x06006110 RID: 24848 RVA: 0x002650DF File Offset: 0x002632DF
	public void SetFuel(ItemStack[] _fuel)
	{
		this.fuel = ItemStack.Clone(_fuel);
		this.setModified();
	}

	// Token: 0x06006111 RID: 24849 RVA: 0x002650F3 File Offset: 0x002632F3
	public ItemStack[] GetInput()
	{
		return this.input;
	}

	// Token: 0x06006112 RID: 24850 RVA: 0x002650FB File Offset: 0x002632FB
	public void SetInput(ItemStack[] _input, bool _bSetModified = true)
	{
		this.input = ItemStack.Clone(_input);
		if (_bSetModified)
		{
			this.setModified();
		}
	}

	// Token: 0x06006113 RID: 24851 RVA: 0x00265112 File Offset: 0x00263312
	public ItemStack GetMold()
	{
		return this.mold;
	}

	// Token: 0x06006114 RID: 24852 RVA: 0x0026511A File Offset: 0x0026331A
	public void SetMold(ItemStack _mold)
	{
		this.mold = _mold;
		this.moldedMetalSoFar = 0;
		this.metalInForge = 0;
		this.setModified();
	}

	// Token: 0x06006115 RID: 24853 RVA: 0x00265137 File Offset: 0x00263337
	public ItemStack GetOutput()
	{
		return this.output;
	}

	// Token: 0x06006116 RID: 24854 RVA: 0x0026513F File Offset: 0x0026333F
	public ItemValue GetBurningItemValue()
	{
		return this.burningItemValue;
	}

	// Token: 0x06006117 RID: 24855 RVA: 0x00265147 File Offset: 0x00263347
	public void SetOutput(ItemStack _output, bool _bSetModified = true)
	{
		this.output = _output;
		if (_bSetModified)
		{
			this.setModified();
		}
	}

	// Token: 0x06006118 RID: 24856 RVA: 0x00265159 File Offset: 0x00263359
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setModified()
	{
		this.recalcStats();
		base.setModified();
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x00265168 File Offset: 0x00263368
	[PublicizedFrom(EAccessModifier.Private)]
	public void recalcStats()
	{
		this.outputWeight = 0;
		this.outputItem = ItemValue.None;
		if (!this.mold.IsEmpty())
		{
			this.outputItem = new ItemValue(ItemClass.GetForId(this.mold.itemValue.type).MoldTarget.Id, false);
			this.outputWeight = ItemClass.GetForId(this.outputItem.type).GetWeight();
		}
		this.fuelInStorageInTicks = 0;
		for (int i = 0; i < this.fuel.Length; i++)
		{
			if (!this.fuel[i].IsEmpty())
			{
				this.fuelInStorageInTicks += ItemClass.GetFuelValue(this.fuel[i].itemValue) * this.fuel[i].count * 20;
			}
		}
		this.inputMetal = 0;
		for (int j = 0; j < this.input.Length; j++)
		{
			ItemClass forId = ItemClass.GetForId(this.input[j].itemValue.type);
			if (forId != null)
			{
				this.inputMetal += forId.GetWeight() * this.input[j].count;
			}
		}
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x0026528C File Offset: 0x0026348C
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			num = 18;
		}
		else if (base.UseLocalVersioning())
		{
			num = (int)_br.ReadUInt16();
		}
		else
		{
			num = base.GetLegacyForkVersion();
		}
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			this.lastTickTime = _br.ReadUInt64();
			this.lastTickTimeDataCalculated = GameTimer.Instance.ticks;
			int num2 = (int)_br.ReadByte();
			if (this.fuel == null || this.fuel.Length != num2)
			{
				this.fuel = ItemStack.CreateArray(num2);
			}
			if (num < 3)
			{
				for (int i = 0; i < num2; i++)
				{
					this.fuel[i].ReadOld(_br);
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					this.fuel[j].Read(_br);
				}
			}
			int num3 = (int)_br.ReadByte();
			if (this.input == null || this.input.Length != num3)
			{
				this.input = ItemStack.CreateArray(num3);
			}
			if (num < 3)
			{
				for (int k = 0; k < num3; k++)
				{
					this.input[k].Read(_br);
				}
			}
			else
			{
				for (int l = 0; l < num3; l++)
				{
					this.input[l].Read(_br);
				}
			}
			if (num < 3)
			{
				this.mold.ReadOld(_br);
				this.output.ReadOld(_br);
			}
			else
			{
				this.mold.Read(_br);
				this.output.Read(_br);
			}
			this.fuelInForgeInTicks = _br.ReadInt32();
			this.moldedMetalSoFar = (int)_br.ReadInt16();
			this.metalInForge = (int)_br.ReadInt16();
			this.burningItemValue.Read(_br);
		}
		else if (_eStreamMode == TileEntity.StreamModeRead.FromClient)
		{
			this.lastTickTimeDataCalculated = GameTimer.Instance.ticks;
			int num4 = (int)_br.ReadByte();
			if (this.fuel == null || this.fuel.Length != num4)
			{
				this.fuel = ItemStack.CreateArray(num4);
			}
			for (int m = 0; m < num4; m++)
			{
				this.fuel[m].ReadDelta(_br, this.fuel[m]);
			}
			int num5 = (int)_br.ReadByte();
			if (this.input == null || this.input.Length != num5)
			{
				this.input = ItemStack.CreateArray(num5);
			}
			for (int n = 0; n < num5; n++)
			{
				this.input[n].ReadDelta(_br, this.input[n]);
			}
			this.mold.ReadDelta(_br, this.mold);
			this.output.ReadDelta(_br, this.output);
			if (this.mold.itemValue.type == 0)
			{
				this.moldedMetalSoFar = 0;
				this.metalInForge = 0;
			}
		}
		else if (_eStreamMode == TileEntity.StreamModeRead.FromServer)
		{
			if (base.bWaitingForServerResponse)
			{
				Log.Warning("Throwing away server packet as we are waiting for status update!");
			}
			this.lastTickTimeDataCalculated = GameTimer.Instance.ticks;
			int num6 = (int)_br.ReadByte();
			if (this.fuel == null || this.fuel.Length != num6)
			{
				this.fuel = ItemStack.CreateArray(num6);
			}
			if (!base.bWaitingForServerResponse)
			{
				for (int num7 = 0; num7 < num6; num7++)
				{
					this.fuel[num7].Read(_br);
				}
				this.lastServerFuel = ItemStack.Clone(this.fuel);
			}
			else
			{
				ItemStack empty = ItemStack.Empty;
				for (int num8 = 0; num8 < num6; num8++)
				{
					empty.Read(_br);
				}
			}
			int num9 = (int)_br.ReadByte();
			if (this.input == null || this.input.Length != num9)
			{
				this.input = ItemStack.CreateArray(num9);
			}
			if (!base.bWaitingForServerResponse)
			{
				for (int num10 = 0; num10 < num9; num10++)
				{
					this.input[num10].Read(_br);
				}
				this.lastServerInput = ItemStack.Clone(this.input);
			}
			else
			{
				ItemStack empty2 = ItemStack.Empty;
				for (int num11 = 0; num11 < num9; num11++)
				{
					empty2.Read(_br);
				}
			}
			if (!base.bWaitingForServerResponse)
			{
				this.mold.Read(_br);
				this.lastServerMold = this.mold.Clone();
			}
			else
			{
				ItemStack.Empty.Read(_br);
			}
			if (!base.bWaitingForServerResponse)
			{
				this.output.Read(_br);
				this.lastServerOutput = this.output.Clone();
			}
			else
			{
				ItemStack.Empty.Read(_br);
			}
			this.fuelInForgeInTicks = _br.ReadInt32();
			this.moldedMetalSoFar = (int)_br.ReadInt16();
			this.metalInForge = (int)_br.ReadInt16();
			this.burningItemValue.Read(_br);
		}
		this.recalcStats();
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x00265704 File Offset: 0x00263904
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(this.lastTickTime);
			_bw.Write((byte)this.fuel.Length);
			for (int i = 0; i < this.fuel.Length; i++)
			{
				this.fuel[i].Write(_bw);
			}
			_bw.Write((byte)this.input.Length);
			for (int j = 0; j < this.input.Length; j++)
			{
				this.input[j].Write(_bw);
			}
			this.mold.Write(_bw);
			this.output.Write(_bw);
			_bw.Write(this.fuelInForgeInTicks);
			_bw.Write((short)this.moldedMetalSoFar);
			_bw.Write((short)this.metalInForge);
			this.burningItemValue.Write(_bw);
			return;
		}
		if (_eStreamMode == TileEntity.StreamModeWrite.ToServer)
		{
			_bw.Write((byte)this.fuel.Length);
			for (int k = 0; k < this.fuel.Length; k++)
			{
				this.fuel[k].WriteDelta(_bw, (this.lastServerFuel != null) ? this.lastServerFuel[k] : ItemStack.Empty);
			}
			_bw.Write((byte)this.input.Length);
			for (int l = 0; l < this.input.Length; l++)
			{
				this.input[l].WriteDelta(_bw, (this.lastServerInput != null) ? this.lastServerInput[l] : ItemStack.Empty);
			}
			this.mold.WriteDelta(_bw, this.lastServerMold);
			this.output.WriteDelta(_bw, this.lastServerOutput);
			return;
		}
		if (_eStreamMode == TileEntity.StreamModeWrite.ToClient)
		{
			_bw.Write((byte)this.fuel.Length);
			for (int m = 0; m < this.fuel.Length; m++)
			{
				this.fuel[m].Write(_bw);
			}
			_bw.Write((byte)this.input.Length);
			for (int n = 0; n < this.input.Length; n++)
			{
				this.input[n].Write(_bw);
			}
			this.mold.Write(_bw);
			this.output.Write(_bw);
			_bw.Write(this.fuelInForgeInTicks);
			_bw.Write((short)this.moldedMetalSoFar);
			_bw.Write((short)this.metalInForge);
			this.burningItemValue.Write(_bw);
		}
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x0008121D File Offset: 0x0007F41D
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Forge;
	}

	// Token: 0x04004BEE RID: 19438
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004BEF RID: 19439
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cFuelBurnPerTick = 1f;

	// Token: 0x04004BF0 RID: 19440
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMoldPerTick = 0.1f;

	// Token: 0x04004BF1 RID: 19441
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] fuel;

	// Token: 0x04004BF2 RID: 19442
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] input;

	// Token: 0x04004BF3 RID: 19443
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack mold;

	// Token: 0x04004BF4 RID: 19444
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack output;

	// Token: 0x04004BF5 RID: 19445
	[PublicizedFrom(EAccessModifier.Private)]
	public int fuelInForgeInTicks;

	// Token: 0x04004BF6 RID: 19446
	[PublicizedFrom(EAccessModifier.Private)]
	public int moldedMetalSoFar;

	// Token: 0x04004BF7 RID: 19447
	[PublicizedFrom(EAccessModifier.Private)]
	public int metalInForge;

	// Token: 0x04004BF8 RID: 19448
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue burningItemValue;

	// Token: 0x04004BF9 RID: 19449
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastTickTime;

	// Token: 0x04004BFA RID: 19450
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastTickTimeDataCalculated;

	// Token: 0x04004BFB RID: 19451
	[PublicizedFrom(EAccessModifier.Private)]
	public int inputMetal;

	// Token: 0x04004BFC RID: 19452
	[PublicizedFrom(EAccessModifier.Private)]
	public int outputWeight;

	// Token: 0x04004BFD RID: 19453
	[PublicizedFrom(EAccessModifier.Private)]
	public int fuelInStorageInTicks;

	// Token: 0x04004BFE RID: 19454
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue outputItem;

	// Token: 0x04004BFF RID: 19455
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] lastServerFuel;

	// Token: 0x04004C00 RID: 19456
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] lastServerInput;

	// Token: 0x04004C01 RID: 19457
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastServerMold = ItemStack.Empty;

	// Token: 0x04004C02 RID: 19458
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastServerOutput = ItemStack.Empty;
}

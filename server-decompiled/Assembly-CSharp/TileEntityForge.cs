using System;
using UnityEngine;

// Token: 0x02000C86 RID: 3206
public class TileEntityForge : TileEntity
{
	// Token: 0x06006124 RID: 24868 RVA: 0x00269464 File Offset: 0x00267664
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

	// Token: 0x06006125 RID: 24869 RVA: 0x002694D9 File Offset: 0x002676D9
	public bool CanOperate(ulong _worldTimeInTicks)
	{
		return this.GetFuelLeft(_worldTimeInTicks) + this.GetFuelInStorage() > 0 && this.outputWeight > 0 && this.metalInForge > 0;
	}

	// Token: 0x06006126 RID: 24870 RVA: 0x00269500 File Offset: 0x00267700
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
					ItemStack itemStack = this.fuel[this.fuel.Length - 1];
					int num3 = itemStack.count;
					itemStack.count = num3 - 1;
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
			int num4 = (int)((float)num * 0.1f);
			while (this.metalInForge < num4 && this.inputMetal >= num4 - this.metalInForge)
			{
				flag |= this.moveDown(this.input);
				if (!this.input[this.input.Length - 1].IsEmpty())
				{
					this.metalInForge += ItemClass.GetForId(this.input[this.input.Length - 1].itemValue.type).GetWeight();
					ItemStack itemStack2 = this.input[this.input.Length - 1];
					int num3 = itemStack2.count;
					itemStack2.count = num3 - 1;
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
				num4 = Utils.FastMin(this.metalInForge, num4);
				this.metalInForge -= num4;
				this.moldedMetalSoFar += num4;
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

	// Token: 0x06006127 RID: 24871 RVA: 0x00269848 File Offset: 0x00267A48
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

	// Token: 0x06006128 RID: 24872 RVA: 0x002698C8 File Offset: 0x00267AC8
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

	// Token: 0x06006129 RID: 24873 RVA: 0x00269920 File Offset: 0x00267B20
	public int GetFuelLeft(ulong _worldTimeInTicks)
	{
		if (_worldTimeInTicks == 0UL || this.lastTickTimeDataCalculated == 0UL)
		{
			return this.fuelInForgeInTicks / 20;
		}
		float num = (_worldTimeInTicks - this.lastTickTimeDataCalculated) * 1f;
		return (int)Math.Max((float)this.fuelInForgeInTicks - num, 0f) / 20;
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x0026996C File Offset: 0x00267B6C
	public override bool IsActive(World world)
	{
		return world.GetBlock(base.ToWorldPos()).meta > 0;
	}

	// Token: 0x0600612B RID: 24875 RVA: 0x00269990 File Offset: 0x00267B90
	public int GetInputWeight()
	{
		return this.inputMetal;
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x00269998 File Offset: 0x00267B98
	public int GetFuelInStorage()
	{
		return this.fuelInStorageInTicks / 20;
	}

	// Token: 0x0600612D RID: 24877 RVA: 0x002699A4 File Offset: 0x00267BA4
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

	// Token: 0x0600612E RID: 24878 RVA: 0x00269A15 File Offset: 0x00267C15
	public int GetOutputWeight()
	{
		return this.outputWeight;
	}

	// Token: 0x0600612F RID: 24879 RVA: 0x00269A20 File Offset: 0x00267C20
	public int GetCurrentMetalInForge(ulong _currentTickTime)
	{
		if (_currentTickTime == 0UL || this.lastTickTimeDataCalculated == 0UL || !this.CanOperate(_currentTickTime))
		{
			return this.metalInForge;
		}
		int num = (int)((_currentTickTime - this.lastTickTimeDataCalculated) * 0.1f);
		return Math.Max(this.metalInForge - num, 0);
	}

	// Token: 0x06006130 RID: 24880 RVA: 0x00269A68 File Offset: 0x00267C68
	public float GetMoldTimeNeeded(ulong _currentTickTime)
	{
		return (float)(this.GetInputWeight() + this.GetCurrentMetalInForge(_currentTickTime)) / 2f;
	}

	// Token: 0x06006131 RID: 24881 RVA: 0x00269A7F File Offset: 0x00267C7F
	public ItemStack[] GetFuel()
	{
		return this.fuel;
	}

	// Token: 0x06006132 RID: 24882 RVA: 0x00269A87 File Offset: 0x00267C87
	public void SetFuel(ItemStack[] _fuel)
	{
		this.fuel = ItemStack.Clone(_fuel);
		this.setModified();
	}

	// Token: 0x06006133 RID: 24883 RVA: 0x00269A9B File Offset: 0x00267C9B
	public ItemStack[] GetInput()
	{
		return this.input;
	}

	// Token: 0x06006134 RID: 24884 RVA: 0x00269AA3 File Offset: 0x00267CA3
	public void SetInput(ItemStack[] _input, bool _bSetModified = true)
	{
		this.input = ItemStack.Clone(_input);
		if (_bSetModified)
		{
			this.setModified();
		}
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x00269ABA File Offset: 0x00267CBA
	public ItemStack GetMold()
	{
		return this.mold;
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x00269AC2 File Offset: 0x00267CC2
	public void SetMold(ItemStack _mold)
	{
		this.mold = _mold;
		this.moldedMetalSoFar = 0;
		this.metalInForge = 0;
		this.setModified();
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x00269ADF File Offset: 0x00267CDF
	public ItemStack GetOutput()
	{
		return this.output;
	}

	// Token: 0x06006138 RID: 24888 RVA: 0x00269AE7 File Offset: 0x00267CE7
	public ItemValue GetBurningItemValue()
	{
		return this.burningItemValue;
	}

	// Token: 0x06006139 RID: 24889 RVA: 0x00269AEF File Offset: 0x00267CEF
	public void SetOutput(ItemStack _output, bool _bSetModified = true)
	{
		this.output = _output;
		if (_bSetModified)
		{
			this.setModified();
		}
	}

	// Token: 0x0600613A RID: 24890 RVA: 0x00269B01 File Offset: 0x00267D01
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setModified()
	{
		this.recalcStats();
		base.setModified();
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x00269B10 File Offset: 0x00267D10
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

	// Token: 0x0600613C RID: 24892 RVA: 0x00269C34 File Offset: 0x00267E34
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != StreamModeRead.Persistency)
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
		if (_eStreamMode == StreamModeRead.Persistency)
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
		else if (_eStreamMode == StreamModeRead.FromClient)
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
		else if (_eStreamMode == StreamModeRead.FromServer)
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

	// Token: 0x0600613D RID: 24893 RVA: 0x0026A0AC File Offset: 0x002682AC
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (_eStreamMode == StreamModeWrite.Persistency)
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
		if (_eStreamMode == StreamModeWrite.ToServer)
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
		if (_eStreamMode == StreamModeWrite.ToClient)
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

	// Token: 0x0600613E RID: 24894 RVA: 0x000816F5 File Offset: 0x0007F8F5
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Forge;
	}

	// Token: 0x04004C4A RID: 19530
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C4B RID: 19531
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cFuelBurnPerTick = 1f;

	// Token: 0x04004C4C RID: 19532
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMoldPerTick = 0.1f;

	// Token: 0x04004C4D RID: 19533
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] fuel;

	// Token: 0x04004C4E RID: 19534
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] input;

	// Token: 0x04004C4F RID: 19535
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack mold;

	// Token: 0x04004C50 RID: 19536
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack output;

	// Token: 0x04004C51 RID: 19537
	[PublicizedFrom(EAccessModifier.Private)]
	public int fuelInForgeInTicks;

	// Token: 0x04004C52 RID: 19538
	[PublicizedFrom(EAccessModifier.Private)]
	public int moldedMetalSoFar;

	// Token: 0x04004C53 RID: 19539
	[PublicizedFrom(EAccessModifier.Private)]
	public int metalInForge;

	// Token: 0x04004C54 RID: 19540
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue burningItemValue;

	// Token: 0x04004C55 RID: 19541
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastTickTime;

	// Token: 0x04004C56 RID: 19542
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastTickTimeDataCalculated;

	// Token: 0x04004C57 RID: 19543
	[PublicizedFrom(EAccessModifier.Private)]
	public int inputMetal;

	// Token: 0x04004C58 RID: 19544
	[PublicizedFrom(EAccessModifier.Private)]
	public int outputWeight;

	// Token: 0x04004C59 RID: 19545
	[PublicizedFrom(EAccessModifier.Private)]
	public int fuelInStorageInTicks;

	// Token: 0x04004C5A RID: 19546
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue outputItem;

	// Token: 0x04004C5B RID: 19547
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] lastServerFuel;

	// Token: 0x04004C5C RID: 19548
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] lastServerInput;

	// Token: 0x04004C5D RID: 19549
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastServerMold = ItemStack.Empty;

	// Token: 0x04004C5E RID: 19550
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastServerOutput = ItemStack.Empty;
}

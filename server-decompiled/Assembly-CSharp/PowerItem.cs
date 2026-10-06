using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class PowerItem
{
	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x060042A3 RID: 17059 RVA: 0x001A29BD File Offset: 0x001A0BBD
	public virtual bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x001A29C5 File Offset: 0x001A0BC5
	public PowerItem()
	{
		this.Children = new List<PowerItem>();
	}

	// Token: 0x060042A5 RID: 17061 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanParent(PowerItem newParent)
	{
		return true;
	}

	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x060042A6 RID: 17062 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual int InputCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x060042A7 RID: 17063 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Consumer;
		}
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x001A29EA File Offset: 0x001A0BEA
	public virtual void AddTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == null)
		{
			this.TileEntity = tileEntityPowered;
			this.TileEntity.CreateWireDataFromPowerItem();
		}
		this.TileEntity.MarkWireDirty();
	}

	// Token: 0x060042A9 RID: 17065 RVA: 0x001A2A11 File Offset: 0x001A0C11
	public void RemoveTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == tileEntityPowered)
		{
			this.TileEntity = null;
		}
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x001A2A23 File Offset: 0x001A0C23
	public virtual PowerItem GetRoot()
	{
		if (this.Parent != null)
		{
			return this.Parent.GetRoot();
		}
		return this;
	}

	// Token: 0x060042AB RID: 17067 RVA: 0x001A2A3C File Offset: 0x001A0C3C
	public virtual void read(PooledBinaryReader _br, byte _version)
	{
		this.BlockID = _br.ReadUInt16();
		this.SetValuesFromBlock();
		this.Position = StreamUtils.ReadVector3i(_br);
		if (_br.ReadBoolean())
		{
			PowerManager.Instance.SetParent(this, PowerManager.Instance.GetPowerItemByWorldPos(StreamUtils.ReadVector3i(_br)));
		}
		int num = (int)_br.ReadByte();
		this.Children.Clear();
		for (int i = 0; i < num; i++)
		{
			PowerItem powerItem = PowerItem.CreateItem((PowerItem.PowerItemTypes)_br.ReadByte());
			powerItem.read(_br, _version);
			PowerManager.Instance.AddPowerNode(powerItem, this);
		}
	}

	// Token: 0x060042AC RID: 17068 RVA: 0x001A2AC8 File Offset: 0x001A0CC8
	public void RemoveSelfFromParent()
	{
		PowerManager.Instance.RemoveParent(this);
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x001A2AD8 File Offset: 0x001A0CD8
	public virtual void write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.BlockID);
		StreamUtils.Write(_bw, this.Position);
		_bw.Write(this.Parent != null);
		if (this.Parent != null)
		{
			StreamUtils.Write(_bw, this.Parent.Position);
		}
		_bw.Write((byte)this.Children.Count);
		for (int i = 0; i < this.Children.Count; i++)
		{
			_bw.Write((byte)this.Children[i].PowerItemType);
			this.Children[i].write(_bw);
		}
	}

	// Token: 0x060042AE RID: 17070 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool PowerChildren()
	{
		return true;
	}

	// Token: 0x060042AF RID: 17071 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void IsPoweredChanged(bool newPowered)
	{
	}

	// Token: 0x060042B0 RID: 17072 RVA: 0x001A2B78 File Offset: 0x001A0D78
	public virtual void HandlePowerReceived(ref ushort power)
	{
		ushort num = (ushort)Mathf.Min((int)this.RequiredPower, (int)power);
		bool flag = num == this.RequiredPower;
		if (flag != this.isPowered)
		{
			this.isPowered = flag;
			this.IsPoweredChanged(flag);
			if (this.TileEntity != null)
			{
				this.TileEntity.SetModified();
			}
		}
		power -= num;
		if (power <= 0)
		{
			return;
		}
		if (this.PowerChildren())
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerReceived(ref power);
				if (power <= 0)
				{
					return;
				}
			}
		}
	}

	// Token: 0x060042B1 RID: 17073 RVA: 0x001A2C0C File Offset: 0x001A0E0C
	[PublicizedFrom(EAccessModifier.Internal)]
	public PowerItem GetChild(Vector3 childPosition)
	{
		Vector3i vector3i = new Vector3i(childPosition);
		for (int i = 0; i < this.Children.Count; i++)
		{
			if (this.Children[i].Position == vector3i)
			{
				return this.Children[i];
			}
		}
		return null;
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x001A2C60 File Offset: 0x001A0E60
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool HasChild(Vector3 child)
	{
		Vector3i vector3i = new Vector3i(child);
		for (int i = 0; i < this.Children.Count; i++)
		{
			if (this.Children[i].Position == vector3i)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060042B3 RID: 17075 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePowerUpdate(bool isOn)
	{
	}

	// Token: 0x060042B4 RID: 17076 RVA: 0x001A2CA8 File Offset: 0x001A0EA8
	public virtual void HandleDisconnect()
	{
		if (this.isPowered)
		{
			this.IsPoweredChanged(false);
		}
		this.isPowered = false;
		this.HandlePowerUpdate(false);
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandleDisconnect();
		}
	}

	// Token: 0x060042B5 RID: 17077 RVA: 0x001A2CFC File Offset: 0x001A0EFC
	public static PowerItem CreateItem(PowerItem.PowerItemTypes itemType)
	{
		switch (itemType)
		{
		case PowerItem.PowerItemTypes.Consumer:
			return new PowerConsumer();
		case PowerItem.PowerItemTypes.ConsumerToggle:
			return new PowerConsumerToggle();
		case PowerItem.PowerItemTypes.Trigger:
			return new PowerTrigger();
		case PowerItem.PowerItemTypes.Timer:
			return new PowerTimerRelay();
		case PowerItem.PowerItemTypes.Generator:
			return new PowerGenerator();
		case PowerItem.PowerItemTypes.SolarPanel:
			return new PowerSolarPanel();
		case PowerItem.PowerItemTypes.BatteryBank:
			return new PowerBatteryBank();
		case PowerItem.PowerItemTypes.RangedTrap:
			return new PowerRangedTrap();
		case PowerItem.PowerItemTypes.ElectricWireRelay:
			return new PowerElectricWireRelay();
		case PowerItem.PowerItemTypes.TripWireRelay:
			return new PowerTripWireRelay();
		case PowerItem.PowerItemTypes.PressurePlate:
			return new PowerPressurePlate();
		default:
			return new PowerItem();
		}
	}

	// Token: 0x060042B6 RID: 17078 RVA: 0x001A2D88 File Offset: 0x001A0F88
	public virtual void SetValuesFromBlock()
	{
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x060042B7 RID: 17079 RVA: 0x001A2DDC File Offset: 0x001A0FDC
	public void ClearChildren()
	{
		for (int i = 0; i < this.Children.Count; i++)
		{
			PowerManager.Instance.RemoveChild(this.Children[i]);
		}
		if (this.TileEntity != null)
		{
			this.TileEntity.DrawWires();
		}
	}

	// Token: 0x060042B8 RID: 17080 RVA: 0x001A2E28 File Offset: 0x001A1028
	public void SendHasLocalChangesToRoot()
	{
		this.hasChangesLocal = true;
		for (PowerItem powerItem = this.Parent; powerItem != null; powerItem = powerItem.Parent)
		{
			powerItem.hasChangesLocal = true;
		}
	}

	// Token: 0x0400361F RID: 13855
	public PowerItem Parent;

	// Token: 0x04003620 RID: 13856
	public Vector3i Position;

	// Token: 0x04003621 RID: 13857
	public PowerItem Root;

	// Token: 0x04003622 RID: 13858
	public ushort Depth = ushort.MaxValue;

	// Token: 0x04003623 RID: 13859
	public ushort BlockID;

	// Token: 0x04003624 RID: 13860
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool hasChangesLocal;

	// Token: 0x04003625 RID: 13861
	public ushort RequiredPower = 5;

	// Token: 0x04003626 RID: 13862
	public List<PowerItem> Children;

	// Token: 0x04003627 RID: 13863
	public TileEntityPowered TileEntity;

	// Token: 0x04003628 RID: 13864
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPowered;

	// Token: 0x02000913 RID: 2323
	public enum PowerItemTypes
	{
		// Token: 0x0400362A RID: 13866
		None,
		// Token: 0x0400362B RID: 13867
		Consumer,
		// Token: 0x0400362C RID: 13868
		ConsumerToggle,
		// Token: 0x0400362D RID: 13869
		Trigger,
		// Token: 0x0400362E RID: 13870
		Timer,
		// Token: 0x0400362F RID: 13871
		Generator,
		// Token: 0x04003630 RID: 13872
		SolarPanel,
		// Token: 0x04003631 RID: 13873
		BatteryBank,
		// Token: 0x04003632 RID: 13874
		RangedTrap,
		// Token: 0x04003633 RID: 13875
		ElectricWireRelay,
		// Token: 0x04003634 RID: 13876
		TripWireRelay,
		// Token: 0x04003635 RID: 13877
		PressurePlate
	}
}

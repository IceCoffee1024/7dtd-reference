using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200091C RID: 2332
public class PowerItem
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x060042C6 RID: 17094 RVA: 0x001A3A65 File Offset: 0x001A1C65
	public virtual bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x001A3A6D File Offset: 0x001A1C6D
	public PowerItem()
	{
		this.Children = new List<PowerItem>();
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanParent(PowerItem newParent)
	{
		return true;
	}

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x060042C9 RID: 17097 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual int InputCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x060042CA RID: 17098 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Consumer;
		}
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x001A3A92 File Offset: 0x001A1C92
	public virtual void AddTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == null)
		{
			this.TileEntity = tileEntityPowered;
			this.TileEntity.CreateWireDataFromPowerItem();
		}
		this.TileEntity.MarkWireDirty();
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x001A3AB9 File Offset: 0x001A1CB9
	public void RemoveTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == tileEntityPowered)
		{
			this.TileEntity = null;
		}
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x001A3ACB File Offset: 0x001A1CCB
	public virtual PowerItem GetRoot()
	{
		if (this.Parent != null)
		{
			return this.Parent.GetRoot();
		}
		return this;
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x001A3AE4 File Offset: 0x001A1CE4
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

	// Token: 0x060042CF RID: 17103 RVA: 0x001A3B70 File Offset: 0x001A1D70
	public void RemoveSelfFromParent()
	{
		PowerManager.Instance.RemoveParent(this);
	}

	// Token: 0x060042D0 RID: 17104 RVA: 0x001A3B80 File Offset: 0x001A1D80
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

	// Token: 0x060042D1 RID: 17105 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool PowerChildren()
	{
		return true;
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void IsPoweredChanged(bool newPowered)
	{
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x001A3C20 File Offset: 0x001A1E20
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

	// Token: 0x060042D4 RID: 17108 RVA: 0x001A3CB4 File Offset: 0x001A1EB4
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

	// Token: 0x060042D5 RID: 17109 RVA: 0x001A3D08 File Offset: 0x001A1F08
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

	// Token: 0x060042D6 RID: 17110 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePowerUpdate(bool isOn)
	{
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x001A3D50 File Offset: 0x001A1F50
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

	// Token: 0x060042D8 RID: 17112 RVA: 0x001A3DA4 File Offset: 0x001A1FA4
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

	// Token: 0x060042D9 RID: 17113 RVA: 0x001A3E30 File Offset: 0x001A2030
	public virtual void SetValuesFromBlock()
	{
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x001A3E84 File Offset: 0x001A2084
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

	// Token: 0x060042DB RID: 17115 RVA: 0x001A3ED0 File Offset: 0x001A20D0
	public void SendHasLocalChangesToRoot()
	{
		this.hasChangesLocal = true;
		for (PowerItem powerItem = this.Parent; powerItem != null; powerItem = powerItem.Parent)
		{
			powerItem.hasChangesLocal = true;
		}
	}

	// Token: 0x0400363F RID: 13887
	public PowerItem Parent;

	// Token: 0x04003640 RID: 13888
	public Vector3i Position;

	// Token: 0x04003641 RID: 13889
	public PowerItem Root;

	// Token: 0x04003642 RID: 13890
	public ushort Depth = ushort.MaxValue;

	// Token: 0x04003643 RID: 13891
	public ushort BlockID;

	// Token: 0x04003644 RID: 13892
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool hasChangesLocal;

	// Token: 0x04003645 RID: 13893
	public ushort RequiredPower = 5;

	// Token: 0x04003646 RID: 13894
	public List<PowerItem> Children;

	// Token: 0x04003647 RID: 13895
	public TileEntityPowered TileEntity;

	// Token: 0x04003648 RID: 13896
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPowered;

	// Token: 0x0200091D RID: 2333
	public enum PowerItemTypes
	{
		// Token: 0x0400364A RID: 13898
		None,
		// Token: 0x0400364B RID: 13899
		Consumer,
		// Token: 0x0400364C RID: 13900
		ConsumerToggle,
		// Token: 0x0400364D RID: 13901
		Trigger,
		// Token: 0x0400364E RID: 13902
		Timer,
		// Token: 0x0400364F RID: 13903
		Generator,
		// Token: 0x04003650 RID: 13904
		SolarPanel,
		// Token: 0x04003651 RID: 13905
		BatteryBank,
		// Token: 0x04003652 RID: 13906
		RangedTrap,
		// Token: 0x04003653 RID: 13907
		ElectricWireRelay,
		// Token: 0x04003654 RID: 13908
		TripWireRelay,
		// Token: 0x04003655 RID: 13909
		PressurePlate
	}
}

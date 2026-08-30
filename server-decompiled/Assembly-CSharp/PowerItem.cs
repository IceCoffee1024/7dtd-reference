using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000901 RID: 2305
public class PowerItem
{
	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x060042AC RID: 17068 RVA: 0x001A07E1 File Offset: 0x0019E9E1
	public virtual bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x001A07E9 File Offset: 0x0019E9E9
	public PowerItem()
	{
		this.Children = new List<PowerItem>();
	}

	// Token: 0x060042AE RID: 17070 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanParent(PowerItem newParent)
	{
		return true;
	}

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x060042AF RID: 17071 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual int InputCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x060042B0 RID: 17072 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Consumer;
		}
	}

	// Token: 0x060042B1 RID: 17073 RVA: 0x001A080E File Offset: 0x0019EA0E
	public virtual void AddTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == null)
		{
			this.TileEntity = tileEntityPowered;
			this.TileEntity.CreateWireDataFromPowerItem();
		}
		this.TileEntity.MarkWireDirty();
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x001A0835 File Offset: 0x0019EA35
	public void RemoveTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == tileEntityPowered)
		{
			this.TileEntity = null;
		}
	}

	// Token: 0x060042B3 RID: 17075 RVA: 0x001A0847 File Offset: 0x0019EA47
	public virtual PowerItem GetRoot()
	{
		if (this.Parent != null)
		{
			return this.Parent.GetRoot();
		}
		return this;
	}

	// Token: 0x060042B4 RID: 17076 RVA: 0x001A0860 File Offset: 0x0019EA60
	public virtual void read(BinaryReader _br, byte _version)
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

	// Token: 0x060042B5 RID: 17077 RVA: 0x001A08EC File Offset: 0x0019EAEC
	public void RemoveSelfFromParent()
	{
		PowerManager.Instance.RemoveParent(this);
	}

	// Token: 0x060042B6 RID: 17078 RVA: 0x001A08FC File Offset: 0x0019EAFC
	public virtual void write(BinaryWriter _bw)
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

	// Token: 0x060042B7 RID: 17079 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool PowerChildren()
	{
		return true;
	}

	// Token: 0x060042B8 RID: 17080 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void IsPoweredChanged(bool newPowered)
	{
	}

	// Token: 0x060042B9 RID: 17081 RVA: 0x001A099C File Offset: 0x0019EB9C
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

	// Token: 0x060042BA RID: 17082 RVA: 0x001A0A30 File Offset: 0x0019EC30
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

	// Token: 0x060042BB RID: 17083 RVA: 0x001A0A84 File Offset: 0x0019EC84
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

	// Token: 0x060042BC RID: 17084 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePowerUpdate(bool isOn)
	{
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x001A0ACC File Offset: 0x0019ECCC
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

	// Token: 0x060042BE RID: 17086 RVA: 0x001A0B20 File Offset: 0x0019ED20
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

	// Token: 0x060042BF RID: 17087 RVA: 0x001A0BAC File Offset: 0x0019EDAC
	public virtual void SetValuesFromBlock()
	{
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x001A0C00 File Offset: 0x0019EE00
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

	// Token: 0x060042C1 RID: 17089 RVA: 0x001A0C4C File Offset: 0x0019EE4C
	public void SendHasLocalChangesToRoot()
	{
		this.hasChangesLocal = true;
		for (PowerItem powerItem = this.Parent; powerItem != null; powerItem = powerItem.Parent)
		{
			powerItem.hasChangesLocal = true;
		}
	}

	// Token: 0x040035AA RID: 13738
	public PowerItem Parent;

	// Token: 0x040035AB RID: 13739
	public Vector3i Position;

	// Token: 0x040035AC RID: 13740
	public PowerItem Root;

	// Token: 0x040035AD RID: 13741
	public ushort Depth = ushort.MaxValue;

	// Token: 0x040035AE RID: 13742
	public ushort BlockID;

	// Token: 0x040035AF RID: 13743
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool hasChangesLocal;

	// Token: 0x040035B0 RID: 13744
	public ushort RequiredPower = 5;

	// Token: 0x040035B1 RID: 13745
	public List<PowerItem> Children;

	// Token: 0x040035B2 RID: 13746
	public TileEntityPowered TileEntity;

	// Token: 0x040035B3 RID: 13747
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPowered;

	// Token: 0x02000902 RID: 2306
	public enum PowerItemTypes
	{
		// Token: 0x040035B5 RID: 13749
		None,
		// Token: 0x040035B6 RID: 13750
		Consumer,
		// Token: 0x040035B7 RID: 13751
		ConsumerToggle,
		// Token: 0x040035B8 RID: 13752
		Trigger,
		// Token: 0x040035B9 RID: 13753
		Timer,
		// Token: 0x040035BA RID: 13754
		Generator,
		// Token: 0x040035BB RID: 13755
		SolarPanel,
		// Token: 0x040035BC RID: 13756
		BatteryBank,
		// Token: 0x040035BD RID: 13757
		RangedTrap,
		// Token: 0x040035BE RID: 13758
		ElectricWireRelay,
		// Token: 0x040035BF RID: 13759
		TripWireRelay,
		// Token: 0x040035C0 RID: 13760
		PressurePlate
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200090D RID: 2317
public class PowerItem
{
	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06004311 RID: 17169 RVA: 0x001A28B9 File Offset: 0x001A0AB9
	public virtual bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x001A28C1 File Offset: 0x001A0AC1
	public PowerItem()
	{
		this.Children = new List<PowerItem>();
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool CanParent(PowerItem newParent)
	{
		return true;
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06004314 RID: 17172 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual int InputCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06004315 RID: 17173 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Consumer;
		}
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x001A28E6 File Offset: 0x001A0AE6
	public virtual void AddTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == null)
		{
			this.TileEntity = tileEntityPowered;
			this.TileEntity.CreateWireDataFromPowerItem();
		}
		this.TileEntity.MarkWireDirty();
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x001A290D File Offset: 0x001A0B0D
	public void RemoveTileEntity(TileEntityPowered tileEntityPowered)
	{
		if (this.TileEntity == tileEntityPowered)
		{
			this.TileEntity = null;
		}
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x001A291F File Offset: 0x001A0B1F
	public virtual PowerItem GetRoot()
	{
		if (this.Parent != null)
		{
			return this.Parent.GetRoot();
		}
		return this;
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x001A2938 File Offset: 0x001A0B38
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

	// Token: 0x0600431A RID: 17178 RVA: 0x001A29C4 File Offset: 0x001A0BC4
	public void RemoveSelfFromParent()
	{
		PowerManager.Instance.RemoveParent(this);
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x001A29D4 File Offset: 0x001A0BD4
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

	// Token: 0x0600431C RID: 17180 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool PowerChildren()
	{
		return true;
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void IsPoweredChanged(bool newPowered)
	{
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x001A2A74 File Offset: 0x001A0C74
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

	// Token: 0x0600431F RID: 17183 RVA: 0x001A2B08 File Offset: 0x001A0D08
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

	// Token: 0x06004320 RID: 17184 RVA: 0x001A2B5C File Offset: 0x001A0D5C
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

	// Token: 0x06004321 RID: 17185 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePowerUpdate(bool isOn)
	{
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x001A2BA4 File Offset: 0x001A0DA4
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

	// Token: 0x06004323 RID: 17187 RVA: 0x001A2BF8 File Offset: 0x001A0DF8
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

	// Token: 0x06004324 RID: 17188 RVA: 0x001A2C84 File Offset: 0x001A0E84
	public virtual void SetValuesFromBlock()
	{
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x06004325 RID: 17189 RVA: 0x001A2CD8 File Offset: 0x001A0ED8
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

	// Token: 0x06004326 RID: 17190 RVA: 0x001A2D24 File Offset: 0x001A0F24
	public void SendHasLocalChangesToRoot()
	{
		this.hasChangesLocal = true;
		for (PowerItem powerItem = this.Parent; powerItem != null; powerItem = powerItem.Parent)
		{
			powerItem.hasChangesLocal = true;
		}
	}

	// Token: 0x0400362B RID: 13867
	public PowerItem Parent;

	// Token: 0x0400362C RID: 13868
	public Vector3i Position;

	// Token: 0x0400362D RID: 13869
	public PowerItem Root;

	// Token: 0x0400362E RID: 13870
	public ushort Depth = ushort.MaxValue;

	// Token: 0x0400362F RID: 13871
	public ushort BlockID;

	// Token: 0x04003630 RID: 13872
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool hasChangesLocal;

	// Token: 0x04003631 RID: 13873
	public ushort RequiredPower = 5;

	// Token: 0x04003632 RID: 13874
	public List<PowerItem> Children;

	// Token: 0x04003633 RID: 13875
	public TileEntityPowered TileEntity;

	// Token: 0x04003634 RID: 13876
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPowered;

	// Token: 0x0200090E RID: 2318
	public enum PowerItemTypes
	{
		// Token: 0x04003636 RID: 13878
		None,
		// Token: 0x04003637 RID: 13879
		Consumer,
		// Token: 0x04003638 RID: 13880
		ConsumerToggle,
		// Token: 0x04003639 RID: 13881
		Trigger,
		// Token: 0x0400363A RID: 13882
		Timer,
		// Token: 0x0400363B RID: 13883
		Generator,
		// Token: 0x0400363C RID: 13884
		SolarPanel,
		// Token: 0x0400363D RID: 13885
		BatteryBank,
		// Token: 0x0400363E RID: 13886
		RangedTrap,
		// Token: 0x0400363F RID: 13887
		ElectricWireRelay,
		// Token: 0x04003640 RID: 13888
		TripWireRelay,
		// Token: 0x04003641 RID: 13889
		PressurePlate
	}
}

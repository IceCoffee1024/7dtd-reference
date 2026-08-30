using System;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x02000913 RID: 2323
public class PowerSource : PowerItem
{
	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06004356 RID: 17238 RVA: 0x001A3AD0 File Offset: 0x001A1CD0
	// (set) Token: 0x06004357 RID: 17239 RVA: 0x001A3AD8 File Offset: 0x001A1CD8
	public bool IsOn
	{
		get
		{
			return this.isOn;
		}
		set
		{
			if (this.isOn != value)
			{
				base.SendHasLocalChangesToRoot();
				this.isOn = value;
				this.HandleOnOffSound();
				if (!this.isOn)
				{
					this.HandleDisconnect();
				}
				this.LastPowerUsed = 0;
				if (this.TileEntity != null)
				{
					this.TileEntity.Activate(this.isOn);
				}
			}
		}
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x001A3B30 File Offset: 0x001A1D30
	public PowerSource()
	{
		this.Stacks = new ItemStack[6];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06004359 RID: 17241 RVA: 0x001A3AD0 File Offset: 0x001A1CD0
	public override bool IsPowered
	{
		get
		{
			return this.isOn;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x0600435A RID: 17242 RVA: 0x00032147 File Offset: 0x00030347
	public virtual string OnSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x0600435B RID: 17243 RVA: 0x00032147 File Offset: 0x00030347
	public virtual string OffSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x001A3B7A File Offset: 0x001A1D7A
	public void Refresh()
	{
		if (this.TileEntity != null)
		{
			this.TileEntity.Activate(this.isOn);
		}
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanParent(PowerItem newParent)
	{
		return false;
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x001A3B98 File Offset: 0x001A1D98
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.LastCurrentPower = (this.CurrentPower = _br.ReadUInt16());
		this.IsOn = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.hasChangesLocal = true;
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x001A3BE1 File Offset: 0x001A1DE1
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentPower);
		_bw.Write(this.IsOn);
		GameUtils.WriteItemStack(_bw, this.Stacks);
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x001A3C10 File Offset: 0x001A1E10
	public virtual void Update()
	{
		this.HandleSendPower();
		if (this.hasChangesLocal)
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerUpdate(this.IsOn);
			}
			this.hasChangesLocal = false;
		}
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x001A3C60 File Offset: 0x001A1E60
	public virtual void HandleSendPower()
	{
		if (this.IsOn)
		{
			if (this.CurrentPower < this.MaxPower)
			{
				this.TickPowerGeneration();
			}
			else if (this.CurrentPower > this.MaxPower)
			{
				this.CurrentPower = this.MaxPower;
			}
			if (this.ShouldAutoTurnOff())
			{
				this.CurrentPower = 0;
				this.IsOn = false;
			}
			if (this.hasChangesLocal)
			{
				this.LastPowerUsed = 0;
				ushort num = (ushort)Mathf.Min((int)this.MaxOutput, (int)this.CurrentPower);
				ushort num2 = num;
				World world = GameManager.Instance.World;
				for (int i = 0; i < this.Children.Count; i++)
				{
					num = num2;
					this.Children[i].HandlePowerReceived(ref num2);
					this.LastPowerUsed += num - num2;
				}
			}
			this.CurrentPower -= this.LastPowerUsed;
		}
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ShouldAutoTurnOff()
	{
		return false;
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TickPowerGeneration()
	{
	}

	// Token: 0x06004364 RID: 17252 RVA: 0x001A3D40 File Offset: 0x001A1F40
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("OutputPerStack"))
		{
			this.OutputPerStack = ushort.Parse(block.Properties.Values["OutputPerStack"]);
		}
		this.RequiredPower = (this.MaxPower = (this.MaxOutput = this.OutputPerStack * this.SlotCount));
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x001A3DBD File Offset: 0x001A1FBD
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
		this.RefreshPowerStats();
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x001A3DCC File Offset: 0x001A1FCC
	public bool TryAddItemToSlot(ItemClass itemClass, ItemStack itemStack)
	{
		if (!this.IsOn)
		{
			for (int i = 0; i < this.Stacks.Length; i++)
			{
				if (this.Stacks[i].IsEmpty())
				{
					this.Stacks[i] = itemStack;
					this.RefreshPowerStats();
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x001A3E18 File Offset: 0x001A2018
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RefreshPowerStats()
	{
		this.SlotCount = 0;
		this.MaxOutput = 0;
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			if (!this.Stacks[i].IsEmpty())
			{
				this.MaxOutput += (ushort)((float)this.OutputPerStack * Mathf.Lerp(0.5f, 1f, (float)this.Stacks[i].itemValue.Quality / 6f));
				this.SlotCount += 1;
			}
		}
		this.MaxOutput = (ushort)((float)this.MaxOutput * PowerSource.PowerOutputModifier);
		if (this.BlockID == 0 && this.TileEntity != null)
		{
			this.BlockID = (ushort)GameManager.Instance.World.GetBlock(this.TileEntity.ToWorldPos()).type;
			this.SetValuesFromBlock();
		}
		if (this.MaxPower == 0)
		{
			this.MaxPower = this.MaxOutput;
		}
		if (this.RequiredPower == 0)
		{
			this.RequiredPower = this.MaxOutput;
		}
	}

	// Token: 0x06004368 RID: 17256 RVA: 0x001A3F1E File Offset: 0x001A211E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleOnOffSound()
	{
		if (GameManager.Instance.GameHasStarted)
		{
			Manager.BroadcastPlay(this.Position.ToVector3(), this.isOn ? this.OnSound : this.OffSound, 0f);
		}
	}

	// Token: 0x0400365E RID: 13918
	public ushort OutputPerStack;

	// Token: 0x0400365F RID: 13919
	public ushort SlotCount;

	// Token: 0x04003660 RID: 13920
	public ushort MaxOutput;

	// Token: 0x04003661 RID: 13921
	public ushort MaxPower = 60000;

	// Token: 0x04003662 RID: 13922
	public ushort LastPowerUsed;

	// Token: 0x04003663 RID: 13923
	public ushort CurrentPower;

	// Token: 0x04003664 RID: 13924
	public ushort LastCurrentPower;

	// Token: 0x04003665 RID: 13925
	public ItemStack[] Stacks;

	// Token: 0x04003666 RID: 13926
	public static float PowerOutputModifier = 1f;

	// Token: 0x04003667 RID: 13927
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOn;
}

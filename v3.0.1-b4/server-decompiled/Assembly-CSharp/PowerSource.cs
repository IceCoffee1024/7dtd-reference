using System;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class PowerSource : PowerItem
{
	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x060042F1 RID: 17137 RVA: 0x001A19F8 File Offset: 0x0019FBF8
	// (set) Token: 0x060042F2 RID: 17138 RVA: 0x001A1A00 File Offset: 0x0019FC00
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

	// Token: 0x060042F3 RID: 17139 RVA: 0x001A1A58 File Offset: 0x0019FC58
	public PowerSource()
	{
		this.Stacks = new ItemStack[6];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x060042F4 RID: 17140 RVA: 0x001A19F8 File Offset: 0x0019FBF8
	public override bool IsPowered
	{
		get
		{
			return this.isOn;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x060042F5 RID: 17141 RVA: 0x00031E8B File Offset: 0x0003008B
	public virtual string OnSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x060042F6 RID: 17142 RVA: 0x00031E8B File Offset: 0x0003008B
	public virtual string OffSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x001A1AA2 File Offset: 0x0019FCA2
	public void Refresh()
	{
		if (this.TileEntity != null)
		{
			this.TileEntity.Activate(this.isOn);
		}
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanParent(PowerItem newParent)
	{
		return false;
	}

	// Token: 0x060042F9 RID: 17145 RVA: 0x001A1AC0 File Offset: 0x0019FCC0
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.LastCurrentPower = (this.CurrentPower = _br.ReadUInt16());
		this.IsOn = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.hasChangesLocal = true;
	}

	// Token: 0x060042FA RID: 17146 RVA: 0x001A1B09 File Offset: 0x0019FD09
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentPower);
		_bw.Write(this.IsOn);
		GameUtils.WriteItemStack(_bw, this.Stacks);
	}

	// Token: 0x060042FB RID: 17147 RVA: 0x001A1B38 File Offset: 0x0019FD38
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

	// Token: 0x060042FC RID: 17148 RVA: 0x001A1B88 File Offset: 0x0019FD88
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

	// Token: 0x060042FD RID: 17149 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ShouldAutoTurnOff()
	{
		return false;
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TickPowerGeneration()
	{
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x001A1C68 File Offset: 0x0019FE68
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

	// Token: 0x06004300 RID: 17152 RVA: 0x001A1CE5 File Offset: 0x0019FEE5
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
		this.RefreshPowerStats();
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x001A1CF4 File Offset: 0x0019FEF4
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

	// Token: 0x06004302 RID: 17154 RVA: 0x001A1D40 File Offset: 0x0019FF40
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

	// Token: 0x06004303 RID: 17155 RVA: 0x001A1E46 File Offset: 0x001A0046
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleOnOffSound()
	{
		if (GameManager.Instance.GameHasStarted)
		{
			Manager.BroadcastPlay(this.Position.ToVector3(), this.isOn ? this.OnSound : this.OffSound, 0f);
		}
	}

	// Token: 0x040035DD RID: 13789
	public ushort OutputPerStack;

	// Token: 0x040035DE RID: 13790
	public ushort SlotCount;

	// Token: 0x040035DF RID: 13791
	public ushort MaxOutput;

	// Token: 0x040035E0 RID: 13792
	public ushort MaxPower = 60000;

	// Token: 0x040035E1 RID: 13793
	public ushort LastPowerUsed;

	// Token: 0x040035E2 RID: 13794
	public ushort CurrentPower;

	// Token: 0x040035E3 RID: 13795
	public ushort LastCurrentPower;

	// Token: 0x040035E4 RID: 13796
	public ItemStack[] Stacks;

	// Token: 0x040035E5 RID: 13797
	public static float PowerOutputModifier = 1f;

	// Token: 0x040035E6 RID: 13798
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOn;
}

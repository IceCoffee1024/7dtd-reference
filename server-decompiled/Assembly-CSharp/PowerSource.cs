using System;
using Audio;
using UnityEngine;

// Token: 0x02000918 RID: 2328
public class PowerSource : PowerItem
{
	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x060042E8 RID: 17128 RVA: 0x001A3BD4 File Offset: 0x001A1DD4
	// (set) Token: 0x060042E9 RID: 17129 RVA: 0x001A3BDC File Offset: 0x001A1DDC
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

	// Token: 0x060042EA RID: 17130 RVA: 0x001A3C34 File Offset: 0x001A1E34
	public PowerSource()
	{
		this.Stacks = new ItemStack[6];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x060042EB RID: 17131 RVA: 0x001A3BD4 File Offset: 0x001A1DD4
	public override bool IsPowered
	{
		get
		{
			return this.isOn;
		}
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x060042EC RID: 17132 RVA: 0x0003223F File Offset: 0x0003043F
	public virtual string OnSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x060042ED RID: 17133 RVA: 0x0003223F File Offset: 0x0003043F
	public virtual string OffSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x001A3C7E File Offset: 0x001A1E7E
	public void Refresh()
	{
		if (this.TileEntity != null)
		{
			this.TileEntity.Activate(this.isOn);
		}
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanParent(PowerItem newParent)
	{
		return false;
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x001A3C9C File Offset: 0x001A1E9C
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.LastCurrentPower = (this.CurrentPower = _br.ReadUInt16());
		this.IsOn = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.hasChangesLocal = true;
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x001A3CE5 File Offset: 0x001A1EE5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentPower);
		_bw.Write(this.IsOn);
		GameUtils.WriteItemStack(_bw, this.Stacks);
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x001A3D14 File Offset: 0x001A1F14
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

	// Token: 0x060042F3 RID: 17139 RVA: 0x001A3D64 File Offset: 0x001A1F64
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

	// Token: 0x060042F4 RID: 17140 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ShouldAutoTurnOff()
	{
		return false;
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TickPowerGeneration()
	{
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x001A3E44 File Offset: 0x001A2044
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

	// Token: 0x060042F7 RID: 17143 RVA: 0x001A3EC1 File Offset: 0x001A20C1
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
		this.RefreshPowerStats();
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x001A3ED0 File Offset: 0x001A20D0
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

	// Token: 0x060042F9 RID: 17145 RVA: 0x001A3F1C File Offset: 0x001A211C
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

	// Token: 0x060042FA RID: 17146 RVA: 0x001A4022 File Offset: 0x001A2222
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleOnOffSound()
	{
		if (GameManager.Instance.GameHasStarted)
		{
			Manager.BroadcastPlay(this.Position.ToVector3(), this.isOn ? this.OnSound : this.OffSound, 0f);
		}
	}

	// Token: 0x04003652 RID: 13906
	public ushort OutputPerStack;

	// Token: 0x04003653 RID: 13907
	public ushort SlotCount;

	// Token: 0x04003654 RID: 13908
	public ushort MaxOutput;

	// Token: 0x04003655 RID: 13909
	public ushort MaxPower = 60000;

	// Token: 0x04003656 RID: 13910
	public ushort LastPowerUsed;

	// Token: 0x04003657 RID: 13911
	public ushort CurrentPower;

	// Token: 0x04003658 RID: 13912
	public ushort LastCurrentPower;

	// Token: 0x04003659 RID: 13913
	public ItemStack[] Stacks;

	// Token: 0x0400365A RID: 13914
	public static float PowerOutputModifier = 1f;

	// Token: 0x0400365B RID: 13915
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOn;
}

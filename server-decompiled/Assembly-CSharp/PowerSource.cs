using System;
using Audio;
using UnityEngine;

// Token: 0x02000922 RID: 2338
public class PowerSource : PowerItem
{
	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x0600430B RID: 17163 RVA: 0x001A4C7C File Offset: 0x001A2E7C
	// (set) Token: 0x0600430C RID: 17164 RVA: 0x001A4C84 File Offset: 0x001A2E84
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

	// Token: 0x0600430D RID: 17165 RVA: 0x001A4CDC File Offset: 0x001A2EDC
	public PowerSource()
	{
		this.Stacks = new ItemStack[6];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x0600430E RID: 17166 RVA: 0x001A4C7C File Offset: 0x001A2E7C
	public override bool IsPowered
	{
		get
		{
			return this.isOn;
		}
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x0600430F RID: 17167 RVA: 0x000321FF File Offset: 0x000303FF
	public virtual string OnSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06004310 RID: 17168 RVA: 0x000321FF File Offset: 0x000303FF
	public virtual string OffSound
	{
		get
		{
			return "";
		}
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x001A4D26 File Offset: 0x001A2F26
	public void Refresh()
	{
		if (this.TileEntity != null)
		{
			this.TileEntity.Activate(this.isOn);
		}
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanParent(PowerItem newParent)
	{
		return false;
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x001A4D44 File Offset: 0x001A2F44
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.LastCurrentPower = (this.CurrentPower = _br.ReadUInt16());
		this.IsOn = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.hasChangesLocal = true;
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x001A4D8D File Offset: 0x001A2F8D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.CurrentPower);
		_bw.Write(this.IsOn);
		GameUtils.WriteItemStack(_bw, this.Stacks);
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x001A4DBC File Offset: 0x001A2FBC
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

	// Token: 0x06004316 RID: 17174 RVA: 0x001A4E0C File Offset: 0x001A300C
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

	// Token: 0x06004317 RID: 17175 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ShouldAutoTurnOff()
	{
		return false;
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TickPowerGeneration()
	{
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x001A4EEC File Offset: 0x001A30EC
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

	// Token: 0x0600431A RID: 17178 RVA: 0x001A4F69 File Offset: 0x001A3169
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
		this.RefreshPowerStats();
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x001A4F78 File Offset: 0x001A3178
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

	// Token: 0x0600431C RID: 17180 RVA: 0x001A4FC4 File Offset: 0x001A31C4
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

	// Token: 0x0600431D RID: 17181 RVA: 0x001A50CA File Offset: 0x001A32CA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleOnOffSound()
	{
		if (GameManager.Instance.GameHasStarted)
		{
			Manager.BroadcastPlay(this.Position.ToVector3(), this.isOn ? this.OnSound : this.OffSound, 0f);
		}
	}

	// Token: 0x04003672 RID: 13938
	public ushort OutputPerStack;

	// Token: 0x04003673 RID: 13939
	public ushort SlotCount;

	// Token: 0x04003674 RID: 13940
	public ushort MaxOutput;

	// Token: 0x04003675 RID: 13941
	public ushort MaxPower = 60000;

	// Token: 0x04003676 RID: 13942
	public ushort LastPowerUsed;

	// Token: 0x04003677 RID: 13943
	public ushort CurrentPower;

	// Token: 0x04003678 RID: 13944
	public ushort LastCurrentPower;

	// Token: 0x04003679 RID: 13945
	public ItemStack[] Stacks;

	// Token: 0x0400367A RID: 13946
	public static float PowerOutputModifier = 1f;

	// Token: 0x0400367B RID: 13947
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOn;
}

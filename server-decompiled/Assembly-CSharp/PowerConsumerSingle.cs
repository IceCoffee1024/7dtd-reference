using System;

// Token: 0x0200090A RID: 2314
public class PowerConsumerSingle : PowerItem
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x060042FC RID: 17148 RVA: 0x0002F168 File Offset: 0x0002D368
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x060042FD RID: 17149 RVA: 0x001A2504 File Offset: 0x001A0704
	public override void HandlePowerUpdate(bool isOn)
	{
		bool isPowered = this.isPowered;
		if (isPowered && this.lastActivate != isPowered && this.TileEntity != null)
		{
			this.TileEntity.ActivateOnce();
		}
		this.lastActivate = isPowered;
		if (this.PowerChildren())
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerUpdate(isOn);
			}
		}
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x001A2570 File Offset: 0x001A0770
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x04003626 RID: 13862
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActivate;
}

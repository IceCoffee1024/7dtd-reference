using System;

// Token: 0x020008FE RID: 2302
public class PowerConsumerSingle : PowerItem
{
	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06004297 RID: 17047 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x001A042C File Offset: 0x0019E62C
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

	// Token: 0x06004299 RID: 17049 RVA: 0x001A0498 File Offset: 0x0019E698
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x040035A5 RID: 13733
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActivate;
}

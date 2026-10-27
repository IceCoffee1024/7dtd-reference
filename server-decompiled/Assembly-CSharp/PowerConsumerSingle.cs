using System;

// Token: 0x02000919 RID: 2329
public class PowerConsumerSingle : PowerItem
{
	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x060042B1 RID: 17073 RVA: 0x0002F260 File Offset: 0x0002D460
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x001A36B0 File Offset: 0x001A18B0
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

	// Token: 0x060042B3 RID: 17075 RVA: 0x001A371C File Offset: 0x001A191C
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x0400363A RID: 13882
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActivate;
}

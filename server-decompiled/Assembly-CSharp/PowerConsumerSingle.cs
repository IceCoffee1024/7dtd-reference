using System;

// Token: 0x0200090F RID: 2319
public class PowerConsumerSingle : PowerItem
{
	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x0600428E RID: 17038 RVA: 0x0002F260 File Offset: 0x0002D460
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x001A2608 File Offset: 0x001A0808
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

	// Token: 0x06004290 RID: 17040 RVA: 0x001A2674 File Offset: 0x001A0874
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = ushort.Parse(block.Properties.Values["RequiredPower"]);
		}
	}

	// Token: 0x0400361A RID: 13850
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActivate;
}

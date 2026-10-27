using System;

// Token: 0x0200092B RID: 2347
public class PowerElectricWireRelay : PowerConsumer
{
	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x0600434E RID: 17230 RVA: 0x00132DA5 File Offset: 0x00130FA5
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ElectricWireRelay;
		}
	}
}

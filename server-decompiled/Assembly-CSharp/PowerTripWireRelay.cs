using System;

// Token: 0x0200091B RID: 2331
public class PowerTripWireRelay : PowerTrigger
{
	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06004397 RID: 17303 RVA: 0x000811EE File Offset: 0x0007F3EE
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.TripWireRelay;
		}
	}
}

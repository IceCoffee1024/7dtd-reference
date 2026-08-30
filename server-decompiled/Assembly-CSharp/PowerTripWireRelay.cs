using System;

// Token: 0x0200090F RID: 2319
public class PowerTripWireRelay : PowerTrigger
{
	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06004332 RID: 17202 RVA: 0x00080B3A File Offset: 0x0007ED3A
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.TripWireRelay;
		}
	}
}

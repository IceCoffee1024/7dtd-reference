using System;

// Token: 0x0200092A RID: 2346
public class PowerTripWireRelay : PowerTrigger
{
	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x0600434C RID: 17228 RVA: 0x000816C6 File Offset: 0x0007F8C6
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.TripWireRelay;
		}
	}
}

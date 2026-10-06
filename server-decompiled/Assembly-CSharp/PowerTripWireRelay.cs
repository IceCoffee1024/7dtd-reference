using System;

// Token: 0x02000920 RID: 2336
public class PowerTripWireRelay : PowerTrigger
{
	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x06004329 RID: 17193 RVA: 0x000814A2 File Offset: 0x0007F6A2
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.TripWireRelay;
		}
	}
}

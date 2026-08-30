using System;

// Token: 0x02000910 RID: 2320
public class PowerElectricWireRelay : PowerConsumer
{
	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06004334 RID: 17204 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ElectricWireRelay;
		}
	}
}

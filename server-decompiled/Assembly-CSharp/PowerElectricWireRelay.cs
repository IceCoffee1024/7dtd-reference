using System;

// Token: 0x0200091C RID: 2332
public class PowerElectricWireRelay : PowerConsumer
{
	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x06004399 RID: 17305 RVA: 0x00131719 File Offset: 0x0012F919
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ElectricWireRelay;
		}
	}
}

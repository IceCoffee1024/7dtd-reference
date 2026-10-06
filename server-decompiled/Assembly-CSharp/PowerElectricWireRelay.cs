using System;

// Token: 0x02000921 RID: 2337
public class PowerElectricWireRelay : PowerConsumer
{
	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x0600432B RID: 17195 RVA: 0x001321B1 File Offset: 0x001303B1
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ElectricWireRelay;
		}
	}
}

using System;
using UnityEngine.Scripting;

// Token: 0x020010C5 RID: 4293
[Preserve]
public class XUiC_VehiclePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x060088A6 RID: 34982 RVA: 0x00081AB0 File Offset: 0x0007FCB0
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x060088A7 RID: 34983 RVA: 0x0034517D File Offset: 0x0034337D
	// (set) Token: 0x060088A8 RID: 34984 RVA: 0x00345185 File Offset: 0x00343385
	public Vehicle CurrentVehicle { get; set; }

	// Token: 0x060088A9 RID: 34985 RVA: 0x0034518E File Offset: 0x0034338E
	public override void Init()
	{
		base.Init();
	}
}

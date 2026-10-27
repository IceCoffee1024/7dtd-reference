using System;
using UnityEngine.Scripting;

// Token: 0x020010D5 RID: 4309
[Preserve]
public class XUiC_VehiclePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x060088F9 RID: 35065 RVA: 0x00081CD4 File Offset: 0x0007FED4
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x060088FA RID: 35066 RVA: 0x00347201 File Offset: 0x00345401
	// (set) Token: 0x060088FB RID: 35067 RVA: 0x00347209 File Offset: 0x00345409
	public Vehicle CurrentVehicle { get; set; }

	// Token: 0x060088FC RID: 35068 RVA: 0x00347212 File Offset: 0x00345412
	public override void Init()
	{
		base.Init();
	}
}

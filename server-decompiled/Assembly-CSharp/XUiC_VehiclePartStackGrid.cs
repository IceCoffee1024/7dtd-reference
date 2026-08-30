using System;
using UnityEngine.Scripting;

// Token: 0x020010B4 RID: 4276
[Preserve]
public class XUiC_VehiclePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x06008857 RID: 34903 RVA: 0x00080719 File Offset: 0x0007E919
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x06008858 RID: 34904 RVA: 0x00341369 File Offset: 0x0033F569
	// (set) Token: 0x06008859 RID: 34905 RVA: 0x00341371 File Offset: 0x0033F571
	public Vehicle CurrentVehicle { get; set; }

	// Token: 0x0600885A RID: 34906 RVA: 0x0034137A File Offset: 0x0033F57A
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600885B RID: 34907 RVA: 0x00341382 File Offset: 0x0033F582
	public void SetMods(ItemValue[] mods)
	{
		base.SetParts(mods);
	}
}

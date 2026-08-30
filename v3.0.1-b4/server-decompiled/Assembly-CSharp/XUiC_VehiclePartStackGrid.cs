using System;
using UnityEngine.Scripting;

// Token: 0x0200109B RID: 4251
[Preserve]
public class XUiC_VehiclePartStackGrid : XUiC_ItemPartStackGrid
{
	// Token: 0x17000FAE RID: 4014
	// (get) Token: 0x06008765 RID: 34661 RVA: 0x00080065 File Offset: 0x0007E265
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x06008766 RID: 34662 RVA: 0x0033CB41 File Offset: 0x0033AD41
	// (set) Token: 0x06008767 RID: 34663 RVA: 0x0033CB49 File Offset: 0x0033AD49
	public Vehicle CurrentVehicle { get; set; }

	// Token: 0x06008768 RID: 34664 RVA: 0x0033CB52 File Offset: 0x0033AD52
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x06008769 RID: 34665 RVA: 0x0033CB5A File Offset: 0x0033AD5A
	public void SetMods(ItemValue[] mods)
	{
		base.SetParts(mods);
	}
}

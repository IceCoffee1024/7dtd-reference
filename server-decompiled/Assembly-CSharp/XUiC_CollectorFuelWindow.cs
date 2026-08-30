using System;
using UnityEngine.Scripting;

// Token: 0x02000EF8 RID: 3832
[Preserve]
public class XUiC_CollectorFuelWindow : XUiController
{
	// Token: 0x06007826 RID: 30758 RVA: 0x002D455B File Offset: 0x002D275B
	public override void Init()
	{
		base.Init();
		this.fuelGrid = base.GetChildByType<XUiC_CollectorFuelGrid>();
		if (this.fuelGrid != null)
		{
			this.fuelGrid.Owner = this;
		}
	}

	// Token: 0x06007827 RID: 30759 RVA: 0x002D4583 File Offset: 0x002D2783
	public override void OnOpen()
	{
		base.OnOpen();
		base.RefreshBindings();
	}

	// Token: 0x06007828 RID: 30760 RVA: 0x002D4591 File Offset: 0x002D2791
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
			this.xui.RecenterWindowGroup(this.windowGroup, false);
		}
	}

	// Token: 0x06007829 RID: 30761 RVA: 0x002D45C4 File Offset: 0x002D27C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "fuel_row_count")
		{
			_value = ((this.fuelGrid == null) ? "0" : this.fuelGrid.GetHeight().ToString());
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x040058DC RID: 22748
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid fuelGrid;
}

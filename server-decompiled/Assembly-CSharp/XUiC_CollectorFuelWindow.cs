using System;
using UnityEngine.Scripting;

// Token: 0x02000F12 RID: 3858
[Preserve]
public class XUiC_CollectorFuelWindow : XUiController
{
	// Token: 0x06007847 RID: 30791 RVA: 0x002D8E47 File Offset: 0x002D7047
	public override void Init()
	{
		base.Init();
		this.fuelGrid = base.GetChildByType<XUiC_CollectorFuelGrid>();
		if (this.fuelGrid != null)
		{
			this.fuelGrid.Owner = this;
		}
	}

	// Token: 0x06007848 RID: 30792 RVA: 0x002D8E6F File Offset: 0x002D706F
	public override void OnOpen()
	{
		base.OnOpen();
		base.RefreshBindings();
	}

	// Token: 0x06007849 RID: 30793 RVA: 0x002D8E7D File Offset: 0x002D707D
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

	// Token: 0x0600784A RID: 30794 RVA: 0x002D8EB0 File Offset: 0x002D70B0
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

	// Token: 0x04005939 RID: 22841
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid fuelGrid;
}

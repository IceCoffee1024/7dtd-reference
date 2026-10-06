using System;
using UnityEngine.Scripting;

// Token: 0x02000F07 RID: 3847
[Preserve]
public class XUiC_CollectorFuelWindow : XUiController
{
	// Token: 0x06007821 RID: 30753 RVA: 0x002D78EF File Offset: 0x002D5AEF
	public override void Init()
	{
		base.Init();
		this.fuelGrid = base.GetChildByType<XUiC_CollectorFuelGrid>();
		if (this.fuelGrid != null)
		{
			this.fuelGrid.Owner = this;
		}
	}

	// Token: 0x06007822 RID: 30754 RVA: 0x002D7917 File Offset: 0x002D5B17
	public override void OnOpen()
	{
		base.OnOpen();
		base.RefreshBindings();
	}

	// Token: 0x06007823 RID: 30755 RVA: 0x002D7925 File Offset: 0x002D5B25
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

	// Token: 0x06007824 RID: 30756 RVA: 0x002D7958 File Offset: 0x002D5B58
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

	// Token: 0x04005914 RID: 22804
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid fuelGrid;
}

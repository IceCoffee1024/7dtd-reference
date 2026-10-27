using System;
using UnityEngine.Scripting;

// Token: 0x02000F46 RID: 3910
[Preserve]
public class XUiC_AssembleDroneWindow : XUiC_AssembleWindow
{
	// Token: 0x06007A10 RID: 31248 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x17000E64 RID: 3684
	// (set) Token: 0x06007A11 RID: 31249 RVA: 0x002E2F36 File Offset: 0x002E1136
	public override ItemStack ItemStack
	{
		set
		{
			this.group.CurrentVehicleEntity.LoadMods();
			base.ItemStack = value;
		}
	}

	// Token: 0x06007A12 RID: 31250 RVA: 0x002E2F4F File Offset: 0x002E114F
	public override void OnChanged()
	{
		this.group.OnItemChanged(this.ItemStack);
		this.IsDirty = true;
	}

	// Token: 0x06007A13 RID: 31251 RVA: 0x002E2F6C File Offset: 0x002E116C
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("btnRepair");
		if (childById != null)
		{
			this.btnRepair_Background = (XUiV_Button)childById.GetChildById("clickable").ViewComponent;
			this.btnRepair_Background.Controller.OnPress += this.BtnRepair_OnPress;
		}
	}

	// Token: 0x06007A14 RID: 31252 RVA: 0x002E2FC8 File Offset: 0x002E11C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRepair_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DroneWindowGroup xuiC_DroneWindowGroup = this.group;
		EntityDrone entityDrone = ((xuiC_DroneWindowGroup != null) ? xuiC_DroneWindowGroup.CurrentVehicleEntity : null);
		if (entityDrone)
		{
			entityDrone.DoRepairAction(this.xui.playerUI);
			base.RefreshBindings();
			this.IsDirty = true;
		}
	}

	// Token: 0x06007A15 RID: 31253 RVA: 0x002E3010 File Offset: 0x002E1210
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		XUiC_DroneWindowGroup xuiC_DroneWindowGroup = this.group;
		EntityDrone entityDrone = ((xuiC_DroneWindowGroup != null) ? xuiC_DroneWindowGroup.CurrentVehicleEntity : null);
		if (bindingName == "vehicledurability")
		{
			value = ((entityDrone != null) ? this.vehicleDurabilityFormatter.Format(entityDrone.Health, entityDrone.GetMaxHealth()) : "");
			return true;
		}
		if (!(bindingName == "vehicledurabilitytitle"))
		{
			return false;
		}
		value = Localization.Get("xuiDurability", false, null);
		return true;
	}

	// Token: 0x04005A71 RID: 23153
	public XUiC_DroneWindowGroup group;

	// Token: 0x04005A72 RID: 23154
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRepair_Background;

	// Token: 0x04005A73 RID: 23155
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> vehicleDurabilityFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => string.Format("{0}/{1}", _i1, _i2));
}

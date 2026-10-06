using System;
using UnityEngine.Scripting;

// Token: 0x02000F37 RID: 3895
[Preserve]
public class XUiC_AssembleDroneWindow : XUiC_AssembleWindow
{
	// Token: 0x060079CD RID: 31181 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x17000E5A RID: 3674
	// (set) Token: 0x060079CE RID: 31182 RVA: 0x002E1646 File Offset: 0x002DF846
	public override ItemStack ItemStack
	{
		set
		{
			this.group.CurrentVehicleEntity.LoadMods();
			base.ItemStack = value;
		}
	}

	// Token: 0x060079CF RID: 31183 RVA: 0x002E165F File Offset: 0x002DF85F
	public override void OnChanged()
	{
		this.group.OnItemChanged(this.ItemStack);
		this.IsDirty = true;
	}

	// Token: 0x060079D0 RID: 31184 RVA: 0x002E167C File Offset: 0x002DF87C
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

	// Token: 0x060079D1 RID: 31185 RVA: 0x002E16D8 File Offset: 0x002DF8D8
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

	// Token: 0x060079D2 RID: 31186 RVA: 0x002E1720 File Offset: 0x002DF920
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

	// Token: 0x04005A3E RID: 23102
	public XUiC_DroneWindowGroup group;

	// Token: 0x04005A3F RID: 23103
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRepair_Background;

	// Token: 0x04005A40 RID: 23104
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> vehicleDurabilityFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => string.Format("{0}/{1}", _i1, _i2));
}

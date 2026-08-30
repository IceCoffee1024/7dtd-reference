using System;
using UnityEngine.Scripting;

// Token: 0x02000F28 RID: 3880
[Preserve]
public class XUiC_AssembleDroneWindow : XUiC_AssembleWindow
{
	// Token: 0x060079D0 RID: 31184 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x17000E3D RID: 3645
	// (set) Token: 0x060079D1 RID: 31185 RVA: 0x002DE27B File Offset: 0x002DC47B
	public override ItemStack ItemStack
	{
		set
		{
			this.group.CurrentVehicleEntity.LoadMods();
			base.ItemStack = value;
		}
	}

	// Token: 0x060079D2 RID: 31186 RVA: 0x002DE294 File Offset: 0x002DC494
	public override void OnChanged()
	{
		this.group.OnItemChanged(this.ItemStack);
		this.IsDirty = true;
	}

	// Token: 0x060079D3 RID: 31187 RVA: 0x002DE2B0 File Offset: 0x002DC4B0
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

	// Token: 0x060079D4 RID: 31188 RVA: 0x002DE30C File Offset: 0x002DC50C
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

	// Token: 0x060079D5 RID: 31189 RVA: 0x002DE354 File Offset: 0x002DC554
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

	// Token: 0x04005A06 RID: 23046
	public XUiC_DroneWindowGroup group;

	// Token: 0x04005A07 RID: 23047
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRepair_Background;

	// Token: 0x04005A08 RID: 23048
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> vehicleDurabilityFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => string.Format("{0}/{1}", _i1, _i2));
}

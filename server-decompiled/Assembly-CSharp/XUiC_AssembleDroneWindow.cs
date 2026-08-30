using System;
using UnityEngine.Scripting;

// Token: 0x02000F11 RID: 3857
[Preserve]
public class XUiC_AssembleDroneWindow : XUiC_AssembleWindow
{
	// Token: 0x06007917 RID: 30999 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x17000E22 RID: 3618
	// (set) Token: 0x06007918 RID: 31000 RVA: 0x002DA2C8 File Offset: 0x002D84C8
	public override ItemStack ItemStack
	{
		set
		{
			this.group.CurrentVehicleEntity.LoadMods();
			base.ItemStack = value;
		}
	}

	// Token: 0x06007919 RID: 31001 RVA: 0x002DA2E1 File Offset: 0x002D84E1
	public override void OnChanged()
	{
		this.group.OnItemChanged(this.ItemStack);
		this.IsDirty = true;
	}

	// Token: 0x0600791A RID: 31002 RVA: 0x002DA2FC File Offset: 0x002D84FC
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

	// Token: 0x0600791B RID: 31003 RVA: 0x002DA358 File Offset: 0x002D8558
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

	// Token: 0x0600791C RID: 31004 RVA: 0x002DA3A0 File Offset: 0x002D85A0
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

	// Token: 0x04005953 RID: 22867
	public XUiC_DroneWindowGroup group;

	// Token: 0x04005954 RID: 22868
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRepair_Background;

	// Token: 0x04005955 RID: 22869
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> vehicleDurabilityFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => string.Format("{0}/{1}", _i1, _i2));
}

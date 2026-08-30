using System;
using UnityEngine.Scripting;

// Token: 0x020002F3 RID: 755
[Preserve]
public class DialogRequirementDroneState : BaseDialogRequirement
{
	// Token: 0x17000271 RID: 625
	// (get) Token: 0x0600159B RID: 5531 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.DroneState;
		}
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x00080B6C File Offset: 0x0007ED6C
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		EntityDrone entityDrone = talkingTo as EntityDrone;
		if (entityDrone)
		{
			EntityDrone.Orders orders;
			if (Enum.TryParse<EntityDrone.Orders>(base.Value, out orders))
			{
				return entityDrone.OrderState == orders;
			}
			EntityDrone.AllyHealMode allyHealMode;
			if (Enum.TryParse<EntityDrone.AllyHealMode>(base.Value, out allyHealMode) && entityDrone.IsHealModAttached)
			{
				return entityDrone.HealAllyMode == allyHealMode;
			}
			if (entityDrone.IsFlashlightAttached)
			{
				if (base.Value.Equals("LightOff"))
				{
					return !entityDrone.IsFlashlightOn;
				}
				if (base.Value.Equals("LightOn"))
				{
					return entityDrone.IsFlashlightOn;
				}
			}
			bool flag = entityDrone.TargetCanBeHealed(player);
			if (base.Value.Equals("Heal") && flag)
			{
				return flag;
			}
		}
		return false;
	}
}

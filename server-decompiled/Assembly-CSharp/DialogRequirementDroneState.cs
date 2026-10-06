using System;
using UnityEngine.Scripting;

// Token: 0x020002FA RID: 762
[Preserve]
public class DialogRequirementDroneState : BaseDialogRequirement
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x060015AB RID: 5547 RVA: 0x000814D1 File Offset: 0x0007F6D1
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.DroneState;
		}
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x000814D4 File Offset: 0x0007F6D4
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

using System;
using System.Xml.Linq;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020006F2 RID: 1778
[Preserve]
public class MinEventActionAltSounds : MinEventActionTargetedBase
{
	// Token: 0x06003663 RID: 13923 RVA: 0x001670D0 File Offset: 0x001652D0
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] is EntityPlayerLocal)
			{
				EntityVehicle entityVehicle = this.targets[i].AttachedToEntity as EntityVehicle;
				if (entityVehicle != null)
				{
					entityVehicle.vehicle.FireEvent(Vehicle.Event.Stop);
					Manager.Instance.bUseAltSounds = this.enabled;
					entityVehicle.vehicle.FireEvent(Vehicle.Event.Start);
				}
				else
				{
					Manager.Instance.bUseAltSounds = this.enabled;
				}
			}
		}
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x00167160 File Offset: 0x00165360
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002BB5 RID: 11189
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;
}

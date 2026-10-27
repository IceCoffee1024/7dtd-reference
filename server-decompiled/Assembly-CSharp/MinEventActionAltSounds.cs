using System;
using System.Xml.Linq;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000706 RID: 1798
[Preserve]
public class MinEventActionAltSounds : MinEventActionTargetedBase
{
	// Token: 0x06003713 RID: 14099 RVA: 0x0016ABA4 File Offset: 0x00168DA4
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

	// Token: 0x06003714 RID: 14100 RVA: 0x0016AC34 File Offset: 0x00168E34
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002C3E RID: 11326
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;
}

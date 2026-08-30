using System;
using System.Xml.Linq;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020006FC RID: 1788
[Preserve]
public class MinEventActionAltSounds : MinEventActionTargetedBase
{
	// Token: 0x060036B9 RID: 14009 RVA: 0x001692A8 File Offset: 0x001674A8
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

	// Token: 0x060036BA RID: 14010 RVA: 0x00169338 File Offset: 0x00167538
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002C29 RID: 11305
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;
}

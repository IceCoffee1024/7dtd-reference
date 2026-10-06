using System;
using System.Xml.Linq;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020006FE RID: 1790
[Preserve]
public class MinEventActionAltSounds : MinEventActionTargetedBase
{
	// Token: 0x060036FC RID: 14076 RVA: 0x00169C0C File Offset: 0x00167E0C
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

	// Token: 0x060036FD RID: 14077 RVA: 0x00169C9C File Offset: 0x00167E9C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002C22 RID: 11298
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;
}

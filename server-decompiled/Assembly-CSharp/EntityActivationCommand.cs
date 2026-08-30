using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003EE RID: 1006
[Preserve]
public struct EntityActivationCommand
{
	// Token: 0x06001E78 RID: 7800 RVA: 0x000B8F70 File Offset: 0x000B7170
	public EntityActivationCommand(string _commandId, string _icon, string _eventName = null, string _customCommandText = null)
	{
		this.commandId = _commandId;
		this.icon = _icon;
		this.eventName = _eventName;
		this.iconColor = Color.white;
		this.activateTime = -1f;
		this.enabled = true;
		this.commandText = this.commandId;
		if (!string.IsNullOrEmpty(_customCommandText))
		{
			this.commandText = _customCommandText;
		}
		this.commandText = Localization.Get("entitycommand_" + this.commandText, false, null);
	}

	// Token: 0x04001465 RID: 5221
	public string commandId;

	// Token: 0x04001466 RID: 5222
	public string icon;

	// Token: 0x04001467 RID: 5223
	public Color iconColor;

	// Token: 0x04001468 RID: 5224
	public bool enabled;

	// Token: 0x04001469 RID: 5225
	public string eventName;

	// Token: 0x0400146A RID: 5226
	public float activateTime;

	// Token: 0x0400146B RID: 5227
	public string commandText;
}

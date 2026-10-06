using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003F5 RID: 1013
[Preserve]
public struct EntityActivationCommand
{
	// Token: 0x06001E8C RID: 7820 RVA: 0x000B98B0 File Offset: 0x000B7AB0
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

	// Token: 0x04001473 RID: 5235
	public string commandId;

	// Token: 0x04001474 RID: 5236
	public string icon;

	// Token: 0x04001475 RID: 5237
	public Color iconColor;

	// Token: 0x04001476 RID: 5238
	public bool enabled;

	// Token: 0x04001477 RID: 5239
	public string eventName;

	// Token: 0x04001478 RID: 5240
	public float activateTime;

	// Token: 0x04001479 RID: 5241
	public string commandText;
}

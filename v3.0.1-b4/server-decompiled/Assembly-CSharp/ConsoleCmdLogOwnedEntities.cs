using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200023A RID: 570
[Preserve]
public class ConsoleCmdLogOwnedEntities : ConsoleCmdAbstract
{
	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600112F RID: 4399 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06001130 RID: 4400 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001131 RID: 4401 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x0006D0E8 File Offset: 0x0006B2E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "playerOwnedEntities", "poe" };
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x0006D100 File Offset: 0x0006B300
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists player owned entities.";
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x0006D107 File Offset: 0x0006B307
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return this.buildHelpLog();
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x0006D110 File Offset: 0x0006B310
	[PublicizedFrom(EAccessModifier.Private)]
	public string buildHelpLog()
	{
		return "Player Owned Entities help:" + Environment.NewLine + "[Client Commands]" + Environment.NewLine + "poe - Lists owned entities for the local player" + Environment.NewLine + "[Server/Host Commands]" + Environment.NewLine + "poe - Lists owned entities for all players online" + Environment.NewLine + "[PlayerName] - Lists the owned entities for passed online player (ex. poe [PlayerName])" + Environment.NewLine;
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x0006D178 File Offset: 0x0006B378
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				if (primaryPlayer)
				{
					Log.Out("Client Player Owned Entities" + Environment.NewLine + this.logPlayerOwnedEntities(primaryPlayer.entityId));
					return;
				}
			}
			else
			{
				string text = "Game Player Owned Entities" + Environment.NewLine;
				Log.Out(this.logPlayerOwnedEntities(text));
			}
			return;
		}
		if (_params[0].ContainsCaseInsensitive("help"))
		{
			Log.Out(this.buildHelpLog());
			return;
		}
		if (_params.Count > 0)
		{
			List<EntityPlayer> list = GameManager.Instance.World.Players.list;
			for (int i = 0; i < list.Count; i++)
			{
				EntityPlayer entityPlayer = list[i];
				if (entityPlayer.EntityName.ContainsCaseInsensitive(_params[0]))
				{
					Log.Out(entityPlayer.EntityName + " Owned Entities" + Environment.NewLine + this.logPlayerOwnedEntities(entityPlayer.entityId));
					return;
				}
			}
			return;
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x0006D290 File Offset: 0x0006B490
	[PublicizedFrom(EAccessModifier.Private)]
	public string logPlayerOwnedEntities(string header)
	{
		string text = header + Environment.NewLine;
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in GameManager.Instance.GetPersistentPlayerList().Players)
		{
			text += this.logPlayerOwnedEntities(keyValuePair.Value.EntityId);
		}
		return text;
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x0006D308 File Offset: 0x0006B508
	[PublicizedFrom(EAccessModifier.Private)]
	public string logPlayerOwnedEntities(int entityId)
	{
		string text = string.Empty;
		EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(entityId) as EntityPlayer;
		if (entityPlayer)
		{
			List<OwnedEntityData> ownedEntities = entityPlayer.ownedEntities;
			string text2 = string.Empty;
			for (int i = 0; i < ownedEntities.Count; i++)
			{
				OwnedEntityData ownedEntityData = ownedEntities[i];
				text2 = text2 + string.Format("entityId: {0}, classId: {1}, lastKnownPosition: {2}", ownedEntityData.Id, EntityClass.GetEntityClassName(ownedEntityData.ClassId), ownedEntityData.hasLastKnownPosition ? ownedEntityData.LastKnownPosition.ToString() : "none") + Environment.NewLine;
			}
			text += string.Format("[{0} - ({1})]" + Environment.NewLine + "{2}", entityPlayer.EntityName, ownedEntities.Count, text2);
		}
		return text;
	}
}

using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200023E RID: 574
[Preserve]
public class ConsoleCmdLogOwnedEntities : ConsoleCmdAbstract
{
	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06001141 RID: 4417 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001142 RID: 4418 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06001143 RID: 4419 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x0006D744 File Offset: 0x0006B944
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "playerOwnedEntities", "poe" };
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x0006D75C File Offset: 0x0006B95C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists player owned entities.";
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x0006D763 File Offset: 0x0006B963
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return this.buildHelpLog();
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x0006D76C File Offset: 0x0006B96C
	[PublicizedFrom(EAccessModifier.Private)]
	public string buildHelpLog()
	{
		return "Player Owned Entities help:" + Environment.NewLine + "[Client Commands]" + Environment.NewLine + "poe - Lists owned entities for the local player" + Environment.NewLine + "[Server/Host Commands]" + Environment.NewLine + "poe - Lists owned entities for all players online" + Environment.NewLine + "[PlayerName] - Lists the owned entities for passed online player (ex. poe [PlayerName])" + Environment.NewLine;
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x0006D7D4 File Offset: 0x0006B9D4
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

	// Token: 0x06001149 RID: 4425 RVA: 0x0006D8EC File Offset: 0x0006BAEC
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

	// Token: 0x0600114A RID: 4426 RVA: 0x0006D964 File Offset: 0x0006BB64
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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001E9 RID: 489
[Preserve]
public class ConsoleCmdAIDirectorSpawnSupplyCrate : ConsoleCmdAbstract
{
	// Token: 0x06000EEF RID: 3823 RVA: 0x000616D2 File Offset: 0x0005F8D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnsupplycrate" };
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x000616E4 File Offset: 0x0005F8E4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		World world = GameManager.Instance.World;
		EntityPlayer entityPlayer;
		if (_senderInfo.IsLocalGame)
		{
			entityPlayer = world.GetPrimaryPlayer();
		}
		else
		{
			if (_senderInfo.RemoteClientInfo == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command RemoteClientInfo null");
				return;
			}
			entityPlayer = world.Players.dict[_senderInfo.RemoteClientInfo.entityId];
		}
		Vector3 position = entityPlayer.position;
		position.y += 8f;
		Entity entity = EntityFactory.CreateEntity(EntityClass.FromString("sc_General"), position, new Vector3(0f, world.GetGameRandom().RandomFloat * 360f, 0f));
		world.SpawnEntityInWorld(entity);
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x00061790 File Offset: 0x0005F990
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawns a supply crate where the player is";
	}
}

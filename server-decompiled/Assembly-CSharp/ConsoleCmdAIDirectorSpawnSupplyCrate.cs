using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001E4 RID: 484
[Preserve]
public class ConsoleCmdAIDirectorSpawnSupplyCrate : ConsoleCmdAbstract
{
	// Token: 0x06000EED RID: 3821 RVA: 0x00060ED6 File Offset: 0x0005F0D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spawnsupplycrate" };
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x00060EE8 File Offset: 0x0005F0E8
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

	// Token: 0x06000EEF RID: 3823 RVA: 0x00060F94 File Offset: 0x0005F194
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Spawns a supply crate where the player is";
	}
}

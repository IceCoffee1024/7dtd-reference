using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002AB RID: 683
[Preserve]
public abstract class ConsoleCmdTeleportsAbs : ConsoleCmdAbstract
{
	// Token: 0x060013C6 RID: 5062 RVA: 0x000785C0 File Offset: 0x000767C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool TryParseV3i(List<string> _params, int _startIndex, out Vector3i _result)
	{
		_result = default(Vector3i);
		if (!int.TryParse(_params[_startIndex], out _result.x))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("x argument is not a valid integer");
			return false;
		}
		if (!int.TryParse(_params[_startIndex + 1], out _result.y))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("y argument is not a valid integer");
			return false;
		}
		if (!int.TryParse(_params[_startIndex + 2], out _result.z))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("z argument is not a valid integer");
			return false;
		}
		return true;
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x00078648 File Offset: 0x00076848
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3? TryParseViewDirection(string _viewDirectionString)
	{
		if (_viewDirectionString.EqualsCaseInsensitive("n") || _viewDirectionString.EqualsCaseInsensitive("north"))
		{
			return new Vector3?(new Vector3(0f, 0f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("ne") || _viewDirectionString.EqualsCaseInsensitive("northeast"))
		{
			return new Vector3?(new Vector3(0f, 45f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("e") || _viewDirectionString.EqualsCaseInsensitive("east"))
		{
			return new Vector3?(new Vector3(0f, 90f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("se") || _viewDirectionString.EqualsCaseInsensitive("southeast"))
		{
			return new Vector3?(new Vector3(0f, 135f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("s") || _viewDirectionString.EqualsCaseInsensitive("south"))
		{
			return new Vector3?(new Vector3(0f, 180f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("sw") || _viewDirectionString.EqualsCaseInsensitive("southwest"))
		{
			return new Vector3?(new Vector3(0f, 225f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("w") || _viewDirectionString.EqualsCaseInsensitive("west"))
		{
			return new Vector3?(new Vector3(0f, 270f, 0f));
		}
		if (_viewDirectionString.EqualsCaseInsensitive("nw") || _viewDirectionString.EqualsCaseInsensitive("northwest"))
		{
			return new Vector3?(new Vector3(0f, 315f, 0f));
		}
		return null;
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x00078800 File Offset: 0x00076A00
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityPlayer GetExecutingEntityPlayer(CommandSenderInfo _senderInfo)
	{
		if (_senderInfo.IsLocalGame)
		{
			return GameManager.Instance.World.GetPrimaryPlayer();
		}
		if (_senderInfo.RemoteClientInfo != null)
		{
			return GameManager.Instance.World.Players.dict[_senderInfo.RemoteClientInfo.entityId];
		}
		return null;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x00078854 File Offset: 0x00076A54
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool TryGetDestinationFromPlayer(string _targetPlayerString, out Vector3 _destination)
	{
		_destination = default(Vector3);
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_targetPlayerString, true, false);
		EntityPlayer entityPlayer;
		if (clientInfo == null)
		{
			if (GameManager.IsDedicatedServer || !ConsoleHelper.ParamIsLocalPlayer(_targetPlayerString, true, false))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Target playername or entity/userid id not found.");
				return false;
			}
			entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		}
		else
		{
			entityPlayer = GameManager.Instance.World.Players.dict[clientInfo.entityId];
		}
		_destination = entityPlayer.GetPosition();
		_destination.y += 1f;
		_destination.z += 1f;
		return true;
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x000788F4 File Offset: 0x00076AF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ExecuteTeleport(ClientInfo _player, Vector3 _destPos, Vector3? _viewDirection)
	{
		int num = ((_player != null) ? _player.entityId : ((GameManager.Instance.GetPersistentLocalPlayer() != null) ? GameManager.Instance.GetPersistentLocalPlayer().EntityId : (-1)));
		LockManager.Instance.ForceUnlockByPlayer(num);
		NetPackageTeleportPlayer netPackageTeleportPlayer = NetPackageManager.GetPackage<NetPackageTeleportPlayer>().Setup(_destPos, _viewDirection, false);
		if (_player == null)
		{
			netPackageTeleportPlayer.ProcessPackage(GameManager.Instance.World, GameManager.Instance);
			return;
		}
		_player.SendPackage(netPackageTeleportPlayer);
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x0005FE37 File Offset: 0x0005E037
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleCmdTeleportsAbs()
	{
	}
}

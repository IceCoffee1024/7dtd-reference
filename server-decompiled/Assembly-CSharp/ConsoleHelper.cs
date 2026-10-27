using System;
using System.Collections.ObjectModel;
using Platform;

// Token: 0x020002CE RID: 718
public static class ConsoleHelper
{
	// Token: 0x06001494 RID: 5268 RVA: 0x0007CB5C File Offset: 0x0007AD5C
	public static bool ParseParamBool(string _param, bool _invalidStringsAsFalse = false)
	{
		if (_param.EqualsCaseInsensitive("y") || _param.EqualsCaseInsensitive("yes") || _param.EqualsCaseInsensitive("true") || _param.EqualsCaseInsensitive("on") || _param.EqualsCaseInsensitive("1"))
		{
			return true;
		}
		if (_param.EqualsCaseInsensitive("n") || _param.EqualsCaseInsensitive("no") || _param.EqualsCaseInsensitive("false") || _param.EqualsCaseInsensitive("off") || _param.EqualsCaseInsensitive("0"))
		{
			return false;
		}
		if (_invalidStringsAsFalse)
		{
			return false;
		}
		throw new ArgumentException("Not a bool value");
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x0007CC00 File Offset: 0x0007AE00
	public static Entity ParseParamEntityIdToEntity(string _param, bool _playersOnly = true)
	{
		int num;
		if (!int.TryParse(_param, out num))
		{
			return null;
		}
		if (_playersOnly)
		{
			if (GameManager.Instance.World.Players.dict.ContainsKey(num))
			{
				return GameManager.Instance.World.Players.dict[num];
			}
		}
		else if (GameManager.Instance.World.Entities.dict.ContainsKey(num))
		{
			return GameManager.Instance.World.Entities.dict[num];
		}
		return null;
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x0007CC8C File Offset: 0x0007AE8C
	public static ClientInfo ParseParamEntityIdToClientInfo(string _param)
	{
		int num;
		if (!int.TryParse(_param, out num))
		{
			return null;
		}
		return SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(num);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0007CCB8 File Offset: 0x0007AEB8
	public static bool ParseParamSteamGroupIdValid(string _param)
	{
		ulong num;
		return _param.Length == 18 && ulong.TryParse(_param, out num);
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0007CCDC File Offset: 0x0007AEDC
	public static bool ParamIsLocalPlayer(string _param, bool _ignoreCase = true, bool _ignoreBlanks = false)
	{
		if (GameManager.IsDedicatedServer)
		{
			return false;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		if (PlatformUserIdentifierAbs.TryFromCombinedString(_param, out platformUserIdentifierAbs) && platformUserIdentifierAbs.Equals(PlatformManager.InternalLocalUserIdentifier))
		{
			return true;
		}
		int num;
		if (int.TryParse(_param, out num) && primaryPlayer.entityId == num)
		{
			return true;
		}
		if (_ignoreBlanks)
		{
			_param = _param.Replace(" ", "");
		}
		string text = primaryPlayer.EntityName ?? string.Empty;
		if (_ignoreBlanks)
		{
			text = text.Replace(" ", "");
		}
		return string.Equals(text, _param, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0007CD7C File Offset: 0x0007AF7C
	public static ClientInfo ParseParamPlayerName(string _param, bool _ignoreCase = true, bool _ignoreBlanks = false)
	{
		return SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.GetForPlayerName(_param, _ignoreCase, _ignoreBlanks);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x0007CD90 File Offset: 0x0007AF90
	public static PlatformUserIdentifierAbs ParseParamUserId(string _param)
	{
		return PlatformUserIdentifierAbs.FromCombinedString(_param, true);
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0007CD99 File Offset: 0x0007AF99
	public static ClientInfo ParseParamIdOrName(string _param, bool _ignoreCase = true, bool _ignoreBlanks = false)
	{
		return SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.GetForNameOrId(_param, _ignoreCase, _ignoreBlanks);
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x0007CDB0 File Offset: 0x0007AFB0
	public static int ParseParamPartialNameOrId(string _param, out PlatformUserIdentifierAbs _id, out ClientInfo _cInfo, bool _sendError = true)
	{
		_id = null;
		_cInfo = null;
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_param, true, false);
		if (clientInfo != null)
		{
			_id = clientInfo.InternalId;
			_cInfo = clientInfo;
			return 1;
		}
		if (PlatformUserIdentifierAbs.TryFromCombinedString(_param, out _id))
		{
			return 1;
		}
		ClientInfo clientInfo2 = null;
		int num = 0;
		ReadOnlyCollection<ClientInfo> list = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List;
		for (int i = 0; i < list.Count; i++)
		{
			ClientInfo clientInfo3 = list[i];
			if (clientInfo3.playerName.ContainsCaseInsensitive(_param))
			{
				num++;
				clientInfo2 = clientInfo3;
			}
		}
		if (num == 1)
		{
			_id = clientInfo2.InternalId;
			_cInfo = clientInfo2;
		}
		else
		{
			_id = null;
			_cInfo = null;
			if (_sendError)
			{
				if (num == 0)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _param + "\" is not a valid entity id, player name or user id.");
				}
				else if (num > 1)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _param + "\" matches multiple player names.");
				}
			}
		}
		return num;
	}
}

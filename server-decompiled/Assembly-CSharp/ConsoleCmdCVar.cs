using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000201 RID: 513
[Preserve]
public class ConsoleCmdCVar : ConsoleCmdAbstract
{
	// Token: 0x06000FA4 RID: 4004 RVA: 0x000658A1 File Offset: 0x00063AA1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "cvar" };
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x000658B1 File Offset: 0x00063AB1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Commands to set, get, track or list CVars.";
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x000658B8 File Offset: 0x00063AB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usages of the commands. Add '-p <playerId>' to any command to apply that command to a remote player. \ncvar get <cvarName>\ncvar set <cvarName> <floatValue>\ncvar track <cvarName> <true|false>\ncvar list <searchFilter>";
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x000658C0 File Offset: 0x00063AC0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Not enough arguments supplied.");
			return;
		}
		int num = GameManager.Instance.World.GetPrimaryPlayerId();
		if (_senderInfo.RemoteClientInfo != null)
		{
			num = _senderInfo.RemoteClientInfo.entityId;
		}
		string text = _params[0].ToLowerInvariant();
		if (text == "get")
		{
			this.ExecuteGet(_params, num);
			return;
		}
		if (text == "set")
		{
			this.ExecuteSet(_params, num);
			return;
		}
		if (text == "track")
		{
			this.ExecuteTrack(_params, num);
			return;
		}
		if (!(text == "list"))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Valid command not supplied. Use 'get', 'set', 'track' or 'list'.");
			return;
		}
		this.ExecuteList(_params, num);
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x00065980 File Offset: 0x00063B80
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteGet(List<string> _params, int _playerId)
	{
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Not enough arguments supplied.");
			return;
		}
		if (_params.Count >= 4)
		{
			_playerId = this.GetPlayerId(_params);
		}
		EntityPlayer player = this.GetPlayer(_playerId);
		if (player == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Could not find player matching ID {0}.", _playerId));
			return;
		}
		string text = _params[1];
		bool flag = player.Buffs.HasCustomVar(text);
		float num = (flag ? player.Buffs.GetCustomVar(text) : 0f);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Player {0} has cvar {1}: {2}. Value: {3}", new object[] { player.EntityName, text, flag, num }));
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x00065A4C File Offset: 0x00063C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteSet(List<string> _params, int _playerId)
	{
		if (_params.Count < 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Not enough arguments supplied.");
			return;
		}
		float num;
		if (!float.TryParse(_params[2], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Could not parse '" + _params[2] + "' into float.");
			return;
		}
		int num2 = _playerId;
		if (_params.Count >= 5)
		{
			_playerId = this.GetPlayerId(_params);
		}
		EntityPlayer player = this.GetPlayer(_playerId);
		if (player == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Could not find player matching ID {0}.", _playerId));
			return;
		}
		string text = _params[1];
		float customVar = player.Buffs.GetCustomVar(text);
		if (_playerId == num2)
		{
			player.Buffs.SetCustomVar(text, num, true, CVarOperation.set, false);
		}
		else
		{
			player.Buffs.SetCustomVarNetwork(text, num, CVarOperation.set);
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Player {0} cvar {1} set from {2} to {3}", new object[] { player.EntityName, text, customVar, num }));
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x00065B58 File Offset: 0x00063D58
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteTrack(List<string> _params, int _playerId)
	{
		if (_params.Count < 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Not enough arguments supplied.");
			return;
		}
		bool flag;
		if (!bool.TryParse(_params[2], out flag))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Could not parse '" + _params[2] + "' into bool.");
			return;
		}
		if (_params.Count >= 5)
		{
			_playerId = this.GetPlayerId(_params);
		}
		EntityPlayer player = this.GetPlayer(_playerId);
		if (player == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Could not find player matching ID {0}.", _playerId));
			return;
		}
		string text = _params[1];
		player.Buffs.TrackCustomVar(text, flag);
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x00065C04 File Offset: 0x00063E04
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteList(List<string> _params, int _playerId)
	{
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Not enough arguments supplied.");
			return;
		}
		if (_params.Count >= 5)
		{
			_playerId = this.GetPlayerId(_params);
		}
		EntityPlayer player = this.GetPlayer(_playerId);
		if (player == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Could not find player matching ID {0}.", _playerId));
			return;
		}
		string text = _params[1];
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[] { "Listing CVars for ", player.EntityName, " which contain \"", text, "\"." }));
		foreach (KeyValuePair<string, float> keyValuePair in player.Buffs.EnumerateCustomVars(text, false))
		{
			string text2;
			float num;
			keyValuePair.Deconstruct(out text2, out num);
			string text3 = text2;
			float num2 = num;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("\t{0} : {1}", text3, num2));
		}
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x00065D1C File Offset: 0x00063F1C
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetPlayerId(List<string> _params)
	{
		int i = 2;
		while (i < _params.Count - 1)
		{
			if (_params[i].EqualsCaseInsensitive("-p"))
			{
				int num;
				if (int.TryParse(_params[i + 1], out num))
				{
					return num;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		return -1;
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x00065D64 File Offset: 0x00063F64
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer GetPlayer(int _playerId)
	{
		if (_playerId == -1)
		{
			return null;
		}
		EntityPlayer entityPlayer;
		if (!GameManager.Instance.World.Players.dict.TryGetValue(_playerId, out entityPlayer))
		{
			return null;
		}
		return entityPlayer;
	}
}

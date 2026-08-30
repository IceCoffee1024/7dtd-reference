using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Scripting;
using Webserver.Permissions;

namespace Webserver.Commands
{
	// Token: 0x02001AE7 RID: 6887
	[Preserve]
	public class WebTokens : ConsoleCmdAbstract
	{
		// Token: 0x0600CE8A RID: 52874 RVA: 0x004B5A24 File Offset: 0x004B3C24
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "webtokens" };
		}

		// Token: 0x0600CE8B RID: 52875 RVA: 0x004B5A34 File Offset: 0x004B3C34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Manage web tokens";
		}

		// Token: 0x0600CE8C RID: 52876 RVA: 0x004B5A3B File Offset: 0x004B3C3B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "Set/get webtoken permission levels. A level of 0 is maximum permission.\nUsage:\n   webtokens add <tokenname> <tokensecret> <level>\n   webtokens remove <tokenname>\n   webtokens list";
		}

		// Token: 0x0600CE8D RID: 52877 RVA: 0x004B5A44 File Offset: 0x004B3C44
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count < 1)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No sub command given.");
				return;
			}
			if (_params[0].EqualsCaseInsensitive("add"))
			{
				this.ExecuteAdd(_params);
				return;
			}
			if (_params[0].EqualsCaseInsensitive("remove"))
			{
				this.ExecuteRemove(_params);
				return;
			}
			if (_params[0].EqualsCaseInsensitive("list"))
			{
				this.ExecuteList();
				return;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid sub command \"" + _params[0] + "\".");
		}

		// Token: 0x0600CE8E RID: 52878 RVA: 0x004B5ADC File Offset: 0x004B3CDC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExecuteAdd(List<string> _params)
		{
			if (_params.Count != 4)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Wrong number of arguments, expected 4, found {0}.", _params.Count));
				return;
			}
			if (string.IsNullOrEmpty(_params[1]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'tokenname' is empty.");
				return;
			}
			if (!WebTokens.validNameTokenMatcher.IsMatch(_params[1]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'tokenname' may only contain characters (A-Z, a-z), digits (0-9) and underscores (_).");
				return;
			}
			if (string.IsNullOrEmpty(_params[2]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'tokensecret' is empty.");
				return;
			}
			if (!WebTokens.validNameTokenMatcher.IsMatch(_params[2]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'tokensecret' may only contain characters (A-Z, a-z), digits (0-9) and underscores (_).");
				return;
			}
			int num;
			if (!int.TryParse(_params[3], out num))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'level' is not a valid integer.");
				return;
			}
			AdminApiTokens.Instance.AddToken(_params[1], _params[2], num);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Web API token with name={0} and secret={1} added with permission level of {2}.", _params[1], _params[2], num));
		}

		// Token: 0x0600CE8F RID: 52879 RVA: 0x004B5BF8 File Offset: 0x004B3DF8
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExecuteRemove(List<string> _params)
		{
			if (_params.Count != 2)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Wrong number of arguments, expected 2, found {0}.", _params.Count));
				return;
			}
			if (string.IsNullOrEmpty(_params[1]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'tokenname' is empty.");
				return;
			}
			if (!WebTokens.validNameTokenMatcher.IsMatch(_params[1]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 'tokenname' may only contain characters (A-Z, a-z), digits (0-9) and underscores (_).");
				return;
			}
			AdminApiTokens.Instance.RemoveToken(_params[1]);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(_params[1] + " removed from web API token permissions list.");
		}

		// Token: 0x0600CE90 RID: 52880 RVA: 0x004B5C9C File Offset: 0x004B3E9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ExecuteList()
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Defined web API token permissions:");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("  Level: Name / Secret");
			foreach (KeyValuePair<string, AdminApiTokens.ApiToken> keyValuePair in AdminApiTokens.Instance.GetTokens())
			{
				string text;
				AdminApiTokens.ApiToken apiToken;
				keyValuePair.Deconstruct(out text, out apiToken);
				AdminApiTokens.ApiToken apiToken2 = apiToken;
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("  {0,5}: {1} / {2}", apiToken2.PermissionLevel, apiToken2.Name, apiToken2.Secret));
			}
		}

		// Token: 0x04009CC1 RID: 40129
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex validNameTokenMatcher = new Regex("^\\w+$");
	}
}

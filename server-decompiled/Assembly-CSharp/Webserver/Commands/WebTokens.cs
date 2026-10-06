using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Scripting;
using Webserver.Permissions;

namespace Webserver.Commands
{
	// Token: 0x02001B3A RID: 6970
	[Preserve]
	public class WebTokens : ConsoleCmdAbstract
	{
		// Token: 0x0600D16B RID: 53611 RVA: 0x004C02E4 File Offset: 0x004BE4E4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "webtokens" };
		}

		// Token: 0x0600D16C RID: 53612 RVA: 0x004C02F4 File Offset: 0x004BE4F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Manage web tokens";
		}

		// Token: 0x0600D16D RID: 53613 RVA: 0x004C02FB File Offset: 0x004BE4FB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "Set/get webtoken permission levels. A level of 0 is maximum permission.\nUsage:\n   webtokens add <tokenname> <tokensecret> <level>\n   webtokens remove <tokenname>\n   webtokens list";
		}

		// Token: 0x0600D16E RID: 53614 RVA: 0x004C0304 File Offset: 0x004BE504
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

		// Token: 0x0600D16F RID: 53615 RVA: 0x004C039C File Offset: 0x004BE59C
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

		// Token: 0x0600D170 RID: 53616 RVA: 0x004C04B8 File Offset: 0x004BE6B8
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

		// Token: 0x0600D171 RID: 53617 RVA: 0x004C055C File Offset: 0x004BE75C
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

		// Token: 0x04009EB8 RID: 40632
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex validNameTokenMatcher = new Regex("^\\w+$");
	}
}

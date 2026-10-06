using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000259 RID: 601
[Preserve]
public class ConsoleCmdPermissionsAllowed : ConsoleCmdAbstract
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060011CA RID: 4554 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060011CB RID: 4555 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0006FC8B File Offset: 0x0006DE8B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "permissionsallowed", "pallowed", "pa" };
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x0006FCAB File Offset: 0x0006DEAB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Apply a mask to permissions for testing purposes (respects the existing conditions though).";
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x0006FCB4 File Offset: 0x0006DEB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		string text = string.Join<EUserPerms>("|", EnumUtils.Values<EUserPerms>());
		return string.Join('\n', new string[]
		{
			"pa i[nfo] - Prints info about the current permissions.",
			"pa g[rant] <" + text + "> - Adds the given permissions to the current debug permissions mask (still respects existing permissions though).",
			"pa rev[oke] <" + text + "> - Removes the given permissions from the current debug permissions mask.",
			"pa res[olve] <" + text + "> <true|false> - Attempt to resolve the specified permissions. True allows prompting the user for input, otherwise it is a silent resolution."
		});
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x0006FD20 File Offset: 0x0006DF20
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		string[] array = _params.ToArray();
		if (_params.Count > 0)
		{
			string text = _params[0].ToLowerInvariant();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			ConsoleCmdPermissionsAllowed.ExecuteSubCommand executeSubCommand;
			if (num <= 2935168099U)
			{
				if (num <= 804546073U)
				{
					if (num != 263456517U)
					{
						if (num != 804546073U)
						{
							goto IL_0152;
						}
						if (!(text == "res"))
						{
							goto IL_0152;
						}
					}
					else
					{
						if (!(text == "info"))
						{
							goto IL_0152;
						}
						goto IL_0124;
					}
				}
				else if (num != 854878930U)
				{
					if (num != 2935168099U)
					{
						goto IL_0152;
					}
					if (!(text == "resolve"))
					{
						goto IL_0152;
					}
				}
				else
				{
					if (!(text == "rev"))
					{
						goto IL_0152;
					}
					goto IL_013B;
				}
				executeSubCommand = new ConsoleCmdPermissionsAllowed.ExecuteSubCommand(this.ExecuteResolve);
				goto IL_0154;
			}
			if (num <= 3691399731U)
			{
				if (num != 2975164269U)
				{
					if (num != 3691399731U)
					{
						goto IL_0152;
					}
					if (!(text == "revoke"))
					{
						goto IL_0152;
					}
					goto IL_013B;
				}
				else if (!(text == "grant"))
				{
					goto IL_0152;
				}
			}
			else if (num != 3792446982U)
			{
				if (num != 3960223172U)
				{
					goto IL_0152;
				}
				if (!(text == "i"))
				{
					goto IL_0152;
				}
				goto IL_0124;
			}
			else if (!(text == "g"))
			{
				goto IL_0152;
			}
			executeSubCommand = ConsoleCmdPermissionsAllowed.ExecuteGrant;
			goto IL_0154;
			IL_0124:
			executeSubCommand = new ConsoleCmdPermissionsAllowed.ExecuteSubCommand(ConsoleCmdPermissionsAllowed.ExecuteInfo);
			goto IL_0154;
			IL_013B:
			executeSubCommand = ConsoleCmdPermissionsAllowed.ExecuteRevoke;
			goto IL_0154;
			IL_0152:
			executeSubCommand = null;
			IL_0154:
			ConsoleCmdPermissionsAllowed.ExecuteSubCommand executeSubCommand2 = executeSubCommand;
			if (executeSubCommand2 == null)
			{
				Log.Warning("Unknown sub-command: " + _params[0]);
			}
			else if (executeSubCommand2(RuntimeHelpers.GetSubArray<string>(array, Range.StartAt(1))))
			{
				return;
			}
		}
		Log.Warning(this.GetHelp());
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x0006FEC8 File Offset: 0x0006E0C8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ExecuteInfo(ReadOnlySpan<string> parameters)
	{
		EUserPerms permissions = PlatformManager.NativePlatform.User.Permissions;
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		EUserPerms? euserPerms;
		if (crossplatformPlatform == null)
		{
			euserPerms = null;
		}
		else
		{
			IUserClient user = crossplatformPlatform.User;
			euserPerms = ((user != null) ? new EUserPerms?(user.Permissions) : null);
		}
		EUserPerms? euserPerms2 = euserPerms;
		StringBuilder stringBuilder = new StringBuilder(string.Format("User Native: {0}\nUser Cross: {1}", permissions, ((euserPerms2 != null) ? euserPerms2.GetValueOrDefault().ToString() : null) ?? "N/A"));
		foreach (object obj in Enum.GetValues(typeof(PermissionsManager.PermissionSources)))
		{
			PermissionsManager.PermissionSources permissionSources = (PermissionsManager.PermissionSources)obj;
			stringBuilder.Append(string.Format("\n{0}: {1}", permissionSources, PermissionsManager.GetPermissions(permissionSources)));
		}
		Log.Out(stringBuilder.ToString());
		return true;
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x0006FFE0 File Offset: 0x0006E1E0
	[PublicizedFrom(EAccessModifier.Private)]
	public static EUserPerms MaskModifierGrant(EUserPerms previous, EUserPerms input)
	{
		return previous | input;
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x0006FFE5 File Offset: 0x0006E1E5
	[PublicizedFrom(EAccessModifier.Private)]
	public static EUserPerms MaskModifierRevoke(EUserPerms previous, EUserPerms input)
	{
		return previous & ~input;
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x0006FFEB File Offset: 0x0006E1EB
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConsoleCmdPermissionsAllowed.ExecuteSubCommand CreateExecuteMaskModifier(ConsoleCmdPermissionsAllowed.MaskModifier modifier)
	{
		ConsoleCmdPermissionsAllowed.<>c__DisplayClass16_0 CS$<>8__locals1 = new ConsoleCmdPermissionsAllowed.<>c__DisplayClass16_0();
		CS$<>8__locals1.modifier = modifier;
		return new ConsoleCmdPermissionsAllowed.ExecuteSubCommand(CS$<>8__locals1.<CreateExecuteMaskModifier>g__ExecuteMaskModifier|0);
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00070004 File Offset: 0x0006E204
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe bool ExecuteResolve(ReadOnlySpan<string> parameters)
	{
		if (parameters.Length != 2)
		{
			Log.Warning("Expected a two arguments.");
			return false;
		}
		EUserPerms euserPerms;
		if (!ConsoleCmdPermissionsAllowed.TryParsePermission(*parameters[0], out euserPerms))
		{
			return false;
		}
		bool flag;
		if (!ConsoleCmdPermissionsAllowed.TryParseBoolean(*parameters[1], out flag))
		{
			return false;
		}
		ThreadManager.StartCoroutine(this.ResolveCoroutine(euserPerms, flag));
		return true;
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0007005E File Offset: 0x0006E25E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator ResolveCoroutine(EUserPerms permissionsToResolve, bool shouldPrompt)
	{
		if (this.m_resolveCoroutineRunning)
		{
			Log.Warning("[Perms Resolve] Resolving in progress already.");
			yield break;
		}
		yield return null;
		try
		{
			this.m_resolveCoroutineRunning = true;
			Log.Out(string.Format("[Perms Resolve] Resolving Permissions '{0}'.", permissionsToResolve));
			yield return PermissionsManager.ResolvePermissions(permissionsToResolve, shouldPrompt, null);
		}
		finally
		{
			this.m_resolveCoroutineRunning = false;
		}
		yield break;
		yield break;
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x0007007C File Offset: 0x0006E27C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryParseBoolean(string input, out bool result)
	{
		string text = input.ToLowerInvariant();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 1303515621U)
		{
			if (num <= 873244444U)
			{
				if (num != 184981848U)
				{
					if (num != 873244444U)
					{
						goto IL_0102;
					}
					if (!(text == "1"))
					{
						goto IL_0102;
					}
				}
				else
				{
					if (!(text == "false"))
					{
						goto IL_0102;
					}
					goto IL_00FD;
				}
			}
			else if (num != 890022063U)
			{
				if (num != 1303515621U)
				{
					goto IL_0102;
				}
				if (!(text == "true"))
				{
					goto IL_0102;
				}
			}
			else
			{
				if (!(text == "0"))
				{
					goto IL_0102;
				}
				goto IL_00FD;
			}
		}
		else if (num <= 2872740362U)
		{
			if (num != 1630810064U)
			{
				if (num != 2872740362U)
				{
					goto IL_0102;
				}
				if (!(text == "off"))
				{
					goto IL_0102;
				}
				goto IL_00FD;
			}
			else if (!(text == "on"))
			{
				goto IL_0102;
			}
		}
		else if (num != 3809224601U)
		{
			if (num != 4044111267U)
			{
				goto IL_0102;
			}
			if (!(text == "t"))
			{
				goto IL_0102;
			}
		}
		else
		{
			if (!(text == "f"))
			{
				goto IL_0102;
			}
			goto IL_00FD;
		}
		result = true;
		return true;
		IL_00FD:
		result = false;
		return true;
		IL_0102:
		Log.Warning("Expected true/false instead of: " + input);
		result = false;
		return false;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x000701A0 File Offset: 0x0006E3A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryParsePermission(string input, out EUserPerms result)
	{
		if (EnumUtils.TryParse<EUserPerms>(input, out result, true))
		{
			return true;
		}
		EUserPerms? euserPerms = null;
		foreach (ValueTuple<EUserPerms, string> valueTuple in EnumUtils.Values<EUserPerms>().Zip<EUserPerms, string, ValueTuple<EUserPerms, string>>(EnumUtils.Names<EUserPerms>(), (EUserPerms perm, string name) => new ValueTuple<EUserPerms, string>(perm, name)))
		{
			EUserPerms item = valueTuple.Item1;
			if (valueTuple.Item2.StartsWith(input, StringComparison.OrdinalIgnoreCase))
			{
				if (euserPerms != null)
				{
					Log.Warning(string.Format("Input '{0}' is ambiguous between '{1}' and '{2}'.", input, euserPerms.Value, item));
					result = (EUserPerms)0;
					return false;
				}
				euserPerms = new EUserPerms?(item);
			}
		}
		if (euserPerms == null)
		{
			Log.Warning("Input '" + input + "' did not match any permissions.");
			result = (EUserPerms)0;
			return false;
		}
		result = euserPerms.Value;
		return true;
	}

	// Token: 0x04000CF3 RID: 3315
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_resolveCoroutineRunning;

	// Token: 0x04000CF4 RID: 3316
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConsoleCmdPermissionsAllowed.ExecuteSubCommand ExecuteGrant = ConsoleCmdPermissionsAllowed.CreateExecuteMaskModifier(new ConsoleCmdPermissionsAllowed.MaskModifier(ConsoleCmdPermissionsAllowed.MaskModifierGrant));

	// Token: 0x04000CF5 RID: 3317
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConsoleCmdPermissionsAllowed.ExecuteSubCommand ExecuteRevoke = ConsoleCmdPermissionsAllowed.CreateExecuteMaskModifier(new ConsoleCmdPermissionsAllowed.MaskModifier(ConsoleCmdPermissionsAllowed.MaskModifierRevoke));

	// Token: 0x0200025A RID: 602
	// (Invoke) Token: 0x060011DB RID: 4571
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool ExecuteSubCommand(ReadOnlySpan<string> parameters);

	// Token: 0x0200025B RID: 603
	// (Invoke) Token: 0x060011DF RID: 4575
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate EUserPerms MaskModifier(EUserPerms previous, EUserPerms input);
}

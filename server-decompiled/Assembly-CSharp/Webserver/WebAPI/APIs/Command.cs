using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001B2F RID: 6959
	[Preserve]
	public class Command : AbsRestApi
	{
		// Token: 0x0600D13D RID: 53565 RVA: 0x004BC8D4 File Offset: 0x004BAAD4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			int permissionLevel = _context.PermissionLevel;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(Command.jsonCommandsKey);
			jsonWriter.WriteBeginArray();
			if (string.IsNullOrEmpty(requestPath))
			{
				IList<IConsoleCommand> commands = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands();
				for (int i = 0; i < commands.Count; i++)
				{
					IConsoleCommand consoleCommand = commands[i];
					if (i > 0)
					{
						jsonWriter.WriteValueSeparator();
					}
					this.writeCommandJson(ref jsonWriter, consoleCommand, permissionLevel);
				}
			}
			else
			{
				IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(requestPath, false);
				if (command == null)
				{
					jsonWriter.WriteEndArray();
					jsonWriter.WriteEndObject();
					AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.NotFound, null, null, null);
					return;
				}
				this.writeCommandJson(ref jsonWriter, command, permissionLevel);
			}
			jsonWriter.WriteEndArray();
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D13E RID: 53566 RVA: 0x004BC9B0 File Offset: 0x004BABB0
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeCommandJson(ref JsonWriter _writer, IConsoleCommand _command, int _userPermissionLevel)
		{
			_writer.WriteRaw(Command.jsonOverloadsKey);
			_writer.WriteBeginArray();
			string text = string.Empty;
			string[] commands = _command.GetCommands();
			for (int i = 0; i < commands.Length; i++)
			{
				string text2 = commands[i];
				if (i > 0)
				{
					_writer.WriteValueSeparator();
				}
				_writer.WriteString(text2);
				if (text2.Length > text.Length)
				{
					text = text2;
				}
			}
			_writer.WriteEndArray();
			_writer.WriteRaw(Command.jsonCommandKey);
			_writer.WriteString(text);
			_writer.WriteRaw(Command.jsonDescriptionKey);
			_writer.WriteString(_command.GetDescription());
			_writer.WriteRaw(Command.jsonHelpKey);
			_writer.WriteString(_command.GetHelp());
			int commandPermissionLevel = GameManager.Instance.adminTools.Commands.GetCommandPermissionLevel(_command.GetCommands());
			_writer.WriteRaw(Command.jsonAllowedKey);
			_writer.WriteBoolean(_userPermissionLevel <= commandPermissionLevel);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D13F RID: 53567 RVA: 0x004BCA94 File Offset: 0x004BAC94
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string text;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "command", out text))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_COMMAND, null);
				return;
			}
			WebCommandResult.ResultType resultType = WebCommandResult.ResultType.Full;
			string text2;
			if (JsonCommons.TryGetJsonField(_jsonInput, "format", out text2))
			{
				if (text2.EqualsCaseInsensitive("raw"))
				{
					resultType = WebCommandResult.ResultType.Raw;
				}
				else if (text2.EqualsCaseInsensitive("simple"))
				{
					resultType = WebCommandResult.ResultType.ResultOnly;
				}
			}
			int num = text.IndexOf(' ');
			string text3 = ((num > 0) ? text.Substring(0, num) : text);
			string text4 = ((num > 0) ? text.Substring(text3.Length + 1) : "");
			IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(text3, true);
			if (command == null)
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.NotFound, _jsonInputData, EApiErrorCode.UNKNOWN_COMMAND, null);
				return;
			}
			int commandPermissionLevel = GameManager.Instance.adminTools.Commands.GetCommandPermissionLevel(command.GetCommands());
			if (_context.PermissionLevel > commandPermissionLevel)
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Forbidden, _jsonInputData, EApiErrorCode.NO_PERMISSION, null);
				return;
			}
			_context.Response.SendChunked = true;
			WebCommandResult webCommandResult = new WebCommandResult(text3, text4, resultType, _context);
			SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteAsync(text, webCommandResult);
		}

		// Token: 0x0600D140 RID: 53568 RVA: 0x00061906 File Offset: 0x0005FB06
		public override int DefaultPermissionLevel()
		{
			return 1000;
		}

		// Token: 0x0600D141 RID: 53569 RVA: 0x004BCBA4 File Offset: 0x004BADA4
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, 2000, int.MinValue, -2147483647, -2147483647 };
		}

		// Token: 0x0600D142 RID: 53570 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public Command()
			: base(null)
		{
		}

		// Token: 0x04009E86 RID: 40582
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandsKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("commands");

		// Token: 0x04009E87 RID: 40583
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOverloadsKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("overloads");

		// Token: 0x04009E88 RID: 40584
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("command");

		// Token: 0x04009E89 RID: 40585
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonDescriptionKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description");

		// Token: 0x04009E8A RID: 40586
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonHelpKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("help");

		// Token: 0x04009E8B RID: 40587
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonAllowedKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("allowed");
	}
}

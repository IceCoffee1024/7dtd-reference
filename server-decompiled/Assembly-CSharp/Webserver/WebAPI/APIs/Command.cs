using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001AD2 RID: 6866
	[Preserve]
	public class Command : AbsRestApi
	{
		// Token: 0x0600CED1 RID: 52945 RVA: 0x004B490C File Offset: 0x004B2B0C
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

		// Token: 0x0600CED2 RID: 52946 RVA: 0x004B49E8 File Offset: 0x004B2BE8
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

		// Token: 0x0600CED3 RID: 52947 RVA: 0x004B4ACC File Offset: 0x004B2CCC
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

		// Token: 0x0600CED4 RID: 52948 RVA: 0x00061652 File Offset: 0x0005F852
		public override int DefaultPermissionLevel()
		{
			return 1000;
		}

		// Token: 0x0600CED5 RID: 52949 RVA: 0x004B4BDC File Offset: 0x004B2DDC
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, 2000, int.MinValue, -2147483647, -2147483647 };
		}

		// Token: 0x0600CED6 RID: 52950 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public Command()
			: base(null)
		{
		}

		// Token: 0x04009CD7 RID: 40151
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandsKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("commands");

		// Token: 0x04009CD8 RID: 40152
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOverloadsKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("overloads");

		// Token: 0x04009CD9 RID: 40153
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("command");

		// Token: 0x04009CDA RID: 40154
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonDescriptionKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description");

		// Token: 0x04009CDB RID: 40155
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonHelpKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("help");

		// Token: 0x04009CDC RID: 40156
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonAllowedKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("allowed");
	}
}

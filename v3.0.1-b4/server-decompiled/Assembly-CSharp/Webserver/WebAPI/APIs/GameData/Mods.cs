using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001AC5 RID: 6853
	[Preserve]
	public class Mods : AbsRestApi
	{
		// Token: 0x0600CDD6 RID: 52694 RVA: 0x004B1A9C File Offset: 0x004AFC9C
		public Mods(Web _parent)
			: base(null)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			for (int i = 0; i < _parent.WebMods.Count; i++)
			{
				WebMod webMod = _parent.WebMods[i];
				if (i > 0)
				{
					jsonWriter.WriteValueSeparator();
				}
				Mods.writeModJson(ref jsonWriter, webMod);
			}
			jsonWriter.WriteEndArray();
			this.loadedWebMods = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600CDD7 RID: 52695 RVA: 0x004B1B0C File Offset: 0x004AFD0C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void writeModJson(ref JsonWriter _writer, WebMod _webMod)
		{
			_writer.WriteBeginObject();
			_writer.WritePropertyName("name");
			_writer.WriteString(_webMod.ParentMod.Name);
			_writer.WriteValueSeparator();
			_writer.WritePropertyName("displayName");
			_writer.WriteString(_webMod.ParentMod.DisplayName);
			_writer.WriteValueSeparator();
			_writer.WritePropertyName("description");
			_writer.WriteString(_webMod.ParentMod.Description);
			_writer.WriteValueSeparator();
			_writer.WritePropertyName("author");
			_writer.WriteString(_webMod.ParentMod.Author);
			_writer.WriteValueSeparator();
			_writer.WritePropertyName("version");
			_writer.WriteString(_webMod.ParentMod.VersionString);
			_writer.WriteValueSeparator();
			_writer.WritePropertyName("website");
			_writer.WriteString(_webMod.ParentMod.Website);
			Mods.writeWebModJson(ref _writer, _webMod);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CDD8 RID: 52696 RVA: 0x004B1BF4 File Offset: 0x004AFDF4
		[PublicizedFrom(EAccessModifier.Private)]
		public static void writeWebModJson(ref JsonWriter _writer, WebMod _webMod)
		{
			if (_webMod.ModUrl != null)
			{
				_writer.WriteValueSeparator();
				_writer.WritePropertyName("web");
				_writer.WriteBeginObject();
				_writer.WritePropertyName("baseUrl");
				_writer.WriteString(_webMod.ModUrl);
				string reactBundle = _webMod.ReactBundle;
				if (reactBundle != null)
				{
					_writer.WriteValueSeparator();
					_writer.WritePropertyName("bundle");
					_writer.WriteString(reactBundle);
				}
				string cssPath = _webMod.CssPath;
				if (cssPath != null)
				{
					_writer.WriteValueSeparator();
					_writer.WritePropertyName("css");
					_writer.WriteString(cssPath);
				}
				_writer.WriteEndObject();
			}
		}

		// Token: 0x0600CDD9 RID: 52697 RVA: 0x004B1C84 File Offset: 0x004AFE84
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.loadedWebMods);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CDDA RID: 52698 RVA: 0x004AEB65 File Offset: 0x004ACD65
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009C3C RID: 39996
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] loadedWebMods;
	}
}

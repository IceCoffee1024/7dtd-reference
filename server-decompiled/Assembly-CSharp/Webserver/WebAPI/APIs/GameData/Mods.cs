using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001AEC RID: 6892
	[Preserve]
	public class Mods : AbsRestApi
	{
		// Token: 0x0600CF5F RID: 53087 RVA: 0x004B7E90 File Offset: 0x004B6090
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

		// Token: 0x0600CF60 RID: 53088 RVA: 0x004B7F00 File Offset: 0x004B6100
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

		// Token: 0x0600CF61 RID: 53089 RVA: 0x004B7FE8 File Offset: 0x004B61E8
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

		// Token: 0x0600CF62 RID: 53090 RVA: 0x004B8078 File Offset: 0x004B6278
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.loadedWebMods);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CF63 RID: 53091 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009D6D RID: 40301
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] loadedWebMods;
	}
}

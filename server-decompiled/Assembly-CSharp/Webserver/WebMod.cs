using System;
using System.IO;
using Webserver.FileCache;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001AC8 RID: 6856
	public class WebMod
	{
		// Token: 0x0600CE8F RID: 52879 RVA: 0x004B3484 File Offset: 0x004B1684
		public WebMod(Web _parentWeb, Mod _parentMod, bool _useStaticCache)
		{
			this.ParentMod = _parentMod;
			string text = _parentMod.Path + "/WebMod";
			this.IsWebMod = Directory.Exists(text);
			if (!this.IsWebMod)
			{
				return;
			}
			this.ModUrl = "/webmods/" + _parentMod.Name + "/";
			this.ReactBundle = text + "/bundle.js";
			this.ReactBundle = (File.Exists(this.ReactBundle) ? (this.ModUrl + "bundle.js") : null);
			this.CssPath = text + "/styling.css";
			this.CssPath = (File.Exists(this.CssPath) ? (this.ModUrl + "styling.css") : null);
			_parentWeb.RegisterPathHandler(this.ModUrl, new StaticHandler(text, _useStaticCache ? new SimpleCache() : new DirectAccess(), false, null));
		}

		// Token: 0x04009C89 RID: 40073
		[PublicizedFrom(EAccessModifier.Private)]
		public const string modsBaseUrl = "/webmods/";

		// Token: 0x04009C8A RID: 40074
		[PublicizedFrom(EAccessModifier.Private)]
		public const string reactBundleName = "bundle.js";

		// Token: 0x04009C8B RID: 40075
		[PublicizedFrom(EAccessModifier.Private)]
		public const string stylingFileName = "styling.css";

		// Token: 0x04009C8C RID: 40076
		public readonly Mod ParentMod;

		// Token: 0x04009C8D RID: 40077
		public readonly string ModUrl;

		// Token: 0x04009C8E RID: 40078
		public readonly string ReactBundle;

		// Token: 0x04009C8F RID: 40079
		public readonly string CssPath;

		// Token: 0x04009C90 RID: 40080
		public readonly bool IsWebMod;
	}
}

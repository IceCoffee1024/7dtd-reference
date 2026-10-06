using System;
using System.IO;
using Webserver.FileCache;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001AF4 RID: 6900
	public class WebMod
	{
		// Token: 0x0600CFE7 RID: 53223 RVA: 0x004B7950 File Offset: 0x004B5B50
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

		// Token: 0x04009D4F RID: 40271
		[PublicizedFrom(EAccessModifier.Private)]
		public const string modsBaseUrl = "/webmods/";

		// Token: 0x04009D50 RID: 40272
		[PublicizedFrom(EAccessModifier.Private)]
		public const string reactBundleName = "bundle.js";

		// Token: 0x04009D51 RID: 40273
		[PublicizedFrom(EAccessModifier.Private)]
		public const string stylingFileName = "styling.css";

		// Token: 0x04009D52 RID: 40274
		public readonly Mod ParentMod;

		// Token: 0x04009D53 RID: 40275
		public readonly string ModUrl;

		// Token: 0x04009D54 RID: 40276
		public readonly string ReactBundle;

		// Token: 0x04009D55 RID: 40277
		public readonly string CssPath;

		// Token: 0x04009D56 RID: 40278
		public readonly bool IsWebMod;
	}
}

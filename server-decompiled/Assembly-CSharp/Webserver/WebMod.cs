using System;
using System.IO;
using Webserver.FileCache;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001B25 RID: 6949
	public class WebMod
	{
		// Token: 0x0600D0FB RID: 53499 RVA: 0x004BB44C File Offset: 0x004B964C
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

		// Token: 0x04009E38 RID: 40504
		[PublicizedFrom(EAccessModifier.Private)]
		public const string modsBaseUrl = "/webmods/";

		// Token: 0x04009E39 RID: 40505
		[PublicizedFrom(EAccessModifier.Private)]
		public const string reactBundleName = "bundle.js";

		// Token: 0x04009E3A RID: 40506
		[PublicizedFrom(EAccessModifier.Private)]
		public const string stylingFileName = "styling.css";

		// Token: 0x04009E3B RID: 40507
		public readonly Mod ParentMod;

		// Token: 0x04009E3C RID: 40508
		public readonly string ModUrl;

		// Token: 0x04009E3D RID: 40509
		public readonly string ReactBundle;

		// Token: 0x04009E3E RID: 40510
		public readonly string CssPath;

		// Token: 0x04009E3F RID: 40511
		public readonly bool IsWebMod;
	}
}

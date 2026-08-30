using System;
using System.IO;
using Webserver.FileCache;
using Webserver.UrlHandlers;

namespace Webserver
{
	// Token: 0x02001AA1 RID: 6817
	public class WebMod
	{
		// Token: 0x0600CD06 RID: 52486 RVA: 0x004AD090 File Offset: 0x004AB290
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

		// Token: 0x04009B58 RID: 39768
		[PublicizedFrom(EAccessModifier.Private)]
		public const string modsBaseUrl = "/webmods/";

		// Token: 0x04009B59 RID: 39769
		[PublicizedFrom(EAccessModifier.Private)]
		public const string reactBundleName = "bundle.js";

		// Token: 0x04009B5A RID: 39770
		[PublicizedFrom(EAccessModifier.Private)]
		public const string stylingFileName = "styling.css";

		// Token: 0x04009B5B RID: 39771
		public readonly Mod ParentMod;

		// Token: 0x04009B5C RID: 39772
		public readonly string ModUrl;

		// Token: 0x04009B5D RID: 39773
		public readonly string ReactBundle;

		// Token: 0x04009B5E RID: 39774
		public readonly string CssPath;

		// Token: 0x04009B5F RID: 39775
		public readonly bool IsWebMod;
	}
}

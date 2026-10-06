using System;
using System.IO;
using Webserver.FileCache;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B24 RID: 6948
	public class StaticHandler : AbsHandler
	{
		// Token: 0x0600D0FF RID: 53503 RVA: 0x004BDFC4 File Offset: 0x004BC1C4
		public StaticHandler(string _filePath, AbstractCache _cache, bool _logMissingFiles, string _moduleName = null)
			: base(_moduleName, 0)
		{
			this.datapath = _filePath + ((_filePath[_filePath.Length - 1] == '/') ? "" : "/");
			this.cache = _cache;
			this.logMissingFiles = _logMissingFiles;
		}

		// Token: 0x0600D100 RID: 53504 RVA: 0x004BE014 File Offset: 0x004BC214
		public override void HandleRequest(RequestContext _context)
		{
			string text = _context.RequestPath.Remove(0, this.urlBasePath.Length);
			byte[] fileContent = this.cache.GetFileContent(this.datapath + text);
			if (fileContent != null)
			{
				_context.Response.ContentType = MimeType.GetMimeType(Path.GetExtension(text));
				_context.Response.ContentLength64 = (long)fileContent.Length;
				_context.Response.OutputStream.Write(fileContent, 0, fileContent.Length);
				return;
			}
			_context.Response.StatusCode = 404;
			if (this.logMissingFiles)
			{
				Log.Warning(string.Concat(new string[] { "[Web] Static: FileNotFound: \"", _context.RequestPath, "\" @ \"", this.datapath, text, "\"" }));
			}
		}

		// Token: 0x04009E7D RID: 40573
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AbstractCache cache;

		// Token: 0x04009E7E RID: 40574
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string datapath;

		// Token: 0x04009E7F RID: 40575
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool logMissingFiles;
	}
}

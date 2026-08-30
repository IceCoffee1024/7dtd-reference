using System;
using System.IO;
using Webserver.FileCache;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AD1 RID: 6865
	public class StaticHandler : AbsHandler
	{
		// Token: 0x0600CE1E RID: 52766 RVA: 0x004B3704 File Offset: 0x004B1904
		public StaticHandler(string _filePath, AbstractCache _cache, bool _logMissingFiles, string _moduleName = null)
			: base(_moduleName, 0)
		{
			this.datapath = _filePath + ((_filePath[_filePath.Length - 1] == '/') ? "" : "/");
			this.cache = _cache;
			this.logMissingFiles = _logMissingFiles;
		}

		// Token: 0x0600CE1F RID: 52767 RVA: 0x004B3754 File Offset: 0x004B1954
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

		// Token: 0x04009C86 RID: 40070
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AbstractCache cache;

		// Token: 0x04009C87 RID: 40071
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string datapath;

		// Token: 0x04009C88 RID: 40072
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool logMissingFiles;
	}
}

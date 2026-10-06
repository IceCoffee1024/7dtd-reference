using System;
using System.Text;
using UnityEngine.Scripting;

namespace Webserver.WebAPI
{
	// Token: 0x02001AFB RID: 6907
	[Preserve]
	public class Null : AbsWebAPI
	{
		// Token: 0x0600D018 RID: 53272 RVA: 0x004B8634 File Offset: 0x004B6834
		public Null(string _name)
			: base(_name)
		{
		}

		// Token: 0x0600D019 RID: 53273 RVA: 0x004B8640 File Offset: 0x004B6840
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.ContentLength64 = 0L;
			_context.Response.ContentType = "text/plain";
			_context.Response.ContentEncoding = Encoding.ASCII;
			_context.Response.OutputStream.Write(Array.Empty<byte>(), 0, 0);
		}
	}
}

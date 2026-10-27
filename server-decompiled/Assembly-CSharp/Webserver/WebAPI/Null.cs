using System;
using System.Text;
using UnityEngine.Scripting;

namespace Webserver.WebAPI
{
	// Token: 0x02001B2C RID: 6956
	[Preserve]
	public class Null : AbsWebAPI
	{
		// Token: 0x0600D12C RID: 53548 RVA: 0x004BC130 File Offset: 0x004BA330
		public Null(string _name)
			: base(_name)
		{
		}

		// Token: 0x0600D12D RID: 53549 RVA: 0x004BC13C File Offset: 0x004BA33C
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.ContentLength64 = 0L;
			_context.Response.ContentType = "text/plain";
			_context.Response.ContentEncoding = Encoding.ASCII;
			_context.Response.OutputStream.Write(Array.Empty<byte>(), 0, 0);
		}
	}
}

using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001ACF RID: 6863
	public class SimpleRedirectHandler : AbsHandler
	{
		// Token: 0x0600CE11 RID: 52753 RVA: 0x004B3260 File Offset: 0x004B1460
		public SimpleRedirectHandler(string _target)
			: base(null, 0)
		{
			this.target = _target;
		}

		// Token: 0x0600CE12 RID: 52754 RVA: 0x004B3271 File Offset: 0x004B1471
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.Redirect(this.target);
		}

		// Token: 0x04009C7E RID: 40062
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;
	}
}

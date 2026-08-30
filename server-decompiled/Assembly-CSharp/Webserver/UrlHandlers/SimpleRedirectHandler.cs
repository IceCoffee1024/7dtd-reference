using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AF6 RID: 6902
	public class SimpleRedirectHandler : AbsHandler
	{
		// Token: 0x0600CF9A RID: 53146 RVA: 0x004B9654 File Offset: 0x004B7854
		public SimpleRedirectHandler(string _target)
			: base(null, 0)
		{
			this.target = _target;
		}

		// Token: 0x0600CF9B RID: 53147 RVA: 0x004B9665 File Offset: 0x004B7865
		public override void HandleRequest(RequestContext _context)
		{
			_context.Response.Redirect(this.target);
		}

		// Token: 0x04009DAF RID: 40367
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;
	}
}

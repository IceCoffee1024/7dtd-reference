using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AF4 RID: 6900
	public class RewriteHandler : AbsHandler
	{
		// Token: 0x0600CF91 RID: 53137 RVA: 0x004B90C4 File Offset: 0x004B72C4
		public RewriteHandler(string _target, bool _fixedTarget = false)
			: base(null, 0)
		{
			this.target = _target;
			this.fixedTarget = _fixedTarget;
		}

		// Token: 0x0600CF92 RID: 53138 RVA: 0x004B90DC File Offset: 0x004B72DC
		public override void HandleRequest(RequestContext _context)
		{
			_context.RequestPath = (this.fixedTarget ? this.target : (this.target + _context.RequestPath.Remove(0, this.urlBasePath.Length)));
			this.parent.ApplyPathHandler(_context);
		}

		// Token: 0x04009DA5 RID: 40357
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;

		// Token: 0x04009DA6 RID: 40358
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool fixedTarget;
	}
}

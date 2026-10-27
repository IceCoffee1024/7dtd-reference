using System;
using Webserver.Permissions;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B4A RID: 6986
	public abstract class AbsHandler
	{
		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x0600D1D0 RID: 53712 RVA: 0x004C0071 File Offset: 0x004BE271
		public string UrlBasePath
		{
			get
			{
				return this.urlBasePath;
			}
		}

		// Token: 0x0600D1D1 RID: 53713 RVA: 0x004C0079 File Offset: 0x004BE279
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsHandler(string _moduleName, int _defaultPermissionLevel = 0)
		{
			this.ModuleName = _moduleName;
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(_moduleName, _defaultPermissionLevel, true));
		}

		// Token: 0x0600D1D2 RID: 53714
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600D1D3 RID: 53715 RVA: 0x004C009A File Offset: 0x004BE29A
		public virtual bool IsAuthorizedForHandler(RequestContext _context)
		{
			return this.ModuleName == null || AdminWebModules.Instance.ModuleAllowedWithLevel(this.ModuleName, _context.PermissionLevel);
		}

		// Token: 0x0600D1D4 RID: 53716 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Shutdown()
		{
		}

		// Token: 0x0600D1D5 RID: 53717 RVA: 0x004C00BC File Offset: 0x004BE2BC
		public virtual void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			this.parent = _parent;
			this.urlBasePath = _relativePath;
			this.parent.OpenApiHelpers.LoadOpenApiSpec(this);
		}

		// Token: 0x04009F1D RID: 40733
		public readonly string ModuleName;

		// Token: 0x04009F1E RID: 40734
		[PublicizedFrom(EAccessModifier.Protected)]
		public string urlBasePath;

		// Token: 0x04009F1F RID: 40735
		[PublicizedFrom(EAccessModifier.Protected)]
		public Web parent;
	}
}

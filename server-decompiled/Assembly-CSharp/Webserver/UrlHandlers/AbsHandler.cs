using System;
using Webserver.Permissions;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AED RID: 6893
	public abstract class AbsHandler
	{
		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x0600CF64 RID: 53092 RVA: 0x004B80A9 File Offset: 0x004B62A9
		public string UrlBasePath
		{
			get
			{
				return this.urlBasePath;
			}
		}

		// Token: 0x0600CF65 RID: 53093 RVA: 0x004B80B1 File Offset: 0x004B62B1
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsHandler(string _moduleName, int _defaultPermissionLevel = 0)
		{
			this.ModuleName = _moduleName;
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(_moduleName, _defaultPermissionLevel, true));
		}

		// Token: 0x0600CF66 RID: 53094
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600CF67 RID: 53095 RVA: 0x004B80D2 File Offset: 0x004B62D2
		public virtual bool IsAuthorizedForHandler(RequestContext _context)
		{
			return this.ModuleName == null || AdminWebModules.Instance.ModuleAllowedWithLevel(this.ModuleName, _context.PermissionLevel);
		}

		// Token: 0x0600CF68 RID: 53096 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Shutdown()
		{
		}

		// Token: 0x0600CF69 RID: 53097 RVA: 0x004B80F4 File Offset: 0x004B62F4
		public virtual void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			this.parent = _parent;
			this.urlBasePath = _relativePath;
			this.parent.OpenApiHelpers.LoadOpenApiSpec(this);
		}

		// Token: 0x04009D6E RID: 40302
		public readonly string ModuleName;

		// Token: 0x04009D6F RID: 40303
		[PublicizedFrom(EAccessModifier.Protected)]
		public string urlBasePath;

		// Token: 0x04009D70 RID: 40304
		[PublicizedFrom(EAccessModifier.Protected)]
		public Web parent;
	}
}

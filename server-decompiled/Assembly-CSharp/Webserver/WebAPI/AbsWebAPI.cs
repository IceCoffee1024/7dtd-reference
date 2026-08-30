using System;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001ACC RID: 6860
	public abstract class AbsWebAPI
	{
		// Token: 0x0600CEAC RID: 52908 RVA: 0x004B3C9D File Offset: 0x004B1E9D
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600CEAD RID: 52909 RVA: 0x004B3CA8 File Offset: 0x004B1EA8
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(Web _parentWeb, string _name = null)
		{
			this.Name = _name ?? base.GetType().Name;
			this.ParentWeb = _parentWeb;
			this.CachedApiModuleName = "webapi." + this.Name;
			this.RegisterPermissions();
		}

		// Token: 0x0600CEAE RID: 52910 RVA: 0x004B3CF4 File Offset: 0x004B1EF4
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600CEAF RID: 52911
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600CEB0 RID: 52912 RVA: 0x004B3D12 File Offset: 0x004B1F12
		public virtual bool Authorized(RequestContext _context)
		{
			return AdminWebModules.Instance.GetModule(this.CachedApiModuleName).LevelGlobal >= _context.PermissionLevel;
		}

		// Token: 0x0600CEB1 RID: 52913 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x04009CA2 RID: 40098
		public readonly string Name;

		// Token: 0x04009CA3 RID: 40099
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Web ParentWeb;

		// Token: 0x04009CA4 RID: 40100
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string CachedApiModuleName;
	}
}

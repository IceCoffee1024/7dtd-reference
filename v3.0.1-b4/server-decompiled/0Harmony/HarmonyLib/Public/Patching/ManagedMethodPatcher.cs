using System;
using System.Reflection;
using HarmonyLib.Internal.Util;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace HarmonyLib.Public.Patching
{
	// Token: 0x0200005B RID: 91
	public class ManagedMethodPatcher : MethodPatcher
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x00012DC1 File Offset: 0x00010FC1
		public ManagedMethodPatcher(MethodBase original)
			: base(original)
		{
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00012DCA File Offset: 0x00010FCA
		public override DynamicMethodDefinition PrepareOriginal()
		{
			return null;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00012DD0 File Offset: 0x00010FD0
		public override MethodBase DetourTo(MethodBase replacement)
		{
			if (this.ilHook == null)
			{
				this.ilHook = new ILHook(base.Original, new ILContext.Manipulator(this.Manipulator), false);
			}
			try
			{
				this.ilHook.Undo();
				this.ilHook.Apply();
			}
			catch (Exception ex)
			{
				throw HarmonyException.Create(ex, this.hookBody);
			}
			return this.ilHook.GetEndOfChain();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00012E48 File Offset: 0x00011048
		public override DynamicMethodDefinition CopyOriginal()
		{
			return new DynamicMethodDefinition(base.Original);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00012E55 File Offset: 0x00011055
		private void Manipulator(ILContext ctx)
		{
			this.hookBody = ctx.Body;
			HarmonyManipulator.Manipulate(base.Original, ctx);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00012E6F File Offset: 0x0001106F
		public static void TryResolve(object sender, PatchManager.PatcherResolverEventArgs args)
		{
			if (args.Original.GetMethodBody() != null)
			{
				args.MethodPatcher = new ManagedMethodPatcher(args.Original);
			}
		}

		// Token: 0x040000F2 RID: 242
		private Mono.Cecil.Cil.MethodBody hookBody;

		// Token: 0x040000F3 RID: 243
		private ILHook ilHook;
	}
}

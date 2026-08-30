using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class Patch : IComparable
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00006708 File Offset: 0x00004908
		// (set) Token: 0x06000137 RID: 311 RVA: 0x000067A0 File Offset: 0x000049A0
		public MethodInfo PatchMethod
		{
			get
			{
				if (this.patchMethod == null)
				{
					Module module = (from a in AppDomain.CurrentDomain.GetAssemblies()
						where !a.FullName.StartsWith("Microsoft.VisualStudio")
						select a).SelectMany<Assembly, Module>((Assembly a) => a.GetLoadedModules()).First<Module>((Module m) => m.ModuleVersionId.ToString() == this.moduleGUID);
					this.patchMethod = (MethodInfo)module.ResolveMethod(this.methodToken);
				}
				return this.patchMethod;
			}
			set
			{
				this.patchMethod = value;
				this.methodToken = this.patchMethod.MetadataToken;
				this.moduleGUID = this.patchMethod.Module.ModuleVersionId.ToString();
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000067EC File Offset: 0x000049EC
		public Patch(MethodInfo patch, int index, string owner, int priority, string[] before, string[] after, bool debug)
		{
			if (patch is DynamicMethod)
			{
				throw new Exception("Cannot directly reference dynamic method \"" + patch.FullDescription() + "\" in Harmony. Use a factory method instead that will return the dynamic method.");
			}
			this.index = index;
			this.owner = owner;
			this.priority = ((priority == -1) ? 400 : priority);
			this.before = before ?? new string[0];
			this.after = after ?? new string[0];
			this.debug = debug;
			this.PatchMethod = patch;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006878 File Offset: 0x00004A78
		public Patch(MethodInfo patch, int index, string owner, int priority, string[] before, string[] after, bool debug, bool wrapTryCatch)
		{
			if (patch is DynamicMethod)
			{
				throw new Exception("Cannot directly reference dynamic method \"" + patch.FullDescription() + "\" in Harmony. Use a factory method instead that will return the dynamic method.");
			}
			this.index = index;
			this.owner = owner;
			this.priority = ((priority == -1) ? 400 : priority);
			this.before = before ?? new string[0];
			this.after = after ?? new string[0];
			this.debug = debug;
			this.wrapTryCatch = wrapTryCatch;
			this.PatchMethod = patch;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000690C File Offset: 0x00004B0C
		public Patch(MethodInfo patch, int index, string owner, int priority, string[] before, string[] after, bool debug, bool wrapTryCatch, string debugEmitPath)
		{
			if (patch is DynamicMethod)
			{
				throw new Exception("Cannot directly reference dynamic method \"" + patch.FullDescription() + "\" in Harmony. Use a factory method instead that will return the dynamic method.");
			}
			this.index = index;
			this.owner = owner;
			this.priority = ((priority == -1) ? 400 : priority);
			this.before = before ?? new string[0];
			this.after = after ?? new string[0];
			this.debug = debug;
			this.debugEmitPath = debugEmitPath;
			this.wrapTryCatch = wrapTryCatch;
			this.PatchMethod = patch;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000069A8 File Offset: 0x00004BA8
		public Patch(HarmonyMethod method, int index, string owner)
			: this(method.method, index, owner, method.priority, method.before, method.after, method.debug.GetValueOrDefault(), method.wrapTryCatch.GetValueOrDefault(), method.debugEmitPath)
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000069F4 File Offset: 0x00004BF4
		internal Patch(int index, string owner, int priority, string[] before, string[] after, bool debug, int methodToken, string moduleGUID)
		{
			this.index = index;
			this.owner = owner;
			this.priority = ((priority == -1) ? 400 : priority);
			this.before = before ?? new string[0];
			this.after = after ?? new string[0];
			this.debug = debug;
			this.methodToken = methodToken;
			this.moduleGUID = moduleGUID;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006A64 File Offset: 0x00004C64
		public MethodInfo GetMethod(MethodBase original)
		{
			MethodInfo methodInfo = this.PatchMethod;
			if (methodInfo.ReturnType != typeof(DynamicMethod) && methodInfo.ReturnType != typeof(MethodInfo))
			{
				return methodInfo;
			}
			if (!methodInfo.IsStatic)
			{
				return methodInfo;
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != 1)
			{
				return methodInfo;
			}
			if (parameters[0].ParameterType != typeof(MethodBase))
			{
				return methodInfo;
			}
			return methodInfo.Invoke(null, new object[] { original }) as MethodInfo;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006AF2 File Offset: 0x00004CF2
		public override bool Equals(object obj)
		{
			return obj != null && obj is Patch && this.PatchMethod == ((Patch)obj).PatchMethod;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006B17 File Offset: 0x00004D17
		public int CompareTo(object obj)
		{
			return PatchInfoSerialization.PriorityComparer(obj, this.index, this.priority);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006B2B File Offset: 0x00004D2B
		public override int GetHashCode()
		{
			return this.PatchMethod.GetHashCode();
		}

		// Token: 0x04000079 RID: 121
		public readonly int index;

		// Token: 0x0400007A RID: 122
		public readonly string owner;

		// Token: 0x0400007B RID: 123
		public readonly int priority;

		// Token: 0x0400007C RID: 124
		public readonly string[] before;

		// Token: 0x0400007D RID: 125
		public readonly string[] after;

		// Token: 0x0400007E RID: 126
		public readonly bool debug;

		// Token: 0x0400007F RID: 127
		public readonly string debugEmitPath;

		// Token: 0x04000080 RID: 128
		public readonly bool wrapTryCatch;

		// Token: 0x04000081 RID: 129
		[NonSerialized]
		private MethodInfo patchMethod;

		// Token: 0x04000082 RID: 130
		private int methodToken;

		// Token: 0x04000083 RID: 131
		private string moduleGUID;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000016 RID: 22
	internal class PatchJobs<T>
	{
		// Token: 0x06000057 RID: 87 RVA: 0x0000361C File Offset: 0x0000181C
		internal PatchJobs<T>.Job GetJob(MethodBase method)
		{
			if (method == null)
			{
				return null;
			}
			PatchJobs<T>.Job job;
			if (!this.state.TryGetValue(method, out job))
			{
				job = new PatchJobs<T>.Job
				{
					original = method
				};
				this.state[method] = job;
			}
			return job;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003659 File Offset: 0x00001859
		internal List<PatchJobs<T>.Job> GetJobs()
		{
			return this.state.Values.Where<PatchJobs<T>.Job>((PatchJobs<T>.Job job) => job.prefixes.Count + job.postfixes.Count + job.transpilers.Count + job.finalizers.Count + job.ilmanipulators.Count > 0).ToList<PatchJobs<T>.Job>();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000368F File Offset: 0x0000188F
		internal List<T> GetReplacements()
		{
			return this.state.Values.Select<PatchJobs<T>.Job, T>((PatchJobs<T>.Job job) => job.replacement).ToList<T>();
		}

		// Token: 0x04000022 RID: 34
		internal Dictionary<MethodBase, PatchJobs<T>.Job> state = new Dictionary<MethodBase, PatchJobs<T>.Job>();

		// Token: 0x02000075 RID: 117
		internal class Job
		{
			// Token: 0x0600048B RID: 1163 RVA: 0x00016D70 File Offset: 0x00014F70
			internal void AddPatch(AttributePatch patch)
			{
				HarmonyPatchType? type = patch.type;
				if (type != null)
				{
					switch (type.GetValueOrDefault())
					{
					case HarmonyPatchType.Prefix:
						this.prefixes.Add(patch.info);
						return;
					case HarmonyPatchType.Postfix:
						this.postfixes.Add(patch.info);
						return;
					case HarmonyPatchType.Transpiler:
						this.transpilers.Add(patch.info);
						return;
					case HarmonyPatchType.Finalizer:
						this.finalizers.Add(patch.info);
						return;
					case HarmonyPatchType.ReversePatch:
						break;
					case HarmonyPatchType.ILManipulator:
						this.ilmanipulators.Add(patch.info);
						break;
					default:
						return;
					}
				}
			}

			// Token: 0x04000148 RID: 328
			internal MethodBase original;

			// Token: 0x04000149 RID: 329
			internal T replacement;

			// Token: 0x0400014A RID: 330
			internal List<HarmonyMethod> prefixes = new List<HarmonyMethod>();

			// Token: 0x0400014B RID: 331
			internal List<HarmonyMethod> postfixes = new List<HarmonyMethod>();

			// Token: 0x0400014C RID: 332
			internal List<HarmonyMethod> transpilers = new List<HarmonyMethod>();

			// Token: 0x0400014D RID: 333
			internal List<HarmonyMethod> finalizers = new List<HarmonyMethod>();

			// Token: 0x0400014E RID: 334
			internal List<HarmonyMethod> ilmanipulators = new List<HarmonyMethod>();
		}
	}
}

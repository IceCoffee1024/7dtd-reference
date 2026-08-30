using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib.Public.Patching;
using HarmonyLib.Tools;

namespace HarmonyLib
{
	// Token: 0x02000043 RID: 67
	public class PatchClassProcessor
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00006B64 File Offset: 0x00004D64
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00006B6C File Offset: 0x00004D6C
		public string Category { get; set; }

		// Token: 0x06000144 RID: 324 RVA: 0x00006B75 File Offset: 0x00004D75
		public PatchClassProcessor(Harmony instance, Type type)
			: this(instance, type, false)
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006B80 File Offset: 0x00004D80
		public PatchClassProcessor(Harmony instance, Type type, bool allowUnannotatedType)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.instance = instance;
			this.containerType = type;
			List<HarmonyMethod> fromType = HarmonyMethodExtensions.GetFromType(type);
			if (!allowUnannotatedType && (fromType == null || fromType.Count == 0))
			{
				return;
			}
			this.containerAttributes = HarmonyMethod.Merge(fromType);
			HarmonyMethod harmonyMethod = this.containerAttributes;
			MethodType methodType = harmonyMethod.methodType.GetValueOrDefault();
			if (harmonyMethod.methodType == null)
			{
				methodType = MethodType.Normal;
				harmonyMethod.methodType = new MethodType?(methodType);
			}
			this.Category = this.containerAttributes.category;
			this.auxilaryMethods = new Dictionary<Type, MethodInfo>();
			foreach (Type type2 in PatchClassProcessor.auxilaryTypes)
			{
				MethodInfo patchMethod = PatchTools.GetPatchMethod(this.containerType, type2.FullName);
				if (patchMethod != null)
				{
					this.auxilaryMethods[type2] = patchMethod;
				}
			}
			this.patchMethods = PatchTools.GetPatchMethods(this.containerType, this.containerAttributes.declaringType != null);
			foreach (AttributePatch attributePatch in this.patchMethods)
			{
				MethodInfo method = attributePatch.info.method;
				attributePatch.info = this.containerAttributes.Merge(attributePatch.info);
				attributePatch.info.method = method;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006D24 File Offset: 0x00004F24
		public List<MethodInfo> Patch()
		{
			if (this.containerAttributes == null)
			{
				return null;
			}
			Exception ex = null;
			if (!this.RunMethod<HarmonyPrepare, bool>(true, false, null, new object[0]))
			{
				this.RunMethod<HarmonyCleanup>(ref ex, new object[0]);
				this.ReportException(ex, null);
				return new List<MethodInfo>();
			}
			List<MethodInfo> list = new List<MethodInfo>();
			MethodBase methodBase = null;
			try
			{
				List<MethodBase> bulkMethods = this.GetBulkMethods();
				if (bulkMethods.Count == 1)
				{
					methodBase = bulkMethods[0];
				}
				this.ReversePatch(ref methodBase);
				list = ((bulkMethods.Count > 0) ? this.BulkPatch(bulkMethods, ref methodBase, false) : this.PatchWithAttributes(ref methodBase, false));
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			this.RunMethod<HarmonyCleanup>(ref ex, new object[] { ex });
			this.ReportException(ex, methodBase);
			return list;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006DEC File Offset: 0x00004FEC
		public void Unpatch()
		{
			if (this.containerAttributes == null)
			{
				return;
			}
			List<MethodBase> bulkMethods = this.GetBulkMethods();
			MethodBase methodBase = null;
			if (bulkMethods.Count > 0)
			{
				this.BulkPatch(bulkMethods, ref methodBase, true);
				return;
			}
			this.PatchWithAttributes(ref methodBase, true);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006E2C File Offset: 0x0000502C
		private void ReversePatch(ref MethodBase lastOriginal)
		{
			for (int i = 0; i < this.patchMethods.Count; i++)
			{
				AttributePatch patchMethod = this.patchMethods[i];
				if (patchMethod.type.GetValueOrDefault() == HarmonyPatchType.ReversePatch)
				{
					MethodBase originalMethod = patchMethod.info.GetOriginalMethod();
					if (originalMethod != null)
					{
						lastOriginal = originalMethod;
					}
					if (lastOriginal == null)
					{
						if (!patchMethod.info.optional.GetValueOrDefault())
						{
							throw new ArgumentException("Undefined target method for reverse patch method " + patchMethod.info.method.FullDescription());
						}
						Logger.Log(Logger.LogChannel.Warn, () => "Skipping optional reverse patch " + patchMethod.info.method.FullDescription() + " - target method not found", false);
					}
					else
					{
						ReversePatcher reversePatcher = this.instance.CreateReversePatcher(lastOriginal, patchMethod.info);
						object locker = PatchProcessor.locker;
						lock (locker)
						{
							reversePatcher.Patch(HarmonyReversePatchType.Original);
						}
					}
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006F44 File Offset: 0x00005144
		private List<MethodInfo> BulkPatch(List<MethodBase> originals, ref MethodBase lastOriginal, bool unpatch)
		{
			PatchJobs<MethodInfo> patchJobs = new PatchJobs<MethodInfo>();
			for (int i = 0; i < originals.Count; i++)
			{
				lastOriginal = originals[i];
				PatchJobs<MethodInfo>.Job job = patchJobs.GetJob(lastOriginal);
				foreach (AttributePatch attributePatch in this.patchMethods)
				{
					string text = "You cannot combine TargetMethod, TargetMethods or [HarmonyPatchAll] with individual annotations";
					HarmonyMethod info = attributePatch.info;
					if (info.methodName != null)
					{
						throw new ArgumentException(text + " [" + info.methodName + "]");
					}
					if (info.methodType != null && info.methodType.Value != MethodType.Normal)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
						defaultInterpolatedStringHandler.AppendFormatted(text);
						defaultInterpolatedStringHandler.AppendLiteral(" [");
						defaultInterpolatedStringHandler.AppendFormatted<MethodType?>(info.methodType);
						defaultInterpolatedStringHandler.AppendLiteral("]");
						throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					if (info.argumentTypes != null)
					{
						throw new ArgumentException(text + " [" + info.argumentTypes.Description() + "]");
					}
					job.AddPatch(attributePatch);
				}
			}
			foreach (PatchJobs<MethodInfo>.Job job2 in patchJobs.GetJobs())
			{
				lastOriginal = job2.original;
				if (unpatch)
				{
					this.ProcessUnpatchJob(job2);
				}
				else
				{
					this.ProcessPatchJob(job2);
				}
			}
			return patchJobs.GetReplacements();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000070F4 File Offset: 0x000052F4
		private List<MethodInfo> PatchWithAttributes(ref MethodBase lastOriginal, bool unpatch)
		{
			PatchJobs<MethodInfo> patchJobs = new PatchJobs<MethodInfo>();
			using (List<AttributePatch>.Enumerator enumerator = this.patchMethods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AttributePatch patchMethod = enumerator.Current;
					lastOriginal = patchMethod.info.GetOriginalMethod();
					if (lastOriginal == null)
					{
						if (!patchMethod.info.optional.GetValueOrDefault())
						{
							throw new ArgumentException("Undefined target method for patch method " + patchMethod.info.method.FullDescription());
						}
						Logger.Log(Logger.LogChannel.Warn, () => "Skipping optional patch " + patchMethod.info.method.FullDescription() + " - target method not found", false);
					}
					else
					{
						PatchJobs<MethodInfo>.Job job = patchJobs.GetJob(lastOriginal);
						job.AddPatch(patchMethod);
					}
				}
			}
			foreach (PatchJobs<MethodInfo>.Job job2 in patchJobs.GetJobs())
			{
				lastOriginal = job2.original;
				if (unpatch)
				{
					this.ProcessUnpatchJob(job2);
				}
				else
				{
					this.ProcessPatchJob(job2);
				}
			}
			return patchJobs.GetReplacements();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007234 File Offset: 0x00005434
		private void ProcessPatchJob(PatchJobs<MethodInfo>.Job job)
		{
			MethodInfo methodInfo = null;
			bool flag = this.RunMethod<HarmonyPrepare, bool>(true, false, null, new object[] { job.original });
			Exception ex = null;
			if (flag)
			{
				object locker = PatchProcessor.locker;
				lock (locker)
				{
					try
					{
						PatchInfo patchInfo = job.original.ToPatchInfo();
						patchInfo.AddPrefixes(this.instance.Id, job.prefixes.ToArray());
						patchInfo.AddPostfixes(this.instance.Id, job.postfixes.ToArray());
						patchInfo.AddTranspilers(this.instance.Id, job.transpilers.ToArray());
						patchInfo.AddFinalizers(this.instance.Id, job.finalizers.ToArray());
						patchInfo.AddILManipulators(this.instance.Id, job.ilmanipulators.ToArray());
						methodInfo = PatchFunctions.UpdateWrapper(job.original, patchInfo);
						PatchManager.AddReplacementOriginal(job.original, methodInfo);
					}
					catch (Exception ex2)
					{
						ex = ex2;
					}
				}
			}
			this.RunMethod<HarmonyCleanup>(ref ex, new object[] { job.original, ex });
			this.ReportException(ex, job.original);
			job.replacement = methodInfo;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000738C File Offset: 0x0000558C
		private void ProcessUnpatchJob(PatchJobs<MethodInfo>.Job job)
		{
			PatchInfo patchInfo = job.original.GetPatchInfo() ?? new PatchInfo();
			bool flag = job.original.HasMethodBody();
			if (flag)
			{
				job.postfixes.Do<HarmonyMethod>(delegate(HarmonyMethod patch)
				{
					patchInfo.RemovePatch(patch.method);
				});
				job.prefixes.Do<HarmonyMethod>(delegate(HarmonyMethod patch)
				{
					patchInfo.RemovePatch(patch.method);
				});
			}
			job.transpilers.Do<HarmonyMethod>(delegate(HarmonyMethod patch)
			{
				patchInfo.RemovePatch(patch.method);
			});
			if (flag)
			{
				job.finalizers.Do<HarmonyMethod>(delegate(HarmonyMethod patch)
				{
					patchInfo.RemovePatch(patch.method);
				});
			}
			MethodInfo methodInfo = PatchFunctions.UpdateWrapper(job.original, patchInfo);
			PatchManager.AddReplacementOriginal(job.original, methodInfo);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007448 File Offset: 0x00005648
		private List<MethodBase> GetBulkMethods()
		{
			bool flag = this.containerType.GetCustomAttributes(true).Any<object>((object a) => a.GetType().FullName == PatchTools.harmonyPatchAllFullName);
			if (flag)
			{
				Type declaringType = this.containerAttributes.declaringType;
				if (declaringType == null)
				{
					throw new ArgumentException("Using " + PatchTools.harmonyPatchAllFullName + " requires an additional attribute for specifying the Class/Type");
				}
				List<MethodBase> list = new List<MethodBase>();
				list.AddRange(AccessTools.GetDeclaredConstructors(declaringType, null).Cast<MethodBase>());
				list.AddRange(AccessTools.GetDeclaredMethods(declaringType).Cast<MethodBase>());
				List<PropertyInfo> declaredProperties = AccessTools.GetDeclaredProperties(declaringType);
				list.AddRange((from prop in declaredProperties
					select prop.GetGetMethod(true) into method
					where method != null
					select method).Cast<MethodBase>());
				list.AddRange((from prop in declaredProperties
					select prop.GetSetMethod(true) into method
					where method != null
					select method).Cast<MethodBase>());
				return list;
			}
			else
			{
				List<MethodBase> list2 = new List<MethodBase>();
				IEnumerable<MethodBase> enumerable = this.RunMethod<HarmonyTargetMethods, IEnumerable<MethodBase>>(null, null, null, new object[0]);
				if (enumerable == null)
				{
					MethodBase methodBase = this.RunMethod<HarmonyTargetMethod, MethodBase>(null, null, delegate(MethodBase method)
					{
						if (method != null)
						{
							return null;
						}
						return "null";
					}, new object[0]);
					if (methodBase != null)
					{
						list2.Add(methodBase);
					}
					return list2;
				}
				string text = null;
				list2 = enumerable.ToList<MethodBase>();
				if (list2 == null)
				{
					text = "null";
				}
				else if (list2.Any<MethodBase>((MethodBase m) => m == null))
				{
					text = "some element was null";
				}
				if (text == null)
				{
					return list2;
				}
				MethodInfo methodInfo;
				if (this.auxilaryMethods.TryGetValue(typeof(HarmonyTargetMethods), out methodInfo))
				{
					throw new Exception("Method " + methodInfo.FullDescription() + " returned an unexpected result: " + text);
				}
				throw new Exception("Some method returned an unexpected result: " + text);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007690 File Offset: 0x00005890
		private void ReportException(Exception exception, MethodBase original)
		{
			if (exception == null)
			{
				return;
			}
			Logger.Log(Logger.LogChannel.Debug, delegate
			{
				Version version;
				Harmony.VersionInfo(out version);
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("### Exception from user \"");
				defaultInterpolatedStringHandler.AppendFormatted(this.instance.Id);
				defaultInterpolatedStringHandler.AppendLiteral("\", Harmony v");
				defaultInterpolatedStringHandler.AppendFormatted<Version>(version);
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				StringBuilder stringBuilder3 = stringBuilder;
				string text = "### Original: ";
				MethodBase original2 = original;
				stringBuilder3.AppendLine(text + (((original2 != null) ? original2.FullDescription() : null) ?? "NULL"));
				stringBuilder.AppendLine("### Patch class: " + this.containerType.FullDescription());
				Exception ex = exception;
				HarmonyException ex2 = ex as HarmonyException;
				if (ex2 != null)
				{
					ex = ex2.InnerException;
				}
				string text2 = ex.ToString();
				while (text2.Contains("\n\n"))
				{
					text2 = text2.Replace("\n\n", "\n");
				}
				text2 = text2.Split(new char[] { '\n' }).Join<string>((string line) => "### " + line, "\n");
				stringBuilder.AppendLine(text2.Trim());
				return stringBuilder.ToString();
			}, false);
			if (exception is HarmonyException)
			{
				throw exception;
			}
			throw new HarmonyException("Patching exception in method " + original.FullDescription(), exception);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000770C File Offset: 0x0000590C
		private T RunMethod<S, T>(T defaultIfNotExisting, T defaultIfFailing, Func<T, string> failOnResult = null, params object[] parameters)
		{
			MethodInfo methodInfo;
			if (!this.auxilaryMethods.TryGetValue(typeof(S), out methodInfo))
			{
				return defaultIfNotExisting;
			}
			object[] array = (parameters ?? new object[0]).Union<object>(new object[] { this.instance }).ToArray<object>();
			object[] array2 = AccessTools.ActualParameters(methodInfo, array);
			if (methodInfo.ReturnType != typeof(void) && !typeof(T).IsAssignableFrom(methodInfo.ReturnType))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Method ");
				defaultInterpolatedStringHandler.AppendFormatted(methodInfo.FullDescription());
				defaultInterpolatedStringHandler.AppendLiteral(" has wrong return type (should be assignable to ");
				defaultInterpolatedStringHandler.AppendFormatted(typeof(T).FullName);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			T t = defaultIfFailing;
			try
			{
				if (methodInfo.ReturnType == typeof(void))
				{
					methodInfo.Invoke(null, array2);
					t = defaultIfNotExisting;
				}
				else
				{
					t = (T)((object)methodInfo.Invoke(null, array2));
				}
				if (failOnResult != null)
				{
					string text = failOnResult(t);
					if (text != null)
					{
						throw new Exception("Method " + methodInfo.FullDescription() + " returned an unexpected result: " + text);
					}
				}
			}
			catch (Exception ex)
			{
				this.ReportException(ex, methodInfo);
			}
			return t;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007874 File Offset: 0x00005A74
		private void RunMethod<S>(ref Exception exception, params object[] parameters)
		{
			MethodInfo methodInfo;
			if (this.auxilaryMethods.TryGetValue(typeof(S), out methodInfo))
			{
				object[] array = (parameters ?? new object[0]).Union<object>(new object[] { this.instance }).ToArray<object>();
				object[] array2 = AccessTools.ActualParameters(methodInfo, array);
				try
				{
					object obj = methodInfo.Invoke(null, array2);
					if (methodInfo.ReturnType == typeof(Exception))
					{
						exception = obj as Exception;
					}
				}
				catch (Exception ex)
				{
					this.ReportException(ex, methodInfo);
				}
			}
		}

		// Token: 0x04000084 RID: 132
		private readonly Harmony instance;

		// Token: 0x04000085 RID: 133
		private readonly Type containerType;

		// Token: 0x04000086 RID: 134
		private readonly HarmonyMethod containerAttributes;

		// Token: 0x04000087 RID: 135
		private readonly Dictionary<Type, MethodInfo> auxilaryMethods;

		// Token: 0x04000088 RID: 136
		private readonly List<AttributePatch> patchMethods;

		// Token: 0x04000089 RID: 137
		private static readonly List<Type> auxilaryTypes = new List<Type>(4)
		{
			typeof(HarmonyPrepare),
			typeof(HarmonyCleanup),
			typeof(HarmonyTargetMethod),
			typeof(HarmonyTargetMethods)
		};
	}
}

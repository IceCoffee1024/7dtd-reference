using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using MonoMod.Cil;
using MonoMod.Core.Platforms;
using MonoMod.Utils;

namespace HarmonyLib.Public.Patching
{
	// Token: 0x0200005D RID: 93
	public class NativeDetourMethodPatcher : MethodPatcher
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00012EA8 File Offset: 0x000110A8
		public NativeDetourMethodPatcher(MethodBase original)
			: base(original)
		{
			MethodSignature methodSignature = new MethodSignature(base.Original, true);
			this._staticSignature = methodSignature;
			MethodInfo methodInfo = NativeDetourMethodPatcher.GenerateAltEntryProxyStub(base.Original, methodSignature);
			this._altEntryMethod = methodInfo;
			this._altEntryPin = PlatformTriple.Current.PinMethodIfNeeded(methodInfo);
			PlatformTriple.Current.Compile(methodInfo);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00012F00 File Offset: 0x00011100
		private static MethodInfo GenerateAltEntryProxyStub(MethodBase original, MethodSignature signature)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
			defaultInterpolatedStringHandler.AppendLiteral("NativeDetourAltEntry<");
			Type declaringType = original.DeclaringType;
			defaultInterpolatedStringHandler.AppendFormatted((declaringType != null) ? declaringType.FullName : null);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(original.Name);
			defaultInterpolatedStringHandler.AppendLiteral(">");
			MethodInfo methodInfo;
			using (DynamicMethodDefinition dynamicMethodDefinition = signature.CreateDmd(defaultInterpolatedStringHandler.ToStringAndClear()))
			{
				ILContext ilcontext = new ILContext(dynamicMethodDefinition.Definition);
				ILCursor ilcursor = new ILCursor(ilcontext);
				ilcursor.EmitLdstr(dynamicMethodDefinition.Definition.Name + " should've been detoured!");
				ilcursor.EmitNewobj(NativeDetourMethodPatcher.InvalidOperationExceptionCtor);
				ilcursor.EmitThrow();
				methodInfo = dynamicMethodDefinition.Generate();
			}
			return methodInfo;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00012FD8 File Offset: 0x000111D8
		public override DynamicMethodDefinition PrepareOriginal()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
			defaultInterpolatedStringHandler.AppendLiteral("NativeDetourProxy<");
			Type declaringType = base.Original.DeclaringType;
			defaultInterpolatedStringHandler.AppendFormatted((declaringType != null) ? declaringType.FullName : null);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(base.Original.Name);
			defaultInterpolatedStringHandler.AppendLiteral(">");
			return this.GenerateAltEntryProxyCaller(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00013050 File Offset: 0x00011250
		public override MethodBase DetourTo(MethodBase replacement)
		{
			PlatformTriple platformTriple = PlatformTriple.Current;
			replacement = platformTriple.GetIdentifiable(replacement);
			if (replacement == this._replacement)
			{
				return replacement;
			}
			MethodInfo altEntryMethod = this._altEntryMethod;
			MethodBase methodBase;
			lock (altEntryMethod)
			{
				if (replacement == this._replacement)
				{
					methodBase = replacement;
				}
				else
				{
					this._replacement = replacement;
					MethodBase realDetourTarget = platformTriple.GetRealDetourTarget(base.Original, replacement);
					IDisposable realReplacementPin = this._realReplacementPin;
					if (realReplacementPin != null)
					{
						realReplacementPin.Dispose();
					}
					this._realReplacementPin = platformTriple.PinMethodIfNeeded(realDetourTarget);
					IntPtr functionPointer = platformTriple.Runtime.GetMethodHandle(realDetourTarget).GetFunctionPointer();
					SimpleNativeDetour detourOriginal = this._detourOriginal;
					if (detourOriginal != null)
					{
						detourOriginal.Dispose();
					}
					IDisposable altHandle = this._altHandle;
					if (altHandle != null)
					{
						altHandle.Dispose();
					}
					if (this._originalPin == null)
					{
						this._originalPin = platformTriple.PinMethodIfNeeded(base.Original);
					}
					IntPtr intPtr = this._originalEntry.GetValueOrDefault();
					IntPtr intPtr2;
					if (this._originalEntry == null)
					{
						intPtr = platformTriple.GetNativeMethodBody(base.Original);
						this._originalEntry = new IntPtr?(intPtr);
						intPtr2 = intPtr;
					}
					else
					{
						intPtr2 = intPtr;
					}
					IntPtr intPtr3 = intPtr2;
					SimpleNativeDetour simpleNativeDetour;
					IDisposable disposable;
					platformTriple.CreateNativeDetour(intPtr3, functionPointer, -1, (IntPtr)0).Deconstruct(out simpleNativeDetour, out intPtr, out disposable);
					this._detourOriginal = simpleNativeDetour;
					IntPtr intPtr4 = intPtr;
					this._altHandle = disposable;
					SimpleNativeDetour detourAltEntry = this._detourAltEntry;
					if (detourAltEntry != null)
					{
						detourAltEntry.ChangeTarget(intPtr4);
					}
					else
					{
						this._detourAltEntry = platformTriple.CreateSimpleDetour(platformTriple.GetNativeMethodBody(this._altEntryMethod), intPtr4, -1, (IntPtr)0);
					}
					PatchManager.AddReplacementOriginal(base.Original, replacement);
					methodBase = replacement;
				}
			}
			return methodBase;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00013204 File Offset: 0x00011404
		public override DynamicMethodDefinition CopyOriginal()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
			defaultInterpolatedStringHandler.AppendLiteral("OrigWrapper<");
			Type declaringType = base.Original.DeclaringType;
			defaultInterpolatedStringHandler.AppendFormatted((declaringType != null) ? declaringType.FullName : null);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(base.Original.Name);
			defaultInterpolatedStringHandler.AppendLiteral(">");
			return this.GenerateAltEntryProxyCaller(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001327C File Offset: 0x0001147C
		private DynamicMethodDefinition GenerateAltEntryProxyCaller(string name)
		{
			DynamicMethodDefinition dynamicMethodDefinition = this._staticSignature.CreateDmd(name);
			ILContext ilcontext = new ILContext(dynamicMethodDefinition.Definition);
			ILCursor ilcursor = new ILCursor(ilcontext);
			for (int i = 0; i < this._staticSignature.ParameterCount; i++)
			{
				ilcursor.EmitLdarg(i);
			}
			ilcursor.EmitCall(this._altEntryMethod);
			ilcursor.EmitRet();
			return dynamicMethodDefinition;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000132DC File Offset: 0x000114DC
		public static void TryResolve(object sender, PatchManager.PatcherResolverEventArgs args)
		{
			if (!PlatformTriple.Current.SupportedFeatures.Has(ArchitectureFeature.CreateAltEntryPoint))
			{
				return;
			}
			if (args.Original.GetMethodBody() == null)
			{
				args.MethodPatcher = new NativeDetourMethodPatcher(args.Original);
			}
		}

		// Token: 0x040000F5 RID: 245
		private static readonly ConstructorInfo InvalidOperationExceptionCtor = typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) });

		// Token: 0x040000F6 RID: 246
		private SimpleNativeDetour _detourOriginal;

		// Token: 0x040000F7 RID: 247
		private SimpleNativeDetour _detourAltEntry;

		// Token: 0x040000F8 RID: 248
		private IntPtr? _originalEntry;

		// Token: 0x040000F9 RID: 249
		private MethodBase _replacement;

		// Token: 0x040000FA RID: 250
		private IDisposable _originalPin;

		// Token: 0x040000FB RID: 251
		private IDisposable _realReplacementPin;

		// Token: 0x040000FC RID: 252
		private IDisposable _altHandle;

		// Token: 0x040000FD RID: 253
		private readonly MethodSignature _staticSignature;

		// Token: 0x040000FE RID: 254
		private readonly MethodInfo _altEntryMethod;

		// Token: 0x040000FF RID: 255
		private readonly IDisposable _altEntryPin;
	}
}

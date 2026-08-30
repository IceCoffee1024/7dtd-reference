using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x020010DF RID: 4319
public class BindingMethodCache
{
	// Token: 0x17000FD1 RID: 4049
	// (get) Token: 0x060088F6 RID: 35062 RVA: 0x00344539 File Offset: 0x00342739
	public static BindingMethodCache Instance
	{
		get
		{
			BindingMethodCache bindingMethodCache;
			if ((bindingMethodCache = BindingMethodCache.instance) == null)
			{
				bindingMethodCache = (BindingMethodCache.instance = new BindingMethodCache());
			}
			return bindingMethodCache;
		}
	}

	// Token: 0x060088F7 RID: 35063 RVA: 0x00344550 File Offset: 0x00342750
	[PublicizedFrom(EAccessModifier.Private)]
	public BindingMethodCache.BindingMethodTypeCache getCacheForType(Type _type)
	{
		BindingMethodCache.BindingMethodTypeCache bindingMethodTypeCache;
		if (!this.cachePerType.TryGetValue(_type, out bindingMethodTypeCache))
		{
			this.mswInit.Start();
			bindingMethodTypeCache = new BindingMethodCache.BindingMethodTypeCache(_type, this);
			this.mswInit.Stop();
			this.cachePerType[_type] = bindingMethodTypeCache;
		}
		return bindingMethodTypeCache;
	}

	// Token: 0x060088F8 RID: 35064 RVA: 0x0034459C File Offset: 0x0034279C
	public void RegisterCustomBindingMethod(string _bindingName, MethodInfo _method)
	{
		if (string.IsNullOrEmpty(_bindingName))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Registering custom binding method for '",
				_bindingName,
				"' failed: Binding name has to be non-empty. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
		}
		if (!_method.IsStatic)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Registering custom binding method for '",
				_bindingName,
				"' failed: Method has to be static. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
			return;
		}
		ParameterInfo[] parameters = _method.GetParameters();
		if (parameters.Length != 1 || !typeof(XUiController).IsAssignableFrom(parameters[0].ParameterType))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Registering custom binding method for '",
				_bindingName,
				"' failed: Methods have to take exactly one parameter with the type inheriting from XUiController. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
			return;
		}
		Type parameterType = parameters[0].ParameterType;
		XuiBindingDelegate xuiBindingDelegate;
		if (!BindingMethodCache.tryGetBindingObjectDelegate(_method, out xuiBindingDelegate))
		{
			return;
		}
		Dictionary<string, XuiBindingDelegate> dictionary;
		if (!this.customBindingDelegates.TryGetValue(parameterType, out dictionary))
		{
			dictionary = new Dictionary<string, XuiBindingDelegate>();
			this.customBindingDelegates[parameterType] = dictionary;
		}
		if (!dictionary.TryAdd(_bindingName, xuiBindingDelegate))
		{
			Log.Warning(string.Concat(new string[]
			{
				"[XUi] Registering custom binding method for '",
				_bindingName,
				"' failed: Custom binding with that name already registered. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
		}
	}

	// Token: 0x060088F9 RID: 35065 RVA: 0x0034472E File Offset: 0x0034292E
	public bool TryGetBindingDelegate(XUiController _controller, string _bindingName, out XuiBindingDelegate _bindingDelegate)
	{
		return this.getCacheForType(_controller.GetType()).TryGetBindingDelegate(_bindingName, out _bindingDelegate);
	}

	// Token: 0x060088FA RID: 35066 RVA: 0x00344743 File Offset: 0x00342943
	public void LogInitTotal()
	{
		Log.Out(string.Format("[XUi] BindingMethodCache total init time: {0:F3} ms", (float)this.mswInit.ElapsedMicroseconds / 1000f));
	}

	// Token: 0x060088FB RID: 35067 RVA: 0x0034476C File Offset: 0x0034296C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryGetBindingObjectDelegate(MethodInfo _method, out XuiBindingDelegate _func)
	{
		_func = null;
		if (_method == null)
		{
			Log.Error("[XUi] Failed creating binding method wrapper: MethodInfo null");
			return false;
		}
		Type returnType = _method.ReturnType;
		if (returnType == typeof(void))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating binding method wrapper: Method has no return value (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		Type type = (_method.IsStatic ? _method.GetParameters()[0].ParameterType : _method.DeclaringType);
		MethodInfo methodInfo;
		if (returnType == typeof(bool))
		{
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreBool|16_1<XUiController>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type });
		}
		else if (returnType == typeof(int))
		{
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreInt|16_2<XUiController>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type });
		}
		else
		{
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreGeneric|16_0<XUiController, object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type, returnType });
		}
		_func = (XuiBindingDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x060088FE RID: 35070 RVA: 0x0034491D File Offset: 0x00342B1D
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreGeneric|16_0<TController, TValue>(MethodInfo _methodInner) where TController : XUiController
	{
		Func<TController, TValue> typedFunc = (Func<TController, TValue>)Delegate.CreateDelegate(typeof(Func<TController, TValue>), _methodInner);
		return (XUiController _instance) => typedFunc((TController)((object)_instance));
	}

	// Token: 0x060088FF RID: 35071 RVA: 0x0034494A File Offset: 0x00342B4A
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreBool|16_1<TController>(MethodInfo _methodInner) where TController : XUiController
	{
		Func<TController, bool> typedFunc = (Func<TController, bool>)Delegate.CreateDelegate(typeof(Func<TController, bool>), _methodInner);
		return delegate(XUiController _instance)
		{
			if (!typedFunc((TController)((object)_instance)))
			{
				return BindingMethodCache.boxedFalse;
			}
			return BindingMethodCache.boxedTrue;
		};
	}

	// Token: 0x06008900 RID: 35072 RVA: 0x00344977 File Offset: 0x00342B77
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreInt|16_2<TController>(MethodInfo _methodInner) where TController : XUiController
	{
		Func<TController, int> typedFunc = (Func<TController, int>)Delegate.CreateDelegate(typeof(Func<TController, int>), _methodInner);
		return delegate(XUiController _instance)
		{
			int num = typedFunc((TController)((object)_instance));
			object obj;
			if (num != 0)
			{
				if (num != 1)
				{
					obj = num;
				}
				else
				{
					obj = BindingMethodCache.boxed1;
				}
			}
			else
			{
				obj = BindingMethodCache.boxed0;
			}
			return obj;
		};
	}

	// Token: 0x040065D4 RID: 26068
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly MicroStopwatch mswInit = new MicroStopwatch(false);

	// Token: 0x040065D5 RID: 26069
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, BindingMethodCache.BindingMethodTypeCache> cachePerType = new Dictionary<Type, BindingMethodCache.BindingMethodTypeCache>(128);

	// Token: 0x040065D6 RID: 26070
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, Dictionary<string, XuiBindingDelegate>> customBindingDelegates = new Dictionary<Type, Dictionary<string, XuiBindingDelegate>>();

	// Token: 0x040065D7 RID: 26071
	[PublicizedFrom(EAccessModifier.Private)]
	public static BindingMethodCache instance;

	// Token: 0x040065D8 RID: 26072
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxed0 = 0;

	// Token: 0x040065D9 RID: 26073
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxed1 = 1;

	// Token: 0x040065DA RID: 26074
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedFalse = false;

	// Token: 0x040065DB RID: 26075
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedTrue = true;

	// Token: 0x020010E0 RID: 4320
	public class BindingMethodData
	{
		// Token: 0x06008901 RID: 35073 RVA: 0x003449A4 File Offset: 0x00342BA4
		public BindingMethodData(string _definingController, XuiBindingDelegate _delegate)
		{
			this.DefiningController = _definingController;
			this.Delegate = _delegate;
		}

		// Token: 0x040065DC RID: 26076
		public readonly string DefiningController;

		// Token: 0x040065DD RID: 26077
		public readonly XuiBindingDelegate Delegate;
	}

	// Token: 0x020010E1 RID: 4321
	public class BindingMethodTypeCache
	{
		// Token: 0x06008902 RID: 35074 RVA: 0x003449BA File Offset: 0x00342BBA
		public BindingMethodTypeCache(Type _controllerType, BindingMethodCache _owner)
		{
			this.controllerType = _controllerType;
			this.initCacheForController(_owner);
		}

		// Token: 0x06008903 RID: 35075 RVA: 0x003449E4 File Offset: 0x00342BE4
		[PublicizedFrom(EAccessModifier.Private)]
		public void initCacheForController(BindingMethodCache _owner)
		{
			if (this.controllerType.BaseType != null)
			{
				_owner.getCacheForType(this.controllerType.BaseType).methods.CopyTo<string, BindingMethodCache.BindingMethodData>(this.methods, false);
			}
			ReflectionHelpers.GetMethodsWithAttribute<XuiXmlBindingAttribute>(this.controllerType, new Action<MethodInfo, bool, XuiXmlBindingAttribute>(this.<initCacheForController>g__MethodFoundCallback|3_1), true, true, true);
			ReflectionHelpers.GetPropertiesWithAttribute<XuiXmlBindingAttribute>(this.controllerType, new Action<PropertyInfo, bool, XuiXmlBindingAttribute>(this.<initCacheForController>g__PropertyFoundCallback|3_0), true, true, true);
			Dictionary<string, XuiBindingDelegate> dictionary;
			if (_owner.customBindingDelegates.TryGetValue(this.controllerType, out dictionary))
			{
				foreach (KeyValuePair<string, XuiBindingDelegate> keyValuePair in dictionary)
				{
					string text;
					XuiBindingDelegate xuiBindingDelegate;
					keyValuePair.Deconstruct(out text, out xuiBindingDelegate);
					string text2 = text;
					XuiBindingDelegate xuiBindingDelegate2 = xuiBindingDelegate;
					BindingMethodCache.BindingMethodData bindingMethodData;
					if (this.methods.TryGetValue(text2, out bindingMethodData))
					{
						Log.Warning(string.Concat(new string[] { "[XUi] Custom binding overriding overriding existing (", bindingMethodData.DefiningController, ") binding definition of '", text2, "'" }));
					}
					this.methods[text2] = new BindingMethodCache.BindingMethodData("<Custom>", xuiBindingDelegate2);
				}
			}
		}

		// Token: 0x06008904 RID: 35076 RVA: 0x00344B1C File Offset: 0x00342D1C
		public bool TryGetBindingDelegate(string _bindingName, out XuiBindingDelegate _bindingDelegate)
		{
			BindingMethodCache.BindingMethodData bindingMethodData;
			if (this.methods.TryGetValue(_bindingName, out bindingMethodData))
			{
				_bindingDelegate = bindingMethodData.Delegate;
				return true;
			}
			_bindingDelegate = null;
			return false;
		}

		// Token: 0x06008905 RID: 35077 RVA: 0x00344B48 File Offset: 0x00342D48
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__PropertyFoundCallback|3_0(PropertyInfo _property, bool _hasMultiple, XuiXmlBindingAttribute _bindingAttribute)
		{
			MethodInfo getMethod = _property.GetGetMethod(true);
			if (getMethod == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"[XUi] Failed creating binding property wrapper: Property has no getter (",
					_property.DeclaringType.FullName,
					".",
					_property.Name,
					")"
				}));
			}
			this.<initCacheForController>g__MethodFoundCallback|3_1(getMethod, _hasMultiple, _bindingAttribute);
		}

		// Token: 0x06008906 RID: 35078 RVA: 0x00344BB0 File Offset: 0x00342DB0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__MethodFoundCallback|3_1(MethodInfo _method, bool _hasMultiple, XuiXmlBindingAttribute _bindingAttribute)
		{
			if (_method.IsStatic)
			{
				Log.Error("[XUi] XML binding has to be non-static! " + _method.DeclaringType.FullName + "." + _method.Name);
				return;
			}
			if (_method.GetParameters().Length != 0)
			{
				Log.Error(string.Concat(new string[]
				{
					"[XUi] Failed creating binding method wrapper: Method has parameters (",
					_method.DeclaringType.FullName,
					".",
					_method.Name,
					")"
				}));
				return;
			}
			XuiBindingDelegate xuiBindingDelegate;
			if (!BindingMethodCache.tryGetBindingObjectDelegate(_method, out xuiBindingDelegate))
			{
				return;
			}
			BindingMethodCache.BindingMethodData bindingMethodData = new BindingMethodCache.BindingMethodData(_method.DeclaringType.FullName, xuiBindingDelegate);
			BindingMethodCache.BindingMethodData bindingMethodData2;
			if (this.methods.TryGetValue(_bindingAttribute.BindingName, out bindingMethodData2))
			{
				Log.Warning(string.Concat(new string[] { "[XUi] Child class (", bindingMethodData.DefiningController, ") overriding parent (", bindingMethodData2.DefiningController, ") binding definition of '", _bindingAttribute.BindingName, "'" }));
			}
			this.methods[_bindingAttribute.BindingName] = bindingMethodData;
		}

		// Token: 0x040065DE RID: 26078
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerType;

		// Token: 0x040065DF RID: 26079
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, BindingMethodCache.BindingMethodData> methods = new Dictionary<string, BindingMethodCache.BindingMethodData>(64, StringComparer.Ordinal);
	}
}

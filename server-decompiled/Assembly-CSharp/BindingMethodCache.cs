using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200111A RID: 4378
public class BindingMethodCache
{
	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x06008A92 RID: 35474 RVA: 0x0034EE35 File Offset: 0x0034D035
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

	// Token: 0x06008A93 RID: 35475 RVA: 0x0034EE4C File Offset: 0x0034D04C
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

	// Token: 0x06008A94 RID: 35476 RVA: 0x0034EE98 File Offset: 0x0034D098
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
		XuiBindingBoxWriterDelegate xuiBindingBoxWriterDelegate;
		Func<object> func;
		if (!BindingMethodCache.tryGetBindingObjectDelegate(_method, out xuiBindingDelegate, out xuiBindingBoxWriterDelegate, out func))
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

	// Token: 0x06008A95 RID: 35477 RVA: 0x0034F02E File Offset: 0x0034D22E
	public bool TryGetBindingDelegate(XUiController _controller, string _bindingName, out XuiBindingDelegate _bindingDelegate)
	{
		return this.getCacheForType(_controller.GetType()).TryGetBindingDelegate(_bindingName, out _bindingDelegate);
	}

	// Token: 0x06008A96 RID: 35478 RVA: 0x0034F043 File Offset: 0x0034D243
	public bool TryGetBindingData(XUiController _controller, string _bindingName, out BindingMethodCache.BindingMethodData _data)
	{
		return this.getCacheForType(_controller.GetType()).TryGetBindingData(_bindingName, out _data);
	}

	// Token: 0x06008A97 RID: 35479 RVA: 0x0034F058 File Offset: 0x0034D258
	public void LogInitTotal()
	{
		Log.Out(string.Format("[XUi] BindingMethodCache total init time: {0:F3} ms", (float)this.mswInit.ElapsedMicroseconds / 1000f));
	}

	// Token: 0x06008A98 RID: 35480 RVA: 0x0034F080 File Offset: 0x0034D280
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryGetBindingObjectDelegate(MethodInfo _method, out XuiBindingDelegate _func, out XuiBindingBoxWriterDelegate _boxWriter, out Func<object> _createBox)
	{
		_func = null;
		_boxWriter = null;
		_createBox = null;
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
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreBool|18_1<XUiController>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type });
		}
		else if (returnType == typeof(int))
		{
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreInt|18_2<XUiController>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type });
		}
		else if (returnType.IsEnum)
		{
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreEnum|18_3<XUiController, int>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type, returnType });
		}
		else
		{
			methodInfo = new Func<MethodInfo, XuiBindingDelegate>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreGeneric|18_0<XUiController, object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type, returnType });
		}
		_func = (XuiBindingDelegate)methodInfo.Invoke(null, new object[] { _method });
		if (BindingMethodCache.reusableBoxStructTypes.Contains(returnType))
		{
			ValueTuple<XuiBindingBoxWriterDelegate, Func<object>> valueTuple = (ValueTuple<XuiBindingBoxWriterDelegate, Func<object>>)new Func<MethodInfo, ValueTuple<XuiBindingBoxWriterDelegate, Func<object>>>(BindingMethodCache.<tryGetBindingObjectDelegate>g__GetBindingBoxWriterCore|18_4<XUiController, int>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type, returnType }).Invoke(null, new object[] { _method });
			XuiBindingBoxWriterDelegate item = valueTuple.Item1;
			Func<object> item2 = valueTuple.Item2;
			_boxWriter = item;
			_createBox = item2;
		}
		return true;
	}

	// Token: 0x06008A9B RID: 35483 RVA: 0x0034F38D File Offset: 0x0034D58D
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreGeneric|18_0<TController, TValue>(MethodInfo _methodInner) where TController : XUiController
	{
		Func<TController, TValue> typedFunc = (Func<TController, TValue>)Delegate.CreateDelegate(typeof(Func<TController, TValue>), _methodInner);
		return (XUiController _instance) => typedFunc((TController)((object)_instance));
	}

	// Token: 0x06008A9C RID: 35484 RVA: 0x0034F3BA File Offset: 0x0034D5BA
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreBool|18_1<TController>(MethodInfo _methodInner) where TController : XUiController
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

	// Token: 0x06008A9D RID: 35485 RVA: 0x0034F3E7 File Offset: 0x0034D5E7
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreInt|18_2<TController>(MethodInfo _methodInner) where TController : XUiController
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

	// Token: 0x06008A9E RID: 35486 RVA: 0x0034F414 File Offset: 0x0034D614
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiBindingDelegate <tryGetBindingObjectDelegate>g__GetBindingObjectDelegateCoreEnum|18_3<TController, TEnum>(MethodInfo _methodInner) where TController : XUiController
	{
		Func<TController, TEnum> typedFunc = (Func<TController, TEnum>)Delegate.CreateDelegate(typeof(Func<TController, TEnum>), _methodInner);
		return (XUiController _instance) => EnumBoxCache<TEnum>.Instance.GetBoxed(typedFunc((TController)((object)_instance)));
	}

	// Token: 0x06008A9F RID: 35487 RVA: 0x0034F444 File Offset: 0x0034D644
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public unsafe static ValueTuple<XuiBindingBoxWriterDelegate, Func<object>> <tryGetBindingObjectDelegate>g__GetBindingBoxWriterCore|18_4<TController, TValue>(MethodInfo _methodInner) where TController : XUiController where TValue : struct
	{
		Func<TController, TValue> typedFunc = (Func<TController, TValue>)Delegate.CreateDelegate(typeof(Func<TController, TValue>), _methodInner);
		XuiBindingBoxWriterDelegate xuiBindingBoxWriterDelegate = delegate(XUiController _instance, object _box)
		{
			*Unsafe.Unbox<TValue>(_box) = typedFunc((TController)((object)_instance));
		};
		Func<object> func = () => default(TValue);
		return new ValueTuple<XuiBindingBoxWriterDelegate, Func<object>>(xuiBindingBoxWriterDelegate, func);
	}

	// Token: 0x04006724 RID: 26404
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly MicroStopwatch mswInit = new MicroStopwatch(false);

	// Token: 0x04006725 RID: 26405
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, BindingMethodCache.BindingMethodTypeCache> cachePerType = new Dictionary<Type, BindingMethodCache.BindingMethodTypeCache>(128);

	// Token: 0x04006726 RID: 26406
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, Dictionary<string, XuiBindingDelegate>> customBindingDelegates = new Dictionary<Type, Dictionary<string, XuiBindingDelegate>>();

	// Token: 0x04006727 RID: 26407
	[PublicizedFrom(EAccessModifier.Private)]
	public static BindingMethodCache instance;

	// Token: 0x04006728 RID: 26408
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxed0 = 0;

	// Token: 0x04006729 RID: 26409
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxed1 = 1;

	// Token: 0x0400672A RID: 26410
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedFalse = false;

	// Token: 0x0400672B RID: 26411
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedTrue = true;

	// Token: 0x0400672C RID: 26412
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly HashSet<Type> reusableBoxStructTypes = new HashSet<Type>
	{
		typeof(Color),
		typeof(Color32),
		typeof(Vector2),
		typeof(Vector3),
		typeof(Vector4),
		typeof(Quaternion),
		typeof(Vector2i),
		typeof(Vector3i),
		typeof(Vector2Int),
		typeof(Vector3Int)
	};

	// Token: 0x0200111B RID: 4379
	public class BindingMethodData
	{
		// Token: 0x06008AA0 RID: 35488 RVA: 0x0034F493 File Offset: 0x0034D693
		public BindingMethodData(string _definingController, XuiBindingDelegate _delegate, XuiBindingBoxWriterDelegate _boxWriter = null, Func<object> _createBox = null)
		{
			this.DefiningController = _definingController;
			this.Delegate = _delegate;
			this.BoxWriter = _boxWriter;
			this.CreateBox = _createBox;
		}

		// Token: 0x0400672D RID: 26413
		public readonly string DefiningController;

		// Token: 0x0400672E RID: 26414
		public readonly XuiBindingDelegate Delegate;

		// Token: 0x0400672F RID: 26415
		public readonly XuiBindingBoxWriterDelegate BoxWriter;

		// Token: 0x04006730 RID: 26416
		public readonly Func<object> CreateBox;
	}

	// Token: 0x0200111C RID: 4380
	public class BindingMethodTypeCache
	{
		// Token: 0x06008AA1 RID: 35489 RVA: 0x0034F4B8 File Offset: 0x0034D6B8
		public BindingMethodTypeCache(Type _controllerType, BindingMethodCache _owner)
		{
			this.controllerType = _controllerType;
			this.initCacheForController(_owner);
		}

		// Token: 0x06008AA2 RID: 35490 RVA: 0x0034F4E0 File Offset: 0x0034D6E0
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
					this.methods[text2] = new BindingMethodCache.BindingMethodData("<Custom>", xuiBindingDelegate2, null, null);
				}
			}
		}

		// Token: 0x06008AA3 RID: 35491 RVA: 0x0034F61C File Offset: 0x0034D81C
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

		// Token: 0x06008AA4 RID: 35492 RVA: 0x0034F647 File Offset: 0x0034D847
		public bool TryGetBindingData(string _bindingName, out BindingMethodCache.BindingMethodData _data)
		{
			return this.methods.TryGetValue(_bindingName, out _data);
		}

		// Token: 0x06008AA5 RID: 35493 RVA: 0x0034F658 File Offset: 0x0034D858
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

		// Token: 0x06008AA6 RID: 35494 RVA: 0x0034F6C0 File Offset: 0x0034D8C0
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
			XuiBindingBoxWriterDelegate xuiBindingBoxWriterDelegate;
			Func<object> func;
			if (!BindingMethodCache.tryGetBindingObjectDelegate(_method, out xuiBindingDelegate, out xuiBindingBoxWriterDelegate, out func))
			{
				return;
			}
			BindingMethodCache.BindingMethodData bindingMethodData = new BindingMethodCache.BindingMethodData(_method.DeclaringType.FullName, xuiBindingDelegate, xuiBindingBoxWriterDelegate, func);
			BindingMethodCache.BindingMethodData bindingMethodData2;
			if (this.methods.TryGetValue(_bindingAttribute.BindingName, out bindingMethodData2))
			{
				Log.Warning(string.Concat(new string[] { "[XUi] Child class (", bindingMethodData.DefiningController, ") overriding parent (", bindingMethodData2.DefiningController, ") binding definition of '", _bindingAttribute.BindingName, "'" }));
			}
			this.methods[_bindingAttribute.BindingName] = bindingMethodData;
		}

		// Token: 0x04006731 RID: 26417
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerType;

		// Token: 0x04006732 RID: 26418
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, BindingMethodCache.BindingMethodData> methods = new Dictionary<string, BindingMethodCache.BindingMethodData>(64, StringComparer.Ordinal);
	}
}

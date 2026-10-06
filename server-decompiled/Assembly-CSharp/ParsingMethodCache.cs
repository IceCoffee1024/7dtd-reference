using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x02001124 RID: 4388
public class ParsingMethodCache
{
	// Token: 0x17001043 RID: 4163
	// (get) Token: 0x06008AD4 RID: 35540 RVA: 0x0034F55D File Offset: 0x0034D75D
	public static ParsingMethodCache Instance
	{
		get
		{
			ParsingMethodCache parsingMethodCache;
			if ((parsingMethodCache = ParsingMethodCache.instance) == null)
			{
				parsingMethodCache = (ParsingMethodCache.instance = new ParsingMethodCache());
			}
			return parsingMethodCache;
		}
	}

	// Token: 0x06008AD5 RID: 35541 RVA: 0x0034F574 File Offset: 0x0034D774
	[PublicizedFrom(EAccessModifier.Private)]
	public ParsingMethodCache()
	{
		this.genericEnumTryParseMethod = new ParsingMethodCache.RawConverterFromStringDelegate<int>(EnumUtils.TryParseIgnoreCase<int>).Method.GetGenericMethodDefinition();
		ReflectionHelpers.FindTypesWithAttribute<XuiXmlAttributeConvertersClassAttribute>(new Action<Type, bool, XuiXmlAttributeConvertersClassAttribute>(this.<.ctor>g__TypeFoundCallback|11_0), true);
	}

	// Token: 0x06008AD6 RID: 35542 RVA: 0x0034F5F0 File Offset: 0x0034D7F0
	[PublicizedFrom(EAccessModifier.Private)]
	public ParsingMethodCache.ParsingMethodTypeCache getCacheForType(Type _type)
	{
		ParsingMethodCache.ParsingMethodTypeCache parsingMethodTypeCache;
		if (!this.cachePerType.TryGetValue(_type, out parsingMethodTypeCache))
		{
			this.mswInit.Start();
			parsingMethodTypeCache = new ParsingMethodCache.ParsingMethodTypeCache(_type, this);
			this.mswInit.Stop();
			this.cachePerType[_type] = parsingMethodTypeCache;
		}
		return parsingMethodTypeCache;
	}

	// Token: 0x06008AD7 RID: 35543 RVA: 0x0034F63C File Offset: 0x0034D83C
	public void RegisterCustomParsingMethod(string _attributeName, MethodInfo _method)
	{
		if (string.IsNullOrEmpty(_attributeName))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Registering custom parsing method for '",
				_attributeName,
				"' failed: Attribute name has to be non-empty. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
		}
		if (!_method.IsStatic)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Registering custom parsing method for '",
				_attributeName,
				"' failed: Method has to be static. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
			return;
		}
		ParameterInfo[] parameters = _method.GetParameters();
		if (parameters.Length != 2 || !typeof(IXUiElement).IsAssignableFrom(parameters[0].ParameterType))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Registering custom parsing method for '",
				_attributeName,
				"' failed: Methods have to take exactly two parameters (first one with the type inheriting from IXUiElement). Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
			return;
		}
		Type parameterType = parameters[0].ParameterType;
		XuiParsingDelegate xuiParsingDelegate;
		Type type;
		if (!ParsingMethodCache.tryGetParsingObjectDelegate(_method, out xuiParsingDelegate, out type))
		{
			return;
		}
		Dictionary<string, ValueTuple<XuiParsingDelegate, Type>> dictionary;
		if (!this.customParsingDelegates.TryGetValue(parameterType, out dictionary))
		{
			dictionary = new Dictionary<string, ValueTuple<XuiParsingDelegate, Type>>();
			this.customParsingDelegates[parameterType] = dictionary;
		}
		if (!dictionary.TryAdd(_attributeName, new ValueTuple<XuiParsingDelegate, Type>(xuiParsingDelegate, type)))
		{
			Log.Warning(string.Concat(new string[]
			{
				"[XUi] Registering custom parsing method for '",
				_attributeName,
				"' failed: Custom parser with that name already registered. Method: ",
				_method.DeclaringType.FullName,
				".",
				_method.Name
			}));
		}
	}

	// Token: 0x06008AD8 RID: 35544 RVA: 0x0034F7D9 File Offset: 0x0034D9D9
	public bool TryGetParsingDelegate(IXUiElement _target, string _attributeName, out ParsingMethodCache.ParsingMethodData _parsingDelegate)
	{
		return this.getCacheForType(_target.GetType()).TryGetParsingDelegate(_attributeName, out _parsingDelegate);
	}

	// Token: 0x06008AD9 RID: 35545 RVA: 0x0034F7F0 File Offset: 0x0034D9F0
	public bool TryParseDirect(IXUiElement _target, string _attributeName, string _value)
	{
		ParsingMethodCache.ParsingMethodData parsingMethodData;
		if (!this.TryGetParsingDelegate(_target, _attributeName, out parsingMethodData))
		{
			return false;
		}
		XuiParsingDelegate xuiParsingDelegate;
		if (!parsingMethodData.TryGetDelegateForSourceType(typeof(string), out xuiParsingDelegate))
		{
			Log.Warning(string.Concat(new string[]
			{
				"[XUi] Found attribute parser but no suitable converter from string input. Attribute: ",
				_attributeName,
				", target type: ",
				parsingMethodData.NativeParseType.FullName,
				" on ",
				_target.GetType().FullName
			}));
			return true;
		}
		try
		{
			xuiParsingDelegate(_target, _value);
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Can not parse input ('",
				_value,
				"') into target type ",
				parsingMethodData.NativeParseType.FullName,
				" for attribute '",
				_attributeName,
				"' on ",
				_target.GetType().FullName,
				": Conversion to target type failed"
			}));
			Log.Exception(ex);
		}
		return true;
	}

	// Token: 0x06008ADA RID: 35546 RVA: 0x0034F8EC File Offset: 0x0034DAEC
	public bool TryGetTypeConverterFromString(Type _outputType, out XuiParsingConverterFromStringDelegate _converterDelegate)
	{
		if (this.convertersFromString.TryGetValue(_outputType, out _converterDelegate))
		{
			return true;
		}
		if (!_outputType.IsEnum)
		{
			return false;
		}
		Type type;
		if (!ParsingMethodCache.tryGetParsingConverterFromStringDelegate(this.genericEnumTryParseMethod.MakeGenericMethod(new Type[] { _outputType }), out _converterDelegate, out type))
		{
			return false;
		}
		this.convertersFromString[type] = _converterDelegate;
		return true;
	}

	// Token: 0x06008ADB RID: 35547 RVA: 0x0034F944 File Offset: 0x0034DB44
	public bool TryGetTypeConverterFromType(Type _outputType, Type _inputType, out XuiParsingConverterTypeToTypeDelegate _converterDelegate)
	{
		Dictionary<Type, XuiParsingConverterTypeToTypeDelegate> dictionary;
		if (!this.convertersTypeToType.TryGetValue(_outputType, out dictionary))
		{
			_converterDelegate = null;
			return false;
		}
		return dictionary.TryGetValue(_inputType, out _converterDelegate);
	}

	// Token: 0x06008ADC RID: 35548 RVA: 0x0034F96E File Offset: 0x0034DB6E
	public void LogInitTotal()
	{
		Log.Out(string.Format("[XUi] ParsingMethodCache total init time: {0:F3} ms", (float)this.mswInit.ElapsedMicroseconds / 1000f));
	}

	// Token: 0x06008ADD RID: 35549 RVA: 0x0034F996 File Offset: 0x0034DB96
	[PublicizedFrom(EAccessModifier.Private)]
	public static object boxBool(bool _value)
	{
		if (!_value)
		{
			return ParsingMethodCache.boxedFalse;
		}
		return ParsingMethodCache.boxedTrue;
	}

	// Token: 0x06008ADE RID: 35550 RVA: 0x0034F9A8 File Offset: 0x0034DBA8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryGetParsingObjectDelegate(MethodInfo _method, out XuiParsingDelegate _func, out Type _inputType)
	{
		_func = null;
		_inputType = null;
		if (_method == null)
		{
			Log.Error("[XUi] Failed creating parsing method wrapper: MethodInfo null");
			return false;
		}
		if (_method.ReturnType != typeof(void))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing method wrapper: Method has return value (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		ParameterInfo[] parameters = _method.GetParameters();
		if (parameters.Length != (_method.IsStatic ? 2 : 1))
		{
			Log.Error(string.Format("[XUi] Failed creating parsing method wrapper: Method has invalid number of parameters, must be {0} ({1}.{2})", _method.IsStatic ? 2 : 1, _method.DeclaringType.FullName, _method.Name));
			return false;
		}
		Type type = (_method.IsStatic ? _method.GetParameters()[0].ParameterType : _method.DeclaringType);
		_inputType = parameters[_method.IsStatic ? 1 : 0].ParameterType;
		MethodInfo methodInfo = new Func<MethodInfo, XuiParsingDelegate>(ParsingMethodCache.<tryGetParsingObjectDelegate>g__GetParsingObjectDelegateCore|22_0<XUiController, object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type, _inputType });
		_func = (XuiParsingDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x06008ADF RID: 35551 RVA: 0x0034FAEC File Offset: 0x0034DCEC
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryGetParsingConverterFromStringDelegate(MethodInfo _method, out XuiParsingConverterFromStringDelegate _func, out Type _outputType)
	{
		_func = null;
		_outputType = null;
		if (_method == null)
		{
			Log.Error("[XUi] Failed creating parsing converter method wrapper: MethodInfo null");
			return false;
		}
		if (_method.ReturnType != typeof(bool))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method has no bool return value (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		ParameterInfo[] parameters = _method.GetParameters();
		if (parameters.Length != 2)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method has invalid number of parameters, must be 2 (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		if (parameters[0].ParameterType != typeof(string))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method has invalid first parameter, must be string (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		if (!parameters[1].IsOut)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method second parameter must be declared 'out' (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		_outputType = parameters[1].ParameterType.GetElementType();
		MethodInfo methodInfo = new Func<MethodInfo, XuiParsingConverterFromStringDelegate>(ParsingMethodCache.<tryGetParsingConverterFromStringDelegate>g__GetParsingConverterDelegateCore|23_0<object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { _outputType });
		_func = (XuiParsingConverterFromStringDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x06008AE0 RID: 35552 RVA: 0x0034FCB0 File Offset: 0x0034DEB0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryGetParsingConverterTypeToTypeDelegate(MethodInfo _method, out XuiParsingConverterTypeToTypeDelegate _func, out Type _outputType, out Type _inputType)
	{
		_func = null;
		_outputType = null;
		_inputType = null;
		if (_method == null)
		{
			Log.Error("[XUi] Failed creating parsing converter method wrapper: MethodInfo null");
			return false;
		}
		if (_method.ReturnType != typeof(bool))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method has no bool return value (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		ParameterInfo[] parameters = _method.GetParameters();
		if (parameters.Length != 2)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method has invalid number of parameters, must be 2 (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		if (!parameters[1].IsOut)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed creating parsing converter method wrapper: Method second parameter must be declared 'out' (",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				")"
			}));
			return false;
		}
		_inputType = parameters[0].ParameterType;
		_outputType = parameters[1].ParameterType.GetElementType();
		MethodInfo methodInfo = new Func<MethodInfo, XuiParsingConverterTypeToTypeDelegate>(ParsingMethodCache.<tryGetParsingConverterTypeToTypeDelegate>g__GetParsingConverterDelegateCore|25_0<object, object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { _outputType, _inputType });
		_func = (XuiParsingConverterTypeToTypeDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x06008AE2 RID: 35554 RVA: 0x0034FE42 File Offset: 0x0034E042
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <.ctor>g__TypeFoundCallback|11_0(Type _type, bool _hasMultiple, XuiXmlAttributeConvertersClassAttribute _xmlParsingConvertersClassAttribute)
	{
		ReflectionHelpers.GetMethodsWithAttribute<XuiXmlAttributeConverterAttribute>(_type, new Action<MethodInfo, bool, XuiXmlAttributeConverterAttribute>(this.<.ctor>g__MethodFoundCallback|11_1), true, false, true);
	}

	// Token: 0x06008AE3 RID: 35555 RVA: 0x0034FE5C File Offset: 0x0034E05C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <.ctor>g__MethodFoundCallback|11_1(MethodInfo _method, bool _hasMultiple, XuiXmlAttributeConverterAttribute _xmlParsingConverterAttribute)
	{
		if (_method.GetParameters().Length < 1 || !(_method.GetParameters()[0].ParameterType == typeof(string)))
		{
			XuiParsingConverterTypeToTypeDelegate xuiParsingConverterTypeToTypeDelegate;
			Type type;
			Type type2;
			ParsingMethodCache.tryGetParsingConverterTypeToTypeDelegate(_method, out xuiParsingConverterTypeToTypeDelegate, out type, out type2);
			Dictionary<Type, XuiParsingConverterTypeToTypeDelegate> dictionary;
			if (!this.convertersTypeToType.TryGetValue(type, out dictionary))
			{
				dictionary = new Dictionary<Type, XuiParsingConverterTypeToTypeDelegate>();
				this.convertersTypeToType[type] = dictionary;
			}
			if (!dictionary.TryAdd(type2, xuiParsingConverterTypeToTypeDelegate))
			{
				Log.Warning(string.Concat(new string[]
				{
					"[XUi] Input type (",
					type2.FullName,
					") -> output type (",
					type.FullName,
					") converter already defined before. New implementation: ",
					_method.DeclaringType.FullName,
					".",
					_method.Name
				}));
			}
			return;
		}
		XuiParsingConverterFromStringDelegate xuiParsingConverterFromStringDelegate;
		Type type3;
		if (!ParsingMethodCache.tryGetParsingConverterFromStringDelegate(_method, out xuiParsingConverterFromStringDelegate, out type3))
		{
			return;
		}
		this.convertersFromString[type3] = xuiParsingConverterFromStringDelegate;
	}

	// Token: 0x06008AE4 RID: 35556 RVA: 0x0034FF45 File Offset: 0x0034E145
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiParsingDelegate <tryGetParsingObjectDelegate>g__GetParsingObjectDelegateCore|22_0<TInstance, TValue>(MethodInfo _methodInner) where TInstance : class
	{
		Action<TInstance, TValue> typedFunc = (Action<TInstance, TValue>)Delegate.CreateDelegate(typeof(Action<TInstance, TValue>), _methodInner);
		return delegate(IXUiElement _instance, object _value)
		{
			typedFunc((TInstance)((object)_instance), (TValue)((object)_value));
		};
	}

	// Token: 0x06008AE5 RID: 35557 RVA: 0x0034FF74 File Offset: 0x0034E174
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiParsingConverterFromStringDelegate <tryGetParsingConverterFromStringDelegate>g__GetParsingConverterDelegateCore|23_0<TValue>(MethodInfo _methodInner)
	{
		if (typeof(TValue) == typeof(bool))
		{
			ParsingMethodCache.RawConverterFromStringDelegate<bool> boolFunc = (ParsingMethodCache.RawConverterFromStringDelegate<bool>)Delegate.CreateDelegate(typeof(ParsingMethodCache.RawConverterFromStringDelegate<bool>), _methodInner);
			return delegate(string _inputValue, out object _outputValue)
			{
				bool flag2;
				bool flag = boolFunc(_inputValue, out flag2);
				_outputValue = ParsingMethodCache.boxBool(flag2);
				return flag;
			};
		}
		ParsingMethodCache.RawConverterFromStringDelegate<TValue> typedFunc = (ParsingMethodCache.RawConverterFromStringDelegate<TValue>)Delegate.CreateDelegate(typeof(ParsingMethodCache.RawConverterFromStringDelegate<TValue>), _methodInner);
		if (typeof(TValue).IsEnum)
		{
			return delegate(string _inputValue, out object _outputValue)
			{
				TValue tvalue;
				bool flag3 = typedFunc(_inputValue, out tvalue);
				_outputValue = EnumBoxCache<TValue>.Instance.GetBoxed(tvalue);
				return flag3;
			};
		}
		return delegate(string _inputValue, out object _outputValue)
		{
			TValue tvalue2;
			bool flag4 = typedFunc(_inputValue, out tvalue2);
			_outputValue = tvalue2;
			return flag4;
		};
	}

	// Token: 0x06008AE6 RID: 35558 RVA: 0x00350014 File Offset: 0x0034E214
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiParsingConverterTypeToTypeDelegate <tryGetParsingConverterTypeToTypeDelegate>g__GetParsingConverterDelegateCore|25_0<TOutput, TInput>(MethodInfo _methodInner)
	{
		if (typeof(TOutput) == typeof(bool))
		{
			ParsingMethodCache.RawConverterTypeToTypeDelegate<bool, TInput> boolFunc = (ParsingMethodCache.RawConverterTypeToTypeDelegate<bool, TInput>)Delegate.CreateDelegate(typeof(ParsingMethodCache.RawConverterTypeToTypeDelegate<bool, TInput>), _methodInner);
			return delegate(object _inputValue, out object _outputValue)
			{
				bool flag2;
				bool flag = boolFunc((TInput)((object)_inputValue), out flag2);
				_outputValue = ParsingMethodCache.boxBool(flag2);
				return flag;
			};
		}
		ParsingMethodCache.RawConverterTypeToTypeDelegate<TOutput, TInput> typedFunc = (ParsingMethodCache.RawConverterTypeToTypeDelegate<TOutput, TInput>)Delegate.CreateDelegate(typeof(ParsingMethodCache.RawConverterTypeToTypeDelegate<TOutput, TInput>), _methodInner);
		if (typeof(TOutput).IsEnum)
		{
			return delegate(object _inputValue, out object _outputValue)
			{
				TOutput toutput;
				bool flag3 = typedFunc((TInput)((object)_inputValue), out toutput);
				_outputValue = EnumBoxCache<TOutput>.Instance.GetBoxed(toutput);
				return flag3;
			};
		}
		return delegate(object _inputValue, out object _outputValue)
		{
			TOutput toutput2;
			bool flag4 = typedFunc((TInput)((object)_inputValue), out toutput2);
			_outputValue = toutput2;
			return flag4;
		};
	}

	// Token: 0x04006714 RID: 26388
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly MicroStopwatch mswInit = new MicroStopwatch(false);

	// Token: 0x04006715 RID: 26389
	[TupleElementNames(new string[] { "func", "inputType" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, Dictionary<string, ValueTuple<XuiParsingDelegate, Type>>> customParsingDelegates = new Dictionary<Type, Dictionary<string, ValueTuple<XuiParsingDelegate, Type>>>();

	// Token: 0x04006716 RID: 26390
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, ParsingMethodCache.ParsingMethodTypeCache> cachePerType = new Dictionary<Type, ParsingMethodCache.ParsingMethodTypeCache>();

	// Token: 0x04006717 RID: 26391
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, XuiParsingConverterFromStringDelegate> convertersFromString = new Dictionary<Type, XuiParsingConverterFromStringDelegate>();

	// Token: 0x04006718 RID: 26392
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, Dictionary<Type, XuiParsingConverterTypeToTypeDelegate>> convertersTypeToType = new Dictionary<Type, Dictionary<Type, XuiParsingConverterTypeToTypeDelegate>>();

	// Token: 0x04006719 RID: 26393
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly MethodInfo genericEnumTryParseMethod;

	// Token: 0x0400671A RID: 26394
	[PublicizedFrom(EAccessModifier.Private)]
	public static ParsingMethodCache instance;

	// Token: 0x0400671B RID: 26395
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedFalse = false;

	// Token: 0x0400671C RID: 26396
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedTrue = true;

	// Token: 0x02001125 RID: 4389
	public class ParsingMethodData
	{
		// Token: 0x06008AE7 RID: 35559 RVA: 0x003500B3 File Offset: 0x0034E2B3
		public ParsingMethodData(Type _controllerOrViewType, string _attributeName, string _definingController, Type _nativeParseType, XuiParsingDelegate _nativeParserDelegate)
		{
			this.controllerOrViewType = _controllerOrViewType;
			this.attributeName = _attributeName;
			this.DefiningController = _definingController;
			this.NativeParseType = _nativeParseType;
			this.nativeParserDelegate = _nativeParserDelegate;
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x003500EC File Offset: 0x0034E2EC
		public bool TryGetDelegateForSourceType(Type _sourceType, out XuiParsingDelegate _delegate)
		{
			if (_sourceType == this.NativeParseType)
			{
				_delegate = this.nativeParserDelegate;
				return true;
			}
			if (this.sourceTypeDelegates.TryGetValue(_sourceType, out _delegate))
			{
				return _delegate != null;
			}
			if (this.NativeParseType == typeof(string))
			{
				_delegate = delegate(IXUiElement _instance, object _value)
				{
					this.nativeParserDelegate(_instance, _value.ToString());
				};
				this.sourceTypeDelegates[_sourceType] = _delegate;
				return true;
			}
			if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
			{
				Log.Warning(string.Concat(new string[]
				{
					"[XUi] Trying to map from ",
					_sourceType.FullName,
					" to ",
					this.NativeParseType.FullName,
					" for attribute '",
					this.attributeName,
					"' on ",
					this.controllerOrViewType.FullName
				}));
			}
			XuiParsingConverterTypeToTypeDelegate typeToTypeConverter;
			if (ParsingMethodCache.Instance.TryGetTypeConverterFromType(this.NativeParseType, _sourceType, out typeToTypeConverter))
			{
				_delegate = delegate(IXUiElement _instance, object _value)
				{
					object obj;
					if (!typeToTypeConverter(_value, out obj))
					{
						Log.Error(string.Format("[XUi] Can not parse input ('{0}', type {1}) into target type {2} for attribute '{3}' on {4}: Conversion to target type failed. --- hierarchy: {5}", new object[]
						{
							_value,
							_value.GetType().FullName,
							this.NativeParseType.FullName,
							this.attributeName,
							this.controllerOrViewType.FullName,
							_instance.GetXuiHierarchy()
						}));
						return;
					}
					this.nativeParserDelegate(_instance, obj);
				};
				this.sourceTypeDelegates[_sourceType] = _delegate;
				return true;
			}
			if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
			{
				Log.Out(string.Concat(new string[]
				{
					"[XUi] Conversion from source type (",
					_sourceType.FullName,
					") into target type (",
					this.NativeParseType.FullName,
					") requires parsing from string! (Attribute '",
					this.attributeName,
					"' on ",
					this.controllerOrViewType.FullName,
					")"
				}));
			}
			XuiParsingConverterFromStringDelegate converter;
			if (!ParsingMethodCache.Instance.TryGetTypeConverterFromString(this.NativeParseType, out converter))
			{
				Log.Error(string.Concat(new string[]
				{
					"[XUi] Can not parse input type ('",
					_sourceType.FullName,
					"') into target type ",
					this.NativeParseType.FullName,
					" for attribute '",
					this.attributeName,
					"' on ",
					this.controllerOrViewType.FullName,
					": No converter for target type"
				}));
				_delegate = null;
				this.sourceTypeDelegates[_sourceType] = _delegate;
				return false;
			}
			_delegate = delegate(IXUiElement _instance, object _value)
			{
				string text = _value.ToString();
				object obj2;
				if (!converter(text, out obj2))
				{
					Log.Error(string.Format("[XUi] Can not parse input ('{0}', type {1}) into target type {2} for attribute '{3}' on {4}: Conversion to target type failed. --- hierarchy: {5}", new object[]
					{
						_value,
						_value.GetType().FullName,
						this.NativeParseType.FullName,
						this.attributeName,
						this.controllerOrViewType.FullName,
						_instance.GetXuiHierarchy()
					}));
					return;
				}
				this.nativeParserDelegate(_instance, obj2);
			};
			this.sourceTypeDelegates[_sourceType] = _delegate;
			return true;
		}

		// Token: 0x0400671D RID: 26397
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerOrViewType;

		// Token: 0x0400671E RID: 26398
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string attributeName;

		// Token: 0x0400671F RID: 26399
		public readonly string DefiningController;

		// Token: 0x04006720 RID: 26400
		public readonly Type NativeParseType;

		// Token: 0x04006721 RID: 26401
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiParsingDelegate nativeParserDelegate;

		// Token: 0x04006722 RID: 26402
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Type, XuiParsingDelegate> sourceTypeDelegates = new Dictionary<Type, XuiParsingDelegate>();
	}

	// Token: 0x02001127 RID: 4391
	[PublicizedFrom(EAccessModifier.Private)]
	public class ParsingMethodTypeCache
	{
		// Token: 0x06008AED RID: 35565 RVA: 0x0035046B File Offset: 0x0034E66B
		public ParsingMethodTypeCache(Type _controllerOrViewType, ParsingMethodCache _owner)
		{
			this.controllerOrViewType = _controllerOrViewType;
			this.initCacheForController(_owner);
		}

		// Token: 0x06008AEE RID: 35566 RVA: 0x00350494 File Offset: 0x0034E694
		[PublicizedFrom(EAccessModifier.Private)]
		public void initCacheForController(ParsingMethodCache _owner)
		{
			if (this.controllerOrViewType.BaseType != null)
			{
				_owner.getCacheForType(this.controllerOrViewType.BaseType).methods.CopyTo<string, ParsingMethodCache.ParsingMethodData>(this.methods, false);
			}
			ReflectionHelpers.GetMethodsWithAttribute<XuiXmlAttributeAttribute>(this.controllerOrViewType, new Action<MethodInfo, bool, XuiXmlAttributeAttribute>(this.<initCacheForController>g__MethodFoundCallback|3_1), true, true, false);
			ReflectionHelpers.GetPropertiesWithAttribute<XuiXmlAttributeAttribute>(this.controllerOrViewType, new Action<PropertyInfo, bool, XuiXmlAttributeAttribute>(this.<initCacheForController>g__PropertyFoundCallback|3_0), true, true, false);
			Dictionary<string, ValueTuple<XuiParsingDelegate, Type>> dictionary;
			if (_owner.customParsingDelegates.TryGetValue(this.controllerOrViewType, out dictionary))
			{
				foreach (KeyValuePair<string, ValueTuple<XuiParsingDelegate, Type>> keyValuePair in dictionary)
				{
					string text;
					ValueTuple<XuiParsingDelegate, Type> valueTuple;
					keyValuePair.Deconstruct(out text, out valueTuple);
					ValueTuple<XuiParsingDelegate, Type> valueTuple2 = valueTuple;
					string text2 = text;
					XuiParsingDelegate item = valueTuple2.Item1;
					Type item2 = valueTuple2.Item2;
					ParsingMethodCache.ParsingMethodData parsingMethodData;
					if (this.methods.TryGetValue(text2, out parsingMethodData))
					{
						Log.Warning(string.Concat(new string[] { "[XUi] Custom parsing overriding overriding existing (", parsingMethodData.DefiningController, ") parsing definition of '", text2, "'" }));
					}
					this.methods[text2] = new ParsingMethodCache.ParsingMethodData(this.controllerOrViewType, text2, "<Custom>", item2, item);
				}
			}
		}

		// Token: 0x06008AEF RID: 35567 RVA: 0x003505E8 File Offset: 0x0034E7E8
		public bool TryGetParsingDelegate(string _attributeName, out ParsingMethodCache.ParsingMethodData _parsingDelegate)
		{
			ParsingMethodCache.ParsingMethodData parsingMethodData;
			if (this.methods.TryGetValue(_attributeName, out parsingMethodData))
			{
				_parsingDelegate = parsingMethodData;
				return true;
			}
			_parsingDelegate = null;
			return false;
		}

		// Token: 0x06008AF0 RID: 35568 RVA: 0x00350610 File Offset: 0x0034E810
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__PropertyFoundCallback|3_0(PropertyInfo _property, bool _hasMultiple, XuiXmlAttributeAttribute _xmlParsingConverterAttribute)
		{
			MethodInfo setMethod = _property.GetSetMethod(true);
			if (setMethod == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"[XUi] Failed creating parsing property wrapper: Property has no setter (",
					_property.DeclaringType.FullName,
					".",
					_property.Name,
					")"
				}));
			}
			this.<initCacheForController>g__MethodFoundCallback|3_1(setMethod, _hasMultiple, _xmlParsingConverterAttribute);
		}

		// Token: 0x06008AF1 RID: 35569 RVA: 0x00350678 File Offset: 0x0034E878
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__MethodFoundCallback|3_1(MethodInfo _method, bool _hasMultiple, XuiXmlAttributeAttribute _parsingAttribute)
		{
			XuiParsingDelegate xuiParsingDelegate;
			Type type;
			if (!ParsingMethodCache.tryGetParsingObjectDelegate(_method, out xuiParsingDelegate, out type))
			{
				return;
			}
			ParsingMethodCache.ParsingMethodData parsingMethodData = new ParsingMethodCache.ParsingMethodData(this.controllerOrViewType, _parsingAttribute.AttributeName, _method.DeclaringType.FullName, type, xuiParsingDelegate);
			ParsingMethodCache.ParsingMethodData parsingMethodData2;
			if (!_parsingAttribute.Override && this.methods.TryGetValue(_parsingAttribute.AttributeName, out parsingMethodData2))
			{
				Log.Warning(string.Concat(new string[] { "[XUi] Child class (", parsingMethodData.DefiningController, ") overriding parent (", parsingMethodData2.DefiningController, ") parsing definition of '", _parsingAttribute.AttributeName, "'" }));
			}
			this.methods[_parsingAttribute.AttributeName] = parsingMethodData;
		}

		// Token: 0x04006726 RID: 26406
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerOrViewType;

		// Token: 0x04006727 RID: 26407
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, ParsingMethodCache.ParsingMethodData> methods = new Dictionary<string, ParsingMethodCache.ParsingMethodData>(64, StringComparer.Ordinal);
	}

	// Token: 0x02001128 RID: 4392
	// (Invoke) Token: 0x06008AF3 RID: 35571
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool RawConverterFromStringDelegate<T>(string _in, out T _out);

	// Token: 0x02001129 RID: 4393
	// (Invoke) Token: 0x06008AF7 RID: 35575
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool RawConverterTypeToTypeDelegate<TOutput, TInput>(TInput _in, out TOutput _out);
}

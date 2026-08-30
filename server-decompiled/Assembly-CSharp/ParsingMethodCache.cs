using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x020010F5 RID: 4341
public class ParsingMethodCache
{
	// Token: 0x17000FD5 RID: 4053
	// (get) Token: 0x0600897E RID: 35198 RVA: 0x00346A41 File Offset: 0x00344C41
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

	// Token: 0x0600897F RID: 35199 RVA: 0x00346A58 File Offset: 0x00344C58
	[PublicizedFrom(EAccessModifier.Private)]
	public ParsingMethodCache()
	{
		this.genericEnumTryParseMethod = new ParsingMethodCache.RawConverterFromStringDelegate<int>(EnumUtils.TryParseIgnoreCase<int>).Method.GetGenericMethodDefinition();
		ReflectionHelpers.FindTypesWithAttribute<XuiXmlAttributeConvertersClassAttribute>(new Action<Type, bool, XuiXmlAttributeConvertersClassAttribute>(this.<.ctor>g__TypeFoundCallback|11_0), true);
	}

	// Token: 0x06008980 RID: 35200 RVA: 0x00346AD4 File Offset: 0x00344CD4
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

	// Token: 0x06008981 RID: 35201 RVA: 0x00346B20 File Offset: 0x00344D20
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

	// Token: 0x06008982 RID: 35202 RVA: 0x00346CBD File Offset: 0x00344EBD
	public bool TryGetParsingDelegate(IXUiElement _target, string _attributeName, out ParsingMethodCache.ParsingMethodData _parsingDelegate)
	{
		return this.getCacheForType(_target.GetType()).TryGetParsingDelegate(_attributeName, out _parsingDelegate);
	}

	// Token: 0x06008983 RID: 35203 RVA: 0x00346CD4 File Offset: 0x00344ED4
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

	// Token: 0x06008984 RID: 35204 RVA: 0x00346DD0 File Offset: 0x00344FD0
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

	// Token: 0x06008985 RID: 35205 RVA: 0x00346E28 File Offset: 0x00345028
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

	// Token: 0x06008986 RID: 35206 RVA: 0x00346E52 File Offset: 0x00345052
	public void LogInitTotal()
	{
		Log.Out(string.Format("[XUi] ParsingMethodCache total init time: {0:F3} ms", (float)this.mswInit.ElapsedMicroseconds / 1000f));
	}

	// Token: 0x06008987 RID: 35207 RVA: 0x00346E7C File Offset: 0x0034507C
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
		MethodInfo methodInfo = new Func<MethodInfo, XuiParsingDelegate>(ParsingMethodCache.<tryGetParsingObjectDelegate>g__GetParsingObjectDelegateCore|19_0<XUiController, object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { type, _inputType });
		_func = (XuiParsingDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x06008988 RID: 35208 RVA: 0x00346FC0 File Offset: 0x003451C0
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
		MethodInfo methodInfo = new Func<MethodInfo, XuiParsingConverterFromStringDelegate>(ParsingMethodCache.<tryGetParsingConverterFromStringDelegate>g__GetParsingConverterDelegateCore|20_0<object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { _outputType });
		_func = (XuiParsingConverterFromStringDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x06008989 RID: 35209 RVA: 0x00347184 File Offset: 0x00345384
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
		MethodInfo methodInfo = new Func<MethodInfo, XuiParsingConverterTypeToTypeDelegate>(ParsingMethodCache.<tryGetParsingConverterTypeToTypeDelegate>g__GetParsingConverterDelegateCore|22_0<object, object>).Method.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { _outputType, _inputType });
		_func = (XuiParsingConverterTypeToTypeDelegate)methodInfo.Invoke(null, new object[] { _method });
		return true;
	}

	// Token: 0x0600898A RID: 35210 RVA: 0x003472FE File Offset: 0x003454FE
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <.ctor>g__TypeFoundCallback|11_0(Type _type, bool _hasMultiple, XuiXmlAttributeConvertersClassAttribute _xmlParsingConvertersClassAttribute)
	{
		ReflectionHelpers.GetMethodsWithAttribute<XuiXmlAttributeConverterAttribute>(_type, new Action<MethodInfo, bool, XuiXmlAttributeConverterAttribute>(this.<.ctor>g__MethodFoundCallback|11_1), true, false, true);
	}

	// Token: 0x0600898B RID: 35211 RVA: 0x00347318 File Offset: 0x00345518
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

	// Token: 0x0600898C RID: 35212 RVA: 0x00347401 File Offset: 0x00345601
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiParsingDelegate <tryGetParsingObjectDelegate>g__GetParsingObjectDelegateCore|19_0<TInstance, TValue>(MethodInfo _methodInner) where TInstance : class
	{
		Action<TInstance, TValue> typedFunc = (Action<TInstance, TValue>)Delegate.CreateDelegate(typeof(Action<TInstance, TValue>), _methodInner);
		return delegate(IXUiElement _instance, object _value)
		{
			typedFunc((TInstance)((object)_instance), (TValue)((object)_value));
		};
	}

	// Token: 0x0600898D RID: 35213 RVA: 0x0034742E File Offset: 0x0034562E
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiParsingConverterFromStringDelegate <tryGetParsingConverterFromStringDelegate>g__GetParsingConverterDelegateCore|20_0<TValue>(MethodInfo _methodInner)
	{
		ParsingMethodCache.RawConverterFromStringDelegate<TValue> typedFunc = (ParsingMethodCache.RawConverterFromStringDelegate<TValue>)Delegate.CreateDelegate(typeof(ParsingMethodCache.RawConverterFromStringDelegate<TValue>), _methodInner);
		return delegate(string _inputValue, out object _outputValue)
		{
			TValue tvalue;
			bool flag = typedFunc(_inputValue, out tvalue);
			_outputValue = tvalue;
			return flag;
		};
	}

	// Token: 0x0600898E RID: 35214 RVA: 0x0034745B File Offset: 0x0034565B
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XuiParsingConverterTypeToTypeDelegate <tryGetParsingConverterTypeToTypeDelegate>g__GetParsingConverterDelegateCore|22_0<TOutput, TInput>(MethodInfo _methodInner)
	{
		ParsingMethodCache.RawConverterTypeToTypeDelegate<TOutput, TInput> typedFunc = (ParsingMethodCache.RawConverterTypeToTypeDelegate<TOutput, TInput>)Delegate.CreateDelegate(typeof(ParsingMethodCache.RawConverterTypeToTypeDelegate<TOutput, TInput>), _methodInner);
		return delegate(object _inputValue, out object _outputValue)
		{
			TOutput toutput;
			bool flag = typedFunc((TInput)((object)_inputValue), out toutput);
			_outputValue = toutput;
			return flag;
		};
	}

	// Token: 0x040065FC RID: 26108
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly MicroStopwatch mswInit = new MicroStopwatch(false);

	// Token: 0x040065FD RID: 26109
	[TupleElementNames(new string[] { "func", "inputType" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, Dictionary<string, ValueTuple<XuiParsingDelegate, Type>>> customParsingDelegates = new Dictionary<Type, Dictionary<string, ValueTuple<XuiParsingDelegate, Type>>>();

	// Token: 0x040065FE RID: 26110
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, ParsingMethodCache.ParsingMethodTypeCache> cachePerType = new Dictionary<Type, ParsingMethodCache.ParsingMethodTypeCache>();

	// Token: 0x040065FF RID: 26111
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, XuiParsingConverterFromStringDelegate> convertersFromString = new Dictionary<Type, XuiParsingConverterFromStringDelegate>();

	// Token: 0x04006600 RID: 26112
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, Dictionary<Type, XuiParsingConverterTypeToTypeDelegate>> convertersTypeToType = new Dictionary<Type, Dictionary<Type, XuiParsingConverterTypeToTypeDelegate>>();

	// Token: 0x04006601 RID: 26113
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly MethodInfo genericEnumTryParseMethod;

	// Token: 0x04006602 RID: 26114
	[PublicizedFrom(EAccessModifier.Private)]
	public static ParsingMethodCache instance;

	// Token: 0x020010F6 RID: 4342
	public class ParsingMethodData
	{
		// Token: 0x0600898F RID: 35215 RVA: 0x00347488 File Offset: 0x00345688
		public ParsingMethodData(Type _controllerOrViewType, string _attributeName, string _definingController, Type _nativeParseType, XuiParsingDelegate _nativeParserDelegate)
		{
			this.controllerOrViewType = _controllerOrViewType;
			this.attributeName = _attributeName;
			this.DefiningController = _definingController;
			this.NativeParseType = _nativeParseType;
			this.nativeParserDelegate = _nativeParserDelegate;
		}

		// Token: 0x06008990 RID: 35216 RVA: 0x003474C0 File Offset: 0x003456C0
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

		// Token: 0x04006603 RID: 26115
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerOrViewType;

		// Token: 0x04006604 RID: 26116
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string attributeName;

		// Token: 0x04006605 RID: 26117
		public readonly string DefiningController;

		// Token: 0x04006606 RID: 26118
		public readonly Type NativeParseType;

		// Token: 0x04006607 RID: 26119
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiParsingDelegate nativeParserDelegate;

		// Token: 0x04006608 RID: 26120
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Type, XuiParsingDelegate> sourceTypeDelegates = new Dictionary<Type, XuiParsingDelegate>();
	}

	// Token: 0x020010F8 RID: 4344
	[PublicizedFrom(EAccessModifier.Private)]
	public class ParsingMethodTypeCache
	{
		// Token: 0x06008995 RID: 35221 RVA: 0x0034783F File Offset: 0x00345A3F
		public ParsingMethodTypeCache(Type _controllerOrViewType, ParsingMethodCache _owner)
		{
			this.controllerOrViewType = _controllerOrViewType;
			this.initCacheForController(_owner);
		}

		// Token: 0x06008996 RID: 35222 RVA: 0x00347868 File Offset: 0x00345A68
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

		// Token: 0x06008997 RID: 35223 RVA: 0x003479BC File Offset: 0x00345BBC
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

		// Token: 0x06008998 RID: 35224 RVA: 0x003479E4 File Offset: 0x00345BE4
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

		// Token: 0x06008999 RID: 35225 RVA: 0x00347A4C File Offset: 0x00345C4C
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

		// Token: 0x0400660C RID: 26124
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerOrViewType;

		// Token: 0x0400660D RID: 26125
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, ParsingMethodCache.ParsingMethodData> methods = new Dictionary<string, ParsingMethodCache.ParsingMethodData>(64, StringComparer.Ordinal);
	}

	// Token: 0x020010F9 RID: 4345
	// (Invoke) Token: 0x0600899B RID: 35227
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool RawConverterFromStringDelegate<T>(string _in, out T _out);

	// Token: 0x020010FA RID: 4346
	// (Invoke) Token: 0x0600899F RID: 35231
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool RawConverterTypeToTypeDelegate<TOutput, TInput>(TInput _in, out TOutput _out);
}

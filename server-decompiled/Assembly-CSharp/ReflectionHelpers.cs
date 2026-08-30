using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x0200148F RID: 5263
public static class ReflectionHelpers
{
	// Token: 0x0600A52C RID: 42284 RVA: 0x003D6F1C File Offset: 0x003D511C
	public static void FindTypesImplementingBase(Type _searchType, Action<Type> _typeFoundCallback, bool _allowAbstract = false)
	{
		try
		{
			List<Assembly> loadedAssemblies = ModManager.GetLoadedAssemblies();
			loadedAssemblies.Insert(0, Assembly.GetExecutingAssembly());
			for (int i = 0; i < loadedAssemblies.Count; i++)
			{
				Assembly assembly = loadedAssemblies[i];
				try
				{
					foreach (Type type in assembly.GetTypes())
					{
						if (type.IsClass && (_allowAbstract || !type.IsAbstract) && _searchType.IsAssignableFrom(type))
						{
							try
							{
								if (i == 0 && Attribute.GetCustomAttribute(type, typeof(PreserveAttribute)) == null)
								{
									Log.Error(string.Format("Type:{0} is missing the UnityEngine.Scripting.Preserve attribute, this will fail on consoles", type));
								}
								if (_typeFoundCallback != null)
								{
									_typeFoundCallback(type);
								}
							}
							catch (Exception ex)
							{
								Log.Error("Error invoking found type callback for '" + type.FullName + "'");
								Log.Exception(ex);
							}
						}
					}
				}
				catch (ReflectionTypeLoadException ex2)
				{
					Log.Error("Error loading types from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex2);
					Console.WriteLine();
					Console.WriteLine("Successfully loaded Types:");
					int num = 1;
					foreach (Type type2 in ex2.Types)
					{
						Console.WriteLine(string.Format("{0}. {1}", num++, (type2 != null) ? type2.FullName : "NULL"));
					}
					Console.WriteLine();
					Console.WriteLine("Exceptions:");
					num = 1;
					foreach (Exception ex3 in ex2.LoaderExceptions)
					{
						Console.WriteLine(string.Format("{0}. {1}", num++, (ex3 != null) ? ex3.Message : "NULL"));
					}
					Console.WriteLine();
				}
				catch (Exception ex4)
				{
					Log.Error("Error loading types from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex4);
				}
			}
		}
		catch (Exception ex5)
		{
			Log.Error("Error loading types");
			Log.Exception(ex5);
		}
	}

	// Token: 0x0600A52D RID: 42285 RVA: 0x003D717C File Offset: 0x003D537C
	public static T Instantiate<T>(Type _type) where T : class
	{
		try
		{
			if (_type.Assembly == Assembly.GetExecutingAssembly() && Attribute.GetCustomAttribute(_type, typeof(PreserveAttribute)) == null)
			{
				Log.Error(string.Format("Type:{0} is missing the UnityEngine.Scripting.Preserve attribute, this will fail on consoles", _type));
			}
			ConstructorInfo constructor = _type.GetConstructor(Type.EmptyTypes);
			if (constructor != null)
			{
				return (T)((object)constructor.Invoke(Array.Empty<object>()));
			}
			Log.Warning("Class '" + _type.FullName + "' does not contain a parameterless constructor, skipping");
		}
		catch (Exception ex)
		{
			Log.Error("Could not instantiate type '" + _type.FullName + "'");
			Log.Exception(ex);
		}
		return default(T);
	}

	// Token: 0x0600A52E RID: 42286 RVA: 0x003D7240 File Offset: 0x003D5440
	public static Type GetTypeWithPrefix(string _prefix, string _name)
	{
		int num = _name.IndexOfAny(ReflectionHelpers.classnameEndMarkers);
		if (num < 0)
		{
			num = _name.Length - 1;
		}
		int num2 = _name.LastIndexOf('.', num);
		string text;
		if (num2 >= 0)
		{
			text = _name.Insert(num2 + 1, _prefix);
		}
		else
		{
			text = _prefix + _name;
		}
		Type type = Type.GetType(text);
		if (type != null)
		{
			if (type.Assembly == Assembly.GetExecutingAssembly() && Attribute.GetCustomAttribute(type, typeof(PreserveAttribute)) == null)
			{
				Log.Warning(string.Format("Type:{0} is missing the UnityEngine.Scripting.Preserve attribute, this will fail on consoles", type));
			}
			return type;
		}
		type = Type.GetType(_name);
		if (type != null)
		{
			if (type.Assembly == Assembly.GetExecutingAssembly() && Attribute.GetCustomAttribute(type, typeof(PreserveAttribute)) == null)
			{
				Log.Warning(string.Format("Type:{0} is missing the UnityEngine.Scripting.Preserve attribute, this will fail on consoles", type));
			}
			return type;
		}
		Log.Warning("Type:" + _name + " was missing when we looked it up via Type.GetType()");
		return null;
	}

	// Token: 0x0600A52F RID: 42287 RVA: 0x003D732C File Offset: 0x003D552C
	public static void FindTypesWithAttribute<T>(Action<Type, bool, T> _typeFoundCallback, bool _allowAbstract = false) where T : Attribute
	{
		Type typeFromHandle = typeof(T);
		try
		{
			List<Assembly> loadedAssemblies = ModManager.GetLoadedAssemblies();
			loadedAssemblies.Insert(0, Assembly.GetExecutingAssembly());
			for (int i = 0; i < loadedAssemblies.Count; i++)
			{
				Assembly assembly = loadedAssemblies[i];
				try
				{
					foreach (Type type in assembly.GetTypes())
					{
						T t;
						bool flag;
						if (type.IsClass && (_allowAbstract || !type.IsAbstract) && ReflectionHelpers.MemberHasAttribute<T>(type, typeFromHandle, out t, out flag))
						{
							try
							{
								if (i == 0 && Attribute.GetCustomAttribute(type, typeof(PreserveAttribute)) == null)
								{
									Log.Error(string.Format("Type:{0} is missing the UnityEngine.Scripting.Preserve attribute, this will fail on consoles", type));
								}
								if (_typeFoundCallback != null)
								{
									_typeFoundCallback(type, flag, t);
								}
							}
							catch (Exception ex)
							{
								Log.Error("Error invoking found type callback for '" + type.FullName + "'");
								Log.Exception(ex);
							}
						}
					}
				}
				catch (ReflectionTypeLoadException ex2)
				{
					Log.Error("Error loading types from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex2);
					Console.WriteLine();
					Console.WriteLine("Successfully loaded Types:");
					int num = 1;
					foreach (Type type2 in ex2.Types)
					{
						Console.WriteLine(string.Format("{0}. {1}", num++, (type2 != null) ? type2.FullName : "NULL"));
					}
					Console.WriteLine();
					Console.WriteLine("Exceptions:");
					num = 1;
					foreach (Exception ex3 in ex2.LoaderExceptions)
					{
						Console.WriteLine(string.Format("{0}. {1}", num++, (ex3 != null) ? ex3.Message : "NULL"));
					}
					Console.WriteLine();
				}
				catch (Exception ex4)
				{
					Log.Error("Error loading types from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex4);
				}
			}
		}
		catch (Exception ex5)
		{
			Log.Error("Error loading types");
			Log.Exception(ex5);
		}
	}

	// Token: 0x0600A530 RID: 42288 RVA: 0x003D75A4 File Offset: 0x003D57A4
	public static void GetMethodsWithAttribute<T>(Action<MethodInfo, bool, T> _methodFoundCallback) where T : Attribute
	{
		Type typeFromHandle = typeof(T);
		try
		{
			List<Assembly> loadedAssemblies = ModManager.GetLoadedAssemblies();
			loadedAssemblies.Insert(0, Assembly.GetExecutingAssembly());
			for (int i = 0; i < loadedAssemblies.Count; i++)
			{
				Assembly assembly = loadedAssemblies[i];
				try
				{
					foreach (Type type in assembly.GetTypes())
					{
						if (type.IsClass)
						{
							foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
							{
								T t;
								bool flag;
								if (ReflectionHelpers.MemberHasAttribute<T>(methodInfo, typeFromHandle, out t, out flag))
								{
									try
									{
										if (_methodFoundCallback != null)
										{
											_methodFoundCallback(methodInfo, flag, t);
										}
									}
									catch (Exception ex)
									{
										Log.Error(string.Concat(new string[]
										{
											"Error invoking found method callback for '",
											methodInfo.DeclaringType.FullName,
											".",
											methodInfo.Name,
											"'"
										}));
										Log.Exception(ex);
									}
								}
							}
						}
					}
				}
				catch (ReflectionTypeLoadException ex2)
				{
					Log.Error("Error loading types from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex2);
					Console.WriteLine();
					Console.WriteLine("Successfully loaded Types:");
					int num = 1;
					foreach (Type type2 in ex2.Types)
					{
						Console.WriteLine(string.Format("{0}. {1}", num++, (type2 != null) ? type2.FullName : "NULL"));
					}
					Console.WriteLine();
					Console.WriteLine("Exceptions:");
					num = 1;
					foreach (Exception ex3 in ex2.LoaderExceptions)
					{
						Console.WriteLine(string.Format("{0}. {1}", num++, (ex3 != null) ? ex3.Message : "NULL"));
					}
					Console.WriteLine();
				}
				catch (Exception ex4)
				{
					Log.Error("Error loading types from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex4);
				}
			}
		}
		catch (Exception ex5)
		{
			Log.Error("Error loading types");
			Log.Exception(ex5);
		}
	}

	// Token: 0x0600A531 RID: 42289 RVA: 0x003D7840 File Offset: 0x003D5A40
	public static void GetMethodsWithAttribute<T>(Type _containingType, Action<MethodInfo, bool, T> _methodFoundCallback, bool _declaredOnly = true, bool _allowInstance = true, bool _allowStatic = true) where T : Attribute
	{
		Type typeFromHandle = typeof(T);
		try
		{
			if (_containingType.IsClass)
			{
				BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
				if (_allowInstance)
				{
					bindingFlags |= BindingFlags.Instance;
				}
				if (_allowStatic)
				{
					bindingFlags |= BindingFlags.Static;
				}
				if (_declaredOnly)
				{
					bindingFlags |= BindingFlags.DeclaredOnly;
				}
				foreach (MethodInfo methodInfo in _containingType.GetMethods(bindingFlags))
				{
					T t;
					bool flag;
					if (ReflectionHelpers.MemberHasAttribute<T>(methodInfo, typeFromHandle, out t, out flag))
					{
						try
						{
							if (_methodFoundCallback != null)
							{
								_methodFoundCallback(methodInfo, flag, t);
							}
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new string[]
							{
								"Error invoking found method callback for '",
								methodInfo.DeclaringType.FullName,
								".",
								methodInfo.Name,
								"'"
							}));
							Log.Exception(ex);
						}
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Log.Error("Error loading methods from type " + _containingType.FullName);
			Log.Exception(ex2);
		}
	}

	// Token: 0x0600A532 RID: 42290 RVA: 0x003D7940 File Offset: 0x003D5B40
	public static void GetPropertiesWithAttribute<T>(Type _containingType, Action<PropertyInfo, bool, T> _propertyFoundCallback, bool _declaredOnly = true, bool _allowInstance = true, bool _allowStatic = true) where T : Attribute
	{
		Type typeFromHandle = typeof(T);
		try
		{
			if (_containingType.IsClass)
			{
				BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
				if (_allowInstance)
				{
					bindingFlags |= BindingFlags.Instance;
				}
				if (_allowStatic)
				{
					bindingFlags |= BindingFlags.Static;
				}
				if (_declaredOnly)
				{
					bindingFlags |= BindingFlags.DeclaredOnly;
				}
				foreach (PropertyInfo propertyInfo in _containingType.GetProperties(bindingFlags))
				{
					T t;
					bool flag;
					if (ReflectionHelpers.MemberHasAttribute<T>(propertyInfo, typeFromHandle, out t, out flag))
					{
						try
						{
							if (_propertyFoundCallback != null)
							{
								_propertyFoundCallback(propertyInfo, flag, t);
							}
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new string[]
							{
								"Error invoking found property callback for '",
								propertyInfo.DeclaringType.FullName,
								".",
								propertyInfo.Name,
								"'"
							}));
							Log.Exception(ex);
						}
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Log.Error("Error loading property from type " + _containingType.FullName);
			Log.Exception(ex2);
		}
	}

	// Token: 0x0600A533 RID: 42291 RVA: 0x003D7A40 File Offset: 0x003D5C40
	public static void GetFieldsWithAttribute<T>(Type _containingType, Action<FieldInfo, bool, T> _fieldFoundCallback, bool _declaredOnly = true, bool _allowInstance = true, bool _allowStatic = true) where T : Attribute
	{
		Type typeFromHandle = typeof(T);
		try
		{
			if (_containingType.IsClass)
			{
				BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
				if (_allowInstance)
				{
					bindingFlags |= BindingFlags.Instance;
				}
				if (_allowStatic)
				{
					bindingFlags |= BindingFlags.Static;
				}
				if (_declaredOnly)
				{
					bindingFlags |= BindingFlags.DeclaredOnly;
				}
				foreach (FieldInfo fieldInfo in _containingType.GetFields(bindingFlags))
				{
					T t;
					bool flag;
					if (ReflectionHelpers.MemberHasAttribute<T>(fieldInfo, typeFromHandle, out t, out flag))
					{
						try
						{
							if (_fieldFoundCallback != null)
							{
								_fieldFoundCallback(fieldInfo, flag, t);
							}
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new string[]
							{
								"Error invoking found field callback for '",
								fieldInfo.DeclaringType.FullName,
								".",
								fieldInfo.Name,
								"'"
							}));
							Log.Exception(ex);
						}
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Log.Error("Error loading fields from type " + _containingType.FullName);
			Log.Exception(ex2);
		}
	}

	// Token: 0x0600A534 RID: 42292 RVA: 0x003D7B40 File Offset: 0x003D5D40
	public static bool MemberHasAttribute<T>(MemberInfo _memberInfo, Type _attributeType, out T _attribute, out bool _hasMultiple) where T : Attribute
	{
		_hasMultiple = false;
		_attribute = default(T);
		foreach (Attribute attribute in _memberInfo.GetCustomAttributes())
		{
			T t = attribute as T;
			if (t != null)
			{
				if (_attribute != null)
				{
					_hasMultiple = true;
				}
				else
				{
					_attribute = t;
				}
			}
		}
		return _attribute != null;
	}

	// Token: 0x0600A535 RID: 42293 RVA: 0x003D7BCC File Offset: 0x003D5DCC
	public static bool MethodCompatibleWithDelegate<TDelegateType>(MethodInfo _method, bool _openDelegate = false) where TDelegateType : Delegate
	{
		return ReflectionHelpers.MethodCompatibleWithDelegate(typeof(TDelegateType), _method, _openDelegate);
	}

	// Token: 0x0600A536 RID: 42294 RVA: 0x003D7BE0 File Offset: 0x003D5DE0
	public static bool MethodCompatibleWithDelegate(Type _delegateType, MethodInfo _method, bool _openDelegate = false)
	{
		MethodInfo method = _delegateType.GetMethod("Invoke");
		if (method.ReturnType != _method.ReturnType)
		{
			return false;
		}
		ParameterInfo[] parameters = method.GetParameters();
		ParameterInfo[] parameters2 = _method.GetParameters();
		if (parameters.Length == 0)
		{
			return parameters2.Length == 0;
		}
		int num = 0;
		if (_openDelegate && !_method.IsStatic)
		{
			num = 1;
			if (!parameters[0].ParameterType.IsAssignableFrom(_method.DeclaringType))
			{
				return false;
			}
		}
		if (parameters.Length - num != parameters2.Length)
		{
			return false;
		}
		for (int i = 0; i < parameters2.Length; i++)
		{
			if (parameters[i + num].ParameterType != parameters2[i].ParameterType)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600A537 RID: 42295 RVA: 0x003D7C88 File Offset: 0x003D5E88
	public static string GetAssemblyNameWithLocation(Assembly _assembly, string _manualLocation = null)
	{
		AssemblyName assemblyName = new AssemblyName(_assembly.FullName);
		string text = _assembly.Location;
		if (string.IsNullOrEmpty(text))
		{
			text = _manualLocation;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return assemblyName.Name + " (in " + text + ")";
		}
		return assemblyName.Name;
	}

	// Token: 0x0600A538 RID: 42296 RVA: 0x003D7CD7 File Offset: 0x003D5ED7
	public static MethodInfo GetMethodInfo(Expression<Action> _expression)
	{
		return ReflectionHelpers.GetMethodInfo(_expression);
	}

	// Token: 0x0600A539 RID: 42297 RVA: 0x003D7CDF File Offset: 0x003D5EDF
	public static MethodInfo GetMethodInfo(LambdaExpression _expression)
	{
		MethodCallExpression methodCallExpression = _expression.Body as MethodCallExpression;
		if (methodCallExpression == null)
		{
			throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
		}
		return methodCallExpression.Method;
	}

	// Token: 0x04007B1A RID: 31514
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] classnameEndMarkers = new char[] { '`', ',' };
}

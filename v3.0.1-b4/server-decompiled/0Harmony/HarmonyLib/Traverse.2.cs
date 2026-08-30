using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000056 RID: 86
	public class Traverse
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x0000EFC9 File Offset: 0x0000D1C9
		[MethodImpl(MethodImplOptions.Synchronized)]
		static Traverse()
		{
			if (Traverse.Cache == null)
			{
				Traverse.Cache = new AccessCache();
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000EFF1 File Offset: 0x0000D1F1
		public static Traverse Create(Type type)
		{
			return new Traverse(type);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000EFF9 File Offset: 0x0000D1F9
		public static Traverse Create<T>()
		{
			return Traverse.Create(typeof(T));
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000F00A File Offset: 0x0000D20A
		public static Traverse Create(object root)
		{
			return new Traverse(root);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000F012 File Offset: 0x0000D212
		public static Traverse CreateWithType(string name)
		{
			return new Traverse(AccessTools.TypeByName(name));
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000F01F File Offset: 0x0000D21F
		private Traverse()
		{
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000F027 File Offset: 0x0000D227
		public Traverse(Type type)
		{
			this._type = type;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000F036 File Offset: 0x0000D236
		public Traverse(object root)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000F057 File Offset: 0x0000D257
		private Traverse(object root, MemberInfo info, object[] index)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null) ?? info.GetUnderlyingType();
			this._info = info;
			this._params = index;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000F090 File Offset: 0x0000D290
		private Traverse(object root, MethodInfo method, object[] parameter)
		{
			this._root = root;
			this._type = method.ReturnType;
			this._method = method;
			this._params = parameter;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		public object GetValue()
		{
			if (this._info is FieldInfo)
			{
				return ((FieldInfo)this._info).GetValue(this._root);
			}
			if (this._info is PropertyInfo)
			{
				return ((PropertyInfo)this._info).GetValue(this._root, AccessTools.all, null, this._params, CultureInfo.CurrentCulture);
			}
			if (this._method != null)
			{
				return this._method.Invoke(this._root, this._params);
			}
			if (this._root == null && this._type != null)
			{
				return this._type;
			}
			return this._root;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000F160 File Offset: 0x0000D360
		public T GetValue<T>()
		{
			object value = this.GetValue();
			if (value == null)
			{
				return default(T);
			}
			return (T)((object)value);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000F187 File Offset: 0x0000D387
		public object GetValue(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return this._method.Invoke(this._root, arguments);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000F1AE File Offset: 0x0000D3AE
		public T GetValue<T>(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return (T)((object)this._method.Invoke(this._root, arguments));
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000F1DC File Offset: 0x0000D3DC
		public Traverse SetValue(object value)
		{
			if (this._info is FieldInfo)
			{
				((FieldInfo)this._info).SetValue(this._root, value, AccessTools.all, null, CultureInfo.CurrentCulture);
			}
			if (this._info is PropertyInfo)
			{
				((PropertyInfo)this._info).SetValue(this._root, value, AccessTools.all, null, this._params, CultureInfo.CurrentCulture);
			}
			if (this._method != null)
			{
				throw new Exception("cannot set value of method " + this._method.FullDescription());
			}
			return this;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000F271 File Offset: 0x0000D471
		public Type GetValueType()
		{
			if (this._info is FieldInfo)
			{
				return ((FieldInfo)this._info).FieldType;
			}
			if (this._info is PropertyInfo)
			{
				return ((PropertyInfo)this._info).PropertyType;
			}
			return null;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000F2B0 File Offset: 0x0000D4B0
		private Traverse Resolve()
		{
			if (this._root == null)
			{
				FieldInfo fieldInfo = this._info as FieldInfo;
				if (fieldInfo != null && fieldInfo.IsStatic)
				{
					return new Traverse(this.GetValue());
				}
				PropertyInfo propertyInfo = this._info as PropertyInfo;
				if (propertyInfo != null && propertyInfo.GetGetMethod().IsStatic)
				{
					return new Traverse(this.GetValue());
				}
				if (this._method != null && this._method.IsStatic)
				{
					return new Traverse(this.GetValue());
				}
				if (this._type != null)
				{
					return this;
				}
			}
			return new Traverse(this.GetValue());
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000F348 File Offset: 0x0000D548
		public Traverse Type(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._type == null)
			{
				return new Traverse();
			}
			Type type = AccessTools.Inner(this._type, name);
			if (type == null)
			{
				return new Traverse();
			}
			return new Traverse(type);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000F390 File Offset: 0x0000D590
		public Traverse Field(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			FieldInfo fieldInfo = Traverse.Cache.GetFieldInfo(traverse._type, name, AccessCache.MemberType.Any, false);
			if (fieldInfo == null)
			{
				return new Traverse();
			}
			if (!fieldInfo.IsStatic && traverse._root == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, fieldInfo, null);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000F400 File Offset: 0x0000D600
		public Traverse<T> Field<T>(string name)
		{
			return new Traverse<T>(this.Field(name));
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000F410 File Offset: 0x0000D610
		public List<string> Fields()
		{
			Traverse traverse = this.Resolve();
			return AccessTools.GetFieldNames(traverse._type);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000F430 File Offset: 0x0000D630
		public Traverse Property(string name, object[] index = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			PropertyInfo propertyInfo = Traverse.Cache.GetPropertyInfo(traverse._type, name, AccessCache.MemberType.Any, false);
			if (propertyInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, propertyInfo, index);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000F48A File Offset: 0x0000D68A
		public Traverse<T> Property<T>(string name, object[] index = null)
		{
			return new Traverse<T>(this.Property(name, index));
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000F49C File Offset: 0x0000D69C
		public List<string> Properties()
		{
			Traverse traverse = this.Resolve();
			return AccessTools.GetPropertyNames(traverse._type);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000F4BC File Offset: 0x0000D6BC
		public Traverse Method(string name, params object[] arguments)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			Type[] types = AccessTools.GetTypes(arguments);
			MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, types, AccessCache.MemberType.Any, false);
			if (methodInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, (MethodInfo)methodInfo, arguments);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000F524 File Offset: 0x0000D724
		public Traverse Method(string name, Type[] paramTypes, object[] arguments = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, paramTypes, AccessCache.MemberType.Any, false);
			if (methodInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, (MethodInfo)methodInfo, arguments);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000F584 File Offset: 0x0000D784
		public List<string> Methods()
		{
			Traverse traverse = this.Resolve();
			return AccessTools.GetMethodNames(traverse._type);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000F5A3 File Offset: 0x0000D7A3
		public bool FieldExists()
		{
			return this._info != null && this._info is FieldInfo;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000F5BD File Offset: 0x0000D7BD
		public bool PropertyExists()
		{
			return this._info != null && this._info is PropertyInfo;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000F5D7 File Offset: 0x0000D7D7
		public bool MethodExists()
		{
			return this._method != null;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000F5E5 File Offset: 0x0000D7E5
		public bool TypeExists()
		{
			return this._type != null;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
		public static void IterateFields(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f));
			});
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000F634 File Offset: 0x0000D834
		public static void IterateFields(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000F680 File Offset: 0x0000D880
		public static void IterateFields(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		public static void IterateProperties(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null));
			});
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F70C File Offset: 0x0000D90C
		public static void IterateProperties(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F758 File Offset: 0x0000D958
		public static void IterateProperties(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		public override string ToString()
		{
			object obj = this._method ?? this.GetValue();
			if (obj == null)
			{
				return null;
			}
			return obj.ToString();
		}

		// Token: 0x040000CA RID: 202
		private static readonly AccessCache Cache;

		// Token: 0x040000CB RID: 203
		private readonly Type _type;

		// Token: 0x040000CC RID: 204
		private readonly object _root;

		// Token: 0x040000CD RID: 205
		private readonly MemberInfo _info;

		// Token: 0x040000CE RID: 206
		private readonly MethodBase _method;

		// Token: 0x040000CF RID: 207
		private readonly object[] _params;

		// Token: 0x040000D0 RID: 208
		public static Action<Traverse, Traverse> CopyFields = delegate(Traverse from, Traverse to)
		{
			to.SetValue(from.GetValue());
		};
	}
}

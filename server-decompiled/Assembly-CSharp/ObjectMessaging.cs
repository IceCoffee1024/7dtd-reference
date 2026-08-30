using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// Token: 0x020008B7 RID: 2231
public sealed class ObjectMessaging
{
	// Token: 0x060040AD RID: 16557 RVA: 0x001937C6 File Offset: 0x001919C6
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.CheckedSendMessage(_returnType, methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x001937D8 File Offset: 0x001919D8
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, methodName, _methodSignature, _target, _cacheDispostion, out obj, true, _arguments);
		return obj;
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x001937FB File Offset: 0x001919FB
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.SendMessage(_returnType, _methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x0019380C File Offset: 0x00191A0C
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, _methodName, _methodSignature, _target, _cacheDispostion, out obj, false, _arguments);
		return obj;
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x0019382F File Offset: 0x00191A2F
	public ObjectMessaging.MethodSignature GenerateMethodSignature(Type _returnType, Type[] _types)
	{
		if (_returnType == null)
		{
			_returnType = typeof(void);
		}
		return new ObjectMessaging.MethodSignature
		{
			ArgumentTypes = _types,
			ReturnType = _returnType
		};
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x0019385C File Offset: 0x00191A5C
	public bool SendMessageEx(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _messageSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, out object _returnValue, bool checkedCall, params object[] _arguments)
	{
		if (_returnType == null)
		{
			_returnType = typeof(void);
		}
		Type[] array = null;
		int num;
		if (_messageSignature == null)
		{
			num = _returnType.GetHashCode();
			for (int i = 0; i < _arguments.Length; i++)
			{
				num ^= _arguments[i].GetType().GetHashCode();
			}
		}
		else
		{
			num = _messageSignature.GetHashCode();
			_returnType = _messageSignature.ReturnType;
			array = _messageSignature.ArgumentTypes;
		}
		Type type = _target.GetType();
		MethodInfo methodInfo = null;
		Dictionary<int, MethodInfo> dictionary = null;
		if (_cacheDispostion == ObjectMessaging.CacheDisposition.CacheTypeInfo)
		{
			int num2 = _methodName.GetHashCode() ^ num;
			if (this.typeCache.TryGetValue(type, out dictionary))
			{
				if (dictionary.TryGetValue(num2, out methodInfo))
				{
					dictionary = null;
				}
			}
			else
			{
				dictionary = new Dictionary<int, MethodInfo>();
				this.typeCache[type] = dictionary;
			}
			if (methodInfo == null && dictionary != null)
			{
				if (array == null)
				{
					array = new Type[_arguments.Length];
					for (int j = 0; j < _arguments.Length; j++)
					{
						array[j] = _arguments[j].GetType();
					}
				}
				methodInfo = this.findMethod(_methodName, type, _returnType, array);
				dictionary[num2] = methodInfo;
			}
		}
		else
		{
			if (array == null)
			{
				array = new Type[_arguments.Length];
				for (int k = 0; k < _arguments.Length; k++)
				{
					array[k] = _arguments[k].GetType();
				}
			}
			methodInfo = this.findMethod(_methodName, type, _returnType, array);
		}
		if (methodInfo != null)
		{
			_returnValue = methodInfo.Invoke(_target, _arguments);
		}
		else if (checkedCall)
		{
			throw new TargetInvocationException("Method signature '" + this.buildMethodSignature(_methodName, _returnType, array) + " does not exist in object type '" + type.FullName, null);
		}
		_returnValue = null;
		return false;
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x001939F2 File Offset: 0x00191BF2
	public void FlushCache()
	{
		this.typeCache.Clear();
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x00193A00 File Offset: 0x00191C00
	[PublicizedFrom(EAccessModifier.Private)]
	public string buildMethodSignature(string _methodName, Type _returnType, Type[] _argTypes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_returnType.Name);
		stringBuilder.Append(" ");
		stringBuilder.Append(_methodName);
		stringBuilder.Append("(");
		for (int i = 0; i < _argTypes.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(_argTypes[i].Name);
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x00193A80 File Offset: 0x00191C80
	[PublicizedFrom(EAccessModifier.Private)]
	public MethodInfo findMethod(string _methodName, Type _target, Type _returnType, Type[] args)
	{
		MethodInfo methodInfo = null;
		Type type = _target;
		while (type != typeof(object))
		{
			methodInfo = _target.GetMethod(_methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.ExactBinding, null, args, null);
			if (methodInfo != null)
			{
				break;
			}
			type = type.BaseType;
		}
		if (methodInfo != null && _returnType != methodInfo.ReturnType && !methodInfo.ReturnType.IsSubclassOf(_returnType))
		{
			methodInfo = null;
		}
		return methodInfo;
	}

	// Token: 0x04003463 RID: 13411
	public static ObjectMessaging Instance = new ObjectMessaging();

	// Token: 0x04003464 RID: 13412
	public const int DYNAMIC_SIGNATURE = 0;

	// Token: 0x04003465 RID: 13413
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Type, Dictionary<int, MethodInfo>> typeCache = new Dictionary<Type, Dictionary<int, MethodInfo>>();

	// Token: 0x020008B8 RID: 2232
	public enum CacheDisposition
	{
		// Token: 0x04003467 RID: 13415
		CacheTypeInfo,
		// Token: 0x04003468 RID: 13416
		Uncached
	}

	// Token: 0x020008B9 RID: 2233
	public sealed class MethodSignature
	{
		// Token: 0x060040B7 RID: 16567 RVA: 0x00193B04 File Offset: 0x00191D04
		public override int GetHashCode()
		{
			if (this.hash == 0)
			{
				this.hash = this.ReturnType.GetHashCode();
				if (this.ArgumentTypes != null && this.ArgumentTypes.Length >= 1)
				{
					for (int i = 0; i < this.ArgumentTypes.Length; i++)
					{
						this.hash ^= this.ArgumentTypes[i].GetHashCode();
					}
				}
			}
			return this.hash;
		}

		// Token: 0x04003469 RID: 13417
		public Type[] ArgumentTypes;

		// Token: 0x0400346A RID: 13418
		public Type ReturnType;

		// Token: 0x0400346B RID: 13419
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}

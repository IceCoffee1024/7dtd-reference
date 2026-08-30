using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// Token: 0x020008AB RID: 2219
public sealed class ObjectMessaging
{
	// Token: 0x06004048 RID: 16456 RVA: 0x001916EE File Offset: 0x0018F8EE
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.CheckedSendMessage(_returnType, methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x00191700 File Offset: 0x0018F900
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, methodName, _methodSignature, _target, _cacheDispostion, out obj, true, _arguments);
		return obj;
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x00191723 File Offset: 0x0018F923
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.SendMessage(_returnType, _methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x00191734 File Offset: 0x0018F934
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, _methodName, _methodSignature, _target, _cacheDispostion, out obj, false, _arguments);
		return obj;
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00191757 File Offset: 0x0018F957
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

	// Token: 0x0600404D RID: 16461 RVA: 0x00191784 File Offset: 0x0018F984
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

	// Token: 0x0600404E RID: 16462 RVA: 0x0019191A File Offset: 0x0018FB1A
	public void FlushCache()
	{
		this.typeCache.Clear();
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x00191928 File Offset: 0x0018FB28
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

	// Token: 0x06004050 RID: 16464 RVA: 0x001919A8 File Offset: 0x0018FBA8
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

	// Token: 0x040033E2 RID: 13282
	public static ObjectMessaging Instance = new ObjectMessaging();

	// Token: 0x040033E3 RID: 13283
	public const int DYNAMIC_SIGNATURE = 0;

	// Token: 0x040033E4 RID: 13284
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Type, Dictionary<int, MethodInfo>> typeCache = new Dictionary<Type, Dictionary<int, MethodInfo>>();

	// Token: 0x020008AC RID: 2220
	public enum CacheDisposition
	{
		// Token: 0x040033E6 RID: 13286
		CacheTypeInfo,
		// Token: 0x040033E7 RID: 13287
		Uncached
	}

	// Token: 0x020008AD RID: 2221
	public sealed class MethodSignature
	{
		// Token: 0x06004052 RID: 16466 RVA: 0x00191A2C File Offset: 0x0018FC2C
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

		// Token: 0x040033E8 RID: 13288
		public Type[] ArgumentTypes;

		// Token: 0x040033E9 RID: 13289
		public Type ReturnType;

		// Token: 0x040033EA RID: 13290
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}

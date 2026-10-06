using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// Token: 0x020008BC RID: 2236
public sealed class ObjectMessaging
{
	// Token: 0x06004043 RID: 16451 RVA: 0x001938CA File Offset: 0x00191ACA
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.CheckedSendMessage(_returnType, methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x001938DC File Offset: 0x00191ADC
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, methodName, _methodSignature, _target, _cacheDispostion, out obj, true, _arguments);
		return obj;
	}

	// Token: 0x06004045 RID: 16453 RVA: 0x001938FF File Offset: 0x00191AFF
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.SendMessage(_returnType, _methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x00193910 File Offset: 0x00191B10
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, _methodName, _methodSignature, _target, _cacheDispostion, out obj, false, _arguments);
		return obj;
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00193933 File Offset: 0x00191B33
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

	// Token: 0x06004048 RID: 16456 RVA: 0x00193960 File Offset: 0x00191B60
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

	// Token: 0x06004049 RID: 16457 RVA: 0x00193AF6 File Offset: 0x00191CF6
	public void FlushCache()
	{
		this.typeCache.Clear();
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x00193B04 File Offset: 0x00191D04
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

	// Token: 0x0600404B RID: 16459 RVA: 0x00193B84 File Offset: 0x00191D84
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

	// Token: 0x04003457 RID: 13399
	public static ObjectMessaging Instance = new ObjectMessaging();

	// Token: 0x04003458 RID: 13400
	public const int DYNAMIC_SIGNATURE = 0;

	// Token: 0x04003459 RID: 13401
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Type, Dictionary<int, MethodInfo>> typeCache = new Dictionary<Type, Dictionary<int, MethodInfo>>();

	// Token: 0x020008BD RID: 2237
	public enum CacheDisposition
	{
		// Token: 0x0400345B RID: 13403
		CacheTypeInfo,
		// Token: 0x0400345C RID: 13404
		Uncached
	}

	// Token: 0x020008BE RID: 2238
	public sealed class MethodSignature
	{
		// Token: 0x0600404D RID: 16461 RVA: 0x00193C08 File Offset: 0x00191E08
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

		// Token: 0x0400345D RID: 13405
		public Type[] ArgumentTypes;

		// Token: 0x0400345E RID: 13406
		public Type ReturnType;

		// Token: 0x0400345F RID: 13407
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}

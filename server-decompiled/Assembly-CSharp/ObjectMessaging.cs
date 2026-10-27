using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// Token: 0x020008C6 RID: 2246
public sealed class ObjectMessaging
{
	// Token: 0x06004066 RID: 16486 RVA: 0x00194972 File Offset: 0x00192B72
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.CheckedSendMessage(_returnType, methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x06004067 RID: 16487 RVA: 0x00194984 File Offset: 0x00192B84
	public object CheckedSendMessage(Type _returnType, string methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, methodName, _methodSignature, _target, _cacheDispostion, out obj, true, _arguments);
		return obj;
	}

	// Token: 0x06004068 RID: 16488 RVA: 0x001949A7 File Offset: 0x00192BA7
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, params object[] _arguments)
	{
		return this.SendMessage(_returnType, _methodName, _methodSignature, _target, ObjectMessaging.CacheDisposition.CacheTypeInfo, _arguments);
	}

	// Token: 0x06004069 RID: 16489 RVA: 0x001949B8 File Offset: 0x00192BB8
	public object SendMessage(Type _returnType, string _methodName, ObjectMessaging.MethodSignature _methodSignature, object _target, ObjectMessaging.CacheDisposition _cacheDispostion, params object[] _arguments)
	{
		object obj = null;
		this.SendMessageEx(_returnType, _methodName, _methodSignature, _target, _cacheDispostion, out obj, false, _arguments);
		return obj;
	}

	// Token: 0x0600406A RID: 16490 RVA: 0x001949DB File Offset: 0x00192BDB
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

	// Token: 0x0600406B RID: 16491 RVA: 0x00194A08 File Offset: 0x00192C08
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

	// Token: 0x0600406C RID: 16492 RVA: 0x00194B9E File Offset: 0x00192D9E
	public void FlushCache()
	{
		this.typeCache.Clear();
	}

	// Token: 0x0600406D RID: 16493 RVA: 0x00194BAC File Offset: 0x00192DAC
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

	// Token: 0x0600406E RID: 16494 RVA: 0x00194C2C File Offset: 0x00192E2C
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

	// Token: 0x04003477 RID: 13431
	public static ObjectMessaging Instance = new ObjectMessaging();

	// Token: 0x04003478 RID: 13432
	public const int DYNAMIC_SIGNATURE = 0;

	// Token: 0x04003479 RID: 13433
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Type, Dictionary<int, MethodInfo>> typeCache = new Dictionary<Type, Dictionary<int, MethodInfo>>();

	// Token: 0x020008C7 RID: 2247
	public enum CacheDisposition
	{
		// Token: 0x0400347B RID: 13435
		CacheTypeInfo,
		// Token: 0x0400347C RID: 13436
		Uncached
	}

	// Token: 0x020008C8 RID: 2248
	public sealed class MethodSignature
	{
		// Token: 0x06004070 RID: 16496 RVA: 0x00194CB0 File Offset: 0x00192EB0
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

		// Token: 0x0400347D RID: 13437
		public Type[] ArgumentTypes;

		// Token: 0x0400347E RID: 13438
		public Type ReturnType;

		// Token: 0x0400347F RID: 13439
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}

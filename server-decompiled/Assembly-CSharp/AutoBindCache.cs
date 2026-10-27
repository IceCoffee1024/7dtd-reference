using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x020010FF RID: 4351
public class AutoBindCache
{
	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x06008A20 RID: 35360 RVA: 0x0034C5B7 File Offset: 0x0034A7B7
	public static AutoBindCache Instance
	{
		get
		{
			AutoBindCache autoBindCache;
			if ((autoBindCache = AutoBindCache.instance) == null)
			{
				autoBindCache = (AutoBindCache.instance = new AutoBindCache());
			}
			return autoBindCache;
		}
	}

	// Token: 0x06008A21 RID: 35361 RVA: 0x0034C5CD File Offset: 0x0034A7CD
	public void BindComponents(XUiController _controller)
	{
		this.getCache(_controller).BindComponents(_controller);
	}

	// Token: 0x06008A22 RID: 35362 RVA: 0x0034C5DC File Offset: 0x0034A7DC
	public void BindEvents(XUiController _controller)
	{
		this.getCache(_controller).BindEvents(_controller);
	}

	// Token: 0x06008A23 RID: 35363 RVA: 0x0034C5EC File Offset: 0x0034A7EC
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoBindCache.AutoBindTypeCache getCache(XUiController _controller)
	{
		Type type = _controller.GetType();
		AutoBindCache.AutoBindTypeCache autoBindTypeCache;
		if (!this.cachePerType.TryGetValue(type, out autoBindTypeCache))
		{
			autoBindTypeCache = new AutoBindCache.AutoBindTypeCache(type);
			this.cachePerType[type] = autoBindTypeCache;
		}
		return autoBindTypeCache;
	}

	// Token: 0x040066C2 RID: 26306
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, AutoBindCache.AutoBindTypeCache> cachePerType = new Dictionary<Type, AutoBindCache.AutoBindTypeCache>(128);

	// Token: 0x040066C3 RID: 26307
	[PublicizedFrom(EAccessModifier.Private)]
	public static AutoBindCache instance;

	// Token: 0x040066C4 RID: 26308
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUiController> controllersList = new List<XUiController>();

	// Token: 0x040066C5 RID: 26309
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUiView> viewsList = new List<XUiView>();

	// Token: 0x02001100 RID: 4352
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindComponentInfo
	{
		// Token: 0x06008A26 RID: 35366 RVA: 0x0034C654 File Offset: 0x0034A854
		public BindComponentInfo(FieldInfo _targetField, AutoBindCache.BindComponentInfo.EFieldXuiType _fieldXuiType, AutoBindCache.BindComponentInfo.EMultiplicity _multiplicity, Type _lookupType, string _lookupId, bool _required, bool _isParentLookup)
		{
			this.targetField = _targetField;
			this.fieldXuiType = _fieldXuiType;
			this.multiplicity = _multiplicity;
			this.lookupType = _lookupType;
			this.lookupId = _lookupId ?? "";
			this.required = _required;
			this.parentLookup = _isParentLookup;
		}

		// Token: 0x06008A27 RID: 35367 RVA: 0x0034C6A8 File Offset: 0x0034A8A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void bindParent(XUiController _controller)
		{
			bool flag;
			object obj;
			if (this.fieldXuiType == AutoBindCache.BindComponentInfo.EFieldXuiType.Controller)
			{
				XUiController xuiController;
				flag = _controller.TryGetParentController(this.lookupType, out xuiController);
				obj = xuiController;
			}
			else
			{
				XUiView xuiView;
				flag = _controller.TryGetParentView(this.lookupType, out xuiView);
				obj = xuiView;
			}
			if (flag)
			{
				this.targetField.SetValue(_controller, obj);
				return;
			}
			if (this.required)
			{
				this.LogNoViewFound(_controller, "Could not find matching controller / view for auto bind parent field.");
				return;
			}
		}

		// Token: 0x06008A28 RID: 35368 RVA: 0x0034C708 File Offset: 0x0034A908
		public void Bind(XUiController _controller)
		{
			if (this.parentLookup)
			{
				this.bindParent(_controller);
				return;
			}
			XUiController xuiController = _controller;
			ReadOnlyMemory<char> readOnlyMemory = this.lookupId.AsMemory();
			for (int i = readOnlyMemory.Span.IndexOf('.'); i > 0; i = readOnlyMemory.Span.IndexOf('.'))
			{
				ReadOnlyMemory<char> readOnlyMemory2 = readOnlyMemory.Slice(0, i);
				readOnlyMemory = readOnlyMemory.Slice(i + 1);
				XUiController xuiController2;
				if (xuiController.TryGetChildController(typeof(XUiController), readOnlyMemory2, out xuiController2))
				{
					xuiController = xuiController2;
				}
				else if (this.required)
				{
					this.LogNoViewFound(_controller, "Could not find matching parent view for auto bind field.");
					return;
				}
			}
			AutoBindCache.BindComponentInfo.EMultiplicity emultiplicity = this.multiplicity;
			if (emultiplicity != AutoBindCache.BindComponentInfo.EMultiplicity.Single)
			{
				if (emultiplicity != AutoBindCache.BindComponentInfo.EMultiplicity.Array)
				{
					Log.Error(string.Format("[XUi] Auto bind field: Multiplicity value '{0}' not known.", this.multiplicity));
					return;
				}
				IList list;
				if (this.fieldXuiType == AutoBindCache.BindComponentInfo.EFieldXuiType.Controller)
				{
					list = AutoBindCache.controllersList;
					list.Clear();
					xuiController.GetChildControllers(this.lookupType, readOnlyMemory, AutoBindCache.controllersList);
				}
				else
				{
					list = AutoBindCache.viewsList;
					list.Clear();
					xuiController.GetChildViews(this.lookupType, readOnlyMemory, AutoBindCache.viewsList);
				}
				Array array = Array.CreateInstance(this.targetField.FieldType.GetElementType(), list.Count);
				for (int j = 0; j < list.Count; j++)
				{
					array.SetValue(list[j], j);
				}
				this.targetField.SetValue(_controller, array);
				if (list.Count == 0 && this.required)
				{
					this.LogNoViewFound(_controller, "Could not find any matching views for auto bind field.");
				}
				list.Clear();
				return;
			}
			else
			{
				bool flag;
				object obj;
				if (this.fieldXuiType == AutoBindCache.BindComponentInfo.EFieldXuiType.Controller)
				{
					XUiController xuiController3;
					flag = xuiController.TryGetChildController(this.lookupType, readOnlyMemory, out xuiController3);
					obj = xuiController3;
				}
				else
				{
					XUiView xuiView;
					flag = xuiController.TryGetChildView(this.lookupType, readOnlyMemory, out xuiView);
					obj = xuiView;
				}
				if (flag)
				{
					this.targetField.SetValue(_controller, obj);
					return;
				}
				if (this.required)
				{
					this.LogNoViewFound(_controller, "Could not find matching view for auto bind field.");
					return;
				}
				return;
			}
		}

		// Token: 0x06008A29 RID: 35369 RVA: 0x0034C8F0 File Offset: 0x0034AAF0
		[PublicizedFrom(EAccessModifier.Private)]
		public void LogNoViewFound(XUiController _controller, string _message)
		{
			Log.Error(string.IsNullOrEmpty(this.lookupId) ? string.Concat(new string[]
			{
				"[XUi] ",
				_message,
				" ",
				this.targetField.DeclaringType.FullName,
				".",
				this.targetField.Name,
				" (expected type: ",
				this.lookupType.Name,
				"). Hierarchy: ",
				_controller.GetXuiHierarchy()
			}) : string.Concat(new string[]
			{
				"[XUi] ",
				_message,
				" ",
				this.targetField.DeclaringType.FullName,
				".",
				this.targetField.Name,
				" (expected type: ",
				this.lookupType.Name,
				", expected name: '",
				this.lookupId,
				"'). Hierarchy: ",
				_controller.GetXuiHierarchy()
			}));
		}

		// Token: 0x040066C6 RID: 26310
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly FieldInfo targetField;

		// Token: 0x040066C7 RID: 26311
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoBindCache.BindComponentInfo.EFieldXuiType fieldXuiType;

		// Token: 0x040066C8 RID: 26312
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoBindCache.BindComponentInfo.EMultiplicity multiplicity;

		// Token: 0x040066C9 RID: 26313
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type lookupType;

		// Token: 0x040066CA RID: 26314
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string lookupId;

		// Token: 0x040066CB RID: 26315
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool required;

		// Token: 0x040066CC RID: 26316
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool parentLookup;

		// Token: 0x02001101 RID: 4353
		public enum EFieldXuiType
		{
			// Token: 0x040066CE RID: 26318
			Controller,
			// Token: 0x040066CF RID: 26319
			View
		}

		// Token: 0x02001102 RID: 4354
		public enum EMultiplicity
		{
			// Token: 0x040066D1 RID: 26321
			Single,
			// Token: 0x040066D2 RID: 26322
			Array
		}
	}

	// Token: 0x02001103 RID: 4355
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindEventInfo
	{
		// Token: 0x06008A2A RID: 35370 RVA: 0x0034CA09 File Offset: 0x0034AC09
		public BindEventInfo(EventInfo _eventInfo, FieldInfo _eventComponentField, MethodInfo _eventMethod)
		{
			this.eventInfo = _eventInfo;
			this.eventComponentField = _eventComponentField;
			this.eventMethod = _eventMethod;
		}

		// Token: 0x06008A2B RID: 35371 RVA: 0x0034CA28 File Offset: 0x0034AC28
		public void Bind(XUiController _controller)
		{
			Delegate @delegate = this.eventMethod.CreateDelegate(this.eventInfo.EventHandlerType, _controller);
			if (this.eventComponentField == null)
			{
				this.eventInfo.AddEventHandler(_controller, @delegate);
				return;
			}
			if (!this.eventComponentField.FieldType.IsArray)
			{
				object value = this.eventComponentField.GetValue(_controller);
				if (value == null)
				{
					if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
					{
						Log.Warning("[XUi] Failed binding event on component to method: Component field is null. Field " + this.eventComponentField.DeclaringType.FullName + "." + this.eventComponentField.Name);
					}
					return;
				}
				this.eventInfo.AddEventHandler(value, @delegate);
				return;
			}
			else
			{
				Array array = (Array)this.eventComponentField.GetValue(_controller);
				if (array == null)
				{
					if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
					{
						Log.Warning("[XUi] Failed binding event on components array to method: Component array field is null. Field " + this.eventComponentField.DeclaringType.FullName + "." + this.eventComponentField.Name);
					}
					return;
				}
				for (int i = 0; i < array.Length; i++)
				{
					object value2 = array.GetValue(i);
					if (value2 == null)
					{
						Log.Error(string.Format("[XUi] Failed binding event on component list to method: Entry {0} is null. Field {1}.{2}", i, this.eventComponentField.DeclaringType.FullName, this.eventComponentField.Name));
					}
					else
					{
						this.eventInfo.AddEventHandler(value2, @delegate);
					}
				}
				return;
			}
		}

		// Token: 0x040066D3 RID: 26323
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EventInfo eventInfo;

		// Token: 0x040066D4 RID: 26324
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly FieldInfo eventComponentField;

		// Token: 0x040066D5 RID: 26325
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MethodInfo eventMethod;
	}

	// Token: 0x02001104 RID: 4356
	[PublicizedFrom(EAccessModifier.Private)]
	public class AutoBindTypeCache
	{
		// Token: 0x06008A2C RID: 35372 RVA: 0x0034CB7C File Offset: 0x0034AD7C
		public AutoBindTypeCache(Type _controllerType)
		{
			this.controllerType = _controllerType;
			this.initCacheForController();
		}

		// Token: 0x06008A2D RID: 35373 RVA: 0x0034CBA9 File Offset: 0x0034ADA9
		[PublicizedFrom(EAccessModifier.Private)]
		public void initCacheForController()
		{
			this.<initCacheForController>g__RecurseTypeTree|4_0(this.controllerType);
		}

		// Token: 0x06008A2E RID: 35374 RVA: 0x0034CBB8 File Offset: 0x0034ADB8
		public void BindComponents(XUiController _controller)
		{
			for (int i = 0; i < this.fields.Count; i++)
			{
				this.fields[i].Bind(_controller);
			}
		}

		// Token: 0x06008A2F RID: 35375 RVA: 0x0034CBF0 File Offset: 0x0034ADF0
		public void BindEvents(XUiController _controller)
		{
			for (int i = 0; i < this.events.Count; i++)
			{
				this.events[i].Bind(_controller);
			}
		}

		// Token: 0x06008A30 RID: 35376 RVA: 0x0034CC28 File Offset: 0x0034AE28
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__RecurseTypeTree|4_0(Type _type)
		{
			if (_type == null)
			{
				return;
			}
			this.<initCacheForController>g__RecurseTypeTree|4_0(_type.BaseType);
			ReflectionHelpers.GetFieldsWithAttribute<XuiBindComponentAttribute>(_type, new Action<FieldInfo, bool, XuiBindComponentAttribute>(this.<initCacheForController>g__ComponentFieldFoundCallback|4_2), true, true, false);
			ReflectionHelpers.GetFieldsWithAttribute<XuiBindParentAttribute>(_type, new Action<FieldInfo, bool, XuiBindParentAttribute>(this.<initCacheForController>g__ParentFieldFoundCallback|4_1), true, true, false);
			ReflectionHelpers.GetMethodsWithAttribute<XuiBindEventAttribute>(_type, new Action<MethodInfo, bool, XuiBindEventAttribute>(this.<initCacheForController>g__EventMethodFoundCallback|4_4), true, true, false);
		}

		// Token: 0x06008A31 RID: 35377 RVA: 0x0034CC8A File Offset: 0x0034AE8A
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__ParentFieldFoundCallback|4_1(FieldInfo _fieldInfo, bool _hasMultiple, XuiBindParentAttribute _attribute)
		{
			this.<initCacheForController>g__FieldFound|4_3(_fieldInfo, _hasMultiple, null, _attribute.Required, true);
		}

		// Token: 0x06008A32 RID: 35378 RVA: 0x0034CC9C File Offset: 0x0034AE9C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__ComponentFieldFoundCallback|4_2(FieldInfo _fieldInfo, bool _hasMultiple, XuiBindComponentAttribute _attribute)
		{
			this.<initCacheForController>g__FieldFound|4_3(_fieldInfo, _hasMultiple, _attribute.XmlElementName, _attribute.Required, false);
		}

		// Token: 0x06008A33 RID: 35379 RVA: 0x0034CCB4 File Offset: 0x0034AEB4
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__FieldFound|4_3(FieldInfo _fieldInfo, bool _hasMultiple, string _xmlElementName, bool _required, bool _isParentLookup)
		{
			Type fieldType = _fieldInfo.FieldType;
			bool isArray = fieldType.IsArray;
			if (_isParentLookup && isArray)
			{
				throw new Exception("[XUi] Field marked as XuiBindParent is an array. Field " + _fieldInfo.DeclaringType.FullName + "." + _fieldInfo.Name);
			}
			Type type = (isArray ? fieldType.GetElementType() : fieldType);
			AutoBindCache.BindComponentInfo.EFieldXuiType efieldXuiType;
			if (typeof(XUiController).IsAssignableFrom(type))
			{
				efieldXuiType = AutoBindCache.BindComponentInfo.EFieldXuiType.Controller;
			}
			else
			{
				if (!typeof(XUiView).IsAssignableFrom(type))
				{
					throw new Exception("[XUi] Field marked as XuiBindComponent is neither a XUiController nor a XUiView or descendant. Field " + _fieldInfo.DeclaringType.FullName + "." + _fieldInfo.Name);
				}
				efieldXuiType = AutoBindCache.BindComponentInfo.EFieldXuiType.View;
			}
			AutoBindCache.BindComponentInfo.EMultiplicity emultiplicity = (isArray ? AutoBindCache.BindComponentInfo.EMultiplicity.Array : AutoBindCache.BindComponentInfo.EMultiplicity.Single);
			AutoBindCache.BindComponentInfo bindComponentInfo = new AutoBindCache.BindComponentInfo(_fieldInfo, efieldXuiType, emultiplicity, type, _xmlElementName, _required, _isParentLookup);
			this.fields.Add(bindComponentInfo);
		}

		// Token: 0x06008A34 RID: 35380 RVA: 0x0034CD84 File Offset: 0x0034AF84
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__EventMethodFoundCallback|4_4(MethodInfo _methodInfo, bool _hasMultiple, XuiBindEventAttribute _attribute)
		{
			Type declaringType = _methodInfo.DeclaringType;
			foreach (XuiBindEventAttribute xuiBindEventAttribute in _methodInfo.GetCustomAttributes<XuiBindEventAttribute>())
			{
				string componentFieldName = xuiBindEventAttribute.ComponentFieldName;
				FieldInfo fieldInfo = null;
				Type type;
				if (string.IsNullOrEmpty(componentFieldName))
				{
					type = declaringType;
				}
				else
				{
					fieldInfo = declaringType.GetField(componentFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (fieldInfo == null)
					{
						Log.Error(string.Concat(new string[] { "[XUi] Event method component field '", componentFieldName, "' not found on type ", declaringType.FullName, ". Method ", declaringType.FullName, ".", _methodInfo.Name }));
						break;
					}
					Type fieldType = fieldInfo.FieldType;
					type = (fieldType.IsArray ? fieldType.GetElementType() : fieldType);
				}
				string targetEvent = xuiBindEventAttribute.TargetEvent;
				if (string.IsNullOrEmpty(targetEvent))
				{
					Log.Error("[XUi] Event method without non-empty event name. Method " + declaringType.FullName + "." + _methodInfo.Name);
					break;
				}
				EventInfo @event = type.GetEvent(targetEvent, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (@event == null)
				{
					Log.Error(string.Concat(new string[] { "[XUi] Event method target event '", targetEvent, "' not found on type '", type.FullName, "'. Method ", declaringType.FullName, ".", _methodInfo.Name }));
					break;
				}
				if (!ReflectionHelpers.MethodCompatibleWithDelegate(@event.EventHandlerType, _methodInfo, false))
				{
					Log.Error(string.Concat(new string[] { "[XUi] Event method incompatible with target event '", type.FullName, ".", targetEvent, "'. Method ", declaringType.FullName, ".", _methodInfo.Name }));
					break;
				}
				AutoBindCache.BindEventInfo bindEventInfo = new AutoBindCache.BindEventInfo(@event, fieldInfo, _methodInfo);
				this.events.Add(bindEventInfo);
			}
		}

		// Token: 0x040066D6 RID: 26326
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerType;

		// Token: 0x040066D7 RID: 26327
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AutoBindCache.BindComponentInfo> fields = new List<AutoBindCache.BindComponentInfo>(64);

		// Token: 0x040066D8 RID: 26328
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AutoBindCache.BindEventInfo> events = new List<AutoBindCache.BindEventInfo>();
	}
}

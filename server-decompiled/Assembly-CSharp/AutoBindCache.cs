using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

// Token: 0x020010C5 RID: 4293
public class AutoBindCache
{
	// Token: 0x17000FB9 RID: 4025
	// (get) Token: 0x0600888C RID: 34956 RVA: 0x00341F32 File Offset: 0x00340132
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

	// Token: 0x0600888D RID: 34957 RVA: 0x00341F48 File Offset: 0x00340148
	public void BindComponents(XUiController _controller)
	{
		this.getCache(_controller).BindComponents(_controller);
	}

	// Token: 0x0600888E RID: 34958 RVA: 0x00341F57 File Offset: 0x00340157
	public void BindEvents(XUiController _controller)
	{
		this.getCache(_controller).BindEvents(_controller);
	}

	// Token: 0x0600888F RID: 34959 RVA: 0x00341F68 File Offset: 0x00340168
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

	// Token: 0x04006579 RID: 25977
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, AutoBindCache.AutoBindTypeCache> cachePerType = new Dictionary<Type, AutoBindCache.AutoBindTypeCache>(128);

	// Token: 0x0400657A RID: 25978
	[PublicizedFrom(EAccessModifier.Private)]
	public static AutoBindCache instance;

	// Token: 0x0400657B RID: 25979
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUiController> controllersList = new List<XUiController>();

	// Token: 0x0400657C RID: 25980
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUiView> viewsList = new List<XUiView>();

	// Token: 0x020010C6 RID: 4294
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindComponentInfo
	{
		// Token: 0x06008892 RID: 34962 RVA: 0x00341FD0 File Offset: 0x003401D0
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

		// Token: 0x06008893 RID: 34963 RVA: 0x00342024 File Offset: 0x00340224
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

		// Token: 0x06008894 RID: 34964 RVA: 0x00342084 File Offset: 0x00340284
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

		// Token: 0x06008895 RID: 34965 RVA: 0x0034226C File Offset: 0x0034046C
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

		// Token: 0x0400657D RID: 25981
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly FieldInfo targetField;

		// Token: 0x0400657E RID: 25982
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoBindCache.BindComponentInfo.EFieldXuiType fieldXuiType;

		// Token: 0x0400657F RID: 25983
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoBindCache.BindComponentInfo.EMultiplicity multiplicity;

		// Token: 0x04006580 RID: 25984
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type lookupType;

		// Token: 0x04006581 RID: 25985
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string lookupId;

		// Token: 0x04006582 RID: 25986
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool required;

		// Token: 0x04006583 RID: 25987
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool parentLookup;

		// Token: 0x020010C7 RID: 4295
		public enum EFieldXuiType
		{
			// Token: 0x04006585 RID: 25989
			Controller,
			// Token: 0x04006586 RID: 25990
			View
		}

		// Token: 0x020010C8 RID: 4296
		public enum EMultiplicity
		{
			// Token: 0x04006588 RID: 25992
			Single,
			// Token: 0x04006589 RID: 25993
			Array
		}
	}

	// Token: 0x020010C9 RID: 4297
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindEventInfo
	{
		// Token: 0x06008896 RID: 34966 RVA: 0x00342385 File Offset: 0x00340585
		public BindEventInfo(EventInfo _eventInfo, FieldInfo _eventComponentField, MethodInfo _eventMethod)
		{
			this.eventInfo = _eventInfo;
			this.eventComponentField = _eventComponentField;
			this.eventMethod = _eventMethod;
		}

		// Token: 0x06008897 RID: 34967 RVA: 0x003423A4 File Offset: 0x003405A4
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

		// Token: 0x0400658A RID: 25994
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EventInfo eventInfo;

		// Token: 0x0400658B RID: 25995
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly FieldInfo eventComponentField;

		// Token: 0x0400658C RID: 25996
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MethodInfo eventMethod;
	}

	// Token: 0x020010CA RID: 4298
	[PublicizedFrom(EAccessModifier.Private)]
	public class AutoBindTypeCache
	{
		// Token: 0x06008898 RID: 34968 RVA: 0x003424F8 File Offset: 0x003406F8
		public AutoBindTypeCache(Type _controllerType)
		{
			this.controllerType = _controllerType;
			this.initCacheForController();
		}

		// Token: 0x06008899 RID: 34969 RVA: 0x00342525 File Offset: 0x00340725
		[PublicizedFrom(EAccessModifier.Private)]
		public void initCacheForController()
		{
			this.<initCacheForController>g__RecurseTypeTree|4_0(this.controllerType);
		}

		// Token: 0x0600889A RID: 34970 RVA: 0x00342534 File Offset: 0x00340734
		public void BindComponents(XUiController _controller)
		{
			for (int i = 0; i < this.fields.Count; i++)
			{
				this.fields[i].Bind(_controller);
			}
		}

		// Token: 0x0600889B RID: 34971 RVA: 0x0034256C File Offset: 0x0034076C
		public void BindEvents(XUiController _controller)
		{
			for (int i = 0; i < this.events.Count; i++)
			{
				this.events[i].Bind(_controller);
			}
		}

		// Token: 0x0600889C RID: 34972 RVA: 0x003425A4 File Offset: 0x003407A4
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

		// Token: 0x0600889D RID: 34973 RVA: 0x00342606 File Offset: 0x00340806
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__ParentFieldFoundCallback|4_1(FieldInfo _fieldInfo, bool _hasMultiple, XuiBindParentAttribute _attribute)
		{
			this.<initCacheForController>g__FieldFound|4_3(_fieldInfo, _hasMultiple, null, _attribute.Required, true);
		}

		// Token: 0x0600889E RID: 34974 RVA: 0x00342618 File Offset: 0x00340818
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <initCacheForController>g__ComponentFieldFoundCallback|4_2(FieldInfo _fieldInfo, bool _hasMultiple, XuiBindComponentAttribute _attribute)
		{
			this.<initCacheForController>g__FieldFound|4_3(_fieldInfo, _hasMultiple, _attribute.XmlElementName, _attribute.Required, false);
		}

		// Token: 0x0600889F RID: 34975 RVA: 0x00342630 File Offset: 0x00340830
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

		// Token: 0x060088A0 RID: 34976 RVA: 0x00342700 File Offset: 0x00340900
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

		// Token: 0x0400658D RID: 25997
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Type controllerType;

		// Token: 0x0400658E RID: 25998
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AutoBindCache.BindComponentInfo> fields = new List<AutoBindCache.BindComponentInfo>(64);

		// Token: 0x0400658F RID: 25999
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AutoBindCache.BindEventInfo> events = new List<AutoBindCache.BindEventInfo>();
	}
}

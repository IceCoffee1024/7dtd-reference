using System;
using System.Collections.Generic;
using Platform;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EBB RID: 3771
[Preserve]
public class XUiController : IXUiElement
{
	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x06007580 RID: 30080 RVA: 0x002C2260 File Offset: 0x002C0460
	// (set) Token: 0x06007581 RID: 30081 RVA: 0x002C2268 File Offset: 0x002C0468
	public XUiView ViewComponent
	{
		get
		{
			return this.viewComponent;
		}
		set
		{
			this.viewComponent = value;
		}
	}

	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x06007582 RID: 30082 RVA: 0x002C2271 File Offset: 0x002C0471
	// (set) Token: 0x06007583 RID: 30083 RVA: 0x002C227C File Offset: 0x002C047C
	public XUiController Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			if (this.parent == value)
			{
				return;
			}
			XUiController xuiController = this.parent;
			if (xuiController != null)
			{
				xuiController.children.Remove(this);
			}
			this.parent = value;
			XUiController xuiController2 = this.parent;
			if (xuiController2 == null)
			{
				return;
			}
			xuiController2.children.Add(this);
		}
	}

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x06007584 RID: 30084 RVA: 0x002C22C8 File Offset: 0x002C04C8
	public List<XUiController> Children
	{
		get
		{
			return this.children;
		}
	}

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x06007585 RID: 30085 RVA: 0x002C22D0 File Offset: 0x002C04D0
	// (set) Token: 0x06007586 RID: 30086 RVA: 0x002C22D8 File Offset: 0x002C04D8
	public XUiWindowGroup WindowGroup
	{
		get
		{
			return this.windowGroup;
		}
		set
		{
			this.windowGroup = value;
		}
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x06007587 RID: 30087 RVA: 0x002C22E1 File Offset: 0x002C04E1
	public bool IsOpen
	{
		get
		{
			return this.windowGroup.isShowing;
		}
	}

	// Token: 0x06007588 RID: 30088 RVA: 0x002C22EE File Offset: 0x002C04EE
	public void AutoBindComponents()
	{
		AutoBindCache.Instance.BindComponents(this);
	}

	// Token: 0x06007589 RID: 30089 RVA: 0x002C22FB File Offset: 0x002C04FB
	public void AutoBindEvents()
	{
		AutoBindCache.Instance.BindEvents(this);
	}

	// Token: 0x0600758A RID: 30090 RVA: 0x002C2308 File Offset: 0x002C0508
	public virtual void Init()
	{
		if (this.viewComponent != null)
		{
			this.viewComponent.InitView();
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].AutoBindComponents();
			this.children[i].Init();
			this.children[i].AutoBindEvents();
		}
		this.curInputStyle = PlatformManager.NativePlatform.Input.CurrentInputStyle;
	}

	// Token: 0x0600758B RID: 30091 RVA: 0x002C2388 File Offset: 0x002C0588
	public virtual void Update(float _dt)
	{
		XUiView xuiView = this.viewComponent;
		bool isShowing = this.windowGroup.isShowing;
		if (this.forceInputStyleChange || this.curInputStyle != this.lastInputStyle)
		{
			this.forceInputStyleChange = false;
			PlayerInputManager.InputStyle inputStyle = this.lastInputStyle;
			this.lastInputStyle = this.curInputStyle;
			this.RefreshBindings();
			this.inputStyleChanged(inputStyle, this.lastInputStyle);
		}
		if (isShowing && xuiView != null && xuiView.IsVisible)
		{
			using (this.pmVcUpdate.Auto())
			{
				using (this.pmVcNamedUpdate.Auto())
				{
					xuiView.Update(_dt);
				}
			}
		}
		List<XUiController> list = this.children;
		for (int i = 0; i < list.Count; i++)
		{
			XUiController xuiController = list[i];
			if (isShowing || xuiController.AlwaysUpdate)
			{
				xuiController.Update(_dt);
			}
		}
	}

	// Token: 0x0600758C RID: 30092 RVA: 0x002C2490 File Offset: 0x002C0690
	public bool TryGetChildController(Type _controllerType, ReadOnlyMemory<char> _id, out XUiController _child)
	{
		if ((_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase))) && _controllerType.IsInstanceOfType(this))
		{
			_child = this;
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildController(_controllerType, _id, out _child))
			{
				return true;
			}
		}
		_child = null;
		return false;
	}

	// Token: 0x0600758D RID: 30093 RVA: 0x002C2510 File Offset: 0x002C0710
	public bool TryGetChildController<T>(ReadOnlyMemory<char> _id, out T _child) where T : XUiController
	{
		if (_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)))
		{
			T t = this as T;
			if (t != null)
			{
				_child = t;
				return true;
			}
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildController<T>(_id, out _child))
			{
				return true;
			}
		}
		_child = default(T);
		return false;
	}

	// Token: 0x0600758E RID: 30094 RVA: 0x002C259F File Offset: 0x002C079F
	public bool TryGetChildController<T>(string _id, out T _child) where T : XUiController
	{
		return this.TryGetChildController<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x0600758F RID: 30095 RVA: 0x002C25B0 File Offset: 0x002C07B0
	public XUiController[] GetChildControllers(Type _controllerType, ReadOnlyMemory<char> _id, List<XUiController> _list = null)
	{
		List<XUiController> list = _list ?? new List<XUiController>();
		if ((_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase))) && _controllerType.IsInstanceOfType(this))
		{
			list.Add(this);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildControllers(_controllerType, _id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007590 RID: 30096 RVA: 0x002C2640 File Offset: 0x002C0840
	public T[] GetChildControllers<T>(ReadOnlyMemory<char> _id, List<T> _list = null) where T : XUiController
	{
		List<T> list = _list ?? new List<T>();
		if (_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)))
		{
			T t = this as T;
			if (t != null)
			{
				list.Add(t);
				goto IL_0082;
			}
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].GetChildControllers<T>(_id, list);
		}
		IL_0082:
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007591 RID: 30097 RVA: 0x002C26DA File Offset: 0x002C08DA
	public T[] GetChildControllers<T>(string _id, List<T> _list = null) where T : XUiController
	{
		return this.GetChildControllers<T>(_id.AsMemory(), _list);
	}

	// Token: 0x06007592 RID: 30098 RVA: 0x002C26EC File Offset: 0x002C08EC
	public bool TryGetChildView(Type _viewType, ReadOnlyMemory<char> _id, out XUiView _child)
	{
		if (this.viewComponent != null && (_id.IsEmpty || _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)) && _viewType.IsInstanceOfType(this.viewComponent))
		{
			_child = this.viewComponent;
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildView(_viewType, _id, out _child))
			{
				return true;
			}
		}
		_child = null;
		return false;
	}

	// Token: 0x06007593 RID: 30099 RVA: 0x002C2774 File Offset: 0x002C0974
	public bool TryGetChildView<T>(ReadOnlyMemory<char> _id, out T _child) where T : XUiView
	{
		T t = this.viewComponent as T;
		if (t != null && (_id.IsEmpty || _id.Span.Equals(t.ID, StringComparison.OrdinalIgnoreCase)))
		{
			_child = t;
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildView<T>(_id, out _child))
			{
				return true;
			}
		}
		_child = default(T);
		return false;
	}

	// Token: 0x06007594 RID: 30100 RVA: 0x002C2800 File Offset: 0x002C0A00
	public bool TryGetChildView<T>(string _id, out T _child) where T : XUiView
	{
		return this.TryGetChildView<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x06007595 RID: 30101 RVA: 0x002C2810 File Offset: 0x002C0A10
	public XUiView[] GetChildViews(Type _viewType, ReadOnlyMemory<char> _id, List<XUiView> _list = null)
	{
		List<XUiView> list = _list ?? new List<XUiView>();
		if (this.viewComponent != null && (_id.IsEmpty || _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)) && _viewType.IsInstanceOfType(this.viewComponent))
		{
			list.Add(this.viewComponent);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildViews(_viewType, _id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007596 RID: 30102 RVA: 0x002C28AC File Offset: 0x002C0AAC
	public T[] GetChildViews<T>(ReadOnlyMemory<char> _id, List<T> _list = null) where T : XUiView
	{
		List<T> list = _list ?? new List<T>();
		T t = this.viewComponent as T;
		if (t != null && (_id.IsEmpty || _id.Span.Equals(t.ID, StringComparison.OrdinalIgnoreCase)))
		{
			list.Add(t);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildViews<T>(_id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007597 RID: 30103 RVA: 0x002C2943 File Offset: 0x002C0B43
	public T[] GetChildViews<T>(string _id, List<T> _list = null) where T : XUiView
	{
		return this.GetChildViews<T>(_id.AsMemory(), _list);
	}

	// Token: 0x06007598 RID: 30104 RVA: 0x002C2954 File Offset: 0x002C0B54
	public bool TryGetChildByIdAndType<T>(string _id, out T _child) where T : XUiController
	{
		if (this.viewComponent != null && string.Equals(this.viewComponent.ID, _id, StringComparison.OrdinalIgnoreCase))
		{
			T t = this as T;
			if (t != null)
			{
				_child = t;
				return true;
			}
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildByIdAndType<T>(_id, out _child))
			{
				return true;
			}
		}
		_child = default(T);
		return false;
	}

	// Token: 0x06007599 RID: 30105 RVA: 0x002C29D0 File Offset: 0x002C0BD0
	public XUiController GetChildById(string _id)
	{
		if (this.viewComponent != null && string.Equals(this.viewComponent.ID, _id, StringComparison.OrdinalIgnoreCase))
		{
			return this;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiController childById = this.children[i].GetChildById(_id);
			if (childById != null)
			{
				return childById;
			}
		}
		return null;
	}

	// Token: 0x0600759A RID: 30106 RVA: 0x002C2A2C File Offset: 0x002C0C2C
	public XUiController[] GetChildrenById(string _id, List<XUiController> _list = null)
	{
		List<XUiController> list = _list ?? new List<XUiController>();
		if (this.viewComponent != null && string.Equals(this.viewComponent.ID, _id, StringComparison.OrdinalIgnoreCase))
		{
			list.Add(this);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildrenById(_id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x0600759B RID: 30107 RVA: 0x002C2AA0 File Offset: 0x002C0CA0
	public T GetChildByType<T>() where T : XUiController
	{
		T t = this as T;
		if (t != null)
		{
			return t;
		}
		foreach (XUiController xuiController in this.children)
		{
			t = xuiController.GetChildByType<T>();
			if (t != null)
			{
				return t;
			}
		}
		return default(T);
	}

	// Token: 0x0600759C RID: 30108 RVA: 0x002C2B20 File Offset: 0x002C0D20
	public T[] GetChildrenByType<T>(List<T> _list = null) where T : XUiController
	{
		List<T> list = _list ?? new List<T>();
		T t = this as T;
		if (t != null)
		{
			list.Add(t);
		}
		else
		{
			foreach (XUiController xuiController in this.children)
			{
				xuiController.GetChildrenByType<T>(list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x0600759D RID: 30109 RVA: 0x002C2BA8 File Offset: 0x002C0DA8
	public T[] GetChildrenByViewType<T>(List<T> _list = null) where T : XUiView
	{
		List<T> list = _list ?? new List<T>();
		T t = this.viewComponent as T;
		if (t != null)
		{
			list.Add(t);
		}
		else
		{
			foreach (XUiController xuiController in this.children)
			{
				xuiController.GetChildrenByViewType<T>(list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x0600759E RID: 30110 RVA: 0x002C2C34 File Offset: 0x002C0E34
	public T GetParentByType<T>() where T : XUiController
	{
		T t = this as T;
		if (t != null)
		{
			return t;
		}
		XUiController xuiController = this.Parent;
		if (xuiController == null)
		{
			return default(T);
		}
		return xuiController.GetParentByType<T>();
	}

	// Token: 0x0600759F RID: 30111 RVA: 0x002C2C70 File Offset: 0x002C0E70
	public bool TryGetParentController(Type _controllerType, out XUiController _parent)
	{
		if (_controllerType.IsInstanceOfType(this))
		{
			_parent = this;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentController(_controllerType, out _parent);
		}
		_parent = null;
		return false;
	}

	// Token: 0x060075A0 RID: 30112 RVA: 0x002C2C9C File Offset: 0x002C0E9C
	public bool TryGetParentController<T>(out T _parent) where T : XUiController
	{
		T t = this as T;
		if (t != null)
		{
			_parent = t;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentController<T>(out _parent);
		}
		_parent = default(T);
		return false;
	}

	// Token: 0x060075A1 RID: 30113 RVA: 0x002C2CE3 File Offset: 0x002C0EE3
	public bool TryGetParentView(Type _viewType, out XUiView _parent)
	{
		if (_viewType.IsInstanceOfType(this.viewComponent))
		{
			_parent = this.viewComponent;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentView(_viewType, out _parent);
		}
		_parent = null;
		return false;
	}

	// Token: 0x060075A2 RID: 30114 RVA: 0x002C2D18 File Offset: 0x002C0F18
	public bool TryGetParentView<T>(out T _parent) where T : XUiView
	{
		T t = this.viewComponent as T;
		if (t != null)
		{
			_parent = t;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentView<T>(out _parent);
		}
		_parent = default(T);
		return false;
	}

	// Token: 0x060075A3 RID: 30115 RVA: 0x002C2D64 File Offset: 0x002C0F64
	public bool IsChildOf(XUiController _controller)
	{
		return this.Parent != null && (this.Parent == _controller || this.Parent.IsChildOf(_controller));
	}

	// Token: 0x060075A4 RID: 30116 RVA: 0x002C2D87 File Offset: 0x002C0F87
	public bool IsSelfOrChildOf(XUiController _controller)
	{
		return this == _controller || this.IsChildOf(_controller);
	}

	// Token: 0x060075A5 RID: 30117 RVA: 0x002C2D98 File Offset: 0x002C0F98
	public XUiV_Window GetParentWindow()
	{
		XUiV_Window xuiV_Window = this.ViewComponent as XUiV_Window;
		if (xuiV_Window != null)
		{
			return xuiV_Window;
		}
		XUiController xuiController = this.Parent;
		if (xuiController == null)
		{
			return null;
		}
		return xuiController.GetParentWindow();
	}

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x060075A6 RID: 30118 RVA: 0x002C2DC8 File Offset: 0x002C0FC8
	// (remove) Token: 0x060075A7 RID: 30119 RVA: 0x002C2E00 File Offset: 0x002C1000
	public event XUiEvent_OnMouseUpDownEventHandler OnMouseUpDown;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x060075A8 RID: 30120 RVA: 0x002C2E38 File Offset: 0x002C1038
	// (remove) Token: 0x060075A9 RID: 30121 RVA: 0x002C2E70 File Offset: 0x002C1070
	public event XUiEvent_OnPressEventHandler OnPress;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060075AA RID: 30122 RVA: 0x002C2EA8 File Offset: 0x002C10A8
	// (remove) Token: 0x060075AB RID: 30123 RVA: 0x002C2EE0 File Offset: 0x002C10E0
	public event XUiEvent_OnPressEventHandler OnDoubleClick;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060075AC RID: 30124 RVA: 0x002C2F18 File Offset: 0x002C1118
	// (remove) Token: 0x060075AD RID: 30125 RVA: 0x002C2F50 File Offset: 0x002C1150
	public event XUiEvent_OnPressEventHandler OnRightPress;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x060075AE RID: 30126 RVA: 0x002C2F88 File Offset: 0x002C1188
	// (remove) Token: 0x060075AF RID: 30127 RVA: 0x002C2FC0 File Offset: 0x002C11C0
	public event XUiEvent_OnHoverEventHandler OnHover;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x060075B0 RID: 30128 RVA: 0x002C2FF8 File Offset: 0x002C11F8
	// (remove) Token: 0x060075B1 RID: 30129 RVA: 0x002C3030 File Offset: 0x002C1230
	public event XUiEvent_OnDragEventHandler OnDrag;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x060075B2 RID: 30130 RVA: 0x002C3068 File Offset: 0x002C1268
	// (remove) Token: 0x060075B3 RID: 30131 RVA: 0x002C30A0 File Offset: 0x002C12A0
	public event XUiEvent_OnHeldHandler OnHold;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x060075B4 RID: 30132 RVA: 0x002C30D8 File Offset: 0x002C12D8
	// (remove) Token: 0x060075B5 RID: 30133 RVA: 0x002C3110 File Offset: 0x002C1310
	public event XUiEvent_OnScrollEventHandler OnScroll;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x060075B6 RID: 30134 RVA: 0x002C3148 File Offset: 0x002C1348
	// (remove) Token: 0x060075B7 RID: 30135 RVA: 0x002C3180 File Offset: 0x002C1380
	public event XUiEvent_OnSelectEventHandler OnSelect;

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x060075B8 RID: 30136 RVA: 0x002C31B8 File Offset: 0x002C13B8
	// (remove) Token: 0x060075B9 RID: 30137 RVA: 0x002C31F0 File Offset: 0x002C13F0
	public event XUiEvent_OnVisibilityChanged OnVisiblity;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x060075BA RID: 30138 RVA: 0x002C3228 File Offset: 0x002C1428
	// (remove) Token: 0x060075BB RID: 30139 RVA: 0x002C3260 File Offset: 0x002C1460
	public event XUiEvent_OnEnabledChanged OnEnabled;

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x060075BC RID: 30140 RVA: 0x002C3298 File Offset: 0x002C1498
	// (remove) Token: 0x060075BD RID: 30141 RVA: 0x002C32D0 File Offset: 0x002C14D0
	public event XUiEvent_OnInteraction OnInteraction;

	// Token: 0x060075BE RID: 30142 RVA: 0x002C3305 File Offset: 0x002C1505
	public void MouseUpDown(bool _pressed)
	{
		XUiEvent_OnMouseUpDownEventHandler onMouseUpDown = this.OnMouseUpDown;
		if (onMouseUpDown != null)
		{
			onMouseUpDown(this, _pressed);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.MouseUpDown);
	}

	// Token: 0x060075BF RID: 30143 RVA: 0x002C3330 File Offset: 0x002C1530
	public virtual void Pressed(int _mouseButton)
	{
		if (_mouseButton != -1)
		{
			if (_mouseButton == -2)
			{
				XUiEvent_OnPressEventHandler onRightPress = this.OnRightPress;
				if (onRightPress != null)
				{
					onRightPress(this, _mouseButton);
				}
				XUiEvent_OnInteraction onInteraction = this.OnInteraction;
				if (onInteraction == null)
				{
					return;
				}
				onInteraction(this, EXUiControllerInteractionType.RightClick);
			}
			return;
		}
		XUiEvent_OnPressEventHandler onPress = this.OnPress;
		if (onPress != null)
		{
			onPress(this, _mouseButton);
		}
		XUiEvent_OnInteraction onInteraction2 = this.OnInteraction;
		if (onInteraction2 == null)
		{
			return;
		}
		onInteraction2(this, EXUiControllerInteractionType.LeftClick);
	}

	// Token: 0x060075C0 RID: 30144 RVA: 0x002C3391 File Offset: 0x002C1591
	public void DoubleClicked(int _mouseButton)
	{
		XUiEvent_OnPressEventHandler onDoubleClick = this.OnDoubleClick;
		if (onDoubleClick != null)
		{
			onDoubleClick(this, _mouseButton);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.DoubleClick);
	}

	// Token: 0x060075C1 RID: 30145 RVA: 0x002C33B8 File Offset: 0x002C15B8
	public void Hovered(bool _isOver)
	{
		XUiEvent_OnHoverEventHandler onHover = this.OnHover;
		if (onHover != null)
		{
			onHover(this, _isOver);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction != null)
		{
			onInteraction(this, EXUiControllerInteractionType.Hover);
		}
		this.OnHovered(_isOver);
	}

	// Token: 0x060075C2 RID: 30146 RVA: 0x002C33E7 File Offset: 0x002C15E7
	public void Scrolled(float _delta)
	{
		XUiEvent_OnScrollEventHandler onScroll = this.OnScroll;
		if (onScroll != null)
		{
			onScroll(this, _delta);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Scroll);
	}

	// Token: 0x060075C3 RID: 30147 RVA: 0x002C340E File Offset: 0x002C160E
	public void Selected(bool _selected)
	{
		XUiEvent_OnSelectEventHandler onSelect = this.OnSelect;
		if (onSelect != null)
		{
			onSelect(this, _selected);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Select);
	}

	// Token: 0x060075C4 RID: 30148 RVA: 0x002C3438 File Offset: 0x002C1638
	public void Dragged(Vector2 _mouseDelta, EDragType _dragType)
	{
		if (this.viewComponent.ID == "btn2")
		{
			Log.Warning(string.Format("[XUi] F={0} ButtonC received Dragged={1} event", Time.frameCount, _dragType));
		}
		XUiEvent_OnDragEventHandler onDrag = this.OnDrag;
		if (onDrag != null)
		{
			onDrag(this, _dragType, _mouseDelta);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Drag);
	}

	// Token: 0x060075C5 RID: 30149 RVA: 0x002C34A1 File Offset: 0x002C16A1
	public void Held(EHoldType _event, float _holdDuration, float _deltaSinceLastTimedEvent = -1f)
	{
		XUiEvent_OnHeldHandler onHold = this.OnHold;
		if (onHold != null)
		{
			onHold(this, _event, _holdDuration, _deltaSinceLastTimedEvent);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Held);
	}

	// Token: 0x060075C6 RID: 30150 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnHovered(bool _isOver)
	{
	}

	// Token: 0x060075C7 RID: 30151 RVA: 0x002C34CC File Offset: 0x002C16CC
	public virtual void OnOpen()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnOpen();
		}
		XUiView xuiView = this.ViewComponent;
		if (xuiView != null)
		{
			xuiView.OnOpen();
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction != null)
		{
			onInteraction(this, EXUiControllerInteractionType.Open);
		}
		this.RefreshBindings();
	}

	// Token: 0x060075C8 RID: 30152 RVA: 0x002C352C File Offset: 0x002C172C
	public virtual void OnClose()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnClose();
		}
		XUiView xuiView = this.ViewComponent;
		if (xuiView != null)
		{
			xuiView.OnClose();
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Close);
	}

	// Token: 0x060075C9 RID: 30153 RVA: 0x002C3584 File Offset: 0x002C1784
	public void OnVisibilityChanged(bool _parentVisibleInScene)
	{
		bool isVisible = this.viewComponent.IsVisible;
		bool flag = isVisible && _parentVisibleInScene;
		XUiEvent_OnVisibilityChanged onVisiblity = this.OnVisiblity;
		if (onVisiblity != null)
		{
			onVisiblity(this, isVisible, flag);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction != null)
		{
			onInteraction(this, EXUiControllerInteractionType.VisibilityChanged);
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnVisibilityChanged(flag);
			this.children[i].viewComponent.OnVisibilityChanged(flag);
		}
	}

	// Token: 0x060075CA RID: 30154 RVA: 0x002C3607 File Offset: 0x002C1807
	public void OnEnabledChanged(bool _enabled)
	{
		XUiEvent_OnEnabledChanged onEnabled = this.OnEnabled;
		if (onEnabled != null)
		{
			onEnabled(this, _enabled);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.EnabledChanged);
	}

	// Token: 0x060075CB RID: 30155 RVA: 0x002C3630 File Offset: 0x002C1830
	public virtual void OnCursorSelected(bool _isActualElement)
	{
		XUiV_ScrollView xuiV_ScrollView;
		if (_isActualElement && this.TryGetParentView<XUiV_ScrollView>(out xuiV_ScrollView))
		{
			xuiV_ScrollView.MakeVisible(this.ViewComponent);
		}
		XUiController xuiController = this.parent;
		if (xuiController == null)
		{
			return;
		}
		xuiController.OnCursorSelected(false);
	}

	// Token: 0x060075CC RID: 30156 RVA: 0x002C3667 File Offset: 0x002C1867
	public virtual void OnCursorUnSelected()
	{
		XUiController xuiController = this.parent;
		if (xuiController == null)
		{
			return;
		}
		xuiController.OnCursorUnSelected();
	}

	// Token: 0x060075CD RID: 30157 RVA: 0x002C3679 File Offset: 0x002C1879
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool handleDirtyUpdateDefault()
	{
		if (!this.IsDirty)
		{
			return false;
		}
		this.IsDirty = false;
		this.RefreshBindings();
		return true;
	}

	// Token: 0x060075CE RID: 30158 RVA: 0x002C3693 File Offset: 0x002C1893
	public virtual bool ParseAttribute(string _name, string _value)
	{
		if (_name == "always_update")
		{
			StringParsers.TryParseBool(_value, out this.AlwaysUpdate);
			return true;
		}
		return false;
	}

	// Token: 0x060075CF RID: 30159 RVA: 0x002C36B4 File Offset: 0x002C18B4
	public bool GetBindingValue(ref string _value, string _bindingName)
	{
		bool bindingValueInternal;
		try
		{
			bindingValueInternal = this.GetBindingValueInternal(ref _value, _bindingName);
		}
		catch
		{
			Log.Error("Unhandled exception in GetBindingValue. Binding name: " + _bindingName + ", hierarchy: " + this.GetXuiHierarchy());
			throw;
		}
		return bindingValueInternal;
	}

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x060075D0 RID: 30160 RVA: 0x002C36FC File Offset: 0x002C18FC
	[XuiXmlBinding("is_online")]
	public bool IsOnline
	{
		get
		{
			return PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.LoggedIn;
		}
	}

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x060075D1 RID: 30161 RVA: 0x002C3710 File Offset: 0x002C1910
	[XuiXmlBinding("system_time")]
	public DateTime SystemTime
	{
		get
		{
			return DateTime.Now;
		}
	}

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x060075D2 RID: 30162 RVA: 0x002C3717 File Offset: 0x002C1917
	[XuiXmlBinding("gamelanguage")]
	public string Language
	{
		get
		{
			return Localization.ActiveLanguage;
		}
	}

	// Token: 0x060075D3 RID: 30163 RVA: 0x002C3720 File Offset: 0x002C1920
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2577987401U)
		{
			if (num <= 820250566U)
			{
				if (num <= 364418046U)
				{
					if (num != 23126121U)
					{
						if (num == 364418046U)
						{
							if (_bindingName == "is_map_enabled")
							{
								_value = World.MapEnabled.ToString();
								return true;
							}
						}
					}
					else if (_bindingName == "is_challenge_enabled")
					{
						_value = (ChallengeJournal.AllowChallenges || World.BiomeProgressionEnabled).ToString();
						return true;
					}
				}
				else if (num != 470361894U)
				{
					if (num == 820250566U)
					{
						if (_bindingName == "is_unityeditor")
						{
							_value = "false";
							return true;
						}
					}
				}
				else if (_bindingName == "version_long")
				{
					_value = Constants.cVersionInformation.LongString;
					return true;
				}
			}
			else if (num <= 1573359332U)
			{
				if (num != 1383400110U)
				{
					if (num == 1573359332U)
					{
						if (_bindingName == "is_playtesting")
						{
							_value = GameUtils.IsPlaytesting().ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "is_world_loaded")
				{
					_value = (GameManager.Instance.World != null).ToString();
					return true;
				}
			}
			else if (num != 1899437583U)
			{
				if (num == 2577987401U)
				{
					if (_bindingName == "is_editmode")
					{
						_value = GameManager.Instance.IsEditMode().ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_modal")
			{
				_value = this.WindowGroup.isModal.ToString();
				return true;
			}
		}
		else if (num <= 2934648263U)
		{
			if (num <= 2759425756U)
			{
				if (num != 2745931225U)
				{
					if (num == 2759425756U)
					{
						if (_bindingName == "is_prefab_editor")
						{
							_value = PrefabEditModeManager.Instance.IsActive().ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "is_server")
				{
					_value = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer.ToString();
					return true;
				}
			}
			else if (num != 2799769034U)
			{
				if (num == 2934648263U)
				{
					if (_bindingName == "is_console")
					{
						_value = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent().ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_ps5")
			{
				_value = DeviceFlag.PS5.IsCurrent().ToString();
				return true;
			}
		}
		else if (num <= 3445481009U)
		{
			if (num != 2963904327U)
			{
				if (num == 3445481009U)
				{
					if (_bindingName == "is_controller_input")
					{
						this.registerForInputStyleChanges();
						_value = (this.lastInputStyle != PlayerInputManager.InputStyle.Keyboard).ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_release")
			{
				_value = "true";
				return true;
			}
		}
		else if (num != 3556752319U)
		{
			if (num == 4180283068U)
			{
				if (_bindingName == "inputstyle")
				{
					this.registerForInputStyleChanges();
					_value = this.lastInputStyle.ToStringCached<PlayerInputManager.InputStyle>();
					return true;
				}
			}
		}
		else if (_bindingName == "is_creative")
		{
			_value = (GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled)).ToString();
			return true;
		}
		return false;
	}

	// Token: 0x060075D4 RID: 30164 RVA: 0x002C3AD4 File Offset: 0x002C1CD4
	public void RefreshBindingsSelfAndChildren()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].RefreshBindingsSelfAndChildren();
		}
		this.Bindings.RefreshBindings();
	}

	// Token: 0x060075D5 RID: 30165 RVA: 0x002C3B13 File Offset: 0x002C1D13
	public void RefreshBindings()
	{
		this.Bindings.RefreshBindings();
	}

	// Token: 0x060075D6 RID: 30166 RVA: 0x002C3B20 File Offset: 0x002C1D20
	public virtual void Cleanup()
	{
		foreach (XUiController xuiController in this.children)
		{
			xuiController.Cleanup();
		}
		if (this.registeredForInputStyleChanges)
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
			{
				PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
			}
		}
		XUiView xuiView = this.ViewComponent;
		if (xuiView == null)
		{
			return;
		}
		xuiView.Cleanup();
	}

	// Token: 0x060075D7 RID: 30167 RVA: 0x002C3BB8 File Offset: 0x002C1DB8
	public void FindNavigatableChildren(List<XUiView> _views)
	{
		foreach (XUiController xuiController in this.children)
		{
			if (xuiController.viewComponent.IsNavigatable)
			{
				_views.Add(xuiController.viewComponent);
			}
			xuiController.FindNavigatableChildren(_views);
		}
	}

	// Token: 0x060075D8 RID: 30168 RVA: 0x002C3C24 File Offset: 0x002C1E24
	public bool TryFindFirstNavigableView(out XUiView _foundView)
	{
		_foundView = null;
		bool isActiveInHierarchy = this.ViewComponent.IsActiveInHierarchy;
		if (this.ViewComponent.IsNavigatable && this.ViewComponent.IsVisible && isActiveInHierarchy)
		{
			_foundView = this.ViewComponent;
			return true;
		}
		if (!isActiveInHierarchy)
		{
			return false;
		}
		using (List<XUiController>.Enumerator enumerator = this.children.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.TryFindFirstNavigableView(out _foundView))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060075D9 RID: 30169 RVA: 0x002C3CBC File Offset: 0x002C1EBC
	public bool SelectCursorElement(bool _withDelay = false, bool _overrideCursorMode = false)
	{
		if (this.ViewComponent == null)
		{
			return false;
		}
		CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
		if (cursorController.CursorModeActive && !_overrideCursorMode)
		{
			return false;
		}
		XUiView xuiView;
		this.TryFindFirstNavigableView(out xuiView);
		if (xuiView == null)
		{
			return false;
		}
		if (_withDelay)
		{
			cursorController.SetNavigationTargetLater(xuiView);
		}
		else
		{
			cursorController.SetNavigationTarget(xuiView);
		}
		return true;
	}

	// Token: 0x060075DA RID: 30170 RVA: 0x002C3D14 File Offset: 0x002C1F14
	public void SetAllChildrenDirty()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].SetAllChildrenDirty();
		}
		XUiView xuiView = this.viewComponent;
		if (xuiView != null)
		{
			xuiView.SetDirty();
		}
		this.IsDirty = true;
	}

	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x060075DB RID: 30171 RVA: 0x002C3D60 File Offset: 0x002C1F60
	public PlayerInputManager.InputStyle CurrentInputStyle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.lastInputStyle;
		}
	}

	// Token: 0x060075DC RID: 30172 RVA: 0x002C3D68 File Offset: 0x002C1F68
	[PublicizedFrom(EAccessModifier.Protected)]
	public void registerForInputStyleChanges()
	{
		if (this.registeredForInputStyleChanges)
		{
			return;
		}
		this.registeredForInputStyleChanges = true;
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
	}

	// Token: 0x060075DD RID: 30173 RVA: 0x002C3D95 File Offset: 0x002C1F95
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInputStyle = _style;
		this.forceInputStyleChange = true;
	}

	// Token: 0x060075DE RID: 30174 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
	}

	// Token: 0x060075DF RID: 30175 RVA: 0x002C3DA8 File Offset: 0x002C1FA8
	public void ForceInputStyleChange(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		if (this.registeredForInputStyleChanges)
		{
			this.inputStyleChanged(_oldStyle, _newStyle);
		}
		foreach (XUiController xuiController in this.children)
		{
			xuiController.ForceInputStyleChange(_oldStyle, _newStyle);
		}
	}

	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x060075E0 RID: 30176 RVA: 0x0012A165 File Offset: 0x00128365
	public XUiController Controller
	{
		get
		{
			return this;
		}
	}

	// Token: 0x060075E1 RID: 30177 RVA: 0x002C3E0C File Offset: 0x002C200C
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this);
	}

	// Token: 0x0400570B RID: 22283
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView viewComponent;

	// Token: 0x0400570C RID: 22284
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController parent;

	// Token: 0x0400570D RID: 22285
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<XUiController> children = new List<XUiController>();

	// Token: 0x0400570E RID: 22286
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiWindowGroup windowGroup;

	// Token: 0x0400570F RID: 22287
	public bool AlwaysUpdate;

	// Token: 0x04005710 RID: 22288
	public bool IsDirty;

	// Token: 0x04005711 RID: 22289
	public object CustomData;

	// Token: 0x04005712 RID: 22290
	public readonly ObservableDictionary<string, object> CustomAttributes = new ObservableDictionary<string, object>();

	// Token: 0x04005713 RID: 22291
	public XUi xui;

	// Token: 0x04005714 RID: 22292
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmControllerUpdate = new ProfilerMarker("XC.Update");

	// Token: 0x04005715 RID: 22293
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcUpdate = new ProfilerMarker("VC");

	// Token: 0x04005716 RID: 22294
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcNamedUpdate;

	// Token: 0x04005723 RID: 22307
	public readonly BindingsManager Bindings = new BindingsManager();

	// Token: 0x04005724 RID: 22308
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceInputStyleChange;

	// Token: 0x04005725 RID: 22309
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04005726 RID: 22310
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInputStyle;

	// Token: 0x04005727 RID: 22311
	[PublicizedFrom(EAccessModifier.Private)]
	public bool registeredForInputStyleChanges;
}

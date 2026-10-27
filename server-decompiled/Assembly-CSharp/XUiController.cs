using System;
using System.Collections.Generic;
using Platform;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ED5 RID: 3797
[Preserve]
public class XUiController : IXUiElement
{
	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x0600759D RID: 30109 RVA: 0x002C6B84 File Offset: 0x002C4D84
	// (set) Token: 0x0600759E RID: 30110 RVA: 0x002C6B8C File Offset: 0x002C4D8C
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

	// Token: 0x17000DF8 RID: 3576
	// (get) Token: 0x0600759F RID: 30111 RVA: 0x002C6B95 File Offset: 0x002C4D95
	// (set) Token: 0x060075A0 RID: 30112 RVA: 0x002C6BA0 File Offset: 0x002C4DA0
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

	// Token: 0x17000DF9 RID: 3577
	// (get) Token: 0x060075A1 RID: 30113 RVA: 0x002C6BEC File Offset: 0x002C4DEC
	public List<XUiController> Children
	{
		get
		{
			return this.children;
		}
	}

	// Token: 0x17000DFA RID: 3578
	// (get) Token: 0x060075A2 RID: 30114 RVA: 0x002C6BF4 File Offset: 0x002C4DF4
	// (set) Token: 0x060075A3 RID: 30115 RVA: 0x002C6BFC File Offset: 0x002C4DFC
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

	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x060075A4 RID: 30116 RVA: 0x002C6C05 File Offset: 0x002C4E05
	public bool IsOpen
	{
		get
		{
			return this.windowGroup.isShowing;
		}
	}

	// Token: 0x060075A5 RID: 30117 RVA: 0x002C6C12 File Offset: 0x002C4E12
	public void AutoBindComponents()
	{
		AutoBindCache.Instance.BindComponents(this);
	}

	// Token: 0x060075A6 RID: 30118 RVA: 0x002C6C1F File Offset: 0x002C4E1F
	public void AutoBindEvents()
	{
		AutoBindCache.Instance.BindEvents(this);
	}

	// Token: 0x060075A7 RID: 30119 RVA: 0x002C6C2C File Offset: 0x002C4E2C
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

	// Token: 0x060075A8 RID: 30120 RVA: 0x002C6CAC File Offset: 0x002C4EAC
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

	// Token: 0x060075A9 RID: 30121 RVA: 0x002C6DB4 File Offset: 0x002C4FB4
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

	// Token: 0x060075AA RID: 30122 RVA: 0x002C6E34 File Offset: 0x002C5034
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

	// Token: 0x060075AB RID: 30123 RVA: 0x002C6EC3 File Offset: 0x002C50C3
	public bool TryGetChildController<T>(string _id, out T _child) where T : XUiController
	{
		return this.TryGetChildController<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x060075AC RID: 30124 RVA: 0x002C6ED4 File Offset: 0x002C50D4
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

	// Token: 0x060075AD RID: 30125 RVA: 0x002C6F64 File Offset: 0x002C5164
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

	// Token: 0x060075AE RID: 30126 RVA: 0x002C6FFE File Offset: 0x002C51FE
	public T[] GetChildControllers<T>(string _id, List<T> _list = null) where T : XUiController
	{
		return this.GetChildControllers<T>(_id.AsMemory(), _list);
	}

	// Token: 0x060075AF RID: 30127 RVA: 0x002C7010 File Offset: 0x002C5210
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

	// Token: 0x060075B0 RID: 30128 RVA: 0x002C7098 File Offset: 0x002C5298
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

	// Token: 0x060075B1 RID: 30129 RVA: 0x002C7124 File Offset: 0x002C5324
	public bool TryGetChildView<T>(string _id, out T _child) where T : XUiView
	{
		return this.TryGetChildView<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x060075B2 RID: 30130 RVA: 0x002C7134 File Offset: 0x002C5334
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

	// Token: 0x060075B3 RID: 30131 RVA: 0x002C71D0 File Offset: 0x002C53D0
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

	// Token: 0x060075B4 RID: 30132 RVA: 0x002C7267 File Offset: 0x002C5467
	public T[] GetChildViews<T>(string _id, List<T> _list = null) where T : XUiView
	{
		return this.GetChildViews<T>(_id.AsMemory(), _list);
	}

	// Token: 0x060075B5 RID: 30133 RVA: 0x002C7278 File Offset: 0x002C5478
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

	// Token: 0x060075B6 RID: 30134 RVA: 0x002C72F4 File Offset: 0x002C54F4
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

	// Token: 0x060075B7 RID: 30135 RVA: 0x002C7350 File Offset: 0x002C5550
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

	// Token: 0x060075B8 RID: 30136 RVA: 0x002C73C4 File Offset: 0x002C55C4
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

	// Token: 0x060075B9 RID: 30137 RVA: 0x002C7444 File Offset: 0x002C5644
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

	// Token: 0x060075BA RID: 30138 RVA: 0x002C74CC File Offset: 0x002C56CC
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

	// Token: 0x060075BB RID: 30139 RVA: 0x002C7558 File Offset: 0x002C5758
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

	// Token: 0x060075BC RID: 30140 RVA: 0x002C7594 File Offset: 0x002C5794
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

	// Token: 0x060075BD RID: 30141 RVA: 0x002C75C0 File Offset: 0x002C57C0
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

	// Token: 0x060075BE RID: 30142 RVA: 0x002C7607 File Offset: 0x002C5807
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

	// Token: 0x060075BF RID: 30143 RVA: 0x002C763C File Offset: 0x002C583C
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

	// Token: 0x060075C0 RID: 30144 RVA: 0x002C7688 File Offset: 0x002C5888
	public bool IsChildOf(XUiController _controller)
	{
		return this.Parent != null && (this.Parent == _controller || this.Parent.IsChildOf(_controller));
	}

	// Token: 0x060075C1 RID: 30145 RVA: 0x002C76AB File Offset: 0x002C58AB
	public bool IsSelfOrChildOf(XUiController _controller)
	{
		return this == _controller || this.IsChildOf(_controller);
	}

	// Token: 0x060075C2 RID: 30146 RVA: 0x002C76BC File Offset: 0x002C58BC
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

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x060075C3 RID: 30147 RVA: 0x002C76EC File Offset: 0x002C58EC
	// (remove) Token: 0x060075C4 RID: 30148 RVA: 0x002C7724 File Offset: 0x002C5924
	public event XUiEvent_OnMouseUpDownEventHandler OnMouseUpDown;

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x060075C5 RID: 30149 RVA: 0x002C775C File Offset: 0x002C595C
	// (remove) Token: 0x060075C6 RID: 30150 RVA: 0x002C7794 File Offset: 0x002C5994
	public event XUiEvent_OnPressEventHandler OnPress;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x060075C7 RID: 30151 RVA: 0x002C77CC File Offset: 0x002C59CC
	// (remove) Token: 0x060075C8 RID: 30152 RVA: 0x002C7804 File Offset: 0x002C5A04
	public event XUiEvent_OnPressEventHandler OnDoubleClick;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060075C9 RID: 30153 RVA: 0x002C783C File Offset: 0x002C5A3C
	// (remove) Token: 0x060075CA RID: 30154 RVA: 0x002C7874 File Offset: 0x002C5A74
	public event XUiEvent_OnPressEventHandler OnRightPress;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060075CB RID: 30155 RVA: 0x002C78AC File Offset: 0x002C5AAC
	// (remove) Token: 0x060075CC RID: 30156 RVA: 0x002C78E4 File Offset: 0x002C5AE4
	public event XUiEvent_OnHoverEventHandler OnHover;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x060075CD RID: 30157 RVA: 0x002C791C File Offset: 0x002C5B1C
	// (remove) Token: 0x060075CE RID: 30158 RVA: 0x002C7954 File Offset: 0x002C5B54
	public event XUiEvent_OnDragEventHandler OnDrag;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x060075CF RID: 30159 RVA: 0x002C798C File Offset: 0x002C5B8C
	// (remove) Token: 0x060075D0 RID: 30160 RVA: 0x002C79C4 File Offset: 0x002C5BC4
	public event XUiEvent_OnHeldHandler OnHold;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x060075D1 RID: 30161 RVA: 0x002C79FC File Offset: 0x002C5BFC
	// (remove) Token: 0x060075D2 RID: 30162 RVA: 0x002C7A34 File Offset: 0x002C5C34
	public event XUiEvent_OnScrollEventHandler OnScroll;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x060075D3 RID: 30163 RVA: 0x002C7A6C File Offset: 0x002C5C6C
	// (remove) Token: 0x060075D4 RID: 30164 RVA: 0x002C7AA4 File Offset: 0x002C5CA4
	public event XUiEvent_OnSelectEventHandler OnSelect;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x060075D5 RID: 30165 RVA: 0x002C7ADC File Offset: 0x002C5CDC
	// (remove) Token: 0x060075D6 RID: 30166 RVA: 0x002C7B14 File Offset: 0x002C5D14
	public event XUiEvent_OnVisibilityChanged OnVisiblity;

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x060075D7 RID: 30167 RVA: 0x002C7B4C File Offset: 0x002C5D4C
	// (remove) Token: 0x060075D8 RID: 30168 RVA: 0x002C7B84 File Offset: 0x002C5D84
	public event XUiEvent_OnEnabledChanged OnEnabled;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x060075D9 RID: 30169 RVA: 0x002C7BBC File Offset: 0x002C5DBC
	// (remove) Token: 0x060075DA RID: 30170 RVA: 0x002C7BF4 File Offset: 0x002C5DF4
	public event XUiEvent_OnInteraction OnInteraction;

	// Token: 0x060075DB RID: 30171 RVA: 0x002C7C29 File Offset: 0x002C5E29
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

	// Token: 0x060075DC RID: 30172 RVA: 0x002C7C54 File Offset: 0x002C5E54
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

	// Token: 0x060075DD RID: 30173 RVA: 0x002C7CB5 File Offset: 0x002C5EB5
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

	// Token: 0x060075DE RID: 30174 RVA: 0x002C7CDC File Offset: 0x002C5EDC
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

	// Token: 0x060075DF RID: 30175 RVA: 0x002C7D0B File Offset: 0x002C5F0B
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

	// Token: 0x060075E0 RID: 30176 RVA: 0x002C7D32 File Offset: 0x002C5F32
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

	// Token: 0x060075E1 RID: 30177 RVA: 0x002C7D5C File Offset: 0x002C5F5C
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

	// Token: 0x060075E2 RID: 30178 RVA: 0x002C7DC5 File Offset: 0x002C5FC5
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

	// Token: 0x060075E3 RID: 30179 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnHovered(bool _isOver)
	{
	}

	// Token: 0x060075E4 RID: 30180 RVA: 0x002C7DF0 File Offset: 0x002C5FF0
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

	// Token: 0x060075E5 RID: 30181 RVA: 0x002C7E50 File Offset: 0x002C6050
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

	// Token: 0x060075E6 RID: 30182 RVA: 0x002C7EA8 File Offset: 0x002C60A8
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

	// Token: 0x060075E7 RID: 30183 RVA: 0x002C7F2B File Offset: 0x002C612B
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

	// Token: 0x060075E8 RID: 30184 RVA: 0x002C7F54 File Offset: 0x002C6154
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

	// Token: 0x060075E9 RID: 30185 RVA: 0x002C7F8B File Offset: 0x002C618B
	public virtual void OnCursorUnSelected()
	{
		XUiController xuiController = this.parent;
		if (xuiController == null)
		{
			return;
		}
		xuiController.OnCursorUnSelected();
	}

	// Token: 0x060075EA RID: 30186 RVA: 0x002C7F9D File Offset: 0x002C619D
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

	// Token: 0x060075EB RID: 30187 RVA: 0x002C7FB7 File Offset: 0x002C61B7
	public virtual bool ParseAttribute(string _name, string _value)
	{
		if (_name == "always_update")
		{
			StringParsers.TryParseBool(_value, out this.AlwaysUpdate);
			return true;
		}
		return false;
	}

	// Token: 0x060075EC RID: 30188 RVA: 0x002C7FD8 File Offset: 0x002C61D8
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

	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x060075ED RID: 30189 RVA: 0x002C8020 File Offset: 0x002C6220
	[XuiXmlBinding("is_online")]
	public bool IsOnline
	{
		get
		{
			return PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.LoggedIn;
		}
	}

	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x060075EE RID: 30190 RVA: 0x002C8034 File Offset: 0x002C6234
	[XuiXmlBinding("system_time")]
	public DateTime SystemTime
	{
		get
		{
			return DateTime.Now;
		}
	}

	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x060075EF RID: 30191 RVA: 0x002C803B File Offset: 0x002C623B
	[XuiXmlBinding("gamelanguage")]
	public string Language
	{
		get
		{
			return Localization.ActiveLanguage;
		}
	}

	// Token: 0x060075F0 RID: 30192 RVA: 0x002C8044 File Offset: 0x002C6244
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

	// Token: 0x060075F1 RID: 30193 RVA: 0x002C83F8 File Offset: 0x002C65F8
	public void RefreshBindingsSelfAndChildren()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].RefreshBindingsSelfAndChildren();
		}
		this.Bindings.RefreshBindings();
	}

	// Token: 0x060075F2 RID: 30194 RVA: 0x002C8437 File Offset: 0x002C6637
	public void RefreshBindings()
	{
		this.Bindings.RefreshBindings();
	}

	// Token: 0x060075F3 RID: 30195 RVA: 0x002C8444 File Offset: 0x002C6644
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

	// Token: 0x060075F4 RID: 30196 RVA: 0x002C84DC File Offset: 0x002C66DC
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

	// Token: 0x060075F5 RID: 30197 RVA: 0x002C8548 File Offset: 0x002C6748
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

	// Token: 0x060075F6 RID: 30198 RVA: 0x002C85E0 File Offset: 0x002C67E0
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

	// Token: 0x060075F7 RID: 30199 RVA: 0x002C8638 File Offset: 0x002C6838
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

	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x060075F8 RID: 30200 RVA: 0x002C8684 File Offset: 0x002C6884
	public PlayerInputManager.InputStyle CurrentInputStyle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.lastInputStyle;
		}
	}

	// Token: 0x060075F9 RID: 30201 RVA: 0x002C868C File Offset: 0x002C688C
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

	// Token: 0x060075FA RID: 30202 RVA: 0x002C86B9 File Offset: 0x002C68B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInputStyle = _style;
		this.forceInputStyleChange = true;
	}

	// Token: 0x060075FB RID: 30203 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
	}

	// Token: 0x060075FC RID: 30204 RVA: 0x002C86CC File Offset: 0x002C68CC
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

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x060075FD RID: 30205 RVA: 0x0012B879 File Offset: 0x00129A79
	public XUiController Controller
	{
		get
		{
			return this;
		}
	}

	// Token: 0x060075FE RID: 30206 RVA: 0x002C8730 File Offset: 0x002C6930
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this);
	}

	// Token: 0x04005767 RID: 22375
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView viewComponent;

	// Token: 0x04005768 RID: 22376
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController parent;

	// Token: 0x04005769 RID: 22377
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<XUiController> children = new List<XUiController>();

	// Token: 0x0400576A RID: 22378
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiWindowGroup windowGroup;

	// Token: 0x0400576B RID: 22379
	public bool AlwaysUpdate;

	// Token: 0x0400576C RID: 22380
	public bool IsDirty;

	// Token: 0x0400576D RID: 22381
	public object CustomData;

	// Token: 0x0400576E RID: 22382
	public readonly ObservableDictionary<string, object> CustomAttributes = new ObservableDictionary<string, object>();

	// Token: 0x0400576F RID: 22383
	public XUi xui;

	// Token: 0x04005770 RID: 22384
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmControllerUpdate = new ProfilerMarker("XC.Update");

	// Token: 0x04005771 RID: 22385
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcUpdate = new ProfilerMarker("VC");

	// Token: 0x04005772 RID: 22386
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcNamedUpdate;

	// Token: 0x0400577F RID: 22399
	public readonly BindingsManager Bindings = new BindingsManager();

	// Token: 0x04005780 RID: 22400
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceInputStyleChange;

	// Token: 0x04005781 RID: 22401
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04005782 RID: 22402
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInputStyle;

	// Token: 0x04005783 RID: 22403
	[PublicizedFrom(EAccessModifier.Private)]
	public bool registeredForInputStyleChanges;
}

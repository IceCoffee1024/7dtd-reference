using System;
using System.Collections.Generic;
using Platform;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EA6 RID: 3750
[Preserve]
public class XUiController : IXUiElement
{
	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x060074E0 RID: 29920 RVA: 0x002BED34 File Offset: 0x002BCF34
	// (set) Token: 0x060074E1 RID: 29921 RVA: 0x002BED3C File Offset: 0x002BCF3C
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

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x060074E2 RID: 29922 RVA: 0x002BED45 File Offset: 0x002BCF45
	// (set) Token: 0x060074E3 RID: 29923 RVA: 0x002BED50 File Offset: 0x002BCF50
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

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x060074E4 RID: 29924 RVA: 0x002BED9C File Offset: 0x002BCF9C
	public List<XUiController> Children
	{
		get
		{
			return this.children;
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x060074E5 RID: 29925 RVA: 0x002BEDA4 File Offset: 0x002BCFA4
	// (set) Token: 0x060074E6 RID: 29926 RVA: 0x002BEDAC File Offset: 0x002BCFAC
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

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x060074E7 RID: 29927 RVA: 0x002BEDB5 File Offset: 0x002BCFB5
	public bool IsOpen
	{
		get
		{
			return this.windowGroup.isShowing;
		}
	}

	// Token: 0x060074E8 RID: 29928 RVA: 0x002BEDC2 File Offset: 0x002BCFC2
	public void AutoBindComponents()
	{
		AutoBindCache.Instance.BindComponents(this);
	}

	// Token: 0x060074E9 RID: 29929 RVA: 0x002BEDCF File Offset: 0x002BCFCF
	public void AutoBindEvents()
	{
		AutoBindCache.Instance.BindEvents(this);
	}

	// Token: 0x060074EA RID: 29930 RVA: 0x002BEDDC File Offset: 0x002BCFDC
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

	// Token: 0x060074EB RID: 29931 RVA: 0x002BEE5C File Offset: 0x002BD05C
	public virtual void Update(float _dt)
	{
		XUiView xuiView = this.viewComponent;
		bool isShowing = this.windowGroup.isShowing;
		if (this.curInputStyle != this.lastInputStyle)
		{
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

	// Token: 0x060074EC RID: 29932 RVA: 0x002BEF54 File Offset: 0x002BD154
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

	// Token: 0x060074ED RID: 29933 RVA: 0x002BEFD4 File Offset: 0x002BD1D4
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

	// Token: 0x060074EE RID: 29934 RVA: 0x002BF063 File Offset: 0x002BD263
	public bool TryGetChildController<T>(string _id, out T _child) where T : XUiController
	{
		return this.TryGetChildController<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x060074EF RID: 29935 RVA: 0x002BF074 File Offset: 0x002BD274
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

	// Token: 0x060074F0 RID: 29936 RVA: 0x002BF104 File Offset: 0x002BD304
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

	// Token: 0x060074F1 RID: 29937 RVA: 0x002BF19E File Offset: 0x002BD39E
	public T[] GetChildControllers<T>(string _id, List<T> _list = null) where T : XUiController
	{
		return this.GetChildControllers<T>(_id.AsMemory(), _list);
	}

	// Token: 0x060074F2 RID: 29938 RVA: 0x002BF1B0 File Offset: 0x002BD3B0
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

	// Token: 0x060074F3 RID: 29939 RVA: 0x002BF238 File Offset: 0x002BD438
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

	// Token: 0x060074F4 RID: 29940 RVA: 0x002BF2C4 File Offset: 0x002BD4C4
	public bool TryGetChildView<T>(string _id, out T _child) where T : XUiView
	{
		return this.TryGetChildView<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x060074F5 RID: 29941 RVA: 0x002BF2D4 File Offset: 0x002BD4D4
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

	// Token: 0x060074F6 RID: 29942 RVA: 0x002BF370 File Offset: 0x002BD570
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

	// Token: 0x060074F7 RID: 29943 RVA: 0x002BF407 File Offset: 0x002BD607
	public T[] GetChildViews<T>(string _id, List<T> _list = null) where T : XUiView
	{
		return this.GetChildViews<T>(_id.AsMemory(), _list);
	}

	// Token: 0x060074F8 RID: 29944 RVA: 0x002BF418 File Offset: 0x002BD618
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

	// Token: 0x060074F9 RID: 29945 RVA: 0x002BF494 File Offset: 0x002BD694
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

	// Token: 0x060074FA RID: 29946 RVA: 0x002BF4F0 File Offset: 0x002BD6F0
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

	// Token: 0x060074FB RID: 29947 RVA: 0x002BF564 File Offset: 0x002BD764
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

	// Token: 0x060074FC RID: 29948 RVA: 0x002BF5E4 File Offset: 0x002BD7E4
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

	// Token: 0x060074FD RID: 29949 RVA: 0x002BF66C File Offset: 0x002BD86C
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

	// Token: 0x060074FE RID: 29950 RVA: 0x002BF6F8 File Offset: 0x002BD8F8
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

	// Token: 0x060074FF RID: 29951 RVA: 0x002BF734 File Offset: 0x002BD934
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

	// Token: 0x06007500 RID: 29952 RVA: 0x002BF760 File Offset: 0x002BD960
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

	// Token: 0x06007501 RID: 29953 RVA: 0x002BF7A7 File Offset: 0x002BD9A7
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

	// Token: 0x06007502 RID: 29954 RVA: 0x002BF7DC File Offset: 0x002BD9DC
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

	// Token: 0x06007503 RID: 29955 RVA: 0x002BF828 File Offset: 0x002BDA28
	public bool IsChildOf(XUiController _controller)
	{
		return this.Parent != null && (this.Parent == _controller || this.Parent.IsChildOf(_controller));
	}

	// Token: 0x06007504 RID: 29956 RVA: 0x002BF84B File Offset: 0x002BDA4B
	public bool IsSelfOrChildOf(XUiController _controller)
	{
		return this == _controller || this.IsChildOf(_controller);
	}

	// Token: 0x06007505 RID: 29957 RVA: 0x002BF85C File Offset: 0x002BDA5C
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
	// (add) Token: 0x06007506 RID: 29958 RVA: 0x002BF88C File Offset: 0x002BDA8C
	// (remove) Token: 0x06007507 RID: 29959 RVA: 0x002BF8C4 File Offset: 0x002BDAC4
	public event XUiEvent_OnMouseUpDownEventHandler OnMouseUpDown;

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x06007508 RID: 29960 RVA: 0x002BF8FC File Offset: 0x002BDAFC
	// (remove) Token: 0x06007509 RID: 29961 RVA: 0x002BF934 File Offset: 0x002BDB34
	public event XUiEvent_OnPressEventHandler OnPress;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x0600750A RID: 29962 RVA: 0x002BF96C File Offset: 0x002BDB6C
	// (remove) Token: 0x0600750B RID: 29963 RVA: 0x002BF9A4 File Offset: 0x002BDBA4
	public event XUiEvent_OnPressEventHandler OnDoubleClick;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x0600750C RID: 29964 RVA: 0x002BF9DC File Offset: 0x002BDBDC
	// (remove) Token: 0x0600750D RID: 29965 RVA: 0x002BFA14 File Offset: 0x002BDC14
	public event XUiEvent_OnPressEventHandler OnRightPress;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x0600750E RID: 29966 RVA: 0x002BFA4C File Offset: 0x002BDC4C
	// (remove) Token: 0x0600750F RID: 29967 RVA: 0x002BFA84 File Offset: 0x002BDC84
	public event XUiEvent_OnHoverEventHandler OnHover;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x06007510 RID: 29968 RVA: 0x002BFABC File Offset: 0x002BDCBC
	// (remove) Token: 0x06007511 RID: 29969 RVA: 0x002BFAF4 File Offset: 0x002BDCF4
	public event XUiEvent_OnDragEventHandler OnDrag;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x06007512 RID: 29970 RVA: 0x002BFB2C File Offset: 0x002BDD2C
	// (remove) Token: 0x06007513 RID: 29971 RVA: 0x002BFB64 File Offset: 0x002BDD64
	public event XUiEvent_OnHeldHandler OnHold;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x06007514 RID: 29972 RVA: 0x002BFB9C File Offset: 0x002BDD9C
	// (remove) Token: 0x06007515 RID: 29973 RVA: 0x002BFBD4 File Offset: 0x002BDDD4
	public event XUiEvent_OnScrollEventHandler OnScroll;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x06007516 RID: 29974 RVA: 0x002BFC0C File Offset: 0x002BDE0C
	// (remove) Token: 0x06007517 RID: 29975 RVA: 0x002BFC44 File Offset: 0x002BDE44
	public event XUiEvent_OnSelectEventHandler OnSelect;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x06007518 RID: 29976 RVA: 0x002BFC7C File Offset: 0x002BDE7C
	// (remove) Token: 0x06007519 RID: 29977 RVA: 0x002BFCB4 File Offset: 0x002BDEB4
	public event XUiEvent_OnVisibilityChanged OnVisiblity;

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x0600751A RID: 29978 RVA: 0x002BFCEC File Offset: 0x002BDEEC
	// (remove) Token: 0x0600751B RID: 29979 RVA: 0x002BFD24 File Offset: 0x002BDF24
	public event XUiEvent_OnEnabledChanged OnEnabled;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x0600751C RID: 29980 RVA: 0x002BFD5C File Offset: 0x002BDF5C
	// (remove) Token: 0x0600751D RID: 29981 RVA: 0x002BFD94 File Offset: 0x002BDF94
	public event XUiEvent_OnInteraction OnInteraction;

	// Token: 0x0600751E RID: 29982 RVA: 0x002BFDC9 File Offset: 0x002BDFC9
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

	// Token: 0x0600751F RID: 29983 RVA: 0x002BFDF4 File Offset: 0x002BDFF4
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

	// Token: 0x06007520 RID: 29984 RVA: 0x002BFE55 File Offset: 0x002BE055
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

	// Token: 0x06007521 RID: 29985 RVA: 0x002BFE7C File Offset: 0x002BE07C
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

	// Token: 0x06007522 RID: 29986 RVA: 0x002BFEAB File Offset: 0x002BE0AB
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

	// Token: 0x06007523 RID: 29987 RVA: 0x002BFED2 File Offset: 0x002BE0D2
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

	// Token: 0x06007524 RID: 29988 RVA: 0x002BFEFC File Offset: 0x002BE0FC
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

	// Token: 0x06007525 RID: 29989 RVA: 0x002BFF65 File Offset: 0x002BE165
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

	// Token: 0x06007526 RID: 29990 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnHovered(bool _isOver)
	{
	}

	// Token: 0x06007527 RID: 29991 RVA: 0x002BFF90 File Offset: 0x002BE190
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

	// Token: 0x06007528 RID: 29992 RVA: 0x002BFFF0 File Offset: 0x002BE1F0
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

	// Token: 0x06007529 RID: 29993 RVA: 0x002C0048 File Offset: 0x002BE248
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

	// Token: 0x0600752A RID: 29994 RVA: 0x002C00CB File Offset: 0x002BE2CB
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

	// Token: 0x0600752B RID: 29995 RVA: 0x002C00F4 File Offset: 0x002BE2F4
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

	// Token: 0x0600752C RID: 29996 RVA: 0x002C012B File Offset: 0x002BE32B
	public virtual void OnCursorUnSelected()
	{
		XUiController xuiController = this.parent;
		if (xuiController == null)
		{
			return;
		}
		xuiController.OnCursorUnSelected();
	}

	// Token: 0x0600752D RID: 29997 RVA: 0x002C013D File Offset: 0x002BE33D
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

	// Token: 0x0600752E RID: 29998 RVA: 0x002C0157 File Offset: 0x002BE357
	public virtual bool ParseAttribute(string _name, string _value)
	{
		if (_name == "always_update")
		{
			StringParsers.TryParseBool(_value, out this.AlwaysUpdate);
			return true;
		}
		return false;
	}

	// Token: 0x0600752F RID: 29999 RVA: 0x002C0178 File Offset: 0x002BE378
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

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x06007530 RID: 30000 RVA: 0x002C01C0 File Offset: 0x002BE3C0
	[XuiXmlBinding("is_online")]
	public bool IsOnline
	{
		get
		{
			return PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.LoggedIn;
		}
	}

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x06007531 RID: 30001 RVA: 0x002C01D4 File Offset: 0x002BE3D4
	[XuiXmlBinding("system_time")]
	public DateTime SystemTime
	{
		get
		{
			return DateTime.Now;
		}
	}

	// Token: 0x06007532 RID: 30002 RVA: 0x002C01DC File Offset: 0x002BE3DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2161620009U)
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
				if (num == 2161620009U)
				{
					if (_bindingName == "gamelanguage")
					{
						_value = Localization.ActiveLanguage;
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
		else if (num <= 2799769034U)
		{
			if (num <= 2745931225U)
			{
				if (num != 2577987401U)
				{
					if (num == 2745931225U)
					{
						if (_bindingName == "is_server")
						{
							_value = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "is_editmode")
				{
					_value = GameManager.Instance.IsEditMode().ToString();
					return true;
				}
			}
			else if (num != 2759425756U)
			{
				if (num == 2799769034U)
				{
					if (_bindingName == "is_ps5")
					{
						_value = DeviceFlag.PS5.IsCurrent().ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_prefab_editor")
			{
				_value = PrefabEditModeManager.Instance.IsActive().ToString();
				return true;
			}
		}
		else if (num <= 2963904327U)
		{
			if (num != 2934648263U)
			{
				if (num == 2963904327U)
				{
					if (_bindingName == "is_release")
					{
						_value = "true";
						return true;
					}
				}
			}
			else if (_bindingName == "is_console")
			{
				_value = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent().ToString();
				return true;
			}
		}
		else if (num != 3445481009U)
		{
			if (num != 3556752319U)
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
		}
		else if (_bindingName == "is_controller_input")
		{
			this.registerForInputStyleChanges();
			_value = (this.lastInputStyle != PlayerInputManager.InputStyle.Keyboard).ToString();
			return true;
		}
		return false;
	}

	// Token: 0x06007533 RID: 30003 RVA: 0x002C05B8 File Offset: 0x002BE7B8
	public void RefreshBindingsSelfAndChildren()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].RefreshBindingsSelfAndChildren();
		}
		this.Bindings.RefreshBindings();
	}

	// Token: 0x06007534 RID: 30004 RVA: 0x002C05F7 File Offset: 0x002BE7F7
	public void RefreshBindings()
	{
		this.Bindings.RefreshBindings();
	}

	// Token: 0x06007535 RID: 30005 RVA: 0x002C0604 File Offset: 0x002BE804
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

	// Token: 0x06007536 RID: 30006 RVA: 0x002C069C File Offset: 0x002BE89C
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

	// Token: 0x06007537 RID: 30007 RVA: 0x002C0708 File Offset: 0x002BE908
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

	// Token: 0x06007538 RID: 30008 RVA: 0x002C07A0 File Offset: 0x002BE9A0
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

	// Token: 0x06007539 RID: 30009 RVA: 0x002C07F8 File Offset: 0x002BE9F8
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

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x0600753A RID: 30010 RVA: 0x002C0844 File Offset: 0x002BEA44
	public PlayerInputManager.InputStyle CurrentInputStyle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.lastInputStyle;
		}
	}

	// Token: 0x0600753B RID: 30011 RVA: 0x002C084C File Offset: 0x002BEA4C
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

	// Token: 0x0600753C RID: 30012 RVA: 0x002C0879 File Offset: 0x002BEA79
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInputStyle = _style;
	}

	// Token: 0x0600753D RID: 30013 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
	}

	// Token: 0x0600753E RID: 30014 RVA: 0x002C0884 File Offset: 0x002BEA84
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

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x0600753F RID: 30015 RVA: 0x001290F5 File Offset: 0x001272F5
	public XUiController Controller
	{
		get
		{
			return this;
		}
	}

	// Token: 0x06007540 RID: 30016 RVA: 0x002C08E8 File Offset: 0x002BEAE8
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this);
	}

	// Token: 0x04005667 RID: 22119
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView viewComponent;

	// Token: 0x04005668 RID: 22120
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController parent;

	// Token: 0x04005669 RID: 22121
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<XUiController> children = new List<XUiController>();

	// Token: 0x0400566A RID: 22122
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiWindowGroup windowGroup;

	// Token: 0x0400566B RID: 22123
	public bool AlwaysUpdate;

	// Token: 0x0400566C RID: 22124
	public bool IsDirty;

	// Token: 0x0400566D RID: 22125
	public object CustomData;

	// Token: 0x0400566E RID: 22126
	public readonly ObservableDictionary<string, object> CustomAttributes = new ObservableDictionary<string, object>();

	// Token: 0x0400566F RID: 22127
	public XUi xui;

	// Token: 0x04005670 RID: 22128
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmControllerUpdate = new ProfilerMarker("XC.Update");

	// Token: 0x04005671 RID: 22129
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcUpdate = new ProfilerMarker("VC");

	// Token: 0x04005672 RID: 22130
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcNamedUpdate;

	// Token: 0x0400567F RID: 22143
	public readonly BindingsManager Bindings = new BindingsManager();

	// Token: 0x04005680 RID: 22144
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04005681 RID: 22145
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInputStyle;

	// Token: 0x04005682 RID: 22146
	[PublicizedFrom(EAccessModifier.Private)]
	public bool registeredForInputStyleChanges;
}

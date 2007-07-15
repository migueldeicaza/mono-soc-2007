/* vi:set ts=4 sw=4: */
//
// System.ComponentModel.Design.DesignerHost
//
// Authors:	 
//	  Ivan N. Zlatev (contact i-nZ.net)
//
// (C) 2006-2007 Ivan N. Zlatev

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NET_2_0

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace Mono.Design
{

	// A container for components and their designers
	//
	internal sealed class DesignerHost : Container, IDesignerLoaderHost, IDesignerHost, IServiceProvider, IComponentChangeService
	{


#region DesignerHostTransaction : DesignerTransaction
		
		private sealed class DesignerHostTransaction : DesignerTransaction
		{
			
			DesignerHost _designerHost;
			
			public DesignerHostTransaction (DesignerHost host, string description) : base (description)
			{
				_designerHost = host;
			}

			protected override void OnCancel ()
			{
				_designerHost.OnTransactionClosing (this, false);
				_designerHost.OnTransactionClosed (this, false);
			}
			
			protected override void OnCommit ()
			{
				_designerHost.OnTransactionClosing (this, true);
				_designerHost.OnTransactionClosed (this, true);
			}

		} // DesignerHostTransaction

#endregion

		
		private IServiceProvider _serviceProvider;
		private Hashtable _designers;
		private Stack _transactions;
		private IServiceContainer _serviceContainer;
		private bool _loading;
		
		public DesignerHost (IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException ("serviceProvider");

			_serviceProvider = serviceProvider;
			_serviceContainer = serviceProvider.GetService (typeof (IServiceContainer)) as IServiceContainer;
			_designers = new Hashtable ();
			_transactions = new Stack ();
			_loading = true;
		}

		
#region IContainer

		// XXX: More validation here?
		// (e.g: Make use of a potentially existing INameCreationService)
		//
		public override void Add (IComponent component, string name)
		{
			if (ComponentAdding != null)
				ComponentAdding (this, new ComponentEventArgs (component));

			AddCore (component, name);

			if (ComponentAdded != null)
				ComponentAdded (this, new ComponentEventArgs (component));
		}

		internal void AddCore (IComponent component, string name)
		{
			IDesigner designer;

			base.Add (component, name);
			
			if (_rootComponent == null) {
				_rootComponent = component;
				designer = this.CreateDesigner (component, true);
			}
			else {
				designer = this.CreateDesigner (component, false);
			}
			
			if (designer != null) {
				_designers[component] = designer;
				designer.Initialize (component);
			}

			// Activate the host and design surface once the root component is added to
			// the container and its designer is loaded and added to the designers collection
			if (_rootComponent == null) {
				_rootComponent = component;
				this.Activate ();
			}

			if (component is IExtenderProvider) {
				IExtenderProviderService service = this.GetService (typeof (IExtenderProviderService)) as IExtenderProviderService;
				if (service != null)
					service.AddExtenderProvider ((IExtenderProvider) component);
			}
		}

		public override void Remove (IComponent component)
		{
			if (ComponentRemoving != null)
				ComponentRemoving (this, new ComponentEventArgs (component));

			RemoveCore (component);

			if (ComponentRemoved != null)
				ComponentRemoved (this, new ComponentEventArgs (component));
		}

		internal void RemoveCore (IComponent component)
		{
			IDesigner designer = _designers[component] as IDesigner;
			if (designer != null)
				designer.Dispose ();
			
			_designers.Remove (component);
			
			if (component == _rootComponent)
				_rootComponent = null;

			if (component is IExtenderProvider) {
				IExtenderProviderService service = GetService (typeof (IExtenderProviderService)) as IExtenderProviderService;
				if (service != null)
					service.RemoveExtenderProvider ((IExtenderProvider) component);
			}
			
			base.Remove (component);
		}

		protected override ISite CreateSite (IComponent component, string name)
		{
			if (name == null) {
				INameCreationService nameService = this.GetService (typeof (INameCreationService)) as INameCreationService;
				if (nameService != null)
					name = nameService.CreateName (this, component.GetType ());
			}
			return new DesignModeSite (component, name, this, this);
		}
		
#endregion

		
#region IDesignerHost

		private IComponent _rootComponent;
		
		public IContainer Container {
			get { return this; }
		}

		public bool InTransaction {
			get {
				if (_transactions != null && _transactions.Count > 0)
					return true;

				return false;
			}
		}

		public bool Loading {
			get { return _loading; }
		}

		public IComponent RootComponent {
			get { return _rootComponent; }
		}

		public string RootComponentClassName {
			get {
				if (_rootComponent != null)
					return ((object)_rootComponent).GetType ().AssemblyQualifiedName;

				return null;
			}
		}

		public string TransactionDescription {
			get {
				if (_transactions != null && _transactions.Count > 0)
					return ((DesignerHostTransaction) _transactions.Peek()).Description;
					
				return null;
			}
		}

		
		// GUI loading in the designer should be done after the Activated event is raised.
		//
		public void Activate ()
		{
			ISelectionService selectionService = GetService (typeof (ISelectionService)) as ISelectionService;

			// Set the Primary Selection to be the root component
			//
			if (selectionService != null)
				selectionService.SetSelectedComponents (new IComponent[] { _rootComponent });
			
			if (Activated != null)
				Activated (this, EventArgs.Empty);
		}
		
		public IComponent CreateComponent (Type componentClass)
		{
			return CreateComponent (componentClass, null);
		}

		public IComponent CreateComponent (Type componentClass, string name)
		{
			if (componentClass == null)
				throw new ArgumentNullException ("componentClass");
				
			else if (!typeof(IComponent).IsAssignableFrom(componentClass))
				throw new ArgumentException ("componentClass");

			IComponent component = this.CreateInstance (componentClass) as IComponent;
			this.Add (component, name);
			
			return component;
		}

		internal object CreateInstance (Type type)
		{
			if (type == null)
				throw new System.ArgumentNullException ("type");
			
			// FIXME: Should I use TypeDescriptor.CreateInstance() for 2.0 ?
			//
			return Activator.CreateInstance (type, BindingFlags.CreateInstance | BindingFlags.Public
							 | BindingFlags.Instance, null,  null, null);
		}

		internal IDesigner CreateDesigner (IComponent component, bool rootDesigner)
		{
			if (component == null)
				throw new System.ArgumentNullException ("component");
			
			if (rootDesigner)
				return TypeDescriptor.CreateDesigner (component, typeof (IRootDesigner));
			else
				return TypeDescriptor.CreateDesigner (component, typeof (IDesigner));
		}

		public void DestroyComponent (IComponent component)
		{
			if (component.Site != null && component.Site.Container == this) {
				OnComponentChanging (component, null);
				
				this.Remove (component); // takes care for the designer as well
				component.Dispose ();

				OnComponentChanged (component, null, null, null);
			}
		}

		public IDesigner GetDesigner (IComponent component)
		{
			if (component == null)
				throw new ArgumentNullException ("component");
			
			return _designers[component] as IDesigner;
		}

		public DesignerTransaction CreateTransaction ()
		{
			return CreateTransaction (null);
		}
		
		public DesignerTransaction CreateTransaction (string description)
		{
			if (TransactionOpening != null)
				TransactionOpening (this, EventArgs.Empty);
			
			DesignerHostTransaction transaction = new DesignerHostTransaction (this, description);
			_transactions.Push (transaction);

			if (TransactionOpened != null)
				TransactionOpened (this, EventArgs.Empty);
			
			return transaction;
		}


		public Type GetType (string typeName)
		{
			Type result;
			ITypeResolutionService s = GetService (typeof (ITypeResolutionService)) as ITypeResolutionService;
			
			if (s != null)
				result = s.GetType (typeName);
			else
				result = Type.GetType (typeName);
			
			return result;
		}

		// Take care of disposing the designer the base.Dispose will cleanup
		// the components.
		//
		protected override void Dispose (bool disposing)
		{
			Unload ();
			base.Dispose (disposing);
		}

		
		public event EventHandler Activated;
		public event EventHandler Deactivated;
		public event EventHandler LoadComplete;	 
		public event DesignerTransactionCloseEventHandler TransactionClosed;
		public event DesignerTransactionCloseEventHandler TransactionClosing;
		public event EventHandler TransactionOpened;
		public event EventHandler TransactionOpening;
		
		
		private void OnTransactionClosing (DesignerHostTransaction raiser, bool commit)
		{
			bool lastTransaction = false;
			if (_transactions.Count > 0 && _transactions.Peek() == raiser)
				lastTransaction = true;
				
			if (TransactionClosing != null)
				TransactionClosing (this, new DesignerTransactionCloseEventArgs (commit, lastTransaction));  
		}

		private void OnTransactionClosed (DesignerHostTransaction raiser, bool commit)
		{
			bool lastTransaction = false;
			if (_transactions.Count > 0 && _transactions.Peek() == raiser) {
				lastTransaction = true;
				_transactions.Pop ();
			}
				
			if (TransactionClosed != null)
				TransactionClosed (this, new DesignerTransactionCloseEventArgs (commit, lastTransaction));
		}
		
#endregion

		
#region IDesignerLoaderHost
		
		internal event LoadedEventHandler DesignerLoaderHostLoaded;
		internal event EventHandler DesignerLoaderHostLoading;
		internal event EventHandler DesignerLoaderHostUnloading;
		internal event EventHandler DesignerLoaderHostUnloaded;
		
		public void EndLoad (string rootClassName, bool successful, ICollection errorCollection)
		{
			if (DesignerLoaderHostLoaded != null)
				DesignerLoaderHostLoaded (this, new LoadedEventArgs (successful, errorCollection));
			
			if (LoadComplete != null)
				LoadComplete (this, EventArgs.Empty);

			_loading = false; // _loading = true is set by the ctor
		}

		// BasicDesignerLoader invokes this.Reload, then invokes BeginLoad on itself, 
		// then when loading it the loader is done it ends up in this.EndLoad.
		// At the end of the day Reload is more like Unload.
		// 
		public void Reload ()
		{
			_loading = true;
			Unload ();
			if (DesignerLoaderHostLoading != null)
				DesignerLoaderHostLoading (this, EventArgs.Empty);
		}

		private void Unload ()
		{
			if (DesignerLoaderHostUnloading != null)
				DesignerLoaderHostUnloading (this, EventArgs.Empty);

			IComponent[] components = new IComponent[this.Components.Count];
			this.Components.CopyTo (components, 0);
			
			foreach (IComponent component in components)
				this.Remove (component);

			_transactions.Clear ();
			
			if (DesignerLoaderHostUnloaded != null)
				DesignerLoaderHostUnloaded (this, EventArgs.Empty);
		}
						
#endregion


#region IComponentChangeService
		
	public event ComponentEventHandler ComponentAdded;
	public event ComponentEventHandler ComponentAdding;
	public event ComponentChangedEventHandler ComponentChanged;
	public event ComponentChangingEventHandler ComponentChanging;
	public event ComponentEventHandler ComponentRemoved;
	public event ComponentEventHandler ComponentRemoving;
	public event ComponentRenameEventHandler ComponentRename;
	   
		
	public void OnComponentChanged (object component, MemberDescriptor member, object oldValue, object newValue)
	{
		if (ComponentChanged != null)
			ComponentChanged (this, new ComponentChangedEventArgs (component, member, oldValue, newValue));
	}
				
	public void OnComponentChanging (object component, MemberDescriptor member)
	{
		if (ComponentChanging != null)
			ComponentChanging (this, new ComponentChangingEventArgs (component, member));
	}

	internal void OnComponentRename (object component, string oldName, string newName)
	{
		if (ComponentRename != null)
			ComponentRename (this, new ComponentRenameEventArgs (component, oldName, newName));
	}
		
#endregion

	
#region IServiceContainer
		// Wrapper around the DesignSurface service container
		//

		public void AddService (Type serviceType, object serviceInstance)
		{
			_serviceContainer.AddService (serviceType, serviceInstance);
		}
		
		public void AddService (Type serviceType, object serviceInstance, bool promote)
		{
			_serviceContainer.AddService (serviceType, serviceInstance, promote);
		}
		
		public void AddService (Type serviceType, ServiceCreatorCallback callback)
		{
			_serviceContainer.AddService (serviceType, callback);
		}

		public void AddService (Type serviceType, ServiceCreatorCallback callback, bool promote)
		{
			_serviceContainer.AddService (serviceType, callback, promote);
		}

		public void RemoveService (Type serviceType)
		{
			_serviceContainer.RemoveService (serviceType);
		}

		public void RemoveService (Type serviceType, bool promote)
		{
			_serviceContainer.RemoveService (serviceType, promote);
		}
			
#endregion


#region IServiceProvider

	public new object GetService (Type serviceType)
	{
		return _serviceProvider.GetService (serviceType);
	}
		
#endregion
	  
		}

	}
#endif

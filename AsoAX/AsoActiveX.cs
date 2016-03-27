/****************************** Module Header ******************************\
* Module Name:  CSActiveXCtrl.cs
* Project:      CSActiveX
* Copyright (c) Microsoft Corporation.
* 
* The sample demonstrates an ActiveX control written in C#. ActiveX controls
* (formerly known as OLE controls) are small program building blocks that can 
* work in a variety of different containers, ranging from software development 
* tools to end-user productivity tools. For example, it can be used to create 
* distributed applications that work over the Internet through web browsers. 
* ActiveX controls can be written in MFC, ATL, C++, C#, Borland Delphi and 
* Visual Basic. In this sample, we focus on writing an ActiveX control using 
* C#. We will go through the basic steps of adding UI, properties, methods,  
* and events to the control.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/en-us/openness/resources/licenses.aspx#MPL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Permissions;
#endregion


namespace AsoAX
{
    #region Interfaces

    /// <summary>
    /// AxCSActiveXCtrl describes the COM interface of the coclass 
    /// </summary>
    [Guid("05777DD1-A023-4C11-9F31-C41C230BDFF1")]
    public interface AxCSActiveXCtrl
    {
        #region Properties

        bool Visible { get; set; }          // Typical control property
        bool Enabled { get; set; }          // Typical control property
        int ForeColor { get; set; }         // Typical control property
        int BackColor { get; set; }         // Typical control property
        float FloatProperty { get; set; }   // Custom property
        string TestString { get; set; }

        #endregion

        #region Methods

        void Refresh();                     // Typical control method
        string HelloWorld();                // Custom method

        string Test();

        #endregion
    }

    /// <summary>
    /// AxCSActiveXCtrlEvents describes the events the coclass can sink
    /// </summary>
    [Guid("0A809A06-DB35-4FC7-B4D3-404B73E9CE5C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    // The public interface describing the events of the control
    public interface AxCSActiveXCtrlEvents
    {
        #region Events

        // Must explicitly define DISPID for each event, otherwise, the 
        // callback address cannot be found when the event is fired.
        [DispId(1)]
        void Click();
        [DispId(2)]
        void FloatPropertyChanging(float NewValue, ref bool Cancel);
        [DispId(3)]
        void TestStringChange(string str);
        #endregion
    }

    #endregion

    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(AxCSActiveXCtrlEvents))]
    [Guid("070661F9-5C1E-4B09-9B8F-A03D1169BA5A")]
    public partial class AsoActiveX : UserControl, AxCSActiveXCtrl, IObjectSafety
    {
        #region Saftey Interface
        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;


        [ComVisible(true)]
        public int GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            return S_OK;
        }

        [ComVisible(true)]
        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            return S_OK;
        }
        #endregion

        #region ActiveX Control Registration

        // These routines perform the additional COM registration needed by 
        // ActiveX controls

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ActiveXCtrlHelper.RegasmRegisterControl(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            try
            {
                ActiveXCtrlHelper.RegasmUnregisterControl(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw; // Re-throw the exception
            }
        }

        #endregion


        #region Initialization

        public AsoActiveX()
        {
            InitializeComponent();

            // For the Click event that is re-defined.
            base.Click += new EventHandler(CSActiveXCtrl_Click);

            // These functions are used to handle Tab-stops for the ActiveX 
            // control (including its child controls) when the control is 
            // hosted in a container.
            this.LostFocus += new EventHandler(CSActiveXCtrl_LostFocus);
            this.ControlAdded += new ControlEventHandler(
                CSActiveXCtrl_ControlAdded);

            // Raise custom Load event
            this.OnCreateControl(); 
        }

        // This event will hook up the necessary handlers
        void CSActiveXCtrl_ControlAdded(object sender, ControlEventArgs e)
        {
            // Register tab handler and focus-related event handlers for 
            // the control and its child controls.
            ActiveXCtrlHelper.WireUpHandlers(e.Control, ValidationHandler);
        }

        // Ensures that the Validating and Validated events fire properly
        internal void ValidationHandler(object sender, System.EventArgs e)
        {
            if (this.ContainsFocus) return;

            this.OnLeave(e); // Raise Leave event

            if (this.CausesValidation)
            {
                CancelEventArgs validationArgs = new CancelEventArgs();
                this.OnValidating(validationArgs);

                if (validationArgs.Cancel && this.ActiveControl != null)
                    this.ActiveControl.Focus();
                else
                    this.OnValidated(e); // Raise Validated event
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_SETFOCUS = 0x7;
            const int WM_PARENTNOTIFY = 0x210;
            const int WM_DESTROY = 0x2;
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_RBUTTONDOWN = 0x204;

            if (m.Msg == WM_SETFOCUS)
            {
                // Raise Enter event
                this.OnEnter(System.EventArgs.Empty);
            }
            else if (m.Msg == WM_PARENTNOTIFY && (
                m.WParam.ToInt32() == WM_LBUTTONDOWN || 
                m.WParam.ToInt32() == WM_RBUTTONDOWN))
            {
                if (!this.ContainsFocus)
                {
                    // Raise Enter event
                    this.OnEnter(System.EventArgs.Empty);
                }
            }
            else if (m.Msg == WM_DESTROY && 
                !this.IsDisposed && !this.Disposing)
            {
                // Used to ensure the cleanup of the control
                this.Dispose();
            }

            base.WndProc(ref m);
        }

        // Ensures that tabbing across the container and the .NET controls
        // works as expected
        void CSActiveXCtrl_LostFocus(object sender, EventArgs e)
        {
            ActiveXCtrlHelper.HandleFocus(this);
        }

        #endregion


        #region Properties

        public new int ForeColor
        {
            get { return ActiveXCtrlHelper.GetOleColorFromColor(base.ForeColor); }
            set { base.ForeColor = ActiveXCtrlHelper.GetColorFromOleColor(value); }
        }

        public new int BackColor
        {
            get { return ActiveXCtrlHelper.GetOleColorFromColor(base.BackColor); }
            set { base.BackColor = ActiveXCtrlHelper.GetColorFromOleColor(value); }
        }

        private float fField = 0;
        private string sField = null;

        /// <summary>
        /// A custom property with both get and set accessor methods.
        /// </summary>
        public float FloatProperty
        {
            get { return this.fField; }
            set 
            {
                bool cancel = false;
                // Raise the event FloatPropertyChanging
                if (null != FloatPropertyChanging)
                    FloatPropertyChanging(value, ref cancel);
                if (!cancel)
                {
                    this.fField = value;
                    this.lbFloatProperty.Text = value.ToString();
                }
            }
        }

        public string TestString
        {
            get { return this.sField; }
            set
            {
                bool cancel = false;
                // Raise the event StringPropertyChanging
                if (null != TestStringChange)
                    MessageBox.Show("event fire");
                    TestStringChange(value);
                if (!cancel)
                {
                    this.sField = value;
                    this.label3.Text = value;
                }
            }
        }

        #endregion


        #region Methods

        public string HelloWorld()
        {
            return "HelloWorld";
        }

        public string Test()
        {
            return "Test";
        }

        #endregion


        #region Events

        // This section shows the examples of exposing a control's events.
        // Typically, you just need to
        // 1) Declare the event as you want it.
        // 2) Raise the event in the appropriate control event.

        [ComVisible(false)]
        public delegate void ClickEventHandler();
        public new event ClickEventHandler Click = null;
        void CSActiveXCtrl_Click(object sender, EventArgs e)
        {
            if (null != Click) Click(); // Raise the new Click event.
        }

        [ComVisible(false)]
        public delegate void FloatPropertyChangingEventHandler(float NewValue, ref bool Cancel);
        public event FloatPropertyChangingEventHandler FloatPropertyChanging = null;

        [ComVisible(false)]
        public delegate void TestStringChangeEventHandler(string str);
        public event TestStringChangeEventHandler TestStringChange = null;

        #endregion


        private void btnMessage_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(tbMessage.Text, "HelloWorld",
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            TestString = tbMessage.Text;

        }
    }

} // namespace AsoAX

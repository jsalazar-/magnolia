﻿#pragma checksum "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "17A681D1A2D2F7E2DEE7F4C85F58E4FD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FirstFloor.ModernUI.App.Pages.Tabs {
    
    
    /// <summary>
    /// DescuentoCliente
    /// </summary>
    public partial class DescuentoCliente : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBuscarCliente;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid datagridCliente;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelar;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnElegir;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ModernUIDemo;component/pages/tabs/descuentocliente.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtBuscarCliente = ((System.Windows.Controls.TextBox)(target));
            
            #line 12 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
            this.txtBuscarCliente.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBuscarCliente_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.datagridCliente = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            this.btnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
            this.btnCancelar.Click += new System.Windows.RoutedEventHandler(this.btnCancelar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnElegir = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\Pages\Tabs\DescuentoCliente.xaml"
            this.btnElegir.Click += new System.Windows.RoutedEventHandler(this.btnElegir_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


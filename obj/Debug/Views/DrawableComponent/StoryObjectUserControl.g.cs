﻿#pragma checksum "..\..\..\..\Views\DrawableComponent\StoryObjectUserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DD610E24E22D371B2E4E92F178BC74366544B103DD7FB0398AE7BDCCB8433B3A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using StoryMaker.Converters;
using StoryMaker.Models.Components;
using StoryMaker.Views.DrawableComponent;
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


namespace StoryMaker.Views.DrawableComponent {
    
    
    /// <summary>
    /// StoryObjectUserControl
    /// </summary>
    public partial class StoryObjectUserControl : System.Windows.Controls.Canvas, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\..\..\Views\DrawableComponent\StoryObjectUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal StoryMaker.Views.DrawableComponent.StoryObjectUserControl Canvas;
        
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
            System.Uri resourceLocater = new System.Uri("/Assembly-CSharp;component/views/drawablecomponent/storyobjectusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\DrawableComponent\StoryObjectUserControl.xaml"
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
            this.Canvas = ((StoryMaker.Views.DrawableComponent.StoryObjectUserControl)(target));
            
            #line 11 "..\..\..\..\Views\DrawableComponent\StoryObjectUserControl.xaml"
            this.Canvas.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.OnMouseDown);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\Views\DrawableComponent\StoryObjectUserControl.xaml"
            this.Canvas.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.StoryObjectUserControl_OnMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


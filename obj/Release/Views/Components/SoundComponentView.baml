   M S B A M L   `   `   `  ����   Assembly-CSharp4    .StoryMaker.Views.Components.SoundComponentView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  -     >�Name$  userControl�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$
 Open��.���  � DataContext ��  �� ElementName$ userControl�� � ColumnDefinitions �� $��10 ��� $��60 ��� $��1* ��� $��7* ��� $��10 � � RowDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� $��1��$��Center=�$
��Left=�$��صدا��� $��0��$��3�� �� $��7* ��� $��1* �.+��� $��70��$
��Auto��$��0,0,5,0q����� *AudioSource.SoundAsset.Name+�� $��1��$��25��$
��Open��.��'  !MaterialDesignThemes.Wpf.PackIcon  $��15��
�������$��15���  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$ Audio=��� $��1��$��1��$��Center=�$
��Left=�$��9(�$��پخش از ابتدا���� $
��Auto��$��1��$��3��$
��Left=� d� 	IsChecked �� *AudioSource.PlayOnAwake+�� $��2��$��1��$��Center=�$
��Left=�$��9(�$��
تکرار���� $
��Auto��$��2��$��3��$
��Left=� �� *AudioSource.Loop+�� $��3��$��1��$��Center=�$
��Left=�$��9(�$��
تاخیر���� $��3��$��3��$��Stretch=����� *AudioSource.Delay+
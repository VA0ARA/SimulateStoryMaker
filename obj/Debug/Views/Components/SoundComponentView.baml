   M S B A M L   `   `   `  ����  Assembly-CSharp4    .StoryMaker.Views.Components.SoundComponentView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5          >�Name$  userControl��5       J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$
 Open��5
      6(   .��5      �  � DataContext 6   ��  �� ElementName$ userControl�� � ColumnDefinitions 5   
   �� 5      $��10 �6   �� 5      $��60 �6   �� 5      $��1* �6   �� 5      $��7* �6   �� 5      $��10 �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� $��1��6   $��Center=�5      $
��Left=�5      $��صدا��5      � 5   
   $��0��6   $��3��5        5!      �� 5"      $��7* �6#   �� 5#      $��1* �6#   .+�5%      �� $��70��6   $
��Auto��5&      $��0,0,5,0q�5(      ��5'      �� *AudioSource.SoundAsset.Name+5(      �� 5)      $��1��6   $��25��5*      $
��Open��5+      .��5,      '  !MaterialDesignThemes.Wpf.PackIcon  $��15��6*   
�������$��15��6M   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$ Audio=�6X   5-      5.      �� 50   
   $��1��6   $��1��51      $��Center=�52      $
��Left=�53      $��9(�54      $��پخش از ابتدا��55      �� 56   
   $
��Auto��6   $��1��57      $��3��58      $
��Left=�59       d� 	IsChecked 5:      �� *AudioSource.PlayOnAwake+�� 5<   
   $��2��6   $��1��5=      $��Center=�5>      $
��Left=�5?      $��9(�5@      $��
تکرار��5A      �� 5B   
   $
��Auto��6   $��2��5C      $��3��5D      $
��Left=�5E       5F      �� *AudioSource.Loop+�� 5H   
   $��3��6   $��1��5I      $��Center=�5J      $
��Left=�5K      $��9(�5L      $��
تاخیر��5M      �� 5N   
   $��3��6   $��3��5O      $��Stretch=�5P      ��5Q      �� *AudioSource.Delay+5R      5S      
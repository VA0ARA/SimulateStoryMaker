   M S B A M L   `   `   `  ����   Assembly-CSharp#    StoryMaker.Views.LeftSideMenu   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  -     >�Name$  UserControl��.��� $
��Left=�$	��250��   MainColor-Primary#��[   K� ZIndex$ 2�� � DataContext ��  �� ElementName$ UserControl�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 7Q�   ShadowDepth$ 3Q�   Color$ #111111��.+�� $��Stretch=� � RowDefinitions �� $��50 ��� $��1* ��� $��50 �.+�� -   	 �Name$	 LeftSideMenuHeader��$��0��.+�]� $��0,10,0,5q�.+��� -   
 ��Name$
 LeftSideMenuCloseButton��$
��Left=�$��50��
������ 
������ $��Center=�$
��Left=�.��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]]   Kind$ Close=�$��20��$��20��  MainColor-Detail#��[ � -   $	 LeftSideMenuBody��$��	25,0,25,0q�$��1��.+���  �� Orientation$ Vertical=�$��2��.+�� .+�]� .+��� $��
ذخیره��$��15(� �� TextAlignment$
 Left=�$��Center=��� 
������ 
������ $��39��$��Stretch=�$��Stretch=����� *ViewModel.SaveProjectCommand+ �� Mode$ OneWay=����� *ProjectModel+� .+�]� .+��� $��ساخت داستان��$��15(�$
 Left=�$��Center=��� 
������ 
������ $��39��$��Stretch=�$��Stretch=����� *ViewModel.BuildStoryCommand+���� *ProjectModel+� .+�]� .+��� $!��افزودن انیمیشن��$��15(�$
 Left=�$��Center=��� 
������ 
������ $��39��$��Stretch=�$��Stretch=����� *ViewModel.AddAnimationCommand+���� *
SceneModel+��   MainColor-Separator#��[ $��Bottom=�� .+�]� .+�  $ Export=�$��0,0,20,0q�$��Center=�$��20��$��20��#��[ �� $��ذخیره سکانس��$��15(�$
 Left=�$��Center=��� -   $
 SaveSceneButton��
������ 
������ $��39��$��Stretch=�$��Stretch=����� *ViewModel.ExportSceneCommand+���� *
SceneModel+� .+�]� .+�  $ Import=�$��0,0,20,0q�$��Center=�$��20��$��20��#��[ �� $!��بارگذاری سکانس��$��15(�$
 Left=�$��Center=��� -   $
 LoadSceneButton��
������ 
������ $��39��$��Stretch=�$��Stretch=����� *ViewModel.LoadSceneCommand+���� *ProjectModel+� .+�]� .+�  $ Import=�$��0,0,20,0q�$��Center=�$��20��$��20��#��[ �� $��
راندو��$��15(�$
 Left=�$��Center=��� -   $
 OpenRenderDialogue��
������ 
������ $��39��$��Stretch=�$��Stretch=����� *!ViewModel.OpenRenderViewCommand+���� *
SceneModel+�� #��[ $��Bottom=�� .+�]� .+�  $ Settings=�$��0,0,20,0q�$��Center=�$��20��$��20��#��[ �� $��تنظیمات��$��15(�$
 Left=�$��Center=��� -   $
 SettingButton��
������ 
������ $��39��$��Stretch=�$��Stretch=�� .+�]� .+�  $ ExitRun=�$��0,0,20,0q�$��Center=�$��20��$��20��#��[ �� $��خروج��$��15(�$
 Left=�$��Center=��� -	   $
 
ExitButton��
������ 
������ $��39��$��Stretch=�$��Stretch=����� *ViewModel.ExitCommandDelegate+
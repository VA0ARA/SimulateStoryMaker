   M S B A M L   `   `   `  ����  Assembly-CSharp'    !StoryMaker.Helpers.ChooseFromList   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          �Name$  window��5   	    � Title$ ChooseFromList��5   	      MainColor-Surface#��[  5   	   $	��297��5	   	   $	��254��5
   	   .��5      �  � RowDefinitions 5   
   �� 5      $	��11* �6   �� 5      $��60 �6   .+�5   
   �� -   5         ��Name$ 	ItemsView��5      $��10q�5      $��1��5      ��5      �� *Items+ �� ElementName$ window��5      ]� 5   
   $��1��6   .+�5      �� -   5         ��Name$ 
SelectView��5      $
��Left=�5      $��Select��5      $��Center=�5      $
��10,0q�5      $��10q�5       �� 	IsDefault . 5      �� 5      -   5        $ 
CancelView��5      $
��Left=�5      $��Cancel��5      $
��Left=�5       $
��10,0q�5!      $��10q�5"       �� IsCancel . $��Center=�5$      5%      5'      5(      
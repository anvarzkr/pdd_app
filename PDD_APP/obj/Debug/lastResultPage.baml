   M S B A M L   `   `   `  ����  Station One    PDD_APP.lastResultPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  lastResultPage��5      .��5
      � -   5         �Name$ mainGrid��5
       � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ resultTextBorder��5      $��0��5      
�������$��1q�5       �� CornerRadius$ 6��5      $��Center=�5      $	��400��5       �� Padding$ 10q�5      $��10q�5      ._�5      �� -   5         ��Name$ 
resultText��5      $��0��5      $��0��5      
�������$��40(�5      $	��500"�5      $��Center=�5       
ИТОГИ5"      5#      �� 5$   
    �� Orientation$ Vertical=�6   $��1��5%      $��Center=�5&      $��Center=�5'      .+�5(      �� $ 
Horizontal=�6   $��Center=�5)      .+�5*      �� -   5        $ fnameBlockBorder��5*      $ 10q�5+      $��20q�5,      $	��400��5-      $ 4��5.      
�������$��1q�50      ._�51      �� -   5        $ 
fnameBlock��51       
�������$��25(�53       $��Center=�54       6?   55      �� 56      -   5        $ lnameBlockBorder��56      $ 10q�57      $��20q�58      $	��400��59      $ 4��5:      
�������$��1q�5<      ._�5=      �� -   5        $ 
lnameBlock��5=       
�������$��25(�5?       $��Center=�5@       6?   5A      �� 5B      -   5        $ idBlockBorder��5B      $ 10q�5C      $��20q�5D      $	��400��5E      $ 4��5F      
�������$��1q�5H      ._�5I      �� -	   5        $ idBlock��5I       
�������$��25(�5K       $��Center=�5L       6?   5M      5N      �� 5O      $ 
Horizontal=�6   $
��0, 5q�5P      $��Center=�5Q      .+�5R      �� -
   5        $ border1��5R      $ 10q�5S      $	��500��5T      $ 4��5U      
�������$��1q�5W      ._�5X      �� -   5        $ border1Text��5X       $��Center=�5Y       
�������$��20(�5[       ОБЩЕЕ ВРЕМЯ6;   5\      �� 5]      -   5        $ border11��5]      $ 10q�5^      $	��120��5_      $��20, 0, 0, 0q�5`      $ 4��5a      
�������$��1q�5c      ._�5d      �� -   5        $ 	timeBlock��5d       
�������$��20(�5f       $��Center=�5g       6?   5h      5i      �� 5j      $ 
Horizontal=�6   $��Center=�5k      $
��0, 5q�5l      .+�5m      �� -   5        $ border2��5m      $ 10q�5n      $	��500��5o      $ 4��5p      
�������$��1q�5r      ._�5s      �� -   5        $ border2Text��5s       $��Center=�5t       
�������$��20(�5v       %#ПРАВИЛЬНЫХ ОТВЕТОВ6B   5w      �� 5x      -   5        $ border12��5x      $ 10q�5y      $	��120��5z      $��20, 0, 0, 0q�5{      $ 4��5|      
�������$��1q�5~      ._�5      �� -   5        $ 	rAnsBlock��5       
�������$��20(�5�       $��Center=�5�       6?   5�      5�      �� 5�      $ 
Horizontal=�6   $��Center=�5�      $
��0, 5q�5�      .+�5�      �� -   5        $ border3��5�      $ 10q�5�      $	��500��5�      $ 4��5�      
�������$��1q�5�      ._�5�      �� -   5        $ border3Text��5�       $��Center=�5�       
�������$��20(�5�       )'НЕПРАВИЛЬНЫХ ОТВЕТОВ6D   5�      �� 5�      -   5        $ border13��5�      $ 10q�5�      $	��120��5�      $��20, 0, 0, 0q�5�      $ 4��5�      
�������$��1q�5�      ._�5�      �� -   5        $ 	wAnsBlock��5�       
�������$��20(�5�       $��Center=�5�       6?   5�      5�      �� 5�      -   5        $ resendButton��5�      $��0, 20q�5�      $ 10q�5�      $	��400��5�      $ 4��5�      
�������$��1q�5�      ._�5�      �� 
�������$��20(�5�      $��Center=�5�      )'ОТПРАВИТЬ РЕЗУЛЬТАТЫ5�      5�      �� 5�      -   5        $ 
helpButton��5�      $��0, 20q�5�      $ 10q�5�      $	��400��5�      $ 4��5�      
�������$��1q�5�      ._�5�      �� 
�������$��20(�5�      $��Center=�5�      ПОЗВАТЬ СУДЬЮ5�      5�      �� 5�      -   5        $ 
menuButton��5�      $��0, 20q�5�      $ 10q�5�      $	��400��5�      $ 4��5�      
�������$��1q�5�      $y�	Collapsed=�5�      ._�5�      �� 
�������$��20(�5�      $��Center=�5�      В ГЛАВНОЕ МЕНЮ5�      5�      5�      5�      5�      
   M S B A M L   `   `   `  ����D  @TicTacToe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TicTacToe	TicTacToe  5   	       TicTacToe.Lobby   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:TicTacToe     5   	     � Title$  Lobby��5   	   $	��620��6   $	��500��6$   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��1.5* �6   �� 5      $��5* �6   �� 5      $
��0.5* �6   .+�5   
   �� -   5         ��Name$ 
PlayerList��5      $	��390��6&   $	��470��63   $��Center=�6?   $��1��6\   $��Center=�6i   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property �@��
IsSelected �� Value 6/   �� *IsFav+5      5      �� 5   
   -   5         ��Name$ ConnButt��5      $��Подключиться��6#   $
��Left=�6:   $��
354,30,0,0q�6U   $	��Top=�6i   $	��125��6�   $��35��6�   $��16(�6�   6�   �� 5   
   -   5        $ PlayButt��5      $��Играть��6#   $
��Left=�64   $��
354,90,0,0q�6O   $	��Top=�6c   $	��125��6{   $��35��6�   $��16(�6�   6�   �� 5   
   -   5        $ ExitButt��5      $��
Выйти��6#   $��Center=�63   $	��Top=�6P   $	��470��6h   $��35��6t   $��2��6�   $��16(�6�   6�   �� 5   
   -   5         ��Name$ NickBox��5      $
��Left=�6#   $��35��6>   $��	15,29,0,0q�6J    �� TextWrapping$
 Wrap=�6]   $��NickName��6q   $	��Top=�6�   $	��300��6�   $��20(�6�   �� 5   
   -   5        	 ��Name$	 	NickLable��5      $#��Введите ваш ник:��6#   $
��Left=�6?   $	��467��6Z   $��	11,1,0,94q�6f   $��14(�6y   �� 5   
   $
�� 50 ��6   $����1) Войдите в лобби, нажав кнопку Подключиться 2) Выберите игрока и нажмите кнопку Играть��6"   $
��Wrap=�6�   $��15,88,174,409q�6�   $��13(�6�   $��2��6�   5      5      
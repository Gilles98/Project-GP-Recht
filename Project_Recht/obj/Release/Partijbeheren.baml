   M S B A M L   `   `   `  ���� H  DProject_Recht, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Project_RechtProject_Recht  !    Project_Recht.Partijbeheren   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     X TXceed.Wpf.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  'localclr-namespace:Project_Recht  b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � Title$ Partijbeheren��$	��450��$	��650�� � 
ResizeMode$ CanMinimize=�.���  � ColumnDefinitions �� ��  � RowDefinitions �� $	��100 ��� �� �� �� �� .+��� $��0��$��2��
����  �$��
0,0,-0.4,0q�.+��� $��Partij Beheren��$��45(� �� TextAlignment$ Center=�
����  �
�������$��
0,0,-0.4,0q�	�  	� 	GroupName$ Rechtspersoon��$��	Beklaagde��$��0��$��1��$��15(�$��214,0,9.6,53q�$��2��$��75��$��Bottom=� d� 	IsChecked �� *CheckRadio1+{��� *EnabledBeklaagde+	� $ Rechtspersoon��$��	Aanklager��$��1��$��1��$��15(�$��10.4,0,198.2,53.2q�$��2�� �� *CheckRadio2+{��� *EnabledAanklager+�� $��2��$��16(�$��0��)  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 Voornaam��$��130,10.4,78.6,10.2q����� *
Voornaam+�� $��2��$��16(�$��1��$	 
Achternaam��$��68.4,10.4,140.2,10.2q����� *
Achternaam+�� $��3��$��16(�$��0��$	 Gemeente��$��130,10.4,78.6,10.2q����� *
Gemeente+�� $��3��$��16(�$��1��$	 Straat��$��68.4,10.4,140.2,10.2q����� *Straat+�� $��4��$��16(�$��0��$	 
Huisnummer��$��130,10.4,78.6,10.2q����� *
Huisnummer+�� $��4��$��16(�$��1��$	 Postcode��$��68.4,10.4,140.2,10.2q����� *
Postcode+
 �� ValidatesOnDataErrors
 . �� 
����  �$��0��$��6��$��Verwijderen��$��Verwijderen��$��5q����� �� 
����  �$��1��$��6��$��Bewaren��$��Bewaren��$��5q����� 
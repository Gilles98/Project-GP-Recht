   M S B A M L   `   `   `  ���� H  DProject_Recht, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Project_RechtProject_Recht  $    Project_Recht.RechtzaakBeheren   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     X TXceed.Wpf.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  'localclr-namespace:Project_Recht     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � Title$ RechtzaakBeheren��$	��800��$
��1000�� � 
ResizeMode$ CanMinimize=�.���  � ColumnDefinitions �� ��  � RowDefinitions �� $	��100 ��� $	��159 ��� $��152.8 ��� $
��143* ��� $	��86* �.+��� $��0��$��2��
����  �$��
0,0,-0.4,0q� �� Orientation$ 
Horizontal=�.+��� $��Rechtzaak Beheren��$��Center=�$��45(�
����  �
�������$��251,18,203.6,17q�$��65��$	��534���� $��0��$��1��$ Vertical=�$��20,20,19.8,19.2q�.+��� $��Klacht��$��40(�$
��Left=�$��10q��� $��10q����� *Klacht+ �� ValidatesOnDataErrors . �� $��1��$��1��$ Vertical=�$��20.2,20,19.6,9.2q�.+��� $��	Rechtbank��$��40(�$
��Left=�$��10q��� 
���   �$��1q�$��10q�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Selecteer een rechtbank��	 �� SelectedItem	 �� *SelectedRechtbank+ . ���� *Rechtbanken+�� $��0��$��2��$ Vertical=�$��20,20.8,19.8,12q�.+��� $��Rechter��$��40(�$
��Left=�$��10q��� 
���   �
 � ToolTip$
 Selecteer een rechter��$��1q�$��10q�	 �� *SelectedRechter+ . {��� *EnableSelectionRechter+���� *
Rechters+�� $��1��$��2��$ Vertical=�$��20.2,20.8,19.6,12q�.+��� $��Datum��$��40(�$
��Left=�$��10q�(  "System.Windows.Controls.DatePicker  |�.  $��
10,0,9.8,0q�$��37��   SelectedDate �� *SelectedDateTime+ . &   System.Windows.Controls.DataGrid  -     Name$ 
aanklagers��
�������$��1q�
���   �$��3��$��300,30,496.6,23q� J� RenderTransformOrigin$ 0.5,0.5&�   
IsReadOnly .    AutoGenerateColumns .  $��2������ *
Aanklagers+	 �� *SelectedPersoon+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Aanklager(s)��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
Voornaam+  -   $ 
beklaagdes��
�������$��1q�
���   �$��3��$��0.2,30,300.6,23q�$ 0.5,0.5&� .  .  $��1������ *
Beklaagdes+	 �� *SelectedPersoon+   $ Beklaagde(s)�� �� *
Voornaam+� $��2��$��4�� �� �� �� .+��� $��Verwijderen��$��Rechtzaak verwijderen��
����  �$
��Auto��$��35,52,35.2,34.4q����� �� $��PartijBeheren��$��Partij beheren��
����  �$��1��$
��Auto��$��34.8,52,35.4,34.4q����� �� $��Bewaren��$��Bewaren��
����  �$��2��$
��Auto��$��34.6,52,35.6,34.4q����� 
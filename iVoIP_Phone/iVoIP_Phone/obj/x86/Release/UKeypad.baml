   M S B A M L   `   `   `  ���� I  ERibbonControl, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null\>clr-namespace:DNBSoft.WPF.RibbonControl;assembly=RibbonControlDNBSoft.WPF.RibbonControl   iVoIP_Phone   iVoIP_Phone.UKeypad   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QMahApps.Metro, Version=0.14.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  Gmy>clr-namespace:DNBSoft.WPF.RibbonControl;assembly=RibbonControl  .��� $	��188��$	��402��  � ColumnDefinitions  �� $��0* ��� $��0 ��� $��0* ��� $
��347* � � RowDefinitions �� $	��181 ��� $��0 ��� $��0 ��� $
��Auto ��� $��7* �.+�0   *DNBSoft.WPF.RibbonControl.RibbonHalfButton  -   $��ribbonHalfButton1��$
��Left=�$��
177,18,0,0q�$	��Top=�$��35��$��48��1   +DNBSoft.WPF.RibbonControl.RibbonControlBase   NormalImage$) #/iVoIP_Phone;component/Images/1.png��$��3��   Text$ 1��  -   $��ribbonHalfButton2��$��35��$
��Left=�$��
231,18,0,0q�$	��Top=�$��48��$��3��$) #/iVoIP_Phone;component/Images/2.png��$ 2��  -   $��ribbonHalfButton3��$��35��$
��Left=�$��
285,18,0,0q�$	��Top=�$��48��$��3��$) #/iVoIP_Phone;component/Images/3.png��$ 3��  -   $��ribbonHalfButton4��$��35��$��
177,59,0,0q�$	��Top=�$��3��$) #/iVoIP_Phone;component/Images/4.png��$ 4��$
��Left=�$��48��  -   $��ribbonHalfButton5��$��35��$
��Left=�$��
231,59,0,0q�$	��Top=�$��48��$��3��$) #/iVoIP_Phone;component/Images/5.png��$ 5��  -   $��ribbonHalfButton6��$��35��$��285,59,69,0q�$	��Top=�$��3��$) #/iVoIP_Phone;component/Images/6.png��$ 6��  -   $��ribbonHalfButton7��$��35��$
��Left=�$��177,100,0,0q�$	��Top=�$��48��$��3��$) #/iVoIP_Phone;component/Images/7.png��$ 7�� � DataContext ��   -   $��ribbonHalfButton8��$��35��$
��Left=�$��231,100,0,0q�$	��Top=�$��48��$��3��$) #/iVoIP_Phone;component/Images/8.png��$ 8��  -	   $��ribbonHalfButton9��$��35��$��
0,100,69,0q�$	��Top=�$��3��$��Right=�$��48��$) #/iVoIP_Phone;component/Images/9.png��$ 9��  -
   $��ribbonHalfButton10��$��35��$
��Left=�$	��Top=�$��48��$��177,141,0,0q�$��3��$ *��$W Q/iVoIP_Phone;component/Images/asterisk-blue-app-icons-vector-897150%20%281%29.jpg��  -   $��ribbonHalfButton11��$��35��$
��Left=�$��231,141,0,0q�$	��Top=�$��48��$��3��$) #/iVoIP_Phone;component/Images/0.png��$ 0��  -   $��ribbonHalfButton12��$��35��$
��Left=�$��285,141,0,0q�$	��Top=�$��48��$��3��$9 3/iVoIP_Phone;component/Images/hashtags-295x3001.jpg��$ #���� -   $��	comboBox1��*  $MahApps.Metro.Controls.TextboxHelper   	Watermark$ Enter # to Dial��$��23��$
��Left=�$��	12,27,0,0q�$	��Top=�$	��145��   MetroComboBox#���    �� 
IsEditable . $��3��  -   $��ribbonHalfButton13��$��3��$
��Left=�$��
339,18,0,0q�$	��Top=�$	��158��$��45��$3 -/iVoIP_Phone;component/Images/phone-18-32.png���� -   $��
textBlock5��$��3��$��104,71,245,87q�  MetroTextBlock#���  $��00:00:00�� �� TextAlignment$ Center=��� -   $��
textBlock1��$��3��$��23��$��104,100,245,58q�#���  $��00:00:00��$ Center=��� -   $��
textBlock2��$��3��$��23��$��104,129,245,29q�#���  $��00:00:00��$ Center=��� -   $��
textBlock3��$��3��$��23��$��12,71,317,87q�#���  $��Handle Time��$
 Left=��� -   $��
textBlock4��$��3��$��23��$��12,100,317,58q�#���  $��	Talk Time��$
 Left=��� -   $��
textBlock6��$��3��$��23��$��12,129,317,29q�#���  $��	Hold Time��$
 Left=�
   M S B A M L   `   `   `  ����I  ERibbonControl, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null\>clr-namespace:DNBSoft.WPF.RibbonControl;assembly=RibbonControlDNBSoft.WPF.RibbonControl  5       iVoIP_Phone   iVoIP_Phone.UKeypad   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      U QMahApps.Metro, Version=0.14.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  5      Gmy>clr-namespace:DNBSoft.WPF.RibbonControl;assembly=RibbonControl  5      .��5
      � $	��188��6   $	��402��6     � ColumnDefinitions  5   
   �� 5      $��0* �6   �� 5      $��0 �6   �� 5      $��0* �6   �� 5      $
��347* �6    � RowDefinitions 5   
   �� 5      $	��181 �6   �� 5      $��0 �6   �� 5      $��0 �6   �� 5      $
��Auto �6   �� 5      $��7* �6   .+�5   
   0   *DNBSoft.WPF.RibbonControl.RibbonHalfButton  -   5        $��ribbonHalfButton1��5   M   $
��Left=�6   $��
177,18,0,0q�69   $	��Top=�6f   $��35��6~   $��48��6�   1   +DNBSoft.WPF.RibbonControl.RibbonControlBase   NormalImage$) #/iVoIP_Phone;component/Images/1.png��6�   $��3��6�      Text$ 1��6�     5   
   -   5        $��ribbonHalfButton2��5   Y   $��35��6   $
��Left=�6*   $��
231,18,0,0q�6E   $	��Top=�6r   $��48��6�   $��3��6�   $) #/iVoIP_Phone;component/Images/2.png��6�   $ 2��6�   6�     5   
   -   5        $��ribbonHalfButton3��5   Y   $��35��6   $
��Left=�6*   $��
285,18,0,0q�6E   $	��Top=�6r   $��48��6�   $��3��6�   $) #/iVoIP_Phone;component/Images/3.png��6�   $ 3��6�   6�     5   
   -   5        $��ribbonHalfButton4��5   >   $��35��6   $��
177,59,0,0q�6*   $	��Top=�6W   $��3��6o   $) #/iVoIP_Phone;component/Images/4.png��6   $ 4��6�   $
��Left=�6�   $��48��6�     5   
   -   5        $��ribbonHalfButton5��5   Y   $��35��6   $
��Left=�6*   $��
231,59,0,0q�6E   $	��Top=�6r   $��48��6�   $��3��6�   $) #/iVoIP_Phone;component/Images/5.png��6�   $ 5��6�   6�     5   
   -   5        $��ribbonHalfButton6��5   ?   $��35��6   $��285,59,69,0q�6*   $	��Top=�6X   $��3��6p   $) #/iVoIP_Phone;component/Images/6.png��6�   $ 6��6�   6�     5   
   -   5        $��ribbonHalfButton7��5   Z   $��35��6   $
��Left=�6*   $��177,100,0,0q�6E   $	��Top=�6s   $��48��6�   $��3��6�   $) #/iVoIP_Phone;component/Images/7.png��6�   $ 7��6�    � DataContext 6�   �� 6�     5   
   -   5        $��ribbonHalfButton8��5   Z   $��35��6   $
��Left=�6*   $��231,100,0,0q�6E   $	��Top=�6s   $��48��6�   $��3��6�   $) #/iVoIP_Phone;component/Images/8.png��6�   $ 8��6�   6�     5    
   -	   5        $��ribbonHalfButton9��5    >   $��35��6   $��
0,100,69,0q�6*   $	��Top=�6W   $��3��6o   $��Right=�6   $��48��6�   $) #/iVoIP_Phone;component/Images/9.png��6�   $ 9��6�   6�     5!   
   -
   5        $��ribbonHalfButton10��5!   E   $��35��6   $
��Left=�6*   $	��Top=�6_   $��48��6w   $��177,141,0,0q�6�   $��3��6�   $ *��6�   $W Q/iVoIP_Phone;component/Images/asterisk-blue-app-icons-vector-897150%20%281%29.jpg��6�     5"   
   -   5        $��ribbonHalfButton11��5"   Z   $��35��6   $
��Left=�6*   $��231,141,0,0q�6E   $	��Top=�6t   $��48��6�   $��3��6�   $) #/iVoIP_Phone;component/Images/0.png��6�   $ 0��6�   6�     5#   
   -   5        $��ribbonHalfButton12��5#   Z   $��35��6   $
��Left=�6*   $��285,141,0,0q�6E   $	��Top=�6t   $��48��6�   $��3��6�   $9 3/iVoIP_Phone;component/Images/hashtags-295x3001.jpg��6�   $ #��6�   6�   �� 5$   
   -   5        $��	comboBox1��5$   �   *  $MahApps.Metro.Controls.TextboxHelper   	Watermark$ Enter # to Dial��6   $��23��6F   $
��Left=�6R   $��	12,27,0,0q�6m   $	��Top=�6�   $	��145��5%         MetroComboBox#���   6    �� 
IsEditable . $��3��6Y     5&   
   -   5        $��ribbonHalfButton13��5&   ]   $��3��6   $
��Left=�6.   $��
339,18,0,0q�6I   $	��Top=�6w   $	��158��6�   $��45��6�   $3 -/iVoIP_Phone;component/Images/phone-18-32.png��6�   6�   �� 5'   
   -   5        $��
textBlock5��5'   ;   $��3��6   $��104,71,245,87q�6$     MetroTextBlock#���  6M   $��00:00:00��6v    �� TextAlignment$ Center=�6�   �� 5(   
   -   5        $��
textBlock1��5(   H   $��3��6   $��23��6$   $��104,100,245,58q�60   #���  6Z   $��00:00:00��6�   $ Center=�6�   �� 5)   
   -   5        $��
textBlock2��5)   H   $��3��6   $��23��6$   $��104,129,245,29q�60   #���  6Z   $��00:00:00��6�   $ Center=�6�   �� 5*   
   -   5        $��
textBlock3��5*   F   $��3��6   $��23��6$   $��12,71,317,87q�60   #���  6X   $��Handle Time��6�   $
 Left=�6�   �� 5+   
   -   5        $��
textBlock4��5+   G   $��3��6   $��23��6$   $��12,100,317,58q�60   #���  6Y   $��	Talk Time��6�   $
 Left=�6�   �� 5,   
   -   5        $��
textBlock6��5,   G   $��3��6   $��23��6$   $��12,129,317,29q�60   #���  6Y   $��	Hold Time��6�   $
 Left=�6�   5-      5.      
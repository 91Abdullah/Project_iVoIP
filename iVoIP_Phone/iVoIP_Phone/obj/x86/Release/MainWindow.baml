   M S B A M L   `   `   `  ���� U  QMahApps.Metro, Version=0.14.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  F BiVoIP_Phone, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null)clr-namespace:iVoIP_PhoneiVoIP_Phone )clr-namespace:iVoIP_PhoneiVoIP_Phone I ERibbonControl, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null\>clr-namespace:DNBSoft.WPF.RibbonControl;assembly=RibbonControlDNBSoft.WPF.RibbonControl J FWPFDigitalClock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP9clr-namespace:DNBSoft.WPF.Clocks;assembly=WPFDigitalClockDNBSoft.WPF.Clocks J6clr-namespace:WPFDigitalClock;assembly=WPFDigitalClockWPFDigitalClock V RCustomControlLibrary, Version=1.0.6585.20943, Culture=neutral, PublicKeyToken=nullY@clr-namespace:CustomControlLibrary;assembly=CustomControlLibraryCustomControlLibrary    iVoIP_Phone.MainWindow   Hcontrols;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  #localclr-namespace:iVoIP_Phone  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     (myclr-namespace:iVoIP_Phone     Nmy1>clr-namespace:DNBSoft.WPF.RibbonControl;assembly=RibbonControl     Imy39clr-namespace:DNBSoft.WPF.Clocks;assembly=WPFDigitalClock     Fmy46clr-namespace:WPFDigitalClock;assembly=WPFDigitalClock     Pmy5@clr-namespace:CustomControlLibrary;assembly=CustomControlLibrary     -     � Title$  iVoIP Softphone��$	��446��$	��546��$��1q�   AccentColorBrush#���    � 
ResizeMode$ CanMinimize=� � Icon$2 ,/iVoIP_Phone;component/1407358855_111025.ico��.��� -   $��MainGrid��$	��530��$	��406��.+��� $��Center=�$��411,12,10,0q�$	��109��$	��110��$	��Top=� � 	Resources "  iVoIP_Phone.SecondsConverter  secondsConverter"  iVoIP_Phone.MinutesConverter  minutesConverter   iVoIP_Phone.HoursConverter  hoursConverter"  iVoIP_Phone.WeekdayConverter  weekdayConverter  AnalogClock �� 
TargetType   CustomControlLibrary.Clock �� Property �� Value �� 
TargetType � ColumnDefinitions	 � RowDefinitions
 � Color � Offset �� Path �� RelativeSource �� 	Converter
 �� Top �� Left �� CenterX �� CenterY �� Angle ��Name��%=  &	       &	      &	 
     &	      &	      7  7  7  7          �� "  .���  � �� �� "  .R��  �� $	��108 �	 �� $	��108 �.+�A� $	��108��$	��108��$��3��$��0��$��0������ ��� $
 	LightBlue��$ 0Q�� $
 DarkBlue��$ 1Q�A� $��Center=�$��Center=�$	��104��$	��104��
����ح�$��3������ ��� $
 DarkBlue��$ 0Q�� $
 	LightBlue��$ 1Q��� $��MonthDay��$��Center=�$��0,0,0,30q�$��Bottom=�$��10(����� $ DateTime� �� *TemplatedParent+8   �� $	��102��$	��102��.+�A� $��8��$��8��
���   �$ 46��$ 46���� $ 5��$ 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$ 0Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$ 30Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$ 60Q��� $ 5��$ 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$ 90Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 120Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 150Q��� $ 5��$ 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$	 180Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 210Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 240Q��� $ 5��$ 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$	 270Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 300Q��� $ 5��$ 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 330Q��� $ HourHand��$ 21��$ 48��
���   �$��4��$��30��z��� $ 2Q�$ 30Q� �� $ DateTime� �� *TemplatedParent+8  �� $ 
MinuteHand��$ 6��$ 49��
���   �$��2��$��45��z��� $ 1Q�$ 45Q� �� $ DateTime� �� *TemplatedParent+8  �� $ 
SecondHand��$ 4��$ 49��
���  ��$��1��$��47��z��� $	 0.5Q�$ 47Q� �� $ DateTime� �� *TemplatedParent+8  .+�  -   $��customControlAnalogClock��#��[ `� $��
0,10,133,0q�$��Right=�$	��408��$��36��$	��Top=�.�c� -   $��toolBar1��$��12,0,12,276q�$y�Visible=�$��LeftToRight=�$
��Left=�$��Center=����	#FFE23131.@��� -   $��btnHome�� � ToolTip$
 Home��  MetroCircleButtonStyle#���  $��30��$��30������  �� ImageSource$, &/iVoIP_Phone;component/Images/Home.png���� -   $��btnDial��$
 Dial��#���  $��30��$��30������ $- '/iVoIP_Phone;component/Images/dial1.png���� -   $��btnMsg��$ Message��#���  $��30��$��30������ $, &/iVoIP_Phone;component/Images/Mail.png���� -   $��btnSUpervise��$ Supervisor Panel��#���  $��30��$��30������ $3 -/iVoIP_Phone;component/Images/Businessman.png���� -	   $��btnBreak��$ Not Ready - Break��#���  $��30��$��30������ $- '/iVoIP_Phone;component/Images/break.png���� -
   $��btnLunch��$ Not Ready - Lunch��#���  $��30��$��30������ $- '/iVoIP_Phone;component/Images/lunch.png���� -   $��btnNamaz��$ Not Ready - Prayer��#���  $��30��$��30������ $/ )/iVoIP_Phone;component/Images/seccade.png���� -   $��btnTea��$ Not Ready - Tea��#���  $��30��$��30������ $+ %/iVoIP_Phone;component/Images/tea.png���� -   $��
btnMeeting��$ Not Ready - Meeting��#���  $��30��$��30������ $/ )/iVoIP_Phone;component/Images/Meeting.png���� -   $��	btnOthers��$ Not Ready - Others��#���  $��30��$��30������ $5 //iVoIP_Phone;component/Images/miscellaneous.png���� -   $��btnAbout��$ Call History��#���  $��30��$��30������ $, &/iVoIP_Phone;component/Images/more.png���� -   $��	btnLogout��$ Logout��#���  $��30��$��30������ $. (/iVoIP_Phone;component/Images/logout.png���� -   $��stackPanel1��$	��119��$
��Left=�$��411,161,0,0q�$	��Top=�$	��109��.+�2  ,DNBSoft.WPF.RibbonControl.RibbonDisplayImage  -   $��ribbonDisplayImage1��   NormalImage$- '/iVoIP_Phone;component/Images/cross.png���� -   $��
textBlock4��$��23��  MetroTextBlock#���  $��NotReady�� �� TextAlignment$ Center=�$��99��$��16(�$��Trebuchet MS)�$��SemiBold"��� -   $��
textBlock5��$��23��#���  $��00:00:00��$ Center=�$��99���� -   $��
statusBar1��$��23��$
��Left=�$��	0,371,0,0q�$	��Top=�$	��518��.@��� -   $��statusLabel��$��	Not Ready�����	#FFB1DCEF � DataContext �� � -   $��	ChildGrid��$	��176��$
��Left=�$��	12,91,0,0q�$	��Top=�$	��393��� -   $��CallButtonsGrid��$��69��$
��Left=�$��	12,0,0,51q�$��Bottom=�$	��393���� $��Right=�$	��109��$��0,311,10,41q�.@��� .����  �� Orientation$ 
Horizontal=�.+��� $��57��Dialed�� -   $��	DialCalls��$��50��0�� .���� $ 
Horizontal=�.+��� $��57��
Recieved�� -   $��RcvCalls��$��51��0�� -   $��
textBlock6��$��22��$
��Left=�$��411,286,0,0q�$��Total Calls��$	��Top=�$	��108��$ Center=�$
��Bold"��� -   $
��Date��$
��Bold"�$��22��$��412,133,0,0q�$��Total Calls��$ Center=�$	��Top=�$
��Left=�$	��108���� -   $��border1��
�������$��1q�$	��100��$
��Left=�$��
732,89,0,0q�$	��Top=�$	��200���� -   $��
textBlock1��$��31��$��	TextBlock��$ Center=�#���  $
��Bold"�$��Normal$�$��Normal&�$��NoWrap=�$��Center=�$��12,54,344,321q��� -   $��
textBlock2��$��31��$��	TextBlock��$ Center=�#���  $
��Bold"�$��Normal$�$��Normal&�$��NoWrap=�$��Center=�$��192,54,267,321q��� -    $��
textBlock3��$��32��$��	TextBlock��$ Center=�#���  $
��Bold"�$��Normal$�$��Normal&�$��NoWrap=�$��Center=�$��269,53,125,321q�
   M S B A M L   `   `   `  ���� G  CModernUIDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:FirstFloor.ModernUI.AppFirstFloor.ModernUI.App  4    .FirstFloor.ModernUI.App.Pages.Users.Vendedores   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     N JFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null0mui&http://firstfloorsoftware.com/ModernUI  /app%clr-namespace:FirstFloor.ModernUI.App    � 	Resources   '  #ItemCollectionViewSourceAllProducto �� CollectionViewType + '                               Imprimir & "ItemCollectionViewSourceAllCliente ' #ItemCollectionViewSourceAllVendedor $  ItemCollectionViewSourceAllVenta��%d   &	        &	 
     &	      &	      &	 (     �� " ���� " ���� " ���� " ���� " ��.���  � RowDefinitions �� $	��40* ��� $
��279* ��� $	��42* ��� $��0* � � ColumnDefinitions �� $
��700* ��� $��35.375 ��� $
��156* ��� $
��171* ��� $��6 �.+��� $��Detalle Vendedor��  Heading1#��[ $��10,0,1,0q�$��2���� $��Añadir Vendedor��#��[ $��0q�$��2��$	��187��$��40��$��2���� $��Nombre��$��	10,70,0,0q�$	��Top=�$��16(�$��2��$��1��$
��Left=�$��21��$��58���� -   $��lfechaingreso��$��Fecha Ingreso��$��
10,104,0,0q�$	��Top=�$��16(�$��2��$��1��$
��Left=�$��21��$��98���� $	��RUT��$��	11,31,0,0q�$	��Top=�$��16(�$��2��$��1��$
��Left=�$��21��$��29��&   System.Windows.Controls.DataGrid  -     Name$ datagridVendedor��$��10q�   GridLinesVisibility$	 All=�   HorizontalGridLinesBrush �	#FFCCCCCC   VerticalGridLinesBrush �	#FFCCCCCC$��1�����	#00000000   MinColumnWidth$	 100Q�	 � IsSharedSizeScope	 . 
 � UseLayoutRounding
 .  ��. $��0q� � DataContext# [    AutoGenerateColumns .     CanUserAddRows .    
IsReadOnly . ���	#FF333333����    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$	 Rut��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *rut+  $ Nombre�� �� *nombre+  $ Ingreso a Trabajar�� �� *fechaIngresoTrabajar+ �� StringFormat$ {0:dd/MM/yyyy}��  $ Ultimo Acceso�� �� *fechaUltimoAcceso+$ {0:dd/MM/yyyy}��  $ Total Ventas�� �� *totalVentas+  $
 Tipo�� �� *
contrasena+�� -   $��txtNombreVendedor��$��1,65,1,0q� �� TextWrapping$
 Wrap=�$��3��$��1��$	��Top=�$	��150��$��23��
�������
���   �$	��150��$��23��(  "System.Windows.Controls.DatePicker  -   $��txtfechaIngreso��$��3��$��	1,103,0,0q�$��1��$	��Top=�$
��Left=�$	��270��$��Center=�$��1,2,0,1q�   Text$	 NaN��$��27����Z� ��Z� �� -   $��txtrutvendedor��$��1,26,0,0q�$
 Wrap=�$	��Top=�$��3��$��1��$	��150��$��23��
�������
���   �$	��150��$��23���� -    ��Name$ btnguardarVendedor��$��Guardar��$��	10,5,12,0q�$	��Top=�$��16(�$��3��$��2��$��23��$	��120��
����������	#FF009688$
��Bold"�$��31����Z� �� -   $ btnActualizarVendedor��$��Editar��$��10,5,0,0q�$��16(�$��2��$��23��$	��120��
����������	#FFFF1744$
��Bold"�$��Center=�$��Center=�$��	16,3,16,5q�$	��Top=�$
��Left=�$��31��$	��120����Z� �� -   $ btnEliminarVendedor��$��Borrar��$��	144,5,0,0q�$��16(�$��2��$��23��$	��120��
����������	#FFFF1744$
��Bold"�$	��Top=�$
��Left=�$��31��$	��120����Z� �� -	   $ btncancelarEditarVendedor��$��Cancelar��$��45,204,10,0q�$	��Top=�$��16(�$��1��$��23��$��35��$	��120��
����������	#FFFF1744$
��Bold"�$��2��$y�Hidden=���Z� �� -
   $" btnGuardarActualizarVendedor��$��
Actualizar��$��10,204,12,0q�$	��Top=�$��16(�$��3��$��1��$��23��$��35��$	��120��
����������	#FF009688$
��Bold"�$y�Hidden=���Z� 	� -   $��rdbtnvendedor��$��Vendedor��$��3��$
��Left=�$��
14,135,0,0q�$��1��$	��Top=� d� 	IsChecked$
 TrueY�$��16(�$��21��$��88��	� -   $��
rdbtnAdmin��$��Administrador��$��3��$
��Left=�$��
14,157,0,0q�$��1��$	��Top=�
�������$��16(�$��21��$	��121���� -   $��lcontraseña��$��Contraseña��$��
10,104,0,0q�$	��Top=�$��16(�$��2��$��1��$��21��$
��Left=�$y�Hidden=�$��80��G� -    G�Name$ txtContraseña��$��3��$��	1,104,1,0q�$��1��$	��Top=� J� RenderTransformOrigin$ 0.5,0.5&�$y�Hidden=�$	��120��$	��150��$��23��
�������
���   �$��23���� -   $ btnRecargar��$��Recargar��$��	274,5,0,0q�$��16(�$��2��$��23��$	��120��
����������	#FFFF1744$
��Bold"�$	��Top=�$
��Left=�$��31��$	��120����Z� �� $��Obs:Sin puntos ni guión��$��3��$��	1,49,20,0q�$��1��$	��Top=�$��10(�
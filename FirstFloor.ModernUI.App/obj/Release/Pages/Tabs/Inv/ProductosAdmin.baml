   M S B A M L   `   `   `  ����   ModernUIDemo;    5FirstFloor.ModernUI.App.Pages.Tabs.Inv.ProductosAdmin   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     N JFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null0mui&http://firstfloorsoftware.com/ModernUI    � 	Resources   '  #ItemCollectionViewSourceAllProducto �� CollectionViewType / +ItemCollectionViewSourceAllProductoImprimir & "ItemCollectionViewSourceAllCliente ' #ItemCollectionViewSourceAllVendedor $  ItemCollectionViewSourceAllVenta��%d   &	        &	 
     &	      &	      &	 (     �� " ���� " ���� " ���� " ���� " ��.���  � ColumnDefinitions �� $
��180* ��� $
��335* ��� $
��196* ��� $
��125* ��� $
��Auto ��� $��0* � � RowDefinitions �� $��40 ��� $
��194* ��� $
��235* ��� $��25 ��� $��0* �.+��� $��Nuevo Producto��  Heading1#��[ $��10,0,0,0q�$��2��$	��295��$��40��$	��295��$��40��$
��Left=�$	��295���� -   $��cbTipoCodigo��$
��Left=�$	��Top=�$��	13,11,0,0q�$��13(�
�������$��1��$��1��
���   �$��Center=����	#FF333333$	��150��$��23�� �� SelectedIndex$ 0��.@��� $��Sin Código�����	#FF333333
�������$��Center=��� $��Con Código�����	#FF333333
�������$��Center=��� $��Codigo Producto��$��	10,54,0,0q�$	��Top=�$��16(�$��1��$
��Left=�$��21��$	��120���� -   $��
txtcodprod��$
��Left=�$��	13,52,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��14(�
�������$��1��$��1��
���   ����	#FF333333$��Center=�$	��150��$��23��{�.  $��23���� $��Nombre��$��	10,98,0,0q�$	��Top=�$��16(�$��1��$
��Left=�$��21��$��58���� -   $��txtnombreproducto��$
��Left=�$��	13,96,0,0q�$
 Wrap=�$	��Top=�$��14(�
�������$��1��$��1��
���   ����	#FF333333$��Center=�$	��150��$��23��$��23���� $��
Categoría��$��
10,135,0,0q�$��16(�$��1��$	��Top=�$
��Left=�$��21��$��68���� $��Stock��$
��Left=�$��
200,54,0,0q�$	��Top=�$��16(�$��1��$��1��$��21��$��39���� -   $��txtstock��$
��Left=�$��23��$��65.5,52,0,0q�$
 Wrap=�$	��Top=�$��14(�
�������$��1��$��2��
���   ����	#FF333333$��Center=�$	��150��$��23��$��2��$	��120���� $��Precio Compra ��$
��Left=�$��
200,96,0,0q�$	��Top=�$��16(�$��1��$��1��$��21��$	��133���� -    ��Name$ txtprecioReal��$
��Left=�$��23��$��65.5,96,0,0q�$
 Wrap=�$	��Top=�$��14(�
�������$��1��$��2��
���   ����	#FF333333$��Center=� J� RenderTransformOrigin$ 
0.55,2.217&�$	��150��$��23��$��2��$	��120���� -    ��Name$ btnGuardarProducto��$��Guardar��$
��Left=�$��132.5,162,0,0q�$��1��$��2�����	#FF009688
�������$
��Bold"�$	��Top=�$��Center=�$��26��$��83��$��2����Z� R� -   	 R�Name$	 treeViewCategoria��$��16(�$��2��$��0,62,5,0q����	#FF333333
�������
 � ForceCursor
 . ����  �� 
TargetType" Q�.���  �� Property �@Q�
IsExpanded �� Value . &   System.Windows.Controls.DataGrid  -     Name$ datagridProducto�� � DataContext# [     AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    HorizontalGridLinesBrush �	#FFCCCCCC   VerticalGridLinesBrush �	#FFCCCCCC$��2�����	#00000000���	#FF333333���	#FF688CAF$��14(�$��2��$��1��$��	0,36,10,0q�$	��199��   MinColumnWidth$	 150Q�   GridLinesVisibility$	 All=�����    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Id��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
idProducto+  $ Nombre��$	 100  �� *nombre+  $ Stock��$ 50  �� *stock+  $ Precio Real��$ 80  �� *
precioReal+  $ Precio Venta��$ 80  �� *precio+  $ Fecha Modificación��$	 120  �� *fecha+ �� StringFormat$ {0:dd/MM/yyyy}���� -	   $ btnEliminarProducto��$��Borrar��$��	5,68,20,0q�$��2��$��3�����	#FFFF1744
�������$��Center=�$
��Bold"�$	��Top=�$��80��$��25����Z� �� -
   $ btnEditarProducto��$��Editar��$��	5,37,20,0q�$��2��$��3�����	#FFFFD740
�������$��Center=�$
��Bold"�$	��Top=���Z� �� -   $ txtNuevaCat��$
��Left=�$��	10,10,0,0q�$
 Wrap=�$	��Top=�$��14(�
�������$��2��
���   ����	#FF333333$��Center=�$	��150��$��23���� -   $ btnNuevaCategoria��$��Nueva��$��10,36,100,0q�$��2�����	#FF2196F3
�������$
��Bold"�$	��Top=���Z� �� $��10,193,25,0q�$��1��$��4��$��Stretch=�$��Stretch=��� -   $ btnEliminarCategoria��$��Borrar��$��	100,0,0,0q�$��3�����	#FFFF1744
�������$
��Left=�$��Center=�$
��Bold"�$��80��$��25��$��Bottom=���Z� �� -   $ btnEditarCategoria��$��Editar��$��13,0,0,0q�$��3�����	#FFFFD740
�������$
��Left=�$��Center=�$
��Bold"�$��25��$��Bottom=���Z� �� $��Precio Venta ��$
��Left=�$��200,135,0,0q�$	��Top=�$��16(�$��1��$��1��$��21��$	��133���� -   $��	txtprecio��$
��Left=�$��23��$��65.5,133,0,0q�$
 Wrap=�$	��Top=�$��14(�
�������$��1��$��2��
���   ����	#FF333333$��Center=�$ 
0.55,2.217&�$	��150��$��23��$��2��$	��120���� -   $ btnEliminarTodoProducto��$��Borrar todo��$��	5,99,20,0q�$��2��$��3�����	#FFFF1744
�������$��Center=�$
��Bold"�$��80��$��25��$	��Top=���Z� �� -   $ btncancelarEditarcat��$��Cancelar��$��	52,36,0,0q�$	��Top=�$��2�����	#FF2196F3
�������$
��Bold"�$y�Hidden=�$��85��$��25��$��Right=���Z� �� -   $ btnGuardaActProducto��$��
Actualizar��$��	10,36,0,0q�$	��Top=�$��2�����	#FF2196F3
�������$
��Bold"�$y�Hidden=�$	��100��$��25��$	��Top=�$
��Left=���Z� �� -   $" btnGuardarActualizarProducto��$��
Actualizar��$��159.5,167,61,0q�$��1��$��2�����	#FF009688
�������$
��Bold"�$	��Top=�$	��Top=�$��2��$y�Hidden=�$	��100��$��25��$	��100��$��25����Z� �� -   $ btncancelarEditarProd��$��Cancelar��$��47.5,167,48,0q�$��1��$��2�����	#FF009688
�������$
��Bold"�$	��Top=�$	��Top=�$y�Hidden=�$	��100��$��25��$	��100��$��25����Z� �� $��Buscar producto��$��0,5,0,0q�$��14(�$��2��$	��Top=�$
��Left=�$��1���� -   $��txtbuscarProducto��$��1��$
��Left=�$��	122,4,0,0q�$��2��$
 Wrap=�$�� ��$	��Top=�$��2��
�������$	��217��$��Center=�
���   ��� -   $��cbCategoria��$ -1��$��1��$
��Left=�$��
13,134,0,0q�$��1��$	��150�����	#FF333333
�������
���   �$��22��$	��Top=�.@��� $��ComboBoxItem�����	#FF333333$��Center=�$��0q�
����������	#FF333333�� -    ��Name$ ladmin��$�� ��$��1��$
��Left=�$��
276,14,0,0q�$	��Top=�$��Right=����#E55300$��3��$	��278���� -   $ btnSalir��$��Salir��$��0q�$��3�����	#FFFF1744
�������$
��Bold"�$��Center=�$	��Top=�$��Center=���Z� 
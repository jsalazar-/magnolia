﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FirstFloor.ModernUI.App.Pages.Users
{
    /// <summary>
    /// Interaction logic for ControlPass.xaml
    /// </summary>
    public partial class ControlPass : Window
    {
        string pass = "";
        public ControlPass()
        {
            InitializeComponent();
        }

        public void setpass(string p)
        {
            pass = p;
        }

        public string getpass()
        {
            return pass.Trim();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpass.Password))
            {
                MessageBox.Show("Ingresar contraseña", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                setpass(txtpass.Password);
                this.Close();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Win8Client
{
    public sealed partial class CompactyStrategyView : UserControl
    {
        public CompactyStrategyView()
        {
            this.InitializeComponent();            
        }

        internal AppDataContext appDataContext
        {
            get
            {
                return this.AppDataContext;
            }
        }

        public AppDataContext AppDataContext
        {
            get
            {
                return (AppDataContext)(base.DataContext);
            }

            set
            {
                this.DataContext = value;
            }
        }

        private void StrategyButton_Click(object sender, RoutedEventArgs e)
        {
            this.appDataContext.ViewStrategiesFullScreen();
        }

        private void ClearStrategy1ButtonClick(object sender, RoutedEventArgs e)
        {
            this.appDataContext.player1Strategy.Clear();
            this.appDataContext.UpdateSimulationStep();
        }

        private void ClearStrategy2ButtonClick(object sender, RoutedEventArgs e)
        {
            this.appDataContext.player2Strategy.Clear();
            this.appDataContext.UpdateSimulationStep();
        }
    }   
}
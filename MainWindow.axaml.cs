using Avalonia.Controls;
using Avalonia.Media;
using DemoTab.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static DemoTab.Helper;

namespace DemoTab
{
    public partial class MainWindow : Window
    {
        private List<Client> _clients = new List<Client>(Helper.DBContext.Clients.Include(x => x.IdGenderNavigation).OrderBy(x => x.Id).ToList()); 
        public MainWindow()
        {
            InitializeComponent();
            lbox_clients.ItemsSource = _clients.ToList();
        }

        private void Panel_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            Client client = lbox_clients.SelectedItem as Client;
            AddWindow addWindow = new(client);
            addWindow.Show();
            Close();
        }

        private void AddClient(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddWindow addWindow = new();
            addWindow.Show();
            Close();
        }
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DemoTab.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DemoTab;

public partial class AddWindow : Window
{
    private Client _ClientDummy;
    //private Client _ClientConfirm;
    private string _PhotoPath = null;
    private string _PhotoDel = null;
    public AddWindow()
    {
        _ClientDummy = new Client();
        InitializeComponent();
        calendar.SelectedDate = DateTime.Now;
        grid_addwindow.DataContext = _ClientDummy;
        tblock_adddate.Text = DateOnly.FromDateTime(DateTime.Now).ToString();
        tblock_header.Text = "Создание клиента";
        btn_confirm.Content = "Добавить";
    }

    public AddWindow(Client client)
    {
        _ClientDummy = client;
        InitializeComponent();
        calendar.SelectedDate = _ClientDummy.Birthday.ToDateTime(TimeOnly.MinValue);
        grid_addwindow.DataContext = _ClientDummy;
        tblock_header.Text = "Создание клиента";
        btn_confirm.Content = "Подтвердить";
        btn_delete.IsVisible = true;
        if (_ClientDummy.Photo != null)
        {
            img_preview.Source = new Bitmap($"Assets/{_ClientDummy.Photo}");
            tblock_preview.Text = _ClientDummy.Photo;
            btn_delImage.IsVisible = true;
        }
    }

    private void Return(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();
        Close();
    }

    private void Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(tbox_firstname.Text != null && tbox_firstname.Text != "" &&
            tbox_name.Text != null && tbox_name.Text != "" &&
            tbox_lastname.Text != null && tbox_lastname.Text != "")
        {
            if (_ClientDummy.Id != 0)
            {
                //_ClientConfirm = _ClientDummy;
                _ClientDummy.IdGender = tswitch_gender.IsChecked == true ? 1 : 2 ;
                _ClientDummy.Birthday = DateOnly.FromDateTime(calendar.SelectedDate.Value);
                if (_PhotoPath != null)
                {
                    if (_ClientDummy.Photo != null)
                        File.Delete($"Assets/{_ClientDummy.Photo}"); 
                    _ClientDummy.Photo = Path.GetFileName(_PhotoPath);
                    File.Copy(_PhotoPath, $"Assets/{_ClientDummy.Photo}");
                }
                if (_PhotoDel != null)
                {
                    File.Delete($"Assets/{_PhotoDel}");
                    _ClientDummy.Photo = null;
                }
                Helper.DBContext.SaveChanges();
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();
            }
            else
            {
                _ClientDummy.Registration = DateOnly.FromDateTime(DateTime.Now);
                _ClientDummy.IdGender = tswitch_gender.IsChecked == true ? 1 : 2;
                _ClientDummy.Birthday = DateOnly.FromDateTime(calendar.SelectedDate.Value);
                if (_PhotoPath != null)
                {
                    _ClientDummy.Photo = Path.GetFileName(_PhotoPath);
                    File.Copy(_PhotoPath, $"Assets/{_ClientDummy.Photo}");
                }
                if (_PhotoDel != null)
                {
                    File.Delete($"Assets/{_PhotoDel}");
                    _ClientDummy.Photo = null;
                }
                Helper.DBContext.Clients.Add(_ClientDummy);
                Helper.DBContext.SaveChanges();
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();
            }
        }
    }

    private void Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Helper.DBContext.Clients.Remove(_ClientDummy);
        Helper.DBContext.SaveChanges();
        MainWindow mainWindow = new();
        mainWindow.Show();
        Close();
    }

    private readonly FileDialogFilter filter = new()
    {
        Extensions = new List<string>() { "jpg", "png" },
        Name = "Файлы изображений"
    };

    private async void AddImage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filters.Add(filter);

        string[] result = await openFileDialog.ShowAsync(this);
        if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
            return;

        _PhotoPath = result[0];
        string photoName = System.IO.Path.GetFileName(_PhotoPath);

        img_preview.Source = new Bitmap(_PhotoPath);
        tblock_preview.Text = photoName;
        btn_delImage.IsVisible = true;
    }

    private void DelImage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_ClientDummy.Photo != null)
        {
            _PhotoDel = _ClientDummy.Photo;
        }
        img_preview.Source = null;
        tblock_preview.Text = "";
        btn_delImage.IsVisible = false;
    }
}
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using Practica_Lamaev.Models;

namespace Practica_Lamaev;

public partial class Authorization : Window
{
    private List<user> users;

    public Authorization()
    {
        InitializeComponent();
        users = new List<user>();
        users.Add(new user { UserName = "admin", Password = "admin" });
    }

    private void LoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string username = Login.Text;
        string password = Password.Text;
        // Проверка введеных учетных данных
        bool yesno = false;
        foreach (user user in users)
        {
            if (user.UserName == username && user.Password == password)
            {
                yesno = true;
                break;
            }
        }

        if (yesno)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Успех", "Успешная авторизация");
            box.ShowAsync();
            new window().Show();
            this.Close();
        }
        else
        {
            var box1 = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ошибка авторизации");
            box1.ShowAsync();
        }
    }
}
﻿using System.Windows;
using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class LanZhou : Window
{
    public LanZhou()
    {
        InitializeComponent();
    }
    public string CardType { set; get; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if(CardType is null || CardType == "")
        {
            Console.WriteLine(CardType);
            MessageBox.Show("请先选择卡类型");
            return;
        }
        Close();
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LuZhouCardType.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
        {
            CardType = selectedItem.DataContext.ToString();
        }
    }
}
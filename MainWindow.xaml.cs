﻿using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;

namespace ReFileNameHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "文件夹|*.none";
            dialog.FileName = "请选择文件夹";
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;
            dialog.Title = "请选择要重命名的文件夹";
            dialog.ValidateNames = false;
            if (dialog.ShowDialog() == true)
            {
                var folderPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                this.FolderPathTextBox.Text = folderPath;
            }
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.FolderPathTextBox.Text))
            {
                MessageBox.Show("请先选择文件夹路径。");
                return;
            }

            if (string.IsNullOrEmpty(this.FileNamePrefixTextBox.Text))
            {
                MessageBox.Show("请输入文件名前缀。");
                return;
            }

            if (!int.TryParse(this.StartNumberTextBox.Text, out int startNumber))
            {
                MessageBox.Show("请输入文件名起始编号。");
                return;
            }

            var directory = new DirectoryInfo(this.FolderPathTextBox.Text);
            var files = directory.GetFiles().OrderBy(f => f.CreationTimeUtc);
            int index = startNumber;

            foreach (var file in files)
            {
                string extension = System.IO.Path.GetExtension(file.FullName);
                string newName = string.Format("{0}_{1:D3}{2}", this.FileNamePrefixTextBox.Text, index, extension);
                string newPath = System.IO.Path.Combine(directory.FullName, newName);
                file.MoveTo(newPath);
                index++;
            }

            MessageBox.Show("文件重命名完成。");
        }
    }

}

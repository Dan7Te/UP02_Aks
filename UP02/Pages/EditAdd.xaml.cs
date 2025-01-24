using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP02.API;

namespace UP02.Pages {
    /// <summary>
    /// Логика взаимодействия для EditAdd.xaml
    /// </summary>
    public partial class EditAdd :Page {
        Users CurrentUser;
        Advertisment CurrentAdd = null;
        Connector connector = new Connector();

        public EditAdd(Users User, Advertisment Add = null) {
            InitializeComponent();
            CurrentUser = User;
            if(Add == null) {
                btnSave.Content   = "Создать";
                btnDelete.Visibility = Visibility.Hidden;
            }
            else {
                CurrentAdd = Add;
                btnDelete.Visibility = Visibility.Visible;
                btnSave.Content   = "Обновить";

                cbStatus.SelectedValue   = CurrentAdd.Statuses;
                cbCity.SelectedValue     = CurrentAdd.City1;
                cbCategory.SelectedValue = CurrentAdd.Category1;
                cbType.SelectedValue     = CurrentAdd.AdType1;
                tbName.Text              = CurrentAdd.AdName;
                tbDescription.Text       = CurrentAdd.AdDescription;
                tbPrice.Text             = CurrentAdd.Price.ToString();
                tbPhoto.Text             = CurrentAdd.Photo;
            }

            cbCategory.ItemsSource = connector.GetCategoryList();
            cbCity.ItemsSource     = connector.GetCityList();
            cbType.ItemsSource     = connector.GetTypeList();
            cbStatus.ItemsSource   = connector.GetStatusList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            

            try {
                Entities.GetContext().Advertisment.Remove(CurrentAdd);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Объявление успешно удалено");
                NavigationService.Navigate(new UserAdds(CurrentUser));
            }
            catch {
                MessageBox.Show("Произошла ошибка удаления");
            }
            //using(var db = new Entities()) {
            //    try {
            //        db.Advertisment.Remove(CurrentAdd);
            //        db.SaveChanges();
            //        MessageBox.Show("Объявление успешно удалено");

            //        //Entities.GetContext().Advertisment.Remove(CurrentAdd);

            //        NavigationService.Navigate(new UserAdds(CurrentUser));
            //    }
            //    catch {
            //        MessageBox.Show("Произошла ошибка удаления");
            //    }
            //}
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            if(
                cbCity.SelectedIndex     == -1 &&
                cbCategory.SelectedIndex == -1 &&
                cbType.SelectedIndex     == -1 &&
                string.IsNullOrEmpty(tbName.Text) &&
                string.IsNullOrEmpty(tbDescription.Text) && 
                string.IsNullOrEmpty(tbPrice.Text)
                ) {
                MessageBox.Show("Не все поля заполнены");

                return;
            }
            if(
                !decimal.TryParse(tbPrice.Text, out _)
                ) {
                MessageBox.Show("Цена должна быть указана числом!!!");
                return;
            }

            if(CurrentAdd == null) CurrentAdd = new Advertisment();

            CurrentAdd.AdDate        = DateTime.Now;
            CurrentAdd.AdOwner       = CurrentUser.Id;
            CurrentAdd.City          = ((City)cbCity.SelectedValue).Id;
            CurrentAdd.Category      = ((Category)cbCategory.SelectedValue).Id;
            CurrentAdd.AdType        = ((AdType)cbType.SelectedValue).Id;
            CurrentAdd.AdStatus      = ((Statuses)cbStatus.SelectedValue).Id;
            CurrentAdd.AdName        = tbName.Text;
            CurrentAdd.AdDescription = tbDescription.Text;
            CurrentAdd.Price         = decimal.Parse(tbPrice.Text);
            CurrentAdd.Photo         = tbPhoto.Text == "" ? null : tbPhoto.Text;

           
            //using(var db = new Entities()) {
            //    try {
            //        db.Advertisment.AddOrUpdate(CurrentAdd);
            //        Entities.GetContext().Advertisment.AddOrUpdate(CurrentAdd);
            //        MessageBox.Show("Объявление успешно добавлено/обновлено");
            //        db.SaveChanges();

            //        NavigationService.Navigate(new UserAdds(CurrentUser));
            //    }
            //    catch {
            //        MessageBox.Show("Произошла ошибка добавления/обновления");
            //    }
            //}

            try {
                Entities.GetContext().Advertisment.AddOrUpdate(CurrentAdd);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Объявление успешно добавлено/обновлено");
                NavigationService.Navigate(new UserAdds(CurrentUser));
            }
            catch {
                MessageBox.Show("Произошла ошибка добавления/обновления");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new UserAdds(CurrentUser));
        }
    }
}

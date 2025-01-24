using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP02.API;

namespace UP02.Pages {
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage :Page {
        Connector connector = new Connector();
        Users CurrentUser = null;

        public MainPage(Users user = null) {
            InitializeComponent();
            
            lvAdds.ItemsSource = connector.GetAddList();

            cbCategory.ItemsSource = connector.GetCategoryList();
            cbCity.ItemsSource = connector.GetCityList();
            cbStatus.ItemsSource = connector.GetStatusList();
            cbType.ItemsSource = connector.GetTypeList();

            //пример работы функции подсчета профита
            //double i = connector.CountProfit(Entities.GetContext().Users.ToList()[5]);
            //var a = 212;

            if(user != null) {
                CurrentUser = user;
                btnAuth.Content = $"{user.UserName} (перезайти)";
                btnMyAdds.IsEnabled = true;

            }
        }

        private void cbCity_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Update();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Update();
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Update();
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Update();
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e) {
            Update();
        }

        public void Update() {
            int City     = cbCity.SelectedIndex == -1 ? -1 : ((UP02.API.City)cbCity.SelectedValue).Id;
            int Category = cbCategory.SelectedIndex == -1 ? -1 : ((UP02.API.Category)cbCategory.SelectedValue).Id;
            int AdType   = cbType.SelectedIndex == -1 ? -1 : ((UP02.API.AdType)cbType.SelectedValue).Id;
            int Status   = cbStatus.SelectedIndex == -1 ? -1 : ((UP02.API.Statuses)cbStatus.SelectedValue).Id;
            string Name = string.IsNullOrEmpty(tbName.Text) ? "" : tbName.Text;

            lvAdds.ItemsSource = connector.GetSortedAddList(Status, City, Category, AdType, Name);

        }

        private void btnClear_Click(object sender, RoutedEventArgs e) {
            lvAdds.ItemsSource = connector.GetAddList();

            cbCategory.SelectedItem = null;
            cbCity.SelectedItem = null;
            cbStatus.SelectedItem = null;
            cbType.SelectedItem = null;
            tbName.Text = "";
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new AuthPage());
        }

        private void btnMyAdds_Click(object sender, RoutedEventArgs e) {
            if(CurrentUser == null) {
                MessageBox.Show("Вы не авторизованы");
            }
            else {
                NavigationService.Navigate(new UserAdds(CurrentUser));
            }
        }
    }
}

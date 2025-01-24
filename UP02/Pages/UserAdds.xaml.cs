using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP02.API;

namespace UP02.Pages {
    /// <summary>
    /// Логика взаимодействия для UserAdds.xaml
    /// </summary>
    public partial class UserAdds :Page {

        Connector connector = new Connector();
        Users CurrentUser;

        public UserAdds(Users user) {
            InitializeComponent();
            CurrentUser = user;

            lvAdds.ItemsSource = connector.GetSortedAddList(-1,-1,-1,-1,"",CurrentUser);
            lvAdds.MouseDoubleClick += lviDoubleClick;
        }

        private void btnAddAdd_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new EditAdd(CurrentUser));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new MainPage(CurrentUser));
        }

        public void lviDoubleClick(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new EditAdd(CurrentUser, (Advertisment)lvAdds.SelectedValue));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            try {
                Entities.GetContext().Advertisment.Remove((Advertisment)lvAdds.SelectedValue);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Объявление успешно удалено");
                NavigationService.Navigate(new UserAdds(CurrentUser));
            }
            catch {
                MessageBox.Show("Произошла ошибка удаления");
            }
        }
    }
}

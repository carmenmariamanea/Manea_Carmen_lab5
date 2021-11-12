using AutoLotModel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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

namespace Manea_Carmen_lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        private CollectionViewSource customerVSource;

        private ActionState Action { get => Action1; set => Action1 = value; }
        public AutoLotEntitiesModel Ctx { get => Ctx1; set => Ctx1 = value; }
        public CollectionViewSource CustomerVSource { get => CustomerVSource1; set => CustomerVSource1 = value; }
        private ActionState Action1 { get => action; set => action = value; }
        public AutoLotEntitiesModel Ctx1 { get => ctx; set => ctx = value; }
        public CollectionViewSource CustomerVSource1 { get => customerVSource; set => customerVSource = value; }

        enum ActionState
        {
            New,
            Edit,
            Delete,
            Nothing
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            CustomerVSource1 =
 ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            CustomerVSource1.Source = Ctx1.Customers.Local;
            Ctx1.Customers.Load();
        }

        private static object GetDebuggerDisplay()
        {
            throw new NotImplementedException();
        }
    }
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        object action = ActionState.Add;
    }
    private void btnEditO_Click(object sender, RoutedEventArgs e)
    {
        Action Action = ActionState.Edit;
    }
    private void btnDeleteO_Click(object sender, RoutedEventArgs e)
    {
        object action = ActionState.Delete;
    }
    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
        object customerVSource = null;
        object p = customerVSource.View.MoveCurrentToNext();
    }
    private void btnPrevious_Click(object sender, RoutedEventArgs e)
    {
        object customerVSource = null;
        customerVSource.View.MoveCurrentToPrevious();
    }
    private void SaveCustomers()
 {
 Customer customer = null;
        object action = null;
        if (action == ActionState.New)
 {
 try
 {
 //instantiem Customer entity
 customer = new Customer()
 {
 FirstName = firstNameTextBox.Text.Trim(),
 LastName = lastNameTextBox.Text.Trim()
 };
//adaugam entitatea nou creata in context
ctx.Customers.Add(customer);
customerVSource.View.Refresh();
 //salvam modificarile
 ctx.SaveChanges();
 }
 //using System.Data;
 catch (DataException ex)
 {
 MessageBox.Show(ex.Message);
 }
 }
 else
if (action == ActionState.Edit)
 {
 try
 {
 customer = (Customer)customerDataGrid.SelectedItem;
 customer.FirstName = firstNameTextBox.Text.Trim();
 customer.LastName = lastNameTextBox.Text.Trim();
 //salvam modificarile
ctx.SaveChanges();
 }
 catch (DataException ex)
 {
 MessageBox.Show(ex.Message);
 }
 }
        else if (action == ActionState.Delete)
 {
 try
 {
 customer = (Customer)customerDataGrid.SelectedItem;
 ctx.Customers.Remove(customer);
 ctx.SaveChanges();
 }
 catch (DataException ex)
 {
 MessageBox.Show(ex.Message);
 }
 customerVSource.View.Refresh();
 }

 }
    <GroupBox x:Name="gbOperations" Button.Click="gbOperations_Click" />
                    private void gbOperations_Click(object sender, RoutedEventArgs e)
 {
 Button SelectedButton = (Button)e.OriginalSource;
 Panel panel = (Panel)SelectedButton.Parent;

 foreach (Button B in panel.Children.OfType<Button>())
 {
 if (B != SelectedButton)
 B.IsEnabled = false;
 }
 gbActions.IsEnabled = true;
 }
    private void ReInitialize()
 {

 Panel panel = gbOperations.Content as Panel;
 foreach (Button B in panel.Children.OfType<Button>())
 {
 B.IsEnabled = true;
 }
 gbActions.IsEnabled = false;
 }
    private void btnCancel_Click(object sender, RoutedEventArgs e)
 {
 ReInitialize();
 }
    private void btnSave_Click(object sender, RoutedEventArgs e)
 {
    TabItem ti = tbCtrlAutoLot.SelectedItem as TabItem;

    switch (ti.Header)
            {
                case "Customers":
        SaveCustomers();
            break;
            case "Inventory":
        SaveInventory();
            break;
            case "Orders":
            break;
 }
        ReInitialize();
 }

    
}
   
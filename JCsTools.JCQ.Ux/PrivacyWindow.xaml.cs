using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.JCQ.Ux
{
  public partial class PrivacyWindow
  {
    public PrivacyWindow()
    {
      _ViewModel = new PrivacyWindowViewModel();

      // This call is required by the Windows Form Designer.
      InitializeComponent();

      // Add any initialization after the InitializeComponent() call.
      App.DefaultWindowStyle.Attach(this);
    }


    private PrivacyWindowViewModel _ViewModel;
    public PrivacyWindowViewModel ViewModel {
      get { return _ViewModel; }
    }

    protected void OnAddVisibleContactClick(object sender, RoutedEventArgs e)
    {
      string identifier;

      try {
        identifier = NewVisibleContact.Text;

        ViewModel.AddVisibleContact(identifier);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected void OnRemoveVisibleContactClick(object sender, RoutedEventArgs e)
    {
      ComponentModel.ICollectionView view;
      ContactViewModel contact;

      try {
        view = CollectionViewSource.GetDefaultView(ViewModel.VisibleContacts);
        contact = (ContactViewModel)view.CurrentItem;

        ViewModel.RemoveVisibleContact(contact);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected void OnAddInvisibleContactClick(object sender, RoutedEventArgs e)
    {
      string identifier;

      try {
        identifier = NewInvisibleContact.Text;

        ViewModel.AddInvisibleContact(identifier);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected void OnRemoveInvisibleContactClick(object sender, RoutedEventArgs e)
    {
      ComponentModel.ICollectionView view;
      ContactViewModel contact;

      try {
        view = CollectionViewSource.GetDefaultView(ViewModel.InvisibleContacts);
        contact = (ContactViewModel)view.CurrentItem;

        ViewModel.RemoveInvisibleContact(contact);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected void OnAddIgnoreContactClick(object sender, RoutedEventArgs e)
    {
      string identifier;

      try {
        identifier = NewIgnoreContact.Text;

        ViewModel.AddIgnoreContact(identifier);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

    protected void OnRemoveIgnoreContactClick(object sender, RoutedEventArgs e)
    {
      ComponentModel.ICollectionView view;
      ContactViewModel contact;

      try {
        view = CollectionViewSource.GetDefaultView(ViewModel.IgnoredContacts);
        contact = (ContactViewModel)view.CurrentItem;

        ViewModel.RemoveIgnoreContact(contact);
      } catch (Exception ex) {
        Core.Kernel.Exceptions.PublishException(ex);
      }
    }

  }
}


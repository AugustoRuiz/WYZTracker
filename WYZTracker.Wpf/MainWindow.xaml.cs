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

namespace WYZTracker.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new ViewModels.MainViewModel();
            this.DataContext = this.viewModel;
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult r = checkModelChanges(e);
            if (r != MessageBoxResult.Cancel)
            {
                this.viewModel.New();
            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult r = checkModelChanges(e);
            if (r != MessageBoxResult.Cancel)
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.Filter = "WYZTracker files (*.wyz)|*.wyz|All files (*.*)|*.*";
                if (ofd.ShowDialog() == true)
                {
                    this.viewModel.Open(ofd.FileName);
                }
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult r = checkModelChanges(e);
            if (r != MessageBoxResult.Cancel)
            {
                string path = this.viewModel.SongFilePath;
                if (string.IsNullOrEmpty(path))
                {
                    this.SaveAs_Executed(this, e);
                }
                else
                {
                    this.viewModel.Save(path);
                }
            }
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult r = checkModelChanges(e);
            if (r != MessageBoxResult.Cancel)
            {
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.Filter = "WYZTracker files (*.wyz)|*.wyz|All files (*.*)|*.*";
                if (sfd.ShowDialog() == true)
                {
                    this.viewModel.Save(sfd.FileName);
                }
            }
        }

        private void ExportMus_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "MUS files (*.mus)|*.mus|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                this.viewModel.ExportMus(sfd.FileName);
            }
        }


        private void ExportAudio_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult r = checkModelChanges(e);
            if (r != MessageBoxResult.Cancel)
            {
                Application.Current.Shutdown();
            }
        }

        private void MovePatternUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.PatternIndex = lboxPatterns.ItemContainerGenerator.IndexFromContainer(Utils.FindParent<ListBoxItem>((DependencyObject)e.OriginalSource));
            this.viewModel.MovePatternUp();
            this.lboxPatterns.Focus();
        }

        private void MovePatternDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.PatternIndex = lboxPatterns.ItemContainerGenerator.IndexFromContainer(Utils.FindParent<ListBoxItem>((DependencyObject)e.OriginalSource));
            this.viewModel.MovePatternDown();
            this.lboxPatterns.Focus();
        }

        private void NextPattern_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.NextPattern();
            this.lboxPatterns.Focus();
        }

        private void PreviousPattern_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.PreviousPattern();
            this.lboxPatterns.Focus();
        }

        private void AddPattern_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.AddPattern();
            this.lboxPatterns.Focus();
        }

        private void RemovePattern_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.RemovePattern();
            this.lboxPatterns.Focus();
        }

        private void ClonePattern_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.ClonePattern();
            this.lboxPatterns.Focus();
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UndoManager.Undo();
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UndoManager.Redo();
        }

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void PasteAsEcho_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private MessageBoxResult checkModelChanges(ExecutedRoutedEventArgs e)
        {
            MessageBoxResult r = MessageBoxResult.No;
            if (this.viewModel.IsDirty)
            {
                r = MessageBox.Show(
                    "There are changes in current song. Would you like to save it first?",
                    "WYZTracker",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    Save_Executed(this, e);
                }
            }
            return r;
        }

        private void IsSelectedPattern(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.viewModel != null && this.viewModel.PatternIndex != -1;
        }

        private void IsNotPatternZero(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.viewModel != null && this.viewModel.PatternIndex != -1 && 
                this.viewModel.PlayOrder[this.viewModel.PatternIndex] > 0;
        }

        private void IsNotLastPattern(object sender, CanExecuteRoutedEventArgs e)
        {
            int idx = lboxPatterns.ItemContainerGenerator.IndexFromContainer(Utils.FindParent<ListBoxItem>((DependencyObject)e.OriginalSource));
            e.CanExecute = this.viewModel!=null && idx < this.viewModel.PlayOrder.Count - 1;
        }

        private void IsNotFirstPattern(object sender, CanExecuteRoutedEventArgs e)
        {
            int idx = lboxPatterns.ItemContainerGenerator.IndexFromContainer(Utils.FindParent<ListBoxItem>((DependencyObject)e.OriginalSource));
            e.CanExecute = this.viewModel != null && idx > 0;
        }

        private void CanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = UndoManager.CanUndo;
        }

        private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = UndoManager.CanRedo;
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}

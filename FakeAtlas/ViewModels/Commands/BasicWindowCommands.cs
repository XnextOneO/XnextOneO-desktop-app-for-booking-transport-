using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeAtlas.ViewModels.Management
{
    #region delegate minimize
    interface IMinimizeWindow
    {
        Action Minimize { get; set; }
        bool CanMinimize();
    }

    public class WindowMinimizer
    {
        public static bool EnableWindowMinimizing(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowMinimizingProperty);
        }

        public static void SetEnableWindowMinimizingProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowMinimizingProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableWindowMinimizingProperty =
            DependencyProperty.RegisterAttached("EnableWindowMinimizing", typeof(bool), typeof(WindowMinimizer), new PropertyMetadata(false, OnEnableWindowsMinimizingChanged));

        private static void OnEnableWindowsMinimizingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is IMinimizeWindow vm)
                    {
                        vm.Minimize += () =>
                        {
                            window.WindowState = WindowState.Minimized;
                        };
                        window.Closing += (s, e) =>
                        {
                            e.Cancel = !vm.CanMinimize();
                        };
                    }
                };
            }
        }
    }
    #endregion

    #region delegate closing
    interface ICloseWindow
    {
        Action Close { get; set; }
        bool CanClose();
    }

    public class WindowCloser
    {


        public static bool EnableWindowClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowClosingProperty);
        }

        public static void SetEnableWindowClosingProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowClosingProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableWindowClosingProperty =
            DependencyProperty.RegisterAttached("EnableWindowClosing", typeof(bool), typeof(WindowCloser), new PropertyMetadata(false, OnEnableWindowsClosingChanged));

        private static void OnEnableWindowsClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is ICloseWindow vm)
                    {
                        vm.Close += () =>
                        {
                            window.Close();
                        };
                        window.Closing += (s, e) =>
                        {
                            e.Cancel = !vm.CanClose();
                        };
                    }
                };
            }
        }
    }

    #endregion

    #region delegate maximize
    interface IMaximizeWindow
    {
        Action Maximize { get; set; }
        bool CanMaximize();
    }

    public class WindowMaximizer
    {


        public static bool EnableWindowMaximizing(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowMaximizingProperty);
        }

        public static void SetEnableWindowMaximizingProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowMaximizingProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableWindowMaximizingProperty =
            DependencyProperty.RegisterAttached("EnableWindowMaximizing", typeof(bool), typeof(WindowMaximizer), new PropertyMetadata(false, OnEnableWindowsMaximizingChanged));

        private static void OnEnableWindowsMaximizingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is IMaximizeWindow vm)
                    {
                        vm.Maximize += () =>
                        {
                            window.WindowState = WindowState.Maximized;
                        };
                        window.Closing += (s, e) =>
                        {
                            e.Cancel = !vm.CanMaximize();
                        };
                    }
                };
            }
        }
    }

    #endregion

    #region delegate restore
    interface IRestoreWindow
    {
        Action Restore { get; set; }
        bool CanRestore();
    }

    public class WindowRestorer
    {
        public static bool EnableWindowRestoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowRestoringProperty);
        }

        public static void SetEnableWindowRestoringProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowRestoringProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableWindowRestoringProperty =
            DependencyProperty.RegisterAttached("EnableWindowRestoring", typeof(bool), typeof(WindowRestorer), new PropertyMetadata(false, OnEnableWindowsRestoringChanged));

        private static void OnEnableWindowsRestoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is IRestoreWindow vm)
                    {
                        vm.Restore += () =>
                        {
                            window.WindowState = WindowState.Normal;
                        };
                        window.Closing += (s, e) =>
                        {
                            e.Cancel = !vm.CanRestore();
                        };
                    }
                };
            }
        }
    }
    #endregion
}


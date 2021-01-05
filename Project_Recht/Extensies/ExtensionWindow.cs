using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Project_Recht.Extensies
{
    //stackoverflow link waar ik dit heb gevonden : https://stackoverflow.com/questions/743906/how-to-hide-close-button-in-wpf-window
    public class ExtensionWindow
    {
     
            private static readonly Type OwnerType = typeof(ExtensionWindow);

            #region HideCloseButton (attached property)
            
            //gaat de dependency registreren bij een property die attached is
            public static readonly DependencyProperty HideCloseButtonProperty =
                DependencyProperty.RegisterAttached(
                    "HideCloseButton",
                    typeof(bool),
                    OwnerType,
                    new FrameworkPropertyMetadata(false, new PropertyChangedCallback(HideCloseButtonChangedCallback)));

            [AttachedPropertyBrowsableForType(typeof(Window))]
            public static bool GetHideCloseButton(Window obj)
            {
                //geeft de value van HideCloseButtonProperty terug als boolean
                return (bool)obj.GetValue(HideCloseButtonProperty);
            }

            [AttachedPropertyBrowsableForType(typeof(Window))]
            public static void SetHideCloseButton(Window obj, bool value)
            {
                //gaat de value setten
                obj.SetValue(HideCloseButtonProperty, value);
            }

            private static void HideCloseButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                //gaat het dependency object als window in een var steken
                var window = d as Window;

                //als de var null is return doen
                if (window == null) return;

                //gaat een variabele opvullen met de EventArgs value als boolean

                var hideCloseButton = (bool)e.NewValue;
                
                //als deze true is en het kruisje zichtbaar is
                if (hideCloseButton && !GetIsHiddenCloseButton(window))
                {
                    if (!window.IsLoaded)
                    {
                        //voegt de delegate toe aan de window als deze loaded is
                        window.Loaded += HideWhenLoadedDelegate;
                    }
                    else
                    {
                        //gaat de toolbar verbergen in de window
                        HideCloseButton(window);
                    }
                    //geeft true door die in de methode op de dependency als value wordt gezet
                    SetIsHiddenCloseButton(window, true);
                }
                //als de toolbar hidden is
                else if (!hideCloseButton && GetIsHiddenCloseButton(window))
                {
                    //geeft false door die in de methode op de dependency als value wordt gezet
                    SetIsHiddenCloseButton(window, false);
                }
            }

            #region Win32 imports

            //nodige dlls importeren
            private const int GWL_STYLE = -16;
            private const int WS_SYSMENU = 0x80000;
            [DllImport("user32.dll", SetLastError = true)]
            private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
            [DllImport("user32.dll")]
            private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            #endregion

            private static readonly RoutedEventHandler HideWhenLoadedDelegate = (sender, args) => {
                if (sender is Window == false) return;
                var w = (Window)sender;
                HideCloseButton(w);
                w.Loaded -= HideWhenLoadedDelegate;
            };


            private static void HideCloseButton(Window w)
            {
                //gaat het kruisje verbergen
                var hwnd = new WindowInteropHelper(w).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            }

          

            #endregion

            #region IsHiddenCloseButton (readonly attached property)
            //boolean property registreren om te bepalen of het kruisje hidden is
            private static readonly DependencyPropertyKey IsHiddenCloseButtonKey =
                DependencyProperty.RegisterAttachedReadOnly(
                    "IsHiddenCloseButton",
                    typeof(bool),
                    OwnerType,
                    new FrameworkPropertyMetadata(false));

            public static readonly DependencyProperty IsHiddenCloseButtonProperty =
                IsHiddenCloseButtonKey.DependencyProperty;

            [AttachedPropertyBrowsableForType(typeof(Window))]
            public static bool GetIsHiddenCloseButton(Window obj)
            {

                return (bool)obj.GetValue(IsHiddenCloseButtonProperty);
            }

            private static void SetIsHiddenCloseButton(Window obj, bool value)
            {
                //stelt true of false in op de dependency van de window
                obj.SetValue(IsHiddenCloseButtonKey, value);
            }

            #endregion

        }
    }


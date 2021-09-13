using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers.CustomViews
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Initialize a page. Called from Custom page classes
        /// </summary>
        public static void Initialize<P, V>(this P page)
            where P : Page, ICustomPage<V>
            where V : CustomViewModel
        {
            page.ViewModel = Activator.CreateInstance<V>();
            page.BindingContext = page.ViewModel;

            // Try to find InitializeComponent method of this object's type and invoke it.
            try
            {
                page.GetType().GetMethod("InitializeComponent", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(page, null);
            }
            catch { }
        }

        /// <summary>
        /// Subscribe a View to a sent message from its ViewModel
        /// </summary>
        public static void Subscribe<V>(this Page page, string message, Action<V> action)
            where V : CustomViewModel
        {
            try
            {
                if (action == null) return;

                MessagingCenter.Subscribe(page, message, action);
            }
            catch { }
        }



        /// <summary>
        /// Subscribe a View to a sent message from its ViewModel
        /// </summary>
        public static void Subscribe<V>(this Page page, string message, MessageSubscriptionType type)
            where V : CustomViewModel
        {
            try
            {
                switch (type)
                {
                    case MessageSubscriptionType.DisplayAlert:
                        var actionArgs = (string[])Messages.ArgsList[message];
                        if (actionArgs == null) return;
                        MessagingCenter.Subscribe<V>(page, message,
                            async (sender) =>
                            {
                                if (actionArgs.Length == 3)
                                {
                                    await page.DisplayAlert(title: actionArgs[0], message: actionArgs[1], cancel: actionArgs[2]);
                                }
                                else if (actionArgs.Length == 4)
                                {
                                    await page.DisplayAlert(title: actionArgs[0], message: actionArgs[1], accept: actionArgs[2], cancel: actionArgs[3]);
                                }
                            });
                        break;


                    default:
                        break;
                }


            }
            catch { }
        }
    }
}

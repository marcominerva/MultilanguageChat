using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;

namespace Xamarin.Forms
{
    /// <summary>
    /// Extensions for saving and restoring navigation states with arguments.
    /// https://github.com/aloisdeniel/Xam.Forms.NavigationExtensions
    /// </summary>
    public static class NavigationExtensions
    {
        #region Arguments access

        /// <summary>
        /// All the navigation arguments associated to pages.
        /// </summary>
        private static ConditionalWeakTable<Page, object> arguments = new ConditionalWeakTable<Page, object>();

        /// <summary>
        /// Gets the navigation arguments for a page.
        /// </summary>
        /// <returns>The navigation arguments.</returns>
        /// <param name="page">Page.</param>
        public static object GetNavigationArgs(this Page page)
        {
            object argument = null;
            arguments.TryGetValue(page, out argument);

            return argument;
        }

        /// <summary>
        /// Stores the navigation arguments for a page.
        /// </summary>
        /// <returns>The navigation arguments.</returns>
        /// <param name="page">Page.</param>
        /// <param name="args">Arguments.</param>
        public static void SetNavigationArgs(this Page page, object args)
        {
            arguments.Add(page, args);
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Navigates to the given page with an argument that will be available from this page, but also stored with the navigation state.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="navigation">The navigation service.</param>
        /// <param name="page">The destination page.</param>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="animated">Indicates whether the navigation should be animated.</param>
        public static Task PushAsync(this INavigation navigation, Page page, object args, bool animated)
        {
            page.SetNavigationArgs(args);
            return navigation.PushAsync(page, animated);
        }

        /// <summary>
        /// Navigates modaly to the given page with an argument that will be available from this page, but also stored with the navigation state.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="navigation">The navigation service.</param>
        /// <param name="page">The destination page.</param>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="animated">Indicates whether the navigation should be animated.</param>
        public static Task PushModalAsync(this INavigation navigation, Page page, object args, bool animated)
        {
            page.SetNavigationArgs(args);
            return navigation.PushModalAsync(page, animated);
        }

        /// <summary>
        /// Instanciates a page from its type(must have an empty constructor) and navigates to this page with an argument that will be available from this page, but also stored with the navigation state.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="navigation">The navigation service.</param>
        /// <param name="pageType">The destination page type.</param>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="animated">Indicates whether the navigation should be animated.</param>
        public static Task PushAsync(this INavigation navigation, Type pageType, object args = null, bool animated = true)
        {
            var page = Activator.CreateInstance(pageType) as Page;
            return navigation.PushAsync(page, args, animated);
        }

        /// <summary>
        /// Instanciates a page from its type(must have an empty constructor) and navigates modaly to this page with an argument that will be available from this page, but also stored with the navigation state.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="navigation">The navigation service.</param>
        /// <param name="pageType">The destination page type.</param>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="animated">Indicates whether the navigation should be animated.</param>
        public static Task PushModalAsync(this INavigation navigation, Type pageType, object args = null, bool animated = true)
        {
            var page = Activator.CreateInstance(pageType) as Page;
            return navigation.PushAsync(page, args, animated);
        }

        /// <summary>
        /// Instanciates a page from its type(must have an empty constructor) and navigates to this page with an argument that will be available from this page, but also stored with the navigation state.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="navigation">The navigation service.</param>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="animated">Indicates whether the navigation should be animated.</param>
        /// <typeparam name="T">The destination page type.</typeparam>
        public static Task PushAsync<T>(this INavigation navigation, object args = null, bool animated = true) where T : Page
        {
            return navigation.PushAsync(typeof(T), args, animated);
        }

        /// <summary>
        /// Instanciates a page from its type(must have an empty constructor) and navigates modaly to this page with an argument that will be available from this page, but also stored with the navigation state.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="navigation">The navigation service.</param>
        /// <param name="args">The navigation arguments.</param>
        /// <param name="animated">Indicates whether the navigation should be animated.</param>
        /// <typeparam name="T">The destination page type.</typeparam>
        public static Task PushModalAsync<T>(this INavigation navigation, object args = null, bool animated = true) where T : Page
        {
            return navigation.PushAsync(typeof(T), args, animated);
        }

        #endregion

        private class NavigationStates
        {
            /// <summary>
            /// Gets or sets the store date.
            /// </summary>
            /// <value>The date.</value>
            public DateTime Date { get; set; }

            /// <summary>
            /// Gets or sets the all the stored navigation states (page and args).
            /// </summary>
            /// <value>The navigation stack.</value>
            public IEnumerable<PageState> Navigation { get; set; }

            /// <summary>
            /// Gets or sets the all the stored modal navigation states (page and args).
            /// </summary>
            /// <value>The modal stack.</value>
            public IEnumerable<PageState> Modal { get; set; }
        }

        private class PageState
        {
            /// <summary>
            /// Gets or sets the type of the page.
            /// </summary>
            /// <value>The type of the page.</value>
            public Type PageType { get; set; }

            /// <summary>
            /// Gets or sets the navigation argument.
            /// </summary>
            /// <value>The argument.</value>
            public object Argument { get; set; }
        }
    }
}
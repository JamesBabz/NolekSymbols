using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace NolekSymbols.Helpers
{
    public static class VisualTreeSearchHelper
    {
        /// <summary>
        ///     Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>
        ///     The first parent item that matches the submitted type parameter.
        ///     If not matching item can be found,
        ///     a null parent is being returned.
        /// </returns>
        public static T FindChildByName<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                if (!(child is T))
                {
                    // recursively drill down the tree
                    foundChild = FindChildByName<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (!(child is FrameworkElement frameworkElement) || frameworkElement.Name != childName) continue;
                    // if the child's name is of the request name
                    foundChild = (T) child;
                    break;
                }
                else
                {
                    // child element found.
                    foundChild = (T) child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        ///     Find parent of child
        /// </summary>
        /// <typeparam name="T">The type of parent</typeparam>
        /// <param name="child">The child</param>
        /// <returns>The parent</returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                //get parent item
                var parentObject = VisualTreeHelper.GetParent(child);

                //we've reached the end of the tree
                switch (parentObject)
                {
                    case null:
                        return null;
                    case T parent:
                        return parent;
                }

                //check if the parent matches the type we're looking for
                child = parentObject;
            }
        }

        /// <summary>
        ///     Find all children of type
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="depObj">The parent</param>
        /// <returns>an IEnumerable of the children of type</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T type)
                    yield return type;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
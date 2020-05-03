using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Stud
{
    public static class CustomCommands
    {
        public static RoutedUICommand ToggleGroupsSearchMode = new RoutedUICommand("ToggleGroupsSearchMode",
            "ToggleGroupsSearchMode", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control) });

        public static RoutedUICommand ToggleFacultiesSearchMode = new RoutedUICommand("ToggleFacultiesSearchMode",
            "ToggleFacultiesSearchMode", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.F, ModifierKeys.Control) });

        public static RoutedUICommand Toggle1CourseCheckbox = new RoutedUICommand("Toggle1CourseCheckbox",
           "Toggle1CourseCheckbox", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.D1, ModifierKeys.Control) });

        public static RoutedUICommand Toggle2CourseCheckbox = new RoutedUICommand("Toggle1CourseCheckbox",
           "Toggle1CourseCheckbox", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.D2, ModifierKeys.Control) });
        
        public static RoutedUICommand Toggle3CourseCheckbox = new RoutedUICommand("Toggle3CourseCheckbox",
           "Toggle1CourseCheckbox", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.D3, ModifierKeys.Control) });
        
        public static RoutedUICommand Toggle4CourseCheckbox = new RoutedUICommand("Toggle4CourseCheckbox",
           "Toggle1CourseCheckbox", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.D4, ModifierKeys.Control) });
        
        public static RoutedUICommand Toggle5CourseCheckbox = new RoutedUICommand("Toggle5CourseCheckbox",
           "Toggle1CourseCheckbox", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.D5, ModifierKeys.Control) });
        
        public static RoutedUICommand Toggle6CourseCheckbox = new RoutedUICommand("Toggle6CourseCheckbox",
           "Toggle1CourseCheckbox", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.D6, ModifierKeys.Control) });

        public static RoutedUICommand CheckAllCources = new RoutedUICommand("CheckAllCources",
           "CheckAllCources", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.A, ModifierKeys.Control) });
    }
}

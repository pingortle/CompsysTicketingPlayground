using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AdmitOne.View
{
    // Based on: https://gist.github.com/bradphelan/6449957
    [ContentProperty("Children")]
    public class SharedRow : Grid
    {
        static SharedRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SharedRow), new FrameworkPropertyMetadata(typeof(SharedRow)));
        }

        public int SharedColumnCount
        {
            get { return (int)GetValue(SharedColumnCountProperty); }
            set { SetValue(SharedColumnCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SharedColumnCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SharedColumnCountProperty =
            DependencyProperty.Register("SharedColumnCount", typeof(int), typeof(SharedRow), new PropertyMetadata(0));

        const int MaxColumns = 20;

        public SharedRow()
        {
            this.WhenAny(p => p.Children.Count, p => p.Value)
                .Subscribe(c =>
                {
                    ColumnDefinitions.Clear();

                    // We should be able to make this more intelligent by looking 
                    // at the children and finding out the max number of columns needed.
                    for (int i = 0; i < MaxColumns; i++)
                    {
                        var cd = new ColumnDefinition();
                        cd.SharedSizeGroup = "Group" + i.ToString();
                        ColumnDefinitions.Add(cd);
                    }

                });
        }
    }
}

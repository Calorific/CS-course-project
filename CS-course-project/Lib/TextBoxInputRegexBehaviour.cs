using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace CS_course_project.Lib;

public class TextBoxInputRegExBehaviour : Behavior<TextBox> {
    
    public static readonly DependencyProperty RegularExpressionProperty =
        DependencyProperty.Register(nameof(RegularExpression), typeof(string), typeof(TextBoxInputRegExBehaviour),
            new FrameworkPropertyMetadata(".*"));

    public string RegularExpression {
        get => (string)GetValue(RegularExpressionProperty);
        set => SetValue(RegularExpressionProperty, value);
    }

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(TextBoxInputRegExBehaviour),
            new FrameworkPropertyMetadata(int.MinValue));

    public int MaxLength {
        get => (int)GetValue(MaxLengthProperty); 
        set => SetValue(MaxLengthProperty, value); 
    }

    public static readonly DependencyProperty EmptyValueProperty =
        DependencyProperty.Register(nameof(EmptyValue), typeof(string), typeof(TextBoxInputRegExBehaviour), null);

    public string EmptyValue {
        get => (string)GetValue(EmptyValueProperty); 
        set => SetValue(EmptyValueProperty, value);
    }

    protected override void OnAttached() {
        base.OnAttached();

        AssociatedObject.PreviewTextInput += PreviewTextInputHandler;
        AssociatedObject.PreviewKeyDown += PreviewKeyDownHandler;
        DataObject.AddPastingHandler(AssociatedObject, PastingHandler);
    }
    
    protected override void OnDetaching() {
        base.OnDetaching();

        AssociatedObject.PreviewTextInput -= PreviewTextInputHandler;
        AssociatedObject.PreviewKeyDown -= PreviewKeyDownHandler;
        DataObject.RemovePastingHandler(AssociatedObject, PastingHandler);
    }

    private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e) {
        string text;
        if (AssociatedObject.Text.Length < AssociatedObject.CaretIndex)
            text = AssociatedObject.Text;
        else {
            text = TreatSelectedText(out var remainingTextAfterRemoveSelection)
                ? remainingTextAfterRemoveSelection.Insert(AssociatedObject.SelectionStart, e.Text)
                : AssociatedObject.Text.Insert(this.AssociatedObject.CaretIndex, e.Text);
        }

        e.Handled = !ValidateText(text);
    }

    private void PreviewKeyDownHandler(object sender, KeyEventArgs e) {
        if (string.IsNullOrEmpty(this.EmptyValue))
            return;

        string? text;

        switch (e.Key) {
            case Key.Back: {
                if (!this.TreatSelectedText(out text)) {
                    if (AssociatedObject.SelectionStart > 0)
                        text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart - 1, 1);
                }

                break;
            }
            
            case Key.Delete: {
                if (!TreatSelectedText(out text) &&
                    AssociatedObject.Text.Length > AssociatedObject.SelectionStart) {
                    text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, 1);
                }

                break;
            }
            
            default:
                return;
        }

        if (text != string.Empty) return;
        AssociatedObject.Text = EmptyValue;
        if (e.Key == Key.Back)
            AssociatedObject.SelectionStart++;
        e.Handled = true;
    }

    private void PastingHandler(object sender, DataObjectPastingEventArgs e) {
        if (e.DataObject.GetDataPresent(DataFormats.Text)) {
            var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

            if (text != null && !ValidateText(text))
                e.CancelCommand();
        }
        else
            e.CancelCommand();
    }
    
    private bool ValidateText(string text) {
        return (new Regex(this.RegularExpression, RegexOptions.IgnoreCase)).IsMatch(text) &&
               (MaxLength == int.MinValue || text.Length <= MaxLength);
    }

    private bool TreatSelectedText(out string text) {
        text = string.Empty;
        if (AssociatedObject.SelectionLength <= 0)
            return false;

        var length = this.AssociatedObject.Text.Length;
        if (AssociatedObject.SelectionStart >= length)
            return true;

        if (AssociatedObject.SelectionStart + AssociatedObject.SelectionLength >= length)
            AssociatedObject.SelectionLength = length - AssociatedObject.SelectionStart;

        text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);
        return true;
    }
}
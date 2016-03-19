using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WYZTracker.Wpf
{
    public static class WYZTrackerCommands
    {
        public static readonly string EXPORT_MUS_COMMAND = "exportMUSCommand";
        public static readonly string EXPORT_AUDIO_COMMAND = "exportAudioCommand";
        public static readonly string EXIT_COMMAND = "exitCommand";
        public static readonly string MOVE_PATTERN_UP_COMMAND = "movePatternUpCommand";
        public static readonly string MOVE_PATTERN_DOWN_COMMAND = "movePatternDownCommand";
        public static readonly string NEXT_PATTERN_COMMAND = "nextPatternCommand";
        public static readonly string PREVIOUS_PATTERN_COMMAND = "previousPatternCommand";
        public static readonly string ADD_PATTERN_COMMAND = "addPatternCommand";
        public static readonly string REMOVE_PATTERN_COMMAND = "removePatternCommand";
        public static readonly string CLONE_PATTERN_COMMAND = "clonePatternCommand";
        public static readonly string PASTE_AS_ECHO_COMMAND = "pasteAsEchoCommand";

        public static readonly RoutedUICommand ExportMUS = 
            new RoutedUICommand("Export MUS...", EXPORT_MUS_COMMAND, typeof(WYZTrackerCommands), 
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.M, ModifierKeys.Control | ModifierKeys.Shift) }));

        public static readonly RoutedUICommand ExportAudio =
            new RoutedUICommand("Export audio...", EXPORT_AUDIO_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift) }));

        public static readonly RoutedUICommand Exit =
            new RoutedUICommand("Exit", EXIT_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.F4, ModifierKeys.Alt) }));

        public static readonly RoutedUICommand MovePatternUp =
            new RoutedUICommand("Move pattern up", MOVE_PATTERN_UP_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.Up, ModifierKeys.Control | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand MovePatternDown =
            new RoutedUICommand("Move pattern up", MOVE_PATTERN_DOWN_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.Down, ModifierKeys.Control | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand NextPattern =
            new RoutedUICommand("Move pattern up", NEXT_PATTERN_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.PageDown, ModifierKeys.Control | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand PreviousPattern =
            new RoutedUICommand("Move pattern up", PREVIOUS_PATTERN_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.PageDown, ModifierKeys.Control | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand AddPattern =
            new RoutedUICommand("Move pattern up", ADD_PATTERN_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand RemovePattern =
            new RoutedUICommand("Move pattern up", REMOVE_PATTERN_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand ClonePattern =
            new RoutedUICommand("Move pattern up", CLONE_PATTERN_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt) }));

        public static readonly RoutedUICommand PasteAsEcho =
            new RoutedUICommand("Paste as echo", PASTE_AS_ECHO_COMMAND, typeof(WYZTrackerCommands),
                new InputGestureCollection(new List<KeyGesture>() {
                    new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift ) }));

    }
}

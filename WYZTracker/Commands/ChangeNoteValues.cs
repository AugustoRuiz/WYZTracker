using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker.Commands
{
    public class ChangeNoteValues : ICommand
    {
        public static PatternView PatEditor { get; set; }

        private ChannelNote originalNote;
        private ChannelNote oldValues;
        private ChannelNote newValues;

        public ChangeNoteValues(ChannelNote channelNote, ChannelNote newValues)
        {
            this.oldValues = channelNote.Clone();
            this.newValues = newValues.Clone();
            this.originalNote = channelNote;
        }
        public void Execute()
        {
            this.originalNote.CopyFrom(newValues);
            this.Refresh();
        }

        public void Undo()
        {
            this.originalNote.CopyFrom(oldValues);
            this.Refresh();
        }

        private void Refresh()
        {
            PatternView patEdit = ChangeNoteValues.PatEditor;
            if (patEdit != null)
            {
                if (patEdit.InvokeRequired)
                {
                    patEdit.Invoke((MethodInvoker)(()=>{ patEdit.Invalidate(true); }));
                }
                else
                {
                    patEdit.Invalidate(true);
                }
            }
        }
    }
}

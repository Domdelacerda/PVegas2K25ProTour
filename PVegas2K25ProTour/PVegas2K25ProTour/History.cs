using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVegas2K25ProTour
{
    internal class History
    {
        private Stack<Command> undo;
        private Stack<Command> redo;
        public History()
        {
            undo = new Stack<Command>();
            redo = new Stack<Command>();
        }
        public void Do(Command new_cmd)
        {
            new_cmd.execute();
            undo.Clear();
            redo.Push(new_cmd);
        }
        public void Undo()
        { 
            if(redo.Count>0) { 
                Command undo=redo.Pop();
                undo.unexecute();
                this.undo.Push(undo);
            }
        }
        public void Redo()
        {
            if (undo.Count > 0) { 
                Command todo = undo.Pop();
                todo.execute();
                redo.Push(todo);
            }
        }
    }
}

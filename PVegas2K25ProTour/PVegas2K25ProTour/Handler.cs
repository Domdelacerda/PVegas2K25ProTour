using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVegas2K25ProTour
{
    internal class Handler
    {
        public History setting_history;
        public GameControl control { get; set; }
        public Handler(GameControl gameControl)
        {
            setting_history = new History();
            control = gameControl;
        }
        public void IncreaseSetting(int type)
        {
            setting_history.Do(new IncreaseCommand(control,type));
        }
        public void DecreaseSetting(int type)
        {
            setting_history.Do(new DecreaseCommand(control,type));
        }
        public void Undo()
        {
            setting_history.Undo();
        }
        public void Redo()
        {
            setting_history.Redo();
        }
    }
}
